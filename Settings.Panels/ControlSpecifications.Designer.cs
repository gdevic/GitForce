namespace Git4Win.Settings.Panels
{
    partial class ControlSpecifications
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
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.listFonts = new System.Windows.Forms.ListBox();
            this.listSizes = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelSample = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numWrap1 = new System.Windows.Forms.NumericUpDown();
            this.numWrap2 = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWrap1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWrap2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(-3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(303, 46);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select fixed width font for commit specificaitons. Also chose wrap around columns" +
                ".";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-3, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Font:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(241, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Size:";
            // 
            // listFonts
            // 
            this.listFonts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listFonts.FormattingEnabled = true;
            this.listFonts.IntegralHeight = false;
            this.listFonts.Location = new System.Drawing.Point(0, 72);
            this.listFonts.Name = "listFonts";
            this.listFonts.Size = new System.Drawing.Size(238, 96);
            this.listFonts.TabIndex = 4;
            this.listFonts.SelectedIndexChanged += new System.EventHandler(this.ListFontsSelectedIndexChanged);
            // 
            // listSizes
            // 
            this.listSizes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listSizes.FormattingEnabled = true;
            this.listSizes.IntegralHeight = false;
            this.listSizes.Location = new System.Drawing.Point(244, 72);
            this.listSizes.Name = "listSizes";
            this.listSizes.Size = new System.Drawing.Size(56, 96);
            this.listSizes.TabIndex = 6;
            this.listSizes.SelectedIndexChanged += new System.EventHandler(this.ListFontsSelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.labelSample);
            this.groupBox1.Location = new System.Drawing.Point(0, 173);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 52);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sample";
            // 
            // labelSample
            // 
            this.labelSample.AutoEllipsis = true;
            this.labelSample.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSample.Location = new System.Drawing.Point(3, 16);
            this.labelSample.Name = "labelSample";
            this.labelSample.Size = new System.Drawing.Size(294, 33);
            this.labelSample.TabIndex = 0;
            this.labelSample.Text = "git: This file caching manager sucks";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(-3, 235);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(177, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Wrap around column of the first line:";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(-3, 261);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(220, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Wrap around column of the commit message:";
            // 
            // numWrap1
            // 
            this.numWrap1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numWrap1.Location = new System.Drawing.Point(250, 228);
            this.numWrap1.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numWrap1.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numWrap1.Name = "numWrap1";
            this.numWrap1.Size = new System.Drawing.Size(50, 20);
            this.numWrap1.TabIndex = 10;
            this.numWrap1.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // numWrap2
            // 
            this.numWrap2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numWrap2.Location = new System.Drawing.Point(250, 254);
            this.numWrap2.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numWrap2.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numWrap2.Name = "numWrap2";
            this.numWrap2.Size = new System.Drawing.Size(50, 20);
            this.numWrap2.TabIndex = 11;
            this.numWrap2.Value = new decimal(new int[] {
            70,
            0,
            0,
            0});
            // 
            // ControlSpecifications
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numWrap2);
            this.Controls.Add(this.numWrap1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listSizes);
            this.Controls.Add(this.listFonts);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ControlSpecifications";
            this.Size = new System.Drawing.Size(300, 300);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numWrap1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWrap2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listFonts;
        private System.Windows.Forms.ListBox listSizes;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelSample;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numWrap1;
        private System.Windows.Forms.NumericUpDown numWrap2;
    }
}
