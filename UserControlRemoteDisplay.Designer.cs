namespace git4win
{
    partial class RemoteDisplay
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textPushCmd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btSsh = new System.Windows.Forms.Button();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textUrlPush = new System.Windows.Forms.TextBox();
            this.textUrlFetch = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textPushCmd);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btSsh);
            this.groupBox1.Controls.Add(this.textPassword);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textUrlPush);
            this.groupBox1.Controls.Add(this.textUrlFetch);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(422, 143);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // textPushCmd
            // 
            this.textPushCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textPushCmd.Location = new System.Drawing.Point(82, 91);
            this.textPushCmd.Name = "textPushCmd";
            this.textPushCmd.ReadOnly = true;
            this.textPushCmd.Size = new System.Drawing.Size(334, 20);
            this.textPushCmd.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Cmd> push";
            // 
            // btSsh
            // 
            this.btSsh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSsh.Enabled = false;
            this.btSsh.Location = new System.Drawing.Point(275, 116);
            this.btSsh.Name = "btSsh";
            this.btSsh.Size = new System.Drawing.Size(141, 23);
            this.btSsh.TabIndex = 5;
            this.btSsh.Text = "Import Remote SSH Key";
            this.btSsh.UseVisualStyleBackColor = true;
            this.btSsh.Click += new System.EventHandler(this.BtSshClick);
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(82, 117);
            this.textPassword.MaxLength = 32;
            this.textPassword.Name = "textPassword";
            this.textPassword.PasswordChar = '*';
            this.textPassword.ReadOnly = true;
            this.textPassword.Size = new System.Drawing.Size(111, 20);
            this.textPassword.TabIndex = 4;
            this.textPassword.TextChanged += new System.EventHandler(this.SomeTextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "HTTPS Pwd:";
            // 
            // textUrlPush
            // 
            this.textUrlPush.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textUrlPush.Location = new System.Drawing.Point(82, 65);
            this.textUrlPush.MaxLength = 128;
            this.textUrlPush.Name = "textUrlPush";
            this.textUrlPush.ReadOnly = true;
            this.textUrlPush.Size = new System.Drawing.Size(334, 20);
            this.textUrlPush.TabIndex = 2;
            this.textUrlPush.TextChanged += new System.EventHandler(this.SomeTextChanged);
            // 
            // textUrlFetch
            // 
            this.textUrlFetch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textUrlFetch.Location = new System.Drawing.Point(82, 39);
            this.textUrlFetch.MaxLength = 128;
            this.textUrlFetch.Name = "textUrlFetch";
            this.textUrlFetch.ReadOnly = true;
            this.textUrlFetch.Size = new System.Drawing.Size(334, 20);
            this.textUrlFetch.TabIndex = 1;
            this.textUrlFetch.TextChanged += new System.EventHandler(this.SomeTextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Push URL:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Fetch URL:";
            // 
            // textName
            // 
            this.textName.Location = new System.Drawing.Point(82, 13);
            this.textName.MaxLength = 32;
            this.textName.Name = "textName";
            this.textName.ReadOnly = true;
            this.textName.Size = new System.Drawing.Size(111, 20);
            this.textName.TabIndex = 0;
            this.textName.TextChanged += new System.EventHandler(this.SomeTextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Name:";
            // 
            // RemoteDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(346, 146);
            this.Name = "RemoteDisplay";
            this.Size = new System.Drawing.Size(425, 146);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textUrlPush;
        private System.Windows.Forms.TextBox textUrlFetch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btSsh;
        private System.Windows.Forms.TextBox textPushCmd;
        private System.Windows.Forms.Label label1;
    }
}
