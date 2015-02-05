namespace GitForce
{
    partial class FormEditTools
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
            this.comboHelpDir = new System.Windows.Forms.ComboBox();
            this.comboHelpArg = new System.Windows.Forms.ComboBox();
            this.textDesc = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBrowse = new System.Windows.Forms.CheckBox();
            this.checkPrompt = new System.Windows.Forms.CheckBox();
            this.checkCloseUponExit = new System.Windows.Forms.CheckBox();
            this.checkRefresh = new System.Windows.Forms.CheckBox();
            this.checkWriteToStatus = new System.Windows.Forms.CheckBox();
            this.checkConsoleApp = new System.Windows.Forms.CheckBox();
            this.checkAddToContextMenu = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textDir = new System.Windows.Forms.TextBox();
            this.btBrowseDir = new System.Windows.Forms.Button();
            this.textArgs = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btBrowse = new System.Windows.Forms.Button();
            this.textCmd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textName = new System.Windows.Forms.TextBox();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.btHelp = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(381, 266);
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
            this.btOK.Enabled = false;
            this.btOK.Location = new System.Drawing.Point(300, 266);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.BtOkClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.comboHelpDir);
            this.groupBox1.Controls.Add(this.comboHelpArg);
            this.groupBox1.Controls.Add(this.textDesc);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.checkBrowse);
            this.groupBox1.Controls.Add(this.checkPrompt);
            this.groupBox1.Controls.Add(this.checkCloseUponExit);
            this.groupBox1.Controls.Add(this.checkRefresh);
            this.groupBox1.Controls.Add(this.checkWriteToStatus);
            this.groupBox1.Controls.Add(this.checkConsoleApp);
            this.groupBox1.Controls.Add(this.checkAddToContextMenu);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textDir);
            this.groupBox1.Controls.Add(this.btBrowseDir);
            this.groupBox1.Controls.Add(this.textArgs);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btBrowse);
            this.groupBox1.Controls.Add(this.textCmd);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textName);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(444, 248);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Menu Item";
            // 
            // comboHelpDir
            // 
            this.comboHelpDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboHelpDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboHelpDir.FormattingEnabled = true;
            this.comboHelpDir.Items.AddRange(new object[] {
            "%r - Root directory of the current git repository",
            "%u - User name",
            "%e - User email",
            "%b - Active branch of the current git repository",
            "%f - Single selected file in the left pane",
            "%F - List of selected files in the left pane",
            "%d - Single selected directory in the left pane",
            "%D - List of selected directories in the left pane"});
            this.comboHelpDir.Location = new System.Drawing.Point(340, 101);
            this.comboHelpDir.Name = "comboHelpDir";
            this.comboHelpDir.Size = new System.Drawing.Size(17, 21);
            this.comboHelpDir.TabIndex = 30;
            this.comboHelpDir.SelectedIndexChanged += new System.EventHandler(this.ComboHelpSelectedIndexChanged);
            // 
            // comboHelpArg
            // 
            this.comboHelpArg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboHelpArg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboHelpArg.DropDownWidth = 17;
            this.comboHelpArg.FormattingEnabled = true;
            this.comboHelpArg.Items.AddRange(new object[] {
            "%r - Root directory of the current git repository",
            "%u - User name",
            "%e - User email",
            "%b - Active branch of the current git repository",
            "%f - Single selected file in the left pane",
            "%F - List of selected files in the left pane",
            "%d - Single selected directory in the left pane",
            "%D - List of selected directories in the left pane"});
            this.comboHelpArg.Location = new System.Drawing.Point(420, 73);
            this.comboHelpArg.Name = "comboHelpArg";
            this.comboHelpArg.Size = new System.Drawing.Size(17, 21);
            this.comboHelpArg.TabIndex = 29;
            this.comboHelpArg.SelectedIndexChanged += new System.EventHandler(this.ComboHelpSelectedIndexChanged);
            // 
            // textDesc
            // 
            this.textDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textDesc.Location = new System.Drawing.Point(199, 200);
            this.textDesc.Name = "textDesc";
            this.textDesc.Size = new System.Drawing.Size(239, 20);
            this.textDesc.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(196, 180);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Description:";
            // 
            // checkBrowse
            // 
            this.checkBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBrowse.AutoSize = true;
            this.checkBrowse.Location = new System.Drawing.Point(199, 156);
            this.checkBrowse.Name = "checkBrowse";
            this.checkBrowse.Size = new System.Drawing.Size(161, 17);
            this.checkBrowse.TabIndex = 12;
            this.checkBrowse.Text = "Add Browse to prompt dialog";
            this.checkBrowse.UseVisualStyleBackColor = true;
            // 
            // checkPrompt
            // 
            this.checkPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkPrompt.AutoSize = true;
            this.checkPrompt.Location = new System.Drawing.Point(199, 133);
            this.checkPrompt.Name = "checkPrompt";
            this.checkPrompt.Size = new System.Drawing.Size(126, 17);
            this.checkPrompt.TabIndex = 11;
            this.checkPrompt.Text = "Prompt for arguments";
            this.checkPrompt.UseVisualStyleBackColor = true;
            // 
            // checkCloseUponExit
            // 
            this.checkCloseUponExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkCloseUponExit.AutoSize = true;
            this.checkCloseUponExit.Location = new System.Drawing.Point(6, 202);
            this.checkCloseUponExit.Name = "checkCloseUponExit";
            this.checkCloseUponExit.Size = new System.Drawing.Size(137, 17);
            this.checkCloseUponExit.TabIndex = 9;
            this.checkCloseUponExit.Text = "Close window upon exit";
            this.checkCloseUponExit.UseVisualStyleBackColor = true;
            this.checkCloseUponExit.CheckedChanged += new System.EventHandler(this.CheckCloseUponExitCheckedChanged);
            // 
            // checkRefresh
            // 
            this.checkRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkRefresh.AutoSize = true;
            this.checkRefresh.Location = new System.Drawing.Point(6, 225);
            this.checkRefresh.Name = "checkRefresh";
            this.checkRefresh.Size = new System.Drawing.Size(152, 17);
            this.checkRefresh.TabIndex = 10;
            this.checkRefresh.Text = "Refresh GitForce upon exit";
            this.checkRefresh.UseVisualStyleBackColor = true;
            // 
            // checkWriteToStatus
            // 
            this.checkWriteToStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkWriteToStatus.AutoSize = true;
            this.checkWriteToStatus.Location = new System.Drawing.Point(6, 179);
            this.checkWriteToStatus.Name = "checkWriteToStatus";
            this.checkWriteToStatus.Size = new System.Drawing.Size(154, 17);
            this.checkWriteToStatus.TabIndex = 8;
            this.checkWriteToStatus.Text = "Write output to status pane";
            this.checkWriteToStatus.UseVisualStyleBackColor = true;
            // 
            // checkConsoleApp
            // 
            this.checkConsoleApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkConsoleApp.AutoSize = true;
            this.checkConsoleApp.Location = new System.Drawing.Point(6, 156);
            this.checkConsoleApp.Name = "checkConsoleApp";
            this.checkConsoleApp.Size = new System.Drawing.Size(118, 17);
            this.checkConsoleApp.TabIndex = 7;
            this.checkConsoleApp.Text = "Console application";
            this.checkConsoleApp.UseVisualStyleBackColor = true;
            this.checkConsoleApp.CheckedChanged += new System.EventHandler(this.CheckConsoleAppCheckedChanged);
            // 
            // checkAddToContextMenu
            // 
            this.checkAddToContextMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkAddToContextMenu.AutoSize = true;
            this.checkAddToContextMenu.Location = new System.Drawing.Point(6, 133);
            this.checkAddToContextMenu.Name = "checkAddToContextMenu";
            this.checkAddToContextMenu.Size = new System.Drawing.Size(124, 17);
            this.checkAddToContextMenu.TabIndex = 6;
            this.checkAddToContextMenu.Text = "Add to context menu";
            this.checkAddToContextMenu.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Initial directory:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Arguments:";
            // 
            // textDir
            // 
            this.textDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textDir.Location = new System.Drawing.Point(119, 102);
            this.textDir.Name = "textDir";
            this.textDir.Size = new System.Drawing.Size(215, 20);
            this.textDir.TabIndex = 4;
            // 
            // btBrowseDir
            // 
            this.btBrowseDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btBrowseDir.Location = new System.Drawing.Point(363, 100);
            this.btBrowseDir.Name = "btBrowseDir";
            this.btBrowseDir.Size = new System.Drawing.Size(75, 23);
            this.btBrowseDir.TabIndex = 5;
            this.btBrowseDir.Text = "Browse...";
            this.btBrowseDir.UseVisualStyleBackColor = true;
            this.btBrowseDir.Click += new System.EventHandler(this.BtBrowseDirClick);
            // 
            // textArgs
            // 
            this.textArgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textArgs.Location = new System.Drawing.Point(119, 74);
            this.textArgs.Name = "textArgs";
            this.textArgs.Size = new System.Drawing.Size(295, 20);
            this.textArgs.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Command:";
            // 
            // btBrowse
            // 
            this.btBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btBrowse.Location = new System.Drawing.Point(363, 45);
            this.btBrowse.Name = "btBrowse";
            this.btBrowse.Size = new System.Drawing.Size(75, 23);
            this.btBrowse.TabIndex = 2;
            this.btBrowse.Text = "Browse...";
            this.btBrowse.UseVisualStyleBackColor = true;
            this.btBrowse.Click += new System.EventHandler(this.BtBrowseClick);
            // 
            // textCmd
            // 
            this.textCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textCmd.Location = new System.Drawing.Point(119, 47);
            this.textCmd.Name = "textCmd";
            this.textCmd.Size = new System.Drawing.Size(238, 20);
            this.textCmd.TabIndex = 1;
            this.textCmd.TextChanged += new System.EventHandler(this.NameCmdChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name:";
            // 
            // textName
            // 
            this.textName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textName.Location = new System.Drawing.Point(119, 19);
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(319, 20);
            this.textName.TabIndex = 0;
            this.textName.TextChanged += new System.EventHandler(this.NameCmdChanged);
            // 
            // openFile
            // 
            this.openFile.DefaultExt = "*.*";
            this.openFile.Filter = "Executable file (*.*)|*.*|All files (*.*)|*.*";
            this.openFile.Title = "Select the command";
            // 
            // folderBrowser
            // 
            this.folderBrowser.Description = "Select the folder you want GitForce to use as a working directory for this tool:";
            this.folderBrowser.Tag = "";
            // 
            // btHelp
            // 
            this.btHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btHelp.Location = new System.Drawing.Point(211, 266);
            this.btHelp.Name = "btHelp";
            this.btHelp.Size = new System.Drawing.Size(75, 23);
            this.btHelp.TabIndex = 3;
            this.btHelp.Text = "Help";
            this.btHelp.UseVisualStyleBackColor = true;
            this.btHelp.Click += new System.EventHandler(this.BtHelpClick);
            // 
            // FormEditTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 301);
            this.Controls.Add(this.btHelp);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(404, 339);
            this.Name = "FormEditTools";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Edit Tools Menu Item";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEditToolsFormClosing);
            this.Shown += new System.EventHandler(this.FormEditToolsShown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textDir;
        private System.Windows.Forms.Button btBrowseDir;
        private System.Windows.Forms.TextBox textArgs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btBrowse;
        private System.Windows.Forms.TextBox textCmd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textDesc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBrowse;
        private System.Windows.Forms.CheckBox checkPrompt;
        private System.Windows.Forms.CheckBox checkCloseUponExit;
        private System.Windows.Forms.CheckBox checkRefresh;
        private System.Windows.Forms.CheckBox checkWriteToStatus;
        private System.Windows.Forms.CheckBox checkConsoleApp;
        private System.Windows.Forms.CheckBox checkAddToContextMenu;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.Button btHelp;
        private System.Windows.Forms.ComboBox comboHelpArg;
        private System.Windows.Forms.ComboBox comboHelpDir;
    }
}