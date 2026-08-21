namespace AcuCleaner.Models
{
    public class IISApp
    {
        public int Id { get; set; }
        public required string InstanceName { get; set; }
        public required string Path { get; set; }
        public required string RootPath { get; set; }
        public required string AspTempFilesPath { get; set; }
        public required string SnapshotPath { get; set; }
        public required string CustomizationPath { get; set; }
        public required string BackupPath { get; set; }
        public required string DatabaseName { get; set; }
        public required string DatabaseConnectionString { get; set; }
        public required string RegistryPath { get; set; }
        public bool Active { get; set; }
        public string Status => Active ? "Active" : "Inactive";
        public DateTime InstalledOn { get; set; } = DateTime.MinValue;
        public override string ToString()
        {
            return InstanceName;
        }
    }
}
