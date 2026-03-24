namespace GitForce
{
    partial class FormCustomizeTools
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
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textDesc = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBrowse = new System.Windows.Forms.CheckBox();
            this.checkPrompt = new System.Windows.Forms.CheckBox();
            this.checkCloseUponExit = new System.Windows.Forms.CheckBox();
            this.checkRefresh = new System.Windows.Forms.CheckBox();
            this.checkWriteToStatus = new System.Windows.Forms.CheckBox();
            this.checkConsoleApp = new System.Windows.Forms.CheckBox();
            this.checkAddToContextMenu = new System.Windows.Forms.CheckBox();
            this.textDir = new System.Windows.Forms.TextBox();
            this.textArgs = new System.Windows.Forms.TextBox();
            this.textCmd = new System.Windows.Forms.TextBox();
            this.btDown = new System.Windows.Forms.Button();
            this.btUp = new System.Windows.Forms.Button();
            this.btRemove = new System.Windows.Forms.Button();
            this.btEdit = new System.Windows.Forms.Button();
            this.btAdd = new System.Windows.Forms.Button();
            this.listTools = new System.Windows.Forms.ListBox();
            this.btHelp = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(353, 390);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(272, 390);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textDesc);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.checkBrowse);
            this.groupBox1.Controls.Add(this.checkPrompt);
            this.groupBox1.Controls.Add(this.checkCloseUponExit);
            this.groupBox1.Controls.Add(this.checkRefresh);
            this.groupBox1.Controls.Add(this.checkWriteToStatus);
            this.groupBox1.Controls.Add(this.checkConsoleApp);
            this.groupBox1.Controls.Add(this.checkAddToContextMenu);
            this.groupBox1.Controls.Add(this.textDir);
            this.groupBox1.Controls.Add(this.textArgs);
            this.groupBox1.Controls.Add(this.textCmd);
            this.groupBox1.Controls.Add(this.btDown);
            this.groupBox1.Controls.Add(this.btUp);
            this.groupBox1.Controls.Add(this.btRemove);
            this.groupBox1.Controls.Add(this.btEdit);
            this.groupBox1.Controls.Add(this.btAdd);
            this.groupBox1.Controls.Add(this.listTools);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(416, 372);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tools menu";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 223);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Initial directory:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 197);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Arguments:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 171);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Command:";
            // 
            // textDesc
            // 
            this.textDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textDesc.Location = new System.Drawing.Point(199, 324);
            this.textDesc.Name = "textDesc";
            this.textDesc.ReadOnly = true;
            this.textDesc.Size = new System.Drawing.Size(211, 20);
            this.textDesc.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(196, 304);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Description:";
            // 
            // checkBrowse
            // 
            this.checkBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBrowse.AutoSize = true;
            this.checkBrowse.Enabled = false;
            this.checkBrowse.Location = new System.Drawing.Point(199, 280);
            this.checkBrowse.Name = "checkBrowse";
            this.checkBrowse.Size = new System.Drawing.Size(161, 17);
            this.checkBrowse.TabIndex = 18;
            this.checkBrowse.Text = "Add Browse to prompt dialog";
            this.checkBrowse.UseVisualStyleBackColor = true;
            // 
            // checkPrompt
            // 
            this.checkPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkPrompt.AutoSize = true;
            this.checkPrompt.Enabled = false;
            this.checkPrompt.Location = new System.Drawing.Point(199, 257);
            this.checkPrompt.Name = "checkPrompt";
            this.checkPrompt.Size = new System.Drawing.Size(126, 17);
            this.checkPrompt.TabIndex = 17;
            this.checkPrompt.Text = "Prompt for arguments";
            this.checkPrompt.UseVisualStyleBackColor = true;
            // 
            // checkCloseUponExit
            // 
            this.checkCloseUponExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkCloseUponExit.AutoSize = true;
            this.checkCloseUponExit.Enabled = false;
            this.checkCloseUponExit.Location = new System.Drawing.Point(6, 326);
            this.checkCloseUponExit.Name = "checkCloseUponExit";
            this.checkCloseUponExit.Size = new System.Drawing.Size(137, 17);
            this.checkCloseUponExit.TabIndex = 16;
            this.checkCloseUponExit.Text = "Close window upon exit";
            this.checkCloseUponExit.UseVisualStyleBackColor = true;
            // 
            // checkRefresh
            // 
            this.checkRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkRefresh.AutoSize = true;
            this.checkRefresh.Enabled = false;
            this.checkRefresh.Location = new System.Drawing.Point(6, 349);
            this.checkRefresh.Name = "checkRefresh";
            this.checkRefresh.Size = new System.Drawing.Size(152, 17);
            this.checkRefresh.TabIndex = 15;
            this.checkRefresh.Text = "Refresh GitForce upon exit";
            this.checkRefresh.UseVisualStyleBackColor = true;
            // 
            // checkWriteToStatus
            // 
            this.checkWriteToStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkWriteToStatus.AutoSize = true;
            this.checkWriteToStatus.Enabled = false;
            this.checkWriteToStatus.Location = new System.Drawing.Point(6, 303);
            this.checkWriteToStatus.Name = "checkWriteToStatus";
            this.checkWriteToStatus.Size = new System.Drawing.Size(154, 17);
            this.checkWriteToStatus.TabIndex = 14;
            this.checkWriteToStatus.Text = "Write output to status pane";
            this.checkWriteToStatus.UseVisualStyleBackColor = true;
            // 
            // checkConsoleApp
            // 
            this.checkConsoleApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkConsoleApp.AutoSize = true;
            this.checkConsoleApp.Enabled = false;
            this.checkConsoleApp.Location = new System.Drawing.Point(6, 280);
            this.checkConsoleApp.Name = "checkConsoleApp";
            this.checkConsoleApp.Size = new System.Drawing.Size(118, 17);
            this.checkConsoleApp.TabIndex = 13;
            this.checkConsoleApp.Text = "Console application";
            this.checkConsoleApp.UseVisualStyleBackColor = true;
            // 
            // checkAddToContextMenu
            // 
            this.checkAddToContextMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkAddToContextMenu.AutoSize = true;
            this.checkAddToContextMenu.Enabled = false;
            this.checkAddToContextMenu.Location = new System.Drawing.Point(6, 257);
            this.checkAddToContextMenu.Name = "checkAddToContextMenu";
            this.checkAddToContextMenu.Size = new System.Drawing.Size(124, 17);
            this.checkAddToContextMenu.TabIndex = 12;
            this.checkAddToContextMenu.Text = "Add to context menu";
            this.checkAddToContextMenu.UseVisualStyleBackColor = true;
            // 
            // textDir
            // 
            this.textDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textDir.Location = new System.Drawing.Point(92, 220);
            this.textDir.Name = "textDir";
            this.textDir.ReadOnly = true;
            this.textDir.Size = new System.Drawing.Size(323, 20);
            this.textDir.TabIndex = 8;
            // 
            // textArgs
            // 
            this.textArgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textArgs.Location = new System.Drawing.Point(92, 194);
            this.textArgs.Name = "textArgs";
            this.textArgs.ReadOnly = true;
            this.textArgs.Size = new System.Drawing.Size(323, 20);
            this.textArgs.TabIndex = 7;
            // 
            // textCmd
            // 
            this.textCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textCmd.Location = new System.Drawing.Point(92, 168);
            this.textCmd.Name = "textCmd";
            this.textCmd.ReadOnly = true;
            this.textCmd.Size = new System.Drawing.Size(323, 20);
            this.textCmd.TabIndex = 6;
            // 
            // btDown
            // 
            this.btDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btDown.Enabled = false;
            this.btDown.Location = new System.Drawing.Point(335, 139);
            this.btDown.Name = "btDown";
            this.btDown.Size = new System.Drawing.Size(75, 23);
            this.btDown.TabIndex = 5;
            this.btDown.Text = "Move Down";
            this.btDown.UseVisualStyleBackColor = true;
            this.btDown.Click += new System.EventHandler(this.BtDownClick);
            // 
            // btUp
            // 
            this.btUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btUp.Enabled = false;
            this.btUp.Location = new System.Drawing.Point(254, 139);
            this.btUp.Name = "btUp";
            this.btUp.Size = new System.Drawing.Size(75, 23);
            this.btUp.TabIndex = 4;
            this.btUp.Text = "Move Up";
            this.btUp.UseVisualStyleBackColor = true;
            this.btUp.Click += new System.EventHandler(this.BtUpClick);
            // 
            // btRemove
            // 
            this.btRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btRemove.Enabled = false;
            this.btRemove.Location = new System.Drawing.Point(173, 139);
            this.btRemove.Name = "btRemove";
            this.btRemove.Size = new System.Drawing.Size(75, 23);
            this.btRemove.TabIndex = 3;
            this.btRemove.Text = "Remove";
            this.btRemove.UseVisualStyleBackColor = true;
            this.btRemove.Click += new System.EventHandler(this.BtRemoveClick);
            // 
            // btEdit
            // 
            this.btEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btEdit.Enabled = false;
            this.btEdit.Location = new System.Drawing.Point(92, 139);
            this.btEdit.Name = "btEdit";
            this.btEdit.Size = new System.Drawing.Size(75, 23);
            this.btEdit.TabIndex = 2;
            this.btEdit.Text = "Edit";
            this.btEdit.UseVisualStyleBackColor = true;
            this.btEdit.Click += new System.EventHandler(this.BtEditClick);
            // 
            // btAdd
            // 
            this.btAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btAdd.Location = new System.Drawing.Point(11, 139);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(75, 23);
            this.btAdd.TabIndex = 1;
            this.btAdd.Text = "Add";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.BtAddClick);
            // 
            // listTools
            // 
            this.listTools.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listTools.FormattingEnabled = true;
            this.listTools.IntegralHeight = false;
            this.listTools.Location = new System.Drawing.Point(6, 19);
            this.listTools.Name = "listTools";
            this.listTools.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listTools.Size = new System.Drawing.Size(404, 114);
            this.listTools.TabIndex = 0;
            this.listTools.SelectedIndexChanged += new System.EventHandler(this.ListToolsSelectedIndexChanged);
            // 
            // btHelp
            // 
            this.btHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btHelp.Location = new System.Drawing.Point(185, 390);
            this.btHelp.Name = "btHelp";
            this.btHelp.Size = new System.Drawing.Size(75, 23);
            this.btHelp.TabIndex = 3;
            this.btHelp.Text = "Help";
            this.btHelp.UseVisualStyleBackColor = true;
            this.btHelp.Click += new System.EventHandler(this.BtHelpClick);
            // 
            // FormCustomizeTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(440, 425);
            this.Controls.Add(this.btHelp);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(456, 370);
            this.Name = "FormCustomizeTools";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Customize Tools";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCustomizeToolsFormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.ListBox listTools;
        private System.Windows.Forms.CheckBox checkWriteToStatus;
        private System.Windows.Forms.CheckBox checkConsoleApp;
        private System.Windows.Forms.CheckBox checkAddToContextMenu;
        private System.Windows.Forms.TextBox textDir;
        private System.Windows.Forms.TextBox textArgs;
        private System.Windows.Forms.TextBox textCmd;
        private System.Windows.Forms.Button btDown;
        private System.Windows.Forms.Button btUp;
        private System.Windows.Forms.Button btRemove;
        private System.Windows.Forms.Button btEdit;
        private System.Windows.Forms.CheckBox checkCloseUponExit;
        private System.Windows.Forms.CheckBox checkRefresh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBrowse;
        private System.Windows.Forms.CheckBox checkPrompt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textDesc;
        private System.Windows.Forms.Button btHelp;
    }
}