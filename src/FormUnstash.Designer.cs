namespace GitForce
{
    partial class FormUnstash
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
            this.btCancel = new System.Windows.Forms.Button();
            this.btApply = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listStashes = new System.Windows.Forms.ListBox();
            this.checkKeepStash = new System.Windows.Forms.CheckBox();
            this.btRemove = new System.Windows.Forms.Button();
            this.btShow = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(298, 133);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 0;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btApply
            // 
            this.btApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btApply.Enabled = false;
            this.btApply.Location = new System.Drawing.Point(217, 133);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(75, 23);
            this.btApply.TabIndex = 1;
            this.btApply.Text = "Apply";
            this.btApply.UseVisualStyleBackColor = true;
            this.btApply.Click += new System.EventHandler(this.BtUnstashClick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(361, 34);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select which stash do you want to appy to your current working set.";
            // 
            // listStashes
            // 
            this.listStashes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listStashes.FormattingEnabled = true;
            this.listStashes.IntegralHeight = false;
            this.listStashes.Location = new System.Drawing.Point(12, 46);
            this.listStashes.Name = "listStashes";
            this.listStashes.Size = new System.Drawing.Size(280, 81);
            this.listStashes.TabIndex = 3;
            this.listStashes.SelectedIndexChanged += new System.EventHandler(this.ListStashesSelectedIndexChanged);
            // 
            // checkKeepStash
            // 
            this.checkKeepStash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkKeepStash.AutoSize = true;
            this.checkKeepStash.Location = new System.Drawing.Point(12, 137);
            this.checkKeepStash.Name = "checkKeepStash";
            this.checkKeepStash.Size = new System.Drawing.Size(168, 17);
            this.checkKeepStash.TabIndex = 4;
            this.checkKeepStash.Text = "Do not remove stash on Apply";
            this.checkKeepStash.UseVisualStyleBackColor = true;
            // 
            // btRemove
            // 
            this.btRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRemove.Enabled = false;
            this.btRemove.Location = new System.Drawing.Point(298, 46);
            this.btRemove.Name = "btRemove";
            this.btRemove.Size = new System.Drawing.Size(75, 23);
            this.btRemove.TabIndex = 5;
            this.btRemove.Text = "Remove";
            this.btRemove.UseVisualStyleBackColor = true;
            this.btRemove.Click += new System.EventHandler(this.BtRemoveClick);
            // 
            // btShow
            // 
            this.btShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btShow.Enabled = false;
            this.btShow.Location = new System.Drawing.Point(298, 75);
            this.btShow.Name = "btShow";
            this.btShow.Size = new System.Drawing.Size(75, 23);
            this.btShow.TabIndex = 6;
            this.btShow.Text = "Show";
            this.btShow.UseVisualStyleBackColor = true;
            this.btShow.Click += new System.EventHandler(this.BtShowClick);
            // 
            // FormUnstash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(385, 168);
            this.Controls.Add(this.btShow);
            this.Controls.Add(this.btRemove);
            this.Controls.Add(this.checkKeepStash);
            this.Controls.Add(this.listStashes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btApply);
            this.Controls.Add(this.btCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(364, 177);
            this.Name = "FormUnstash";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Unstash";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormUnstashFormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btApply;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listStashes;
        private System.Windows.Forms.CheckBox checkKeepStash;
        private System.Windows.Forms.Button btRemove;
        private System.Windows.Forms.Button btShow;
    }
}