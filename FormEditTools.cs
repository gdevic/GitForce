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
        public ClassTool Tool;

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
    }
}
