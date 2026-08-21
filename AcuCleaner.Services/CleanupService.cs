using AcuCleaner.Models;
using AcuCleaner.Services.IIS;

namespace AcuCleaner.Services
{
    public class CleanupService : IAppsCollection
    {
        public IEnumerable<ICleanupService> Cleaners { get; private set; }
        private IEnumerable<IISApp> _apps = new List<IISApp>();
        public CleanupParameters CleanupParameters { get; set; } = new CleanupParameters();
        public FilterParameters FilterParameters { get; set; } = new FilterParameters();

        public CleanupService(IEnumerable<ICleanupService> cleaners)
        {
            Cleaners = cleaners;
        }

        public IAppsCollection GetAll()
        {
            _apps = new List<IISApp>();

            foreach (var cleaner in Cleaners)
            {
                var appsForCleanup = cleaner?.GetAppsForCleanup(CleanupParameters) ?? Enumerable.Empty<IISApp>();
                if (appsForCleanup.Any())
                {
                    CleanupParameters.Apps = CleanupParameters.Apps.UnionBy(appsForCleanup, x => x.InstanceName);
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
            var iisService = Cleaners.OfType<IISCleaner>().FirstOrDefault();

            iisService?.Stop();

            foreach (var cleaner in Cleaners)
            {
                cleaner?.Cleanup(CleanupParameters);
            }
            ;

            iisService?.Start();
        }
    }
}
