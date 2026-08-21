using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GitForce
{
    /// <summary>
    /// This class contains the code to load and save workspaces.
    /// It also implements a set of last recently used workspace files.
    /// </summary>
    static class ClassWorkspace
    {
        /// <summary>
        /// Return a set of last recently used workspace files as a list of absolute file names.
        /// </summary>
        public static List<string> GetLRU()
        {
            List<string> lru = Properties.Settings.Default.WorkspaceLRU.
                Split(("\t").ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries).ToList();
            return lru;
        }

        /// <summary>
        /// Set a list of recently used workspace files as a list of absolute file names.
        /// Calling this method will overwrite currently stored LRU list!
        /// </summary>
        public static void SetLRU(List<string> lru)
        {
            string s = string.Join("\t", lru.ToArray());
            Properties.Settings.Default.WorkspaceLRU = s;
        }

        /// <summary>
        /// Add a file name to the list of last recently used files.
        /// Place it at the top (LRU), even if it already exists.
        /// </summary>
        private static void AddLRU(string name)
        {
            List<string> lru = GetLRU();

            // If a name already exists, remove it first since we will be re-adding it at the top
            lru.Remove(name);       // Remove is safe to call even if the item is not in the list
            lru.Insert(0, name);    // Insert the new name to the top of LRU

            // Keep the number of recently used files down to a reasonable value
            if (lru.Count >= 6)
                lru.RemoveRange(5, lru.Count - 5);

            string s = string.Join("\t", lru.ToArray());
            Properties.Settings.Default.WorkspaceLRU = s;
        }

        /// <summary>
        /// Import workspace given the file name.
        /// If the file name is null, return false.
        /// </summary>
        public static bool Import(string name)
        {
            if (name == null)
                return false;

            App.PrintStatusMessage("Importing workspace: " + name, MessageType.General);
            if (App.Repos.Load(name, true))     // Merge loaded repos with the current set
            {
                AddLRU(name);
                return true;
            }
            App.PrintStatusMessage("Import cancelled. Current workspace file: " + Properties.Settings.Default.WorkspaceFile, MessageType.General);
            return false;
        }

        /// <summary>
        /// Load workspace given the workspace file name.
        /// </summary>
        public static bool Load(string name)
        {
            App.PrintStatusMessage("Loading workspace: " + name, MessageType.General);
            if (App.Repos.Load(name, false))    // Load operation (not merge)
            {
                AddLRU(name);
                Properties.Settings.Default.WorkspaceFile = name;
                return true;
            }
            App.PrintStatusMessage("Load cancelled. Current workspace file: " + Properties.Settings.Default.WorkspaceFile, MessageType.General);
            return false;
        }

        /// <summary>
        /// Save workspace given the file name.
        /// If the file name is null, save the current workspace.
        /// </summary>
        public static bool Save(string name)
        {
            if (name == null)
                name = Properties.Settings.Default.WorkspaceFile;

            App.PrintStatusMessage("Saving workspace: " + name, MessageType.General);
            if (App.Repos.Save(name))
            {
                AddLRU(name);
                Properties.Settings.Default.WorkspaceFile = name;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Helper function that clears current workspace
        /// </summary>
        public static void Clear()
        {
            // Drop the repos in one step, then reset the pointers that referenced them. Clearing
            // the list on its own would leave 'Current' pointing at a repo that is no longer in the
            // workspace, which the panels would keep showing and refreshing.
            // Do not delete the repos one by one through ClassRepos.Delete(): that re-elects a new
            // 'Current' after every removal, and each election runs a git command against a repo
            // which is about to be deleted anyway. Those commands also pump the message loop, so
            // queued input could run against a half-cleared workspace.
            App.Repos.Repos.Clear();
            App.Repos.Default = null;
            App.Repos.SetCurrent(null);     // With the list empty this simply clears 'Current'

            // Clearing a workspace also clears its project layout, so no empty project
            // folders are left behind in the repo tree view
            App.Repos.ProjectLayout = new ClassProjectLayout();

            App.Repos.InitAll();
            App.DoRefresh();
        }
    }
}
