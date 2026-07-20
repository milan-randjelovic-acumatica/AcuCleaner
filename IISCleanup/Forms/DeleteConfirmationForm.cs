namespace IISCleanup.Forms
{
    public partial class DeleteConfirmationForm : Form
    {
        public DeleteConfirmationForm()
        {
            InitializeComponent();
        }

        private void buttonCleanup_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        { 
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
