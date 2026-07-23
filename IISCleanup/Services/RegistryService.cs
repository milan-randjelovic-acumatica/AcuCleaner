using IISCleanup.Models;
using Microsoft.Win32;

namespace IISCleanup.Services
{
    public class RegistryService
    {
        public IEnumerable<IISApp> GetRegistryEnties()
        {
            var result = new List<IISApp>();

            string path = @"SOFTWARE\ACUMATICA ERP";

            using (RegistryKey baseKey = Registry.LocalMachine.OpenSubKey(path)!)
            {
                if (baseKey != null)
                {
                    var subKeys = baseKey.GetSubKeyNames();
                    foreach (var subKey in subKeys)
                    {
                        result.Add(new IISApp
                        {
                            InstanceName = subKey,
                            RegistryPath = $@"{path}\{subKey}",
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
    }
}
