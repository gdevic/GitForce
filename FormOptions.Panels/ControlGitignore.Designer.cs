namespace git4win.FormOptions_Panels
{
    partial class ControlGitignore
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
            this.userControlEditGitignore = new git4win.UserControlEditGitignore();
            this.SuspendLayout();
            // 
            // userControlEditGitignore
            // 
            this.userControlEditGitignore.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.userControlEditGitignore.Location = new System.Drawing.Point(3, 3);
            this.userControlEditGitignore.Name = "userControlEditGitignore";
            this.userControlEditGitignore.Size = new System.Drawing.Size(294, 294);
            this.userControlEditGitignore.TabIndex = 0;
            // 
            // ControlGitignore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.userControlEditGitignore);
            this.Name = "ControlGitignore";
            this.Size = new System.Drawing.Size(300, 300);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControlEditGitignore userControlEditGitignore;
    }
}
