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

namespace Git4Win
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

        /// <summary>
        /// Class constructor that also presets command to be run
        /// </summary>
        public FormGitRun(string cmd, string args)
        {
            InitializeComponent();
            _cmd = cmd;
            _args = args;
        }

        /// <summary>
        /// Start running the preset command at the time form is first shown
        /// </summary>
        private void FormGitRunShown(object sender, EventArgs e)
        {
            // Create a start an execution thread with various
            // callbacks for stdout, stderr and command completion
            ClassExecute.ThreadedParameters parameters;
            _thRun = new Thread(ClassExecute.RunThreaded);

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
                textStdout.Text += e.Data;
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
                toolStripStatus.Text = e.Data;
            }
        }

        /// <summary>
        /// Callback that handles process completion event
        /// </summary>
        private void PComplete()
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(PComplete));
            else
            {
                btCancel.Text = "Done";
            }
        }

        /// <summary>
        /// User presses a cancel button. This is a multi-function button
        /// that starts as "Cancel"...
        /// </summary>
        private void BtCancelClick(object sender, EventArgs e)
        {
            if(btCancel.Text=="Cancel")
            {
                ClassExecute.TerminateThreaded();
                _thRun.Join(1000);

                btCancel.Text = "Close";
            }
            else
            {
                if (btCancel.Text == "Done")
                    DialogResult = DialogResult.OK;
                if (btCancel.Text == "Close")
                    DialogResult = DialogResult.Cancel;
            }
        }
    }
}
