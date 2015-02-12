namespace GitForce
{
    partial class FormGitRun
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
            this.components = new System.ComponentModel.Container();
            this.btCancel = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelProgress = new System.Windows.Forms.Label();
            this.timerProgress = new System.Windows.Forms.Timer(this.components);
            this.checkAutoclose = new System.Windows.Forms.CheckBox();
            this.textStdout = new GitForce.RichTextBoxEx();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(497, 214);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 0;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.BtCancelClick);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 240);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(584, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatus
            // 
            this.toolStripStatus.Name = "toolStripStatus";
            this.toolStripStatus.Size = new System.Drawing.Size(150, 17);
            this.toolStripStatus.Text = "Git command in progress...";
            // 
            // labelProgress
            // 
            this.labelProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(483, 218);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(10, 13);
            this.labelProgress.TabIndex = 3;
            this.labelProgress.Tag = "";
            this.labelProgress.Text = ".";
            // 
            // timerProgress
            // 
            this.timerProgress.Enabled = true;
            this.timerProgress.Interval = 200;
            this.timerProgress.Tag = "";
            this.timerProgress.Tick += new System.EventHandler(this.TimerProgressTick);
            // 
            // checkAutoclose
            // 
            this.checkAutoclose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkAutoclose.AutoSize = true;
            this.checkAutoclose.Location = new System.Drawing.Point(12, 220);
            this.checkAutoclose.Name = "checkAutoclose";
            this.checkAutoclose.Size = new System.Drawing.Size(244, 17);
            this.checkAutoclose.TabIndex = 4;
            this.checkAutoclose.Text = "Auto-close this dialog if operation is successful";
            this.checkAutoclose.UseVisualStyleBackColor = true;
            this.checkAutoclose.CheckedChanged += new System.EventHandler(this.CheckAutocloseCheckedChanged);
            // 
            // textStdout
            // 
            this.textStdout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textStdout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textStdout.DetectUrls = false;
            this.textStdout.Location = new System.Drawing.Point(12, 12);
            this.textStdout.Name = "textStdout";
            this.textStdout.ReadOnly = true;
            this.textStdout.Size = new System.Drawing.Size(560, 196);
            this.textStdout.TabIndex = 1;
            this.textStdout.Text = "";
            this.textStdout.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.textLinkClicked);
            // 
            // FormGitRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 262);
            this.Controls.Add(this.checkAutoclose);
            this.Controls.Add(this.labelProgress);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.textStdout);
            this.Controls.Add(this.btCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(372, 155);
            this.Name = "FormGitRun";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Git";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGitRunFormClosing);
            this.Shown += new System.EventHandler(this.FormGitRunShown);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private RichTextBoxEx textStdout;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Timer timerProgress;
        private System.Windows.Forms.CheckBox checkAutoclose;
    }
}