namespace GitForce
{
    partial class FormCommit
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
            this.checkAmend = new System.Windows.Forms.CheckBox();
            this.btSelectAll = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.listFiles = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btUnselectAll = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textDescription = new GitForce.RichTextBoxEx();
            this.btCommit = new System.Windows.Forms.Button();
            this.labelWidth = new System.Windows.Forms.Label();
            this.labelCursor = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkAmend
            // 
            this.checkAmend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkAmend.AutoSize = true;
            this.checkAmend.Location = new System.Drawing.Point(12, 465);
            this.checkAmend.Name = "checkAmend";
            this.checkAmend.Size = new System.Drawing.Size(290, 17);
            this.checkAmend.TabIndex = 1;
            this.checkAmend.Text = "Amend the last change instead of committing a new one";
            this.checkAmend.UseVisualStyleBackColor = true;
            this.checkAmend.CheckedChanged += new System.EventHandler(this.CheckAmendCheckedChanged);
            // 
            // btSelectAll
            // 
            this.btSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSelectAll.Location = new System.Drawing.Point(506, 426);
            this.btSelectAll.Name = "btSelectAll";
            this.btSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btSelectAll.TabIndex = 2;
            this.btSelectAll.Text = "Select All";
            this.btSelectAll.UseVisualStyleBackColor = true;
            this.btSelectAll.Click += new System.EventHandler(this.BtSelectAllClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Files:";
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(587, 461);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 5;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // listFiles
            // 
            this.listFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listFiles.CheckOnClick = true;
            this.listFiles.FormattingEnabled = true;
            this.listFiles.IntegralHeight = false;
            this.listFiles.Location = new System.Drawing.Point(66, 0);
            this.listFiles.Name = "listFiles";
            this.listFiles.Size = new System.Drawing.Size(584, 168);
            this.listFiles.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Description:";
            // 
            // btUnselectAll
            // 
            this.btUnselectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btUnselectAll.Location = new System.Drawing.Point(587, 426);
            this.btUnselectAll.Name = "btUnselectAll";
            this.btUnselectAll.Size = new System.Drawing.Size(75, 23);
            this.btUnselectAll.TabIndex = 3;
            this.btUnselectAll.Text = "Unselect All";
            this.btUnselectAll.UseVisualStyleBackColor = true;
            this.btUnselectAll.Click += new System.EventHandler(this.BtUnselectAllClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textDescription);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listFiles);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Size = new System.Drawing.Size(650, 408);
            this.splitContainer1.SplitterDistance = 236;
            this.splitContainer1.TabIndex = 0;
            // 
            // textDescription
            // 
            this.textDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textDescription.DetectUrls = false;
            this.textDescription.Location = new System.Drawing.Point(66, 0);
            this.textDescription.Name = "textDescription";
            this.textDescription.Size = new System.Drawing.Size(584, 234);
            this.textDescription.TabIndex = 0;
            this.textDescription.Text = "";
            this.textDescription.SelectionChanged += new System.EventHandler(this.TextDescriptionTextChanged);
            this.textDescription.TextChanged += new System.EventHandler(this.TextDescriptionTextChanged);
            // 
            // btCommit
            // 
            this.btCommit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCommit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btCommit.Enabled = false;
            this.btCommit.Location = new System.Drawing.Point(506, 461);
            this.btCommit.Name = "btCommit";
            this.btCommit.Size = new System.Drawing.Size(75, 23);
            this.btCommit.TabIndex = 4;
            this.btCommit.Text = "Commit";
            this.btCommit.UseVisualStyleBackColor = true;
            // 
            // labelWidth
            // 
            this.labelWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelWidth.AutoSize = true;
            this.labelWidth.Location = new System.Drawing.Point(75, 431);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(44, 13);
            this.labelWidth.TabIndex = 11;
            this.labelWidth.Text = "No text.";
            // 
            // labelCursor
            // 
            this.labelCursor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCursor.AutoSize = true;
            this.labelCursor.Location = new System.Drawing.Point(9, 431);
            this.labelCursor.Name = "labelCursor";
            this.labelCursor.Size = new System.Drawing.Size(37, 13);
            this.labelCursor.TabIndex = 12;
            this.labelCursor.Text = "Cursor";
            // 
            // FormCommit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 496);
            this.Controls.Add(this.labelCursor);
            this.Controls.Add(this.labelWidth);
            this.Controls.Add(this.checkAmend);
            this.Controls.Add(this.btSelectAll);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btUnselectAll);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btCommit);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(519, 273);
            this.Name = "FormCommit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "GitForce Commit Specification";
            this.Load += new System.EventHandler(this.FormCommitLoad);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCommitFormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkAmend;
        private System.Windows.Forms.Button btSelectAll;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.CheckedListBox listFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btUnselectAll;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btCommit;
        private RichTextBoxEx textDescription;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.Label labelCursor;
    }
}