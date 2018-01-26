namespace GitForce.Settings.Panels
{
    partial class ControlOptions
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBoxTabs = new System.Windows.Forms.CheckBox();
            this.checkBoxWarnMultipleInstances = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoCloseGitOnSuccess = new System.Windows.Forms.CheckBox();
            this.checkBoxWarnIfAdmin = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBoxTabs
            // 
            this.checkBoxTabs.AutoSize = true;
            this.checkBoxTabs.Location = new System.Drawing.Point(3, 3);
            this.checkBoxTabs.Name = "checkBoxTabs";
            this.checkBoxTabs.Size = new System.Drawing.Size(159, 17);
            this.checkBoxTabs.TabIndex = 0;
            this.checkBoxTabs.Text = "Show tabs on the right pane";
            this.checkBoxTabs.UseVisualStyleBackColor = true;
            // 
            // checkBoxWarnMultipleInstances
            // 
            this.checkBoxWarnMultipleInstances.AutoSize = true;
            this.checkBoxWarnMultipleInstances.Location = new System.Drawing.Point(3, 26);
            this.checkBoxWarnMultipleInstances.Name = "checkBoxWarnMultipleInstances";
            this.checkBoxWarnMultipleInstances.Size = new System.Drawing.Size(268, 17);
            this.checkBoxWarnMultipleInstances.TabIndex = 1;
            this.checkBoxWarnMultipleInstances.Text = "&Warn before starting additional instance of GitForce";
            this.checkBoxWarnMultipleInstances.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoCloseGitOnSuccess
            // 
            this.checkBoxAutoCloseGitOnSuccess.AutoSize = true;
            this.checkBoxAutoCloseGitOnSuccess.Location = new System.Drawing.Point(3, 72);
            this.checkBoxAutoCloseGitOnSuccess.Name = "checkBoxAutoCloseGitOnSuccess";
            this.checkBoxAutoCloseGitOnSuccess.Size = new System.Drawing.Size(211, 17);
            this.checkBoxAutoCloseGitOnSuccess.TabIndex = 3;
            this.checkBoxAutoCloseGitOnSuccess.Text = "&Close git command window on success";
            this.checkBoxAutoCloseGitOnSuccess.UseVisualStyleBackColor = true;
            // 
            // checkBoxWarnIfAdmin
            // 
            this.checkBoxWarnIfAdmin.AutoSize = true;
            this.checkBoxWarnIfAdmin.Location = new System.Drawing.Point(3, 49);
            this.checkBoxWarnIfAdmin.Name = "checkBoxWarnIfAdmin";
            this.checkBoxWarnIfAdmin.Size = new System.Drawing.Size(143, 17);
            this.checkBoxWarnIfAdmin.TabIndex = 2;
            this.checkBoxWarnIfAdmin.Text = "Warn if starting as &Admin";
            this.checkBoxWarnIfAdmin.UseVisualStyleBackColor = true;
            // 
            // ControlOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxWarnIfAdmin);
            this.Controls.Add(this.checkBoxAutoCloseGitOnSuccess);
            this.Controls.Add(this.checkBoxTabs);
            this.Controls.Add(this.checkBoxWarnMultipleInstances);
            this.Name = "ControlOptions";
            this.Size = new System.Drawing.Size(300, 300);
            this.Tag = "Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxTabs;
        private System.Windows.Forms.CheckBox checkBoxWarnMultipleInstances;
        private System.Windows.Forms.CheckBox checkBoxAutoCloseGitOnSuccess;
        private System.Windows.Forms.CheckBox checkBoxWarnIfAdmin;
    }
}
