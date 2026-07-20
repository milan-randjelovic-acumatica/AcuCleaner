namespace IISCleanup.Models
{
    public class CleanupParameters
    {
        public bool CleanupIISApps { get; set; }
        public bool CleanupFiles { get; set; }
        public bool CleanupAspTemp { get; set; }
    }
}
