namespace GitForce
{
    partial class UserControlEditGitignore
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
            this.listFilters = new System.Windows.Forms.ListBox();
            this.textBox = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btAdd = new System.Windows.Forms.Button();
            this.labelFileName = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listFilters
            // 
            this.listFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listFilters.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listFilters.FormattingEnabled = true;
            this.listFilters.IntegralHeight = false;
            this.listFilters.Items.AddRange(new object[] {
            "*.obj - Object files",
            "*.exe - Executable files",
            "*.pdb - VC Program database",
            "*.pch - Precompiled help files",
            "*.suo - VC Solution user files",
            "*.ncb",
            "*.aps",
            "*.bak - Typical backup files",
            "*.ilk",
            "*.log",
            "*.lib - Library files",
            "*.sbr",
            "[Bb]in - Binary folders",
            "[Dd]ebug*/ - Debug folders",
            "[Rr]elease*/ - Release folders",
            "obj/ - Object file folders"});
            this.listFilters.Location = new System.Drawing.Point(2, 3);
            this.listFilters.Name = "listFilters";
            this.listFilters.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listFilters.Size = new System.Drawing.Size(173, 147);
            this.listFilters.TabIndex = 4;
            // 
            // textBox
            // 
            this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox.Location = new System.Drawing.Point(0, 0);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox.Size = new System.Drawing.Size(172, 179);
            this.textBox.TabIndex = 0;
            this.textBox.WordWrap = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(2, 19);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listFilters);
            this.splitContainer1.Panel2.Controls.Add(this.btAdd);
            this.splitContainer1.Size = new System.Drawing.Size(356, 179);
            this.splitContainer1.SplitterDistance = 172;
            this.splitContainer1.TabIndex = 6;
            // 
            // btAdd
            // 
            this.btAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btAdd.Location = new System.Drawing.Point(2, 156);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(173, 23);
            this.btAdd.TabIndex = 3;
            this.btAdd.Text = "<< Add Selected";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.BtAddClick);
            // 
            // labelFileName
            // 
            this.labelFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileName.Location = new System.Drawing.Point(0, 0);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(355, 19);
            this.labelFileName.TabIndex = 5;
            this.labelFileName.Text = "File:";
            // 
            // UserControlEditGitignore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.labelFileName);
            this.Name = "UserControlEditGitignore";
            this.Size = new System.Drawing.Size(356, 198);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listFilters;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Label labelFileName;
    }
}
