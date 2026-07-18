using IISCleanup.Models;
using Microsoft.Web.Administration;
using System.ServiceProcess;

namespace IISCleanup.Services
{
    internal class IISService
    {
        public void StopIIS()
        {
            var service = new ServiceController("W3SVC");

            if (service.Status == ServiceControllerStatus.Running)
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
            }
        }

        public void StartIIS()
        {
            var service = new ServiceController("W3SVC");
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
            }
        }

        public IEnumerable<IISApp> GetIISApps()
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

                        var physicalPath = app.VirtualDirectories.First().PhysicalPath;
                        var rootPath = new DirectoryInfo(physicalPath).Parent?.FullName ?? string.Empty;

                        if (Directory.GetFiles(rootPath).Select(x => x.Split("\\").Last()).Contains("Pure.sln"))
                        {
                            continue;
                        }

                        result.Add(new IISApp
                        {
                            Instance = app.Path.NormalizePath(),
                            Path = physicalPath.NormalizePath(),
                            RootPath = rootPath.NormalizePath(),
                            Active = true,
                            InstalledOn = Directory.GetCreationTime(physicalPath),
                        });
                    }
                }
            }

            return result;
        }

        public IEnumerable<IISApp> GetIISJunk(IEnumerable<IISApp> iisApps)
        {
            var result = new List<IISApp>();
            var iisRootPaths = iisApps.Select(x => x.RootPath).Distinct().ToList();
            foreach (var iisRootPath in iisRootPaths)
            {
                var folders = Directory.GetDirectories(iisRootPath);
                foreach (var folder in folders)
                {
                    result.Add(new IISApp
                    {
                        Instance = Path.GetFileName(folder).NormalizePath(),
                        Path = folder.NormalizePath(),
                        RootPath = iisRootPath.NormalizePath(),
                        Active = false,
                        InstalledOn = Directory.GetCreationTime(folder),
                    });
                }
            }
            return result;
        }

        public IEnumerable<IISApp> FilterByStatus(IEnumerable<IISApp> iisApps, bool isActive, bool junk)
        {
            var result = iisApps
                .Where(x => x.Active == isActive || x.Active == !junk);
            return result;
        }

        public IEnumerable<IISApp> FilterByDate(IEnumerable<IISApp> iisApps, DateTime cutoff)
        {
            var result = iisApps
                .Where(x => x.InstalledOn.Date <= cutoff)
                .OrderBy(x => x.InstalledOn);

            return result;
        }

        public void DeleteIISApps(List<IISApp> iisApps)
        {
            using (ServerManager serverManager = new ServerManager())
            {
                foreach (var site in serverManager.Sites)
                {
                    //delete selected apps from IIS
                    var appsToRemove = FindAppsToRemove(site, iisApps);
                    foreach (var app in appsToRemove)
                    {
                        site.Applications.Remove(app);
                    }

                    //delete junk files
                    foreach (var iisApp in iisApps)
                    {
                        if (Directory.Exists(iisApp.Path))
                        {
                            Directory.Delete(iisApp.Path, true);
                        }
                    }

                    serverManager.CommitChanges();
                }
            }
        }

        public void CeanupIISApps(List<IISApp> iisApps, DateTime cutoff)
        {
            StopIIS();

            DeleteIISApps(iisApps);

            StartIIS();
        }

        public List<Microsoft.Web.Administration.Application> FindAppsToRemove(Site site, List<IISApp> iisApps)
        {
            var appsToRemove = new List<Microsoft.Web.Administration.Application>();
            foreach (var appInstance in iisApps.Select(app => app.Instance))
            {
                foreach (var app in site.Applications)
                {
                    if (app.Path.NormalizePath() == appInstance)
                    {
                        appsToRemove.Add(app);
                    }
                }
            }

            return appsToRemove;
        }
    }
}
