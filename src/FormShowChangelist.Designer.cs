namespace GitForce
{
    partial class FormShowChangelist
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormShowChangelist));
            this.btPrev = new System.Windows.Forms.Button();
            this.btNext = new System.Windows.Forms.Button();
            this.btClose = new System.Windows.Forms.Button();
            this.comboShow = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuText = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuFile = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemDiffPrev = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDiffNext = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDiffHead = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemRev = new System.Windows.Forms.ToolStripMenuItem();
            this.textChangelist = new GitForce.RichTextBoxEx();
            this.contextMenuText.SuspendLayout();
            this.contextMenuFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // btPrev
            // 
            this.btPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btPrev.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btPrev.Image = ((System.Drawing.Image)(resources.GetObject("btPrev.Image")));
            this.btPrev.Location = new System.Drawing.Point(93, 383);
            this.btPrev.Name = "btPrev";
            this.btPrev.Size = new System.Drawing.Size(75, 23);
            this.btPrev.TabIndex = 7;
            this.btPrev.UseVisualStyleBackColor = true;
            // 
            // btNext
            // 
            this.btNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btNext.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btNext.Image = ((System.Drawing.Image)(resources.GetObject("btNext.Image")));
            this.btNext.Location = new System.Drawing.Point(12, 383);
            this.btNext.Name = "btNext";
            this.btNext.Size = new System.Drawing.Size(75, 23);
            this.btNext.TabIndex = 6;
            this.btNext.UseVisualStyleBackColor = true;
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btClose.Location = new System.Drawing.Point(490, 383);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(75, 23);
            this.btClose.TabIndex = 5;
            this.btClose.Text = "Close";
            this.btClose.UseVisualStyleBackColor = true;
            // 
            // comboShow
            // 
            this.comboShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboShow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboShow.FormattingEnabled = true;
            this.comboShow.Items.AddRange(new object[] {
            "(none)",
            "oneline",
            "short",
            "medium",
            "full",
            "fuller",
            "email",
            "raw"});
            this.comboShow.Location = new System.Drawing.Point(222, 384);
            this.comboShow.Name = "comboShow";
            this.comboShow.Size = new System.Drawing.Size(73, 21);
            this.comboShow.TabIndex = 9;
            this.comboShow.SelectedIndexChanged += new System.EventHandler(this.ComboShowSelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(174, 388);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Details:";
            // 
            // contextMenuText
            // 
            this.contextMenuText.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCopy});
            this.contextMenuText.Name = "contextMenuText";
            this.contextMenuText.Size = new System.Drawing.Size(103, 26);
            // 
            // menuItemCopy
            // 
            this.menuItemCopy.Name = "menuItemCopy";
            this.menuItemCopy.Size = new System.Drawing.Size(102, 22);
            this.menuItemCopy.Text = "Copy";
            this.menuItemCopy.Click += new System.EventHandler(this.MenuItemCopyClick);
            // 
            // contextMenuFile
            // 
            this.contextMenuFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemDiffPrev,
            this.menuItemDiffNext,
            this.menuItemDiffHead,
            this.toolStripSeparator1,
            this.menuItemRev});
            this.contextMenuFile.Name = "contextMenuFile";
            this.contextMenuFile.Size = new System.Drawing.Size(169, 98);
            // 
            // menuItemDiffPrev
            // 
            this.menuItemDiffPrev.Name = "menuItemDiffPrev";
            this.menuItemDiffPrev.Size = new System.Drawing.Size(168, 22);
            this.menuItemDiffPrev.Text = "Diff vs Previous";
            this.menuItemDiffPrev.Click += new System.EventHandler(this.MenuItemDiffClick);
            // 
            // menuItemDiffNext
            // 
            this.menuItemDiffNext.Name = "menuItemDiffNext";
            this.menuItemDiffNext.Size = new System.Drawing.Size(168, 22);
            this.menuItemDiffNext.Text = "Diff vs Next";
            this.menuItemDiffNext.Click += new System.EventHandler(this.MenuItemDiffClick);
            // 
            // menuItemDiffHead
            // 
            this.menuItemDiffHead.Name = "menuItemDiffHead";
            this.menuItemDiffHead.Size = new System.Drawing.Size(168, 22);
            this.menuItemDiffHead.Text = "Diff vs HEAD";
            this.menuItemDiffHead.Click += new System.EventHandler(this.MenuItemDiffClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(165, 6);
            // 
            // menuItemRev
            // 
            this.menuItemRev.Name = "menuItemRev";
            this.menuItemRev.Size = new System.Drawing.Size(168, 22);
            this.menuItemRev.Text = "Revision History...";
            this.menuItemRev.Click += new System.EventHandler(this.MenuItemRevClick);
            // 
            // textChangelist
            // 
            this.textChangelist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textChangelist.BackColor = System.Drawing.SystemColors.Info;
            this.textChangelist.ContextMenuStrip = this.contextMenuText;
            this.textChangelist.DetectUrls = false;
            this.textChangelist.Location = new System.Drawing.Point(12, 12);
            this.textChangelist.Name = "textChangelist";
            this.textChangelist.ReadOnly = true;
            this.textChangelist.Size = new System.Drawing.Size(553, 365);
            this.textChangelist.TabIndex = 8;
            this.textChangelist.Text = "";
            this.textChangelist.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextChangelistKeyDown);
            this.textChangelist.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.TextChangelistLinkClicked);
            // 
            // FormShowChangelist
            // 
            this.AcceptButton = this.btPrev;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btClose;
            this.ClientSize = new System.Drawing.Size(577, 418);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboShow);
            this.Controls.Add(this.textChangelist);
            this.Controls.Add(this.btPrev);
            this.Controls.Add(this.btNext);
            this.Controls.Add(this.btClose);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(402, 130);
            this.Name = "FormShowChangelist";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Describe Git Changelist";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormShowChangelistFormClosing);
            this.contextMenuText.ResumeLayout(false);
            this.contextMenuFile.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RichTextBoxEx textChangelist;
        private System.Windows.Forms.Button btPrev;
        private System.Windows.Forms.Button btNext;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.ComboBox comboShow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuText;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopy;
        private System.Windows.Forms.ContextMenuStrip contextMenuFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemDiffPrev;
        private System.Windows.Forms.ToolStripMenuItem menuItemDiffNext;
        private System.Windows.Forms.ToolStripMenuItem menuItemRev;
        private System.Windows.Forms.ToolStripMenuItem menuItemDiffHead;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}