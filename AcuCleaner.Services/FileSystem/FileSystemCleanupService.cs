using AcuCleaner.Models;
using System.Security.AccessControl;
using System.Security.Principal;

namespace AcuCleaner.Services.FileSystem
{
    public class FileSystemCleanupService : ICleanupService
    {
        private readonly IProgress<string>? _progressHandler;

        public FileSystemCleanupService()
        {
        }

        public FileSystemCleanupService(IProgress<string> progressHandler)
        {
            _progressHandler = progressHandler;
        }

        public IEnumerable<IISApp> GetAllApps()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IISApp> GetAppsForCleanup(CleanupParameters cleanupParameters)
        {
            var result = new Dictionary<string, IISApp>(StringComparer.OrdinalIgnoreCase);

            // Pre-populate dictionary with existing apps for O(1) lookups
            foreach (var app in cleanupParameters.Apps)
            {
                if (!string.IsNullOrEmpty(app.InstanceName) && !result.ContainsKey(app.InstanceName))
                {
                    result[app.InstanceName] = app;
                }
            }

            foreach (var rootFolder in cleanupParameters.FileSystemRootPaths)
            {
                var normalizedRootFolder = rootFolder.NormalizePath();
                var paths = Directory.GetDirectories(rootFolder).ExcludeStandardFolders();

                foreach (var path in paths)
                {
                    string folderName = Path.GetFileName(path).NormalizePath();
                    if (string.IsNullOrEmpty(folderName))
                    {
                        continue;
                    }

                    if (folderName == Constants.TemporaryAspFiles)
                    {
                        // Process temporary ASP files directly without recursion
                        var normalizedPath = path.NormalizePath();
                        var aspTempPaths = Directory.GetDirectories(path).ExcludeStandardFolders();

                        foreach (var tmpPath in aspTempPaths)
                        {
                            string tmpInstanceName = Path.GetFileName(tmpPath).NormalizePath();
                            if (string.IsNullOrEmpty(tmpInstanceName))
                            {
                                continue;
                            }

                            var normalizedTmpPath = tmpPath.NormalizePath();

                            if (result.TryGetValue(tmpInstanceName, out var existingApp))
                            {
                                existingApp.InstanceName = existingApp.InstanceName != string.Empty ? existingApp.InstanceName : tmpInstanceName;
                                existingApp.AspFilesTempPath = normalizedTmpPath;
                                if (existingApp.InstalledOn == DateTime.MinValue)
                                {
                                    existingApp.InstalledOn = Directory.GetCreationTime(tmpPath);
                                }
                            }
                            else
                            {
                                result[tmpInstanceName] = new IISApp
                                {
                                    InstanceName = tmpInstanceName,
                                    DatabaseName = string.Empty,
                                    DatabaseConnectionString = string.Empty,
                                    Path = string.Empty,
                                    RootPath = normalizedPath,
                                    AspFilesTempPath = normalizedTmpPath,
                                    SnapshotPath = string.Empty,
                                    RegistryPath = string.Empty,
                                    Active = false,
                                    InstalledOn = Directory.GetCreationTime(tmpPath),
                                };
                            }
                        }
                    }
                    else if (folderName == Constants.Snapshots)
                    {
                        // Process snapshot files directly without recursion
                        var normalizedPath = path.NormalizePath();
                        var aspTempPaths = Directory.GetDirectories(path).ExcludeStandardFolders();

                        foreach (var tmpPath in aspTempPaths)
                        {
                            string tmpInstanceName = Path.GetFileName(tmpPath).NormalizePath();
                            if (string.IsNullOrEmpty(tmpInstanceName))
                            {
                                continue;
                            }

                            var normalizedTmpPath = tmpPath.NormalizePath();

                            if (result.TryGetValue(tmpInstanceName, out var existingApp))
                            {
                                existingApp.InstanceName = existingApp.InstanceName != string.Empty ? existingApp.InstanceName : tmpInstanceName;
                                existingApp.SnapshotPath = normalizedTmpPath;
                                if (existingApp.InstalledOn == DateTime.MinValue)
                                {
                                    existingApp.InstalledOn = Directory.GetCreationTime(tmpPath);
                                }
                            }
                            else
                            {
                                result[tmpInstanceName] = new IISApp
                                {
                                    InstanceName = tmpInstanceName,
                                    DatabaseName = string.Empty,
                                    DatabaseConnectionString = string.Empty,
                                    Path = string.Empty,
                                    RootPath = normalizedPath,
                                    AspFilesTempPath = string.Empty,
                                    SnapshotPath = normalizedTmpPath,
                                    RegistryPath = string.Empty,
                                    Active = false,
                                    InstalledOn = Directory.GetCreationTime(tmpPath),
                                };
                            }
                        }
                    }
                    else
                    {
                        var normalizedPath = path.NormalizePath();

                        if (result.TryGetValue(folderName, out var existingApp))
                        {
                            existingApp.InstanceName = existingApp.InstanceName != string.Empty ? existingApp.InstanceName : folderName;
                            existingApp.Path = normalizedPath;
                            if (existingApp.InstalledOn == DateTime.MinValue)
                            {
                                existingApp.InstalledOn = Directory.GetCreationTime(path);
                            }
                        }
                        else
                        {
                            result[folderName] = new IISApp
                            {
                                InstanceName = folderName,
                                DatabaseName = string.Empty,
                                DatabaseConnectionString = string.Empty,
                                Path = normalizedPath,
                                RootPath = normalizedRootFolder,
                                AspFilesTempPath = string.Empty,
                                SnapshotPath = string.Empty,
                                RegistryPath = string.Empty,
                                Active = false,
                                InstalledOn = Directory.GetCreationTime(path),
                            };
                        }


                    }
                }
            }

            return result.Values;
        }

        public void Cleanup(CleanupParameters cleanupParameters)
        {
            if (cleanupParameters.CleanupFiles)
            {
                CleanupAppFolders(cleanupParameters);
            }

            if (cleanupParameters.CleanupAspTemp)
            {
                CleanupAspTempFolders(cleanupParameters);
            }
        }

        public void CleanupAppFolders(CleanupParameters cleanupParameters)
        {
            int i = 1;
            foreach (var app in cleanupParameters.Apps)
            {
                if (!string.IsNullOrEmpty(app.Path) && Directory.Exists(app.Path))
                {
                    _progressHandler?.Report($"Cleaning folders {i++}/{cleanupParameters.Apps.Count}: {app.Path}");
                    PrepareDirectoryForDeletion(app.Path);
                    Directory.Delete(app.Path, true);
                }
            }
        }

        public void CleanupAspTempFolders(CleanupParameters cleanupParameters)
        {
            int i = 1;
            foreach (var app in cleanupParameters.Apps)
            {
                if (!string.IsNullOrEmpty(app.AspFilesTempPath) && Directory.Exists(app.AspFilesTempPath))
                {
                    _progressHandler?.Report($"Deleting ASP temp files: {i++}/{cleanupParameters.Apps.Count} {app.AspFilesTempPath}");
                    PrepareDirectoryForDeletion(app.AspFilesTempPath);
                    Directory.Delete(app.AspFilesTempPath, true);
                }
            }
        }

        public void PrepareDirectoryForDeletion(string path)
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public void GrantFullControl(string path)
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
