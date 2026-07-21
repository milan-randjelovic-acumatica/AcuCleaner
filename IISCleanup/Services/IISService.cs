using System.ServiceProcess;

namespace IISCleanup.Services
{
    public class IISService
    {
        private readonly IProgress<string>? _progressHandler;

        public IISService()
        {
            
        }

        public IISService(IProgress<string>? progressHandler)
        {
            _progressHandler = progressHandler;
        }

        public void Stop()
        {
            _progressHandler?.Report("Stopping IIS...");

            var service = new ServiceController("W3SVC");

            if (service.Status == ServiceControllerStatus.Running)
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
            }

            _progressHandler?.Report("IIS stopped.");
        }

        public void Start()
        {
            _progressHandler?.Report("Starting IIS...");

            var service = new ServiceController("W3SVC");
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
            }

            _progressHandler?.Report("IIS started.");
        }
    }
}
