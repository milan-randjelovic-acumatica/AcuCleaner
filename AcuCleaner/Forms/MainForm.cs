using AcuCleaner.Models;
using AcuCleaner.Services;

namespace AcuCleaner.Forms
{
    public partial class MainForm : Form
    {
        private static MainForm? _instance;
        private List<IISApp> _allApps = new List<IISApp>();
        protected readonly IProgress<string> ProgressHandler;
        private readonly Image? _warningImage;
        private readonly Image? _folderImage;
        private readonly Image? _databaseImage;
        private readonly Image? _registryImage;
        private readonly Dictionary<(int RowIndex, int ColumnIndex), Action> _cellIconActions = new();

        public AcuManager AcuManager { get; private set; }

        public MainForm()
        {
            InitializeComponent();
            _instance = this;

            ProgressHandler = new Progress<string>(message => this.toolStripStatusLabel.Text = message);

            _warningImage = LoadImage("AcuCleaner.Images.warning.png");
            _folderImage = LoadImage("AcuCleaner.Images.folder.png");
            _databaseImage = LoadImage("AcuCleaner.Images.database.png");
            _registryImage = LoadImage("AcuCleaner.Images.registry.png");

            AcuManager = new AcuManager(ProgressHandler);
        }

        private Image? LoadImage(string resourceName)
        {
            using var stream = typeof(MainForm).Assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                return Image.FromStream(stream);
            }
            return null;
        }

        public static void HandleError(string message)
        {
            if (_instance == null) return;

            if (_instance.InvokeRequired)
                _instance.Invoke(() => _instance.ProgressHandler.Report($"Error: {message}"));
            else
                _instance.ProgressHandler.Report($"Error: {message}");

            _instance.AcuManager.IISManager.Start();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            toolStripProgressBar.Visible = false;
            ProgressHandler.Report("Ready");

            RefreshAppList();
        }

        private void RefreshAppList()
        {
            SetParameters();

            _allApps = AcuManager
                .GetAllInstances()
                .Filter()
                .OrderBy(x => x.InstanceName)
                .AddRowNumbers()
                .ToList();

            iISAppBindingSource1.DataSource = _allApps;
            iISAppBindingSource1.ResetBindings(false);

            ProgressHandler.Report($"{_allApps.Count} instance(s) loaded.");
        }

        private void dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var app = dataGridView.Rows[e.RowIndex].DataBoundItem as IISApp;
            if (app == null) return;

            _cellIconActions.Remove((e.RowIndex, e.ColumnIndex));

            if (!app.Active)
            {
                PaintCell(e, Status.Index, app.Status, _warningImage, () => { });
            }

            if (!string.IsNullOrEmpty(app.Database))
            {
                PaintCell(e, Database.Index, app.Database, _databaseImage, () => { });

            }

            if (!string.IsNullOrEmpty(app.Path))
            {
                PaintCell(e, FilesPath.Index, app.Path, _folderImage, () => AcuManager.FileSystemManager.OpenInExplorer(app.Path));
            }

            if (!string.IsNullOrEmpty(app.AspTempFilesPath))
            {
                PaintCell(e, AspTempFilesPath.Index, app.AspTempFilesPath, _folderImage, () => AcuManager.FileSystemManager.OpenInExplorer(app.AspTempFilesPath));
            }

            if (!string.IsNullOrEmpty(app.SnapshotPath))
            {
                PaintCell(e, SnapshotPath.Index, app.SnapshotPath, _folderImage, () => AcuManager.FileSystemManager.OpenInExplorer(app.SnapshotPath));
            }

            if (!string.IsNullOrEmpty(app.CustomizationPath))
            {
                PaintCell(e, CustomizationPath.Index, app.CustomizationPath, _folderImage, () => AcuManager.FileSystemManager.OpenInExplorer(app.CustomizationPath));
            }

            if (!string.IsNullOrEmpty(app.BackupPath))
            {
                PaintCell(e, BackupPath.Index, app.BackupPath, _folderImage, () => AcuManager.FileSystemManager.OpenInExplorer(app.BackupPath));
            }

            if (!string.IsNullOrEmpty(app.RegistryPath))
            {
                PaintCell(e, RegistryPath.Index, app.RegistryPath, _registryImage, () => AcuManager.RegistryManager.OpenInRegedit(app.RegistryPath));
            }
        }

        private void PaintCell(DataGridViewCellPaintingEventArgs e, int index, string text, Image? image, Action action)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != index)
            {
                return;
            }

            var isSelected = (e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected;
            using (var backgroundBrush = new SolidBrush(isSelected ? e.CellStyle!.SelectionBackColor : e.CellStyle!.BackColor))
            {
                e.Graphics!.FillRectangle(backgroundBrush, e.CellBounds);
            }
            e.Paint(e.CellBounds, DataGridViewPaintParts.Border);

            var iconSize = 16;
            var padding = 4;
            var textBounds = new Rectangle(
                e.CellBounds.Left,
                e.CellBounds.Top,
                e.CellBounds.Width - iconSize,
                e.CellBounds.Height);

            TextRenderer.DrawText(
                e.Graphics!,
                text,
                e.CellStyle!.Font,
                textBounds,
                isSelected ? e.CellStyle.SelectionForeColor : e.CellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

            if (image != null)
            {
                var iconX = e.CellBounds.Right - iconSize - padding;
                var iconY = e.CellBounds.Top + ((e.CellBounds.Height - iconSize) / 2);
                e.Graphics!.DrawImage(image, new Rectangle(iconX, iconY, iconSize, iconSize));
            }

            _cellIconActions[(e.RowIndex, e.ColumnIndex)] = action;

            e.Handled = true;
        }

        private void dataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (_cellIconActions.TryGetValue((e.RowIndex, e.ColumnIndex), out var action))
            {
                action();
            }
        }

        private async Task DeleteSelectedApps()
        {
            FilterSelectedApps();

            if (!AcuManager.CleanupParameters.Apps.Any())
            {
                ProgressHandler.Report("No rows selected.");
                return;
            }

            var message = $"Delete {AcuManager.CleanupParameters.Apps.Count()} selected app(s)?";

            var confirmation = MessageBox.Show(
                message,
                "Confirm delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirmation != DialogResult.Yes)
            {
                ProgressHandler.Report("Delete canceled.");
                return;
            }

            buttonDeleteSelected.Enabled = false;
            buttonDeleteAll.Enabled = false;
            toolStripProgressBar.Visible = true;
            ProgressHandler.Report($"Deleting {AcuManager.CleanupParameters.Apps.Count()} app(s)...");

            try
            {
                await Task.Run(() => AcuManager.Cleanup());
                ProgressHandler.Report($"Deleted {AcuManager.CleanupParameters.Apps.Count()} app(s).");
                RefreshAppList();
            }
            catch (Exception ex)
            {
                ProgressHandler.Report($"Delete failed: {ex.Message}");
            }
            finally
            {
                toolStripProgressBar.Visible = false;
                buttonDeleteSelected.Enabled = true;
                buttonDeleteAll.Enabled = true;
            }
        }

        private void FilterSelectedApps()
        {
            SetParameters();

            foreach (DataGridViewRow selectedRow in dataGridView.SelectedRows)
            {
                if (selectedRow.DataBoundItem is IISApp app)
                {
                    (AcuManager.CleanupParameters.Apps as List<IISApp>)?.Add(app);
                }
            }
        }

        private void SetCleanupParamerers()
        {
            AcuManager.CleanupParameters = new CleanupParameters
            {
                CleanupIISApps = checkBoxcleanupIISApps.Checked,
                CleanupFiles = checkBoxCleanupFiles.Checked,
                CleanupDatabases = checkBoxCleanupDatabases.Checked,
                CleanupAspTemp = checkBoxCleanupAspTemp.Checked,
                CleanupSnapshots = checkBoxCleanupSnapshots.Checked,
                CleanupRegistry = checkBoxCleanupRegistry.Checked,
                CleanupCustomizations = checkBoxCleanupCustomizations.Checked,
                CleanupBackups = checkBoxCleanupBackups.Checked,
            };
        }

        private void SetFilterParameters()
        {
            AcuManager.FilterParameters = new FilterParameters
            {
                ShowActiveApps = checkBoxActive.Checked,
                ShowJunk = checkBoxJunk.Checked,
                ShowFiles = checkBoxIISFiles.Checked,
                ShowAspTempFiles = checkBoxAspTmpFiles.Checked,
                ShowDatabases = checkBoxDatabases.Checked,
                ShowRegistry = checkBoxRegistry.Checked,
                ShowSnapshots = checkBoxSnapshots.Checked,
                ShowCustomizations = checkBoxCustomizations.Checked,
                ShowBackups = checkBoxBackups.Checked,
                CutoffDate = dateTimePicker.Value.Date
            };
        }

        private void SetParameters()
        {
            SetFilterParameters();
            SetCleanupParamerers();
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            RefreshAppList();
        }

        private async void buttonDeleteSelected_Click(object sender, EventArgs e)
        {
            await DeleteSelectedApps();
        }

        private void buttonDeleteAll_Click(object sender, EventArgs e)
        {
            dataGridView.SelectAll();

            DeleteSelectedApps();
        }

        private void filter_CheckedChanged(object sender, EventArgs e)
        {
            RefreshAppList();
        }

        private void checkBoxJunk_CheckedChanged(object sender, EventArgs e)
        {
            RefreshAppList();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshAppList();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterSelectedApps();

            var result = MessageBox.Show($"This action will reset password for {AcuManager.CleanupParameters.Apps.Count()} selected instance(s). Are you sure you want to continue?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                AcuManager.DatabaseManager.ResetPassword(AcuManager.CleanupParameters);
            }
        }

        private void enableModernUISwitchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterSelectedApps();

            var result = MessageBox.Show($"This action will enable Modern UI switch for {AcuManager.CleanupParameters.Apps.Count()} selected instance(s). Are you sure you want to continue?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                AcuManager.WebConfigManager.EnableModernUISwitch(AcuManager.CleanupParameters);
            }
        }

        private void setSessionTimeoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterSelectedApps();

            var result = MessageBox.Show($"This action will set session timeout to 1 day for {AcuManager.CleanupParameters.Apps.Count()} selected instance(s). Are you sure you want to continue?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                AcuManager.WebConfigManager.ExtendSessionTimeout(AcuManager.CleanupParameters);
            }
        }
    }
}
