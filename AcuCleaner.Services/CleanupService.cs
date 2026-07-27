using AcuCleaner.Models;
using AcuCleaner.Services.Database;
using AcuCleaner.Services.FileSystem;
using AcuCleaner.Services.IIS;
using AcuCleaner.Services.Registry;

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

        public IAppsCollection FilterByStatus(bool isActive, bool junk)
        {
            _apps = _apps.Where(x => (isActive == true && x.Active == true) || (junk == true && x.Active == false));

            return this;
        }

        public IAppsCollection FilterByDate(DateTime cutoff)
        {
            _apps = _apps
                .Where(x => x.InstalledOn.Date <= cutoff)
                .OrderBy(x => x.InstalledOn);

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
