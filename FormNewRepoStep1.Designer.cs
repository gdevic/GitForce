namespace GitForce
{
    partial class FormNewRepoStep1
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
            this.btNext = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rbEmpty = new System.Windows.Forms.RadioButton();
            this.rbLocal = new System.Windows.Forms.RadioButton();
            this.rbRemote = new System.Windows.Forms.RadioButton();
            this.textBoxLocal = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btBrowse = new System.Windows.Forms.Button();
            this.folder = new System.Windows.Forms.FolderBrowserDialog();
            this.remoteDisplay = new GitForce.RemoteDisplay();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(369, 308);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 9;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btNext
            // 
            this.btNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btNext.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btNext.Location = new System.Drawing.Point(288, 308);
            this.btNext.Name = "btNext";
            this.btNext.Size = new System.Drawing.Size(75, 23);
            this.btNext.TabIndex = 8;
            this.btNext.Text = "Next >>";
            this.btNext.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(432, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select what kind of repository you are creating:";
            // 
            // rbEmpty
            // 
            this.rbEmpty.AutoSize = true;
            this.rbEmpty.Checked = true;
            this.rbEmpty.Location = new System.Drawing.Point(12, 45);
            this.rbEmpty.Name = "rbEmpty";
            this.rbEmpty.Size = new System.Drawing.Size(335, 17);
            this.rbEmpty.TabIndex = 1;
            this.rbEmpty.TabStop = true;
            this.rbEmpty.Tag = "empty";
            this.rbEmpty.Text = "New empty working repository (with or without your files already in)";
            this.rbEmpty.UseVisualStyleBackColor = true;
            this.rbEmpty.CheckedChanged += new System.EventHandler(this.RbSourceCheckedChanged);
            // 
            // rbLocal
            // 
            this.rbLocal.AutoSize = true;
            this.rbLocal.Location = new System.Drawing.Point(12, 71);
            this.rbLocal.Name = "rbLocal";
            this.rbLocal.Size = new System.Drawing.Size(311, 17);
            this.rbLocal.TabIndex = 2;
            this.rbLocal.Tag = "local";
            this.rbLocal.Text = "Clone of an existing local repository accessible by a file share";
            this.rbLocal.UseVisualStyleBackColor = true;
            this.rbLocal.CheckedChanged += new System.EventHandler(this.RbSourceCheckedChanged);
            // 
            // rbRemote
            // 
            this.rbRemote.AutoSize = true;
            this.rbRemote.Location = new System.Drawing.Point(12, 134);
            this.rbRemote.Name = "rbRemote";
            this.rbRemote.Size = new System.Drawing.Size(200, 17);
            this.rbRemote.TabIndex = 6;
            this.rbRemote.Tag = "remote";
            this.rbRemote.Text = "Clone of an existing remote repository";
            this.rbRemote.UseVisualStyleBackColor = true;
            this.rbRemote.CheckedChanged += new System.EventHandler(this.RbSourceCheckedChanged);
            // 
            // textBoxLocal
            // 
            this.textBoxLocal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLocal.Location = new System.Drawing.Point(32, 107);
            this.textBoxLocal.Name = "textBoxLocal";
            this.textBoxLocal.ReadOnly = true;
            this.textBoxLocal.Size = new System.Drawing.Size(331, 20);
            this.textBoxLocal.TabIndex = 4;
            this.textBoxLocal.TextChanged += new System.EventHandler(this.TextBoxLocalTextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(331, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Select the location containing a git repository you want to clone from:";
            // 
            // btBrowse
            // 
            this.btBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btBrowse.Enabled = false;
            this.btBrowse.Location = new System.Drawing.Point(369, 105);
            this.btBrowse.Name = "btBrowse";
            this.btBrowse.Size = new System.Drawing.Size(75, 23);
            this.btBrowse.TabIndex = 5;
            this.btBrowse.Text = "Browse...";
            this.btBrowse.UseVisualStyleBackColor = true;
            this.btBrowse.Click += new System.EventHandler(this.BtBrowseClick);
            // 
            // folder
            // 
            this.folder.ShowNewFolderButton = false;
            // 
            // remoteDisplay
            // 
            this.remoteDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.remoteDisplay.Location = new System.Drawing.Point(32, 153);
            this.remoteDisplay.MinimumSize = new System.Drawing.Size(309, 119);
            this.remoteDisplay.Name = "remoteDisplay";
            this.remoteDisplay.Size = new System.Drawing.Size(412, 148);
            this.remoteDisplay.TabIndex = 7;
            // 
            // FormNewRepoStep1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(456, 343);
            this.Controls.Add(this.remoteDisplay);
            this.Controls.Add(this.btBrowse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxLocal);
            this.Controls.Add(this.rbRemote);
            this.Controls.Add(this.rbLocal);
            this.Controls.Add(this.rbEmpty);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btNext);
            this.Controls.Add(this.btCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(384, 377);
            this.Name = "FormNewRepoStep1";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "New Repository (step 1 of 2)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormNewRepoStep1FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btNext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbEmpty;
        private System.Windows.Forms.RadioButton rbLocal;
        private System.Windows.Forms.RadioButton rbRemote;
        private System.Windows.Forms.TextBox textBoxLocal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btBrowse;
        private RemoteDisplay remoteDisplay;
        private System.Windows.Forms.FolderBrowserDialog folder;

    }
}