using AcuCleaner.Models;

namespace AcuCleaner.Services
{
    public interface ICleanupService : IUtilityService
    {
        IEnumerable<IISApp> GetAllApps();
        IEnumerable<IISApp> GetAppsForCleanup(CleanupParameters cleanupParameters);
        void Cleanup(CleanupParameters cleanupParameters);
    }
}
