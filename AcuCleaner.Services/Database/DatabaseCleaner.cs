using AcuCleaner.Models;
using Microsoft.Data.SqlClient;

namespace AcuCleaner.Services.Database
{
    public class DatabaseCleaner : ICleanupService
    {
        private readonly IProgress<string>? _progressHandler;

        public DatabaseCleaner()
        {
        }

        public DatabaseCleaner(IProgress<string> progressHandler)
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

                    var command = new SqlCommand("SELECT name, create_date FROM sys.databases ORDER BY name;", connection);

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
                                AspTempFilesPath = string.Empty,
                                CustomizationPath = string.Empty,
                                SnapshotPath = string.Empty,
                                BackupPath = string.Empty,
                                RegistryPath = string.Empty,
                                Active = false,
                                InstalledOn = reader.GetDateTime(1),
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
                    existingApp.InstanceName = existingApp.InstanceName != string.Empty ? existingApp.InstanceName : sqlDatabase.DatabaseName;
                    existingApp.DatabaseName = sqlDatabase.DatabaseName;
                    existingApp.DatabaseConnectionString = sqlDatabase.DatabaseConnectionString;
                    existingApp.InstalledOn = existingApp.InstalledOn == DateTime.MinValue ? sqlDatabase.InstalledOn : existingApp.InstalledOn;
                    result.Add(existingApp);
                }
                else
                {
                    result.Add(new IISApp
                    {
                        InstanceName = sqlDatabase.DatabaseName,
                        DatabaseName = sqlDatabase.DatabaseName,
                        DatabaseConnectionString = sqlDatabase.DatabaseConnectionString,
                        Path = string.Empty,
                        RootPath = string.Empty,
                        AspTempFilesPath = string.Empty,
                        SnapshotPath = string.Empty,
                        CustomizationPath = string.Empty,
                        BackupPath = string.Empty,
                        RegistryPath = string.Empty,
                        Active = false,
                        InstalledOn = sqlDatabase.InstalledOn,
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
                    _progressHandler?.Report($"Deleting SQL database {i++}/{cleanupParameters.Apps.Count()}: {iisApp.DatabaseName}");
                    using (var connection = new SqlConnection(iisApp.DatabaseConnectionString))
                    {
                        connection.Open();
                        var command = new SqlCommand($"DROP DATABASE [{iisApp.DatabaseName}];", connection);
                        command.ExecuteNonQuery();
                    }
                    _progressHandler?.Report($"Successfully deleted SQL database: {iisApp.DatabaseName}");
                }
            }
        }

        public void ResetPassword(CleanupParameters cleanupParameters, string newPassword = "123")
        {
            int i = 1;
            foreach (var iisApp in cleanupParameters.Apps)
            {
                if (!string.IsNullOrEmpty(iisApp.DatabaseName))
                {
                    _progressHandler?.Report($"Resetting password for SQL database {i++}/{cleanupParameters.Apps.Count()}: {iisApp.DatabaseName}");
                    using (var connection = new SqlConnection($"{iisApp.DatabaseConnectionString};Initial Catalog={iisApp.DatabaseName}"))
                    {
                        connection.Open();
                        var command = new SqlCommand(
                            $"update PreferencesSecurity set PasswordMinLength = 3, PasswordComplexity = 0, PasswordSecurityType = 0;" +
                            $"update users set password = '{newPassword}', PasswordChangeOnNextLogin =0, Guest = 0, FailedPasswordAttemptCount = 0, PasswordNeverExpires = 1,LockedOutDate=NULL,LastLockedOutDate=NULL,FailedPasswordAnswerAttemptCount=0,FailedPasswordAttemptWindowStart=NULL,PasswordRecoveryExpirationDate=NULL,IsApproved = 1 where source = 0;" +
                            $"UPDATE AUSchedule SET IsActive =  0;" +
                            $"update PushNotificationsHook set Active = 0", connection);
                        command.ExecuteNonQuery();
                    }
                    _progressHandler?.Report($"Password reset successfully for SQL database: {iisApp.DatabaseName}");
                }
            }
        }
    }
}
