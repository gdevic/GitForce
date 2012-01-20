using System;
using System.Drawing;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Run a Git command using threading and a run window.
    /// This form should be used for Git commands that take long time to complete,
    /// such are clone, push and pull.
    /// </summary>
    public partial class FormGitRun : Form
    {
        private Exec job;
        private ExecResult _result = new ExecResult();

        /// <summary>
        /// Class constructor that also pre-sets the command and argument to be run
        /// </summary>
        public FormGitRun(string cmd, string args)
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            // WAR: On Linux, remove status bar resizing grip (since it does not work under X)
            if (ClassUtils.IsMono())
                statusStrip.SizingGrip = false;

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
            // Start the job using our own output handlers
            job.AsyncRun(PStdout, PStderr, PComplete);
        }

        /// <summary>
        /// Returns the result structure from running a job
        /// </summary>
        public ExecResult GetResult()
        {
            return _result;
        }

        /// <summary>
        /// Callback that handles process printing to stdout
        /// </summary>
        private void PStdout(String message)
        {
            textStdout.Text += ClassUtils.ToPlainAscii(message) + Environment.NewLine;

            // Keep the newly added text visible
            textStdout.SelectionStart = textStdout.TextLength;                    
            textStdout.ScrollToCaret();                    
        }

        /// <summary>
        /// Callback that handles process printing to stderr
        /// </summary>
        private void PStderr(String message)
        {
            textStdout.AppendText(ClassUtils.ToPlainAscii(message) + Environment.NewLine, Color.Red);
        }

        /// <summary>
        /// Callback that handles process completion event
        /// </summary>
        private void PComplete(ExecResult result)
        {
            _result = result;
            toolStripStatus.Text = "Git command completed: " + (result.Success() ? "OK" : "Failed");
            btCancel.Text = "Done";
            StopProgress();
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
        /// It signals to the user the end of command by enabling the text box
        /// and disabling the progress indicator.
        /// </summary>
        private void StopProgress()
        {
            textStdout.ReadOnly = false;
            timerProgress.Enabled = false;
            labelProgress.Text = " ";
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
            labelProgress.Text = @"|/-\|/-\"[_progressPhase].ToString();
            _progressPhase = (_progressPhase + 1)%8;
        }

        /// <summary>
        /// Called when the user clicks on an HTML link inside the output text.
        /// </summary>
        private void textLinkClicked(object sender, LinkClickedEventArgs e)
        {
            ClassUtils.OpenWebLink(e.LinkText);
        }
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
