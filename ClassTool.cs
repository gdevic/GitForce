using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace GitForce
{
    /// <summary>
    /// Class describing a custom tool.
    /// Also contains functions to Load, Save and Run custom tools.
    /// </summary>
    public class ClassTool : ICloneable
    {
        /// <summary>
        /// Short name of the tool
        /// </summary>
        public string Name;
        /// <summary>
        /// Full path and name of the tool executable
        /// </summary>
        public string Cmd;
        /// <summary>
        /// Arguments to pass to the tool before it executes. Macros allowed.
        /// </summary>
        public string Args;
        /// <summary>
        /// Starting directory for a tool
        /// </summary>
        public string Dir;
        /// <summary>
        /// Longer description of the tool
        /// </summary>
        public string Desc;
        /// <summary>
        /// Add a tool to the context menu
        /// </summary>
        public bool IsAddToContextMenu;
        /// <summary>
        /// Tool is a console app
        /// </summary>
        public bool IsConsoleApp;
        /// <summary>
        /// Capture and write the output of a console tool into the status pane
        /// </summary>
        public bool IsWriteOutput;
        /// <summary>
        /// After a console tool exits, close its host command window
        /// </summary>
        public bool IsCloseWindowOnExit;
        /// <summary>
        /// After a tool exits, do a GitForce global refresh
        /// </summary>
        public bool IsRefresh;
        /// <summary>
        /// Before running a tool, prompt the user for extra arguments
        /// </summary>
        public bool IsPromptForArgs;
        /// <summary>
        /// Within the extra arguments dialog, show the file browse button
        /// </summary>
        public bool IsAddBrowse;

        /// <summary>
        /// Implements the clonable interface.
        /// </summary>
        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// ToString override dumps all tool information for debug.
        /// </summary>
        public override string ToString()
        {
            return String.Format("Name: {0}\nCmd: {1}\nArgs: {2}\nDir: {3}\n",
                                 Name, Cmd, Args, Dir);
        }

        /// <summary>
        /// Runs a custom tool.
        /// Returns a string with a tool output to be printed out.
        /// This string can be empty, in which case nothing should be printed.
        /// </summary>
        public string Run(List<string> files)
        {
            App.PrintLogMessage(ToString(), MessageType.Command);

            string stdout = string.Empty;
            string args = DeMacroise(Args, files);

            // Add custom arguments if the checkbox to Prompt for Arguments was checked
            if (IsPromptForArgs)
            {
                // Description is used as a question for the arguments, shown in the window title bar
                string desc = Name;
                if (!string.IsNullOrEmpty(Desc)) desc += ": " + Desc;

                FormCustomToolArgs formCustomToolArgs = new FormCustomToolArgs(desc, args, IsAddBrowse);
                if (formCustomToolArgs.ShowDialog() == DialogResult.Cancel)
                    return string.Empty;

                args = formCustomToolArgs.GetArgs();
            }

            App.StatusBusy(true);

            // Prepare the process to be run
            Process proc = new Process();
            proc.StartInfo.FileName = "\"" + Cmd + "\"";
            proc.StartInfo.Arguments = args;
            proc.StartInfo.WorkingDirectory = DeMacroise(Dir, new List<string>());
            proc.StartInfo.UseShellExecute = false;

            try
            {
                // Run the custom tool in two ways (console app and GUI app)
                if (IsConsoleApp)
                {
                    // Start a console process
                    proc.StartInfo.CreateNoWindow = false;

                    // If we have to keep the window open (CMD/SHELL) after exit,
                    // we start the command line app in a different way, using a
                    // shell command (in which case we cannot redirect the stdout)
                    if (IsCloseWindowOnExit)
                    {
                        App.MainForm.SetTitle("Waiting for " + Cmd + " to finish...");

                        // Redirect standard output to our status pane if requested
                        if (IsWriteOutput)
                        {
                            proc.StartInfo.RedirectStandardOutput = true;
                            proc.StartInfo.RedirectStandardError = true;
                            proc.OutputDataReceived += ProcOutputDataReceived;
                            proc.ErrorDataReceived += ProcErrorDataReceived;
                            proc.Start();
                            proc.BeginOutputReadLine();
                            proc.WaitForExit();
                        }
                        else
                        {
                            proc.Start();
                            proc.WaitForExit();
                        }
                    }
                    else
                    {
                        // We need to keep the CMD/SHELL window open, so start the process using
                        // the CMD/SHELL as the root process and pass it our command to execute
                        proc.StartInfo.Arguments = string.Format("{0} {1} {2}",
                            ClassUtils.GetShellExecFlags(), proc.StartInfo.FileName, proc.StartInfo.Arguments);
                        proc.StartInfo.FileName = ClassUtils.GetShellExecCmd();
                        App.PrintLogMessage(proc.StartInfo.Arguments, MessageType.Command);

                        proc.Start();
                    }
                }
                else
                {
                    // Start a GUI process
                    proc.StartInfo.CreateNoWindow = true;

                    // We can start the process and wait for it to finish only if we need to
                    // refresh the app after the process has exited.
                    proc.Start();
                }

                if (IsRefresh)
                {
                    App.MainForm.SetTitle("Waiting for " + Cmd + " to finish...");

                    proc.WaitForExit();

                    App.DoRefresh();
                }
            }
            catch (Exception ex)
            {
                App.PrintStatusMessage(ex.Message, MessageType.Error);
                MessageBox.Show(ex.Message, "Error executing custom tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            proc.Close();
            App.StatusBusy(false);

            return stdout;
        }

        /// <summary>
        /// Callback that handles process printing to stdout
        /// Print to the application status pane.
        /// </summary>
        private void ProcOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (String.IsNullOrEmpty(e.Data)) return;
            App.PrintStatusMessage(e.Data + Environment.NewLine, MessageType.General);
        }

        /// <summary>
        /// Callback that handles process printing to stderr
        /// Print to the application status pane.
        /// </summary>
        private void ProcErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (String.IsNullOrEmpty(e.Data)) return;
            App.PrintStatusMessage(e.Data + Environment.NewLine, MessageType.Error);
        }

        /// <summary>
        /// Applies a set of macro resolutions to the input string
        /// </summary>
        private string DeMacroise(string s, List<string> files)
        {
            // Without the current repo, we cannot have reasonable macro expansions
            s = s.Replace("%r", App.Repos.Current==null ? "" : App.Repos.Current.Root);
            s = s.Replace("%u", App.Repos.Current==null ? "" : App.Repos.Current.UserName);
            s = s.Replace("%e", App.Repos.Current==null ? "" : App.Repos.Current.UserEmail);
            s = s.Replace("%b", App.Repos.Current==null ? "" : App.Repos.Current.Branches.Current);

            // Separate given list into list of files and list of directories
            List<string> F = new List<string>();
            List<string> D = new List<string>();

            foreach (string f in files)
            {
                if (Directory.Exists(f))
                    // For directories, remove trailing slash
                    D.Add(f.TrimEnd(new[] {'\\', '/'}));
                else
                    F.Add(f);
            }

            // Single file and single directory
            string sf = F.Count > 0 ? F[0] : "";
            string sd = D.Count > 0 ? D[0] : "";

            s = s.Replace("%f", sf);
            s = s.Replace("%d", sd);
            s = s.Replace("%F", string.Join(" ", F.ToArray()));
            s = s.Replace("%D", string.Join(" ", D.ToArray()));

            return s;
        }
    }

    /// <summary>
    /// Class describing our set of custom tools
    /// </summary>
    public class ClassCustomTools
    {
        /// <summary>
        /// List of tools
        /// </summary>
        public readonly List<ClassTool> Tools = new List<ClassTool>();

        /// <summary>
        /// Load a set of tools from a given file into the current tool-set.
        /// Returns a new class structure containing all the tools if the tools loaded correctly.
        /// If load failed, return empty class and print the error message to a main pane.
        /// </summary>
        public static ClassCustomTools Load(string name)
        {
            App.PrintStatusMessage("Loading custom tools: " + name, MessageType.General);
            ClassCustomTools ct = new ClassCustomTools();
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(ClassCustomTools));
                using (TextReader textReader = new StreamReader(name))
                {
                    ct = (ClassCustomTools)deserializer.Deserialize(textReader);                    
                }
            }
            catch (Exception ex)
            {
                // It is OK not to find custom tools file (for example, the app is being run the very first time)
                // All other errors are being reported
                if (!(ex is FileNotFoundException))
                    App.PrintStatusMessage("Error loading custom tools: " + ex.Message, MessageType.Error);
            }
            return ct;
        }

        /// <summary>
        /// Save current set of tools to a given file.
        /// Returns true if save successful.
        /// If save failed, return false and print the error message to a main pane.
        /// </summary>
        public bool Save(string name)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ClassCustomTools));
                using (TextWriter textWriter = new StreamWriter(name))
                {
                    serializer.Serialize(textWriter, this);
                }
            }
            catch (Exception ex)
            {
                App.PrintStatusMessage("Error saving custom tools: " + ex.Message, MessageType.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Implements a deep copy of the whole class.
        /// </summary>
        public ClassCustomTools Copy()
        {
            ClassCustomTools ct = new ClassCustomTools();
            foreach (var classTool in Tools)
                ct.Tools.Add((ClassTool)classTool.Clone());
            return ct;
        }

        #region Utility function to find local tools

        /// <summary>
        /// Utility function to find few local tools: experimental
        /// </summary>
        public static List<ClassTool> FindLocalTools()
        {
            List<ClassTool> tools = new List<ClassTool>();

            try
            {
                // If we have msysgit, we should also have few other bundled tools
                string gitpath = Properties.Settings.Default.GitPath;
                if (File.Exists(gitpath))
                {
                    string gitRoot = Path.GetDirectoryName(gitpath);

                    //      ======= Find and add Git Bash =======
                    string GitBash = Path.Combine(gitRoot, "sh.exe");
                    if (File.Exists(GitBash))
                    {
                        ClassTool newTool = new ClassTool();
                        newTool.Name = "Git Bash";
                        newTool.Cmd = GitBash;
                        newTool.Dir = "%r";
                        newTool.Args = "--login -i";
                        newTool.IsConsoleApp = true;
                        newTool.IsAddToContextMenu = true;
                        tools.Add(newTool);
                    }
                }
            }
            catch { }   // Never mind.

            return tools;
        }

        #endregion
    }
}