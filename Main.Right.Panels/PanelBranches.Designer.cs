namespace GitForce.Main.Right.Panels
{
    partial class PanelBranches
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Local Branches");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Remote Branches");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelBranches));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.treeBranches = new System.Windows.Forms.TreeView();
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
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(400, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(72, 22);
            this.toolStripLabel1.Text = "git Branches";
            // 
            // treeBranches
            // 
            this.treeBranches.ContextMenuStrip = this.contextMenu;
            this.treeBranches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeBranches.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeBranches.ImageIndex = 0;
            this.treeBranches.ImageList = this.imageList;
            this.treeBranches.Location = new System.Drawing.Point(0, 25);
            this.treeBranches.Name = "treeBranches";
            treeNode1.Name = "tnLocal";
            treeNode1.Text = "Local Branches";
            treeNode2.Name = "tnRemote";
            treeNode2.Text = "Remote Branches";
            this.treeBranches.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.treeBranches.SelectedImageIndex = 0;
            this.treeBranches.Size = new System.Drawing.Size(400, 375);
            this.treeBranches.TabIndex = 1;
            this.treeBranches.Tag = "";
            this.treeBranches.DoubleClick += new System.EventHandler(this.TreeBranchesDoubleClick);
            this.treeBranches.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TreeBranchesMouseUp);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dummyMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(153, 48);
            // 
            // dummyMenuItem
            // 
            this.dummyMenuItem.Name = "dummyMenuItem";
            this.dummyMenuItem.Size = new System.Drawing.Size(152, 22);
            this.dummyMenuItem.Text = "No Repository";
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Black;
            this.imageList.Images.SetKeyName(0, "branch.bmp");
            this.imageList.Images.SetKeyName(1, "112_RightArrowLong_Grey_16x16_72.png");
            this.imageList.Images.SetKeyName(2, "112_RightArrowLong_Green_16x16_72.png");
            // 
            // PanelBranches
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeBranches);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PanelBranches";
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
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem dummyMenuItem;
        private System.Windows.Forms.TreeView treeBranches;
    }
}
