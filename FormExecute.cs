using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace git4win
{
    public partial class FormExecute : Form
    {
        public FormExecute()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the Execute form title (usually set to the command being run)
        /// </summary>
        public void SetTitle(string title)
        {
            Text = title;
        }

        /// <summary>
        /// Adds a text string to the end of the text box.
        /// For performance reasons, only up to 120 characters of text are added in one call.
        /// </summary>
        public void Add(string text)
        {
            int len = Math.Min(text.Length, 120);
            textBox.Text += text.Substring(0, len) + Environment.NewLine;

            // Scroll to the bottom, but don't move the caret position.
            Win32.SendMessage(textBox.Handle, Win32.WM_VSCROLL, (IntPtr)Win32.SB_BOTTOM, IntPtr.Zero);
        }

        public string[] RunThread(string cmd, string args, Dictionary<string, string> env)
        {
            ClassRunCmd worker = new ClassRunCmd(
                this,
                delegate(string _stdout)
                {
                    lock (textBox)
                    {
                        if (!string.IsNullOrEmpty(_stdout))
                        {
                            textBox.Text += _stdout + '\r';
                            textBox.SelectionStart = textBox.Text.Length;
                            textBox.ScrollToCaret();
                        }
                    }
                });
            worker.cmd = cmd;
            worker.args = args;
            worker.StartProcess();
            worker.WaitForExit();

            string[] response = new string[2];
            response[0] = string.Join("\0", worker.stdout);
            response[1] = string.Join("\0", worker.stderr);

            return response;
        }

        /// <summary>
        /// Form helper function that shows or hides a form using a simple boolean argument directive
        /// </summary>
        public void Show(bool toShow)
        {
            if (toShow)
                Show();
            else
                Hide();
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassRunCmd.keys.Enqueue(e.KeyChar);
            if (e.KeyChar == '\r')
                ClassRunCmd.keys.Enqueue('\n');
            ClassRunCmd.eventHanle.Set();
        }

        /// <summary>
        /// Copy selected text to the clipboard
        /// </summary>
        private void menuExecCopy_Click(object sender, EventArgs e)
        {
            textBox.Copy();
        }

        /// <summary>
        /// Select all text
        /// </summary>
        private void menuExecSelectAll_Click(object sender, EventArgs e)
        {
            textBox.SelectAll();
        }

        /// <summary>
        /// Clear the text box
        /// </summary>
        private void menuExecClear_Click(object sender, EventArgs e)
        {
            textBox.Clear();
        }
    }

    public class ClassRunCmd
    {
        public static EventWaitHandle eventHanle = new EventWaitHandle(false, EventResetMode.AutoReset);
        public static volatile Queue keys = new Queue();

        private Thread thread;
        private Process p = null;
        private EventWaitHandle eventThreadStarted = new EventWaitHandle(false, EventResetMode.AutoReset);
        private EventWaitHandle eventThreadExited = new EventWaitHandle(false, EventResetMode.AutoReset);

        public string cmd = null;
        public string args = null;

        public List<string> stdout = new List<string>();
        public List<string> stderr = new List<string>();

        //our ISynchronizeInvoke object
        private static ISynchronizeInvoke _synch;

        //instance of our delegate
        private static ProcessStatus _status;

        /// <summary>
        /// Delegate that mashalls our call to the UI thread for updating UI
        /// </summary>
        public delegate void ProcessStatus(string stdout);

        /// <summary>
        /// Constructor using overloading
        /// </summary>
        /// <param name="syn">our ISynchronizeInvoke object</param>
        /// <param name="notify">our ProcessStatus object</param>
        public ClassRunCmd(ISynchronizeInvoke syn, ProcessStatus notify)
        {
            _synch = syn;
            _status = notify;
        }

        public void StartProcess()
        {
            //we need to create a new thread for our process
            thread = new Thread(RunCmd);
            //set the thread to run in the background
            thread.IsBackground = true;
            //name our thread (optional)
            thread.Name = "RunCmdThread_" + cmd;
            //start our thread
            thread.Start();
        }

        private void RunCmd()
        {
            p = new Process();
            p.StartInfo = new ProcessStartInfo(cmd, args);
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.OutputDataReceived += p_OutputDataReceived;
            p.ErrorDataReceived += p_ErrorDataReceived;
            p.Start();

            eventThreadStarted.Set();

            StreamWriter streamWr = p.StandardInput;
            streamWr.AutoFlush = true;

            p.BeginOutputReadLine();

            while (!p.HasExited)
            {
                // If a key is available, get it, but wait for it max 1 ms
                if (eventHanle.WaitOne(1))
                {
                    while (keys.Count > 0)
                    {
                        char key = (char)keys.Dequeue();
                        streamWr.Write(key);
                    }
                }
            }
            p.Close();
            eventThreadExited.Set();
        }

        public void WaitForExit()
        {
            eventThreadStarted.WaitOne();
            eventThreadExited.WaitOne(-1);
        }

        private void p_OutputDataReceived(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                stdout.Add(outLine.Data);

                object[] items = new object[1];
                items[0] = outLine.Data;
                _synch.Invoke(_status, items);
            }
        }

        private void p_ErrorDataReceived(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                stderr.Add(outLine.Data);
            }
        }
    }
}
