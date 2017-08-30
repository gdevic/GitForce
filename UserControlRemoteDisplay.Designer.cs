using System.Windows.Forms;

namespace GitForce
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
            this.btSsh = new System.Windows.Forms.Button();
            this.textPushCmd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textUrlPush = new System.Windows.Forms.TextBox();
            this.textUrlFetch = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btHttpsAuth = new System.Windows.Forms.Button();
            this.btListPush = new System.Windows.Forms.Button();
            this.btListFetch = new System.Windows.Forms.Button();
            this.btWWW2 = new System.Windows.Forms.Button();
            this.btWWW1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btSsh
            // 
            this.btSsh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSsh.Location = new System.Drawing.Point(341, 115);
            this.btSsh.Name = "btSsh";
            this.btSsh.Size = new System.Drawing.Size(75, 23);
            this.btSsh.TabIndex = 15;
            this.btSsh.Text = "SSH...";
            this.btSsh.UseVisualStyleBackColor = true;
            this.btSsh.Click += new System.EventHandler(this.BtSshClick);
            // 
            // textPushCmd
            // 
            this.textPushCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textPushCmd.Location = new System.Drawing.Point(82, 91);
            this.textPushCmd.Name = "textPushCmd";
            this.textPushCmd.ReadOnly = true;
            this.textPushCmd.Size = new System.Drawing.Size(334, 20);
            this.textPushCmd.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Cmd> push";
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(82, 117);
            this.textPassword.MaxLength = 32;
            this.textPassword.Name = "textPassword";
            this.textPassword.ReadOnly = true;
            this.textPassword.Size = new System.Drawing.Size(111, 20);
            this.textPassword.TabIndex = 13;
            this.textPassword.UseSystemPasswordChar = true;
            this.textPassword.TextChanged += new System.EventHandler(this.SomeTextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 12;
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
            this.textUrlPush.Size = new System.Drawing.Size(265, 20);
            this.textUrlPush.TabIndex = 7;
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
            this.textUrlFetch.Size = new System.Drawing.Size(265, 20);
            this.textUrlFetch.TabIndex = 3;
            this.textUrlFetch.TextChanged += new System.EventHandler(this.SomeTextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btHttpsAuth);
            this.groupBox1.Controls.Add(this.btListPush);
            this.groupBox1.Controls.Add(this.btListFetch);
            this.groupBox1.Controls.Add(this.btWWW2);
            this.groupBox1.Controls.Add(this.btWWW1);
            this.groupBox1.Controls.Add(this.btSsh);
            this.groupBox1.Controls.Add(this.textPushCmd);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textPassword);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textUrlPush);
            this.groupBox1.Controls.Add(this.textUrlFetch);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(422, 143);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btHttpsAuth
            // 
            this.btHttpsAuth.Image = global::GitForce.Properties.Resources.key;
            this.btHttpsAuth.Location = new System.Drawing.Point(199, 115);
            this.btHttpsAuth.Name = "btHttpsAuth";
            this.btHttpsAuth.Size = new System.Drawing.Size(27, 23);
            this.btHttpsAuth.TabIndex = 14;
            this.btHttpsAuth.UseVisualStyleBackColor = true;
            this.btHttpsAuth.Click += new System.EventHandler(this.BtHttpsClicked);
            // 
            // btListPush
            // 
            this.btListPush.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btListPush.Image = global::GitForce.Properties.Resources.pulldown;
            this.btListPush.Location = new System.Drawing.Point(353, 63);
            this.btListPush.Name = "btListPush";
            this.btListPush.Size = new System.Drawing.Size(27, 23);
            this.btListPush.TabIndex = 8;
            this.btListPush.UseVisualStyleBackColor = true;
            this.btListPush.Click += new System.EventHandler(this.BtPushClicked);
            // 
            // btListFetch
            // 
            this.btListFetch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btListFetch.Image = global::GitForce.Properties.Resources.pulldown;
            this.btListFetch.Location = new System.Drawing.Point(353, 37);
            this.btListFetch.Name = "btListFetch";
            this.btListFetch.Size = new System.Drawing.Size(27, 23);
            this.btListFetch.TabIndex = 4;
            this.btListFetch.UseVisualStyleBackColor = true;
            this.btListFetch.Click += new System.EventHandler(this.BtFetchClicked);
            // 
            // btWWW2
            // 
            this.btWWW2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btWWW2.Enabled = false;
            this.btWWW2.Image = global::GitForce.Properties.Resources.world;
            this.btWWW2.Location = new System.Drawing.Point(386, 63);
            this.btWWW2.Name = "btWWW2";
            this.btWWW2.Size = new System.Drawing.Size(30, 23);
            this.btWWW2.TabIndex = 9;
            this.btWWW2.Tag = "Push";
            this.btWWW2.UseVisualStyleBackColor = true;
            this.btWWW2.Click += new System.EventHandler(this.BtWwwClick);
            // 
            // btWWW1
            // 
            this.btWWW1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btWWW1.Enabled = false;
            this.btWWW1.Image = global::GitForce.Properties.Resources.world;
            this.btWWW1.Location = new System.Drawing.Point(386, 37);
            this.btWWW1.Name = "btWWW1";
            this.btWWW1.Size = new System.Drawing.Size(30, 23);
            this.btWWW1.TabIndex = 5;
            this.btWWW1.Tag = "Fetch";
            this.btWWW1.UseVisualStyleBackColor = true;
            this.btWWW1.Click += new System.EventHandler(this.BtWwwClick);
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
            this.label3.TabIndex = 2;
            this.label3.Text = "Fetch URL:";
            // 
            // textName
            // 
            this.textName.Location = new System.Drawing.Point(82, 13);
            this.textName.MaxLength = 32;
            this.textName.Name = "textName";
            this.textName.ReadOnly = true;
            this.textName.Size = new System.Drawing.Size(111, 20);
            this.textName.TabIndex = 1;
            this.textName.TextChanged += new System.EventHandler(this.SomeTextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Name:";
            // 
            // RemoteDisplay
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(346, 146);
            this.Name = "RemoteDisplay";
            this.Size = new System.Drawing.Size(425, 146);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button btSsh;
        private System.Windows.Forms.TextBox textPushCmd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textUrlPush;
        private System.Windows.Forms.TextBox textUrlFetch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btWWW2;
        private System.Windows.Forms.Button btWWW1;
        private System.Windows.Forms.Button btListFetch;
        private System.Windows.Forms.Button btListPush;
        private System.Windows.Forms.Button btHttpsAuth;
    }
}
