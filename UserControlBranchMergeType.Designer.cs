namespace git4win
{
    partial class UserControlBranchMergeStyle
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
            this.rb0 = new System.Windows.Forms.RadioButton();
            this.rb1 = new System.Windows.Forms.RadioButton();
            this.rb2 = new System.Windows.Forms.RadioButton();
            this.comboStrategy = new System.Windows.Forms.ComboBox();
            this.labelHelp = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rb0
            // 
            this.rb0.AutoSize = true;
            this.rb0.Checked = true;
            this.rb0.Location = new System.Drawing.Point(3, 3);
            this.rb0.Name = "rb0";
            this.rb0.Size = new System.Drawing.Size(257, 17);
            this.rb0.TabIndex = 0;
            this.rb0.TabStop = true;
            this.rb0.Tag = "fastforward";
            this.rb0.Text = "Keep a single branch line if possible (fast forward)";
            this.rb0.UseVisualStyleBackColor = true;
            this.rb0.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // rb1
            // 
            this.rb1.AutoSize = true;
            this.rb1.Location = new System.Drawing.Point(3, 26);
            this.rb1.Name = "rb1";
            this.rb1.Size = new System.Drawing.Size(191, 17);
            this.rb1.TabIndex = 1;
            this.rb1.Tag = "commit";
            this.rb1.Text = "Always create a new merge commit";
            this.rb1.UseVisualStyleBackColor = true;
            this.rb1.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // rb2
            // 
            this.rb2.AutoSize = true;
            this.rb2.Location = new System.Drawing.Point(3, 49);
            this.rb2.Name = "rb2";
            this.rb2.Size = new System.Drawing.Size(160, 17);
            this.rb2.TabIndex = 2;
            this.rb2.Tag = "strategy";
            this.rb2.Text = "Use different merge strategy:";
            this.rb2.UseVisualStyleBackColor = true;
            this.rb2.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // comboStrategy
            // 
            this.comboStrategy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboStrategy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStrategy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboStrategy.FormattingEnabled = true;
            this.comboStrategy.Items.AddRange(new object[] {
            "resolve",
            "recursive",
            "octopus",
            "ours",
            "subtree"});
            this.comboStrategy.Location = new System.Drawing.Point(169, 49);
            this.comboStrategy.Name = "comboStrategy";
            this.comboStrategy.Size = new System.Drawing.Size(190, 21);
            this.comboStrategy.TabIndex = 3;
            this.comboStrategy.SelectedIndexChanged += new System.EventHandler(this.comboStrategy_SelectedIndexChanged);
            // 
            // labelHelp
            // 
            this.labelHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHelp.BackColor = System.Drawing.SystemColors.Info;
            this.labelHelp.Location = new System.Drawing.Point(3, 81);
            this.labelHelp.Name = "labelHelp";
            this.labelHelp.Size = new System.Drawing.Size(356, 80);
            this.labelHelp.TabIndex = 4;
            this.labelHelp.Text = "Help";
            // 
            // UserControlBranchMergeType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelHelp);
            this.Controls.Add(this.comboStrategy);
            this.Controls.Add(this.rb2);
            this.Controls.Add(this.rb1);
            this.Controls.Add(this.rb0);
            this.Name = "UserControlBranchMergeType";
            this.Size = new System.Drawing.Size(362, 161);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rb0;
        private System.Windows.Forms.RadioButton rb1;
        private System.Windows.Forms.RadioButton rb2;
        private System.Windows.Forms.ComboBox comboStrategy;
        private System.Windows.Forms.Label labelHelp;
    }
}
