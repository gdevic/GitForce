﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GitForce
{
    /// <summary>
    /// This class contains the code to load and save workspaces.
    /// It also implements a set of last recently used workspace files.
    /// </summary>
    static class ClassWorkspace
    {
        /// <summary>
        /// Return a set of last recently used workspace files
        /// as a list of absolute file names.
        /// </summary>
        public static List<string> GetLRU()
        {
            List<string> lru = Properties.Settings.Default.WorkspaceLRU.
                Split(("\t").ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries).ToList();
            return lru;
        }

        /// <summary>
        /// Add a file name to the list of last recently used files.
        /// If a file is already in the set, it will be ignored.
        /// </summary>
        private static void AddLRU(string name)
        {
            List<string> lru = GetLRU();

            // Add a name only if it already does not exist
            if (!lru.Contains(name))
            {
                lru.Insert(0, name);

                // Keep the number of recently used files down to a reasonable number
                if (lru.Count >= 6)
                    lru.RemoveRange(5, lru.Count - 5);

                string s = string.Join("\t", lru.ToArray());
                Properties.Settings.Default.WorkspaceLRU = s;                
            }
        }

        /// <summary>
        /// Load workspace given the file name.
        /// If the file name is null, load default workspace.
        /// </summary>
        public static bool Load(string name)
        {
            // Initialize workspace file name, if this is a first run
            if (string.IsNullOrEmpty(Properties.Settings.Default.WorkspaceFile))
                Properties.Settings.Default.WorkspaceFile = Path.Combine(App.AppHome, "repos.giw");

            if (name == null)
                name = Properties.Settings.Default.WorkspaceFile;

            App.PrintStatusMessage("Loading workspace: " + name, MessageType.General);
            if (App.Repos.Load(name))
            {
                AddLRU(name);
                Properties.Settings.Default.WorkspaceFile = name;
                return true;
            }
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
            App.Repos.Repos.Clear();
            App.Repos.Refresh();
            App.DoRefresh();
        }
    }
}