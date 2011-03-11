namespace Git4Win.Settings.Panels
{
    partial class ControlDiff
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
            this.listBoxDiffs = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(-3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(303, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Check a diff utility to use with git4win:";
            // 
            // listBoxDiffs
            // 
            this.listBoxDiffs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxDiffs.CheckOnClick = true;
            this.listBoxDiffs.FormattingEnabled = true;
            this.listBoxDiffs.IntegralHeight = false;
            this.listBoxDiffs.Location = new System.Drawing.Point(0, 32);
            this.listBoxDiffs.Name = "listBoxDiffs";
            this.listBoxDiffs.Size = new System.Drawing.Size(300, 265);
            this.listBoxDiffs.TabIndex = 5;
            this.listBoxDiffs.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ListBoxDiffsItemCheck);
            // 
            // ControlDiff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBoxDiffs);
            this.Controls.Add(this.label1);
            this.Name = "ControlDiff";
            this.Size = new System.Drawing.Size(300, 300);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox listBoxDiffs;
    }
}
