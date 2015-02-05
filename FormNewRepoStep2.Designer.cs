namespace GitForce
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
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.btPrev = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxRepoPath = new System.Windows.Forms.TextBox();
            this.btBrowse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxExtraArgs = new System.Windows.Forms.TextBox();
            this.folder = new System.Windows.Forms.FolderBrowserDialog();
            this.checkBoxBare = new System.Windows.Forms.CheckBox();
            this.labelCloneOperation = new System.Windows.Forms.Label();
            this.textBoxProjectName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(369, 308);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 7;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Enabled = false;
            this.btOK.Location = new System.Drawing.Point(288, 308);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 6;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            // 
            // btPrev
            // 
            this.btPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btPrev.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.btPrev.Location = new System.Drawing.Point(207, 308);
            this.btPrev.Name = "btPrev";
            this.btPrev.Size = new System.Drawing.Size(75, 23);
            this.btPrev.TabIndex = 5;
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
            this.label1.Size = new System.Drawing.Size(432, 33);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select or create a local folder to host the repository root. The final folder nee" +
    "ds to be empty for a repo clone operation.";
            // 
            // textBoxRepoPath
            // 
            this.textBoxRepoPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRepoPath.Location = new System.Drawing.Point(12, 48);
            this.textBoxRepoPath.Name = "textBoxRepoPath";
            this.textBoxRepoPath.Size = new System.Drawing.Size(351, 20);
            this.textBoxRepoPath.TabIndex = 0;
            this.textBoxRepoPath.TextChanged += new System.EventHandler(this.TextBoxRepoPathTextChanged);
            // 
            // btBrowse
            // 
            this.btBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btBrowse.Location = new System.Drawing.Point(369, 45);
            this.btBrowse.Name = "btBrowse";
            this.btBrowse.Size = new System.Drawing.Size(75, 23);
            this.btBrowse.TabIndex = 1;
            this.btBrowse.Text = "Browse...";
            this.btBrowse.UseVisualStyleBackColor = true;
            this.btBrowse.Click += new System.EventHandler(this.BtBrowseClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(304, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "You can optionally specify extra arguments to git init / git clone:";
            // 
            // textBoxExtraArgs
            // 
            this.textBoxExtraArgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxExtraArgs.Location = new System.Drawing.Point(12, 152);
            this.textBoxExtraArgs.Name = "textBoxExtraArgs";
            this.textBoxExtraArgs.Size = new System.Drawing.Size(351, 20);
            this.textBoxExtraArgs.TabIndex = 4;
            // 
            // checkBoxBare
            // 
            this.checkBoxBare.AutoSize = true;
            this.checkBoxBare.Location = new System.Drawing.Point(12, 116);
            this.checkBoxBare.Name = "checkBoxBare";
            this.checkBoxBare.Size = new System.Drawing.Size(312, 17);
            this.checkBoxBare.TabIndex = 3;
            this.checkBoxBare.Text = "Create a shared or \"bare\" repository with no working files in it";
            this.checkBoxBare.UseVisualStyleBackColor = true;
            this.checkBoxBare.CheckedChanged += new System.EventHandler(this.CheckBoxBareCheckedChanged);
            // 
            // labelCloneOperation
            // 
            this.labelCloneOperation.AutoSize = true;
            this.labelCloneOperation.Location = new System.Drawing.Point(151, 78);
            this.labelCloneOperation.Name = "labelCloneOperation";
            this.labelCloneOperation.Size = new System.Drawing.Size(194, 13);
            this.labelCloneOperation.TabIndex = 9;
            this.labelCloneOperation.Text = "Project name (will create a subdirectory)";
            // 
            // textBoxProjectName
            // 
            this.textBoxProjectName.Location = new System.Drawing.Point(12, 75);
            this.textBoxProjectName.Name = "textBoxProjectName";
            this.textBoxProjectName.Size = new System.Drawing.Size(133, 20);
            this.textBoxProjectName.TabIndex = 2;
            this.textBoxProjectName.TextChanged += new System.EventHandler(this.TextBoxRepoPathTextChanged);
            // 
            // FormNewRepoStep2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btPrev;
            this.ClientSize = new System.Drawing.Size(456, 343);
            this.Controls.Add(this.textBoxProjectName);
            this.Controls.Add(this.labelCloneOperation);
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
            this.MinimumSize = new System.Drawing.Size(369, 251);
            this.Name = "FormNewRepoStep2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "New Repository (step 2 of 2)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormNewRepoStep2FormClosing);
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
        private System.Windows.Forms.FolderBrowserDialog folder;
        private System.Windows.Forms.CheckBox checkBoxBare;
        private System.Windows.Forms.Label labelCloneOperation;
        private System.Windows.Forms.TextBox textBoxProjectName;
    }
}