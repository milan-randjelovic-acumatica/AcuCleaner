using IISCleanup.Models;

namespace IISCleanup.Services
{
    public interface IAppsCollection
    {
        IAppsCollection FilterByStatus(bool isActive, bool junk);
        IAppsCollection FilterByDate(DateTime cutoff);
        IAppsCollection OrderBy(Func<IISApp, object> keySelector);
        IAppsCollection AddRowNumbers();
        List<IISApp> ToList();
    }
}
