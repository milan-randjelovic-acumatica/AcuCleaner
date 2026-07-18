using Microsoft.Web.Administration;

namespace IISCleanup
{
    public partial class Form : System.Windows.Forms.Form
    {
        private List<string> EXCLUDED_FOLDERS = new List<string> { "bin", "obj", ".vs", ".git" };

        private List<IISApp> _allApps = new List<IISApp>();

        public Form()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            RefreshAppList();
        }

        private void RefreshAppList()
        {
            var iisApps = GetIISApps();
            var iisJunk = GetIISJunk(iisApps);
            _allApps = iisApps.Union(iisJunk).ToList();

            iISAppBindingSource1.DataSource = FilterByDate(_allApps, dateTimePicker.Value.Date);
            iISAppBindingSource1.ResetBindings(false);
        }

        private List<IISApp> GetIISApps()
        {
            var result = new List<IISApp>();
            using (ServerManager serverManager = new ServerManager())
            {
                foreach (var site in serverManager.Sites)
                {
                    foreach (var app in site.Applications)
                    {
                        if (app.Path == "/")
                        {
                            continue;
                        }

                        var physicalPath = app.VirtualDirectories.First().PhysicalPath;
                        var rootPath = new DirectoryInfo(physicalPath).Parent?.FullName ?? string.Empty;

                        if (Directory.GetFiles(rootPath).Select(x => x.Split("\\").Last()).Contains("Pure.sln"))
                        {
                            continue;
                        }

                        result.Add(new IISApp
                        {
                            Instance = app.Path.Replace("/", ""),
                            Path = physicalPath,
                            RootPath = rootPath,
                            Junk = false,
                            InstalledOn = Directory.GetCreationTime(physicalPath),
                        });
                    }
                }
            }

            return result;
        }

        private List<IISApp> GetIISJunk(List<IISApp> iisApps)
        {
            var result = new List<IISApp>();
            var iisRootPaths = iisApps.Select(x => x.RootPath).Distinct().ToList();
            foreach (var iisRootPath in iisRootPaths)
            {
                var folders = Directory.GetDirectories(iisRootPath);
                foreach (var folder in folders)
                {
                    result.Add(new IISApp
                    {
                        Instance = Path.GetFileName(folder),
                        Path = folder,
                        RootPath = iisRootPath,
                        Junk = true,
                        InstalledOn = Directory.GetCreationTime(folder),
                    });
                }
            }
            return result;
        }

        private List<IISApp> FilterByDate(List<IISApp> iisApps, DateTime cutoff)
        {
            var result = iisApps
                .Where(x => x.InstalledOn.Date <= cutoff)
                .OrderBy(x => x.InstalledOn)
                .ToList();

            return result;
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            RefreshAppList();
        }

        private void buttonDeleteSelected_Click(object sender, EventArgs e)
        {

        }

        private void buttonDeleteAll_Click(object sender, EventArgs e)
        {

        }
    }
}
