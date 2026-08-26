using AcuCleaner.Models;
using AcuCleaner.Services.Database;
using AcuCleaner.Services.FileSystem;
using AcuCleaner.Services.IIS;
using AcuCleaner.Services.Registry;

namespace AcuCleaner.Services
{
    public class AcuManager : IAppsCollection
    {
        private IEnumerable<ICleanupService> _utilityServices { get; set; }
        private IEnumerable<IISApp> _apps = new List<IISApp>();
        public CleanupParameters CleanupParameters { get; set; } = new CleanupParameters();
        public FilterParameters FilterParameters { get; set; } = new FilterParameters();
        public WebConfigManager WebConfigManager { get; private set; }
        public IISManager IISManager { get; private set; }
        public DatabaseManager DatabaseManager { get; private set; }
        public FileSystemManager FileSystemManager { get; private set; }
        public RegistryManager RegistryManager { get; private set; }

        public AcuManager(IProgress<string> progressHandler)
        {
            IISManager = new IISManager(progressHandler);
            DatabaseManager = new DatabaseManager(progressHandler);
            FileSystemManager = new FileSystemManager(progressHandler);
            RegistryManager = new RegistryManager(progressHandler);
            _utilityServices = new List<ICleanupService>()
            {
                IISManager,
                FileSystemManager,
                DatabaseManager,
                RegistryManager
            };

            WebConfigManager = new WebConfigManager(progressHandler);
        }

        public IAppsCollection GetAllInstances()
        {
            _apps = new List<IISApp>();

            foreach (var utilityService in _utilityServices)
            {
                var appsForCleanup = utilityService?.GetAppsForCleanup(CleanupParameters) ?? Enumerable.Empty<IISApp>();
                if (appsForCleanup.Any())
                {
                    CleanupParameters.Apps = CleanupParameters.Apps
                        .Concat(appsForCleanup)
                        .GroupBy(x => x.InstanceName)
                        .Select(g => g.First());
                }
            }

            _apps = CleanupParameters.Apps;

            return this;
        }

        public IAppsCollection Filter()
        {
            var apps = _apps
                .Where(x => (FilterParameters.ShowActiveApps && x.Active) || (FilterParameters.ShowJunk && !x.Active))
                .Where(x => x.InstalledOn.Date <= FilterParameters.CutoffDate);

            var result = new List<IISApp>();
            foreach (var app in apps)
            {
                if (
                    FilterParameters.ShowDatabases && !string.IsNullOrEmpty(app.DatabaseName) ||
                    FilterParameters.ShowFiles && !string.IsNullOrEmpty(app.Path) ||
                    FilterParameters.ShowAspTempFiles && !string.IsNullOrEmpty(app.AspTempFilesPath) ||
                    FilterParameters.ShowSnapshots && !string.IsNullOrEmpty(app.SnapshotPath) ||
                    FilterParameters.ShowCustomizations && !string.IsNullOrEmpty(app.CustomizationPath) ||
                    FilterParameters.ShowBackups && !string.IsNullOrEmpty(app.BackupPath) ||
                    FilterParameters.ShowRegistry && !string.IsNullOrEmpty(app.RegistryPath))
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

        public void Cleanup()
        {
            var iisService = _utilityServices.OfType<IISManager>().FirstOrDefault();

            iisService?.Stop();

            foreach (var utilityService in _utilityServices)
            {
                utilityService?.Cleanup(CleanupParameters);
            }
            ;

            iisService?.Start();
        }
    }
}
