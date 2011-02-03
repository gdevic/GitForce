namespace git4win
{
    partial class FormNewRepoStep2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNewRepoStep2));
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.btPrev = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxRepoPath = new System.Windows.Forms.TextBox();
            this.btBrowse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxExtraArgs = new System.Windows.Forms.TextBox();
            this.folderDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.checkBoxBare = new System.Windows.Forms.CheckBox();
            this.groupBoxSsh = new System.Windows.Forms.GroupBox();
            this.btMyKeys = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxSsh.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(393, 293);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 0;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Enabled = false;
            this.btOK.Location = new System.Drawing.Point(312, 293);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 1;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            // 
            // btPrev
            // 
            this.btPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btPrev.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.btPrev.Location = new System.Drawing.Point(231, 293);
            this.btPrev.Name = "btPrev";
            this.btPrev.Size = new System.Drawing.Size(75, 23);
            this.btPrev.TabIndex = 2;
            this.btPrev.Text = "<< Prev";
            this.btPrev.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(456, 36);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select or create a local folder to host the repository root. This folder needs to" +
                " be empty for clone operation.";
            // 
            // textBoxRepoPath
            // 
            this.textBoxRepoPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRepoPath.Location = new System.Drawing.Point(12, 67);
            this.textBoxRepoPath.Name = "textBoxRepoPath";
            this.textBoxRepoPath.Size = new System.Drawing.Size(375, 20);
            this.textBoxRepoPath.TabIndex = 4;
            this.textBoxRepoPath.TextChanged += new System.EventHandler(this.textBoxRepoPath_TextChanged);
            // 
            // btBrowse
            // 
            this.btBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btBrowse.Location = new System.Drawing.Point(393, 65);
            this.btBrowse.Name = "btBrowse";
            this.btBrowse.Size = new System.Drawing.Size(75, 23);
            this.btBrowse.TabIndex = 5;
            this.btBrowse.Text = "Browse...";
            this.btBrowse.UseVisualStyleBackColor = true;
            this.btBrowse.Click += new System.EventHandler(this.btBrowse_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(304, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "You can optionally specify extra arguments to git init / git clone:";
            // 
            // textBoxExtraArgs
            // 
            this.textBoxExtraArgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxExtraArgs.Location = new System.Drawing.Point(12, 144);
            this.textBoxExtraArgs.Name = "textBoxExtraArgs";
            this.textBoxExtraArgs.Size = new System.Drawing.Size(375, 20);
            this.textBoxExtraArgs.TabIndex = 7;
            this.textBoxExtraArgs.TextChanged += new System.EventHandler(this.textBoxExtraArgs_TextChanged);
            // 
            // checkBoxBare
            // 
            this.checkBoxBare.AutoSize = true;
            this.checkBoxBare.Location = new System.Drawing.Point(12, 98);
            this.checkBoxBare.Name = "checkBoxBare";
            this.checkBoxBare.Size = new System.Drawing.Size(312, 17);
            this.checkBoxBare.TabIndex = 8;
            this.checkBoxBare.Text = "Create a shared or \"bare\" repository with no working files in it";
            this.checkBoxBare.UseVisualStyleBackColor = true;
            this.checkBoxBare.CheckedChanged += new System.EventHandler(this.checkBoxBare_CheckedChanged);
            // 
            // groupBoxSsh
            // 
            this.groupBoxSsh.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSsh.Controls.Add(this.btMyKeys);
            this.groupBoxSsh.Controls.Add(this.label3);
            this.groupBoxSsh.Location = new System.Drawing.Point(12, 170);
            this.groupBoxSsh.Name = "groupBoxSsh";
            this.groupBoxSsh.Size = new System.Drawing.Size(456, 117);
            this.groupBoxSsh.TabIndex = 9;
            this.groupBoxSsh.TabStop = false;
            this.groupBoxSsh.Text = "SSH";
            // 
            // btMyKeys
            // 
            this.btMyKeys.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btMyKeys.Image = ((System.Drawing.Image)(resources.GetObject("btMyKeys.Image")));
            this.btMyKeys.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btMyKeys.Location = new System.Drawing.Point(9, 88);
            this.btMyKeys.Name = "btMyKeys";
            this.btMyKeys.Size = new System.Drawing.Size(144, 23);
            this.btMyKeys.TabIndex = 1;
            this.btMyKeys.Text = "Manage PuTTY Keys...";
            this.btMyKeys.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btMyKeys.UseVisualStyleBackColor = true;
            this.btMyKeys.Click += new System.EventHandler(this.btPageant_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.SystemColors.Info;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(444, 39);
            this.label3.TabIndex = 0;
            this.label3.Text = "If you are cloning a remote repo using the SSH connection, make sure your key has" +
                " been loaded into Pageant process.";
            // 
            // FormNewRepoStep2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 328);
            this.Controls.Add(this.groupBoxSsh);
            this.Controls.Add(this.checkBoxBare);
            this.Controls.Add(this.textBoxExtraArgs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btBrowse);
            this.Controls.Add(this.textBoxRepoPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btPrev);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(349, 336);
            this.Name = "FormNewRepoStep2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Repository (step 2 of 2)";
            this.groupBoxSsh.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btPrev;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxRepoPath;
        private System.Windows.Forms.Button btBrowse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxExtraArgs;
        private System.Windows.Forms.FolderBrowserDialog folderDlg;
        private System.Windows.Forms.CheckBox checkBoxBare;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btMyKeys;
        private System.Windows.Forms.GroupBox groupBoxSsh;
    }
}