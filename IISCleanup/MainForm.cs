using IISCleanup.Models;
using IISCleanup.Services;

namespace IISCleanup
{
    public partial class Form : System.Windows.Forms.Form
    {
        private List<IISApp> _allApps = new List<IISApp>();

        public Form()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            toolStripProgressBar.Visible = false;
            toolStripStatusLabel.Text = "Ready";
            RefreshAppList();
        }

        private void RefreshAppList()
        {
            _allApps = IISService
                .GetAllApps()
                .FilterByStatus(checkBoxActive.Checked, checkBoxJunk.Checked)
                .FilterByDate(dateTimePicker.Value.Date)
                .OrderBy(x => x.Instance)
                .AddRowNumbers()
                .ToList();

            iISAppBindingSource1.DataSource = _allApps;
            iISAppBindingSource1.ResetBindings(false);
        }

        private async Task DeleteSelectedAppsAsync(bool deleteAll = false)
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

            var message = deleteAll
                ? $"Delete all {selectedApps.Count} listed app(s)?"
                : $"Delete {selectedApps.Count} selected app(s)?";

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
                await Task.Run(() => IISService.CeanupIISApps(selectedApps, dateTimePicker.Value.Date));
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
            await DeleteSelectedAppsAsync();
        }

        private async void buttonDeleteAll_Click(object sender, EventArgs e)
        {
            dataGridView.SelectAll();
            await DeleteSelectedAppsAsync(deleteAll: true);
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
