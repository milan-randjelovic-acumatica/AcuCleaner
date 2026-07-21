using IISCleanup.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Web.Administration;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;

namespace IISCleanup.Services
{
    internal static class IISService
    {
        public static void StopIIS(IProgress<string>? progressHandler = null)
        {
            progressHandler?.Report("Stopping IIS...");

            var service = new ServiceController("W3SVC");

            if (service.Status == ServiceControllerStatus.Running)
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
            }

            progressHandler?.Report("IIS stopped.");
        }

        public static void StartIIS(IProgress<string>? progressHandler = null)
        {
            progressHandler?.Report("Starting IIS...");

            var service = new ServiceController("W3SVC");
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
            }

            progressHandler?.Report("IIS started.");
        }

        public static IEnumerable<IISApp> GetAllApps()
        {
            var iisApps = GetIISApps();

            var iisRootFolders = iisApps.Select(x => x.RootPath).Distinct().ToList();

            var iisJunk = GetIISJunk(iisRootFolders);

            var allApps = iisApps
                .UnionBy(iisJunk, x => x.InstanceName)
                .ExcludeStandardFolders();

            var sqlDatabases = GetSqlDatabases(allApps);

            allApps = allApps
                .UnionBy(sqlDatabases, x => x.DatabaseName);

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
                            Active = true,
                            InstalledOn = Directory.GetCreationTime(path),
                        });
                    }
                }
            }

            return result.DistinctBy(app => app.Path);
        }

        public static IEnumerable<IISApp> GetSqlDatabases(IEnumerable<IISApp> iisApps)
        {
            var result = new List<IISApp>();

            var sqlDatabases = GetAllSqlDatabases()
                              .ExcludeStandardSqlDatabases();

            foreach (var sqlDatabase in sqlDatabases)
            {
                var existingApp = iisApps.FirstOrDefault(app => app.InstanceName == sqlDatabase.DatabaseName);
                if (existingApp != null)
                {
                    existingApp.DatabaseName = sqlDatabase.DatabaseName;
                    result.Add(existingApp);
                }
                else
                {
                    result.Add(new IISApp
                    {
                        InstanceName = string.Empty,
                        DatabaseName = sqlDatabase.DatabaseName,
                        DatabaseConnectionString = sqlDatabase.DatabaseConnectionString,
                        Path = string.Empty,
                        RootPath = string.Empty,
                        AspFilesTempPath = string.Empty,
                        Active = false,
                        InstalledOn = DateTime.MinValue,
                    });
                }
            }

            return result;
        }

        public static IEnumerable<IISApp> GetAllSqlDatabases()
        {
            var result = new List<IISApp>();

            foreach (var connectionString in Constants.ConnectionStrings)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand("SELECT name FROM sys.databases ORDER BY name;", connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader.GetString(0));
                            result.Add(new IISApp
                            {
                                InstanceName = string.Empty,
                                DatabaseName = reader.GetString(0),
                                DatabaseConnectionString = connectionString,
                                Path = string.Empty,
                                RootPath = string.Empty,
                                AspFilesTempPath = string.Empty,
                                Active = false,
                                InstalledOn = DateTime.MinValue,
                            });
                        }
                    }
                }
            }

            return result;
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
                        InstanceName = folderName,
                        DatabaseName = string.Empty,
                        DatabaseConnectionString = string.Empty,
                        Path = folder.NormalizePath(),
                        RootPath = rootFolder.NormalizePath(),
                        AspFilesTempPath = string.Empty,
                        Active = false,
                        InstalledOn = Directory.GetCreationTime(folder),
                    });

                    if (folderName == Constants.TemporaryAspFiles)
                    {
                        var tmpASPApps = GetIISJunk(new List<string> { folder });
                        foreach (var tmpApp in tmpASPApps)
                        {
                            string tmpInstanceName = Path.GetFileName(tmpApp.Path).NormalizePath();

                            var existingApp = result.FirstOrDefault(app => app.InstanceName == tmpInstanceName);
                            if (existingApp != null)
                            {
                                existingApp.AspFilesTempPath = tmpApp.Path.NormalizePath();
                            }
                            else
                            {
                                result.Add(new IISApp
                                {
                                    InstanceName = tmpInstanceName,
                                    DatabaseName = string.Empty,
                                    DatabaseConnectionString = string.Empty,
                                    Path = string.Empty,
                                    RootPath = folder.NormalizePath(),
                                    AspFilesTempPath = tmpApp.Path.NormalizePath(),
                                    Active = false,
                                    InstalledOn = Directory.GetCreationTime(folder),
                                });
                            }
                        }
                    }
                }
            }

            return result.DistinctBy(app => app.Path);
        }

        public static IEnumerable<IISApp> FilterByStatus(this IEnumerable<IISApp> iisApps, bool isActive, bool junk)
        {
            return iisApps.Where(x => x.Active == isActive || (junk == true && x.Active == false));
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

        public static void DeleteIISApps(List<IISApp> iisApps, IProgress<string>? progressHandler = null)
        {
            using (ServerManager serverManager = new ServerManager())
            {
                foreach (var site in serverManager.Sites)
                {
                    var appsToRemove = FindAppsToRemove(site, iisApps);
                    int i = 1;
                    foreach (var app in appsToRemove)
                    {
                        progressHandler?.Report($"Removing IIS entry {i++}/{appsToRemove.Count}: {app.Path}");
                        site.Applications.Remove(app);
                    }

                    serverManager.CommitChanges();
                }
            }
        }

        public static void CeanupIISApps(List<IISApp> iisApps, CleanupParameters cleanupParameters, IProgress<string>? progressHandler = null)
        {
            StopIIS(progressHandler);

            if (cleanupParameters.CleanupIISApps)
            {
                DeleteIISApps(iisApps, progressHandler);
            }

            if (cleanupParameters.CleanupFiles)
            {
                CleanupAppFolders(iisApps, progressHandler);
            }

            if (cleanupParameters.CleanupAspTemp)
            {
                CleanupAspTempFolders(iisApps, progressHandler);
            }

            if (cleanupParameters.CleanupDatabases)
            {
                CleanupDatabases(iisApps, progressHandler);
            }

            StartIIS();
        }

        public static void CleanupAppFolders(List<IISApp> iisApps, IProgress<string>? progressHandler = null)
        {
            int i = 1;
            foreach (var iisApp in iisApps)
            {
                if (Directory.Exists(iisApp.Path))
                {
                    progressHandler?.Report($"Cleaning folders {i++}/{iisApps.Count}: {iisApp.Path}");
                    PrepareDirectoryForDeletion(iisApp.Path);
                    Directory.Delete(iisApp.Path, true);
                }
            }
        }

        public static void CleanupAspTempFolders(List<IISApp> iisApps, IProgress<string>? progressHandler = null)
        {
            int i = 1;
            foreach (var iisApp in iisApps)
            {
                if (Directory.Exists(iisApp.AspFilesTempPath))
                {
                    progressHandler?.Report($"Deleting ASP temp files: {i++}/{iisApps.Count} {iisApp.AspFilesTempPath}");
                    PrepareDirectoryForDeletion(iisApp.AspFilesTempPath);
                    Directory.Delete(iisApp.AspFilesTempPath, true);
                }
            }
        }

        public static void CleanupDatabases(List<IISApp> iisApps, IProgress<string>? progressHandler = null)
        {
            int i = 1;
            foreach (var iisApp in iisApps)
            {
                if (!string.IsNullOrEmpty(iisApp.DatabaseName))
                {
                    progressHandler?.Report($"Deleting SQL database {i++}/{iisApps.Count}: {iisApp.DatabaseName}");
                    using (var connection = new SqlConnection(iisApp.DatabaseConnectionString))
                    {
                        connection.Open();
                        var command = new SqlCommand($"DROP DATABASE [{iisApp.DatabaseName}];", connection);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static List<Microsoft.Web.Administration.Application> FindAppsToRemove(Site site, List<IISApp> iisApps)
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
