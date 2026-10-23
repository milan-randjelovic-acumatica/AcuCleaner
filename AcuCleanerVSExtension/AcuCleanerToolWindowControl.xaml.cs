using AcuCleaner.Models;
using AcuCleaner.Services;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AcuCleanerVSExtension
{
    /// <summary>
    /// The WPF content hosted inside the AcuCleaner tool window.
    /// Mirrors the layout and behaviour of the standalone AcuCleaner MainForm.
    /// </summary>
    public partial class AcuCleanerToolWindowControl : UserControl
    {
        private static AcuCleanerToolWindowControl? _instance;

        private readonly ObservableCollection<IISApp> _apps = new ObservableCollection<IISApp>();
        private List<IISApp> _allApps = new List<IISApp>();
        private readonly IProgress<string> _progressHandler;

        public AcuManager AcuManager { get; private set; }

        public AcuCleanerToolWindowControl()
        {
            InitializeComponent();

            _instance = this;
            dataGrid.ItemsSource = _apps;

            _progressHandler = new Progress<string>(message =>
                Dispatcher.Invoke(() => statusLabel.Text = message));

            AcuManager = new AcuManager(_progressHandler);
        }

        public static void HandleError(string message)
        {
            if (_instance == null) return;

            _instance.Dispatcher.Invoke(() =>
                _instance.statusLabel.Text = $"Error: {message}");
            _instance.AcuManager.IISManager.Start();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = Visibility.Collapsed;
            _progressHandler.Report("Ready");
            datePicker.SelectedDate = DateTime.Today;
            RefreshAppList();
        }

        private void RefreshAppList()
        {
            // Skip if controls haven't been fully initialized yet
            if (!IsLoaded)
                return;

            SetParameters();

            _allApps = AcuManager
                .GetAllInstances()
                .Filter()
                .OrderBy(x => x.InstanceName)
                .AddRowNumbers()
                .ToList();

            _apps.Clear();
            foreach (var app in _allApps)
                _apps.Add(app);

            _progressHandler.Report($"{_apps.Count} instance(s) loaded.");
        }

        private void FilterSelectedApps()
        {
            SetParameters();

            foreach (var selectedItem in dataGrid.SelectedItems.OfType<IISApp>())
                (AcuManager.CleanupParameters.Apps as List<IISApp>)?.Add(selectedItem);
        }

        private void SetFilterParameters()
        {
            AcuManager.FilterParameters = new FilterParameters
            {
                ShowActiveApps = checkBoxActive.IsChecked == true,
                ShowJunk = checkBoxJunk.IsChecked == true,
                ShowFiles = checkBoxIISFiles.IsChecked == true,
                ShowAspTempFiles = checkBoxAspTmpFiles.IsChecked == true,
                ShowDatabases = checkBoxDatabases.IsChecked == true,
                ShowRegistry = checkBoxRegistry.IsChecked == true,
                ShowSnapshots = checkBoxSnapshots.IsChecked == true,
                ShowCustomizations = checkBoxCustomizations.IsChecked == true,
                ShowBackups = checkBoxBackups.IsChecked == true,
                CutoffDate = datePicker.SelectedDate?.Date ?? DateTime.MinValue,
            };
        }

        private void SetCleanupParameters()
        {
            AcuManager.CleanupParameters = new CleanupParameters
            {
                CleanupIISApps = checkBoxCleanupIISApps.IsChecked == true,
                CleanupFiles = checkBoxCleanupFiles.IsChecked == true,
                CleanupDatabases = checkBoxCleanupDatabases.IsChecked == true,
                CleanupAspTemp = checkBoxCleanupAspTemp.IsChecked == true,
                CleanupSnapshots = checkBoxCleanupSnapshots.IsChecked == true,
                CleanupRegistry = checkBoxCleanupRegistry.IsChecked == true,
                CleanupCustomizations = checkBoxCleanupCustomizations.IsChecked == true,
                CleanupBackups = checkBoxCleanupBackups.IsChecked == true,
            };
        }

        private void SetParameters()
        {
            SetFilterParameters();
            SetCleanupParameters();
        }

        private bool ShowConfirmation(string message, string title)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (AcuCleanerToolWindow.Package == null)
                return false;

            int result = VsShellUtilities.ShowMessageBox(
                AcuCleanerToolWindow.Package,
                message,
                title,
                OLEMSGICON.OLEMSGICON_WARNING,
                OLEMSGBUTTON.OLEMSGBUTTON_YESNO,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_SECOND);

            return result == (int)VSConstants.MessageBoxResult.IDYES;
        }

        private async Task DeleteSelectedAppsAsync()
        {
            FilterSelectedApps();

            if (!AcuManager.CleanupParameters.Apps.Any())
            {
                statusLabel.Text = "No rows selected.";
                return;
            }

            int count = AcuManager.CleanupParameters.Apps.Count();
            bool confirmed = ShowConfirmation(
                $"Delete {count} selected app(s)?",
                "Confirm delete");

            if (!confirmed)
            {
                statusLabel.Text = "Delete canceled.";
                return;
            }

            groupBoxCleanup.IsEnabled = false;
            groupBoxFilter.IsEnabled = false;
            progressBar.Visibility = Visibility.Visible;
            _progressHandler.Report($"Deleting {count} app(s)...");

            try
            {
                await Task.Run(() => AcuManager.Cleanup());
                _progressHandler.Report($"Deleted {count} app(s).");
                RefreshAppList();
            }
            catch (Exception ex)
            {
                _progressHandler.Report($"Delete failed: {ex.Message}");
            }
            finally
            {
                progressBar.Visibility = Visibility.Collapsed;
                groupBoxCleanup.IsEnabled = true;
                groupBoxFilter.IsEnabled = true;
            }
        }

        private void UserControl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.F5)
                RefreshAppList();
        }

        private void Filter_Changed(object sender, RoutedEventArgs e) => RefreshAppList();

        private void DatePicker_SelectedDateChanged(object sender,
            SelectionChangedEventArgs e) => RefreshAppList();

        private async void ButtonClean_Click(object sender, RoutedEventArgs e)
        {
            await DeleteSelectedAppsAsync();
        }

        private async void ButtonCleanAll_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.SelectAll();
            await DeleteSelectedAppsAsync();
        }

        private void RefreshMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RefreshAppList();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (AcuCleanerToolWindow.Package?.FindToolWindow(typeof(AcuCleanerToolWindow), 0, false) is ToolWindowPane pane
                && pane.Frame is IVsWindowFrame frame)
            {
                frame.CloseFrame((uint)__FRAMECLOSE.FRAMECLOSE_NoSave);
            }
        }

        private void ResetPasswordMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FilterSelectedApps();
            int count = AcuManager.CleanupParameters.Apps.Count();
            if (ShowConfirmation(
                $"This action will reset password for {count} selected instance(s). Are you sure you want to continue?",
                "Confirm Action"))
            {
                AcuManager.DatabaseManager.ResetPassword(AcuManager.CleanupParameters);
            }
        }

        private void EnableModernUISwitchMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FilterSelectedApps();
            int count = AcuManager.CleanupParameters.Apps.Count();
            if (ShowConfirmation(
                $"This action will enable Modern UI switch for {count} selected instance(s). Are you sure you want to continue?",
                "Confirm Action"))
            {
                AcuManager.WebConfigManager.EnableModernUISwitch(AcuManager.CleanupParameters);
            }
        }

        private void ExtendSessionTimeoutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FilterSelectedApps();
            int count = AcuManager.CleanupParameters.Apps.Count();
            if (ShowConfirmation(
                $"This action will set session timeout to 1 day for {count} selected instance(s). Are you sure you want to continue?",
                "Confirm Action"))
            {
                AcuManager.WebConfigManager.ExtendSessionTimeout(AcuManager.CleanupParameters);
            }
        }

        private void ApplyAllPatchesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ResetPasswordMenuItem_Click(sender, e);
            EnableModernUISwitchMenuItem_Click(sender, e);
            ExtendSessionTimeoutMenuItem_Click(sender, e);
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var originalSource = e.OriginalSource as DependencyObject;
            if(originalSource == null)
                return;

            var cell = FindVisualParent<DataGridCell>(originalSource);
            if (cell == null || !(cell.DataContext is IISApp app))
                return;

            var propertyName = !string.IsNullOrEmpty(cell.Column?.SortMemberPath)
                ? cell?.Column?.SortMemberPath
                : (cell.Column as DataGridBoundColumn)?.Binding is System.Windows.Data.Binding binding
                    ? binding.Path.Path
                    : null;

            switch (propertyName)
            {
                case nameof(IISApp.Path):
                    AcuManager.FileSystemManager.OpenInExplorer(app.Path);
                    break;
                case nameof(IISApp.AspTempFilesPath):
                    AcuManager.FileSystemManager.OpenInExplorer(app.AspTempFilesPath);
                    break;
                case nameof(IISApp.CustomizationPath):
                    AcuManager.FileSystemManager.OpenInExplorer(app.CustomizationPath);
                    break;
                case nameof(IISApp.BackupPath):
                    AcuManager.FileSystemManager.OpenInExplorer(app.BackupPath);
                    break;
                case nameof(IISApp.SnapshotPath):
                    AcuManager.FileSystemManager.OpenInExplorer(app.SnapshotPath);
                    break;
                case nameof(IISApp.RegistryPath):
                    AcuManager.RegistryManager.OpenInRegedit(app.RegistryPath);
                    break;
            }
        }

        private static T? FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (child != null)
            {
                if (child is T typed)
                    return typed;

                child = VisualTreeHelper.GetParent(child);
            }

            return null;
        }
    }
}