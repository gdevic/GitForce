namespace git4win
{
    partial class FormExecute
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExecute));
            this.textBox = new System.Windows.Forms.RichTextBox();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuExecCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExecSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExecClear = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.ContextMenuStrip = this.contextMenu;
            this.textBox.DetectUrls = false;
            this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox.Location = new System.Drawing.Point(0, 0);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(562, 355);
            this.textBox.TabIndex = 0;
            this.textBox.Text = "";
            this.textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(TextBoxKeyPress);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuExecCopy,
            this.menuExecSelectAll,
            this.toolStripSeparator1,
            this.menuExecClear});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(123, 76);
            // 
            // menuExecCopy
            // 
            this.menuExecCopy.Name = "menuExecCopy";
            this.menuExecCopy.Size = new System.Drawing.Size(122, 22);
            this.menuExecCopy.Text = "Copy";
            this.menuExecCopy.Click += new System.EventHandler(this.MenuExecCopyClick);
            // 
            // menuExecSelectAll
            // 
            this.menuExecSelectAll.Name = "menuExecSelectAll";
            this.menuExecSelectAll.Size = new System.Drawing.Size(122, 22);
            this.menuExecSelectAll.Text = "Select All";
            this.menuExecSelectAll.Click += new System.EventHandler(this.MenuExecSelectAllClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
            // 
            // menuExecClear
            // 
            this.menuExecClear.Name = "menuExecClear";
            this.menuExecClear.Size = new System.Drawing.Size(122, 22);
            this.menuExecClear.Text = "Clear";
            this.menuExecClear.Click += new System.EventHandler(this.MenuExecClearClick);
            // 
            // FormExecute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 355);
            this.Controls.Add(this.textBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormExecute";
            this.Text = "Command";
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox textBox;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuExecCopy;
        private System.Windows.Forms.ToolStripMenuItem menuExecSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuExecClear;
    }
}