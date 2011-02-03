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
            this.btPutty = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxLeavePageant = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btPutty
            // 
            this.btPutty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btPutty.Location = new System.Drawing.Point(225, 34);
            this.btPutty.Name = "btPutty";
            this.btPutty.Size = new System.Drawing.Size(75, 23);
            this.btPutty.TabIndex = 0;
            this.btPutty.Text = "PuTTYgen";
            this.btPutty.UseVisualStyleBackColor = true;
            this.btPutty.Click += new System.EventHandler(this.btPutty_Click);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-3, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(185, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Configure SSL by running PuTTYgen:";
            // 
            // checkBoxLeavePageant
            // 
            this.checkBoxLeavePageant.AutoSize = true;
            this.checkBoxLeavePageant.Location = new System.Drawing.Point(0, 63);
            this.checkBoxLeavePageant.Name = "checkBoxLeavePageant";
            this.checkBoxLeavePageant.Size = new System.Drawing.Size(289, 17);
            this.checkBoxLeavePageant.TabIndex = 3;
            this.checkBoxLeavePageant.Text = "Leave Pageant daemon running after git4win has exited";
            this.checkBoxLeavePageant.UseVisualStyleBackColor = true;
            // 
            // ControlSsl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxLeavePageant);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btPutty);
            this.Name = "ControlSsl";
            this.Size = new System.Drawing.Size(300, 300);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btPutty;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxLeavePageant;
    }
}
