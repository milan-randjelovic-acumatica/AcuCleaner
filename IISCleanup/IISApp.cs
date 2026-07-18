namespace IISCleanup
{
    public class IISApp
    {
        public required string Instance { get; set; }
        public required string Path { get; set; }
        public required string RootPath { get; set; }
        public bool Junk { get; set; }
        public DateTime InstalledOn { get; set; } = DateTime.MinValue;
        public override string ToString()
        {
            return Instance;
        }
    }
}
