using AcuCleaner.Models;

namespace AcuCleaner.Services.FileSystem
{
    public class WebConfigManager : IUtilityService
    {
        private readonly IProgress<string>? _progressHandler;

        public WebConfigManager()
        {
            
        }

        public WebConfigManager(IProgress<string> progressHandler)
        {
            _progressHandler = progressHandler;
        }

        public void EnableModernUISwitch(CleanupParameters cleanupParameters)
        {
            int i = 1;
            foreach (var app in cleanupParameters.Apps)
            {
                if (!string.IsNullOrEmpty(app.Path) && Directory.Exists(app.Path))
                {
                    var webConfigFilePath = Path.Combine(app.Path, Constants.WebConfig);
                    if (File.Exists(webConfigFilePath))
                    {
                        _progressHandler?.Report($"Enabling Modern UI switch {i++}/{cleanupParameters.Apps.Count()} for: {app.InstanceName}");

                        var doc = new System.Xml.XmlDocument();
                        doc.Load(webConfigFilePath);

                        var appSettings = doc.SelectSingleNode("//appSettings");
                        if (appSettings != null)
                        {
                            // Update existing key if present, otherwise add it
                            var existing = appSettings.SelectSingleNode("add[@key='EnableSiteMapSwitchUI']");
                            if (existing != null)
                            {
                                existing.Attributes!["value"]!.Value = "true";
                            }
                            else
                            {
                                var entry = doc.CreateElement("add");
                                entry.SetAttribute("key", "EnableSiteMapSwitchUI");
                                entry.SetAttribute("value", "true");
                                appSettings.AppendChild(entry);
                            }

                            doc.Save(webConfigFilePath);
                            _progressHandler?.Report($"Successfully enabled Modern UI switch for: {app.InstanceName}");
                        }
                    }
                }
            }
        }

        public void ExtendSessionTimeout(CleanupParameters cleanupParameters, int newTimeoutInMinutes = 24 * 60)
        {
            int i = 1;
            foreach (var app in cleanupParameters.Apps)
            {
                if (!string.IsNullOrEmpty(app.Path) && Directory.Exists(app.Path))
                {
                    var webConfigFilePath = Path.Combine(app.Path, Constants.WebConfig);
                    if (File.Exists(webConfigFilePath))
                    {
                        _progressHandler?.Report($"Extending session timeout {i++}/{cleanupParameters.Apps.Count()} for: {app.InstanceName}");
                        var doc = new System.Xml.XmlDocument();
                        doc.Load(webConfigFilePath);
                        var sessionStateNode = doc.SelectSingleNode("//sessionState");
                        if (sessionStateNode != null)
                        {
                            sessionStateNode.Attributes!["timeout"]!.Value = newTimeoutInMinutes.ToString();
                            doc.Save(webConfigFilePath);
                            _progressHandler?.Report($"Successfully extended session timeout for: {app.InstanceName}");
                        }
                    }
                }
            }
        }
    }
}
