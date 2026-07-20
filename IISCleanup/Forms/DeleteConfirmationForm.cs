using IISCleanup.Models;

namespace IISCleanup.Forms
{
    public partial class DeleteConfirmationForm : Form
    {
        public bool CleanupIISApps => checkBoxcleanupIISApps.Checked;
        public bool CleanupFiles => checkBoxCleanupFiles.Checked;
        public bool CleanupAspTemp => checkBoxCleanupAspTemp.Checked;
        public CleanupParameters CleanupParameters { get; private set; } = new CleanupParameters();

        public DeleteConfirmationForm()
        {
            InitializeComponent();
        }

        private void buttonCleanup_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            CleanupParameters.CleanupIISApps = checkBoxcleanupIISApps.Checked;
            CleanupParameters.CleanupFiles = checkBoxCleanupFiles.Checked;
            CleanupParameters.CleanupAspTemp = checkBoxCleanupAspTemp.Checked;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            CleanupParameters.CleanupIISApps = checkBoxcleanupIISApps.Checked;
            CleanupParameters.CleanupFiles = checkBoxCleanupFiles.Checked;
            CleanupParameters.CleanupAspTemp = checkBoxCleanupAspTemp.Checked;
            Close();
        }
    }
}
