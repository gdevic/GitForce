namespace GitForce.Settings.Panels
{
    partial class ControlFiles
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
            this.checkBoxIgnoreCase = new System.Windows.Forms.CheckBox();
            this.checkBoxShowDotGit = new System.Windows.Forms.CheckBox();
            this.checkBoxDeepScan = new System.Windows.Forms.CheckBox();
            this.checkBoxRefreshOnChange = new System.Windows.Forms.CheckBox();
            this.checkBoxReaddOnChange = new System.Windows.Forms.CheckBox();
            this.checkBoxScanTabs = new System.Windows.Forms.CheckBox();
            this.textBoxScanExt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // checkBoxIgnoreCase
            // 
            this.checkBoxIgnoreCase.AutoSize = true;
            this.checkBoxIgnoreCase.Location = new System.Drawing.Point(3, 3);
            this.checkBoxIgnoreCase.Name = "checkBoxIgnoreCase";
            this.checkBoxIgnoreCase.Size = new System.Drawing.Size(256, 17);
            this.checkBoxIgnoreCase.TabIndex = 0;
            this.checkBoxIgnoreCase.Text = "Ignore file name case making git case-insensitive";
            this.checkBoxIgnoreCase.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowDotGit
            // 
            this.checkBoxShowDotGit.AutoSize = true;
            this.checkBoxShowDotGit.Location = new System.Drawing.Point(3, 26);
            this.checkBoxShowDotGit.Name = "checkBoxShowDotGit";
            this.checkBoxShowDotGit.Size = new System.Drawing.Size(225, 17);
            this.checkBoxShowDotGit.TabIndex = 1;
            this.checkBoxShowDotGit.Text = "Reveal .git folders in Local File View mode";
            this.checkBoxShowDotGit.UseVisualStyleBackColor = true;
            // 
            // checkBoxDeepScan
            // 
            this.checkBoxDeepScan.AutoSize = true;
            this.checkBoxDeepScan.Location = new System.Drawing.Point(3, 49);
            this.checkBoxDeepScan.Name = "checkBoxDeepScan";
            this.checkBoxDeepScan.Size = new System.Drawing.Size(266, 17);
            this.checkBoxDeepScan.TabIndex = 2;
            this.checkBoxDeepScan.Text = "Perform deep Repo Scan (able to find submodules)";
            this.checkBoxDeepScan.UseVisualStyleBackColor = true;
            // 
            // checkBoxRefreshOnChange
            // 
            this.checkBoxRefreshOnChange.AutoSize = true;
            this.checkBoxRefreshOnChange.Location = new System.Drawing.Point(3, 72);
            this.checkBoxRefreshOnChange.Name = "checkBoxRefreshOnChange";
            this.checkBoxRefreshOnChange.Size = new System.Drawing.Size(250, 17);
            this.checkBoxRefreshOnChange.TabIndex = 3;
            this.checkBoxRefreshOnChange.Text = "Automatically refresh if any index file is changed";
            this.checkBoxRefreshOnChange.UseVisualStyleBackColor = true;
            this.checkBoxRefreshOnChange.CheckedChanged += new System.EventHandler(this.CheckBoxRefreshOnChangeCheckedChanged);
            // 
            // checkBoxReaddOnChange
            // 
            this.checkBoxReaddOnChange.AutoSize = true;
            this.checkBoxReaddOnChange.Location = new System.Drawing.Point(30, 95);
            this.checkBoxReaddOnChange.Name = "checkBoxReaddOnChange";
            this.checkBoxReaddOnChange.Size = new System.Drawing.Size(176, 17);
            this.checkBoxReaddOnChange.TabIndex = 4;
            this.checkBoxReaddOnChange.Text = "Re-add files to index on change";
            this.checkBoxReaddOnChange.UseVisualStyleBackColor = true;
            // 
            // checkBoxScanTabs
            // 
            this.checkBoxScanTabs.AutoSize = true;
            this.checkBoxScanTabs.Location = new System.Drawing.Point(3, 117);
            this.checkBoxScanTabs.Name = "checkBoxScanTabs";
            this.checkBoxScanTabs.Size = new System.Drawing.Size(278, 17);
            this.checkBoxScanTabs.TabIndex = 5;
            this.checkBoxScanTabs.Text = "Warn when files to add contain TABs or EOL spaces:";
            this.checkBoxScanTabs.UseVisualStyleBackColor = true;
            this.checkBoxScanTabs.CheckedChanged += new System.EventHandler(this.CheckBoxScanTabsCheckedChanged);
            // 
            // textBoxScanExt
            // 
            this.textBoxScanExt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxScanExt.Location = new System.Drawing.Point(30, 140);
            this.textBoxScanExt.Name = "textBoxScanExt";
            this.textBoxScanExt.Size = new System.Drawing.Size(239, 20);
            this.textBoxScanExt.TabIndex = 6;
            // 
            // ControlFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxScanExt);
            this.Controls.Add(this.checkBoxScanTabs);
            this.Controls.Add(this.checkBoxReaddOnChange);
            this.Controls.Add(this.checkBoxRefreshOnChange);
            this.Controls.Add(this.checkBoxDeepScan);
            this.Controls.Add(this.checkBoxShowDotGit);
            this.Controls.Add(this.checkBoxIgnoreCase);
            this.Name = "ControlFiles";
            this.Size = new System.Drawing.Size(300, 300);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxIgnoreCase;
        private System.Windows.Forms.CheckBox checkBoxShowDotGit;
        private System.Windows.Forms.CheckBox checkBoxDeepScan;
        private System.Windows.Forms.CheckBox checkBoxRefreshOnChange;
        private System.Windows.Forms.CheckBox checkBoxReaddOnChange;
        private System.Windows.Forms.CheckBox checkBoxScanTabs;
        private System.Windows.Forms.TextBox textBoxScanExt;
    }
}
