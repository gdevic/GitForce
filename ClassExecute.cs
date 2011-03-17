using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

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
        }

        public delegate void PCompleteDelegate();

        public struct ThreadedParameters
        {
            public string Cmd;
            public string Args;
            public DataReceivedEventHandler F0;
            public DataReceivedEventHandler F1;
            public PCompleteDelegate FComplete;
        }

        private static Process proc;

        public static void RunThreaded(object argument)
        {
            ThreadedParameters p = (ThreadedParameters) argument;

            proc = new Process
            {
                StartInfo =
                {
                    FileName = p.Cmd,
                    Arguments = p.Args,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
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
            proc.Close();

            p.FComplete();
        }

        public static void TerminateThreaded()
        {
            proc.Kill();
            proc.WaitForExit(1000);
            proc.Close();
        }

        /// <summary>
        /// Execute a command
        /// </summary>
        public static string Run(string cmd, string arg)
        {
            string output = "";
            ClassUtils.ClearLastError();

            try
            {
                App.StatusBusy(true);
                App.Log.Print(String.Format("$ {0} {1}", cmd, arg));

                Process p = new Process
                {
                    StartInfo =
                    {
                        FileName = cmd,
                        Arguments = arg,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };

                // TODO: This is a hack for mergetool: We need to show the window to ask the user if the merge succeeded.
                // The problem is with .NET (and MONO!) buffering of streams prevents us to catching the question on time.
                if (arg.StartsWith("mergetool "))
                {
                    p.StartInfo.CreateNoWindow = false;
                    p.StartInfo.RedirectStandardOutput = false;
                    p.StartInfo.RedirectStandardError = false;
                }

                // Add all environment variables listed
                foreach (var variable in Env)
                {
                    // A variable with that name might already be there, so update it
                    if(p.StartInfo.EnvironmentVariables.ContainsKey(variable.Key))
                        p.StartInfo.EnvironmentVariables[variable.Key] = variable.Value;
                    else
                        p.StartInfo.EnvironmentVariables.Add(variable.Key, variable.Value);
                }

                p.Start();

                output = p.StandardOutput.ReadToEnd();
                ClassUtils.LastError = p.StandardError.ReadToEnd();

                p.WaitForExit();
                p.Close();

                if (output != "")
                    App.Log.Print(output);
            }
            catch (Exception ex)
            {
                ClassUtils.LastError = ex.Message;
            }
            finally
            {
                App.StatusBusy(false);
            }

            return output;
        }
    }
}
