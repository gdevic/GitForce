namespace GitForce.Main.Left.Panels
{
    partial class PanelView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelView));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.viewLabel = new System.Windows.Forms.ToolStripLabel();
            this.dropViewMode = new System.Windows.Forms.ToolStripDropDownButton();
            this.menuView0 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSortFilesByExtension = new System.Windows.Forms.ToolStripMenuItem();
            this.btRefresh = new System.Windows.Forms.ToolStripButton();
            this.btListView = new System.Windows.Forms.ToolStripButton();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dummyItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView = new TreeViewEx();
            this.toolStrip1.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewLabel,
            this.dropViewMode,
            this.btRefresh,
            this.btListView});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(400, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // viewLabel
            // 
            this.viewLabel.Name = "viewLabel";
            this.viewLabel.Size = new System.Drawing.Size(59, 22);
            this.viewLabel.Text = "viewLabel";
            // 
            // dropViewMode
            // 
            this.dropViewMode.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.dropViewMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.dropViewMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuView0,
            this.menuView1,
            this.menuView2,
            this.menuView3,
            this.menuView4,
            this.toolStripSeparator1,
            this.menuSortFilesByExtension});
            this.dropViewMode.Image = ((System.Drawing.Image)(resources.GetObject("dropViewMode.Image")));
            this.dropViewMode.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.dropViewMode.ImageTransparentColor = System.Drawing.Color.Black;
            this.dropViewMode.Name = "dropViewMode";
            this.dropViewMode.Size = new System.Drawing.Size(29, 22);
            this.dropViewMode.Text = "View";
            // 
            // menuView0
            // 
            this.menuView0.Name = "menuView0";
            this.menuView0.Size = new System.Drawing.Size(194, 22);
            this.menuView0.Tag = "0";
            this.menuView0.Text = "Git View of Local Files";
            this.menuView0.Click += new System.EventHandler(this.ViewSetByMenuItem);
            // 
            // menuView1
            // 
            this.menuView1.Name = "menuView1";
            this.menuView1.Size = new System.Drawing.Size(194, 22);
            this.menuView1.Tag = "1";
            this.menuView1.Text = "Git View of Files";
            this.menuView1.Click += new System.EventHandler(this.ViewSetByMenuItem);
            // 
            // menuView2
            // 
            this.menuView2.Name = "menuView2";
            this.menuView2.Size = new System.Drawing.Size(194, 22);
            this.menuView2.Tag = "2";
            this.menuView2.Text = "Git View of Repo";
            this.menuView2.Click += new System.EventHandler(this.ViewSetByMenuItem);
            // 
            // menuView3
            // 
            this.menuView3.Name = "menuView3";
            this.menuView3.Size = new System.Drawing.Size(194, 22);
            this.menuView3.Tag = "3";
            this.menuView3.Text = "Local File View";
            this.menuView3.Click += new System.EventHandler(this.ViewSetByMenuItem);
            // 
            // menuView4
            // 
            this.menuView4.Name = "menuView4";
            this.menuView4.Size = new System.Drawing.Size(194, 22);
            this.menuView4.Tag = "4";
            this.menuView4.Text = "Local Files Not in Repo";
            this.menuView4.Click += new System.EventHandler(this.ViewSetByMenuItem);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(191, 6);
            // 
            // menuSortFilesByExtension
            // 
            this.menuSortFilesByExtension.CheckOnClick = true;
            this.menuSortFilesByExtension.Name = "menuSortFilesByExtension";
            this.menuSortFilesByExtension.Size = new System.Drawing.Size(194, 22);
            this.menuSortFilesByExtension.Text = "Sort Files by Extension";
            this.menuSortFilesByExtension.Click += new System.EventHandler(this.MenuSortFilesByExtensionClick);
            // 
            // btRefresh
            // 
            this.btRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btRefresh.Image")));
            this.btRefresh.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btRefresh.ImageTransparentColor = System.Drawing.Color.Black;
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(23, 22);
            this.btRefresh.Text = "Refresh";
            this.btRefresh.Click += new System.EventHandler(this.MenuRefresh);
            // 
            // btListView
            // 
            this.btListView.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btListView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btListView.Image = ((System.Drawing.Image)(resources.GetObject("btListView.Image")));
            this.btListView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btListView.Name = "btListView";
            this.btListView.Size = new System.Drawing.Size(23, 22);
            this.btListView.Text = "Toggle between list or tree views";
            this.btListView.Click += new System.EventHandler(this.BtListViewClick);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dummyItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(150, 26);
            // 
            // dummyItem
            // 
            this.dummyItem.Name = "dummyItem";
            this.dummyItem.Size = new System.Drawing.Size(149, 22);
            this.dummyItem.Text = "No Repository";
            // 
            // treeView
            // 
            this.treeView.AllowDrop = true;
            this.treeView.ContextMenuStrip = this.contextMenu;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView.HideSelection = false;
            this.treeView.Indent = 18;
            this.treeView.Location = new System.Drawing.Point(0, 25);
            this.treeView.Name = "treeView";
            this.treeView.SelectedNodes = ((System.Collections.Generic.List<System.Windows.Forms.TreeNode>)(resources.GetObject("treeView.SelectedNodes")));
            this.treeView.Size = new System.Drawing.Size(400, 375);
            this.treeView.TabIndex = 1;
            this.treeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewAfterCollapse);
            this.treeView.DoubleClick += new System.EventHandler(this.TreeViewDoubleClick);
            this.treeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TreeViewMouseUp);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewAfterSelect);
            this.treeView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TreeViewMouseMove);
            this.treeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.TreeViewDragEnter);
            this.treeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewAfterExpand);
            this.treeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TreeViewItemDrag);
            // 
            // PanelView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PanelView";
            this.Size = new System.Drawing.Size(400, 400);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private TreeViewEx treeView;
        private System.Windows.Forms.ToolStripLabel viewLabel;
        private System.Windows.Forms.ToolStripDropDownButton dropViewMode;
        private System.Windows.Forms.ToolStripButton btRefresh;
        private System.Windows.Forms.ToolStripMenuItem menuView0;
        private System.Windows.Forms.ToolStripMenuItem menuView1;
        private System.Windows.Forms.ToolStripMenuItem menuView2;
        private System.Windows.Forms.ToolStripMenuItem menuView3;
        private System.Windows.Forms.ToolStripMenuItem menuView4;
        private System.Windows.Forms.ToolStripButton btListView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuSortFilesByExtension;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem dummyItem;
    }
}
