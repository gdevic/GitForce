using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitForce
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
        public readonly List<ClassCommit> Bundle = new List<ClassCommit>();

        /// <summary>
        /// Constructor for list of commits - always create at least one (empty) commit named "Default"
        /// </summary>
        public ClassCommits()
        {
            Bundle.Add(new ClassCommit("Default"));
            Bundle[0].IsDefault = true;
        }

        /// <summary>
        /// Add a new commit bundle to the list
        /// </summary>
        public void NewBundle(string description, List<string> files)
        {
            // Remove all files listed from every bundle
            foreach (var c in Bundle)
                c.Prune(files);

            // Add a new bundle with these files
            ClassCommit commit = new ClassCommit(description);
            commit.AddFiles(files);
            Bundle.Add(commit);
        }

        /// <summary>
        /// Move or add each file in the list of files to a specified bundle.
        /// Files in that list that are present in any other bundle will be moved.
        /// </summary>
        public void MoveOrAdd(ClassCommit bundle, List<string> files)
        {
            // Remove all listed files from any bundle in which they might appear
            foreach (var c in Bundle)
                c.Prune(files);

            // Add listed files to a named bundle
            bundle.AddFiles(files);
        }

        /// <summary>
        /// Rebuild the list of files by using only the files from the given list
        /// </summary>
        /// <param name="files"></param>
        public void Rebuild(List<string> files)
        {
            files = Bundle.Aggregate(files, (current, c) => c.Renew(current));

            // Assign the remaining files to the first commit ("Default")
            Bundle[0].AddFiles(files);
        }
    }
}
