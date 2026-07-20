namespace IISCleanup.Forms
{
    partial class DeleteConfirmationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            buttonCleanup = new Button();
            checkBoxCleanupFiles = new CheckBox();
            buttonCancel = new Button();
            checkBoxcleanupIISApps = new CheckBox();
            checkBoxCleanupAspTemp = new CheckBox();
            SuspendLayout();
            // 
            // buttonCleanup
            // 
            buttonCleanup.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCleanup.Location = new Point(166, 89);
            buttonCleanup.Name = "buttonCleanup";
            buttonCleanup.Size = new Size(75, 23);
            buttonCleanup.TabIndex = 4;
            buttonCleanup.Text = "Cleanup";
            buttonCleanup.UseVisualStyleBackColor = true;
            buttonCleanup.Click += buttonCleanup_Click;
            // 
            // checkBoxCleanupFiles
            // 
            checkBoxCleanupFiles.AutoSize = true;
            checkBoxCleanupFiles.Checked = true;
            checkBoxCleanupFiles.CheckState = CheckState.Checked;
            checkBoxCleanupFiles.Location = new Point(12, 37);
            checkBoxCleanupFiles.Name = "checkBoxCleanupFiles";
            checkBoxCleanupFiles.Size = new Size(137, 19);
            checkBoxCleanupFiles.TabIndex = 6;
            checkBoxCleanupFiles.Text = "Cleanup IIS apps files";
            checkBoxCleanupFiles.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Location = new Point(247, 89);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 7;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // checkBoxRemoveIISApps
            // 
            checkBoxcleanupIISApps.AutoSize = true;
            checkBoxcleanupIISApps.Checked = true;
            checkBoxcleanupIISApps.CheckState = CheckState.Checked;
            checkBoxcleanupIISApps.Location = new Point(12, 12);
            checkBoxcleanupIISApps.Name = "checkBoxRemoveIISApps";
            checkBoxcleanupIISApps.Size = new Size(113, 19);
            checkBoxcleanupIISApps.TabIndex = 8;
            checkBoxcleanupIISApps.Text = "Cleanup IIS apps";
            checkBoxcleanupIISApps.UseVisualStyleBackColor = true;
            // 
            // checkBoxCleanupAspTemp
            // 
            checkBoxCleanupAspTemp.AutoSize = true;
            checkBoxCleanupAspTemp.Checked = true;
            checkBoxCleanupAspTemp.CheckState = CheckState.Checked;
            checkBoxCleanupAspTemp.Location = new Point(12, 62);
            checkBoxCleanupAspTemp.Name = "checkBoxCleanupAspTemp";
            checkBoxCleanupAspTemp.Size = new Size(140, 19);
            checkBoxCleanupAspTemp.TabIndex = 9;
            checkBoxCleanupAspTemp.Text = "Cleanup IIS temp files";
            checkBoxCleanupAspTemp.UseVisualStyleBackColor = true;
            // 
            // DeleteConfirmationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(334, 124);
            Controls.Add(checkBoxCleanupAspTemp);
            Controls.Add(checkBoxcleanupIISApps);
            Controls.Add(buttonCancel);
            Controls.Add(checkBoxCleanupFiles);
            Controls.Add(buttonCleanup);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "DeleteConfirmationForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Confirm cleanup";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button2;
        private Button buttonCleanup;
        private GroupBox groupBoxIISJunk;
        private CheckBox checkBoxCleanupFiles;
        private Button buttonCancel;
        private CheckBox checkBoxcleanupIISApps;
        private CheckBox checkBoxCleanupAspTemp;
    }
}