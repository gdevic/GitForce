namespace Git4Win.Repo.Edit.Panels
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
            this.userControlEditGitignore = new Git4Win.UserControlEditGitignore();
            this.SuspendLayout();
            // 
            // userControlEditGitignore
            // 
            this.userControlEditGitignore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlEditGitignore.Location = new System.Drawing.Point(0, 0);
            this.userControlEditGitignore.Name = "userControlEditGitignore";
            this.userControlEditGitignore.Size = new System.Drawing.Size(300, 300);
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
