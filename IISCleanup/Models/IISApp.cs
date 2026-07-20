namespace IISCleanup.Models
{
    public class IISApp
    {
        public int Id { get; set; }
        public required string Instance { get; set; }
        public required string Path { get; set; }
        public required string RootPath { get; set; }
        public required string AspFilesTempPath { get; set; }
        public bool Active { get; set; }
        public DateTime InstalledOn { get; set; } = DateTime.MinValue;
        public override string ToString()
        {
            return Instance;
        }
    }
}
