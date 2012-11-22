namespace GitForce.Repo.Edit.Panels
{
    partial class ControlLocal
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
            this.radio3 = new System.Windows.Forms.RadioButton();
            this.radio2 = new System.Windows.Forms.RadioButton();
            this.radio1 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.radio4 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // radio3
            // 
            this.radio3.AutoSize = true;
            this.radio3.Location = new System.Drawing.Point(3, 96);
            this.radio3.Name = "radio3";
            this.radio3.Size = new System.Drawing.Size(158, 17);
            this.radio3.TabIndex = 7;
            this.radio3.TabStop = true;
            this.radio3.Tag = "false";
            this.radio3.Text = "Checkout as-is, commit as-is";
            this.radio3.UseVisualStyleBackColor = true;
            // 
            // radio2
            // 
            this.radio2.AutoSize = true;
            this.radio2.Location = new System.Drawing.Point(3, 73);
            this.radio2.Name = "radio2";
            this.radio2.Size = new System.Drawing.Size(182, 17);
            this.radio2.TabIndex = 6;
            this.radio2.TabStop = true;
            this.radio2.Tag = "input";
            this.radio2.Text = "Checkout as-is, commit Unix-style";
            this.radio2.UseVisualStyleBackColor = true;
            // 
            // radio1
            // 
            this.radio1.AutoSize = true;
            this.radio1.Location = new System.Drawing.Point(3, 50);
            this.radio1.Name = "radio1";
            this.radio1.Size = new System.Drawing.Size(229, 17);
            this.radio1.TabIndex = 5;
            this.radio1.TabStop = true;
            this.radio1.Tag = "true";
            this.radio1.Text = "Checkout Windows-style, commit Unix-style";
            this.radio1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Line endings:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 30);
            this.label1.TabIndex = 8;
            this.label1.Text = "Set local git options for this repo";
            // 
            // radio4
            // 
            this.radio4.AutoSize = true;
            this.radio4.Checked = true;
            this.radio4.Location = new System.Drawing.Point(3, 119);
            this.radio4.Name = "radio4";
            this.radio4.Size = new System.Drawing.Size(204, 17);
            this.radio4.TabIndex = 10;
            this.radio4.TabStop = true;
            this.radio4.Text = "Remove this setting (use global value)";
            this.radio4.UseVisualStyleBackColor = true;
            // 
            // ControlLocal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radio4);
            this.Controls.Add(this.radio3);
            this.Controls.Add(this.radio2);
            this.Controls.Add(this.radio1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ControlLocal";
            this.Size = new System.Drawing.Size(300, 300);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radio3;
        private System.Windows.Forms.RadioButton radio2;
        private System.Windows.Forms.RadioButton radio1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radio4;

    }
}
