namespace Git4Win.Main.Right.Panels
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
            this.btBranch});
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
            // 
            // masterToolStripMenuItem
            // 
            this.masterToolStripMenuItem.Name = "masterToolStripMenuItem";
            this.masterToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.masterToolStripMenuItem.Text = "master";
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
            this.contextMenu.Size = new System.Drawing.Size(133, 26);
            // 
            // menuItem
            // 
            this.menuItem.Name = "menuItem";
            this.menuItem.Size = new System.Drawing.Size(152, 22);
            this.menuItem.Text = "Menu Item";
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
    }
}
