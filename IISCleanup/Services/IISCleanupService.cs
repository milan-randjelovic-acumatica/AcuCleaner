using IISCleanup.Models;
using Microsoft.Web.Administration;
using System.ServiceProcess;

namespace IISCleanup.Services
{
    public class IISCleanupService : ICleanupService
    {
        private readonly IProgress<string>? _progressHandler;

        public IISCleanupService()
        {           
        }

        public IISCleanupService(IProgress<string>? progressHandler)
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

        public IEnumerable<IISApp> GetAllApps()
        {
            var result = new List<IISApp>();
            using (ServerManager serverManager = new ServerManager())
            {
                foreach (var site in serverManager.Sites)
                {
                    foreach (var app in site.Applications)
                    {
                        if (app.Path == "/")
                        {
                            continue;
                        }

                        var path = app.VirtualDirectories.First().PhysicalPath;
                        var rootPath = new DirectoryInfo(path).Parent?.FullName ?? string.Empty;
                        var instanceName = app.Path.NormalizePath();

                        if (Directory.GetFiles(rootPath).Select(x => x.Split("\\").Last()).Contains("Pure.sln"))
                        {
                            continue;
                        }

                        result.Add(new IISApp
                        {
                            InstanceName = instanceName,
                            Path = path.NormalizePath(),
                            RootPath = rootPath.NormalizePath(),
                            AspFilesTempPath = Path.Combine(rootPath, Constants.TemporaryAspFiles, instanceName).NormalizePath(),
                            DatabaseName = string.Empty,
                            DatabaseConnectionString = string.Empty,
                            RegistryPath = string.Empty,
                            Active = true,
                            InstalledOn = Directory.GetCreationTime(path),
                        });
                    }
                }
            }

            return result.DistinctBy(app => app.Path);
        }

        public IEnumerable<IISApp> GetAppsForCleanup(CleanupParameters cleanupParameters)
        {
            throw new NotImplementedException();
        }

        public void Cleanup(CleanupParameters cleanupParameters)
        {
            if (cleanupParameters.CleanupIISApps)
            {
                using (ServerManager serverManager = new ServerManager())
                {
                    foreach (var site in serverManager.Sites)
                    {
                        var appsToRemove = FindAppsToRemove(site, cleanupParameters.Apps);
                        int i = 1;
                        foreach (var app in appsToRemove)
                        {
                            _progressHandler?.Report($"Removing IIS entry {i++}/{appsToRemove.Count}: {app.Path}");
                            site.Applications.Remove(app);
                        }

                        serverManager.CommitChanges();
                    }
                }
            }
        }

        public List<Microsoft.Web.Administration.Application> FindAppsToRemove(Site site, IEnumerable<IISApp> iisApps)
        {
            var appsToRemove = new List<Microsoft.Web.Administration.Application>();
            var appInstances = iisApps.Where(app => !string.IsNullOrEmpty(app.InstanceName)).Select(app => app.InstanceName);
            foreach (var appInstance in appInstances)
            {
                foreach (var app in site.Applications)
                {
                    if (!string.IsNullOrEmpty(appInstance))
                    {
                        if (app.Path.NormalizePath() == appInstance)
                        {
                            appsToRemove.Add(app);
                        }
                    }
                }
            }

            return appsToRemove;
        }
    }
}
