namespace AcuCleaner.Models
{
    public class Constants
    {
        public const string TemporaryAspFiles = "TemporaryAspFiles";
        public const string Snapshots = "Snapshots";

        public static readonly List<string> ConnectionStrings = new List<string>()
        {
            @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;TrustServerCertificate=True;",
            @"Server=localhost;Integrated Security=true;TrustServerCertificate=True;",
        };
    }
}
