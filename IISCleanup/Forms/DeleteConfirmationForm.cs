using IISCleanup.Models;

namespace IISCleanup.Forms
{
    public partial class DeleteConfirmationForm : Form
    {
        public CleanupParameters CleanupParameters { get; private set; }

        public DeleteConfirmationForm(CleanupParameters cleanupParameters)
        {
            InitializeComponent();
            CleanupParameters = cleanupParameters;
        }

        private void buttonCleanup_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            CleanupParameters.CleanupIISApps = checkBoxcleanupIISApps.Checked;
            CleanupParameters.CleanupFiles = checkBoxCleanupFiles.Checked;
            CleanupParameters.CleanupAspTemp = checkBoxCleanupAspTemp.Checked;
            CleanupParameters.CleanupDatabases = checkBoxCleanupSqlDatabases.Checked;
            CleanupParameters.CleanupRegistry = checkBoxCleanupRegistry.Checked;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
