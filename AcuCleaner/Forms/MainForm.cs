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
        public AcuCleanerService IISService { get; private set; }
        public FileSystemCleanupService FileSystemCleanupService { get; private set; }
        public RegistryCleanupService RegistryService { get; private set; }
        public DatabaseCleanupService DatabaseCleanupService { get; private set; }
        public CleanupService CleanupService { get; private set; }
        public CleanupParameters CleanupParameters { get; private set; }

        public MainForm()
        {
            InitializeComponent();
            _instance = this;

            _progressHandler = new Progress<string>(message => this.toolStripStatusLabel.Text = message);
            IISService = new AcuCleanerService(_progressHandler);
            FileSystemCleanupService = new FileSystemCleanupService(_progressHandler);
            DatabaseCleanupService = new DatabaseCleanupService(_progressHandler);
            RegistryService = new RegistryCleanupService();
            CleanupService = new CleanupService(IISService, FileSystemCleanupService, DatabaseCleanupService, RegistryService, _progressHandler);
            CleanupParameters = new CleanupParameters();
        }

        public static void ShowError(string message)
        {
            if (_instance == null) return;

            if (_instance.InvokeRequired)
                _instance.Invoke(() => _instance.toolStripStatusLabel.Text = $"Error: {message}");
            else
                _instance.toolStripStatusLabel.Text = $"Error: {message}";

            _instance.IISService.Start();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            toolStripProgressBar.Visible = false;
            toolStripStatusLabel.Text = "Ready";

            RefreshAppList();
        }

        private void RefreshAppList()
        {
            SetCleanupParamerers();

            _allApps = CleanupService
                .GetAll(CleanupParameters)
                .FilterByStatus(checkBoxActive.Checked, checkBoxJunk.Checked)
                .FilterByDate(dateTimePicker.Value.Date)
                .OrderBy(x => x.InstanceName)
                .AddRowNumbers()
                .ToList();

            iISAppBindingSource1.DataSource = _allApps;
            iISAppBindingSource1.ResetBindings(false);
        }

        private async Task DeleteSelectedAppsAsync()
        {
            SetCleanupParamerers();

            foreach (DataGridViewRow selectedRow in dataGridView.SelectedRows)
            {
                if (selectedRow.DataBoundItem is IISApp app)
                {
                    CleanupParameters.Apps.Add(app);
                }
            }

            if (CleanupParameters.Apps.Count == 0)
            {
                toolStripStatusLabel.Text = "No rows selected.";
                return;
            }

            var message = $"Delete {CleanupParameters.Apps.Count} selected app(s)?";

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
            toolStripStatusLabel.Text = $"Deleting {CleanupParameters.Apps.Count} app(s)...";

            try
            {
                await Task.Run(() => CleanupService.Cleanup(CleanupParameters));
                toolStripStatusLabel.Text = $"Deleted {CleanupParameters.Apps.Count} app(s).";
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

        private void SetCleanupParamerers()
        {
            CleanupParameters = new CleanupParameters
            {
                CleanupIISApps = checkBoxcleanupIISApps.Checked,
                CleanupFiles = checkBoxCleanupFiles.Checked,
                CleanupDatabases = checkBoxCleanupDatabases.Checked,
                CleanupAspTemp = checkBoxCleanupAspTemp.Checked,
                CleanupSnapshots = checkBoxCleanupSnapshots.Checked,
                CleanupRegistry = checkBoxCleanupRegistry.Checked,
                CleanupCustomizations = checkBoxCleanupCustomizations.Checked,
            };
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

        private void checkBoxActive_CheckedChanged(object sender, EventArgs e)
        {
            RefreshAppList();
        }

        private void checkBoxJunk_CheckedChanged(object sender, EventArgs e)
        {
            RefreshAppList();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshAppList();
        }
    }
}
