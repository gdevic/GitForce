using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace git4win
{
    public sealed class GitFileInfo
    {
        /// <summary>
        /// Short name of a file
        /// </summary>
        public string Name;

        /// <summary>
        /// Full path name to a file
        /// </summary>
        public string FullName;

        /// <summary>
        /// Class constructor
        /// </summary>
        public GitFileInfo(string fullName)
        {
            FullName = fullName;
            Name = Path.GetFileName(FullName);
        }
    }

    public sealed class GitDirectoryInfo
    {
        /// <summary>
        /// Sort the files returned by GetFiles() funtion by name or by their extension
        /// </summary>
        public enum SortBy { Name, Extension };

        /// <summary>
        /// Short name of a directory
        /// </summary>
        public string Name;

        /// <summary>
        /// Full path to the directory
        /// </summary>
        public string FullName;

        /// <summary>
        /// List of files and directories at this level
        /// </summary>
        public List<string> List;

        /// <summary>
        /// Class constructor
        /// </summary>
        public GitDirectoryInfo(string fullName, List<string> list)
        {
            FullName = fullName;
            Name = Path.GetFileName(FullName);
            List = list;
        }

        /// <summary>
        /// Returns a list of subdirectories of the current directory
        /// </summary>
        public GitDirectoryInfo[] GetDirectories()
        {
            // As the subdirectories are being added, keep them sorted
            SortedDictionary<string, GitDirectoryInfo> dict = new SortedDictionary<string, GitDirectoryInfo>();

            // For every node item in our list that represents a directory, add it
            // to the bucket where each represents one subdirectory node at that level
            foreach (string s in List)
            {
                // Pick only directories from the list and create a new directory node
                int index = s.IndexOf('\\');
                if (index > 0)
                {
                    string name = s.Substring(0, index);
                    string rest = s.Substring(index + 1);
                    string path = Path.Combine(FullName, name);

                    GitDirectoryInfo newDir = new GitDirectoryInfo(path, new List<string>());

                    if (dict.ContainsKey(name))
                        newDir = dict[name];

                    newDir.List.Add(rest);
                    dict[name] = newDir;
                }
            }

            // Return the array of values of a sorted dictionary:
            // values are GitDirectoryInfo structures
            return dict.Values.ToArray();
        }

        /// <summary>
        /// Return a list of files for the current directory
        /// </summary>
        public GitFileInfo[] GetFiles(SortBy sortBy)
        {
            List<GitFileInfo> files = (from s in List
                                       where s.IndexOf('\\') < 0
                                       select Path.Combine(FullName, s)
                                       into path select new GitFileInfo(path)).ToList();

            // Add all items in the list that dont have directory separator
            // Those are the files residing at this level

            // Sort the list by the name or extension of the files in it
            switch (sortBy)
            {
                case SortBy.Name:
                    files.Sort((f1, f2) => f1.Name.CompareTo(f2.Name));
                    break;

                case SortBy.Extension:
                    files.Sort((f1, f2) => Path.GetExtension(f1.Name).CompareTo(Path.GetExtension(f2.Name)));
                    break;
            }

            return files.ToArray();
        }
    }
}
