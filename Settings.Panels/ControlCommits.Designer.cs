namespace Git4Win.Settings.Panels
{
    partial class ControlCommits
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
            this.textBoxLast = new System.Windows.Forms.TextBox();
            this.rbRetrieveLast = new System.Windows.Forms.RadioButton();
            this.rbRetrieveAll = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxLast
            // 
            this.textBoxLast.Location = new System.Drawing.Point(93, 54);
            this.textBoxLast.MaxLength = 5;
            this.textBoxLast.Name = "textBoxLast";
            this.textBoxLast.Size = new System.Drawing.Size(45, 20);
            this.textBoxLast.TabIndex = 2;
            // 
            // rbRetrieveLast
            // 
            this.rbRetrieveLast.AutoSize = true;
            this.rbRetrieveLast.Location = new System.Drawing.Point(3, 55);
            this.rbRetrieveLast.Name = "rbRetrieveLast";
            this.rbRetrieveLast.Size = new System.Drawing.Size(87, 17);
            this.rbRetrieveLast.TabIndex = 1;
            this.rbRetrieveLast.TabStop = true;
            this.rbRetrieveLast.Text = "Retrieve last:";
            this.rbRetrieveLast.UseVisualStyleBackColor = true;
            this.rbRetrieveLast.CheckedChanged += new System.EventHandler(this.RbRetrieveAllCheckedChanged);
            // 
            // rbRetrieveAll
            // 
            this.rbRetrieveAll.AutoSize = true;
            this.rbRetrieveAll.Location = new System.Drawing.Point(3, 32);
            this.rbRetrieveAll.Name = "rbRetrieveAll";
            this.rbRetrieveAll.Size = new System.Drawing.Size(78, 17);
            this.rbRetrieveAll.TabIndex = 0;
            this.rbRetrieveAll.TabStop = true;
            this.rbRetrieveAll.Text = "Retrieve all";
            this.rbRetrieveAll.UseVisualStyleBackColor = true;
            this.rbRetrieveAll.CheckedChanged += new System.EventHandler(this.RbRetrieveAllCheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(-3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(303, 29);
            this.label1.TabIndex = 3;
            this.label1.Text = "When retrieving submitted commits from the repo:";
            // 
            // ControlCommits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxLast);
            this.Controls.Add(this.rbRetrieveLast);
            this.Controls.Add(this.rbRetrieveAll);
            this.Name = "ControlCommits";
            this.Size = new System.Drawing.Size(300, 300);
            this.Tag = "Commits";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxLast;
        private System.Windows.Forms.RadioButton rbRetrieveLast;
        private System.Windows.Forms.RadioButton rbRetrieveAll;
        private System.Windows.Forms.Label label1;
    }
}
