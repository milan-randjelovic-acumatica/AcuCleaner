namespace AcuCleaner.Models
{
    public interface IAppsCollection
    {
        IAppsCollection Filter(FilterParameters filterParameters);
        IAppsCollection OrderBy(Func<IISApp, object> keySelector);
        IAppsCollection AddRowNumbers();
        List<IISApp> ToList();
    }
}
