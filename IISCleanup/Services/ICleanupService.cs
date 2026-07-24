using IISCleanup.Models;

namespace IISCleanup.Services
{
    public interface ICleanupService
    {
        IEnumerable<IISApp> GetAllApps();
        IEnumerable<IISApp> GetAppsForCleanup(CleanupParameters cleanupParameters);
        void Cleanup(CleanupParameters cleanupParameters);
    }
}
