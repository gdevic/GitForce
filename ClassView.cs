using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GitForce.Properties;

namespace GitForce
{
    /// <summary>
    /// This class contains support functions and data definitions
    /// for the various views within forms.
    /// </summary>
    public static class ClassView
    {
        /// <summary>
        /// Enumeration of icons for files at different stage
        /// </summary>
        public enum Img
        {
            FileUnmodified,
            FileModified,
            FileAdded,
            FileDeleted,
            FileRenamed,
            FileCopied,
            FileUnmerged,
            FileUntracked,

            FolderClosed,
            FolderOpened,
            DatabaseClosed,
            DatabaseOpened,
            ChangelistRoot,
            Changelist,
        }

        /// <summary>
        /// Describes a mapping from image number into the resource icon
        /// </summary>
        private static readonly Dictionary<Img, Icon> Res = new Dictionary<Img, Icon> {
            { Img.FileUnmodified,   Resources.TreeIconU },
            { Img.FileModified,     Resources.TreeIconM },
            { Img.FileAdded,        Resources.TreeIconA },
            { Img.FileDeleted,      Resources.TreeIconD },
            { Img.FileRenamed,      Resources.TreeIconR },
            { Img.FileCopied,       Resources.TreeIconC },
            { Img.FileUnmerged,     Resources.TreeIconX },
            { Img.FileUntracked,    Resources.TreeIconQ },

            { Img.FolderClosed,     Resources.TreeIconFC },
            { Img.FolderOpened,     Resources.TreeIconFO },
            { Img.DatabaseClosed,   Resources.TreeIconDB },
            { Img.DatabaseOpened,   Resources.TreeIconDB },
            { Img.ChangelistRoot,   Resources.Change0 },
            { Img.Changelist,       Resources.Change1 },
        };

        /// <summary>
        /// Describes a mapping from a git file status code to the image associated with it
        /// </summary>
        private static readonly Dictionary<char, Img> Staticons = new Dictionary<char, Img> {
            { ' ', Img.FileUnmodified },
            { 'M', Img.FileModified },
            { 'A', Img.FileAdded },
            { 'D', Img.FileDeleted },
            { 'R', Img.FileRenamed },
            { 'C', Img.FileCopied },
            { 'U', Img.FileUnmerged },
            { '?', Img.FileUntracked },
        };

        /// <summary>
        /// Returns the image list to be used with a tree view showing the files
        /// </summary>
        public static ImageList GetImageList()
        {
            ImageList il = new ImageList();
            il.ImageSize = new Size(32, 16);
            il.ColorDepth = ColorDepth.Depth32Bit;
            foreach (var kvp in Res)
                il.Images.Add(kvp.Value);

            return il;
        }

        /// <summary>
        /// Traverse tree nodes and assign an icon corresponding to each file status.
        /// Since each file has 2 status codes, X and Y, for index with relation to the
        /// git cache, and for current file with relation to the index, isIndex
        /// specifies which code will be used.
        /// </summary>
        public static void ViewAssignIcon(ClassStatus status, TreeNode rootNode, bool isIndex)
        {
            TreeNodeCollection nodes = rootNode.Nodes;
            foreach (TreeNode tn in nodes)
            {
                if (tn.Tag is ClassCommit)
                    tn.ImageIndex = (int)Img.Changelist;
                else
                {
                    string name = isIndex ? tn.Text : tn.Tag.ToString();
                    if (status.IsMarked(name) || tn.Nodes.Count == 0)
                    {
                        char icon = isIndex ? status.Xcode(name) : status.Ycode(name);
                        tn.ImageIndex = (int)Staticons[icon];
                    }
                    else
                        tn.ImageIndex = (int)Img.FolderClosed;
                }
                ViewAssignIcon(status, tn, isIndex);
            }
        }

        /// <summary>
        /// Build a tree view given the list of files
        /// </summary>
        /// <param name="tnRoot">Root node to base the tree on</param>
        /// <param name="list">List of files in git format</param>
        /// <param name="sortBy">File sorting order</param>
        public static void BuildTree(TreeNode tnRoot, List<string> list, GitDirectoryInfo.SortBy sortBy)
        {
            BuildTreeRecurse(tnRoot, list, sortBy);
        }

        /// <summary>
        /// Recursive function to traverse the virtual directory of
        /// a flat git response files and build a visual tree component.
        /// </summary>
        /// <param name="tnRoot">Root node to base the tree on</param>
        /// <param name="list">List of files in git format</param>
        /// <param name="sortBy">File sorting order</param>
        private static void BuildTreeRecurse(TreeNode tnRoot, List<string> list, GitDirectoryInfo.SortBy sortBy)
        {
            string rootPath = tnRoot.Tag.ToString();
            GitDirectoryInfo dir = new GitDirectoryInfo(rootPath, list);
            GitDirectoryInfo[] dirs = dir.GetDirectories();

            foreach (GitDirectoryInfo d in dirs)
            {
                TreeNode tn = new TreeNode(d.Name);
                tn.Tag = d.FullName + Path.DirectorySeparatorChar;
                tnRoot.Nodes.Add(tn);
                BuildTreeRecurse(tn, d.List, sortBy);
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
        /// Builds a flat list of tree nodes based on a given list of files
        /// </summary>
        /// <param name="tnRoot">Root node to build the list on</param>
        /// <param name="list">List of relative path file names</param>
        public static void BuildFileList(TreeNode tnRoot, List<string> list, GitDirectoryInfo.SortBy sortBy)
        {
            // Sort the list according to the sorting rule
            if (sortBy == GitDirectoryInfo.SortBy.Name)
                list.Sort();
            else
            {
                var vSort = from s in list
                            orderby Path.GetExtension(s), s ascending
                            select s;
                list = vSort.ToList();
            }

            // Build a list view);
            foreach (string s in list)
            {
                TreeNode tn = new TreeNode(s);
                tn.Tag = s;
                tnRoot.Nodes.Add(tn);
            }
        }

        /// <summary>
        /// Build a tree view of commit nodes using hints from repo commit groups.
        /// Commit nodes have Tags containing absolute paths to files,
        /// while the relative paths (with respect to repo root) are simply in the Text.
        /// </summary>
        public static TreeNode BuildCommitsView(ClassRepo repo, List<string> list)
        {
            repo.Commits.Rebuild(list);

            // Step 1: Build the default list with all files under this status class
            TreeNode root = new TreeNode("Pending Changelists");
            root.Tag = "root";                      // Tag of the root node

            foreach (var c in repo.Commits.Bundle)
            {
                var commitNode = new TreeNode(GetCommitNodeText(c)) { Tag = c };

                if (c.IsCollapsed) commitNode.Collapse();

                foreach (var f in c.Files)
                {
                    var tn = new TreeNode(f) { Tag = f };
                    commitNode.Nodes.Add(tn);
                }

                root.Nodes.Add(commitNode);
            }
            root.ExpandAll();
            return root;
        }

        public static string GetCommitNodeText(ClassCommit commit)
        {
            if (commit == null) return string.Empty;
            var rerult = commit.DescriptionTitle;
            if (commit.Files.Count > 0)
            {
                rerult += " (" + commit.Files.Count + ")";
            }
            return rerult;
        }
    }
}