namespace GitForce.Settings.Panels
{
    partial class ControlOptions
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
            this.checkBoxTabs = new System.Windows.Forms.CheckBox();
            this.checkBoxWarnMultipleInstances = new System.Windows.Forms.CheckBox(); 
            this.SuspendLayout();
            // 
            // checkBoxTabs
            // 
            this.checkBoxTabs.AutoSize = true;
            this.checkBoxTabs.Location = new System.Drawing.Point(3, 3);
            this.checkBoxTabs.Name = "checkBoxTabs";
            this.checkBoxTabs.Size = new System.Drawing.Size(159, 17);
            this.checkBoxTabs.TabIndex = 0;
            this.checkBoxTabs.Text = "Show tabs on the right pane";
            this.checkBoxTabs.UseVisualStyleBackColor = true;
            // 
            // checkBoxWarnMultipleInstances
            // 
            this.checkBoxWarnMultipleInstances.AutoSize = true;
            this.checkBoxWarnMultipleInstances.Location = new System.Drawing.Point(3, 26);
            this.checkBoxWarnMultipleInstances.Name = "checkBoxWarnMultipleInstances";
            this.checkBoxWarnMultipleInstances.Size = new System.Drawing.Size(259, 17);
            this.checkBoxWarnMultipleInstances.TabIndex = 1;
            this.checkBoxWarnMultipleInstances.Text = "&Warn before starting additional instance of GitForce";
            this.checkBoxWarnMultipleInstances.UseVisualStyleBackColor = true;
            // 
            // ControlOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxTabs);
            this.Controls.Add(this.checkBoxWarnMultipleInstances);
            this.Name = "ControlOptions";
            this.Size = new System.Drawing.Size(300, 300);
            this.Tag = "Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxTabs;
        private System.Windows.Forms.CheckBox checkBoxWarnMultipleInstances;

    }
}
