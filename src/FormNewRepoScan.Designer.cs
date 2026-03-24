namespace GitForce
{
    partial class FormNewRepoScan
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
            this.btCancel = new System.Windows.Forms.Button();
            this.btAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textRoot = new System.Windows.Forms.TextBox();
            this.btBrowse = new System.Windows.Forms.Button();
            this.listRepos = new System.Windows.Forms.CheckedListBox();
            this.checkBoxDeepScan = new System.Windows.Forms.CheckBox();
            this.btSelectAll = new System.Windows.Forms.Button();
            this.btSelectNone = new System.Windows.Forms.Button();
            this.btScan = new System.Windows.Forms.Button();
            this.folderDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(323, 293);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 0;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btAdd
            // 
            this.btAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAdd.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btAdd.Enabled = false;
            this.btAdd.Location = new System.Drawing.Point(242, 293);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(75, 23);
            this.btAdd.TabIndex = 1;
            this.btAdd.Text = "Add";
            this.btAdd.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(386, 32);
            this.label1.TabIndex = 2;
            this.label1.Text = "Scan the local file system and find all existing Git repositories. Select the roo" +
    "t directory and start scannng, then select which ones to add to your workspace.";
            // 
            // textRoot
            // 
            this.textRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textRoot.Location = new System.Drawing.Point(12, 46);
            this.textRoot.Name = "textRoot";
            this.textRoot.Size = new System.Drawing.Size(305, 20);
            this.textRoot.TabIndex = 3;
            this.textRoot.TextChanged += new System.EventHandler(this.TextRootTextChanged);
            // 
            // btBrowse
            // 
            this.btBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btBrowse.Location = new System.Drawing.Point(323, 44);
            this.btBrowse.Name = "btBrowse";
            this.btBrowse.Size = new System.Drawing.Size(75, 23);
            this.btBrowse.TabIndex = 4;
            this.btBrowse.Text = "Browse...";
            this.btBrowse.UseVisualStyleBackColor = true;
            this.btBrowse.Click += new System.EventHandler(this.BtBrowseClick);
            // 
            // listRepos
            // 
            this.listRepos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listRepos.CheckOnClick = true;
            this.listRepos.FormattingEnabled = true;
            this.listRepos.IntegralHeight = false;
            this.listRepos.Location = new System.Drawing.Point(12, 102);
            this.listRepos.Name = "listRepos";
            this.listRepos.Size = new System.Drawing.Size(386, 185);
            this.listRepos.TabIndex = 5;
            this.listRepos.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ListReposItemCheck);
            // 
            // checkBoxDeepScan
            // 
            this.checkBoxDeepScan.AutoSize = true;
            this.checkBoxDeepScan.Location = new System.Drawing.Point(12, 77);
            this.checkBoxDeepScan.Name = "checkBoxDeepScan";
            this.checkBoxDeepScan.Size = new System.Drawing.Size(279, 17);
            this.checkBoxDeepScan.TabIndex = 6;
            this.checkBoxDeepScan.Text = "Find all nested repositories";
            this.checkBoxDeepScan.UseVisualStyleBackColor = true;
            // 
            // btSelectAll
            // 
            this.btSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btSelectAll.Enabled = false;
            this.btSelectAll.Location = new System.Drawing.Point(12, 293);
            this.btSelectAll.Name = "btSelectAll";
            this.btSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btSelectAll.TabIndex = 7;
            this.btSelectAll.Text = "Select All";
            this.btSelectAll.UseVisualStyleBackColor = true;
            this.btSelectAll.Click += new System.EventHandler(this.BtSelectAllClick);
            // 
            // btSelectNone
            // 
            this.btSelectNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btSelectNone.Enabled = false;
            this.btSelectNone.Location = new System.Drawing.Point(93, 293);
            this.btSelectNone.Name = "btSelectNone";
            this.btSelectNone.Size = new System.Drawing.Size(75, 23);
            this.btSelectNone.TabIndex = 8;
            this.btSelectNone.Text = "Select None";
            this.btSelectNone.UseVisualStyleBackColor = true;
            this.btSelectNone.Click += new System.EventHandler(this.BtSelectNoneClick);
            // 
            // btScan
            // 
            this.btScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btScan.Enabled = false;
            this.btScan.Location = new System.Drawing.Point(323, 73);
            this.btScan.Name = "btScan";
            this.btScan.Size = new System.Drawing.Size(75, 23);
            this.btScan.TabIndex = 9;
            this.btScan.Text = "Scan";
            this.btScan.UseVisualStyleBackColor = true;
            this.btScan.Click += new System.EventHandler(this.BtScanClick);
            // 
            // folderDlg
            // 
            this.folderDlg.Description = "Select the root folder to start scanning for the repos:";
            this.folderDlg.ShowNewFolderButton = false;
            // 
            // FormNewRepoScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 328);
            this.Controls.Add(this.btScan);
            this.Controls.Add(this.btSelectNone);
            this.Controls.Add(this.btSelectAll);
            this.Controls.Add(this.checkBoxDeepScan);
            this.Controls.Add(this.listRepos);
            this.Controls.Add(this.btBrowse);
            this.Controls.Add(this.textRoot);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btAdd);
            this.Controls.Add(this.btCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(356, 211);
            this.Name = "FormNewRepoScan";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Repository Scan";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormNewRepoScanFormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textRoot;
        private System.Windows.Forms.Button btBrowse;
        private System.Windows.Forms.CheckedListBox listRepos;
        private System.Windows.Forms.CheckBox checkBoxDeepScan;
        private System.Windows.Forms.Button btSelectAll;
        private System.Windows.Forms.Button btSelectNone;
        private System.Windows.Forms.Button btScan;
        private System.Windows.Forms.FolderBrowserDialog folderDlg;
    }
}