namespace AcuCleaner.Models
{
    public class FilterParameters
    {
        public bool ShowActiveApps { get; set; } = true;
        public bool ShowJunk { get; set; } = true;
        public bool ShowFiles { get; set; } = true;
        public bool ShowAspTempFiles { get; set; } = true;
        public bool ShowDatabases { get; set; } = true;
        public bool ShowRegistry { get; set; } = true;
        public bool ShowSnapshots { get; set; } = true;
        public bool ShowCustomizations { get; set; } = true;
        public bool ShowBackups { get; set; } = true;
        public DateTime CutoffDate { get; set; }
    }
}
