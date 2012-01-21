namespace GitForce
{
    partial class FormAbout
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
            this.btOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.textLic = new System.Windows.Forms.TextBox();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.linkLabel = new System.Windows.Forms.LinkLabel();
            this.labelBuild = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btCopyEmail = new System.Windows.Forms.Button();
            this.linkEmail = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.labelNewVersionAvailable = new System.Windows.Forms.Label();
            this.btDownload = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(356, 251);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "A GIT Visual Client";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.Location = new System.Drawing.Point(12, 37);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(53, 13);
            this.labelVersion.TabIndex = 3;
            this.labelVersion.Text = "Version:";
            // 
            // textLic
            // 
            this.textLic.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textLic.Location = new System.Drawing.Point(12, 160);
            this.textLic.Multiline = true;
            this.textLic.Name = "textLic";
            this.textLic.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textLic.Size = new System.Drawing.Size(419, 85);
            this.textLic.TabIndex = 4;
            // 
            // labelCopyright
            // 
            this.labelCopyright.Location = new System.Drawing.Point(6, 21);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(309, 13);
            this.labelCopyright.TabIndex = 5;
            this.labelCopyright.Text = "Written by Goran Devic, released under GPL";
            // 
            // linkLabel
            // 
            this.linkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel.AutoSize = true;
            this.linkLabel.Location = new System.Drawing.Point(403, 12);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new System.Drawing.Size(28, 13);
            this.linkLabel.TabIndex = 6;
            this.linkLabel.TabStop = true;
            this.linkLabel.Tag = "http://www.gnu.org/licenses/gpl.html";
            this.linkLabel.Text = "GPL";
            this.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelLinkClicked);
            // 
            // labelBuild
            // 
            this.labelBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBuild.AutoSize = true;
            this.labelBuild.Location = new System.Drawing.Point(234, 37);
            this.labelBuild.Name = "labelBuild";
            this.labelBuild.Size = new System.Drawing.Size(30, 13);
            this.labelBuild.TabIndex = 7;
            this.labelBuild.Text = "Build";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btCopyEmail);
            this.groupBox1.Controls.Add(this.linkEmail);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.labelCopyright);
            this.groupBox1.Location = new System.Drawing.Point(12, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(419, 92);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Contact information";
            // 
            // btCopyEmail
            // 
            this.btCopyEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCopyEmail.Location = new System.Drawing.Point(344, 63);
            this.btCopyEmail.Name = "btCopyEmail";
            this.btCopyEmail.Size = new System.Drawing.Size(69, 23);
            this.btCopyEmail.TabIndex = 8;
            this.btCopyEmail.Text = "Copy Email";
            this.btCopyEmail.UseVisualStyleBackColor = true;
            this.btCopyEmail.Click += new System.EventHandler(this.btCopyEmail_Click);
            // 
            // linkEmail
            // 
            this.linkEmail.AutoSize = true;
            this.linkEmail.Location = new System.Drawing.Point(7, 62);
            this.linkEmail.Name = "linkEmail";
            this.linkEmail.Size = new System.Drawing.Size(141, 13);
            this.linkEmail.TabIndex = 7;
            this.linkEmail.TabStop = true;
            this.linkEmail.Tag = "mailto:GitForce.Project@gmail.com";
            this.linkEmail.Text = "GitForce.Project@gmail.com";
            this.linkEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelLinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(353, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "For comments and suggestions, including bug reports, please email me at:";
            // 
            // labelNewVersionAvailable
            // 
            this.labelNewVersionAvailable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelNewVersionAvailable.AutoSize = true;
            this.labelNewVersionAvailable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNewVersionAvailable.ForeColor = System.Drawing.Color.Brown;
            this.labelNewVersionAvailable.Location = new System.Drawing.Point(12, 256);
            this.labelNewVersionAvailable.Name = "labelNewVersionAvailable";
            this.labelNewVersionAvailable.Size = new System.Drawing.Size(226, 13);
            this.labelNewVersionAvailable.TabIndex = 0;
            this.labelNewVersionAvailable.Text = "A new version of GitForce is available!";
            // 
            // btDownload
            // 
            this.btDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btDownload.Location = new System.Drawing.Point(278, 251);
            this.btDownload.Name = "btDownload";
            this.btDownload.Size = new System.Drawing.Size(69, 23);
            this.btDownload.TabIndex = 1;
            this.btDownload.Text = "Download";
            this.btDownload.UseVisualStyleBackColor = true;
            this.btDownload.Click += new System.EventHandler(this.DownloadClick);
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 286);
            this.Controls.Add(this.btDownload);
            this.Controls.Add(this.labelNewVersionAvailable);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelBuild);
            this.Controls.Add(this.textLic);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.linkLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(443, 253);
            this.Name = "FormAbout";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "About GitForce";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAboutFormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.TextBox textLic;
        private System.Windows.Forms.Label labelCopyright;
        private System.Windows.Forms.LinkLabel linkLabel;
        private System.Windows.Forms.Label labelBuild;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel linkEmail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btCopyEmail;
        private System.Windows.Forms.Button btDownload;
        private System.Windows.Forms.Label labelNewVersionAvailable;
    }
}