using AcuCleaner.Models;

namespace AcuCleaner
{
    internal static class Extensions
    {
        public static IEnumerable<IISApp> ExcludeStandardFolders(this IEnumerable<IISApp> iisApps)
        {
            var EXCLUDED_FOLDERS = new List<string>
            {
                "NodeJS",
                "NpmCache",
                "Snapshots",
                "Backup",
                "Logs",
                "Files",
                "Data",
                "Database",
                "Portal",
                "Report Designer",
                "HelpAndTraining",
                "DeviceHub",
                "Licenses",
                "Sources",
                "ScreenBasedAPIWrapper",
                "UnitTesting",
                "NodeTemp",
                "BackUp",
                "Customization",
                "TemporaryAspFiles"
            };

            iisApps = iisApps
                .Where(app => !EXCLUDED_FOLDERS.Contains(GetCurrentFolder(app.Path), StringComparer.OrdinalIgnoreCase));

            return iisApps;
        }

        public static IEnumerable<IISApp> ExcludeStandardSqlDatabases(this IEnumerable<IISApp> iisApps)
        {
            var EXCLUDED_INSTANCES = new List<string>
            {
                "master",
                "model",
                "msdb",
                "tempdb",
            };

            iisApps = iisApps
                .Where(app => !EXCLUDED_INSTANCES.Contains(app.DatabaseName, StringComparer.OrdinalIgnoreCase));

            return iisApps;
        }

        public static string GetCurrentFolder(string path)
        {
            return path.Split("\\").Last().Replace("\\", "");
        }

        public static string NormalizePath(this string path)
        {
            return path.Trim('\\').Trim('/');
        }
    }
}
