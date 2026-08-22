using AcuCleaner.Models;
using AcuCleaner.Services;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

        /// <summary>
        /// Called by IIS cleaner or other services to surface an error in the status bar.
        /// </summary>
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
            statusLabel.Text = "Ready";
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

            statusLabel.Text = $"{_apps.Count} instance(s) loaded.";
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
                ShowActiveApps     = checkBoxActive.IsChecked == true,
                ShowJunk           = checkBoxJunk.IsChecked == true,
                ShowFiles          = checkBoxIISFiles.IsChecked == true,
                ShowAspTempFiles   = checkBoxAspTmpFiles.IsChecked == true,
                ShowDatabases      = checkBoxDatabases.IsChecked == true,
                ShowRegistry       = checkBoxRegistry.IsChecked == true,
                ShowSnapshots      = checkBoxSnapshots.IsChecked == true,
                ShowCustomizations = checkBoxCustomizations.IsChecked == true,
                ShowBackups        = checkBoxBackups.IsChecked == true,
                CutoffDate         = datePicker.SelectedDate?.Date ?? DateTime.MinValue,
            };
        }

        private void SetCleanupParameters()
        {
            AcuManager.CleanupParameters = new CleanupParameters
            {
                CleanupIISApps       = checkBoxCleanupIISApps.IsChecked == true,
                CleanupFiles         = checkBoxCleanupFiles.IsChecked == true,
                CleanupDatabases     = checkBoxCleanupDatabases.IsChecked == true,
                CleanupAspTemp       = checkBoxCleanupAspTemp.IsChecked == true,
                CleanupSnapshots     = checkBoxCleanupSnapshots.IsChecked == true,
                CleanupRegistry      = checkBoxCleanupRegistry.IsChecked == true,
                CleanupCustomizations = checkBoxCleanupCustomizations.IsChecked == true,
                CleanupBackups       = checkBoxCleanupBackups.IsChecked == true,
            };
        }

        private void SetParameters()
        {
            SetFilterParameters();
            SetCleanupParameters();
        }

        private void DeleteSelectedApps()
        {
            FilterSelectedApps();

            if (!AcuManager.CleanupParameters.Apps.Any())
            {
                statusLabel.Text = "No rows selected.";
                return;
            }

            int count = AcuManager.CleanupParameters.Apps.Count();
            var result = MessageBox.Show(
                $"Delete {count} selected app(s)?",
                "Confirm delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
            {
                statusLabel.Text = "Delete canceled.";
                return;
            }

            buttonClean.IsEnabled    = false;
            buttonCleanAll.IsEnabled = false;
            progressBar.Visibility   = Visibility.Visible;
            statusLabel.Text         = $"Deleting {count} app(s)...";

            try
            {
                AcuManager.Cleanup();
                statusLabel.Text = $"Deleted {count} app(s).";
                RefreshAppList();
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Delete failed: {ex.Message}";
            }
            finally
            {
                progressBar.Visibility   = Visibility.Collapsed;
                buttonClean.IsEnabled    = true;
                buttonCleanAll.IsEnabled = true;
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

        private void ButtonClean_Click(object sender, RoutedEventArgs e)
            => DeleteSelectedApps();

        private void ButtonCleanAll_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.SelectAll();
            DeleteSelectedApps();
        }

        private void RefreshMenuItem_Click(object sender, RoutedEventArgs e)
            => RefreshAppList();

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
            var result = MessageBox.Show(
                $"This action will reset password for {count} selected instance(s). Are you sure you want to continue?",
                "Confirm Action", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                AcuManager.DatabaseManager.ResetPassword(AcuManager.CleanupParameters);
        }

        private void EnableModernUISwitchMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FilterSelectedApps();
            int count = AcuManager.CleanupParameters.Apps.Count();
            var result = MessageBox.Show(
                $"This action will enable Modern UI switch for {count} selected instance(s). Are you sure you want to continue?",
                "Confirm Action", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                AcuManager.WebConfigManager.EnableModernUISwitch(AcuManager.CleanupParameters);
        }

        private void ExtendSessionTimeoutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FilterSelectedApps();
            int count = AcuManager.CleanupParameters.Apps.Count();
            var result = MessageBox.Show(
                $"This action will set session timeout to 1 day for {count} selected instance(s). Are you sure you want to continue?",
                "Confirm Action", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                AcuManager.WebConfigManager.ExtendSessionTimeout(AcuManager.CleanupParameters);
        }
    }
}




