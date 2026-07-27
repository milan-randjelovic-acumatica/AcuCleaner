using AcuCleaner.Models;
using AcuCleaner.Services.Database;
using AcuCleaner.Services.FileSystem;
using AcuCleaner.Services.IIS;
using AcuCleaner.Services.Registry;
using System.Net.WebSockets;

namespace AcuCleaner.Services
{
    public class CleanupService : IAppsCollection
    {
        private readonly AcuCleanerService? _iisService;
        private readonly RegistryCleanupService? _registryService;
        private readonly DatabaseCleanupService? _databaseCleanupService;
        private readonly FileSystemCleanupService? _fileSystemCleanupService;
        private IEnumerable<IISApp> _apps = new List<IISApp>();

        public CleanupService(AcuCleanerService iISService, FileSystemCleanupService fileSystemCleanupService, DatabaseCleanupService databaseCleanupService, RegistryCleanupService registryService)
        {
            _iisService = iISService;
            _fileSystemCleanupService = fileSystemCleanupService;
            _databaseCleanupService = databaseCleanupService;
            _registryService = registryService;
        }

        public CleanupService(AcuCleanerService iISService, FileSystemCleanupService fileSystemCleanupService, DatabaseCleanupService databaseCleanupService, RegistryCleanupService registryService, IProgress<string>? progressHandler)
        {
            _iisService = iISService;
            _fileSystemCleanupService = fileSystemCleanupService;
            _databaseCleanupService = databaseCleanupService;
            _registryService = registryService;
        }

        public IAppsCollection GetAll(CleanupParameters cleanupParameters)
        {
            cleanupParameters.Apps = _iisService?.GetAllApps()?.ToList() ?? new List<IISApp>();

            var iisJunk = _fileSystemCleanupService?.GetAppsForCleanup(cleanupParameters) ?? Enumerable.Empty<IISApp>();

            cleanupParameters.Apps = cleanupParameters.Apps
                .UnionBy(iisJunk, x => x.InstanceName)
                .ToList();

            var sqlDatabases = _databaseCleanupService?.GetAppsForCleanup(cleanupParameters) ?? Enumerable.Empty<IISApp>();
            cleanupParameters.Apps = cleanupParameters.Apps
                .UnionBy(sqlDatabases, x => x.InstanceName)
                .ToList();

            var registryEnties = _registryService?.GetAppsForCleanup(cleanupParameters) ?? Enumerable.Empty<IISApp>();
            cleanupParameters.Apps = cleanupParameters.Apps
               .UnionBy(registryEnties, x => x.InstanceName)
               .ToList();

            _apps = cleanupParameters.Apps;

            return this;
        }

        public IAppsCollection Filter(FilterParameters filterParameters)
        {
            var apps = _apps
                .Where(x => (filterParameters.ShowActiveApps && x.Active) || (filterParameters.ShowJunk && !x.Active))
                .Where(x => x.InstalledOn.Date <= filterParameters.CutoffDate);

            var result = new List<IISApp>();
            foreach (var app in apps)
            {
                if (
                    filterParameters.ShowDatabases && !string.IsNullOrEmpty(app.DatabaseName) ||
                    filterParameters.ShowFiles && !string.IsNullOrEmpty(app.Path) ||
                    filterParameters.ShowAspTempFiles && !string.IsNullOrEmpty(app.AspTempFilesPath) ||
                    filterParameters.ShowSnapshots && !string.IsNullOrEmpty(app.SnapshotPath) ||
                    filterParameters.ShowCustomizations && !string.IsNullOrEmpty(app.CustomizationPath) ||
                    filterParameters.ShowBackups && !string.IsNullOrEmpty(app.BackupPath) ||
                    filterParameters.ShowRegistry && !string.IsNullOrEmpty(app.RegistryPath))
                {
                    result.Add(app);
                }
            }
            
            _apps = result;

            return this;
        }

        public IAppsCollection OrderBy(Func<IISApp, object> keySelector)
        {
            _apps = _apps.OrderBy(keySelector);
            return this;
        }

        public IAppsCollection AddRowNumbers()
        {
            for (int i = 0; i < _apps.Count(); i++)
            {
                _apps.ElementAt(i).Id = i + 1;
            }

            return this;
        }

        public List<IISApp> ToList()
        {
            return _apps.ToList();
        }

        public void Cleanup(CleanupParameters cleanupParameters)
        {
            _iisService?.Stop();

            _iisService?.Cleanup(cleanupParameters);

            _fileSystemCleanupService?.Cleanup(cleanupParameters);

            _databaseCleanupService?.Cleanup(cleanupParameters);

            _registryService?.Cleanup(cleanupParameters);

            _iisService?.Start();
        }
    }
}
