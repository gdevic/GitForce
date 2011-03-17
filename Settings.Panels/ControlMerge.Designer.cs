namespace GitForce.Settings.Panels
{
    partial class ControlMerge
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxPath = new System.Windows.Forms.ComboBox();
            this.btBrowse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textArgs = new System.Windows.Forms.TextBox();
            this.openApp = new System.Windows.Forms.OpenFileDialog();
            this.textDesc = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chose a Merge utility to use with GitForce, or select one from those detected on y" +
                "our system:";
            // 
            // comboBoxPath
            // 
            this.comboBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPath.FormattingEnabled = true;
            this.comboBoxPath.Location = new System.Drawing.Point(0, 40);
            this.comboBoxPath.Name = "comboBoxPath";
            this.comboBoxPath.Size = new System.Drawing.Size(219, 21);
            this.comboBoxPath.TabIndex = 5;
            this.comboBoxPath.SelectionChangeCommitted += new System.EventHandler(this.ComboBoxPathSelectionChangeCommitted);
            // 
            // btBrowse
            // 
            this.btBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btBrowse.Location = new System.Drawing.Point(225, 40);
            this.btBrowse.Name = "btBrowse";
            this.btBrowse.Size = new System.Drawing.Size(75, 23);
            this.btBrowse.TabIndex = 6;
            this.btBrowse.Text = "Browse...";
            this.btBrowse.UseVisualStyleBackColor = true;
            this.btBrowse.Click += new System.EventHandler(this.BtBrowseClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Arguments:";
            // 
            // textArgs
            // 
            this.textArgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textArgs.Location = new System.Drawing.Point(0, 80);
            this.textArgs.Name = "textArgs";
            this.textArgs.Size = new System.Drawing.Size(300, 20);
            this.textArgs.TabIndex = 8;
            // 
            // openApp
            // 
            this.openApp.Title = "Select Merge utility";
            // 
            // textDesc
            // 
            this.textDesc.AutoSize = true;
            this.textDesc.Location = new System.Drawing.Point(3, 114);
            this.textDesc.Name = "textDesc";
            this.textDesc.Size = new System.Drawing.Size(58, 13);
            this.textDesc.TabIndex = 9;
            this.textDesc.Text = "description";
            // 
            // ControlMerge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textDesc);
            this.Controls.Add(this.textArgs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btBrowse);
            this.Controls.Add(this.comboBoxPath);
            this.Controls.Add(this.label1);
            this.Name = "ControlMerge";
            this.Size = new System.Drawing.Size(300, 300);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxPath;
        private System.Windows.Forms.Button btBrowse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textArgs;
        private System.Windows.Forms.OpenFileDialog openApp;
        private System.Windows.Forms.Label textDesc;
    }
}
