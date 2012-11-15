namespace GitForce.Main.Right.Panels
{
    partial class PanelRevlist
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelRevlist));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.btBranch = new System.Windows.Forms.ToolStripDropDownButton();
            this.masterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btClearFilter = new System.Windows.Forms.ToolStripButton();
            this.btSetFilter = new System.Windows.Forms.ToolStripButton();
            this.btRefresh = new System.Windows.Forms.ToolStripButton();
            this.labelLogBranch = new System.Windows.Forms.ToolStripLabel();
            this.listRev = new System.Windows.Forms.ListView();
            this.colHash = new System.Windows.Forms.ColumnHeader();
            this.colDate = new System.Windows.Forms.ColumnHeader();
            this.colAuthor = new System.Windows.Forms.ColumnHeader();
            this.colSubject = new System.Windows.Forms.ColumnHeader();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.btBranch,
            this.toolStripSeparator1,
            this.btClearFilter,
            this.btSetFilter,
            this.btRefresh,
            this.labelLogBranch});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(400, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(126, 22);
            this.toolStripLabel1.Text = "Submitted Changelists";
            // 
            // btBranch
            // 
            this.btBranch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btBranch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btBranch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.masterToolStripMenuItem});
            this.btBranch.Image = ((System.Drawing.Image)(resources.GetObject("btBranch.Image")));
            this.btBranch.ImageTransparentColor = System.Drawing.Color.Black;
            this.btBranch.Name = "btBranch";
            this.btBranch.Size = new System.Drawing.Size(29, 22);
            this.btBranch.Text = "Branch Selection";
            // 
            // masterToolStripMenuItem
            // 
            this.masterToolStripMenuItem.Name = "masterToolStripMenuItem";
            this.masterToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.masterToolStripMenuItem.Text = "master";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btClearFilter
            // 
            this.btClearFilter.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btClearFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btClearFilter.Enabled = false;
            this.btClearFilter.Image = ((System.Drawing.Image)(resources.GetObject("btClearFilter.Image")));
            this.btClearFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btClearFilter.Name = "btClearFilter";
            this.btClearFilter.Size = new System.Drawing.Size(23, 22);
            this.btClearFilter.Text = "Clear Filter";
            this.btClearFilter.Click += new System.EventHandler(this.MenuClearFilterClick);
            // 
            // btSetFilter
            // 
            this.btSetFilter.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btSetFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btSetFilter.Image = ((System.Drawing.Image)(resources.GetObject("btSetFilter.Image")));
            this.btSetFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSetFilter.Name = "btSetFilter";
            this.btSetFilter.Size = new System.Drawing.Size(23, 22);
            this.btSetFilter.Text = "Set Filter";
            this.btSetFilter.Click += new System.EventHandler(this.MenuSetFilterClick);
            // 
            // btRefresh
            // 
            this.btRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btRefresh.Image")));
            this.btRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(23, 22);
            this.btRefresh.Text = "Refresh Submitted Changelists view";
            this.btRefresh.Click += new System.EventHandler(this.MenuRefreshClick);
            // 
            // labelLogBranch
            // 
            this.labelLogBranch.Name = "labelLogBranch";
            this.labelLogBranch.Size = new System.Drawing.Size(0, 22);
            // 
            // listRev
            // 
            this.listRev.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHash,
            this.colDate,
            this.colAuthor,
            this.colSubject});
            this.listRev.ContextMenuStrip = this.contextMenu;
            this.listRev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listRev.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listRev.FullRowSelect = true;
            this.listRev.HideSelection = false;
            this.listRev.Location = new System.Drawing.Point(0, 25);
            this.listRev.MultiSelect = false;
            this.listRev.Name = "listRev";
            this.listRev.Size = new System.Drawing.Size(400, 375);
            this.listRev.TabIndex = 1;
            this.listRev.UseCompatibleStateImageBehavior = false;
            this.listRev.View = System.Windows.Forms.View.Details;
            this.listRev.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListRevMouseDoubleClick);
            this.listRev.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ListRevMouseUp);
            // 
            // colHash
            // 
            this.colHash.Text = "SHA1";
            this.colHash.Width = 40;
            // 
            // colDate
            // 
            this.colDate.Text = "Date";
            this.colDate.Width = 35;
            // 
            // colAuthor
            // 
            this.colAuthor.Text = "Author";
            this.colAuthor.Width = 56;
            // 
            // colSubject
            // 
            this.colSubject.Text = "Subject";
            this.colSubject.Width = 265;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(150, 26);
            // 
            // menuItem
            // 
            this.menuItem.Name = "menuItem";
            this.menuItem.Size = new System.Drawing.Size(149, 22);
            this.menuItem.Text = "No Repository";
            this.menuItem.Click += new System.EventHandler(this.MenuResetClick);
            // 
            // PanelRevlist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listRev);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PanelRevlist";
            this.Size = new System.Drawing.Size(400, 400);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ListView listRev;
        private System.Windows.Forms.ColumnHeader colHash;
        private System.Windows.Forms.ColumnHeader colDate;
        private System.Windows.Forms.ColumnHeader colAuthor;
        private System.Windows.Forms.ColumnHeader colSubject;
        private System.Windows.Forms.ToolStripDropDownButton btBranch;
        private System.Windows.Forms.ToolStripMenuItem masterToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btClearFilter;
        private System.Windows.Forms.ToolStripButton btSetFilter;
        private System.Windows.Forms.ToolStripButton btRefresh;
        private System.Windows.Forms.ToolStripLabel labelLogBranch;
    }
}
