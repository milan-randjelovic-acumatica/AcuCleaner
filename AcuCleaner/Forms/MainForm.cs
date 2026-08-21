using AcuCleaner.Models;
using AcuCleaner.Services;
using AcuCleaner.Services.Database;
using AcuCleaner.Services.FileSystem;
using AcuCleaner.Services.IIS;
using AcuCleaner.Services.Registry;

namespace AcuCleaner.Forms
{
    public partial class MainForm : Form
    {
        private static MainForm? _instance;
        private List<IISApp> _allApps = new List<IISApp>();
        private readonly IProgress<string> _progressHandler;
        private readonly Image? _warningImage;

        public CleanupService CleanupService { get; private set; }

        public MainForm()
        {
            InitializeComponent();
            _instance = this;

            _progressHandler = new Progress<string>(message => this.toolStripStatusLabel.Text = message);

            var warningImagePath = Path.Combine(AppContext.BaseDirectory, "warning.png");
            if (File.Exists(warningImagePath))
            {
                using var stream = File.OpenRead(warningImagePath);
                _warningImage = Image.FromStream(stream);
            }

            CleanupService = new CleanupService(new List<ICleanupService>()
            {
                new IISCleaner(_progressHandler),
                new FileSystemCleaner(_progressHandler),
                new DatabaseCleaner(_progressHandler),
                new RegistryCleaner(_progressHandler)
            });
        }

        public static void HandleError(string message)
        {
            if (_instance == null) return;

            if (_instance.InvokeRequired)
                _instance.Invoke(() => _instance.toolStripStatusLabel.Text = $"Error: {message}");
            else
                _instance.toolStripStatusLabel.Text = $"Error: {message}";

            _instance.CleanupService.Cleaners.OfType<IISCleaner>().FirstOrDefault()?.Start();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            toolStripProgressBar.Visible = false;
            toolStripStatusLabel.Text = "Ready";

            RefreshAppList();
        }

        private void RefreshAppList()
        {
            SetParameters();

            _allApps = CleanupService
                .GetAll()
                .Filter()
                .OrderBy(x => x.InstanceName)
                .AddRowNumbers()
                .ToList();

            iISAppBindingSource1.DataSource = _allApps;
            iISAppBindingSource1.ResetBindings(false);
        }

        private void dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != Status.Index)
            {
                return;
            }

            if (dataGridView.Rows[e.RowIndex].DataBoundItem is not IISApp app || app.Active)
            {
                return;
            }

            e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

            var iconSize = 16;
            var padding = 4;
            var textBounds = new Rectangle(
                e.CellBounds.Left,
                e.CellBounds.Top,
                e.CellBounds.Width - iconSize,
                e.CellBounds.Height);

            TextRenderer.DrawText(
                e.Graphics!,
                app.Status,
                e.CellStyle!.Font,
                textBounds,
                e.CellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

            if (_warningImage != null)
            {
                var iconX = e.CellBounds.Right - iconSize - padding;
                var iconY = e.CellBounds.Top + ((e.CellBounds.Height - iconSize) / 2);
                e.Graphics!.DrawImage(_warningImage, new Rectangle(iconX, iconY, iconSize, iconSize));
            }

            e.Handled = true;
        }

        private async Task DeleteSelectedAppsAsync()
        {
            FilterSelectedApps();

            if (!CleanupService.CleanupParameters.Apps.Any())
            {
                toolStripStatusLabel.Text = "No rows selected.";
                return;
            }

            var message = $"Delete {CleanupService.CleanupParameters.Apps.Count()} selected app(s)?";

            var confirmation = MessageBox.Show(
                message,
                "Confirm delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirmation != DialogResult.Yes)
            {
                toolStripStatusLabel.Text = "Delete canceled.";
                return;
            }

            buttonDeleteSelected.Enabled = false;
            buttonDeleteAll.Enabled = false;
            toolStripProgressBar.Visible = true;
            toolStripStatusLabel.Text = $"Deleting {CleanupService.CleanupParameters.Apps.Count()} app(s)...";

            try
            {
                await Task.Run(() => CleanupService.Cleanup());
                toolStripStatusLabel.Text = $"Deleted {CleanupService.CleanupParameters.Apps.Count()} app(s).";
                RefreshAppList();
            }
            catch (Exception ex)
            {
                toolStripStatusLabel.Text = $"Delete failed: {ex.Message}";
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
                    (CleanupService.CleanupParameters.Apps as List<IISApp>)?.Add(app);
                }
            }
        }

        private void SetCleanupParamerers()
        {
            CleanupService.CleanupParameters = new CleanupParameters
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
            CleanupService.FilterParameters = new FilterParameters
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
            await DeleteSelectedAppsAsync();
        }

        private async void buttonDeleteAll_Click(object sender, EventArgs e)
        {
            dataGridView.SelectAll();

            await DeleteSelectedAppsAsync();
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

            var result = MessageBox.Show($"This action will reset password for {CleanupService.CleanupParameters.Apps.Count()} selected instance(s). Are you sure you want to continue?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                CleanupService.Cleaners.OfType<DatabaseCleaner>().FirstOrDefault()?.ResetPassword(CleanupService.CleanupParameters);
            }
        }

        private void enableModernUISwitchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterSelectedApps();

            var result = MessageBox.Show($"This action will enable Modern UI switch for {CleanupService.CleanupParameters.Apps.Count()} selected instance(s). Are you sure you want to continue?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                CleanupService.Cleaners.OfType<FileSystemCleaner>().FirstOrDefault()?.EnableModernUISwitch(CleanupService.CleanupParameters);
            }
        }

        private void setSessionTimeoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterSelectedApps();

            var result = MessageBox.Show($"This action will set session timeout to 1 day for {CleanupService.CleanupParameters.Apps.Count()} selected instance(s). Are you sure you want to continue?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                CleanupService.Cleaners.OfType<FileSystemCleaner>().FirstOrDefault()?.ExtendSessionTimeout(CleanupService.CleanupParameters);
            }
        }
    }
}
