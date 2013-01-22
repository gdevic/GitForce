namespace GitForce
{
    partial class FormSSH
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSSH));
            this.textBoxHost = new System.Windows.Forms.TextBox();
            this.btImport2 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBoxKeys = new System.Windows.Forms.ListBox();
            this.byAdd = new System.Windows.Forms.Button();
            this.btImport1 = new System.Windows.Forms.Button();
            this.btRemove = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxInputPf = new System.Windows.Forms.TextBox();
            this.listBoxPf = new System.Windows.Forms.ListBox();
            this.btAddPf = new System.Windows.Forms.Button();
            this.btRemovePf = new System.Windows.Forms.Button();
            this.btShowPf = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabLocalKeys = new System.Windows.Forms.TabPage();
            this.tabRemoteKeys = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btTestHost = new System.Windows.Forms.Button();
            this.btRemoveHost = new System.Windows.Forms.Button();
            this.btAddHost = new System.Windows.Forms.Button();
            this.listHosts = new System.Windows.Forms.ListBox();
            this.btPuttygen = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btOK = new System.Windows.Forms.Button();
            this.btHelp = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabLocalKeys.SuspendLayout();
            this.tabRemoteKeys.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxHost
            // 
            this.textBoxHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxHost.Location = new System.Drawing.Point(6, 259);
            this.textBoxHost.Name = "textBoxHost";
            this.textBoxHost.Size = new System.Drawing.Size(399, 20);
            this.textBoxHost.TabIndex = 1;
            this.textBoxHost.TextChanged += new System.EventHandler(this.TextBoxHostTextChanged);
            // 
            // btImport2
            // 
            this.btImport2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btImport2.Location = new System.Drawing.Point(411, 104);
            this.btImport2.Name = "btImport2";
            this.btImport2.Size = new System.Drawing.Size(75, 23);
            this.btImport2.TabIndex = 5;
            this.btImport2.Text = "Import Now";
            this.btImport2.UseVisualStyleBackColor = true;
            this.btImport2.Click += new System.EventHandler(this.BtImportClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBoxKeys);
            this.groupBox2.Controls.Add(this.byAdd);
            this.groupBox2.Controls.Add(this.btImport1);
            this.groupBox2.Controls.Add(this.btRemove);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(492, 135);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select private keys to import";
            // 
            // listBoxKeys
            // 
            this.listBoxKeys.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxKeys.FormattingEnabled = true;
            this.listBoxKeys.HorizontalScrollbar = true;
            this.listBoxKeys.IntegralHeight = false;
            this.listBoxKeys.Location = new System.Drawing.Point(6, 19);
            this.listBoxKeys.Name = "listBoxKeys";
            this.listBoxKeys.Size = new System.Drawing.Size(399, 111);
            this.listBoxKeys.TabIndex = 0;
            this.listBoxKeys.SelectedIndexChanged += new System.EventHandler(this.ListBoxKeysSelectedIndexChanged);
            this.listBoxKeys.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListBoxKeysMouseDown);
            // 
            // byAdd
            // 
            this.byAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.byAdd.Location = new System.Drawing.Point(411, 19);
            this.byAdd.Name = "byAdd";
            this.byAdd.Size = new System.Drawing.Size(75, 23);
            this.byAdd.TabIndex = 1;
            this.byAdd.Text = "Add...";
            this.byAdd.UseVisualStyleBackColor = true;
            this.byAdd.Click += new System.EventHandler(this.ByAddClick);
            // 
            // btImport1
            // 
            this.btImport1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btImport1.Location = new System.Drawing.Point(411, 77);
            this.btImport1.Name = "btImport1";
            this.btImport1.Size = new System.Drawing.Size(75, 23);
            this.btImport1.TabIndex = 3;
            this.btImport1.Text = "Import Now";
            this.btImport1.UseVisualStyleBackColor = true;
            this.btImport1.Click += new System.EventHandler(this.BtImportClick);
            // 
            // btRemove
            // 
            this.btRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRemove.Enabled = false;
            this.btRemove.Location = new System.Drawing.Point(411, 48);
            this.btRemove.Name = "btRemove";
            this.btRemove.Size = new System.Drawing.Size(75, 23);
            this.btRemove.TabIndex = 2;
            this.btRemove.Text = "Remove";
            this.btRemove.UseVisualStyleBackColor = true;
            this.btRemove.Click += new System.EventHandler(this.BtRemoveClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(492, 286);
            this.splitContainer1.SplitterDistance = 135;
            this.splitContainer1.TabIndex = 15;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btImport2);
            this.groupBox1.Controls.Add(this.textBoxInputPf);
            this.groupBox1.Controls.Add(this.listBoxPf);
            this.groupBox1.Controls.Add(this.btAddPf);
            this.groupBox1.Controls.Add(this.btRemovePf);
            this.groupBox1.Controls.Add(this.btShowPf);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(492, 147);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input the passphrases to import";
            // 
            // textBoxInputPf
            // 
            this.textBoxInputPf.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInputPf.Location = new System.Drawing.Point(6, 19);
            this.textBoxInputPf.MaxLength = 32;
            this.textBoxInputPf.Name = "textBoxInputPf";
            this.textBoxInputPf.Size = new System.Drawing.Size(399, 20);
            this.textBoxInputPf.TabIndex = 0;
            this.textBoxInputPf.UseSystemPasswordChar = true;
            this.textBoxInputPf.TextChanged += new System.EventHandler(this.TextBoxInputPfTextChanged);
            // 
            // listBoxPf
            // 
            this.listBoxPf.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxPf.FormattingEnabled = true;
            this.listBoxPf.IntegralHeight = false;
            this.listBoxPf.Location = new System.Drawing.Point(6, 45);
            this.listBoxPf.Name = "listBoxPf";
            this.listBoxPf.Size = new System.Drawing.Size(399, 96);
            this.listBoxPf.TabIndex = 1;
            this.listBoxPf.SelectedIndexChanged += new System.EventHandler(this.ListBoxPfSelectedIndexChanged);
            this.listBoxPf.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListBoxPfMouseDown);
            // 
            // btAddPf
            // 
            this.btAddPf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAddPf.Enabled = false;
            this.btAddPf.Location = new System.Drawing.Point(411, 17);
            this.btAddPf.Name = "btAddPf";
            this.btAddPf.Size = new System.Drawing.Size(75, 23);
            this.btAddPf.TabIndex = 2;
            this.btAddPf.Text = "Add";
            this.btAddPf.UseVisualStyleBackColor = true;
            this.btAddPf.Click += new System.EventHandler(this.BtAddPClick);
            // 
            // btRemovePf
            // 
            this.btRemovePf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRemovePf.Enabled = false;
            this.btRemovePf.Location = new System.Drawing.Point(411, 46);
            this.btRemovePf.Name = "btRemovePf";
            this.btRemovePf.Size = new System.Drawing.Size(75, 23);
            this.btRemovePf.TabIndex = 3;
            this.btRemovePf.Text = "Remove";
            this.btRemovePf.UseVisualStyleBackColor = true;
            this.btRemovePf.Click += new System.EventHandler(this.BtRemovePClick);
            // 
            // btShowPf
            // 
            this.btShowPf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btShowPf.Location = new System.Drawing.Point(411, 75);
            this.btShowPf.Name = "btShowPf";
            this.btShowPf.Size = new System.Drawing.Size(75, 23);
            this.btShowPf.TabIndex = 4;
            this.btShowPf.Text = "Show";
            this.btShowPf.UseVisualStyleBackColor = true;
            this.btShowPf.Click += new System.EventHandler(this.BtShowClick);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabLocalKeys);
            this.tabControl.Controls.Add(this.tabRemoteKeys);
            this.tabControl.Location = new System.Drawing.Point(12, 67);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(506, 318);
            this.tabControl.TabIndex = 0;
            // 
            // tabLocalKeys
            // 
            this.tabLocalKeys.BackColor = System.Drawing.SystemColors.Control;
            this.tabLocalKeys.Controls.Add(this.splitContainer1);
            this.tabLocalKeys.Location = new System.Drawing.Point(4, 22);
            this.tabLocalKeys.Name = "tabLocalKeys";
            this.tabLocalKeys.Padding = new System.Windows.Forms.Padding(3);
            this.tabLocalKeys.Size = new System.Drawing.Size(498, 292);
            this.tabLocalKeys.TabIndex = 0;
            this.tabLocalKeys.Text = "Local Keys";
            // 
            // tabRemoteKeys
            // 
            this.tabRemoteKeys.BackColor = System.Drawing.SystemColors.Control;
            this.tabRemoteKeys.Controls.Add(this.groupBox3);
            this.tabRemoteKeys.Location = new System.Drawing.Point(4, 22);
            this.tabRemoteKeys.Name = "tabRemoteKeys";
            this.tabRemoteKeys.Padding = new System.Windows.Forms.Padding(3);
            this.tabRemoteKeys.Size = new System.Drawing.Size(498, 292);
            this.tabRemoteKeys.TabIndex = 1;
            this.tabRemoteKeys.Text = "Remote Keys";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btTestHost);
            this.groupBox3.Controls.Add(this.btRemoveHost);
            this.groupBox3.Controls.Add(this.btAddHost);
            this.groupBox3.Controls.Add(this.textBoxHost);
            this.groupBox3.Controls.Add(this.listHosts);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(492, 286);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Known hosts";
            // 
            // btTestHost
            // 
            this.btTestHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btTestHost.Enabled = false;
            this.btTestHost.Location = new System.Drawing.Point(411, 48);
            this.btTestHost.Name = "btTestHost";
            this.btTestHost.Size = new System.Drawing.Size(75, 23);
            this.btTestHost.TabIndex = 4;
            this.btTestHost.Text = "Test";
            this.btTestHost.UseVisualStyleBackColor = true;
            this.btTestHost.Click += new System.EventHandler(this.BtTestHostClick);
            // 
            // btRemoveHost
            // 
            this.btRemoveHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRemoveHost.Enabled = false;
            this.btRemoveHost.Location = new System.Drawing.Point(411, 19);
            this.btRemoveHost.Name = "btRemoveHost";
            this.btRemoveHost.Size = new System.Drawing.Size(75, 23);
            this.btRemoveHost.TabIndex = 3;
            this.btRemoveHost.Text = "Remove";
            this.btRemoveHost.UseVisualStyleBackColor = true;
            this.btRemoveHost.Click += new System.EventHandler(this.BtRemoveHostClick);
            // 
            // btAddHost
            // 
            this.btAddHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAddHost.Enabled = false;
            this.btAddHost.Location = new System.Drawing.Point(411, 257);
            this.btAddHost.Name = "btAddHost";
            this.btAddHost.Size = new System.Drawing.Size(75, 23);
            this.btAddHost.TabIndex = 2;
            this.btAddHost.Text = "Add Host";
            this.btAddHost.UseVisualStyleBackColor = true;
            this.btAddHost.Click += new System.EventHandler(this.BtAddHostClick);
            // 
            // listHosts
            // 
            this.listHosts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listHosts.FormattingEnabled = true;
            this.listHosts.IntegralHeight = false;
            this.listHosts.Location = new System.Drawing.Point(6, 19);
            this.listHosts.Name = "listHosts";
            this.listHosts.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listHosts.Size = new System.Drawing.Size(399, 232);
            this.listHosts.TabIndex = 0;
            this.listHosts.SelectedIndexChanged += new System.EventHandler(this.ListHostsSelectedIndexChanged);
            this.listHosts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListHostsMouseDown);
            // 
            // btPuttygen
            // 
            this.btPuttygen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btPuttygen.Location = new System.Drawing.Point(12, 391);
            this.btPuttygen.Name = "btPuttygen";
            this.btPuttygen.Size = new System.Drawing.Size(75, 23);
            this.btPuttygen.TabIndex = 1;
            this.btPuttygen.Text = "PuTTYgen";
            this.btPuttygen.UseVisualStyleBackColor = true;
            this.btPuttygen.Click += new System.EventHandler(this.BtPuttygenClick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(506, 53);
            this.label1.TabIndex = 12;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "ppk";
            this.openFileDialog.Filter = "\"PuTTY Keys|*.ppk|All files|*.*\"";
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(443, 391);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 3;
            this.btOK.Text = "Done";
            this.btOK.UseVisualStyleBackColor = true;
            // 
            // btHelp
            // 
            this.btHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btHelp.Location = new System.Drawing.Point(362, 391);
            this.btHelp.Name = "btHelp";
            this.btHelp.Size = new System.Drawing.Size(75, 23);
            this.btHelp.TabIndex = 2;
            this.btHelp.Text = "Help";
            this.btHelp.UseVisualStyleBackColor = true;
            this.btHelp.Click += new System.EventHandler(this.BtHelpClick);
            // 
            // FormSSH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 424);
            this.Controls.Add(this.btHelp);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btPuttygen);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(280, 428);
            this.Name = "FormSSH";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Manage SSH Keys";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSshFormClosing);
            this.groupBox2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabLocalKeys.ResumeLayout(false);
            this.tabRemoteKeys.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxHost;
        private System.Windows.Forms.Button btImport2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listBoxKeys;
        private System.Windows.Forms.Button byAdd;
        private System.Windows.Forms.Button btImport1;
        private System.Windows.Forms.Button btRemove;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxInputPf;
        private System.Windows.Forms.ListBox listBoxPf;
        private System.Windows.Forms.Button btAddPf;
        private System.Windows.Forms.Button btRemovePf;
        private System.Windows.Forms.Button btShowPf;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabLocalKeys;
        private System.Windows.Forms.TabPage tabRemoteKeys;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btAddHost;
        private System.Windows.Forms.ListBox listHosts;
        private System.Windows.Forms.Button btPuttygen;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btHelp;
        private System.Windows.Forms.Button btRemoveHost;
        private System.Windows.Forms.Button btTestHost;
    }
}