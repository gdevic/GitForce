using System;
using System.Drawing;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Run a Git command using threading and a run window.
    /// This form should be used for Git commands that take long time to complete, such are clone, push and pull.
    /// </summary>
    public partial class FormGitRun : Form
    {
        private Exec job;
        private ExecResult result = new ExecResult();

        /// <summary>
        /// Class constructor that also pre-sets the command and argument to be run
        /// </summary>
        public FormGitRun(string cmd, string args)
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);
            checkAutoclose.Checked = Properties.Settings.Default.AutoCloseGitOnSuccess;

            // WAR: On Linux, remove status bar resizing grip (since it does not work under X)
            if (ClassUtils.IsMono())
                statusStrip.SizingGrip = false;

            // Detect URL in this text box
            textStdout.DetectUrls = true;

            job = new Exec(cmd, args);

            // Reuse the same font selected as fixed-pitch
            textStdout.Font = Properties.Settings.Default.commitFont;
            textStdout.Text += cmd + Environment.NewLine;
            textStdout.Text += args + Environment.NewLine;
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormGitRunFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Start running the preset command at the time form is first shown
        /// </summary>
        private void FormGitRunShown(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            // Start the job using our own output handlers
            job.AsyncRun(PStdout, PStderr, PComplete);
        }

        /// <summary>
        /// Returns the result structure from running a job
        /// </summary>
        public ExecResult GetResult()
        {
            return result;
        }

        /// <summary>
        /// Callback that handles process printing to stdout
        /// </summary>
        private void PStdout(String message)
        {
            textStdout.AppendText(ClassUtils.ToPlainAscii(message) + Environment.NewLine);

            // Keep the newly added text visible
            textStdout.SelectionStart = textStdout.TextLength;
            textStdout.ScrollToCaret();
        }

        /// <summary>
        /// Callback that handles process printing to stderr.
        /// Prints the stderr to a log window.
        /// </summary>
        private void PStderr(String message)
        {
            // This is a workaround for Linux Mono:
            // On Windows, when we clone a remote repo, we receive each status line as a separate message
            // On Linux, it is all clumped together without any newlines (or 0A), so we inject them
            if (ClassUtils.IsMono())
            {
                // A bit of a hack since we simply hard-code recognized types of messages. Oh, well...
                message = message.Replace("remote:", Environment.NewLine + "remote:");
                message = message.Replace("Receiving", Environment.NewLine + "Receiving");
                message = message.Replace("Resolving", Environment.NewLine + "Resolving");
            }
            textStdout.AppendText(ClassUtils.ToPlainAscii(message) + Environment.NewLine, Color.Red);

            // Keep the newly added text visible
            textStdout.SelectionStart = textStdout.TextLength;
            textStdout.ScrollToCaret();

            // This hack recognizes a common problem where the host RSA key was not added to the list
            // of known hosts. Help the user by telling him that and (on Windows) offering to open the
            // Manage Keys dialog.
            if (message.Contains("key fingerprint is"))
            {
                // On Linux / Mono, we don't have a Manage SSH dialog
                if (ClassUtils.IsMono())
                {
                    MessageBox.Show(@"The remote server RSA key was not added to the list of known hosts.", @"Host Key error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    DialogResult response = MessageBox.Show(@"The remote server RSA key was not added to the list of known hosts." + Environment.NewLine + @"Would you like to open the Manage SSH Keys dialog to add the host in the Remote Keys tab?", @"Host Key error", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (response == DialogResult.Yes)
                    {
                        FormSSH formSsh = new FormSSH();
                        formSsh.ShowDialog();
                    }
                }
            }

            // Append the stderr stream message to a log window
            App.PrintLogMessage("stderr: " + message, MessageType.Error);
        }

        /// <summary>
        /// Callback that handles process completion event
        /// </summary>
        private void PComplete(ExecResult result)
        {
            UseWaitCursor = false;
            this.result = result;
            if (result.Success())
            {
                toolStripStatus.Text = "Git command completed successfully.";
                textStdout.AppendText("Git command completed successfully.", Color.Green);
                // On success, auto-close the dialog if the user's preference was checked
                // This behavior can be skipped if the user holds down the Control key
                if (Properties.Settings.Default.AutoCloseGitOnSuccess && Control.ModifierKeys != Keys.Control)
                    DialogResult = DialogResult.OK;
            }
            else
            {
                toolStripStatus.Text = "Git command failed!";
                textStdout.AppendText("Git command failed!", Color.Red);
            }
            btCancel.Text = "Done";
            StopProgress();
        }

        /// <summary>
        /// When the user presses ESC key, close the dialog, but *only* if the git operation is completed.
        /// We need to hook into the key chain and test the completion by checking the button text.
        /// The text changes depending on the execution status.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData==Keys.Escape && btCancel.Text=="Done")
                DialogResult = DialogResult.OK;
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// User presses a cancel button. This is a multi-function button
        /// that starts as "Cancel"...
        /// </summary>
        private void BtCancelClick(object sender, EventArgs e)
        {
            StopProgress();
            if (btCancel.Text == "Cancel")
            {
                textStdout.AppendText(Environment.NewLine + "Error: Git command interrupted!" + Environment.NewLine, Color.Purple);
                toolStripStatus.Text = "Git command interrupted.";
                job.Terminate();
                btCancel.Text = "Close";
                DialogResult = DialogResult.None;
            }
            else
            {
                if (btCancel.Text == "Done")
                    DialogResult = DialogResult.OK;
                if (btCancel.Text == "Close")
                    DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// Call this function when the command completed, or is about to complete.
        /// It signals to the user the end of command by enabling the text box and stopping the progress indicator.
        /// </summary>
        private void StopProgress()
        {
            textStdout.ReadOnly = false;
            timerProgress.Enabled = false;
        }

        /// <summary>
        /// The phase of the progress indicator (0..7)
        /// </summary>
        private int _progressPhase;

        /// <summary>
        /// Use timer to animate progress indicator.
        /// </summary>
        private void TimerProgressTick(object sender, EventArgs e)
        {
            toolStripStatus.Text = "Git command in progress... " + @"|/-\|/-\"[_progressPhase];
            _progressPhase = (_progressPhase + 1) % 8;
        }

        /// <summary>
        /// Called when the user clicks on an HTML link inside the output text.
        /// </summary>
        private void textLinkClicked(object sender, LinkClickedEventArgs e)
        {
            ClassUtils.OpenWebLink(e.LinkText);
        }

        /// <summary>
        /// User clicked on the autoclose checkbox, changed the checked state
        /// Update preferences with the new state
        /// </summary>
        private void CheckAutocloseCheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoCloseGitOnSuccess = checkAutoclose.Checked;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Context menu for the text box (which is read-only, so we can only select and copy text from it)
        /// </summary>
        private void TextStdoutMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu contextMenu = new ContextMenu();

                MenuItem menuItemSelectAll = new MenuItem("Select All");
                menuItemSelectAll.Click += SelectAllAction;
                contextMenu.MenuItems.Add(menuItemSelectAll);

                MenuItem menuItemCopy = new MenuItem("Copy");
                menuItemCopy.Click += CopyAction;
                contextMenu.MenuItems.Add(menuItemCopy);

                textStdout.ContextMenu = contextMenu;
            }
        }

        /// <summary>
        /// Context menu to select all text
        /// </summary>
        private void SelectAllAction(object sender, EventArgs e)
        {
            textStdout.SelectAll();
        }

        /// <summary>
        /// Context menu to copy selected text onto the clipboard
        /// </summary>
        private void CopyAction(object sender, EventArgs e)
        {
            // Copy data in two formats: a simple text format and the more complex rich-text
            // format that keeps the text color and font information
            DataObject data = new DataObject();
            data.SetData(DataFormats.Rtf, true, textStdout.SelectedRtf);
            data.SetData(DataFormats.Text, true, textStdout.SelectedText);
            Clipboard.SetDataObject(data, true);
        }
    }
}
