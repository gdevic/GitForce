namespace git4win
{
    partial class FormMergeBranch
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
            this.labelCurrentBranchName = new System.Windows.Forms.Label();
            this.listBranches = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btMerge = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelCurrentBranchName
            // 
            this.labelCurrentBranchName.AutoSize = true;
            this.labelCurrentBranchName.Location = new System.Drawing.Point(12, 12);
            this.labelCurrentBranchName.Name = "labelCurrentBranchName";
            this.labelCurrentBranchName.Size = new System.Drawing.Size(80, 13);
            this.labelCurrentBranchName.TabIndex = 1;
            this.labelCurrentBranchName.Text = "Current branch:";
            // 
            // listBranches
            // 
            this.listBranches.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBranches.FormattingEnabled = true;
            this.listBranches.IntegralHeight = false;
            this.listBranches.Location = new System.Drawing.Point(147, 29);
            this.listBranches.Name = "listBranches";
            this.listBranches.Size = new System.Drawing.Size(185, 117);
            this.listBranches.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Select a branch to merge:";
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(257, 152);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btMerge
            // 
            this.btMerge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btMerge.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btMerge.Enabled = false;
            this.btMerge.Location = new System.Drawing.Point(176, 152);
            this.btMerge.Name = "btMerge";
            this.btMerge.Size = new System.Drawing.Size(75, 23);
            this.btMerge.TabIndex = 5;
            this.btMerge.Text = "Merge";
            this.btMerge.UseVisualStyleBackColor = true;
            // 
            // FormMergeBranch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 187);
            this.Controls.Add(this.btMerge);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBranches);
            this.Controls.Add(this.labelCurrentBranchName);
            this.MinimumSize = new System.Drawing.Size(283, 143);
            this.Name = "FormMergeBranch";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Merge Branch";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCurrentBranchName;
        private System.Windows.Forms.ListBox listBranches;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btMerge;
    }
}