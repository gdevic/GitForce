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
    ///     safety built-in: self-terminate if there is no response within some time
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
        private Semaphore Exited = new Semaphore(0, 1);

        public Exec(string cmd, string args)
        {
            Proc = new Process {
                StartInfo =
                {
                    FileName = cmd,
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = System.Text.Encoding.UTF8,
                    WorkingDirectory = Directory.GetCurrentDirectory()
                }};

            Proc.OutputDataReceived += POutputDataReceived;
            Proc.ErrorDataReceived += PErrorDataReceived;

            // TODO: This is a hack for mergetool: We need to show the window to ask the user if the merge succeeded.
            // The problem is with .NET (and MONO!) buffering of streams prevents us to catching the question on time.
            if (args.StartsWith("mergetool "))
            {
                Proc.StartInfo.CreateNoWindow = false;
                Proc.StartInfo.RedirectStandardOutput = false;
                Proc.StartInfo.RedirectStandardError = false;
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
            job.Thread.Start(1000 * 60); // Give the job 60 sec to cleanly finish
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
            Thread.Start(1000 * 60); // Give the job 60 sec to cleanly finish
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
        private void ThreadedRun(object wait)
        {
            try
            {
                Proc.Start();
                Proc.BeginOutputReadLine();
                Proc.BeginErrorReadLine();

                if (Proc.WaitForExit((int)wait))
                    Proc.WaitForExit();

                // Wait for stdout and stderr signals to complete
                Exited.WaitOne();
                Result.retcode = Proc.ExitCode;
                Proc.Close();
            }
            catch (Exception ex)
            {
                Result.stderr += ex.Message;
            }
            finally
            {
                // Call the completion function in the context of a GUI thread
                if (FComplete != null)
                    App.MainForm.BeginInvoke((MethodInvoker) (() => FComplete(Result)));
            }
        }

        /// <summary>
        /// Callback that handles process printing to stdout.
        /// Collect all strings into one stdout variable and call a custom handler.
        /// </summary>
        private void POutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (String.IsNullOrEmpty(e.Data))   // If the stream ended, ignore stdout
                return;

            if (Result.stdout != string.Empty)
                Result.stdout += Environment.NewLine;
            Result.stdout += e.Data;

            if (FStdout != null)
                App.MainForm.BeginInvoke((MethodInvoker)(() => FStdout(e.Data)));
        }

        /// <summary>
        /// Callback that handles process printing to stderr
        /// Collect all strings into one stderr variable and call a custom handler.
        /// </summary>
        private void PErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (String.IsNullOrEmpty(e.Data))   // If the stream ended
            {
                // Sometimes we receive multiple null strings on error stream
                // (example: when adding a new key with plink)
                // This catches these cases which would increment semaphore over it's limit
                try
                {
                    Exited.Release();           // release its semaphore
                }
                catch (Exception ex)
                {
                    App.PrintLogMessage(ex.Message, MessageType.Error);
                }
            }
            else
            {
                if (Result.stderr != string.Empty)
                    Result.stderr += Environment.NewLine;
                Result.stderr += e.Data;

                if (FStderr != null)
                    App.MainForm.BeginInvoke((MethodInvoker)(() => FStderr(e.Data)));
            }
        }
    }
}
