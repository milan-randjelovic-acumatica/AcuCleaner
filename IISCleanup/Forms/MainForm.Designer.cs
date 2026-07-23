using IISCleanup.Models;

namespace IISCleanup.Forms
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
            iISAppBindingSource1 = new BindingSource(components);
            iISAppBindingSource = new BindingSource(components);
            buttonDeleteSelected = new Button();
            buttonDeleteAll = new Button();
            dateTimePicker = new DateTimePicker();
            labelOlderThan = new Label();
            checkBoxActive = new CheckBox();
            checkBoxJunk = new CheckBox();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            toolStripProgressBar = new ToolStripProgressBar();
            Id = new DataGridViewTextBoxColumn();
            Instance = new DataGridViewTextBoxColumn();
            Active = new DataGridViewCheckBoxColumn();
            InstalledOn = new DataGridViewTextBoxColumn();
            DatabaseName = new DataGridViewTextBoxColumn();
            pathDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            AspFilesTempPath = new DataGridViewTextBoxColumn();
            RegistryPath = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iISAppBindingSource1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iISAppBindingSource).BeginInit();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView.AutoGenerateColumns = false;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new DataGridViewColumn[] { Id, Instance, Active, InstalledOn, DatabaseName, pathDataGridViewTextBoxColumn, AspFilesTempPath, RegistryPath });
            dataGridView.DataSource = iISAppBindingSource1;
            dataGridView.Location = new Point(0, 55);
            dataGridView.Margin = new Padding(3, 4, 3, 4);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersWidth = 51;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new Size(1628, 660);
            dataGridView.TabIndex = 2;
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
            buttonDeleteSelected.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonDeleteSelected.Location = new Point(1450, 11);
            buttonDeleteSelected.Margin = new Padding(3, 4, 3, 4);
            buttonDeleteSelected.Name = "buttonDeleteSelected";
            buttonDeleteSelected.Size = new Size(86, 31);
            buttonDeleteSelected.TabIndex = 3;
            buttonDeleteSelected.Text = "Delete";
            buttonDeleteSelected.UseVisualStyleBackColor = true;
            buttonDeleteSelected.Click += buttonDeleteSelected_Click;
            // 
            // buttonDeleteAll
            // 
            buttonDeleteAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonDeleteAll.Location = new Point(1542, 11);
            buttonDeleteAll.Margin = new Padding(3, 4, 3, 4);
            buttonDeleteAll.Name = "buttonDeleteAll";
            buttonDeleteAll.Size = new Size(86, 31);
            buttonDeleteAll.TabIndex = 4;
            buttonDeleteAll.Text = "Delete All";
            buttonDeleteAll.UseVisualStyleBackColor = true;
            buttonDeleteAll.Click += buttonDeleteAll_Click;
            // 
            // dateTimePicker
            // 
            dateTimePicker.Location = new Point(96, 11);
            dateTimePicker.Margin = new Padding(3, 4, 3, 4);
            dateTimePicker.Name = "dateTimePicker";
            dateTimePicker.Size = new Size(239, 27);
            dateTimePicker.TabIndex = 5;
            dateTimePicker.ValueChanged += dateTimePicker_ValueChanged;
            // 
            // labelOlderThan
            // 
            labelOlderThan.AutoSize = true;
            labelOlderThan.Location = new Point(14, 16);
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
            checkBoxActive.Location = new Point(343, 15);
            checkBoxActive.Margin = new Padding(3, 4, 3, 4);
            checkBoxActive.Name = "checkBoxActive";
            checkBoxActive.Size = new Size(72, 24);
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
            checkBoxJunk.Location = new Point(417, 15);
            checkBoxJunk.Margin = new Padding(3, 4, 3, 4);
            checkBoxJunk.Name = "checkBoxJunk";
            checkBoxJunk.Size = new Size(59, 24);
            checkBoxJunk.TabIndex = 8;
            checkBoxJunk.Text = "Junk";
            checkBoxJunk.UseVisualStyleBackColor = true;
            checkBoxJunk.CheckedChanged += checkBoxJunk_CheckedChanged;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel, toolStripProgressBar });
            statusStrip.Location = new Point(0, 721);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 16, 0);
            statusStrip.Size = new Size(1642, 27);
            statusStrip.TabIndex = 9;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(52, 21);
            toolStripStatusLabel.Text = "Status:";
            // 
            // toolStripProgressBar
            // 
            toolStripProgressBar.Name = "toolStripProgressBar";
            toolStripProgressBar.Size = new Size(114, 19);
            toolStripProgressBar.Style = ProgressBarStyle.Marquee;
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
            Instance.Width = 150;
            // 
            // Active
            // 
            Active.DataPropertyName = "Active";
            Active.HeaderText = "Active/Junk";
            Active.MinimumWidth = 6;
            Active.Name = "Active";
            Active.ReadOnly = true;
            Active.SortMode = DataGridViewColumnSortMode.Automatic;
            Active.Width = 125;
            // 
            // InstalledOn
            // 
            InstalledOn.DataPropertyName = "InstalledOn";
            InstalledOn.HeaderText = "Installed On";
            InstalledOn.MinimumWidth = 6;
            InstalledOn.Name = "InstalledOn";
            InstalledOn.ReadOnly = true;
            InstalledOn.Width = 150;
            // 
            // DatabaseName
            // 
            DatabaseName.DataPropertyName = "DatabaseName";
            DatabaseName.HeaderText = "Database";
            DatabaseName.MinimumWidth = 6;
            DatabaseName.Name = "DatabaseName";
            DatabaseName.ReadOnly = true;
            DatabaseName.Width = 200;
            // 
            // pathDataGridViewTextBoxColumn
            // 
            pathDataGridViewTextBoxColumn.DataPropertyName = "Path";
            pathDataGridViewTextBoxColumn.HeaderText = "Path";
            pathDataGridViewTextBoxColumn.MinimumWidth = 6;
            pathDataGridViewTextBoxColumn.Name = "pathDataGridViewTextBoxColumn";
            pathDataGridViewTextBoxColumn.ReadOnly = true;
            pathDataGridViewTextBoxColumn.Width = 300;
            // 
            // AspFilesTempPath
            // 
            AspFilesTempPath.DataPropertyName = "AspFilesTempPath";
            AspFilesTempPath.HeaderText = "Asp Temp Path";
            AspFilesTempPath.MinimumWidth = 6;
            AspFilesTempPath.Name = "AspFilesTempPath";
            AspFilesTempPath.ReadOnly = true;
            AspFilesTempPath.Width = 300;
            // 
            // RegistryPath
            // 
            RegistryPath.DataPropertyName = "RegistryPath";
            RegistryPath.HeaderText = "Registry Path";
            RegistryPath.MinimumWidth = 6;
            RegistryPath.Name = "RegistryPath";
            RegistryPath.ReadOnly = true;
            RegistryPath.Width = 300;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1642, 748);
            Controls.Add(statusStrip);
            Controls.Add(checkBoxJunk);
            Controls.Add(checkBoxActive);
            Controls.Add(labelOlderThan);
            Controls.Add(dateTimePicker);
            Controls.Add(buttonDeleteAll);
            Controls.Add(buttonDeleteSelected);
            Controls.Add(dataGridView);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "IIS Cleanup";
            Load += Form_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)iISAppBindingSource1).EndInit();
            ((System.ComponentModel.ISupportInitialize)iISAppBindingSource).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
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
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn Instance;
        private DataGridViewCheckBoxColumn Active;
        private DataGridViewTextBoxColumn InstalledOn;
        private DataGridViewTextBoxColumn DatabaseName;
        private DataGridViewTextBoxColumn pathDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn AspFilesTempPath;
        private DataGridViewTextBoxColumn RegistryPath;
    }
}
