namespace git4win.FormOptions_Panels
{
    partial class ControlMerge
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
            this.Merge = new git4win.UserControlBranchMergeStyle();
            this.SuspendLayout();
            // 
            // userControlBranchMergeType1
            // 
            this.Merge.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Merge.Location = new System.Drawing.Point(3, 3);
            this.Merge.Name = "userControlBranchMergeType1";
            this.Merge.Size = new System.Drawing.Size(294, 294);
            this.Merge.TabIndex = 0;
            // 
            // ControlMerge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Merge);
            this.Name = "ControlMerge";
            this.Size = new System.Drawing.Size(300, 300);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControlBranchMergeStyle Merge;
    }
}
