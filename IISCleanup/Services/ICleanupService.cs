using IISCleanup.Models;

namespace IISCleanup.Services
{
    public interface ICleanupService
    {
        IAppsCollection GetAllApps();
        void Ceanup(List<IISApp> iisApps, CleanupParameters cleanupParameters);
    }
}
