namespace AcuCleaner.Models
{
    public class IISApp
    {
        public int Id { get; set; }
        public string InstanceName { get; set; } = string.Empty;
        public string InstanceUrl { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string RootPath { get; set; } = string.Empty;
        public string AspTempFilesPath { get; set; } = string.Empty;
        public string SnapshotPath { get; set; } = string.Empty;
        public string CustomizationPath { get; set; } = string.Empty;
        public string BackupPath { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string DatabaseConnectionString { get; set; } = string.Empty;
        public string DatabaseVersion { get; set; } = string.Empty;
        public string Database => $"{DatabaseName}{(string.IsNullOrEmpty(DatabaseVersion) ? string.Empty : $" ({DatabaseVersion})")}";
        public string RegistryPath { get; set; } = string.Empty;
        public bool Active { get; set; }
        public string Status => Active ? "Active" : "Inactive";
        public DateTime InstalledOn { get; set; } = DateTime.MinValue;
        public override string ToString()
        {
            return InstanceName;
        }
    }
}
