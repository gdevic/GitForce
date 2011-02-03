namespace git4win
{
    partial class FormPuTTY
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPuTTY));
            this.btOK = new System.Windows.Forms.Button();
            this.listBoxKeys = new System.Windows.Forms.ListBox();
            this.byAdd = new System.Windows.Forms.Button();
            this.btRemove = new System.Windows.Forms.Button();
            this.listBoxPf = new System.Windows.Forms.ListBox();
            this.btAddPf = new System.Windows.Forms.Button();
            this.btRemovePf = new System.Windows.Forms.Button();
            this.btShowPf = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btImport = new System.Windows.Forms.Button();
            this.btPuttygen = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxInputPf = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(371, 338);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 2;
            this.btOK.Text = "Done";
            this.btOK.UseVisualStyleBackColor = true;
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
            this.listBoxKeys.Size = new System.Drawing.Size(341, 108);
            this.listBoxKeys.TabIndex = 2;
            this.listBoxKeys.SelectedIndexChanged += new System.EventHandler(this.listBoxKeys_SelectedIndexChanged);
            // 
            // byAdd
            // 
            this.byAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.byAdd.Location = new System.Drawing.Point(353, 19);
            this.byAdd.Name = "byAdd";
            this.byAdd.Size = new System.Drawing.Size(75, 23);
            this.byAdd.TabIndex = 0;
            this.byAdd.Text = "Add...";
            this.byAdd.UseVisualStyleBackColor = true;
            this.byAdd.Click += new System.EventHandler(this.byAdd_Click);
            // 
            // btRemove
            // 
            this.btRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRemove.Enabled = false;
            this.btRemove.Location = new System.Drawing.Point(353, 48);
            this.btRemove.Name = "btRemove";
            this.btRemove.Size = new System.Drawing.Size(75, 23);
            this.btRemove.TabIndex = 1;
            this.btRemove.Text = "Remove";
            this.btRemove.UseVisualStyleBackColor = true;
            this.btRemove.Click += new System.EventHandler(this.btRemove_Click);
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
            this.listBoxPf.Size = new System.Drawing.Size(341, 79);
            this.listBoxPf.TabIndex = 2;
            this.listBoxPf.SelectedIndexChanged += new System.EventHandler(this.listBoxPf_SelectedIndexChanged);
            // 
            // btAddPf
            // 
            this.btAddPf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAddPf.Enabled = false;
            this.btAddPf.Location = new System.Drawing.Point(353, 17);
            this.btAddPf.Name = "btAddPf";
            this.btAddPf.Size = new System.Drawing.Size(75, 23);
            this.btAddPf.TabIndex = 1;
            this.btAddPf.Text = "Add";
            this.btAddPf.UseVisualStyleBackColor = true;
            this.btAddPf.Click += new System.EventHandler(this.btAddP_Click);
            // 
            // btRemovePf
            // 
            this.btRemovePf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRemovePf.Enabled = false;
            this.btRemovePf.Location = new System.Drawing.Point(353, 48);
            this.btRemovePf.Name = "btRemovePf";
            this.btRemovePf.Size = new System.Drawing.Size(75, 23);
            this.btRemovePf.TabIndex = 3;
            this.btRemovePf.Text = "Remove";
            this.btRemovePf.UseVisualStyleBackColor = true;
            this.btRemovePf.Click += new System.EventHandler(this.btRemoveP_Click);
            // 
            // btShowPf
            // 
            this.btShowPf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btShowPf.Location = new System.Drawing.Point(353, 77);
            this.btShowPf.Name = "btShowPf";
            this.btShowPf.Size = new System.Drawing.Size(75, 23);
            this.btShowPf.TabIndex = 4;
            this.btShowPf.Text = "Show";
            this.btShowPf.UseVisualStyleBackColor = true;
            this.btShowPf.Click += new System.EventHandler(this.btShow_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(434, 53);
            this.label1.TabIndex = 8;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "ppk";
            this.openFileDialog.Filter = "\"PuTTY Keys|*.ppk|All files|*.*\"";
            // 
            // btImport
            // 
            this.btImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btImport.Location = new System.Drawing.Point(12, 338);
            this.btImport.Name = "btImport";
            this.btImport.Size = new System.Drawing.Size(75, 23);
            this.btImport.TabIndex = 0;
            this.btImport.Text = "Import Now";
            this.btImport.UseVisualStyleBackColor = true;
            this.btImport.Click += new System.EventHandler(this.btImport_Click);
            // 
            // btPuttygen
            // 
            this.btPuttygen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btPuttygen.Location = new System.Drawing.Point(93, 338);
            this.btPuttygen.Name = "btPuttygen";
            this.btPuttygen.Size = new System.Drawing.Size(75, 23);
            this.btPuttygen.TabIndex = 1;
            this.btPuttygen.Text = "PuTTYgen";
            this.btPuttygen.UseVisualStyleBackColor = true;
            this.btPuttygen.Click += new System.EventHandler(this.btPuttygen_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxInputPf);
            this.groupBox1.Controls.Add(this.listBoxPf);
            this.groupBox1.Controls.Add(this.btAddPf);
            this.groupBox1.Controls.Add(this.btRemovePf);
            this.groupBox1.Controls.Add(this.btShowPf);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(434, 130);
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
            this.textBoxInputPf.Size = new System.Drawing.Size(341, 20);
            this.textBoxInputPf.TabIndex = 0;
            this.textBoxInputPf.UseSystemPasswordChar = true;
            this.textBoxInputPf.TextChanged += new System.EventHandler(this.textBoxInputPf_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBoxKeys);
            this.groupBox2.Controls.Add(this.byAdd);
            this.groupBox2.Controls.Add(this.btRemove);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(434, 133);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select keys to import";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 65);
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
            this.splitContainer1.Size = new System.Drawing.Size(434, 267);
            this.splitContainer1.SplitterDistance = 133;
            this.splitContainer1.TabIndex = 15;
            // 
            // FormPuTTY
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 373);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btPuttygen);
            this.Controls.Add(this.btImport);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(280, 362);
            this.Name = "FormPuTTY";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage PuTTY Keys";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.ListBox listBoxKeys;
        private System.Windows.Forms.Button byAdd;
        private System.Windows.Forms.Button btRemove;
        private System.Windows.Forms.ListBox listBoxPf;
        private System.Windows.Forms.Button btAddPf;
        private System.Windows.Forms.Button btRemovePf;
        private System.Windows.Forms.Button btShowPf;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btImport;
        private System.Windows.Forms.Button btPuttygen;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxInputPf;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}