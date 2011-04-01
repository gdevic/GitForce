using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitForce
{
    /// <summary>
    /// Class describing and working on one commit
    /// A commit is a set of files grouped together under one node
    /// </summary>
    [Serializable]
    public class ClassCommit
    {
        /// <summary>
        /// List of git files stored under this commit.
        /// Files have a path relative to the repo root.
        /// </summary>
        public List<string> Files = new List<string>();

        /// <summary>
        /// User description text of a commit
        /// </summary>
        public string Description;

        /// <summary>
        /// Is this commit a default one (not a user added)
        /// Default commit cannot be deleted.
        /// </summary>
        public bool IsDefault;

        /// <summary>
        /// Create a commit with the given description
        /// </summary>
        public ClassCommit(string desc)
        {
            Description = desc;
        }

        /// <summary>
        /// ToString override returns the commit description
        /// </summary>
        public override string ToString()
        {
            return Description;
        }

        /// <summary>
        /// Add a set of files to the commit list.
        /// Do not create any duplicates!
        /// </summary>
        public void AddFiles(List<string> newFiles)
        {
            Files = Files.Union(newFiles).ToList();
        }

        /// <summary>
        /// Remove all files listed from our list of files.
        /// Any file on that list may or may not appear on this commit list.
        /// </summary>
        public void Prune(List<string> outlaws)
        {
            Files = Files.Except(outlaws).ToList();
        }

        /// <summary>
        /// Renew the existing list of files by keeping only those that exist in the
        /// given list. Return the given list trimmed by files which are now "taken".
        /// </summary>
        public List<string> Renew(List<string> allFiles)
        {
            Files = Files.Intersect(allFiles).ToList();
            return allFiles.Except(Files).ToList();
        }
    }
}
