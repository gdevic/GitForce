namespace git4win.FormMain_RightPanels
{
    partial class PanelCommits
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Staged Files");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelCommits));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.treeCommits = new MultiSelectTreeview.MultiSelectTreeview();
            this.toolStrip1.SuspendLayout();
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
            this.toolStripLabel1.Size = new System.Drawing.Size(133, 22);
            this.toolStripLabel1.Text = "Pending Git Changelists";
            // 
            // contextMenu
            // 
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // treeCommits
            // 
            this.treeCommits.AllowDrop = true;
            this.treeCommits.ContextMenuStrip = this.contextMenu;
            this.treeCommits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeCommits.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeCommits.Indent = 16;
            this.treeCommits.Location = new System.Drawing.Point(0, 25);
            this.treeCommits.Name = "treeCommits";
            treeNode1.Name = "Node0";
            treeNode1.Text = "Staged Files";
            this.treeCommits.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeCommits.SelectedNodes = ((System.Collections.Generic.List<System.Windows.Forms.TreeNode>)(resources.GetObject("treeCommits.SelectedNodes")));
            this.treeCommits.Size = new System.Drawing.Size(400, 375);
            this.treeCommits.TabIndex = 1;
            this.treeCommits.DragDrop += new System.Windows.Forms.DragEventHandler(this.TreeCommitsDragDrop);
            this.treeCommits.DragEnter += new System.Windows.Forms.DragEventHandler(TreeCommitsDragEnter);
            this.treeCommits.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TreeCommitsMouseMove);
            this.treeCommits.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TreeCommitsMouseUp);
            // 
            // PanelCommits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeCommits);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PanelCommits";
            this.Size = new System.Drawing.Size(400, 400);
            this.Tag = "Commits";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private MultiSelectTreeview.MultiSelectTreeview treeCommits;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
    }
}
