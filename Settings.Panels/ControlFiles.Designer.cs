namespace Git4Win.Settings.Panels
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
            // ControlFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
    }
}
