using AcuCleaner.Models;

namespace AcuCleaner.Forms
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            dataGridView = new DataGridView();
            Id = new DataGridViewTextBoxColumn();
            Instance = new DataGridViewTextBoxColumn();
            Status = new DataGridViewTextBoxColumn();
            InstalledOn = new DataGridViewTextBoxColumn();
            Database = new DataGridViewTextBoxColumn();
            FilesPath = new DataGridViewTextBoxColumn();
            AspTempFilesPath = new DataGridViewTextBoxColumn();
            CustomizationPath = new DataGridViewTextBoxColumn();
            BackupPath = new DataGridViewTextBoxColumn();
            SnapshotPath = new DataGridViewTextBoxColumn();
            RegistryPath = new DataGridViewTextBoxColumn();
            iISAppBindingSource1 = new BindingSource(components);
            iISAppBindingSource = new BindingSource(components);
            buttonDeleteSelected = new Button();
            buttonDeleteAll = new Button();
            dateTimePicker = new DateTimePicker();
            labelOlderThan = new Label();
            checkBoxActive = new CheckBox();
            checkBoxJunk = new CheckBox();
            statusStrip = new StatusStrip();
            toolStripProgressBar = new ToolStripProgressBar();
            toolStripStatusLabel = new ToolStripStatusLabel();
            toolStripWarningLabel = new ToolStripStatusLabel();
            groupBoxCleanup = new GroupBox();
            checkBoxCleanupBackups = new CheckBox();
            checkBoxCleanupCustomizations = new CheckBox();
            checkBoxCleanupSnapshots = new CheckBox();
            checkBoxCleanupRegistry = new CheckBox();
            checkBoxCleanupDatabases = new CheckBox();
            checkBoxCleanupAspTemp = new CheckBox();
            checkBoxcleanupIISApps = new CheckBox();
            checkBoxCleanupFiles = new CheckBox();
            groupBoxFilter = new GroupBox();
            checkBoxRegistry = new CheckBox();
            checkBoxDatabases = new CheckBox();
            checkBoxBackups = new CheckBox();
            checkBoxCustomizations = new CheckBox();
            checkBoxSnapshots = new CheckBox();
            checkBoxAspTmpFiles = new CheckBox();
            checkBoxIISFiles = new CheckBox();
            groupBoxInstances = new GroupBox();
            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            refreshToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            resetToolStripMenuItem = new ToolStripMenuItem();
            enableModernUISwitchToolStripMenuItem = new ToolStripMenuItem();
            setSessionTimeoutToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            applyAllToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem1 = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iISAppBindingSource1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iISAppBindingSource).BeginInit();
            statusStrip.SuspendLayout();
            groupBoxCleanup.SuspendLayout();
            groupBoxFilter.SuspendLayout();
            groupBoxInstances.SuspendLayout();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AutoGenerateColumns = false;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new DataGridViewColumn[] { Id, Instance, Status, InstalledOn, Database, FilesPath, AspTempFilesPath, CustomizationPath, BackupPath, SnapshotPath, RegistryPath });
            dataGridView.DataSource = iISAppBindingSource1;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.Location = new Point(3, 24);
            dataGridView.Margin = new Padding(3, 4, 3, 4);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersWidth = 51;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new Size(1154, 771);
            dataGridView.TabIndex = 2;
            dataGridView.CellMouseDoubleClick += dataGridView_CellMouseDoubleClick;
            dataGridView.CellPainting += dataGridView_CellPainting;
            // 
            // Id
            // 
            Id.DataPropertyName = "Id";
            Id.HeaderText = "#";
            Id.MinimumWidth = 6;
            Id.Name = "Id";
            Id.ReadOnly = true;
            Id.Width = 50;
            // 
            // Instance
            // 
            Instance.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Instance.DataPropertyName = "InstanceName";
            Instance.HeaderText = "Instance";
            Instance.MinimumWidth = 6;
            Instance.Name = "Instance";
            Instance.ReadOnly = true;
            // 
            // Status
            // 
            Status.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Status.DataPropertyName = "Status";
            Status.HeaderText = "Status";
            Status.MinimumWidth = 6;
            Status.Name = "Status";
            Status.ReadOnly = true;
            // 
            // InstalledOn
            // 
            InstalledOn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            InstalledOn.DataPropertyName = "InstalledOn";
            InstalledOn.HeaderText = "Installed On";
            InstalledOn.MinimumWidth = 6;
            InstalledOn.Name = "InstalledOn";
            InstalledOn.ReadOnly = true;
            // 
            // Database
            // 
            Database.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Database.DataPropertyName = "Database";
            Database.HeaderText = "Database";
            Database.MinimumWidth = 6;
            Database.Name = "Database";
            Database.ReadOnly = true;
            // 
            // FilesPath
            // 
            FilesPath.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            FilesPath.DataPropertyName = "Path";
            FilesPath.HeaderText = "Path";
            FilesPath.MinimumWidth = 6;
            FilesPath.Name = "FilesPath";
            FilesPath.ReadOnly = true;
            // 
            // AspTempFilesPath
            // 
            AspTempFilesPath.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            AspTempFilesPath.DataPropertyName = "AspTempFilesPath";
            AspTempFilesPath.HeaderText = "AspTempFiles Path";
            AspTempFilesPath.MinimumWidth = 6;
            AspTempFilesPath.Name = "AspTempFilesPath";
            AspTempFilesPath.ReadOnly = true;
            // 
            // CustomizationPath
            // 
            CustomizationPath.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CustomizationPath.DataPropertyName = "CustomizationPath";
            CustomizationPath.HeaderText = "Customization Path";
            CustomizationPath.MinimumWidth = 6;
            CustomizationPath.Name = "CustomizationPath";
            CustomizationPath.ReadOnly = true;
            // 
            // BackupPath
            // 
            BackupPath.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            BackupPath.DataPropertyName = "BackupPath";
            BackupPath.HeaderText = "BackupPath";
            BackupPath.MinimumWidth = 6;
            BackupPath.Name = "BackupPath";
            BackupPath.ReadOnly = true;
            // 
            // SnapshotPath
            // 
            SnapshotPath.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            SnapshotPath.DataPropertyName = "SnapshotPath";
            SnapshotPath.HeaderText = "Snapshot Path";
            SnapshotPath.MinimumWidth = 6;
            SnapshotPath.Name = "SnapshotPath";
            SnapshotPath.ReadOnly = true;
            // 
            // RegistryPath
            // 
            RegistryPath.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            RegistryPath.DataPropertyName = "RegistryPath";
            RegistryPath.HeaderText = "Registry Path";
            RegistryPath.MinimumWidth = 6;
            RegistryPath.Name = "RegistryPath";
            RegistryPath.ReadOnly = true;
            // 
            // iISAppBindingSource1
            // 
            iISAppBindingSource1.DataSource = typeof(IISApp);
            // 
            // iISAppBindingSource
            // 
            iISAppBindingSource.DataSource = typeof(IISApp);
            // 
            // buttonDeleteSelected
            // 
            buttonDeleteSelected.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonDeleteSelected.Location = new Point(72, 359);
            buttonDeleteSelected.Margin = new Padding(3, 4, 3, 4);
            buttonDeleteSelected.Name = "buttonDeleteSelected";
            buttonDeleteSelected.Size = new Size(86, 31);
            buttonDeleteSelected.TabIndex = 3;
            buttonDeleteSelected.Text = "Clean";
            buttonDeleteSelected.UseVisualStyleBackColor = true;
            buttonDeleteSelected.Click += buttonDeleteSelected_Click;
            // 
            // buttonDeleteAll
            // 
            buttonDeleteAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonDeleteAll.Location = new Point(165, 359);
            buttonDeleteAll.Margin = new Padding(3, 4, 3, 4);
            buttonDeleteAll.Name = "buttonDeleteAll";
            buttonDeleteAll.Size = new Size(86, 31);
            buttonDeleteAll.TabIndex = 4;
            buttonDeleteAll.Text = "Clean all";
            buttonDeleteAll.UseVisualStyleBackColor = true;
            buttonDeleteAll.Click += buttonDeleteAll_Click;
            // 
            // dateTimePicker
            // 
            dateTimePicker.Location = new Point(7, 49);
            dateTimePicker.Margin = new Padding(3, 4, 3, 4);
            dateTimePicker.Name = "dateTimePicker";
            dateTimePicker.Size = new Size(239, 27);
            dateTimePicker.TabIndex = 5;
            dateTimePicker.ValueChanged += dateTimePicker_ValueChanged;
            // 
            // labelOlderThan
            // 
            labelOlderThan.AutoSize = true;
            labelOlderThan.Location = new Point(7, 25);
            labelOlderThan.Name = "labelOlderThan";
            labelOlderThan.Size = new Size(82, 20);
            labelOlderThan.TabIndex = 6;
            labelOlderThan.Text = "Older than:";
            // 
            // checkBoxActive
            // 
            checkBoxActive.AutoSize = true;
            checkBoxActive.Checked = true;
            checkBoxActive.CheckState = CheckState.Checked;
            checkBoxActive.Location = new Point(7, 88);
            checkBoxActive.Margin = new Padding(3, 4, 3, 4);
            checkBoxActive.Name = "checkBoxActive";
            checkBoxActive.Size = new Size(72, 24);
            checkBoxActive.TabIndex = 7;
            checkBoxActive.Text = "Active";
            checkBoxActive.UseVisualStyleBackColor = true;
            checkBoxActive.CheckedChanged += filter_CheckedChanged;
            // 
            // checkBoxJunk
            // 
            checkBoxJunk.AutoSize = true;
            checkBoxJunk.Checked = true;
            checkBoxJunk.CheckState = CheckState.Checked;
            checkBoxJunk.Location = new Point(7, 121);
            checkBoxJunk.Margin = new Padding(3, 4, 3, 4);
            checkBoxJunk.Name = "checkBoxJunk";
            checkBoxJunk.Size = new Size(82, 24);
            checkBoxJunk.TabIndex = 8;
            checkBoxJunk.Text = "Inactive";
            checkBoxJunk.UseVisualStyleBackColor = true;
            checkBoxJunk.CheckedChanged += filter_CheckedChanged;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripProgressBar, toolStripStatusLabel });
            statusStrip.Location = new Point(0, 841);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 16, 0);
            statusStrip.Size = new Size(1445, 27);
            statusStrip.TabIndex = 9;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripProgressBar
            // 
            toolStripProgressBar.Alignment = ToolStripItemAlignment.Right;
            toolStripProgressBar.Name = "toolStripProgressBar";
            toolStripProgressBar.Size = new Size(114, 19);
            toolStripProgressBar.Style = ProgressBarStyle.Marquee;
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(52, 21);
            toolStripStatusLabel.Text = "Status:";
            // 
            // toolStripWarningLabel
            // 
            toolStripWarningLabel.Name = "toolStripWarningLabel";
            toolStripWarningLabel.Size = new Size(23, 23);
            // 
            // groupBoxCleanup
            // 
            groupBoxCleanup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            groupBoxCleanup.Controls.Add(checkBoxCleanupBackups);
            groupBoxCleanup.Controls.Add(checkBoxCleanupCustomizations);
            groupBoxCleanup.Controls.Add(checkBoxCleanupSnapshots);
            groupBoxCleanup.Controls.Add(checkBoxCleanupRegistry);
            groupBoxCleanup.Controls.Add(checkBoxCleanupDatabases);
            groupBoxCleanup.Controls.Add(checkBoxCleanupAspTemp);
            groupBoxCleanup.Controls.Add(checkBoxcleanupIISApps);
            groupBoxCleanup.Controls.Add(checkBoxCleanupFiles);
            groupBoxCleanup.Controls.Add(buttonDeleteSelected);
            groupBoxCleanup.Controls.Add(buttonDeleteAll);
            groupBoxCleanup.Location = new Point(1181, 437);
            groupBoxCleanup.Margin = new Padding(3, 4, 3, 4);
            groupBoxCleanup.Name = "groupBoxCleanup";
            groupBoxCleanup.Padding = new Padding(3, 4, 3, 4);
            groupBoxCleanup.Size = new Size(257, 397);
            groupBoxCleanup.TabIndex = 10;
            groupBoxCleanup.TabStop = false;
            groupBoxCleanup.Text = "Cleanup";
            // 
            // checkBoxCleanupBackups
            // 
            checkBoxCleanupBackups.AutoSize = true;
            checkBoxCleanupBackups.Checked = true;
            checkBoxCleanupBackups.CheckState = CheckState.Checked;
            checkBoxCleanupBackups.Location = new Point(7, 196);
            checkBoxCleanupBackups.Margin = new Padding(3, 4, 3, 4);
            checkBoxCleanupBackups.Name = "checkBoxCleanupBackups";
            checkBoxCleanupBackups.Size = new Size(168, 24);
            checkBoxCleanupBackups.TabIndex = 19;
            checkBoxCleanupBackups.Text = "Cleanup Backup files";
            checkBoxCleanupBackups.UseVisualStyleBackColor = true;
            // 
            // checkBoxCleanupCustomizations
            // 
            checkBoxCleanupCustomizations.AutoSize = true;
            checkBoxCleanupCustomizations.Checked = true;
            checkBoxCleanupCustomizations.CheckState = CheckState.Checked;
            checkBoxCleanupCustomizations.Location = new Point(7, 163);
            checkBoxCleanupCustomizations.Margin = new Padding(3, 4, 3, 4);
            checkBoxCleanupCustomizations.Name = "checkBoxCleanupCustomizations";
            checkBoxCleanupCustomizations.Size = new Size(215, 24);
            checkBoxCleanupCustomizations.TabIndex = 18;
            checkBoxCleanupCustomizations.Text = "Cleanup Customization files";
            checkBoxCleanupCustomizations.UseVisualStyleBackColor = true;
            // 
            // checkBoxCleanupSnapshots
            // 
            checkBoxCleanupSnapshots.AutoSize = true;
            checkBoxCleanupSnapshots.Checked = true;
            checkBoxCleanupSnapshots.CheckState = CheckState.Checked;
            checkBoxCleanupSnapshots.Location = new Point(7, 129);
            checkBoxCleanupSnapshots.Margin = new Padding(3, 4, 3, 4);
            checkBoxCleanupSnapshots.Name = "checkBoxCleanupSnapshots";
            checkBoxCleanupSnapshots.Size = new Size(181, 24);
            checkBoxCleanupSnapshots.TabIndex = 17;
            checkBoxCleanupSnapshots.Text = "Cleanup Snapshot files";
            checkBoxCleanupSnapshots.UseVisualStyleBackColor = true;
            // 
            // checkBoxCleanupRegistry
            // 
            checkBoxCleanupRegistry.AutoSize = true;
            checkBoxCleanupRegistry.Checked = true;
            checkBoxCleanupRegistry.CheckState = CheckState.Checked;
            checkBoxCleanupRegistry.Location = new Point(7, 263);
            checkBoxCleanupRegistry.Margin = new Padding(3, 4, 3, 4);
            checkBoxCleanupRegistry.Name = "checkBoxCleanupRegistry";
            checkBoxCleanupRegistry.Size = new Size(142, 24);
            checkBoxCleanupRegistry.TabIndex = 16;
            checkBoxCleanupRegistry.Text = "Cleanup Registry";
            checkBoxCleanupRegistry.UseVisualStyleBackColor = true;
            // 
            // checkBoxCleanupDatabases
            // 
            checkBoxCleanupDatabases.AutoSize = true;
            checkBoxCleanupDatabases.Checked = true;
            checkBoxCleanupDatabases.CheckState = CheckState.Checked;
            checkBoxCleanupDatabases.Location = new Point(7, 229);
            checkBoxCleanupDatabases.Margin = new Padding(3, 4, 3, 4);
            checkBoxCleanupDatabases.Name = "checkBoxCleanupDatabases";
            checkBoxCleanupDatabases.Size = new Size(158, 24);
            checkBoxCleanupDatabases.TabIndex = 15;
            checkBoxCleanupDatabases.Text = "Cleanup Databases";
            checkBoxCleanupDatabases.UseVisualStyleBackColor = true;
            // 
            // checkBoxCleanupAspTemp
            // 
            checkBoxCleanupAspTemp.AutoSize = true;
            checkBoxCleanupAspTemp.Checked = true;
            checkBoxCleanupAspTemp.CheckState = CheckState.Checked;
            checkBoxCleanupAspTemp.Location = new Point(7, 96);
            checkBoxCleanupAspTemp.Margin = new Padding(3, 4, 3, 4);
            checkBoxCleanupAspTemp.Name = "checkBoxCleanupAspTemp";
            checkBoxCleanupAspTemp.Size = new Size(186, 24);
            checkBoxCleanupAspTemp.TabIndex = 14;
            checkBoxCleanupAspTemp.Text = "Cleanup Asp Temp files";
            checkBoxCleanupAspTemp.UseVisualStyleBackColor = true;
            // 
            // checkBoxcleanupIISApps
            // 
            checkBoxcleanupIISApps.AutoSize = true;
            checkBoxcleanupIISApps.Checked = true;
            checkBoxcleanupIISApps.CheckState = CheckState.Checked;
            checkBoxcleanupIISApps.Location = new Point(7, 29);
            checkBoxcleanupIISApps.Margin = new Padding(3, 4, 3, 4);
            checkBoxcleanupIISApps.Name = "checkBoxcleanupIISApps";
            checkBoxcleanupIISApps.Size = new Size(141, 24);
            checkBoxcleanupIISApps.TabIndex = 13;
            checkBoxcleanupIISApps.Text = "Cleanup IIS apps";
            checkBoxcleanupIISApps.UseVisualStyleBackColor = true;
            // 
            // checkBoxCleanupFiles
            // 
            checkBoxCleanupFiles.AutoSize = true;
            checkBoxCleanupFiles.Checked = true;
            checkBoxCleanupFiles.CheckState = CheckState.Checked;
            checkBoxCleanupFiles.Location = new Point(7, 63);
            checkBoxCleanupFiles.Margin = new Padding(3, 4, 3, 4);
            checkBoxCleanupFiles.Name = "checkBoxCleanupFiles";
            checkBoxCleanupFiles.Size = new Size(118, 24);
            checkBoxCleanupFiles.TabIndex = 12;
            checkBoxCleanupFiles.Text = "Cleanup Files";
            checkBoxCleanupFiles.UseVisualStyleBackColor = true;
            // 
            // groupBoxFilter
            // 
            groupBoxFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxFilter.Controls.Add(checkBoxRegistry);
            groupBoxFilter.Controls.Add(checkBoxDatabases);
            groupBoxFilter.Controls.Add(checkBoxBackups);
            groupBoxFilter.Controls.Add(checkBoxCustomizations);
            groupBoxFilter.Controls.Add(checkBoxSnapshots);
            groupBoxFilter.Controls.Add(checkBoxAspTmpFiles);
            groupBoxFilter.Controls.Add(checkBoxIISFiles);
            groupBoxFilter.Controls.Add(dateTimePicker);
            groupBoxFilter.Controls.Add(labelOlderThan);
            groupBoxFilter.Controls.Add(checkBoxActive);
            groupBoxFilter.Controls.Add(checkBoxJunk);
            groupBoxFilter.Location = new Point(1181, 36);
            groupBoxFilter.Margin = new Padding(3, 4, 3, 4);
            groupBoxFilter.Name = "groupBoxFilter";
            groupBoxFilter.Padding = new Padding(3, 4, 3, 4);
            groupBoxFilter.Size = new Size(257, 393);
            groupBoxFilter.TabIndex = 11;
            groupBoxFilter.TabStop = false;
            groupBoxFilter.Text = "Filter";
            // 
            // checkBoxRegistry
            // 
            checkBoxRegistry.AutoSize = true;
            checkBoxRegistry.Checked = true;
            checkBoxRegistry.CheckState = CheckState.Checked;
            checkBoxRegistry.Location = new Point(7, 355);
            checkBoxRegistry.Margin = new Padding(3, 4, 3, 4);
            checkBoxRegistry.Name = "checkBoxRegistry";
            checkBoxRegistry.Size = new Size(84, 24);
            checkBoxRegistry.TabIndex = 17;
            checkBoxRegistry.Text = "Registry";
            checkBoxRegistry.UseVisualStyleBackColor = true;
            checkBoxRegistry.CheckedChanged += filter_CheckedChanged;
            // 
            // checkBoxDatabases
            // 
            checkBoxDatabases.AutoSize = true;
            checkBoxDatabases.Checked = true;
            checkBoxDatabases.CheckState = CheckState.Checked;
            checkBoxDatabases.Location = new Point(7, 321);
            checkBoxDatabases.Margin = new Padding(3, 4, 3, 4);
            checkBoxDatabases.Name = "checkBoxDatabases";
            checkBoxDatabases.Size = new Size(100, 24);
            checkBoxDatabases.TabIndex = 16;
            checkBoxDatabases.Text = "Databases";
            checkBoxDatabases.UseVisualStyleBackColor = true;
            checkBoxDatabases.CheckedChanged += filter_CheckedChanged;
            // 
            // checkBoxBackups
            // 
            checkBoxBackups.AutoSize = true;
            checkBoxBackups.Checked = true;
            checkBoxBackups.CheckState = CheckState.Checked;
            checkBoxBackups.Location = new Point(7, 288);
            checkBoxBackups.Margin = new Padding(3, 4, 3, 4);
            checkBoxBackups.Name = "checkBoxBackups";
            checkBoxBackups.Size = new Size(110, 24);
            checkBoxBackups.TabIndex = 15;
            checkBoxBackups.Text = "Backup files";
            checkBoxBackups.UseVisualStyleBackColor = true;
            checkBoxBackups.CheckedChanged += filter_CheckedChanged;
            // 
            // checkBoxCustomizations
            // 
            checkBoxCustomizations.AutoSize = true;
            checkBoxCustomizations.Checked = true;
            checkBoxCustomizations.CheckState = CheckState.Checked;
            checkBoxCustomizations.Location = new Point(7, 255);
            checkBoxCustomizations.Margin = new Padding(3, 4, 3, 4);
            checkBoxCustomizations.Name = "checkBoxCustomizations";
            checkBoxCustomizations.Size = new Size(157, 24);
            checkBoxCustomizations.TabIndex = 14;
            checkBoxCustomizations.Text = "Customization files";
            checkBoxCustomizations.UseVisualStyleBackColor = true;
            checkBoxCustomizations.CheckedChanged += filter_CheckedChanged;
            // 
            // checkBoxSnapshots
            // 
            checkBoxSnapshots.AutoSize = true;
            checkBoxSnapshots.Checked = true;
            checkBoxSnapshots.CheckState = CheckState.Checked;
            checkBoxSnapshots.Location = new Point(7, 221);
            checkBoxSnapshots.Margin = new Padding(3, 4, 3, 4);
            checkBoxSnapshots.Name = "checkBoxSnapshots";
            checkBoxSnapshots.Size = new Size(123, 24);
            checkBoxSnapshots.TabIndex = 13;
            checkBoxSnapshots.Text = "Snapshot files";
            checkBoxSnapshots.UseVisualStyleBackColor = true;
            checkBoxSnapshots.CheckedChanged += filter_CheckedChanged;
            // 
            // checkBoxAspTmpFiles
            // 
            checkBoxAspTmpFiles.AutoSize = true;
            checkBoxAspTmpFiles.Checked = true;
            checkBoxAspTmpFiles.CheckState = CheckState.Checked;
            checkBoxAspTmpFiles.Location = new Point(7, 188);
            checkBoxAspTmpFiles.Margin = new Padding(3, 4, 3, 4);
            checkBoxAspTmpFiles.Name = "checkBoxAspTmpFiles";
            checkBoxAspTmpFiles.Size = new Size(130, 24);
            checkBoxAspTmpFiles.TabIndex = 12;
            checkBoxAspTmpFiles.Text = "Asp Temp Files";
            checkBoxAspTmpFiles.UseVisualStyleBackColor = true;
            checkBoxAspTmpFiles.CheckedChanged += filter_CheckedChanged;
            // 
            // checkBoxIISFiles
            // 
            checkBoxIISFiles.AutoSize = true;
            checkBoxIISFiles.Checked = true;
            checkBoxIISFiles.CheckState = CheckState.Checked;
            checkBoxIISFiles.Location = new Point(7, 155);
            checkBoxIISFiles.Margin = new Padding(3, 4, 3, 4);
            checkBoxIISFiles.Name = "checkBoxIISFiles";
            checkBoxIISFiles.Size = new Size(60, 24);
            checkBoxIISFiles.TabIndex = 11;
            checkBoxIISFiles.Text = "Files";
            checkBoxIISFiles.UseVisualStyleBackColor = true;
            checkBoxIISFiles.CheckedChanged += filter_CheckedChanged;
            // 
            // groupBoxInstances
            // 
            groupBoxInstances.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxInstances.Controls.Add(dataGridView);
            groupBoxInstances.Location = new Point(14, 36);
            groupBoxInstances.Margin = new Padding(3, 4, 3, 4);
            groupBoxInstances.Name = "groupBoxInstances";
            groupBoxInstances.Padding = new Padding(3, 4, 3, 4);
            groupBoxInstances.Size = new Size(1160, 799);
            groupBoxInstances.TabIndex = 12;
            groupBoxInstances.TabStop = false;
            groupBoxInstances.Text = "Instances";
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(7, 3, 0, 3);
            menuStrip.Size = new Size(1445, 30);
            menuStrip.TabIndex = 13;
            menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { refreshToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.ShortcutKeys = Keys.F5;
            refreshToolStripMenuItem.Size = new Size(169, 26);
            refreshToolStripMenuItem.Text = "Refresh";
            refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(166, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.ShortcutKeyDisplayString = "";
            exitToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
            exitToolStripMenuItem.Size = new Size(169, 26);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { resetToolStripMenuItem, enableModernUISwitchToolStripMenuItem, setSessionTimeoutToolStripMenuItem, toolStripSeparator4, applyAllToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(58, 24);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // resetToolStripMenuItem
            // 
            resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            resetToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.R;
            resetToolStripMenuItem.Size = new Size(313, 26);
            resetToolStripMenuItem.Text = "Reset Password";
            resetToolStripMenuItem.Click += resetToolStripMenuItem_Click;
            // 
            // enableModernUISwitchToolStripMenuItem
            // 
            enableModernUISwitchToolStripMenuItem.Name = "enableModernUISwitchToolStripMenuItem";
            enableModernUISwitchToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.M;
            enableModernUISwitchToolStripMenuItem.Size = new Size(313, 26);
            enableModernUISwitchToolStripMenuItem.Text = "Enable Modern UI Switch";
            enableModernUISwitchToolStripMenuItem.Click += enableModernUISwitchToolStripMenuItem_Click;
            // 
            // setSessionTimeoutToolStripMenuItem
            // 
            setSessionTimeoutToolStripMenuItem.Name = "setSessionTimeoutToolStripMenuItem";
            setSessionTimeoutToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.T;
            setSessionTimeoutToolStripMenuItem.Size = new Size(313, 26);
            setSessionTimeoutToolStripMenuItem.Text = "Extend Session Timeout";
            setSessionTimeoutToolStripMenuItem.Click += setSessionTimeoutToolStripMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(310, 6);
            // 
            // applyAllToolStripMenuItem
            // 
            applyAllToolStripMenuItem.Name = "applyAllToolStripMenuItem";
            applyAllToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;
            applyAllToolStripMenuItem.Size = new Size(313, 26);
            applyAllToolStripMenuItem.Text = "Apply All Patches";
            applyAllToolStripMenuItem.Click += applyAllToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem1 });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.ShortcutKeys = Keys.F1;
            helpToolStripMenuItem.Size = new Size(55, 24);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem1
            // 
            aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            aboutToolStripMenuItem1.ShortcutKeys = Keys.F1;
            aboutToolStripMenuItem1.Size = new Size(224, 26);
            aboutToolStripMenuItem1.Text = "About";
            aboutToolStripMenuItem1.Click += aboutToolStripMenuItem1_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1445, 868);
            Controls.Add(groupBoxInstances);
            Controls.Add(groupBoxFilter);
            Controls.Add(groupBoxCleanup);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AcuCleaner";
            Load += Form_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)iISAppBindingSource1).EndInit();
            ((System.ComponentModel.ISupportInitialize)iISAppBindingSource).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            groupBoxCleanup.ResumeLayout(false);
            groupBoxCleanup.PerformLayout();
            groupBoxFilter.ResumeLayout(false);
            groupBoxFilter.PerformLayout();
            groupBoxInstances.ResumeLayout(false);
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView;
        private BindingSource iISAppBindingSource;
        private BindingSource iISAppBindingSource1;
        private Button buttonDeleteSelected;
        private Button buttonDeleteAll;
        private DateTimePicker dateTimePicker;
        private Label labelOlderThan;
        private CheckBox checkBoxActive;
        private CheckBox checkBoxJunk;
        private StatusStrip statusStrip;
        private ToolStripProgressBar toolStripProgressBar;
        private ToolStripStatusLabel toolStripWarningLabel;
        private ToolStripStatusLabel toolStripStatusLabel;
        private GroupBox groupBoxCleanup;
        private GroupBox groupBoxFilter;
        private CheckBox checkBoxCleanupRegistry;
        private CheckBox checkBoxCleanupDatabases;
        private CheckBox checkBoxCleanupAspTemp;
        private CheckBox checkBoxcleanupIISApps;
        private CheckBox checkBoxCleanupFiles;
        private CheckBox checkBoxCleanupSnapshots;
        private CheckBox checkBoxCleanupCustomizations;
        private CheckBox checkBoxCleanupBackups;
        private CheckBox checkBoxAspTmpFiles;
        private CheckBox checkBoxIISFiles;
        private CheckBox checkBoxCustomizations;
        private CheckBox checkBoxSnapshots;
        private CheckBox checkBoxRegistry;
        private CheckBox checkBoxDatabases;
        private CheckBox checkBoxBackups;
        private GroupBox groupBoxInstances;
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem resetToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem setSessionTimeoutToolStripMenuItem;
        private ToolStripMenuItem refreshToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem enableModernUISwitchToolStripMenuItem;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn Instance;
        private DataGridViewTextBoxColumn Status;
        private DataGridViewTextBoxColumn InstalledOn;
        private DataGridViewTextBoxColumn Database;
        private DataGridViewTextBoxColumn FilesPath;
        private DataGridViewTextBoxColumn AspTempFilesPath;
        private DataGridViewTextBoxColumn CustomizationPath;
        private DataGridViewTextBoxColumn BackupPath;
        private DataGridViewTextBoxColumn SnapshotPath;
        private DataGridViewTextBoxColumn RegistryPath;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem applyAllToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem1;
    }
}
