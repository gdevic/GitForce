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
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.checkBoxMessage = new System.Windows.Forms.CheckBox();
            this.dateTimeAfter = new System.Windows.Forms.DateTimePicker();
            this.dateTimeBefore = new System.Windows.Forms.DateTimePicker();
            this.checkBoxAfter = new System.Windows.Forms.CheckBox();
            this.checkBoxBefore = new System.Windows.Forms.CheckBox();
            this.textBoxAuthor = new System.Windows.Forms.TextBox();
            this.checkBoxAuthor = new System.Windows.Forms.CheckBox();
            this.checkBoxSha = new System.Windows.Forms.CheckBox();
            this.textBoxSha = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(275, 169);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(194, 169);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 1;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.BtOkClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBoxSha);
            this.groupBox1.Controls.Add(this.checkBoxSha);
            this.groupBox1.Controls.Add(this.textBoxMessage);
            this.groupBox1.Controls.Add(this.checkBoxMessage);
            this.groupBox1.Controls.Add(this.dateTimeAfter);
            this.groupBox1.Controls.Add(this.dateTimeBefore);
            this.groupBox1.Controls.Add(this.checkBoxAfter);
            this.groupBox1.Controls.Add(this.checkBoxBefore);
            this.groupBox1.Controls.Add(this.textBoxAuthor);
            this.groupBox1.Controls.Add(this.checkBoxAuthor);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(338, 151);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Changelist criteria";
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMessage.Location = new System.Drawing.Point(125, 45);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(207, 20);
            this.textBoxMessage.TabIndex = 3;
            // 
            // checkBoxMessage
            // 
            this.checkBoxMessage.AutoSize = true;
            this.checkBoxMessage.Location = new System.Drawing.Point(6, 47);
            this.checkBoxMessage.Name = "checkBoxMessage";
            this.checkBoxMessage.Size = new System.Drawing.Size(72, 17);
            this.checkBoxMessage.TabIndex = 2;
            this.checkBoxMessage.Text = "Message:";
            this.checkBoxMessage.UseVisualStyleBackColor = true;
            // 
            // dateTimeAfter
            // 
            this.dateTimeAfter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimeAfter.CustomFormat = "";
            this.dateTimeAfter.Location = new System.Drawing.Point(125, 123);
            this.dateTimeAfter.Name = "dateTimeAfter";
            this.dateTimeAfter.Size = new System.Drawing.Size(207, 20);
            this.dateTimeAfter.TabIndex = 9;
            // 
            // dateTimeBefore
            // 
            this.dateTimeBefore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimeBefore.CustomFormat = "";
            this.dateTimeBefore.Location = new System.Drawing.Point(125, 97);
            this.dateTimeBefore.Name = "dateTimeBefore";
            this.dateTimeBefore.Size = new System.Drawing.Size(207, 20);
            this.dateTimeBefore.TabIndex = 7;
            // 
            // checkBoxAfter
            // 
            this.checkBoxAfter.AutoSize = true;
            this.checkBoxAfter.Location = new System.Drawing.Point(6, 128);
            this.checkBoxAfter.Name = "checkBoxAfter";
            this.checkBoxAfter.Size = new System.Drawing.Size(81, 17);
            this.checkBoxAfter.TabIndex = 8;
            this.checkBoxAfter.Text = "After (date):";
            this.checkBoxAfter.UseVisualStyleBackColor = true;
            // 
            // checkBoxBefore
            // 
            this.checkBoxBefore.AutoSize = true;
            this.checkBoxBefore.Location = new System.Drawing.Point(6, 102);
            this.checkBoxBefore.Name = "checkBoxBefore";
            this.checkBoxBefore.Size = new System.Drawing.Size(90, 17);
            this.checkBoxBefore.TabIndex = 6;
            this.checkBoxBefore.Text = "Before (date):";
            this.checkBoxBefore.UseVisualStyleBackColor = true;
            // 
            // textBoxAuthor
            // 
            this.textBoxAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAuthor.Location = new System.Drawing.Point(125, 71);
            this.textBoxAuthor.Name = "textBoxAuthor";
            this.textBoxAuthor.Size = new System.Drawing.Size(207, 20);
            this.textBoxAuthor.TabIndex = 5;
            // 
            // checkBoxAuthor
            // 
            this.checkBoxAuthor.AutoSize = true;
            this.checkBoxAuthor.Location = new System.Drawing.Point(6, 73);
            this.checkBoxAuthor.Name = "checkBoxAuthor";
            this.checkBoxAuthor.Size = new System.Drawing.Size(60, 17);
            this.checkBoxAuthor.TabIndex = 4;
            this.checkBoxAuthor.Text = "Author:";
            this.checkBoxAuthor.UseVisualStyleBackColor = true;
            // 
            // checkBoxSha
            // 
            this.checkBoxSha.AutoSize = true;
            this.checkBoxSha.Location = new System.Drawing.Point(6, 21);
            this.checkBoxSha.Name = "checkBoxSha";
            this.checkBoxSha.Size = new System.Drawing.Size(57, 17);
            this.checkBoxSha.TabIndex = 0;
            this.checkBoxSha.Text = "SHA1:";
            this.checkBoxSha.UseVisualStyleBackColor = true;
            // 
            // textBoxSha
            // 
            this.textBoxSha.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSha.Location = new System.Drawing.Point(125, 19);
            this.textBoxSha.Name = "textBoxSha";
            this.textBoxSha.Size = new System.Drawing.Size(207, 20);
            this.textBoxSha.TabIndex = 1;
            // 
            // FormChangelistFilter
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(362, 204);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(256, 242);
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
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.CheckBox checkBoxMessage;
        private System.Windows.Forms.TextBox textBoxSha;
        private System.Windows.Forms.CheckBox checkBoxSha;
    }
}