using IISCleanup.Models;
using IISCleanup.Services;

namespace IISCleanup
{
    public partial class Form : System.Windows.Forms.Form
    {
        private List<IISApp> _allApps = new List<IISApp>();
        private IISService _iisService = new IISService();

        public Form()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            RefreshAppList();
        }

        private void RefreshAppList()
        {
            var iisApps = _iisService.GetIISApps();
            var iisJunk = _iisService.GetIISJunk(iisApps);

            var allApps = iisApps.UnionBy(iisJunk, x => x.Path.NormalizePath());
            allApps = allApps.ExcludeStandardFolders();

            allApps = _iisService.FilterByStatus(allApps, checkBoxActive.Checked, checkBoxJunk.Checked);

            allApps = _iisService.FilterByDate(allApps, dateTimePicker.Value.Date).ToList();
            for (int i = 0; i < allApps.Count(); i++)
            {
                allApps.ElementAt(i).Id = i + 1;
            }

            _allApps = allApps.OrderBy(x => x.Id).ToList();

            iISAppBindingSource1.DataSource = _allApps;
            iISAppBindingSource1.ResetBindings(false);
        }

        private void DeleteSelectedApps()
        {
            var selectedApps = new List<IISApp>();
            foreach (DataGridViewRow selectedRow in dataGridView.SelectedRows)
            {
                var app = selectedRow.DataBoundItem as IISApp;
                if (app != null)
                {
                    selectedApps.Add(app);
                }
            }

            _iisService.CeanupIISApps(selectedApps, dateTimePicker.Value.Date);

            RefreshAppList();
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
