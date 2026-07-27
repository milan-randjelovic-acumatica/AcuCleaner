using AcuCleaner.Models;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace AcuCleaner.Services.Registry
{
    public class RegistryCleanupService : ICleanupService
    {
        private readonly string _path = @"SOFTWARE\ACUMATICA ERP";

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        private static extern int RegQueryInfoKey(
            SafeHandle hKey,
            IntPtr lpClass,
            IntPtr lpcbClass,
            IntPtr lpReserved,
            IntPtr lpcSubKeys,
            IntPtr lpcbMaxSubKeyLen,
            IntPtr lpcbMaxClassLen,
            IntPtr lpcValues,
            IntPtr lpcbMaxValueNameLen,
            IntPtr lpcbMaxValueLen,
            IntPtr lpcbSecurityDescriptor,
            out long lpftLastWriteTime);

        private static DateTime GetSubKeyLastWriteTime(RegistryKey key)
        {
            int result = RegQueryInfoKey(
                key.Handle,
                IntPtr.Zero, IntPtr.Zero, IntPtr.Zero,
                IntPtr.Zero, IntPtr.Zero, IntPtr.Zero,
                IntPtr.Zero, IntPtr.Zero, IntPtr.Zero,
                IntPtr.Zero,
                out long lastWriteTime);

            return result == 0
                ? DateTime.FromFileTimeUtc(lastWriteTime).ToLocalTime()
                : DateTime.MinValue;
        }

        public IEnumerable<IISApp> GetAllApps()
        {
            var result = new List<IISApp>();

            using (RegistryKey? baseKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(_path))
            {
                if (baseKey != null)
                {
                    var subKeys = baseKey.GetSubKeyNames();
                    foreach (var subKey in subKeys)
                    {
                        using RegistryKey? subKeyEntry = baseKey.OpenSubKey(subKey);
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
                            InstalledOn = subKeyEntry != null ? GetSubKeyLastWriteTime(subKeyEntry) : DateTime.MinValue,
                        });
                    }
                }
            }

            return result;
        }

        public IEnumerable<IISApp> GetAppsForCleanup(CleanupParameters cleanupParameters)
        {
            var result = new List<IISApp>();
            var registryEnties = GetAllApps() ?? Enumerable.Empty<IISApp>();
            foreach (var registryEntry in registryEnties)
            {
                var existingApp = cleanupParameters.Apps.FirstOrDefault(app => app.InstanceName == registryEntry.InstanceName);
                if (existingApp != null)
                {
                    existingApp.InstanceName = existingApp.InstanceName != string.Empty ? existingApp.InstanceName : registryEntry.InstanceName;
                    existingApp.RegistryPath = registryEntry.RegistryPath;
                    existingApp.InstalledOn = existingApp.InstalledOn == DateTime.MinValue ? registryEntry.InstalledOn : existingApp.InstalledOn;
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
                        InstalledOn = registryEntry.InstalledOn,
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

            using (var baseKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(_path, writable: true))
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
