namespace GitForce.Repo.Edit.Panels
{
    partial class ControlRemotes
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
            this.label1 = new System.Windows.Forms.Label();
            this.listRemotes = new System.Windows.Forms.ListBox();
            this.btEdit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 30);
            this.label1.TabIndex = 14;
            this.label1.Text = "Show and edit remote git repositories for this repo.";
            // 
            // listRemotes
            // 
            this.listRemotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listRemotes.FormattingEnabled = true;
            this.listRemotes.IntegralHeight = false;
            this.listRemotes.Location = new System.Drawing.Point(3, 33);
            this.listRemotes.Name = "listRemotes";
            this.listRemotes.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listRemotes.Size = new System.Drawing.Size(294, 235);
            this.listRemotes.TabIndex = 15;
            // 
            // btEdit
            // 
            this.btEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btEdit.Location = new System.Drawing.Point(3, 274);
            this.btEdit.Name = "btEdit";
            this.btEdit.Size = new System.Drawing.Size(294, 23);
            this.btEdit.TabIndex = 16;
            this.btEdit.Text = "Edit Remotes";
            this.btEdit.UseVisualStyleBackColor = true;
            this.btEdit.Click += new System.EventHandler(this.BtEditClick);
            // 
            // ControlRemotes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btEdit);
            this.Controls.Add(this.listRemotes);
            this.Controls.Add(this.label1);
            this.Name = "ControlRemotes";
            this.Size = new System.Drawing.Size(300, 300);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listRemotes;
        private System.Windows.Forms.Button btEdit;

    }
}
