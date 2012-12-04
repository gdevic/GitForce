namespace GitForce
{
    partial class FormDeleteBranch
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
            this.checkForce = new System.Windows.Forms.CheckBox();
            this.listBranches = new System.Windows.Forms.ListBox();
            this.radioRemoteBranch = new System.Windows.Forms.RadioButton();
            this.radioLocalBranch = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btDelete = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkForce
            // 
            this.checkForce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkForce.AutoSize = true;
            this.checkForce.Location = new System.Drawing.Point(13, 162);
            this.checkForce.Name = "checkForce";
            this.checkForce.Size = new System.Drawing.Size(201, 17);
            this.checkForce.TabIndex = 13;
            this.checkForce.Text = "Force delete unmerged branch (\"-D\")";
            this.checkForce.UseVisualStyleBackColor = true;
            // 
            // listBranches
            // 
            this.listBranches.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBranches.FormattingEnabled = true;
            this.listBranches.IntegralHeight = false;
            this.listBranches.Location = new System.Drawing.Point(129, 39);
            this.listBranches.Name = "listBranches";
            this.listBranches.Size = new System.Drawing.Size(189, 117);
            this.listBranches.TabIndex = 12;
            this.listBranches.SelectedIndexChanged += new System.EventHandler(this.ListBranchesSelectedIndexChanged);
            // 
            // radioRemoteBranch
            // 
            this.radioRemoteBranch.AutoSize = true;
            this.radioRemoteBranch.Location = new System.Drawing.Point(13, 62);
            this.radioRemoteBranch.Name = "radioRemoteBranch";
            this.radioRemoteBranch.Size = new System.Drawing.Size(101, 17);
            this.radioRemoteBranch.TabIndex = 11;
            this.radioRemoteBranch.Tag = "Remote";
            this.radioRemoteBranch.Text = "Remote branch:";
            this.radioRemoteBranch.UseVisualStyleBackColor = true;
            this.radioRemoteBranch.CheckedChanged += new System.EventHandler(this.RadioButton1CheckedChanged);
            // 
            // radioLocalBranch
            // 
            this.radioLocalBranch.AutoSize = true;
            this.radioLocalBranch.Checked = true;
            this.radioLocalBranch.Location = new System.Drawing.Point(13, 39);
            this.radioLocalBranch.Name = "radioLocalBranch";
            this.radioLocalBranch.Size = new System.Drawing.Size(90, 17);
            this.radioLocalBranch.TabIndex = 10;
            this.radioLocalBranch.TabStop = true;
            this.radioLocalBranch.Tag = "Local";
            this.radioLocalBranch.Text = "Local branch:";
            this.radioLocalBranch.UseVisualStyleBackColor = true;
            this.radioLocalBranch.CheckedChanged += new System.EventHandler(this.RadioButton1CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Select a branch to delete:";
            // 
            // btDelete
            // 
            this.btDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btDelete.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btDelete.Enabled = false;
            this.btDelete.Location = new System.Drawing.Point(162, 185);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(75, 23);
            this.btDelete.TabIndex = 8;
            this.btDelete.Text = "Delete";
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.DeleteClick);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(243, 185);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 7;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // FormDeleteBranch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(329, 219);
            this.Controls.Add(this.checkForce);
            this.Controls.Add(this.listBranches);
            this.Controls.Add(this.radioRemoteBranch);
            this.Controls.Add(this.radioLocalBranch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btDelete);
            this.Controls.Add(this.btCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(276, 187);
            this.Name = "FormDeleteBranch";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Delete Branch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDeleteBranchFormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkForce;
        private System.Windows.Forms.ListBox listBranches;
        private System.Windows.Forms.RadioButton radioRemoteBranch;
        private System.Windows.Forms.RadioButton radioLocalBranch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.Button btCancel;
    }
}