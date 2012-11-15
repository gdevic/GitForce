using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Class containing status of the files in a git repository.
    /// </summary>
    public class ClassStatus
    {
        /// <summary>
        /// Reference to class repo that this Status class wraps
        /// </summary>
        public ClassRepo Repo;

        /// <summary>
        /// Lookup dictionary to get the status code string (length of 2) from a file path relative to the root.
        /// </summary>
        private readonly Dictionary<string, string> XY = new Dictionary<string, string>();

        /// <summary>
        /// Supplemental file name addressed by the primary file name, both relative to the root.
        /// This is used to store the original file name of a renamed and copied files (codes 'R' and 'C')
        /// </summary>
        private readonly Dictionary<string, string> AltFile = new Dictionary<string, string>();

        /// <summary>
        /// Class constructor
        /// </summary>
        public ClassStatus(ClassRepo repo)
        {
            Repo = repo;
        }

        /// <summary>
        /// Constructor used only once at FormMain init to register
        /// the repo status refresh before any other refresh in the chain.
        /// </summary>
        public ClassStatus()
        {
            App.Refresh += Refresh;
        }

        /// <summary>
        /// Global status refresh function.
        /// Refresh status of the current repo.
        /// </summary>
        public static void Refresh()
        {
            ClassRepo repo = App.Repos.Current;
            if(repo!=null)
            {
                repo.Status = new ClassStatus(repo);
                repo.Status.Status();
            }
        }

        /// <summary>
        /// Reload status fields in the context of a status class
        /// </summary>
        private void Status()
        {
            XY.Clear();
            AltFile.Clear();
            ExecResult result = Repo.Run("status --porcelain -uall -z");
            if (!result.Success()) return;
            string[] response = result.stdout
                .Replace('/', Path.DirectorySeparatorChar)  // Correct the path slash on Windows
                .Split(("\0")
                           .ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < response.Length; i++)
            {
                string s = response[i].Substring(3);
                string code = response[i].Substring(0, 2);
                XY[s] = code;
                if ("RC".Contains(response[i][0]))
                {
                    i++;
                    AltFile[s] = response[i];
                }
            }
        }

        /// <summary>
        /// Returns a list of files stored in a class
        /// </summary>
        public List<string> GetFiles()
        {
            return XY.Keys.ToList();
        }

        /// <summary>
        /// Return true if a given file is part of the git file status database
        /// </summary>
        public bool IsMarked(string path)
        {
            return XY.ContainsKey(path);
        }

        /// <summary>
        /// Returns the git status "X" key code for a file
        /// </summary>
        public char Xcode(string file)
        {
            return XY.ContainsKey(file) ? XY[file][0] : ' ';
        }

        /// <summary>
        /// Returns the git status "Y" key code for a file
        /// </summary>
        public char Ycode(string file)
        {
            return XY.ContainsKey(file) ? XY[file][1] : ' ';
        }

        /// <summary>
        /// Returns the alternate file name associated with the given file
        /// </summary>
        public string GetAltFile(string key)
        {
            return AltFile.ContainsKey(key) ? AltFile[key] : string.Empty;
        }

        /// <summary>
        /// Display the info from the given tree node on the main application status pane.
        /// </summary>
        public void ShowTreeInfo(TreeNode tn)
        {
            // Translation of git status codes into useful human readable strings
            Dictionary<char, string> desc = new Dictionary<char, string> {
            { ' ', "OK" },
            { 'M', "Modified" },
            { 'A', "Added" },
            { 'D', "Deleted" },
            { 'R', "Renamed" },
            { 'C', "Copied" },
            { 'U', "Unmerged" },
            { '?', "Untracked" } };

            string status = "";
            if (tn != null)
            {
                string name = tn.Tag.ToString();
#if DEBUG
                status = String.Format("{0} Tag:'{1}': ", tn.Tag.GetType(), name);
#endif
                if (IsMarked(name))
                {
                    char xcode = Xcode(name);
                    char ycode = Ycode(name);
                    string x = "", y = "";
                    if (ycode != ' ')
                        y = desc[ycode];
                    if (xcode != ' ' && xcode != '?')
                        x = ((ycode!=' ')? ", " : "") + desc[xcode] + " in index";
                    status += name + ((x.Length>0 || y.Length>0) ? " ... <" + y + x + ">" : "");
                }
            }
            App.MainForm.SetStatusText(status);
        }
    }
}
