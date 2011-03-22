namespace GitForce
{
    partial class FormChangelistFilter
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
            this.btOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dateTimeAfter = new System.Windows.Forms.DateTimePicker();
            this.dateTimeBefore = new System.Windows.Forms.DateTimePicker();
            this.checkBoxAfter = new System.Windows.Forms.CheckBox();
            this.checkBoxBefore = new System.Windows.Forms.CheckBox();
            this.textBoxAuthor = new System.Windows.Forms.TextBox();
            this.checkBoxAuthor = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(275, 116);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(194, 116);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.BtOkClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dateTimeAfter);
            this.groupBox1.Controls.Add(this.dateTimeBefore);
            this.groupBox1.Controls.Add(this.checkBoxAfter);
            this.groupBox1.Controls.Add(this.checkBoxBefore);
            this.groupBox1.Controls.Add(this.textBoxAuthor);
            this.groupBox1.Controls.Add(this.checkBoxAuthor);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(338, 98);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Changelist criteria";
            // 
            // dateTimeAfter
            // 
            this.dateTimeAfter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimeAfter.CustomFormat = "";
            this.dateTimeAfter.Location = new System.Drawing.Point(125, 72);
            this.dateTimeAfter.Name = "dateTimeAfter";
            this.dateTimeAfter.Size = new System.Drawing.Size(207, 20);
            this.dateTimeAfter.TabIndex = 5;
            // 
            // dateTimeBefore
            // 
            this.dateTimeBefore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimeBefore.CustomFormat = "";
            this.dateTimeBefore.Location = new System.Drawing.Point(125, 46);
            this.dateTimeBefore.Name = "dateTimeBefore";
            this.dateTimeBefore.Size = new System.Drawing.Size(207, 20);
            this.dateTimeBefore.TabIndex = 3;
            // 
            // checkBoxAfter
            // 
            this.checkBoxAfter.AutoSize = true;
            this.checkBoxAfter.Location = new System.Drawing.Point(20, 75);
            this.checkBoxAfter.Name = "checkBoxAfter";
            this.checkBoxAfter.Size = new System.Drawing.Size(81, 17);
            this.checkBoxAfter.TabIndex = 4;
            this.checkBoxAfter.Text = "After (date):";
            this.checkBoxAfter.UseVisualStyleBackColor = true;
            // 
            // checkBoxBefore
            // 
            this.checkBoxBefore.AutoSize = true;
            this.checkBoxBefore.Location = new System.Drawing.Point(20, 49);
            this.checkBoxBefore.Name = "checkBoxBefore";
            this.checkBoxBefore.Size = new System.Drawing.Size(90, 17);
            this.checkBoxBefore.TabIndex = 2;
            this.checkBoxBefore.Text = "Before (date):";
            this.checkBoxBefore.UseVisualStyleBackColor = true;
            // 
            // textBoxAuthor
            // 
            this.textBoxAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAuthor.Location = new System.Drawing.Point(125, 20);
            this.textBoxAuthor.Name = "textBoxAuthor";
            this.textBoxAuthor.Size = new System.Drawing.Size(207, 20);
            this.textBoxAuthor.TabIndex = 1;
            // 
            // checkBoxAuthor
            // 
            this.checkBoxAuthor.AutoSize = true;
            this.checkBoxAuthor.Location = new System.Drawing.Point(20, 22);
            this.checkBoxAuthor.Name = "checkBoxAuthor";
            this.checkBoxAuthor.Size = new System.Drawing.Size(60, 17);
            this.checkBoxAuthor.TabIndex = 0;
            this.checkBoxAuthor.Text = "Author:";
            this.checkBoxAuthor.UseVisualStyleBackColor = true;
            // 
            // FormChangelistFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 151);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(256, 189);
            this.Name = "FormChangelistFilter";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Filter Submitted Changelists";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormChangelistFilterFormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxAfter;
        private System.Windows.Forms.CheckBox checkBoxBefore;
        private System.Windows.Forms.TextBox textBoxAuthor;
        private System.Windows.Forms.CheckBox checkBoxAuthor;
        private System.Windows.Forms.DateTimePicker dateTimeAfter;
        private System.Windows.Forms.DateTimePicker dateTimeBefore;
    }
}