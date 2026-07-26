using AcuCleaner.Models;
using Microsoft.Data.SqlClient;

namespace AcuCleaner.Services
{
    public class DatabaseCleanupService : ICleanupService
    {
        private readonly IProgress<string>? _progressHandler;

        public DatabaseCleanupService()
        {
        }

        public DatabaseCleanupService(IProgress<string> progressHandler)
        {
            _progressHandler = progressHandler;
        }

        public IEnumerable<IISApp> GetAllApps()
        {
            var result = new List<IISApp>();

            foreach (var connectionString in Constants.ConnectionStrings)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand("SELECT name FROM sys.databases ORDER BY name;", connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader.GetString(0));
                            result.Add(new IISApp
                            {
                                InstanceName = string.Empty,
                                DatabaseName = reader.GetString(0),
                                DatabaseConnectionString = connectionString,
                                Path = string.Empty,
                                RootPath = string.Empty,
                                AspFilesTempPath = string.Empty,
                                RegistryPath = string.Empty,
                                Active = false,
                                InstalledOn = DateTime.MinValue,
                            });
                        }
                    }
                }
            }

            return result;
        }

        public IEnumerable<IISApp> GetAppsForCleanup(CleanupParameters cleanupParameters)
        {
            var result = new List<IISApp>();

            var sqlDatabases = GetAllApps()
                              .ExcludeStandardSqlDatabases();

            foreach (var sqlDatabase in sqlDatabases)
            {
                var existingApp = cleanupParameters.Apps.FirstOrDefault(app => app.InstanceName == sqlDatabase.DatabaseName);
                if (existingApp != null)
                {
                    existingApp.DatabaseName = sqlDatabase.DatabaseName;
                    result.Add(existingApp);
                }
                else
                {
                    result.Add(new IISApp
                    {
                        InstanceName = string.Empty,
                        DatabaseName = sqlDatabase.DatabaseName,
                        DatabaseConnectionString = sqlDatabase.DatabaseConnectionString,
                        Path = string.Empty,
                        RootPath = string.Empty,
                        AspFilesTempPath = string.Empty,
                        RegistryPath = string.Empty,
                        Active = false,
                        InstalledOn = DateTime.MinValue,
                    });
                }
            }

            return result;
        }

        public void Cleanup(CleanupParameters cleanupParameters)
        {
            if (!cleanupParameters.CleanupDatabases)
            {
                return;
            }

            int i = 1;
            foreach (var iisApp in cleanupParameters.Apps)
            {
                if (!string.IsNullOrEmpty(iisApp.DatabaseName))
                {
                    _progressHandler?.Report($"Deleting SQL database {i++}/{cleanupParameters.Apps.Count}: {iisApp.DatabaseName}");
                    using (var connection = new SqlConnection(iisApp.DatabaseConnectionString))
                    {
                        connection.Open();
                        var command = new SqlCommand($"DROP DATABASE [{iisApp.DatabaseName}];", connection);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
