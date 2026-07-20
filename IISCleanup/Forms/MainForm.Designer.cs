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
            Id = new DataGridViewTextBoxColumn();
            Instance = new DataGridViewTextBoxColumn();
            Active = new DataGridViewCheckBoxColumn();
            InstalledOn = new DataGridViewTextBoxColumn();
            pathDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            AspFilesTempPath = new DataGridViewTextBoxColumn();
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
            dataGridView.Columns.AddRange(new DataGridViewColumn[] { Id, Instance, Active, InstalledOn, pathDataGridViewTextBoxColumn, AspFilesTempPath });
            dataGridView.DataSource = iISAppBindingSource1;
            dataGridView.Location = new Point(0, 41);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new Size(1184, 495);
            dataGridView.TabIndex = 2;
            // 
            // Id
            // 
            Id.DataPropertyName = "Id";
            Id.HeaderText = "#";
            Id.Name = "Id";
            Id.ReadOnly = true;
            Id.Width = 50;
            // 
            // Instance
            // 
            Instance.DataPropertyName = "Instance";
            Instance.HeaderText = "Instance";
            Instance.Name = "Instance";
            Instance.ReadOnly = true;
            Instance.Width = 150;
            // 
            // Active
            // 
            Active.DataPropertyName = "Active";
            Active.HeaderText = "Active/Junk";
            Active.Name = "Active";
            Active.ReadOnly = true;
            Active.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // InstalledOn
            // 
            InstalledOn.DataPropertyName = "InstalledOn";
            InstalledOn.HeaderText = "Installed On";
            InstalledOn.Name = "InstalledOn";
            InstalledOn.ReadOnly = true;
            InstalledOn.Width = 150;
            // 
            // pathDataGridViewTextBoxColumn
            // 
            pathDataGridViewTextBoxColumn.DataPropertyName = "Path";
            pathDataGridViewTextBoxColumn.HeaderText = "Path";
            pathDataGridViewTextBoxColumn.Name = "pathDataGridViewTextBoxColumn";
            pathDataGridViewTextBoxColumn.ReadOnly = true;
            pathDataGridViewTextBoxColumn.Width = 335;
            // 
            // AspFilesTempPath
            // 
            AspFilesTempPath.DataPropertyName = "AspFilesTempPath";
            AspFilesTempPath.HeaderText = "Asp Temp Path";
            AspFilesTempPath.Name = "AspFilesTempPath";
            AspFilesTempPath.ReadOnly = true;
            AspFilesTempPath.Width = 350;
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
            buttonDeleteSelected.Location = new Point(1016, 8);
            buttonDeleteSelected.Name = "buttonDeleteSelected";
            buttonDeleteSelected.Size = new Size(75, 23);
            buttonDeleteSelected.TabIndex = 3;
            buttonDeleteSelected.Text = "Delete";
            buttonDeleteSelected.UseVisualStyleBackColor = true;
            buttonDeleteSelected.Click += buttonDeleteSelected_Click;
            // 
            // buttonDeleteAll
            // 
            buttonDeleteAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonDeleteAll.Location = new Point(1097, 8);
            buttonDeleteAll.Name = "buttonDeleteAll";
            buttonDeleteAll.Size = new Size(75, 23);
            buttonDeleteAll.TabIndex = 4;
            buttonDeleteAll.Text = "Delete All";
            buttonDeleteAll.UseVisualStyleBackColor = true;
            buttonDeleteAll.Click += buttonDeleteAll_Click;
            // 
            // dateTimePicker
            // 
            dateTimePicker.Location = new Point(84, 8);
            dateTimePicker.Name = "dateTimePicker";
            dateTimePicker.Size = new Size(210, 23);
            dateTimePicker.TabIndex = 5;
            dateTimePicker.ValueChanged += dateTimePicker_ValueChanged;
            // 
            // labelOlderThan
            // 
            labelOlderThan.AutoSize = true;
            labelOlderThan.Location = new Point(12, 12);
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
            checkBoxActive.Location = new Point(300, 11);
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
            checkBoxJunk.Location = new Point(365, 11);
            checkBoxJunk.Name = "checkBoxJunk";
            checkBoxJunk.Size = new Size(50, 19);
            checkBoxJunk.TabIndex = 8;
            checkBoxJunk.Text = "Junk";
            checkBoxJunk.UseVisualStyleBackColor = true;
            checkBoxJunk.CheckedChanged += checkBoxJunk_CheckedChanged;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel, toolStripProgressBar });
            statusStrip.Location = new Point(0, 539);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1184, 22);
            statusStrip.TabIndex = 9;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(42, 17);
            toolStripStatusLabel.Text = "Status:";
            // 
            // toolStripProgressBar
            // 
            toolStripProgressBar.Name = "toolStripProgressBar";
            toolStripProgressBar.Size = new Size(100, 16);
            toolStripProgressBar.Style = ProgressBarStyle.Marquee;
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 561);
            Controls.Add(statusStrip);
            Controls.Add(checkBoxJunk);
            Controls.Add(checkBoxActive);
            Controls.Add(labelOlderThan);
            Controls.Add(dateTimePicker);
            Controls.Add(buttonDeleteAll);
            Controls.Add(buttonDeleteSelected);
            Controls.Add(dataGridView);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form";
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

        private ListBox AppsList;
        private Label labelIISApps;
        private DataGridView dataGridView;
        private BindingSource iISAppBindingSource;
        private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
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
        private DataGridViewTextBoxColumn pathDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn AspFilesTempPath;
    }
}
