using AcuCleaner.Models;
using AcuCleaner.Services;

namespace AcuCleaner.Forms
{
    public partial class MainForm : Form
    {
        private static MainForm? _instance;
        private List<IISApp> _allApps = new List<IISApp>();
        private readonly IProgress<string> _progressHandler;
        private readonly Image? _warningImage;

        public AcuManager AcuManager { get; private set; }

        public MainForm()
        {
            InitializeComponent();
            _instance = this;

            _progressHandler = new Progress<string>(message => this.toolStripStatusLabel.Text = message);

            var warningResourceName = "AcuCleaner.warning.png";
            using var stream = typeof(MainForm).Assembly.GetManifestResourceStream(warningResourceName);
            if (stream != null)
            {
                _warningImage = Image.FromStream(stream);
            }

            AcuManager = new AcuManager(_progressHandler);
        }

        public static void HandleError(string message)
        {
            if (_instance == null) return;

            if (_instance.InvokeRequired)
                _instance.Invoke(() => _instance.toolStripStatusLabel.Text = $"Error: {message}");
            else
                _instance.toolStripStatusLabel.Text = $"Error: {message}";

            _instance.AcuManager.IISManager.Start();
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

            _allApps = AcuManager
                .GetAllInstances()
                .Filter()
                .OrderBy(x => x.InstanceName)
                .AddRowNumbers()
                .ToList();

            iISAppBindingSource1.DataSource = _allApps;
            iISAppBindingSource1.ResetBindings(false);

            toolStripStatusLabel.Text = $"{_allApps.Count} instance(s) loaded.";
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
                app.Status,
                e.CellStyle!.Font,
                textBounds,
                isSelected ? e.CellStyle.SelectionForeColor : e.CellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

            if (_warningImage != null)
            {
                var iconX = e.CellBounds.Right - iconSize - padding;
                var iconY = e.CellBounds.Top + ((e.CellBounds.Height - iconSize) / 2);
                e.Graphics!.DrawImage(_warningImage, new Rectangle(iconX, iconY, iconSize, iconSize));
            }

            e.Handled = true;
        }

        private void DeleteSelectedApps()
        {
            FilterSelectedApps();

            if (!AcuManager.CleanupParameters.Apps.Any())
            {
                toolStripStatusLabel.Text = "No rows selected.";
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
                toolStripStatusLabel.Text = "Delete canceled.";
                return;
            }

            buttonDeleteSelected.Enabled = false;
            buttonDeleteAll.Enabled = false;
            toolStripProgressBar.Visible = true;
            toolStripStatusLabel.Text = $"Deleting {AcuManager.CleanupParameters.Apps.Count()} app(s)...";

            try
            {
                AcuManager.Cleanup();
                toolStripStatusLabel.Text = $"Deleted {AcuManager.CleanupParameters.Apps.Count()} app(s).";
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

        private void buttonDeleteSelected_Click(object sender, EventArgs e)
        {
            DeleteSelectedApps();
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
