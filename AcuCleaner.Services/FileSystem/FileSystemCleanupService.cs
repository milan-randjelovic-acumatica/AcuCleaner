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
                        ProcessSpecialFolder(path, result, (app, normalizedPath) => app.AspTempFilesPath = normalizedPath);
                    }
                    else if (folderName == Constants.Snapshots)
                    {
                        ProcessSpecialFolder(path, result, (app, normalizedPath) => app.SnapshotPath = normalizedPath);
                    }
                    else if(folderName == Constants.Customization)
                    {
                        ProcessSpecialFolder(path, result, (app, normalizedPath) => app.CustomizationPath = normalizedPath);
                    }
                    else if(folderName == Constants.BackUp)
                    {
                        ProcessSpecialFolder( Path.Combine(path, Constants.Sites), result, (app, normalizedPath) => app.BackupPath = normalizedPath);
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
                                AspTempFilesPath = string.Empty,
                                CustomizationPath = string.Empty,
                                BackupPath = string.Empty,
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

        private void ProcessSpecialFolder(string path, Dictionary<string, IISApp> result, Action<IISApp, string> setPathAction)
        {
            var normalizedPath = path.NormalizePath();
            var subPaths = Directory.GetDirectories(path).ExcludeStandardFolders();

            foreach (var subPath in subPaths)
            {
                string instanceName = Path.GetFileName(subPath).NormalizePath();
                if (string.IsNullOrEmpty(instanceName))
                {
                    continue;
                }

                var normalizedSubPath = subPath.NormalizePath();

                if (result.TryGetValue(instanceName, out var existingApp))
                {
                    existingApp.InstanceName = existingApp.InstanceName != string.Empty ? existingApp.InstanceName : instanceName;
                    setPathAction(existingApp, normalizedSubPath);
                    if (existingApp.InstalledOn == DateTime.MinValue)
                    {
                        existingApp.InstalledOn = Directory.GetCreationTime(subPath);
                    }
                }
                else
                {
                    var newApp = new IISApp
                    {
                        InstanceName = instanceName,
                        DatabaseName = string.Empty,
                        DatabaseConnectionString = string.Empty,
                        Path = string.Empty,
                        RootPath = normalizedPath,
                        AspTempFilesPath = string.Empty,
                        SnapshotPath = string.Empty,
                        CustomizationPath = string.Empty,
                        BackupPath = string.Empty,
                        RegistryPath = string.Empty,
                        Active = false,
                        InstalledOn = Directory.GetCreationTime(subPath),
                    };
                    setPathAction(newApp, normalizedSubPath);
                    result[instanceName] = newApp;
                }
            }
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

            if(cleanupParameters.CleanupSnapshots)
            {
                CleanupSnapshotFolders(cleanupParameters);
            }

            if(cleanupParameters.CleanupCustomizations)
            {
                CleanupCustomizationFolders(cleanupParameters);
            }

            if(cleanupParameters.CleanupBackups)
            {
                CleanupBackupFolders(cleanupParameters);
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
                if (!string.IsNullOrEmpty(app.AspTempFilesPath) && Directory.Exists(app.AspTempFilesPath))
                {
                    _progressHandler?.Report($"Deleting ASP temp files: {i++}/{cleanupParameters.Apps.Count} {app.AspTempFilesPath}");
                    PrepareDirectoryForDeletion(app.AspTempFilesPath);
                    Directory.Delete(app.AspTempFilesPath, true);
                }
            }
        }

        public void CleanupSnapshotFolders(CleanupParameters cleanupParameters)
        {
            int i = 1;
            foreach (var app in cleanupParameters.Apps)
            {
                if (!string.IsNullOrEmpty(app.SnapshotPath) && Directory.Exists(app.SnapshotPath))
                {
                    _progressHandler?.Report($"Deleting snapshot files: {i++}/{cleanupParameters.Apps.Count} {app.SnapshotPath}");
                    PrepareDirectoryForDeletion(app.SnapshotPath);
                    Directory.Delete(app.SnapshotPath, true);
                }
            }
        }

        public void CleanupCustomizationFolders(CleanupParameters cleanupParameters)
        {
            int i = 1;
            foreach (var app in cleanupParameters.Apps)
            {
                if (!string.IsNullOrEmpty(app.CustomizationPath) && Directory.Exists(app.CustomizationPath))
                {
                    _progressHandler?.Report($"Deleting customization files: {i++}/{cleanupParameters.Apps.Count} {app.CustomizationPath}");
                    PrepareDirectoryForDeletion(app.CustomizationPath);
                    Directory.Delete(app.CustomizationPath, true);
                }
            }
        }

        public void CleanupBackupFolders(CleanupParameters cleanupParameters)
        {
            int i = 1;
            foreach (var app in cleanupParameters.Apps)
            {
                if (!string.IsNullOrEmpty(app.BackupPath) && Directory.Exists(app.BackupPath))
                {
                    _progressHandler?.Report($"Deleting backup files: {i++}/{cleanupParameters.Apps.Count} {app.BackupPath}");
                    PrepareDirectoryForDeletion(app.BackupPath);
                    Directory.Delete(app.BackupPath, true);
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
