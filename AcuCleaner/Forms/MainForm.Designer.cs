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
            Active = new DataGridViewCheckBoxColumn();
            InstalledOn = new DataGridViewTextBoxColumn();
            DatabaseName = new DataGridViewTextBoxColumn();
            pathDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
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
            buttonRefresh = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iISAppBindingSource1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iISAppBindingSource).BeginInit();
            statusStrip.SuspendLayout();
            groupBoxCleanup.SuspendLayout();
            groupBoxFilter.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView.AutoGenerateColumns = false;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new DataGridViewColumn[] { Id, Instance, Active, InstalledOn, DatabaseName, pathDataGridViewTextBoxColumn, AspTempFilesPath, CustomizationPath, BackupPath, SnapshotPath, RegistryPath });
            dataGridView.DataSource = iISAppBindingSource1;
            dataGridView.Location = new Point(12, 12);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersWidth = 51;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new Size(1009, 524);
            dataGridView.TabIndex = 2;
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
            Instance.DataPropertyName = "InstanceName";
            Instance.HeaderText = "Instance";
            Instance.MinimumWidth = 6;
            Instance.Name = "Instance";
            Instance.ReadOnly = true;
            // 
            // Active
            // 
            Active.DataPropertyName = "Active";
            Active.HeaderText = "Active/Junk";
            Active.MinimumWidth = 6;
            Active.Name = "Active";
            Active.ReadOnly = true;
            Active.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // InstalledOn
            // 
            InstalledOn.DataPropertyName = "InstalledOn";
            InstalledOn.HeaderText = "Installed On";
            InstalledOn.MinimumWidth = 6;
            InstalledOn.Name = "InstalledOn";
            InstalledOn.ReadOnly = true;
            // 
            // DatabaseName
            // 
            DatabaseName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DatabaseName.DataPropertyName = "DatabaseName";
            DatabaseName.HeaderText = "Database";
            DatabaseName.MinimumWidth = 6;
            DatabaseName.Name = "DatabaseName";
            DatabaseName.ReadOnly = true;
            // 
            // pathDataGridViewTextBoxColumn
            // 
            pathDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            pathDataGridViewTextBoxColumn.DataPropertyName = "Path";
            pathDataGridViewTextBoxColumn.HeaderText = "Path";
            pathDataGridViewTextBoxColumn.MinimumWidth = 6;
            pathDataGridViewTextBoxColumn.Name = "pathDataGridViewTextBoxColumn";
            pathDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // AspTempFilesPath
            // 
            AspTempFilesPath.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            AspTempFilesPath.DataPropertyName = "AspTempFilesPath";
            AspTempFilesPath.HeaderText = "AspTempFiles Path";
            AspTempFilesPath.Name = "AspTempFilesPath";
            AspTempFilesPath.ReadOnly = true;
            // 
            // CustomizationPath
            // 
            CustomizationPath.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CustomizationPath.DataPropertyName = "CustomizationPath";
            CustomizationPath.HeaderText = "Customization Path";
            CustomizationPath.Name = "CustomizationPath";
            CustomizationPath.ReadOnly = true;
            // 
            // BackupPath
            // 
            BackupPath.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            BackupPath.DataPropertyName = "BackupPath";
            BackupPath.HeaderText = "BackupPath";
            BackupPath.Name = "BackupPath";
            BackupPath.ReadOnly = true;
            // 
            // SnapshotPath
            // 
            SnapshotPath.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            SnapshotPath.DataPropertyName = "SnapshotPath";
            SnapshotPath.HeaderText = "Snapshot Path";
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
            buttonDeleteSelected.Location = new Point(63, 349);
            buttonDeleteSelected.Name = "buttonDeleteSelected";
            buttonDeleteSelected.Size = new Size(75, 23);
            buttonDeleteSelected.TabIndex = 3;
            buttonDeleteSelected.Text = "Clean";
            buttonDeleteSelected.UseVisualStyleBackColor = true;
            buttonDeleteSelected.Click += buttonDeleteSelected_Click;
            // 
            // buttonDeleteAll
            // 
            buttonDeleteAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonDeleteAll.Location = new Point(144, 349);
            buttonDeleteAll.Name = "buttonDeleteAll";
            buttonDeleteAll.Size = new Size(75, 23);
            buttonDeleteAll.TabIndex = 4;
            buttonDeleteAll.Text = "Clean all";
            buttonDeleteAll.UseVisualStyleBackColor = true;
            buttonDeleteAll.Click += buttonDeleteAll_Click;
            // 
            // dateTimePicker
            // 
            dateTimePicker.Location = new Point(6, 37);
            dateTimePicker.Name = "dateTimePicker";
            dateTimePicker.Size = new Size(210, 23);
            dateTimePicker.TabIndex = 5;
            dateTimePicker.ValueChanged += dateTimePicker_ValueChanged;
            // 
            // labelOlderThan
            // 
            labelOlderThan.AutoSize = true;
            labelOlderThan.Location = new Point(6, 19);
            labelOlderThan.Name = "labelOlderThan";
            labelOlderThan.Size = new Size(66, 15);
            labelOlderThan.TabIndex = 6;
            labelOlderThan.Text = "Older than:";
            // 
            // checkBoxActive
            // 
            checkBoxActive.AutoSize = true;
            checkBoxActive.Checked = true;
            checkBoxActive.CheckState = CheckState.Checked;
            checkBoxActive.Location = new Point(6, 66);
            checkBoxActive.Name = "checkBoxActive";
            checkBoxActive.Size = new Size(59, 19);
            checkBoxActive.TabIndex = 7;
            checkBoxActive.Text = "Active";
            checkBoxActive.UseVisualStyleBackColor = true;
            checkBoxActive.CheckedChanged += checkBoxActive_CheckedChanged;
            // 
            // checkBoxJunk
            // 
            checkBoxJunk.AutoSize = true;
            checkBoxJunk.Checked = true;
            checkBoxJunk.CheckState = CheckState.Checked;
            checkBoxJunk.Location = new Point(6, 91);
            checkBoxJunk.Name = "checkBoxJunk";
            checkBoxJunk.Size = new Size(50, 19);
            checkBoxJunk.TabIndex = 8;
            checkBoxJunk.Text = "Junk";
            checkBoxJunk.UseVisualStyleBackColor = true;
            checkBoxJunk.CheckedChanged += checkBoxJunk_CheckedChanged;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripProgressBar, toolStripStatusLabel });
            statusStrip.Location = new Point(0, 539);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1264, 22);
            statusStrip.TabIndex = 9;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripProgressBar
            // 
            toolStripProgressBar.Alignment = ToolStripItemAlignment.Right;
            toolStripProgressBar.Name = "toolStripProgressBar";
            toolStripProgressBar.Size = new Size(100, 16);
            toolStripProgressBar.Style = ProgressBarStyle.Marquee;
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(42, 17);
            toolStripStatusLabel.Text = "Status:";
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
            groupBoxCleanup.Location = new Point(1027, 158);
            groupBoxCleanup.Name = "groupBoxCleanup";
            groupBoxCleanup.Size = new Size(225, 378);
            groupBoxCleanup.TabIndex = 10;
            groupBoxCleanup.TabStop = false;
            groupBoxCleanup.Text = "Cleanup";
            // 
            // checkBoxCleanupBackups
            // 
            checkBoxCleanupBackups.AutoSize = true;
            checkBoxCleanupBackups.Checked = true;
            checkBoxCleanupBackups.CheckState = CheckState.Checked;
            checkBoxCleanupBackups.Location = new Point(6, 147);
            checkBoxCleanupBackups.Name = "checkBoxCleanupBackups";
            checkBoxCleanupBackups.Size = new Size(136, 19);
            checkBoxCleanupBackups.TabIndex = 19;
            checkBoxCleanupBackups.Text = "Cleanup Backup files";
            checkBoxCleanupBackups.UseVisualStyleBackColor = true;
            // 
            // checkBoxCleanupCustomizations
            // 
            checkBoxCleanupCustomizations.AutoSize = true;
            checkBoxCleanupCustomizations.Checked = true;
            checkBoxCleanupCustomizations.CheckState = CheckState.Checked;
            checkBoxCleanupCustomizations.Location = new Point(6, 122);
            checkBoxCleanupCustomizations.Name = "checkBoxCleanupCustomizations";
            checkBoxCleanupCustomizations.Size = new Size(174, 19);
            checkBoxCleanupCustomizations.TabIndex = 18;
            checkBoxCleanupCustomizations.Text = "Cleanup Customization files";
            checkBoxCleanupCustomizations.UseVisualStyleBackColor = true;
            // 
            // checkBoxCleanupSnapshots
            // 
            checkBoxCleanupSnapshots.AutoSize = true;
            checkBoxCleanupSnapshots.Checked = true;
            checkBoxCleanupSnapshots.CheckState = CheckState.Checked;
            checkBoxCleanupSnapshots.Location = new Point(6, 97);
            checkBoxCleanupSnapshots.Name = "checkBoxCleanupSnapshots";
            checkBoxCleanupSnapshots.Size = new Size(146, 19);
            checkBoxCleanupSnapshots.TabIndex = 17;
            checkBoxCleanupSnapshots.Text = "Cleanup Snapshot files";
            checkBoxCleanupSnapshots.UseVisualStyleBackColor = true;
            // 
            // checkBoxCleanupRegistry
            // 
            checkBoxCleanupRegistry.AutoSize = true;
            checkBoxCleanupRegistry.Checked = true;
            checkBoxCleanupRegistry.CheckState = CheckState.Checked;
            checkBoxCleanupRegistry.Location = new Point(6, 197);
            checkBoxCleanupRegistry.Name = "checkBoxCleanupRegistry";
            checkBoxCleanupRegistry.Size = new Size(115, 19);
            checkBoxCleanupRegistry.TabIndex = 16;
            checkBoxCleanupRegistry.Text = "Cleanup Registry";
            checkBoxCleanupRegistry.UseVisualStyleBackColor = true;
            // 
            // checkBoxCleanupDatabases
            // 
            checkBoxCleanupDatabases.AutoSize = true;
            checkBoxCleanupDatabases.Checked = true;
            checkBoxCleanupDatabases.CheckState = CheckState.Checked;
            checkBoxCleanupDatabases.Location = new Point(6, 172);
            checkBoxCleanupDatabases.Name = "checkBoxCleanupDatabases";
            checkBoxCleanupDatabases.Size = new Size(126, 19);
            checkBoxCleanupDatabases.TabIndex = 15;
            checkBoxCleanupDatabases.Text = "Cleanup Databases";
            checkBoxCleanupDatabases.UseVisualStyleBackColor = true;
            // 
            // checkBoxCleanupAspTemp
            // 
            checkBoxCleanupAspTemp.AutoSize = true;
            checkBoxCleanupAspTemp.Checked = true;
            checkBoxCleanupAspTemp.CheckState = CheckState.Checked;
            checkBoxCleanupAspTemp.Location = new Point(6, 72);
            checkBoxCleanupAspTemp.Name = "checkBoxCleanupAspTemp";
            checkBoxCleanupAspTemp.Size = new Size(127, 19);
            checkBoxCleanupAspTemp.TabIndex = 14;
            checkBoxCleanupAspTemp.Text = "Cleanup Temp files";
            checkBoxCleanupAspTemp.UseVisualStyleBackColor = true;
            // 
            // checkBoxcleanupIISApps
            // 
            checkBoxcleanupIISApps.AutoSize = true;
            checkBoxcleanupIISApps.Checked = true;
            checkBoxcleanupIISApps.CheckState = CheckState.Checked;
            checkBoxcleanupIISApps.Location = new Point(6, 22);
            checkBoxcleanupIISApps.Name = "checkBoxcleanupIISApps";
            checkBoxcleanupIISApps.Size = new Size(113, 19);
            checkBoxcleanupIISApps.TabIndex = 13;
            checkBoxcleanupIISApps.Text = "Cleanup IIS apps";
            checkBoxcleanupIISApps.UseVisualStyleBackColor = true;
            // 
            // checkBoxCleanupFiles
            // 
            checkBoxCleanupFiles.AutoSize = true;
            checkBoxCleanupFiles.Checked = true;
            checkBoxCleanupFiles.CheckState = CheckState.Checked;
            checkBoxCleanupFiles.Location = new Point(6, 47);
            checkBoxCleanupFiles.Name = "checkBoxCleanupFiles";
            checkBoxCleanupFiles.Size = new Size(96, 19);
            checkBoxCleanupFiles.TabIndex = 12;
            checkBoxCleanupFiles.Text = "Cleanup Files";
            checkBoxCleanupFiles.UseVisualStyleBackColor = true;
            // 
            // groupBoxFilter
            // 
            groupBoxFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxFilter.Controls.Add(buttonRefresh);
            groupBoxFilter.Controls.Add(dateTimePicker);
            groupBoxFilter.Controls.Add(labelOlderThan);
            groupBoxFilter.Controls.Add(checkBoxActive);
            groupBoxFilter.Controls.Add(checkBoxJunk);
            groupBoxFilter.Location = new Point(1027, 12);
            groupBoxFilter.Name = "groupBoxFilter";
            groupBoxFilter.Size = new Size(225, 140);
            groupBoxFilter.TabIndex = 11;
            groupBoxFilter.TabStop = false;
            groupBoxFilter.Text = "Filter";
            // 
            // buttonRefresh
            // 
            buttonRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonRefresh.Location = new Point(144, 111);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new Size(75, 23);
            buttonRefresh.TabIndex = 9;
            buttonRefresh.Text = "Refresh";
            buttonRefresh.UseVisualStyleBackColor = true;
            buttonRefresh.Click += buttonRefresh_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1264, 561);
            Controls.Add(groupBoxFilter);
            Controls.Add(groupBoxCleanup);
            Controls.Add(statusStrip);
            Controls.Add(dataGridView);
            Icon = (Icon)resources.GetObject("$this.Icon");
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
        private ToolStripStatusLabel toolStripStatusLabel;
        private GroupBox groupBoxCleanup;
        private GroupBox groupBoxFilter;
        private CheckBox checkBoxCleanupRegistry;
        private CheckBox checkBoxCleanupDatabases;
        private CheckBox checkBoxCleanupAspTemp;
        private CheckBox checkBoxcleanupIISApps;
        private CheckBox checkBoxCleanupFiles;
        private Button buttonRefresh;
        private CheckBox checkBoxCleanupSnapshots;
        private CheckBox checkBoxCleanupCustomizations;
        private CheckBox checkBoxCleanupBackups;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn Instance;
        private DataGridViewCheckBoxColumn Active;
        private DataGridViewTextBoxColumn InstalledOn;
        private DataGridViewTextBoxColumn DatabaseName;
        private DataGridViewTextBoxColumn pathDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn AspTempFilesPath;
        private DataGridViewTextBoxColumn CustomizationPath;
        private DataGridViewTextBoxColumn BackupPath;
        private DataGridViewTextBoxColumn SnapshotPath;
        private DataGridViewTextBoxColumn RegistryPath;
    }
}
