namespace GitForce
{
    partial class FormNewBranch
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
            this.listBranches = new System.Windows.Forms.ListBox();
            this.textSHA1 = new System.Windows.Forms.TextBox();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.btCancel = new System.Windows.Forms.Button();
            this.checkCheckOut = new System.Windows.Forms.CheckBox();
            this.textBranchName = new System.Windows.Forms.TextBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btCreate = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBranches
            // 
            this.listBranches.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBranches.BackColor = System.Drawing.SystemColors.Control;
            this.listBranches.FormattingEnabled = true;
            this.listBranches.IntegralHeight = false;
            this.listBranches.Location = new System.Drawing.Point(196, 68);
            this.listBranches.Name = "listBranches";
            this.listBranches.Size = new System.Drawing.Size(223, 88);
            this.listBranches.TabIndex = 6;
            this.listBranches.SelectedIndexChanged += new System.EventHandler(this.ListBoxSelectedIndexChanged);
            // 
            // textSHA1
            // 
            this.textSHA1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textSHA1.Enabled = false;
            this.textSHA1.Location = new System.Drawing.Point(196, 41);
            this.textSHA1.MaxLength = 40;
            this.textSHA1.Name = "textSHA1";
            this.textSHA1.Size = new System.Drawing.Size(223, 20);
            this.textSHA1.TabIndex = 5;
            this.textSHA1.TextChanged += new System.EventHandler(this.TextSha1TextChanged);
            this.textSHA1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBranchNameKeyPress);
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(41, 122);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(47, 17);
            this.radioButton5.TabIndex = 4;
            this.radioButton5.Tag = "Tag";
            this.radioButton5.Text = "Tag:";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.RadioBranchSourceCheckedChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(41, 95);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(102, 17);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.Tag = "Remote";
            this.radioButton4.Text = "Remote Branch:";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.RadioBranchSourceCheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(41, 68);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(91, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Tag = "Local";
            this.radioButton3.Text = "Local Branch:";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.RadioBranchSourceCheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(41, 42);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(131, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Tag = "SHA1";
            this.radioButton2.Text = "Specify Commit SHA1:";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.RadioBranchSourceCheckedChanged);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(365, 229);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 6;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // checkCheckOut
            // 
            this.checkCheckOut.AutoSize = true;
            this.checkCheckOut.Checked = true;
            this.checkCheckOut.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkCheckOut.Location = new System.Drawing.Point(56, 38);
            this.checkCheckOut.Name = "checkCheckOut";
            this.checkCheckOut.Size = new System.Drawing.Size(181, 17);
            this.checkCheckOut.TabIndex = 10;
            this.checkCheckOut.Text = "Checkout branch after creating it";
            this.checkCheckOut.UseVisualStyleBackColor = true;
            // 
            // textBranchName
            // 
            this.textBranchName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBranchName.Location = new System.Drawing.Point(56, 12);
            this.textBranchName.Name = "textBranchName";
            this.textBranchName.Size = new System.Drawing.Size(378, 20);
            this.textBranchName.TabIndex = 9;
            this.textBranchName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBranchNameKeyUp);
            this.textBranchName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBranchNameKeyPress);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(41, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(92, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Tag = "Head";
            this.radioButton1.Text = "Current HEAD";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.RadioBranchSourceCheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.listBranches);
            this.groupBox1.Controls.Add(this.textSHA1);
            this.groupBox1.Controls.Add(this.radioButton5);
            this.groupBox1.Controls.Add(this.radioButton4);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(15, 61);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(425, 162);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Starting Revision";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Name:";
            // 
            // btCreate
            // 
            this.btCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btCreate.Enabled = false;
            this.btCreate.Location = new System.Drawing.Point(284, 229);
            this.btCreate.Name = "btCreate";
            this.btCreate.Size = new System.Drawing.Size(75, 23);
            this.btCreate.TabIndex = 7;
            this.btCreate.Text = "Create";
            this.btCreate.UseVisualStyleBackColor = true;
            this.btCreate.Click += new System.EventHandler(this.BtCreateClick);
            // 
            // FormNewBranch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(452, 264);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.checkCheckOut);
            this.Controls.Add(this.textBranchName);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btCreate);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(347, 288);
            this.Name = "FormNewBranch";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Create New Branch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormNewBranchFormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBranches;
        private System.Windows.Forms.TextBox textSHA1;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.CheckBox checkCheckOut;
        private System.Windows.Forms.TextBox textBranchName;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btCreate;
    }
}