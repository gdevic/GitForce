namespace GitForce
{
    partial class FormSubmoduleInfo
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
            this.labelName = new System.Windows.Forms.Label();
            this.labelPath = new System.Windows.Forms.Label();
            this.labelUrl = new System.Windows.Forms.Label();
            this.labelSha = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.textName = new System.Windows.Forms.TextBox();
            this.textPath = new System.Windows.Forms.TextBox();
            this.textUrl = new System.Windows.Forms.TextBox();
            this.textSha = new System.Windows.Forms.TextBox();
            this.textStatus = new System.Windows.Forms.TextBox();
            this.btCopyUrl = new System.Windows.Forms.Button();
            this.btCopyPath = new System.Windows.Forms.Button();
            this.btCopySha = new System.Windows.Forms.Button();
            this.btClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(12, 15);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(38, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name:";
            // 
            // labelPath
            // 
            this.labelPath.AutoSize = true;
            this.labelPath.Location = new System.Drawing.Point(12, 41);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(32, 13);
            this.labelPath.TabIndex = 1;
            this.labelPath.Text = "Path:";
            // 
            // labelUrl
            // 
            this.labelUrl.AutoSize = true;
            this.labelUrl.Location = new System.Drawing.Point(12, 67);
            this.labelUrl.Name = "labelUrl";
            this.labelUrl.Size = new System.Drawing.Size(32, 13);
            this.labelUrl.TabIndex = 2;
            this.labelUrl.Text = "URL:";
            // 
            // labelSha
            // 
            this.labelSha.AutoSize = true;
            this.labelSha.Location = new System.Drawing.Point(12, 93);
            this.labelSha.Name = "labelSha";
            this.labelSha.Size = new System.Drawing.Size(44, 13);
            this.labelSha.TabIndex = 3;
            this.labelSha.Text = "Commit:";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(12, 119);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(40, 13);
            this.labelStatus.TabIndex = 4;
            this.labelStatus.Text = "Status:";
            // 
            // textName
            // 
            this.textName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textName.Location = new System.Drawing.Point(62, 12);
            this.textName.Name = "textName";
            this.textName.ReadOnly = true;
            this.textName.Size = new System.Drawing.Size(310, 20);
            this.textName.TabIndex = 5;
            // 
            // textPath
            // 
            this.textPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textPath.Location = new System.Drawing.Point(62, 38);
            this.textPath.Name = "textPath";
            this.textPath.ReadOnly = true;
            this.textPath.Size = new System.Drawing.Size(260, 20);
            this.textPath.TabIndex = 6;
            // 
            // textUrl
            // 
            this.textUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textUrl.Location = new System.Drawing.Point(62, 64);
            this.textUrl.Name = "textUrl";
            this.textUrl.ReadOnly = true;
            this.textUrl.Size = new System.Drawing.Size(260, 20);
            this.textUrl.TabIndex = 7;
            // 
            // textSha
            // 
            this.textSha.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSha.Location = new System.Drawing.Point(62, 90);
            this.textSha.Name = "textSha";
            this.textSha.ReadOnly = true;
            this.textSha.Size = new System.Drawing.Size(260, 20);
            this.textSha.TabIndex = 8;
            // 
            // textStatus
            // 
            this.textStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textStatus.Location = new System.Drawing.Point(62, 116);
            this.textStatus.Name = "textStatus";
            this.textStatus.ReadOnly = true;
            this.textStatus.Size = new System.Drawing.Size(310, 20);
            this.textStatus.TabIndex = 9;
            // 
            // btCopyUrl
            // 
            this.btCopyUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCopyUrl.Location = new System.Drawing.Point(328, 62);
            this.btCopyUrl.Name = "btCopyUrl";
            this.btCopyUrl.Size = new System.Drawing.Size(44, 23);
            this.btCopyUrl.TabIndex = 11;
            this.btCopyUrl.Text = "Copy";
            this.btCopyUrl.UseVisualStyleBackColor = true;
            this.btCopyUrl.Click += new System.EventHandler(this.BtCopyUrlClick);
            // 
            // btCopyPath
            // 
            this.btCopyPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCopyPath.Location = new System.Drawing.Point(328, 36);
            this.btCopyPath.Name = "btCopyPath";
            this.btCopyPath.Size = new System.Drawing.Size(44, 23);
            this.btCopyPath.TabIndex = 10;
            this.btCopyPath.Text = "Copy";
            this.btCopyPath.UseVisualStyleBackColor = true;
            this.btCopyPath.Click += new System.EventHandler(this.BtCopyPathClick);
            //
            // btCopySha
            //
            this.btCopySha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCopySha.Location = new System.Drawing.Point(328, 88);
            this.btCopySha.Name = "btCopySha";
            this.btCopySha.Size = new System.Drawing.Size(44, 23);
            this.btCopySha.TabIndex = 12;
            this.btCopySha.Text = "Copy";
            this.btCopySha.UseVisualStyleBackColor = true;
            this.btCopySha.Click += new System.EventHandler(this.BtCopyShaClick);
            //
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btClose.Location = new System.Drawing.Point(297, 151);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(75, 23);
            this.btClose.TabIndex = 14;
            this.btClose.Text = "Close";
            this.btClose.UseVisualStyleBackColor = true;
            // 
            // FormSubmoduleInfo
            // 
            this.AcceptButton = this.btClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btClose;
            this.ClientSize = new System.Drawing.Size(384, 186);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.btCopySha);
            this.Controls.Add(this.btCopyUrl);
            this.Controls.Add(this.btCopyPath);
            this.Controls.Add(this.textStatus);
            this.Controls.Add(this.textSha);
            this.Controls.Add(this.textUrl);
            this.Controls.Add(this.textPath);
            this.Controls.Add(this.textName);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelSha);
            this.Controls.Add(this.labelUrl);
            this.Controls.Add(this.labelPath);
            this.Controls.Add(this.labelName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 225);
            this.Name = "FormSubmoduleInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Submodule Information";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSubmoduleInfoFormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelPath;
        private System.Windows.Forms.Label labelUrl;
        private System.Windows.Forms.Label labelSha;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.TextBox textPath;
        private System.Windows.Forms.TextBox textUrl;
        private System.Windows.Forms.TextBox textSha;
        private System.Windows.Forms.TextBox textStatus;
        private System.Windows.Forms.Button btCopyUrl;
        private System.Windows.Forms.Button btCopyPath;
        private System.Windows.Forms.Button btCopySha;
        private System.Windows.Forms.Button btClose;
    }
}
