using IISCleanup.Models;
using Microsoft.Win32;

namespace IISCleanup.Services
{
    public class RegistryCleanupService : ICleanupService
    {
        private readonly string _path = @"SOFTWARE\ACUMATICA ERP";

        public IEnumerable<IISApp> GetAllApps()
        {
            var result = new List<IISApp>();

            using (RegistryKey? baseKey = Registry.LocalMachine.OpenSubKey(_path))
            {
                if (baseKey != null)
                {
                    var subKeys = baseKey.GetSubKeyNames();
                    foreach (var subKey in subKeys)
                    {
                        result.Add(new IISApp
                        {
                            InstanceName = subKey,
                            RegistryPath = $@"{_path}\{subKey}",
                            AspFilesTempPath = string.Empty,
                            DatabaseConnectionString = string.Empty,
                            DatabaseName = string.Empty,
                            Path = string.Empty,
                            RootPath = string.Empty,
                            Active = true,
                        });
                    }
                }
            }

            return result;
        }

        public IEnumerable<IISApp> GetAppsForCleanup(CleanupParameters cleanupParameters)
        {
            var result = new List<IISApp>();
            var registryEnties = GetAllApps();
            foreach (var registryEntry in registryEnties ?? Enumerable.Empty<IISApp>())
            {
                var existingApp = cleanupParameters.Apps.FirstOrDefault(app => app.InstanceName == registryEntry.InstanceName);
                if (existingApp != null)
                {
                    existingApp.RegistryPath = registryEntry.RegistryPath;
                    result.Add(existingApp);
                }
                else
                {
                    result.Add(new IISApp
                    {
                        InstanceName = registryEntry.InstanceName,
                        DatabaseName = string.Empty,
                        DatabaseConnectionString = string.Empty,
                        Path = string.Empty,
                        RootPath = string.Empty,
                        AspFilesTempPath = string.Empty,
                        RegistryPath = registryEntry.RegistryPath,
                        Active = false,
                        InstalledOn = DateTime.MinValue,
                    });
                }
            }
            return result;
        }

        public void Cleanup(CleanupParameters cleanupParameters)
        {
            if (!cleanupParameters.CleanupRegistry)
            {
                return;
            }

            using (var baseKey = Registry.LocalMachine.OpenSubKey(_path, writable: true))
            {
                foreach (var app in cleanupParameters.Apps)
                {
                    if (!string.IsNullOrEmpty(app.RegistryPath))
                    {
                        var registryPath = app.RegistryPath.Replace('/', '\\').TrimEnd('\\');
                        baseKey?.DeleteSubKeyTree(app.InstanceName, throwOnMissingSubKey: true);
                    }
                }
            }
        }
    }
}
