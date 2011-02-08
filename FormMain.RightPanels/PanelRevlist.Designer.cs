namespace git4win.FormMain_RightPanels
{
    partial class PanelRevlist
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.listRev = new System.Windows.Forms.ListView();
            this.colHash = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSubmitter = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSubject = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(400, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(126, 22);
            this.toolStripLabel1.Text = "Submitted Changelists";
            // 
            // listRev
            // 
            this.listRev.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHash,
            this.colDate,
            this.colSubmitter,
            this.colSubject});
            this.listRev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listRev.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listRev.FullRowSelect = true;
            this.listRev.Location = new System.Drawing.Point(0, 25);
            this.listRev.Name = "listRev";
            this.listRev.Size = new System.Drawing.Size(400, 375);
            this.listRev.TabIndex = 1;
            this.listRev.UseCompatibleStateImageBehavior = false;
            this.listRev.View = System.Windows.Forms.View.Details;
            // 
            // colHash
            // 
            this.colHash.Text = "SHA1";
            this.colHash.Width = 40;
            // 
            // colDate
            // 
            this.colDate.Text = "Date";
            this.colDate.Width = 35;
            // 
            // colSubmitter
            // 
            this.colSubmitter.Text = "Submitter";
            this.colSubmitter.Width = 56;
            // 
            // colSubject
            // 
            this.colSubject.Text = "Subject";
            this.colSubject.Width = 265;
            // 
            // PanelRevlist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listRev);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PanelRevlist";
            this.Size = new System.Drawing.Size(400, 400);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ListView listRev;
        private System.Windows.Forms.ColumnHeader colHash;
        private System.Windows.Forms.ColumnHeader colDate;
        private System.Windows.Forms.ColumnHeader colSubmitter;
        private System.Windows.Forms.ColumnHeader colSubject;
    }
}
