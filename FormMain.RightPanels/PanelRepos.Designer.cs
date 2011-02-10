namespace git4win.FormMain_RightPanels
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
            this.listRepos = new System.Windows.Forms.ListView();
            this.colRoot = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEmail = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.dummyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1});
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
            // listRepos
            // 
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
            this.listRepos.MultiSelect = false;
            this.listRepos.Name = "listRepos";
            this.listRepos.Size = new System.Drawing.Size(400, 375);
            this.listRepos.SmallImageList = this.imageList;
            this.listRepos.TabIndex = 1;
            this.listRepos.UseCompatibleStateImageBehavior = false;
            this.listRepos.View = System.Windows.Forms.View.Details;
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
            this.contextMenu.Size = new System.Drawing.Size(153, 48);
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
            // dummyMenuItem
            // 
            this.dummyMenuItem.Name = "dummyMenuItem";
            this.dummyMenuItem.Size = new System.Drawing.Size(152, 22);
            this.dummyMenuItem.Text = "Menu Item";
            // 
            // PanelRepos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listRepos);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PanelRepos";
            this.Size = new System.Drawing.Size(400, 400);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ListView listRepos;
        private System.Windows.Forms.ColumnHeader colRoot;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colEmail;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStripMenuItem dummyMenuItem;
    }
}
