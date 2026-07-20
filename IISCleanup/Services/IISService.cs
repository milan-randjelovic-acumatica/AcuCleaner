using IISCleanup.Models;
using Microsoft.Web.Administration;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;

namespace IISCleanup.Services
{
    internal static class IISService
    {
        private const string TemporaryAspFiles = "TemporaryAspFiles";

        public static void StopIIS()
        {
            var service = new ServiceController("W3SVC");

            if (service.Status == ServiceControllerStatus.Running)
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
            }
        }

        public static void StartIIS()
        {
            var service = new ServiceController("W3SVC");
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
            }
        }

        public static IEnumerable<IISApp> GetAllApps()
        {
            var iisApps = GetIISApps();

            var iisRootFolders = iisApps.Select(x => x.RootPath).Distinct().ToList();

            var iisJunk = GetIISJunk(iisRootFolders);

            var allApps = iisApps
                .UnionBy(iisJunk, x => x.Path.NormalizePath())
                .ExcludeStandardFolders();

            return allApps;
        }

        public static IEnumerable<IISApp> GetIISApps()
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

            return result.DistinctBy(app => app.Path);
        }

        public static IEnumerable<IISApp> GetIISJunk(IEnumerable<string> rootFolders)
        {
            var result = new List<IISApp>();

            foreach (var rootFolder in rootFolders)
            {
                var folders = Directory.GetDirectories(rootFolder);
                foreach (var folder in folders)
                {
                    string folderName = Path.GetFileName(folder).NormalizePath();

                    result.Add(new IISApp
                    {
                        Instance = folderName,
                        Path = folder.NormalizePath(),
                        RootPath = rootFolder.NormalizePath(),
                        Active = false,
                        InstalledOn = Directory.GetCreationTime(folder),
                    });

                    if (folderName == TemporaryAspFiles)
                    {
                        var tmpASPApps = GetIISJunk(new List<string> { folder });
                        foreach (var tmpApp in tmpASPApps)
                        {
                            string tmpAppfolderName = Path.GetFileName(tmpApp.Path).NormalizePath();

                            result.Add(new IISApp
                            {
                                Instance = tmpAppfolderName,
                                Path = tmpApp.Path.NormalizePath(),
                                RootPath = folder.Normalize(),
                                Active = false,
                                InstalledOn = Directory.GetCreationTime(folder),
                            });
                        }
                    }
                }
            }

            return result.DistinctBy(app => app.Path);
        }

        public static IEnumerable<IISApp> FilterByStatus(this IEnumerable<IISApp> iisApps, bool isActive, bool junk)
        {
            iisApps = iisApps
                .Where(x => x.Active == isActive || x.Active == !junk);

            return iisApps;
        }

        public static IEnumerable<IISApp> FilterByDate(this IEnumerable<IISApp> iisApps, DateTime cutoff)
        {
            iisApps = iisApps
                .Where(x => x.InstalledOn.Date <= cutoff)
                .OrderBy(x => x.InstalledOn);

            return iisApps;
        }

        public static IEnumerable<IISApp> AddRowNumbers(this IEnumerable<IISApp> allApps)
        {
            for (int i = 0; i < allApps.Count(); i++)
            {
                allApps.ElementAt(i).Id = i + 1;
            }

            return allApps;
        }

        public static void DeleteIISApps(List<IISApp> iisApps)
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

                    serverManager.CommitChanges();
                }
            }
        }

        public static void DeleteFiles(List<IISApp> iisApps)
        {
            foreach (var iisApp in iisApps)
            {
                if (!Directory.Exists(iisApp.Path))
                {
                    continue;
                }

                PrepareDirectoryForDeletion(iisApp.Path);
                Directory.Delete(iisApp.Path, true);
            }
        }

        public static void CeanupIISApps(List<IISApp> iisApps)
        {
            StopIIS();

            DeleteIISApps(iisApps);
            DeleteFiles(iisApps);

            StartIIS();
        }

        public static List<Microsoft.Web.Administration.Application> FindAppsToRemove(Site site, List<IISApp> iisApps)
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

        public static void PrepareDirectoryForDeletion(string path)
        {
            GrantFullControl(path);
            File.SetAttributes(path, FileAttributes.Normal);

            foreach (var directory in Directory.GetDirectories(path, "*", SearchOption.AllDirectories))
            {
                GrantFullControl(directory);
                File.SetAttributes(directory, FileAttributes.Normal);
            }

            foreach (var file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
            {
                GrantFullControl(file);
                File.SetAttributes(file, FileAttributes.Normal);
            }
        }

        public static void GrantFullControl(string path)
        {
            var admins = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
            var rule = new FileSystemAccessRule(
                admins,
                FileSystemRights.FullControl,
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                PropagationFlags.None,
                AccessControlType.Allow);

            if (Directory.Exists(path))
            {
                var dirInfo = new DirectoryInfo(path);
                var security = dirInfo.GetAccessControl();
                security.AddAccessRule(rule);
                dirInfo.SetAccessControl(security);
                return;
            }

            if (File.Exists(path))
            {
                var fileInfo = new FileInfo(path);
                var security = fileInfo.GetAccessControl();
                security.AddAccessRule(new FileSystemAccessRule(
                    admins,
                    FileSystemRights.FullControl,
                    AccessControlType.Allow));
                fileInfo.SetAccessControl(security);
            }
        }

    }
}
