using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
        private readonly string _cmd;
        private readonly string _args;
        private Thread _thRun;
        private string _lastError;
        private string _ec;

        /// <summary>
        /// Class constructor that also presets command to be run
        /// </summary>
        public FormGitRun(string cmd, string args)
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            // WAR: On Linux, remove status bar resizing grip (since it does not work under X)
            if (ClassUtils.IsMono())
                statusStrip.SizingGrip = false;

            _cmd = cmd;
            _args = args;

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
            // Create and start an execution thread with various
            // callbacks for stdout, stderr and command completion
            ClassExecute.ThreadedParameters parameters;
            _thRun = new Thread(ClassExecute.RunNativeProcess);

            parameters.Cmd = _cmd;
            parameters.Args = _args;
            parameters.F0 = POutputDataReceived;
            parameters.F1 = PErrorDataReceived;
            parameters.FComplete = PComplete;

            _thRun.Start(parameters);
        }

        /// <summary>
        /// Callback that handles process printing to stdout
        /// </summary>
        private void POutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (String.IsNullOrEmpty(e.Data)) return;
            if(InvokeRequired)
                BeginInvoke((MethodInvoker) (() => POutputDataReceived(sender, e)));
            else
            {
                textStdout.Text += e.Data + Environment.NewLine;
                textStdout.Refresh();
            }
        }

        /// <summary>
        /// Callback that handles process printing to stderr
        /// </summary>
        private void PErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (String.IsNullOrEmpty(e.Data)) return;
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => PErrorDataReceived(sender, e)));
            else
            {
                _lastError = e.Data;
                toolStripStatus.Text = e.Data;
            }
        }

        /// <summary>
        /// Callback that handles process completion event
        /// </summary>
        private void PComplete(object exitCode)
        {
            if(InvokeRequired)
            {
                BeginInvoke((MethodInvoker) delegate {
                    _ec = (string)exitCode;
                    textStdout.Text += _lastError + Environment.NewLine;
                    btCancel.Text = "Done";
                    StopProgress();
                });
            }
        }

        /// <summary>
        /// Returns a string containing the result of Git command as printed to stdout stream
        /// </summary>
        public string GetStdout()
        {
            return textStdout.Text;
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
                ClassExecute.TerminateThreaded();
                _thRun.Join(3000);

                btCancel.Text = "Close";
            }
            else
            {
                if (_ec != "0")
                    ClassUtils.LastError = _lastError;

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
    }
}
