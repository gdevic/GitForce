namespace GitForce.Repo.Edit.Panels
{
    partial class ControlGitconfig
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
            this.userControlEditFile = new GitForce.UserControlEditFile();
            this.SuspendLayout();
            // 
            // userControlEditFile
            // 
            this.userControlEditFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlEditFile.Location = new System.Drawing.Point(0, 0);
            this.userControlEditFile.Name = "userControlEditFile";
            this.userControlEditFile.Size = new System.Drawing.Size(300, 300);
            this.userControlEditFile.TabIndex = 0;
            this.userControlEditFile.Tag = "";
            // 
            // ControlGitconfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.userControlEditFile);
            this.Name = "ControlGitconfig";
            this.Size = new System.Drawing.Size(300, 300);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControlEditFile userControlEditFile;
    }
}
