using IISCleanup.Models;
using IISCleanup.Services;

namespace IISCleanup.Forms
{
    public partial class MainForm : Form
    {
        private static MainForm? _instance;

        private List<IISApp> _allApps = new List<IISApp>();
        private readonly IProgress<string> _progressHandler;
        public IISService IISService { get; private set; }
        public RegistryService RegistryService { get; private set; }
        public ICleanupService CleanupService { get; private set; }

        public MainForm()
        {
            InitializeComponent();
            _instance = this;

            _progressHandler = new Progress<string>(message => this.toolStripStatusLabel.Text = message);
            IISService = new IISService(_progressHandler);
            RegistryService = new RegistryService();
            CleanupService = new CleanupService(IISService, RegistryService, _progressHandler);
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
            _allApps = CleanupService
                .GetAllApps()
                .FilterByStatus(checkBoxActive.Checked, checkBoxJunk.Checked)
                .FilterByDate(dateTimePicker.Value.Date)
                .OrderBy(x => x.InstanceName)
                .AddRowNumbers()
                .ToList();

            iISAppBindingSource1.DataSource = _allApps;
            iISAppBindingSource1.ResetBindings(false);
        }

        private async Task DeleteSelectedAppsAsync(CleanupParameters cleanupParameters)
        {
            var selectedApps = new List<IISApp>();
            foreach (DataGridViewRow selectedRow in dataGridView.SelectedRows)
            {
                if (selectedRow.DataBoundItem is IISApp app)
                {
                    selectedApps.Add(app);
                }
            }

            if (selectedApps.Count == 0)
            {
                toolStripStatusLabel.Text = "No rows selected.";
                return;
            }

            var message = $"Delete {selectedApps.Count} selected app(s)?";

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
            toolStripStatusLabel.Text = $"Deleting {selectedApps.Count} app(s)...";

            try
            {
                await Task.Run(() => CleanupService.Ceanup(selectedApps, cleanupParameters));
                toolStripStatusLabel.Text = $"Deleted {selectedApps.Count} app(s).";
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

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            RefreshAppList();
        }

        private async void buttonDeleteSelected_Click(object sender, EventArgs e)
        {
            DeleteConfirmationForm confirmation = new DeleteConfirmationForm();
            var result = confirmation.ShowDialog();
            if (result == DialogResult.Yes)
            {
                await DeleteSelectedAppsAsync(confirmation.CleanupParameters);
            }
        }

        private async void buttonDeleteAll_Click(object sender, EventArgs e)
        {
            dataGridView.SelectAll();

            DeleteConfirmationForm confirmation = new DeleteConfirmationForm();
            var result = confirmation.ShowDialog();
            if (result == DialogResult.Yes)
            {
                await DeleteSelectedAppsAsync(confirmation.CleanupParameters);
            }
        }

        private void checkBoxActive_CheckedChanged(object sender, EventArgs e)
        {
            RefreshAppList();
        }

        private void checkBoxJunk_CheckedChanged(object sender, EventArgs e)
        {
            RefreshAppList();
        }
    }
}
