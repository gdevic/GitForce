namespace git4win.FormOptions_Panels
{
    partial class ControlSsl
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
            this.checkBoxLeavePageant = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btSsh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "Git4Win uses PuTTY internally to manage SSL connections.";
            // 
            // checkBoxLeavePageant
            // 
            this.checkBoxLeavePageant.AutoSize = true;
            this.checkBoxLeavePageant.Location = new System.Drawing.Point(3, 70);
            this.checkBoxLeavePageant.Name = "checkBoxLeavePageant";
            this.checkBoxLeavePageant.Size = new System.Drawing.Size(289, 17);
            this.checkBoxLeavePageant.TabIndex = 3;
            this.checkBoxLeavePageant.Text = "Leave Pageant daemon running after git4win has exited";
            this.checkBoxLeavePageant.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Manage SSH keys:";
            // 
            // btSsh
            // 
            this.btSsh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btSsh.Location = new System.Drawing.Point(214, 41);
            this.btSsh.Name = "btSsh";
            this.btSsh.Size = new System.Drawing.Size(75, 23);
            this.btSsh.TabIndex = 4;
            this.btSsh.Text = "SSH...";
            this.btSsh.UseVisualStyleBackColor = true;
            this.btSsh.Click += new System.EventHandler(this.btSshClick);
            // 
            // ControlSsl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxLeavePageant);
            this.Controls.Add(this.btSsh);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ControlSsl";
            this.Size = new System.Drawing.Size(300, 300);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxLeavePageant;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btSsh;
    }
}
