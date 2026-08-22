using Microsoft.VisualStudio.PlatformUI;

namespace AcuCleanerVSExtension
{
    /// <summary>
    /// AcuCleaner popup dialog window.
    /// Opened via Tools > AcuCleaner.
    /// </summary>
    public partial class AcuCleanerDialog : DialogWindow
    {
        public AcuCleanerDialog()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }
    }
}
