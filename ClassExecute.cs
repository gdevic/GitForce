using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Contains functions to execute a native OS command
    /// </summary>
    public static class ClassExecute
    {
        /// <summary>
        /// Set of environment variables used by the Run() method
        /// </summary>
        private static readonly Dictionary<string, string> Env = new Dictionary<string, string>();

        /// <summary>
        /// Delegate for the completion function
        /// </summary>
        public delegate void PCompleteDelegate(object exitCode);

        /// <summary>
        /// Structure holding a set of parameters for the threaded execution
        /// </summary>
        public struct ThreadedParameters
        {
            public string Cmd;
            public string Args;
            public DataReceivedEventHandler F0;
            public DataReceivedEventHandler F1;
            public PCompleteDelegate FComplete;
        }

        /// <summary>
        /// Process handle
        /// </summary>
        private static Process proc;

        /// <summary>
        /// Adds an environment variable for the Run method
        /// </summary>
        public static void AddEnvar(string name, string value)
        {
            if (Env.ContainsKey(name))
                Env[name] = value;
            else
                Env.Add(name, value);
        }

        /// <summary>
        /// Stops any executing thread job, if any.
        /// </summary>
        public static void KillJob()
        {
            // TODO: Need to implement this
        }

        /// <summary>
        /// Executes a native command using asynchronous standard streams
        /// </summary>
        public static void RunNativeProcess(object argument)
        {
            ThreadedParameters p = (ThreadedParameters) argument;

            ProcessStartInfo startInfo = new ProcessStartInfo {
                FileName = p.Cmd,
                Arguments = p.Args,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            // TODO: This is a hack for mergetool: We need to show the window to ask the user if the merge succeeded.
            // The problem is with .NET (and MONO!) buffering of streams prevents us to catching the question on time.
            if (p.Args.StartsWith("mergetool "))
            {
                startInfo.CreateNoWindow = false;
                startInfo.RedirectStandardOutput = false;
                startInfo.RedirectStandardError = false;
            }

            proc = new Process();
            proc.StartInfo = startInfo;
            proc.OutputDataReceived += p.F0;
            proc.ErrorDataReceived += p.F1;

            // Add all environment variables listed
            foreach (var variable in Env)
            {
                // A variable with that name might already be there, so update it
                if (proc.StartInfo.EnvironmentVariables.ContainsKey(variable.Key))
                    proc.StartInfo.EnvironmentVariables[variable.Key] = variable.Value;
                else
                    proc.StartInfo.EnvironmentVariables.Add(variable.Key, variable.Value);
            }

            proc.Start();

            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();

            proc.WaitForExit();
            p.FComplete(proc.ExitCode);
            proc.Close();
        }

        /// <summary>
        /// Terminate this class threaded command
        /// </summary>
        public static void TerminateThreaded()
        {
            proc.Kill();
            proc.WaitForExit(1000);
            proc.Close();
        }

        private static string stdout;
        private static string stderr;

        /// <summary>
        /// Executes a command
        /// </summary>
        public static string Run(string cmd, string args)
        {
            App.StatusBusy(true);
            App.Log.Print(String.Format("$ {0} {1}", cmd, args));

            ClassUtils.ClearLastError();
            stdout = stderr = string.Empty;

            // Create and start an execution thread with various
            // callbacks for stdout, stderr and command completion
            ThreadedParameters parameters;
            parameters.Cmd = cmd;
            parameters.Args = args;
            parameters.F0 = POutputDataReceived;
            parameters.F1 = PErrorDataReceived;
            parameters.FComplete = PComplete;

            Thread thRun = new Thread(RunNativeProcess);
            thRun.Start(parameters);
            thRun.Join();

            App.StatusBusy(false);

            return stdout;
        }

        /// <summary>
        /// Callback that handles process printing to stdout.
        /// Collect all strings into one stdout variable.
        /// </summary>
        private static void POutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (String.IsNullOrEmpty(e.Data)) return;
            stdout += e.Data;
        }

        /// <summary>
        /// Callback that handles process printing to stderr
        /// Print to the application status pane.
        /// </summary>
        private static void PErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (String.IsNullOrEmpty(e.Data)) return;
            if (App.MainForm.InvokeRequired)
                App.MainForm.BeginInvoke((MethodInvoker)(() => PErrorDataReceived(sender, e)));
            else
            {
                App.PrintStatusMessage(e.Data);
                stderr += e.Data;
            }
        }

        /// <summary>
        /// Callback that handles process completion event
        /// </summary>
        private static void PComplete(object exitCode)
        {
            if ((int)exitCode != 0)
                ClassUtils.LastError = "Error: Exit code=" + (int)exitCode;
        }
    }
}
