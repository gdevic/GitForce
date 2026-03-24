namespace GitForce.Main.Right.Panels
{
    partial class PanelRepos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelRepos));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.btFlatList = new System.Windows.Forms.ToolStripButton();
            this.listRepos = new GitForce.ListViewReorderEx();
            this.colRoot = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEmail = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dummyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.btFlatList});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(400, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Tag = "Repos";
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(89, 22);
            this.toolStripLabel1.Text = "Git Repositories";
            //
            // btFlatList
            //
            this.btFlatList.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btFlatList.CheckOnClick = true;
            this.btFlatList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btFlatList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btFlatList.Name = "btFlatList";
            this.btFlatList.Size = new System.Drawing.Size(23, 22);
            this.btFlatList.ToolTipText = "Toggle between flat list and Project tree view";
            this.btFlatList.Click += new System.EventHandler(this.BtFlatListClick);
            //
            // listRepos
            // 
            this.listRepos.AllowDrop = true;
            this.listRepos.AllowRowReorder = true;
            this.listRepos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colRoot,
            this.colName,
            this.colEmail});
            this.listRepos.ContextMenuStrip = this.contextMenu;
            this.listRepos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listRepos.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listRepos.FullRowSelect = true;
            this.listRepos.HideSelection = false;
            this.listRepos.Location = new System.Drawing.Point(0, 25);
            this.listRepos.Name = "listRepos";
            this.listRepos.Size = new System.Drawing.Size(400, 375);
            this.listRepos.SmallImageList = this.imageList;
            this.listRepos.TabIndex = 1;
            this.listRepos.UseCompatibleStateImageBehavior = false;
            this.listRepos.View = System.Windows.Forms.View.Details;
            this.listRepos.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.ColumnWidthChanged);
            this.listRepos.VisibleChanged += new System.EventHandler(this.ListReposVisibleChanged);
            this.listRepos.DragDrop += new System.Windows.Forms.DragEventHandler(this.ListReposDragDrop);
            this.listRepos.DragEnter += new System.Windows.Forms.DragEventHandler(this.ListReposDragEnter);
            this.listRepos.DoubleClick += new System.EventHandler(this.ListReposDoubleClick);
            this.listRepos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ListReposMouseUp);
            // 
            // colRoot
            // 
            this.colRoot.Text = "Root Folder";
            this.colRoot.Width = 104;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 77;
            // 
            // colEmail
            // 
            this.colEmail.Text = "Email";
            this.colEmail.Width = 86;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dummyMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(133, 26);
            // 
            // dummyMenuItem
            // 
            this.dummyMenuItem.Name = "dummyMenuItem";
            this.dummyMenuItem.Size = new System.Drawing.Size(132, 22);
            this.dummyMenuItem.Text = "Menu Item";
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Repo0.ico");
            this.imageList.Images.SetKeyName(1, "Repo1.ico");
            this.imageList.Images.SetKeyName(2, "Repo2.ico");
            this.imageList.Images.SetKeyName(3, "Repo3.ico");
            //
            // treeRepos
            //
            this.treeRepos = new GitForce.TreeViewEx();
            this.treeRepos.AllowDrop = true;
            this.treeRepos.ContextMenuStrip = this.contextMenu;
            this.treeRepos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeRepos.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeRepos.HideSelection = false;
            this.treeRepos.ImageList = this.imageList;
            this.treeRepos.Location = new System.Drawing.Point(0, 25);
            this.treeRepos.Name = "treeRepos";
            this.treeRepos.Size = new System.Drawing.Size(400, 375);
            this.treeRepos.TabIndex = 2;
            this.treeRepos.Visible = false;
            this.treeRepos.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TreeReposItemDrag);
            this.treeRepos.DragDrop += new System.Windows.Forms.DragEventHandler(this.TreeReposDragDrop);
            this.treeRepos.DragEnter += new System.Windows.Forms.DragEventHandler(this.TreeReposDragEnter);
            this.treeRepos.DragOver += new System.Windows.Forms.DragEventHandler(this.TreeReposDragOver);
            this.treeRepos.DoubleClick += new System.EventHandler(this.TreeReposDoubleClick);
            this.treeRepos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TreeReposMouseUp);
            this.treeRepos.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.TreeReposAfterCollapseExpand);
            this.treeRepos.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.TreeReposAfterCollapseExpand);
            //
            // PanelRepos
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeRepos);
            this.Controls.Add(this.listRepos);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PanelRepos";
            this.Size = new System.Drawing.Size(400, 400);
            this.Tag = "Repos";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ColumnHeader colRoot;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colEmail;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStripMenuItem dummyMenuItem;
        private System.Windows.Forms.ToolStripButton btFlatList;
        private ListViewReorderEx listRepos;
        private GitForce.TreeViewEx treeRepos;
    }
}
