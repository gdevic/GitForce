using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win
{
    /// <summary>
    /// Class containing the status of a git repository. This class also contains
    /// other utility functions related to file views.
    /// </summary>
    public class ClassStatus
    {
        /// <summary>
        /// Lookup dictionary to get the status code string (length of 2) from a full file path.
        /// This dictionary contains keys that are absolute full file paths.
        /// </summary>
        private Dictionary<string, string> lookup = new Dictionary<string, string>();

        /// <summary>
        /// List of file nodes. This is an intermediate step when building code dictionary.
        /// List contains files relative to the Repo root.
        /// </summary>
        private List<string> list = new List<string>();

        /// <summary>
        /// Repo that this status class is bound with
        /// </summary>
        private ClassRepo Repo = null;

        /// <summary>
        /// Translation of git status codes into useful human readable strings
        /// </summary>
        private static Dictionary<char, string> desc = new Dictionary<char, string> {
            { ' ', "OK" },
            { 'M', "Modified" },
            { 'A', "Added" },
            { 'D', "Deleted" },
            { 'R', "Renamed" },
            { 'C', "Copied" },
            { 'U', "Unmerged" },
            { '?', "Untracked" }
        };

        /// <summary>
        /// Enumeration of icons for files in different stage
        /// </summary>
        public enum Img
        {
            FOLDER_CLOSED, FOLDER_OPENED, DATABASE_CLOSED, DATABASE_OPENED,
            FILE_UNMODIFIED,        // ID=5
            FILE_UNTRACKED,         // ID=6
            FILE_DELETED,           // ID=7
            FILE_ADDED,             // ID=8
            FILE_MODIFIED,          // ID=9
            FILE_COPIED,            // ID=10
            FILE_RENAMED,           // ID=11

            CHANGE_ALL,             // ID=12
            CHANGE_ONE,             // ID=13
        }

        /// <summary>
        /// Describes a mapping from a file status to the image associated with it
        /// </summary>
        private static Dictionary<char, Img> staticons = new Dictionary<char, Img> {
            { ' ', Img.FILE_UNMODIFIED },
            { '?', Img.FILE_UNTRACKED },
            { 'D', Img.FILE_DELETED },
            { 'A', Img.FILE_ADDED },
            { 'M', Img.FILE_MODIFIED },
            { 'C', Img.FILE_COPIED },
            { 'R', Img.FILE_RENAMED }
        };

        /// <summary>
        /// Sets the way files are sorted: by native file name or by extension
        /// </summary>
        public GitDirectoryInfo.SortBy sortBy = GitDirectoryInfo.SortBy.Name;

        /// <summary>
        /// Class constructor
        /// </summary>
        public ClassStatus(ClassRepo repo)
        {
            Repo = repo;
        }

        /// <summary>
        /// Returns the image list to be used with a tree view showing the files
        /// </summary>
        public static ImageList GetImageList()
        {
            ImageList il = new ImageList();
            il.ImageSize = new System.Drawing.Size(32, 16);
            il.ColorDepth = ColorDepth.Depth32Bit;

            il.Images.Add(Properties.Resources.TreeIcon1);
            il.Images.Add(Properties.Resources.TreeIcon2);
            il.Images.Add(Properties.Resources.TreeIcon3);
            il.Images.Add(Properties.Resources.TreeIcon4);
            il.Images.Add(Properties.Resources.TreeIcon5);
            il.Images.Add(Properties.Resources.TreeIcon6);
            il.Images.Add(Properties.Resources.TreeIcon7);
            il.Images.Add(Properties.Resources.TreeIcon8);
            il.Images.Add(Properties.Resources.TreeIcon9);
            il.Images.Add(Properties.Resources.TreeIcon10);
            il.Images.Add(Properties.Resources.TreeIcon11);

            il.Images.Add(Properties.Resources.Change0);
            il.Images.Add(Properties.Resources.Change1);

            return il;
        }

        /// <summary>
        /// Creates the class status list by running a git command.
        /// This is a first stage as the list needs to be "sealed".
        /// </summary>
        public void SetListByCommand(string cmd)
        {
            string[] response = Repo.Run(cmd)
                .Replace('/','\\')      // From now on we use Windows slash only
                .Split(("\0")
                .ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            list = new List<string>(response.Length);
            list.AddRange(response);
        }

        public delegate bool FilterDelegate(string s);

        /// <summary>
        /// Provides a filter function for the status list.
        /// Supply your custome delegate to decide which list strings to filter out.
        /// </summary>
        public void Filter(FilterDelegate d)
        {
            List<string> newList = list.Where(s => d(s) == false).ToList();
            list = newList;
        }

        /// <summary>
        /// Seals the status list into the dictionary. It assumes that the
        /// list contains git XY codes in the form "[XY] [file]"
        /// </summary>
        public void Seal()
        {
            lookup.Clear();
            list = list.ConvertAll(delegate(string s)
            {
                string XY = s.Substring(0, 2);
                string name = s.Substring(3);
                string fullPath = Path.Combine(Repo.root, name);
                lookup[fullPath] = XY;
                return name;
            });
        }

        /// <summary>
        /// Seals the status list into the dictionary. It assumes that the
        /// list contains git ls-tree format in the form "[attrib] [blob] [SHA]\t[file]"
        /// </summary>
        public void ConvertSeal()
        {
            lookup.Clear();
            list = list.ConvertAll(delegate(string s)
            {
                string name = s.Split('\t').Last();
                string fullPath = Path.Combine(Repo.root, name);
                lookup[fullPath] = "  ";
                return name;
            });
        }

        /// <summary>
        /// Return true if a given file is part of the git file status database
        /// </summary>
        /// <param name="path">Full absolute path to file</param>
        public bool isMarked(string path)
        {
            return lookup.ContainsKey(path);
        }

        /// <summary>
        /// Returns the 'X' code from the given file git status (index status)
        /// </summary>
        /// <param name="path">Full absolute path to file</param>
        public char GetX(string path)
        {
            return lookup[path][0];
        }

        /// <summary>
        /// Returns the 'Y' code from the given file git status (user status)
        /// </summary>
        /// <param name="path">Full absolute path to file</param>
        public char GetY(string path)
        {
            return lookup[path][1];
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
                if (isMarked(name))
                {
                    char X = GetX(name);
                    char Y = GetY(name);
                    string x = "", y = "";
                    if (Y != ' ')
                        y = desc[Y];
                    if (X != ' ' && X != '?')
                        x = ((Y!=' ')? ", " : "") + desc[X] + " in index";
                    status = name + ((x.Length>0 || y.Length>0) ? " ... <" + y + x + ">" : "");
                }
            }
            App.StatusInfo(status);
        }

        /// <summary>
        /// Traverse tree nodes and assign an icon corresponding to each file status.
        /// Since each file has 2 status codes, X and Y, for index with relation to the
        /// git cache, and for current file with relation to the index, isIndex
        /// specifies which code will be used.
        /// </summary>
        public void viewAssignIcon(TreeNode rootNode, bool isIndex)
        {
            TreeNodeCollection nodes = rootNode.Nodes;
            foreach (TreeNode tn in nodes)
            {
                string name = tn.Tag.ToString();
                if (isMarked(name))
                {
                    char icon = isIndex ? GetX(name) : GetY(name);
                    tn.ImageIndex = (int)staticons[icon];
                }
                else
                {
                    tn.ImageIndex = isIndex? (int)Img.CHANGE_ONE : (int)Img.FOLDER_CLOSED;
                }
                viewAssignIcon(tn, isIndex);
            }
        }

        /// <summary>
        /// Build a tree view of commit nodes using hints from repo commit groups
        /// </summary>
        public TreeNode BuildCommitsView()
        {
            Repo.commits.Rebuild(list);

            // Step 1: Build the default list with all files under this status class
            TreeNode root = new TreeNode("Pending Changelists");
            root.Tag = "root";                      // Tag of the root node

            foreach (var c in Repo.commits.bundle)
            {
                TreeNode commitNode = new TreeNode(c.description);
                commitNode.Tag = c;

                foreach (var f in c.files)
                {
                    TreeNode fNode = new TreeNode(f);
                    fNode.Tag = Path.Combine(Repo.root, f);
                    commitNode.Nodes.Add(fNode);
                }
                root.Nodes.Add(commitNode);
            }
            root.ExpandAll();
            return root;
        }

        /// <summary>
        /// Build a tree view or a list view of the list information
        /// </summary>
        public TreeNode BuildView(bool isTree=true)
        {
            TreeNode node = new TreeNode(Repo.root);
            node.Tag = Repo.root + @"\";
            if (isTree)
                BuildTreeRecurse(node, list);
            else
            {   
                // Files are already sorted by their native file name. Override the sort
                // with the sort by their extension only, if requested
                if (sortBy == GitDirectoryInfo.SortBy.Extension)
                    list.Sort((x, y) => Path.GetExtension(x).CompareTo(Path.GetExtension(y)));

                // Build a list view
                foreach (string s in list)
                {
                    TreeNode tn = new TreeNode(s);
                    tn.Tag = Path.Combine(Repo.root, s);
                    node.Nodes.Add(tn);
                }
            }
            return node;
        }

        /// <summary>
        /// Recursive function to traverse the virtual directory of
        /// a flat git response files and build a visual tree component.
        /// 
        /// Root tree node needs to have its Tag set to the full directory
        /// path of the root location upon which the git list is based. This
        /// string will be a default prefix to all nodes' Tags.
        /// </summary>
        /// <param name="tnRoot">Root node to base the tree on</param>
        /// <param name="list">List of files in git format</param>
        private void BuildTreeRecurse(TreeNode tnRoot, List<string> list)
        {
            string fullPath = tnRoot.Tag.ToString();
            GitDirectoryInfo dir = new GitDirectoryInfo(fullPath, list);
            GitDirectoryInfo[] dirs = dir.GetDirectories();

            foreach (GitDirectoryInfo d in dirs)
            {
                TreeNode tn = new TreeNode(d.Name);
                tn.Tag = d.FullName + @"\";
                tnRoot.Nodes.Add(tn);
                BuildTreeRecurse(tn, d.List);
            }

            GitFileInfo[] files = dir.GetFiles(sortBy);

            foreach (GitFileInfo file in files)
            {
                TreeNode tn = new TreeNode(file.Name);
                tn.Tag = file.FullName;
                tnRoot.Nodes.Add(tn);
            }
        }

        /// <summary>
        /// Build a list from traversing the local directory file system.
        /// The format of strings in the list mirrors the git status output, so
        /// the rest of the process can be the same.
        /// </summary>
        public void LoadLocalFiles()
        {
            list = GetFilesRecursive(Repo.root);
            list = list.ConvertAll(delegate(string s)
            {
                string name = s.Substring(Repo.root.Length + 1);
                string XY = isMarked(s) ? lookup[s] : "  ";
                return XY + " " + name;
            });
        }

        /// <summary>
        /// Recursively create a list of directories and files from the given path.
        /// </summary>
        private static List<string> GetFilesRecursive(string path)
        {
            List<string> result = new List<string>();
            Stack<string> stack = new Stack<string>();
            stack.Push(path);

            while (stack.Count > 0)
            {
                string dir = stack.Pop();
                try
                {
                    result.AddRange(Directory.GetFiles(dir, "*.*"));

                    foreach (string d in
                        Directory.GetDirectories(dir).Where(d => !d.EndsWith("\\.git") 
                        || Properties.Settings.Default.ShowDotGitFolders))
                    {
                        stack.Push(d);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return result;
        }
    }
}
