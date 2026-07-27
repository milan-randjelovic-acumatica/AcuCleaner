namespace AcuCleaner.Models
{
    public class CleanupParameters
    {
        public bool CleanupIISApps { get; set; } = true;
        public bool CleanupFiles { get; set; } = true;
        public bool CleanupAspTemp { get; set; } = true;
        public bool CleanupDatabases { get; set; } = true;
        public bool CleanupRegistry { get; set; } = true;
        public bool CleanupSnapshots { get; set; } = true;
        public List<IISApp> Apps { get; set; } = new List<IISApp>();
        public List<string> FileSystemRootPaths => Apps.Select(x => x.RootPath).Distinct().ToList();
    }
}
