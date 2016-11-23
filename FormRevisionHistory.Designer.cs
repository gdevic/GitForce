namespace GitForce
{
    partial class FormRevisionHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRevisionHistory));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.diffRevisionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.diffVsClientFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listRev = new System.Windows.Forms.ListView();
            this.colRev = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHash = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAuthor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSubject = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textDescription = new System.Windows.Forms.RichTextBox();
            this.menuStrip.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitMenuItem,
            this.viewMenuItem,
            this.syncMenuItem,
            this.diffRevisionsMenuItem,
            this.diffVsClientFileMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(563, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(37, 20);
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuItemClick);
            // 
            // viewMenuItem
            // 
            this.viewMenuItem.Enabled = false;
            this.viewMenuItem.Name = "viewMenuItem";
            this.viewMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewMenuItem.Text = "View";
            this.viewMenuItem.DropDownOpening += new System.EventHandler(this.ViewMenuItemDropDownOpening);
            // 
            // syncMenuItem
            // 
            this.syncMenuItem.Enabled = false;
            this.syncMenuItem.Name = "syncMenuItem";
            this.syncMenuItem.Size = new System.Drawing.Size(44, 20);
            this.syncMenuItem.Text = "Sync";
            this.syncMenuItem.Click += new System.EventHandler(this.SyncMenuItemClick);
            // 
            // diffRevisionsMenuItem
            // 
            this.diffRevisionsMenuItem.Enabled = false;
            this.diffRevisionsMenuItem.Name = "diffRevisionsMenuItem";
            this.diffRevisionsMenuItem.Size = new System.Drawing.Size(90, 20);
            this.diffRevisionsMenuItem.Text = "Diff Revisions";
            this.diffRevisionsMenuItem.Click += new System.EventHandler(this.DiffRevisionsMenuItemClick);
            // 
            // diffVsClientFileMenuItem
            // 
            this.diffVsClientFileMenuItem.Enabled = false;
            this.diffVsClientFileMenuItem.Name = "diffVsClientFileMenuItem";
            this.diffVsClientFileMenuItem.Size = new System.Drawing.Size(108, 20);
            this.diffVsClientFileMenuItem.Text = "Diff vs. Client file";
            this.diffVsClientFileMenuItem.Click += new System.EventHandler(this.DiffVsClientFileMenuItemClick);
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 349);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(563, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listRev);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textDescription);
            this.splitContainer1.Size = new System.Drawing.Size(563, 325);
            this.splitContainer1.SplitterDistance = 184;
            this.splitContainer1.TabIndex = 3;
            // 
            // listRev
            // 
            this.listRev.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colRev,
            this.colHash,
            this.colDate,
            this.colAuthor,
            this.colSubject});
            this.listRev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listRev.FullRowSelect = true;
            this.listRev.Location = new System.Drawing.Point(0, 0);
            this.listRev.Name = "listRev";
            this.listRev.Size = new System.Drawing.Size(563, 184);
            this.listRev.TabIndex = 0;
            this.listRev.UseCompatibleStateImageBehavior = false;
            this.listRev.View = System.Windows.Forms.View.Details;
            this.listRev.SelectedIndexChanged += new System.EventHandler(this.ListRevSelectedIndexChanged);
            this.listRev.DoubleClick += new System.EventHandler(this.ListRevDoubleClick);
            // 
            // colHash
            // 
            this.colHash.Text = "SHA1";
            // 
            // colDate
            // 
            this.colDate.Text = "Date";
            // 
            // colAuthor
            // 
            this.colAuthor.Text = "Author";
            // 
            // colSubject
            // 
            this.colSubject.Text = "Subject";
            // 
            // textDescription
            // 
            this.textDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textDescription.Location = new System.Drawing.Point(0, 0);
            this.textDescription.Name = "textDescription";
            this.textDescription.ReadOnly = true;
            this.textDescription.Size = new System.Drawing.Size(563, 137);
            this.textDescription.TabIndex = 0;
            this.textDescription.Text = "";
            // 
            // colRev
            // 
            this.colRev.Text = "Revision";
            // 
            // FormRevisionHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 371);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRevisionHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Revision History";
            this.Load += new System.EventHandler(this.FormRevisionHistoryLoad);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRevisionHistoryFormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem viewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem syncMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diffRevisionsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diffVsClientFileMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ListView listRev;
        private System.Windows.Forms.RichTextBox textDescription;
        private System.Windows.Forms.ColumnHeader colRev;
        private System.Windows.Forms.ColumnHeader colHash;
        private System.Windows.Forms.ColumnHeader colDate;
        private System.Windows.Forms.ColumnHeader colAuthor;
        private System.Windows.Forms.ColumnHeader colSubject;
    }
}