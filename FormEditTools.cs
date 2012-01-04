using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Edit individual tool item.
    /// </summary>
    public partial class FormEditTools : Form
    {
        /// <summary>
        /// Public tool structure containing what the user edited
        /// </summary>
        public readonly ClassTool Tool;

        public FormEditTools(ClassTool tool)
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);
            Tool = (ClassTool) tool.Clone();

            textName.Text = tool.Name;
            textCmd.Text = tool.Cmd;
            textArgs.Text = tool.Args;
            textDir.Text = tool.Dir;
            textDesc.Text = tool.Desc;
            checkAddToContextMenu.Checked = tool.Checks[0];
            checkConsoleApp.Checked = tool.Checks[1];
            checkWriteToStatus.Checked = tool.Checks[2];
            checkCloseUponExit.Checked = tool.Checks[3];
            checkRefresh.Checked = tool.Checks[4];
            checkPrompt.Checked = tool.Checks[5];
            checkBrowse.Checked = tool.Checks[6];

            // TODO: Running a command line tool does not work on Linux at the moment
            if(ClassUtils.IsMono())
            {
                checkConsoleApp.Enabled = checkWriteToStatus.Enabled = checkCloseUponExit.Enabled = false;
                checkConsoleApp.Checked = checkWriteToStatus.Checked = checkCloseUponExit.Checked = false;
            }

            // Adjust the enables (in this order)
            CheckCloseUponExitCheckedChanged(null, null);
            CheckConsoleAppCheckedChanged(null, null);
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormEditToolsFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// User clicked on the OK button to accept the tool.
        /// </summary>
        private void BtOkClick(object sender, EventArgs e)
        {
            Tool.Name = textName.Text.Trim();
            Tool.Cmd = textCmd.Text.Trim();
            Tool.Args = textArgs.Text.Trim();
            Tool.Dir = textDir.Text.Trim();
            Tool.Desc = textDesc.Text.Trim();
            Tool.Checks[0] = checkAddToContextMenu.Checked;
            Tool.Checks[1] = checkConsoleApp.Checked;
            Tool.Checks[2] = checkWriteToStatus.Checked;
            Tool.Checks[3] = checkCloseUponExit.Checked;
            Tool.Checks[4] = checkRefresh.Checked;
            Tool.Checks[5] = checkPrompt.Checked;
            Tool.Checks[6] = checkBrowse.Checked;
        }

        /// <summary>
        /// As the form is shown, put the focus to the name field
        /// </summary>
        private void FormEditToolsShown(object sender, EventArgs e)
        {
            textName.Focus();
        }

        /// <summary>
        /// User clicked on the Browse button to select a command to run
        /// </summary>
        private void BtBrowseClick(object sender, EventArgs e)
        {
            if(openFile.ShowDialog()==DialogResult.OK)
                textCmd.Text = openFile.FileName;
        }

        /// <summary>
        /// User clicked on the Browse button to select the initial working directory
        /// </summary>
        private void BtBrowseDirClick(object sender, EventArgs e)
        {
            if(folderBrowser.ShowDialog()==DialogResult.OK)
                textDir.Text = folderBrowser.SelectedPath;
        }

        /// <summary>
        /// Called when tool name or command input text have changed.
        /// We can allow OK button to be enabled (and the tool accepted) only
        /// when the tool name is not empty and the command exists.
        /// </summary>
        private void NameCmdChanged(object sender, EventArgs e)
        {
            string name = textName.Text.Trim();
            string cmd = textCmd.Text.Trim();
            btOK.Enabled = !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(cmd);
        }

        /// <summary>
        /// Checkbox changed for the console app vs. GUI app
        /// </summary>
        private void CheckConsoleAppCheckedChanged(object sender, EventArgs e)
        {
            // If this is a console app, we can have control over the stdout redirection
            // and closing the command upon exit, otherwise these options do not make sense
            if(checkConsoleApp.Checked==false)
                checkWriteToStatus.Enabled = checkCloseUponExit.Enabled = false;
            else
                checkWriteToStatus.Enabled = checkCloseUponExit.Enabled = true;
        }

        /// <summary>
        /// Checkbox changed for the Close upon exit vs. leave the console up
        /// </summary>
        private void CheckCloseUponExitCheckedChanged(object sender, EventArgs e)
        {
            // If we are running a command line tool, we can redirect the stdout only
            // if the tool is run directly (thus closing upon exit), otherwise we need
            // to leave the CMD/SHELL open and cannot redirect the output
            if (checkCloseUponExit.Checked)
                checkWriteToStatus.Enabled = true;
            else
                checkWriteToStatus.Enabled = false;
        }

        /// <summary>
        /// Open the context help
        /// </summary>
        private void BtHelpClick(object sender, EventArgs e)
        {
            ClassHelp.Handler("Edit Tools");
        }
    }
}
