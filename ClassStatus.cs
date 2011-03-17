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
    /// Class containing the status of a git repository.
    /// </summary>
    public class ClassStatus
    {
        /// <summary>
        /// Reference to class repo that this Status class wraps
        /// </summary>
        public ClassRepo Repo;

        /// <summary>
        /// Lookup dictionary to get the status code string (length of 2) from a full file path.
        /// This dictionary contains keys that are absolute full file paths.
        /// </summary>
        private readonly Dictionary<string, string> _lookup = new Dictionary<string, string>();

        /// <summary>
        /// List of file nodes. This is an intermediate step when building code dictionary.
        /// List contains files relative to the Repo root.
        /// </summary>
        private List<string> _list = new List<string>();

        /// <summary>
        /// Translation of git status codes into useful human readable strings
        /// </summary>
        private readonly Dictionary<char, string> _desc = new Dictionary<char, string> {
            { ' ', "OK" },
            { 'M', "Modified" },
            { 'A', "Added" },
            { 'D', "Deleted" },
            { 'R', "Renamed" },
            { 'C', "Copied" },
            { 'U', "Unmerged" },
            { '?', "Untracked" }
        };

        public ClassStatus(ClassRepo repo)
        {
            Repo = repo;
        }

        /// <summary>
        /// Creates the class status list by running a git status command.
        /// This is a first stage as the list needs to be "sealed".
        /// </summary>
        public void SetListByStatusCommand(string cmd)
        {
            string[] response = Repo.Run(cmd)
                .Replace('/', Path.DirectorySeparatorChar)  // Correct the path slash on Windows
                .Split(("\0")
                .ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            _list = new List<string>(response.Length);
            _list.AddRange(response);

            // After being renamed, original file names are listed without any prefix,
            // immediately after the "R <new-file>" entries. Prune original names.
            _list = _list.FindAll(s => s.Length >= 3 && s[2] == ' ');
        }

        /// <summary>
        /// Creates the class status list by running a git ls-tree command.
        /// This is a first stage as the list needs to be "sealed".
        /// </summary>
        public void SetListByLsTreeCommand(string cmd)
        {
            string[] response = Repo.Run(cmd)
                .Replace('/', Path.DirectorySeparatorChar)  // Correct the path slash on Windows
                .Split(("\0")
                .ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            _list = new List<string>(response.Length);
            _list.AddRange(response);
        }

        /// <summary>
        /// Build the internal list using the given list of files.
        /// The format of strings in the list mirrors the git status output, so
        /// the rest of the process can be the same.
        /// </summary>
        public void SetListByList(List<string> list)
        {
            _list = list;
            _list = _list.ConvertAll(delegate(string s)
            {
                string name = s.Substring(Repo.Root.Length + 1);
                string xy = IsMarked(s) ? _lookup[s] : "  ";
                return xy + " " + name;
            });
        }

        public delegate bool FilterDelegate(string s);

        /// <summary>
        /// Provides a filter function for the status list.
        /// Supply your custome delegate to decide which list strings to filter out.
        /// </summary>
        public void Filter(FilterDelegate d)
        {
            List<string> newList = _list.Where(s => d(s) == false).ToList();
            _list = newList;
        }

        /// <summary>
        /// Seals the status list into the dictionary. It assumes that the
        /// list contains git XY codes in the form "[XY] [file]"
        /// </summary>
        public void Seal()
        {
            _lookup.Clear();
            _list = _list.ConvertAll(delegate(string s)
            {
                string xy = s.Substring(0, 2);
                string name = s.Substring(3);
                string fullPath = Path.Combine(Repo.Root, name);
                _lookup[fullPath] = xy;
                return name;
            });
        }

        /// <summary>
        /// Seals the status list into the dictionary. It assumes that the
        /// list contains git ls-tree format in the form "[attrib] [blob] [SHA]\t[file]"
        /// </summary>
        public void ConvertSeal()
        {
            _lookup.Clear();
            _list = _list.ConvertAll(delegate(string s)
            {
                string name = s.Split('\t').Last();
                string fullPath = Path.Combine(Repo.Root, name);
                _lookup[fullPath] = "  ";
                return name;
            });
        }

        /// <summary>
        /// Sort the files in the list
        /// </summary>
        public void Sort()
        {
            // Files are already sorted by their native file name. Override the sort
            // with the sort by their extension only, if requested
            if (Repo.SortBy == GitDirectoryInfo.SortBy.Extension)
                _list.Sort((x, y) => Path.GetExtension(x).CompareTo(Path.GetExtension(y)));
        }

        /// <summary>
        /// Return true if a given file is part of the git file status database
        /// </summary>
        /// <param name="path">Full absolute path to file</param>
        public bool IsMarked(string path)
        {
            return _lookup.ContainsKey(path);
        }

        /// <summary>
        /// Returns the 'X' code from the given file git status (index status)
        /// </summary>
        /// <param name="path">Full absolute path to file</param>
        public char GetXcode(string path)
        {
            return _lookup[path][0];
        }

        /// <summary>
        /// Returns the 'Y' code from the given file git status (user status)
        /// </summary>
        /// <param name="path">Full absolute path to file</param>
        public char GetYcode(string path)
        {
            return _lookup[path][1];
        }

        /// <summary>
        /// Return the list of files (unsealed)
        /// </summary>
        /// <returns></returns>
        public List<string> GetFileList()
        {
            return _list;
        }

        /// <summary>
        /// Display the info from the given tree node on the main application status pane.
        /// </summary>
        public void ShowTreeInfo(TreeNode tn)
        {
            string status = "";
            if (tn != null)
            {
                string name = tn.Tag.ToString();
                if (IsMarked(name))
                {
                    char xcode = GetXcode(name);
                    char ycode = GetYcode(name);
                    string x = "", y = "";
                    if (ycode != ' ')
                        y = _desc[ycode];
                    if (xcode != ' ' && xcode != '?')
                        x = ((ycode!=' ')? ", " : "") + _desc[xcode] + " in index";
                    status = name + ((x.Length>0 || y.Length>0) ? " ... <" + y + x + ">" : "");
                }
            }
            App.MainForm.SetStatusText(status);
        }
    }
}
