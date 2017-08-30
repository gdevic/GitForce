namespace GitForce
{
    partial class FormHttps
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHttps));
            this.btHelp = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabNetrc = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listHosts = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btAddHost = new System.Windows.Forms.Button();
            this.btListHosts = new System.Windows.Forms.Button();
            this.textBoxHost = new System.Windows.Forms.TextBox();
            this.btRemoveHost = new System.Windows.Forms.Button();
            this.tabEmbedded = new System.Windows.Forms.TabPage();
            this.tabControl.SuspendLayout();
            this.tabNetrc.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btHelp
            // 
            this.btHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btHelp.Location = new System.Drawing.Point(362, 389);
            this.btHelp.Name = "btHelp";
            this.btHelp.Size = new System.Drawing.Size(75, 23);
            this.btHelp.TabIndex = 15;
            this.btHelp.Text = "Help";
            this.btHelp.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(443, 389);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 16;
            this.btOK.Text = "Done";
            this.btOK.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(506, 53);
            this.label1.TabIndex = 17;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabNetrc);
            this.tabControl.Controls.Add(this.tabEmbedded);
            this.tabControl.Location = new System.Drawing.Point(15, 65);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(503, 318);
            this.tabControl.TabIndex = 18;
            // 
            // tabNetrc
            // 
            this.tabNetrc.BackColor = System.Drawing.Color.Transparent;
            this.tabNetrc.Controls.Add(this.groupBox1);
            this.tabNetrc.Location = new System.Drawing.Point(4, 22);
            this.tabNetrc.Name = "tabNetrc";
            this.tabNetrc.Size = new System.Drawing.Size(495, 292);
            this.tabNetrc.TabIndex = 1;
            this.tabNetrc.Text = ".netrc";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.listHosts);
            this.groupBox1.Controls.Add(this.btAddHost);
            this.groupBox1.Controls.Add(this.btListHosts);
            this.groupBox1.Controls.Add(this.textBoxHost);
            this.groupBox1.Controls.Add(this.btRemoveHost);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(495, 292);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hosts";
            // 
            // listHosts
            // 
            this.listHosts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listHosts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listHosts.FullRowSelect = true;
            this.listHosts.Location = new System.Drawing.Point(6, 19);
            this.listHosts.Name = "listHosts";
            this.listHosts.Size = new System.Drawing.Size(402, 239);
            this.listHosts.TabIndex = 7;
            this.listHosts.UseCompatibleStateImageBehavior = false;
            this.listHosts.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Git server";
            this.columnHeader1.Width = 188;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "User name";
            this.columnHeader2.Width = 93;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Password";
            this.columnHeader3.Width = 117;
            // 
            // btAddHost
            // 
            this.btAddHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAddHost.Enabled = false;
            this.btAddHost.Location = new System.Drawing.Point(414, 263);
            this.btAddHost.Name = "btAddHost";
            this.btAddHost.Size = new System.Drawing.Size(75, 23);
            this.btAddHost.TabIndex = 6;
            this.btAddHost.Text = "Add Host";
            this.btAddHost.UseVisualStyleBackColor = true;
            // 
            // btListHosts
            // 
            this.btListHosts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btListHosts.Image = global::GitForce.Properties.Resources.pulldown;
            this.btListHosts.Location = new System.Drawing.Point(381, 264);
            this.btListHosts.Name = "btListHosts";
            this.btListHosts.Size = new System.Drawing.Size(27, 23);
            this.btListHosts.TabIndex = 5;
            this.btListHosts.Text = ".";
            this.btListHosts.UseVisualStyleBackColor = true;
            this.btListHosts.Click += new System.EventHandler(this.BtListHostsClick);
            // 
            // textBoxHost
            // 
            this.textBoxHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxHost.Location = new System.Drawing.Point(6, 266);
            this.textBoxHost.Name = "textBoxHost";
            this.textBoxHost.Size = new System.Drawing.Size(369, 20);
            this.textBoxHost.TabIndex = 4;
            // 
            // btRemoveHost
            // 
            this.btRemoveHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRemoveHost.Enabled = false;
            this.btRemoveHost.Location = new System.Drawing.Point(414, 19);
            this.btRemoveHost.Name = "btRemoveHost";
            this.btRemoveHost.Size = new System.Drawing.Size(75, 23);
            this.btRemoveHost.TabIndex = 2;
            this.btRemoveHost.Text = "Remove";
            this.btRemoveHost.UseVisualStyleBackColor = true;
            // 
            // tabEmbedded
            // 
            this.tabEmbedded.Location = new System.Drawing.Point(4, 22);
            this.tabEmbedded.Name = "tabEmbedded";
            this.tabEmbedded.Padding = new System.Windows.Forms.Padding(3);
            this.tabEmbedded.Size = new System.Drawing.Size(495, 292);
            this.tabEmbedded.TabIndex = 2;
            this.tabEmbedded.Text = "embedded";
            this.tabEmbedded.UseVisualStyleBackColor = true;
            // 
            // FormHttps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 424);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btHelp);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormHttps";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Manage HTTPS Passwords";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormHttpsFormClosing);
            this.tabControl.ResumeLayout(false);
            this.tabNetrc.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btHelp;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabNetrc;
        private System.Windows.Forms.TabPage tabEmbedded;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btRemoveHost;
        private System.Windows.Forms.TextBox textBoxHost;
        private System.Windows.Forms.Button btListHosts;
        private System.Windows.Forms.Button btAddHost;
        private System.Windows.Forms.ListView listHosts;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}