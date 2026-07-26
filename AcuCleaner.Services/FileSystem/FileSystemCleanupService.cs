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
            var result = new List<IISApp>();

            foreach (var rootFolder in cleanupParameters.FileSystemRootPaths)
            {
                var folders = Directory.GetDirectories(rootFolder).ExcludeStandardFolders();
                foreach (var folder in folders)
                {
                    string folderName = Path.GetFileName(folder).NormalizePath();

                    if (folderName != Constants.TemporaryAspFiles)
                    {

                        var existingApp = cleanupParameters.Apps.FirstOrDefault(app => app.InstanceName == folderName);
                        if (existingApp != null)
                        {
                            existingApp.Path = folder.NormalizePath();
                            existingApp.InstalledOn = existingApp.InstalledOn == DateTime.MinValue ? Directory.GetCreationTime(folder) : existingApp.InstalledOn;
                        }
                        else
                        {
                            result.Add(new IISApp
                            {
                                InstanceName = folderName,
                                DatabaseName = string.Empty,
                                DatabaseConnectionString = string.Empty,
                                Path = folder.NormalizePath(),
                                RootPath = rootFolder.NormalizePath(),
                                AspFilesTempPath = string.Empty,
                                RegistryPath = string.Empty,
                                Active = false,
                                InstalledOn = Directory.GetCreationTime(folder),
                            });
                        }
                    }
                    else
                    {
                        var parameters = new CleanupParameters()
                        {
                            Apps = new List<IISApp>()
                            {
                                new IISApp
                                {
                                     RootPath = folder.NormalizePath(),
                                     AspFilesTempPath = string.Empty,
                                     DatabaseConnectionString = string.Empty,
                                     DatabaseName = string.Empty,
                                     InstanceName = folderName,
                                     Path = string.Empty,
                                     RegistryPath = string.Empty,
                                     Active = false
                                }
                            }
                        };


                        var tmpASPApps = GetAppsForCleanup(parameters);
                        foreach (var tmpApp in tmpASPApps)
                        {
                            string tmpInstanceName = Path.GetFileName(tmpApp.Path).NormalizePath();

                            var existingApp = result.FirstOrDefault(app => app.InstanceName == tmpInstanceName);
                            if (existingApp != null)
                            {
                                existingApp.AspFilesTempPath = tmpApp.Path.NormalizePath();
                                existingApp.InstalledOn = existingApp.InstalledOn == DateTime.MinValue ? Directory.GetCreationTime(tmpApp.Path) : existingApp.InstalledOn;
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
                                    RegistryPath = string.Empty,
                                    Active = false,
                                    InstalledOn = Directory.GetCreationTime(tmpApp.Path),
                                });
                            }
                        }
                    }
                }
            }

            return result.DistinctBy(app => app.Path);
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
