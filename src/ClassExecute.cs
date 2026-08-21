using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// * Simple case: execute one command: Run()
    ///     returns when the command completes
    ///     return structure including the stdout
    ///     blocks the calling thread until the command completes
    /// * More complex case: AsyncRun()
    ///     command takes more time: asynchronous execution with a completion callback
    ///     callbacks for stdout and stderr
    ///     can terminate execution from another thread: Terminate()
    /// </summary>
    public class ExecResult
    {
        public string stdout = string.Empty;
        public string stderr = string.Empty;
        public int retcode = -1;

        public override string ToString()
        {
            return stdout;
        }

        public bool Success()
        {
            return retcode == 0;
        }
    }

    /// <summary>
    /// Contains functions to execute external console applications.
    /// Standard streams (stdout/stderr) are captured and returned.
    ///
    /// Command shell is not invoked as that would prevent capturing
    /// the streams. Internally, the invocation is asynchronous.
    /// </summary>
    public class Exec
    {
        private readonly ExecResult Result = new ExecResult();

        /// <summary>
        /// Delegate for the completion function
        /// </summary>
        public delegate void PStdoutDelegate(String s);
        public delegate void PStderrDelegate(String s);
        public delegate void PCompleteDelegate(ExecResult result);

        private Process Proc;
        private Thread Thread;
        private PStdoutDelegate FStdout;
        private PStderrDelegate FStderr;
        private PCompleteDelegate FComplete;
        /// <summary>
        /// Signalled when the corresponding redirected stream reports end-of-stream.
        /// Manual-reset so that repeated end-of-stream callbacks are harmless (plink is
        /// known to deliver more than one when a new key is added).
        /// </summary>
        private readonly ManualResetEvent StdoutEof = new ManualResetEvent(false);
        private readonly ManualResetEvent StderrEof = new ManualResetEvent(false);

        /// <summary>
        /// True when the standard streams of the child process are redirected to us.
        /// When false, the streams belong to the child's own console and must not be
        /// read, encoded or waited upon (doing so throws).
        /// </summary>
        private readonly bool Redirected;

        /// <summary>
        /// Set once the corresponding stream has delivered at least one line, so that
        /// a leading empty line is preserved instead of being folded away.
        /// </summary>
        private bool anyStdout;
        private bool anyStderr;

        /// <summary>
        /// Grace period (ms) to wait, after the process has already exited, for its
        /// redirected streams to report end-of-stream. Bounded because a grandchild
        /// process that inherited the pipe write handles (a detached 'git gc', a
        /// credential helper) can hold them open long after git itself is gone.
        /// </summary>
        private const int DrainTimeout = 5000;

        public Exec(string cmd, string args)
        {
            // TODO: This is a hack for mergetool: We need to show the window to ask the user if the merge succeeded.
            // The problem is with .NET (and MONO!) buffering of streams prevents us to catching the question on time.
            Redirected = !args.StartsWith("mergetool ");

            Proc = new Process {
                StartInfo =
                {
                    FileName = cmd,
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = Redirected,
                    RedirectStandardOutput = Redirected,
                    RedirectStandardError = Redirected,
                    WorkingDirectory = Directory.GetCurrentDirectory()
                }};

            // Stream encodings may only be set for streams that are actually redirected,
            // otherwise Process.Start() throws. Set both so that non-ASCII text coming
            // back on either stream is decoded the same way.
            if (Redirected)
            {
                Proc.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
                Proc.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;

                Proc.OutputDataReceived += POutputDataReceived;
                Proc.ErrorDataReceived += PErrorDataReceived;
            }

            // Add all environment variables registered for our process environment
            foreach (var variable in ClassUtils.GetEnvars())
            {
                // If a variable with that name already exists, update it
                if (Proc.StartInfo.EnvironmentVariables.ContainsKey(variable.Key))
                    Proc.StartInfo.EnvironmentVariables[variable.Key] = variable.Value;
                else
                    Proc.StartInfo.EnvironmentVariables.Add(variable.Key, variable.Value);
            }
        }

        /// <summary>
        /// Main command execution function.
        /// Upon completion, prints all errors to the log window.
        /// </summary>
        public static ExecResult Run(string cmd, string args)
        {
            App.PrintLogMessage(String.Format("Exec: {0} {1}", cmd, args), MessageType.Command);
            App.StatusBusy(true);
            Exec job = new Exec(cmd, args);
            job.Thread = new Thread(job.ThreadedRun);
            job.Thread.Start();
            job.Thread.Join();
            // There are known problems with async output not being flushed as the
            // thread exits. Releasing a time-slice using DoEvents seems to fix
            // the problem in this particular setting.
            Application.DoEvents();
            App.StatusBusy(false);
            if (job.Result.Success() == false)
                App.PrintLogMessage("Error: " + job.Result.stderr, MessageType.Error);

            return job.Result;
        }

        public void AsyncRun(PStdoutDelegate pstdout, PStderrDelegate pstderr, PCompleteDelegate pcomplete)
        {
            FStdout = pstdout;
            FStderr = pstderr;
            FComplete = pcomplete;

            Thread = new Thread(ThreadedRun);
            Thread.Start();
        }

        /// <summary>
        /// Terminate this job
        /// </summary>
        public void Terminate()
        {
            try
            {
                FStdout = null; // Disable all callbacks since the client class could have been disposed of
                FStderr = null;
                FComplete = null;
                Proc.Kill();    // Immediately stop the process!
            }
            catch (Exception)
            {
                App.PrintLogMessage("Exec.Terminate() exception", MessageType.Error);
            }
        }

        /// <summary>
        /// Executes a job process and blocks until it completes.
        /// </summary>
        private void ThreadedRun()
        {
            try
            {
                Proc.Start();

                // The streams may only be read when they have been redirected (see the constructor)
                if (Redirected)
                {
                    Proc.BeginOutputReadLine();
                    Proc.BeginErrorReadLine();
                }

                // Wait for the process itself for as long as it takes. Git commands are
                // routinely slow (clone, gc, submodule update) and an interactive mergetool
                // runs for as long as the user needs, so we must never terminate one: killing
                // git mid-command leaves .git/index.lock behind and blocks every later command.
                //
                // Note the argument: the parameterless WaitForExit() would additionally block
                // until both pipes report end-of-stream, and a grandchild process holding the
                // inherited write handles can delay that indefinitely. Any value other than
                // Timeout.Infinite waits for the process only, which is what we want here.
                Proc.WaitForExit(int.MaxValue);

                // The process is gone; now collect whatever the readers still owe us, bounded
                if (Redirected)
                {
                    StdoutEof.WaitOne(DrainTimeout);
                    StderrEof.WaitOne(DrainTimeout);
                }

                Result.retcode = Proc.ExitCode;
            }
            catch (Exception ex)
            {
                Result.stderr += ex.Message;
            }
            finally
            {
                // Release the process handle on every path, including the exception one
                try
                {
                    Proc.Close();
                }
                catch (Exception) { }

                // Copy the delegate first: Terminate() may null the field from another thread
                // between the test and the invocation, which would fault on the GUI thread
                PCompleteDelegate complete = FComplete;

                // Call the completion function in the context of a GUI thread
                if (complete != null)
                    App.MainForm.BeginInvoke((MethodInvoker) (() => complete(Result)));
            }
        }

        /// <summary>
        /// Callback that handles process printing to stdout.
        /// Collect all strings into one stdout variable and call a custom handler.
        /// </summary>
        private void POutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)   // If the stream ended, signal it and ignore stdout
            {
                StdoutEof.Set();
                return;
            }

            // Track whether anything was received rather than testing the accumulated
            // text, so that a leading empty line is kept instead of being dropped
            if (anyStdout)
                Result.stdout += Environment.NewLine;
            Result.stdout += e.Data;
            anyStdout = true;

            PStdoutDelegate stdout = FStdout;   // Copy: Terminate() may null the field
            if (stdout != null)
                App.MainForm.BeginInvoke((MethodInvoker)(() => stdout(e.Data)));
        }

        /// <summary>
        /// Callback that handles process printing to stderr
        /// Collect all strings into one stderr variable and call a custom handler.
        /// </summary>
        private void PErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            // Only a null marks the end of the stream. An empty string is a genuine blank
            // line: treating it as the end used to signal completion early and drop the line.
            if (e.Data == null)
            {
                // Sometimes we receive more than one end-of-stream on the error stream
                // (example: when adding a new key with plink). Setting a manual-reset
                // event is idempotent, so repeated signals are harmless.
                StderrEof.Set();
            }
            else
            {
                if (anyStderr)
                    Result.stderr += Environment.NewLine;
                Result.stderr += e.Data;
                anyStderr = true;

                PStderrDelegate stderr = FStderr;   // Copy: Terminate() may null the field
                if (stderr != null)
                    App.MainForm.BeginInvoke((MethodInvoker)(() => stderr(e.Data)));
            }
        }
    }
}
