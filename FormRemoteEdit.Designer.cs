namespace GitForce
{
    partial class FormRemoteEdit
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
            this.userControlRemotesEdit = new GitForce.RemotesEdit();
            this.btDone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _userControlRemotesEdit
            // 
            this.userControlRemotesEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.userControlRemotesEdit.Location = new System.Drawing.Point(12, 12);
            this.userControlRemotesEdit.MinimumSize = new System.Drawing.Size(320, 165);
            this.userControlRemotesEdit.Name = "userControlRemotesEdit";
            this.userControlRemotesEdit.Size = new System.Drawing.Size(542, 197);
            this.userControlRemotesEdit.TabIndex = 0;
            // 
            // btDone
            // 
            this.btDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btDone.Location = new System.Drawing.Point(479, 215);
            this.btDone.Name = "btDone";
            this.btDone.Size = new System.Drawing.Size(75, 23);
            this.btDone.TabIndex = 2;
            this.btDone.Text = "Done";
            this.btDone.UseVisualStyleBackColor = true;
            // 
            // FormRemoteEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btDone;
            this.ClientSize = new System.Drawing.Size(566, 247);
            this.Controls.Add(this.btDone);
            this.Controls.Add(this.userControlRemotesEdit);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(533, 285);
            this.Name = "FormRemoteEdit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Edit Remote Repositories";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRemoteEditFormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private RemotesEdit userControlRemotesEdit;
        private System.Windows.Forms.Button btDone;

    }
}