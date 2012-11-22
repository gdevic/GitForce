namespace GitForce
{
    partial class FormRepoEdit
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("User");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Gitignore");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Git config");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Repo Objects", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.panel = new System.Windows.Forms.Panel();
            this.treeSections = new System.Windows.Forms.TreeView();
            this.btApply = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.Location = new System.Drawing.Point(176, 12);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(321, 201);
            this.panel.TabIndex = 9;
            // 
            // treeSections
            // 
            this.treeSections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treeSections.HideSelection = false;
            this.treeSections.Location = new System.Drawing.Point(12, 12);
            this.treeSections.Name = "treeSections";
            treeNode1.Name = "Node1";
            treeNode1.Tag = "User";
            treeNode1.Text = "User";
            treeNode2.Name = "Node2";
            treeNode2.Tag = "Gitignore";
            treeNode2.Text = "Gitignore";
            treeNode3.Name = "Node0";
            treeNode3.Tag = "Gitconfig";
            treeNode3.Text = "Git config";
            treeNode4.Name = "Node0";
            treeNode4.Tag = "Local";
            treeNode4.Text = "Repo Objects";
            this.treeSections.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4});
            this.treeSections.Size = new System.Drawing.Size(158, 230);
            this.treeSections.TabIndex = 5;
            this.treeSections.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeSectionsAfterSelect);
            // 
            // btApply
            // 
            this.btApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btApply.Location = new System.Drawing.Point(260, 219);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(75, 23);
            this.btApply.TabIndex = 6;
            this.btApply.Text = "Apply";
            this.btApply.UseVisualStyleBackColor = true;
            this.btApply.Click += new System.EventHandler(this.BtApplyClick);
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(341, 219);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 7;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.BtOkClick);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(422, 219);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 8;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // FormRepoEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 254);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.treeSections);
            this.Controls.Add(this.btApply);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(525, 271);
            this.Name = "FormRepoEdit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Edit Repository";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRepoEditFormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.TreeView treeSections;
        private System.Windows.Forms.Button btApply;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
    }
}