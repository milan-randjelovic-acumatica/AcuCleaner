namespace AcuCleaner
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += (sender, e) =>
                Forms.MainForm.HandleError(e.Exception.Message);

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
                Forms.MainForm.HandleError(e.ExceptionObject?.ToString() ?? "An unexpected error occurred.");

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Forms.MainForm());
        }
    }
}
