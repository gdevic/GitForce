using System;
using System.Drawing;
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
            checkAddToContextMenu.Checked = tool.IsAddToContextMenu;
            checkConsoleApp.Checked = tool.IsConsoleApp;
            checkWriteToStatus.Checked = tool.IsWriteOutput;
            checkCloseUponExit.Checked = tool.IsCloseWindowOnExit;
            checkRefresh.Checked = tool.IsRefresh;
            checkPrompt.Checked = tool.IsPromptForArgs;
            checkBrowse.Checked = tool.IsAddBrowse;

            // TODO: Running a command line tool does not work on Linux at the moment
            if(ClassUtils.IsMono())
            {
                checkConsoleApp.Enabled = checkWriteToStatus.Enabled = checkCloseUponExit.Enabled = false;
                checkConsoleApp.Checked = checkWriteToStatus.Checked = checkCloseUponExit.Checked = false;
            }

            // Adjust the enables (in this order)
            CheckCloseUponExitCheckedChanged(null, null);
            CheckConsoleAppCheckedChanged(null, null);

            // Set the width of drop-down portions of help combo box so when it expands (down)
            // it will show horizontally the complete text from all pre-defined lines.
            // Also set their tags to point to the buddy edit boxes into which to insert selected tokens.
            SetComboBoxWidth(comboHelpArg);
            comboHelpArg.Tag = textArgs;
            SetComboBoxWidth(comboHelpDir);
            comboHelpDir.Tag = textDir;
        }

        /// <summary>
        /// Resizes the combo box size horizontally to show all content strings
        /// http://rajeshkm.blogspot.com/2006/11/adjust-combobox-drop-down-list-width-c.html
        /// </summary>
        private static void SetComboBoxWidth(ComboBox box)
        {
            int width = box.Width;
            Graphics g = box.CreateGraphics();
            Font font = box.Font;

            // Checks if a scrollbar will be displayed.
            // If yes, then get its width to adjust the size of the drop down list.
            int vertScrollBarWidth =
                (box.Items.Count > box.MaxDropDownItems)
                ? SystemInformation.VerticalScrollBarWidth : 0;

            // Loop through list items and check size of each items.
            // Set the width of the drop down list to the width of the largest item.
            foreach (string s in box.Items)
            {
                if (s != null)
                {
                    int newWidth = (int)g.MeasureString(s.Trim(), font).Width + vertScrollBarWidth;
                    if (width < newWidth)
                        width = newWidth;
                }
            }
            box.DropDownWidth = width;
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
            Tool.IsAddToContextMenu = checkAddToContextMenu.Checked;
            Tool.IsConsoleApp = checkConsoleApp.Checked;
            Tool.IsWriteOutput = checkWriteToStatus.Checked;
            Tool.IsCloseWindowOnExit = checkCloseUponExit.Checked;
            Tool.IsRefresh = checkRefresh.Checked;
            Tool.IsPromptForArgs = checkPrompt.Checked;
            Tool.IsAddBrowse = checkBrowse.Checked;
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
        /// User clicked on the Help button, open the context help
        /// </summary>
        private void BtHelpClick(object sender, EventArgs e)
        {
            ClassHelp.Handler("Edit Tools");
        }

        /// <summary>
        /// The user selected an item from the list of help items to insert into
        /// the edit field.
        /// </summary>
        private void ComboHelpSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Insert (or substitute if there is a selection) first few characters
                // of a token into the corresponding edit field (kept in Tag of a combo box)
                ComboBox box = sender as ComboBox;
                box.Text = string.Empty;
                TextBox textBox = box.Tag as TextBox;
                string token = (string)box.SelectedItem;
                textBox.SelectedText = token.Substring(0, 2);
                textBox.Focus();
            }
            catch (Exception ex)
            {
                App.PrintLogMessage("Error: " + ex.Message, MessageType.Error);
            }
        }
    }
}