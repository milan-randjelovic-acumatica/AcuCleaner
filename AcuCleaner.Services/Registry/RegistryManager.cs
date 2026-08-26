using AcuCleaner.Models;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AcuCleaner.Services.Registry
{
    public class RegistryManager : ICleanupService, IUtilityService
    {
        private readonly string _path = @"SOFTWARE\ACUMATICA ERP";
        private readonly IProgress<string>? _progressHandler;

        public RegistryManager(IProgress<string> progressHandler)
        {
            _progressHandler = progressHandler;
        }

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
                            AspTempFilesPath = string.Empty,
                            DatabaseConnectionString = string.Empty,
                            DatabaseName = string.Empty,
                            Path = string.Empty,
                            SnapshotPath = string.Empty,
                            CustomizationPath = string.Empty,
                            BackupPath = string.Empty,
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
                        AspTempFilesPath = string.Empty,
                        RegistryPath = registryEntry.RegistryPath,
                        SnapshotPath = string.Empty,
                        CustomizationPath = string.Empty,
                        BackupPath = string.Empty,
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
                int i = 1;
                foreach (var app in cleanupParameters.Apps)
                {
                    if (!string.IsNullOrEmpty(app.RegistryPath))
                    {
                        _progressHandler?.Report($"Deleting registry entry {i++}/{cleanupParameters.Apps.Count()}: {app.RegistryPath}");
                        var registryPath = app.RegistryPath.Replace('/', '\\').TrimEnd('\\');
                        baseKey?.DeleteSubKeyTree(app.InstanceName, throwOnMissingSubKey: true);
                        _progressHandler?.Report($"Successfully deleted registry entry: {app.RegistryPath}");
                    }
                }
            }
        }

        public void OpenInRegedit(string registryPath)
        {
            try
            {
                if (string.IsNullOrEmpty(registryPath))
                {
                    _progressHandler?.Report("Registry path not found.");
                    return;
                }

                var normalizedPath = registryPath.Replace('/', '\\').TrimEnd('\\');
                var lastKeyPath = $@"Computer\HKEY_LOCAL_MACHINE\{normalizedPath}";

                using (var regeditKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Applets\Regedit"))
                {
                    regeditKey?.SetValue("LastKey", lastKeyPath, RegistryValueKind.String);
                }

                Process.Start(new ProcessStartInfo("regedit.exe") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                _progressHandler?.Report($"Error opening registry path: {ex.Message}");
            }
        }
    }
}
