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
            NativeMethods.SendMessage(textBox.Handle, NativeMethods.WM_VSCROLL, (IntPtr)NativeMethods.SB_BOTTOM, IntPtr.Zero);
        }

        public string[] RunThread(string cmd, string args, Dictionary<string, string> env)
        {
            ClassRunCmd worker = new ClassRunCmd(
                this,
                delegate(string stdout)
                {
                    lock (textBox)
                    {
                        if (!string.IsNullOrEmpty(stdout))
                        {
                            textBox.Text += stdout + '\r';
                            textBox.SelectionStart = textBox.Text.Length;
                            textBox.ScrollToCaret();
                        }
                    }
                });
            worker.Cmd = cmd;
            worker.Args = args;
            worker.StartProcess();
            worker.WaitForExit();

            string[] response = new string[2];
            response[0] = string.Join("\0", worker.Stdout);
            response[1] = string.Join("\0", worker.Stderr);

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

        private static void TextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            ClassRunCmd.Keys.Enqueue(e.KeyChar);
            if (e.KeyChar == '\r')
                ClassRunCmd.Keys.Enqueue('\n');
            ClassRunCmd.EventHanle.Set();
        }

        /// <summary>
        /// Copy selected text to the clipboard
        /// </summary>
        private void MenuExecCopyClick(object sender, EventArgs e)
        {
            textBox.Copy();
        }

        /// <summary>
        /// Select all text
        /// </summary>
        private void MenuExecSelectAllClick(object sender, EventArgs e)
        {
            textBox.SelectAll();
        }

        /// <summary>
        /// Clear the text box
        /// </summary>
        private void MenuExecClearClick(object sender, EventArgs e)
        {
            textBox.Clear();
        }
    }

    public class ClassRunCmd
    {
        public static EventWaitHandle EventHanle = new EventWaitHandle(false, EventResetMode.AutoReset);
        public static volatile Queue Keys = new Queue();

        private Thread _thread;
        private Process _p;
        private readonly EventWaitHandle _eventThreadStarted = new EventWaitHandle(false, EventResetMode.AutoReset);
        private readonly EventWaitHandle _eventThreadExited = new EventWaitHandle(false, EventResetMode.AutoReset);

        public string Cmd;
        public string Args;

        public List<string> Stdout = new List<string>();
        public List<string> Stderr = new List<string>();

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
            _thread = new Thread(RunCmd);
            //set the thread to run in the background
            _thread.IsBackground = true;
            //name our thread (optional)
            _thread.Name = "RunCmdThread_" + Cmd;
            //start our thread
            _thread.Start();
        }

        private void RunCmd()
        {
            _p = new Process();
            _p.StartInfo = new ProcessStartInfo(Cmd, Args);
            _p.StartInfo.RedirectStandardError = true;
            _p.StartInfo.RedirectStandardOutput = true;
            _p.StartInfo.RedirectStandardInput = true;
            _p.StartInfo.CreateNoWindow = true;
            _p.StartInfo.UseShellExecute = false;
            _p.OutputDataReceived += POutputDataReceived;
            _p.ErrorDataReceived += PErrorDataReceived;
            _p.Start();

            _eventThreadStarted.Set();

            StreamWriter streamWr = _p.StandardInput;
            streamWr.AutoFlush = true;

            _p.BeginOutputReadLine();

            while (!_p.HasExited)
            {
                // If a key is available, get it, but wait for it max 1 ms
                if (EventHanle.WaitOne(1))
                {
                    while (Keys.Count > 0)
                    {
                        char key = (char)Keys.Dequeue();
                        streamWr.Write(key);
                    }
                }
            }
            _p.Close();
            _eventThreadExited.Set();
        }

        public void WaitForExit()
        {
            _eventThreadStarted.WaitOne();
            _eventThreadExited.WaitOne(-1);
        }

        private void POutputDataReceived(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                Stdout.Add(outLine.Data);

                object[] items = new object[1];
                items[0] = outLine.Data;
                _synch.Invoke(_status, items);
            }
        }

        private void PErrorDataReceived(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                Stderr.Add(outLine.Data);
            }
        }
    }
}
