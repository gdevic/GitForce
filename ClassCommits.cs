using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace git4win
{
    /// <summary>
    /// Class describing and working on a set of commits.
    /// </summary>
    [Serializable]
    public class ClassCommits
    {
        /// <summary>
        /// List of commits
        /// </summary>
        public List<ClassCommit> bundle = new List<ClassCommit>();

        /// <summary>
        /// Constructor for list of commits - always create at least one (empty) commit named "Default"
        /// </summary>
        public ClassCommits()
        {
            bundle.Add(new ClassCommit("Default"));
            bundle[0].isDefault = true;
        }

        /// <summary>
        /// Add a new commit bundle to the list
        /// </summary>
        public void NewBundle(string description, List<string> files)
        {
            // Remove all files listed from every bundle
            foreach (var c in bundle)
                c.Prune(files);

            // Add a new bundle with these files
            ClassCommit commit = new ClassCommit(description);
            commit.AddFiles(files);
            bundle.Add(commit);
        }

        /// <summary>
        /// Rebuild the list of files by using only the files from the given list
        /// </summary>
        /// <param name="files"></param>
        public void Rebuild(List<string> files)
        {
            files = bundle.Aggregate(files, (current, c) => c.Renew(current));

            // Assign the remaining files to the first commit ("Default")
            bundle[0].AddFiles(files);
        }
    }
}
