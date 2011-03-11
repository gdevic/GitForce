namespace Git4Win.Settings.Panels
{
    partial class ControlStatus
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
            this.checkBoxShowGitCommands = new System.Windows.Forms.CheckBox();
            this.checkBoxShowTimestamp = new System.Windows.Forms.CheckBox();
            this.checkBoxUse24HourClock = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBoxShowGitCommands
            // 
            this.checkBoxShowGitCommands.AutoSize = true;
            this.checkBoxShowGitCommands.Location = new System.Drawing.Point(3, 3);
            this.checkBoxShowGitCommands.Name = "checkBoxShowGitCommands";
            this.checkBoxShowGitCommands.Size = new System.Drawing.Size(208, 17);
            this.checkBoxShowGitCommands.TabIndex = 0;
            this.checkBoxShowGitCommands.Text = "Show git commands in the status pane";
            this.checkBoxShowGitCommands.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowTimestamp
            // 
            this.checkBoxShowTimestamp.AutoSize = true;
            this.checkBoxShowTimestamp.Location = new System.Drawing.Point(3, 26);
            this.checkBoxShowTimestamp.Name = "checkBoxShowTimestamp";
            this.checkBoxShowTimestamp.Size = new System.Drawing.Size(199, 17);
            this.checkBoxShowTimestamp.TabIndex = 1;
            this.checkBoxShowTimestamp.Text = "Show timestamp for status messages";
            this.checkBoxShowTimestamp.UseVisualStyleBackColor = true;
            // 
            // checkBoxUse24HourClock
            // 
            this.checkBoxUse24HourClock.AutoSize = true;
            this.checkBoxUse24HourClock.Location = new System.Drawing.Point(30, 49);
            this.checkBoxUse24HourClock.Name = "checkBoxUse24HourClock";
            this.checkBoxUse24HourClock.Size = new System.Drawing.Size(113, 17);
            this.checkBoxUse24HourClock.TabIndex = 2;
            this.checkBoxUse24HourClock.Text = "Use 24 hour clock";
            this.checkBoxUse24HourClock.UseVisualStyleBackColor = true;
            // 
            // ControlStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxUse24HourClock);
            this.Controls.Add(this.checkBoxShowTimestamp);
            this.Controls.Add(this.checkBoxShowGitCommands);
            this.Name = "ControlStatus";
            this.Size = new System.Drawing.Size(300, 300);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxShowGitCommands;
        private System.Windows.Forms.CheckBox checkBoxShowTimestamp;
        private System.Windows.Forms.CheckBox checkBoxUse24HourClock;
    }
}
