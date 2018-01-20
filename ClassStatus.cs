using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
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
        /// Helper accessor to get the path to the MERGE_MSG file
        /// </summary>
        public string pathToMergeMsg {
            get { return Repo.Path + Path.DirectorySeparatorChar + ".git" + Path.DirectorySeparatorChar + "MERGE_MSG"; } }

        /// <summary>
        /// Class constructor
        /// </summary>
        public ClassStatus(ClassRepo repo)
        {
            Repo = repo;
            Status();
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
        /// Return true if the repo contains files that are not yet merged
        /// </summary>
        public bool IsUnmerged()
        {
            if (XY.Values.Any(status => status[0]=='U' || status[1]=='U'))
                return true;
            return false;
        }

        /// <summary>
        /// Return true if the repo is in the merge state
        /// </summary>
        public bool IsMergeState()
        {
            string checkFile = Repo.Path + Path.DirectorySeparatorChar + ".git" + Path.DirectorySeparatorChar + "MERGE_HEAD";
            return File.Exists(checkFile);
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
        /// Returns true if any of the files in any of the subfolders (any descendant)
        /// has any modification (addition, update, delete, rename, unmerged, ...)
        /// </summary>
        public bool HasModifiedDescendants(string key)
        {
            bool modified = false;
            foreach(KeyValuePair<string, string> entry in XY)
            {
                // skip non-descendants
                if (!entry.Key.StartsWith (key))
                    continue;

                if (entry.Value[0] != ' ' || entry.Value[1] != ' ') {
                    modified = true;
                }
            }

            return modified;
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
            { '?', "Untracked" },
            { '!', "Ignored" } };

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
