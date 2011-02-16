using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace git4win
{
    /// <summary>
    /// Manage various diff programs and diff execution
    ///
    /// Common diff utilities:
    /// 
    /// Perforce merge and diff tool :  C:\Program Files (x86)\Perforce\P4Merge.exe
    /// WinMerge :                      C:\Program Files (x86)\WinMerge\WinMergeU.exe
    /// Beyond Compare diff tool :      C:\Program Files (x86)\Beyond Compare 3\BComp.exe
    /// KDiff3 :                        C:\Program Files (x86)\KDiff3\kdiff3.exe
    /// </summary>
    public class ClassDiff
    {
        /// <summary>
        /// Structure describing a diff utility
        /// </summary>
        public struct Diff
        {
            /// <summary>
            /// Git configuration name for diff tool
            /// </summary>
            public string Difftool;

            /// <summary>
            /// User-friendly name of the diff utility
            /// </summary>
            public string Name;

            /// <summary>
            /// Full path/name to the diff utility
            /// </summary>
            public string Path;

            /// <summary>
            /// Arguments needed to execute a 2-file diff
            /// </summary>
            public string Args;

            public Diff(string diffTool, string name, string path, string args)
            {
                Difftool = diffTool;
                Name = name;
                Path = path;
                Args = args;
            }
        }

        /// <summary>
        /// List of diff programs recognized by the application
        /// </summary>
        public List<Diff> Diffs;
        
        /// <summary>
        /// Init code to be called on the application startup.
        /// </summary>
        public bool Initialize()
        {
            // Find selected diff programs installed and add them to the list
            Diffs = FindKnownDiffProgs();

            // If none of preset diff apps are present, extract our internal one
            if (Diffs.Count == 0)
                Diffs.Add(GetInternalDiff());

            // Assign the active diff utility or the first one on the list
            string activeName = Properties.Settings.Default.DiffActiveName;
            if (string.IsNullOrWhiteSpace(activeName))
                activeName = Diffs[0].Name;

            Diff active = Diffs[0];
            foreach (var d in Diffs.Where(d => d.Name == activeName))
                active = d;

            Properties.Settings.Default.DiffActiveName = active.Name;

            // Lastly, configure the git by setting all optional diffs and the active one
            return Configure(Diffs, active);
        }

        /// <summary>
        /// Configure git's external diff application by adding all tools from the given
        /// list into git's difftool sections and picking the active as the current app to use
        /// when doing the visual diff.
        /// Returns false if the list is empty, making no changes to the git config.
        /// </summary>
        public static bool Configure(List<Diff> diffs, Diff active)
        {
            // Set up global git config with each given diff program
            if (diffs.Count > 0)
            {
                foreach (var d in diffs)
                {
                    // All all diff tools to git global config as sections we can select from
                    string path = d.Path.Replace('\\', '/');
                    string usr = d.Args.Replace("%1", "$LOCAL").Replace("%2", "$REMOTE");
                    string arg = "'" + path + "' " + usr;
                    ClassConfig.Set("difftool." + d.Difftool + ".cmd", arg);
                }

                // Make sure the prompt will be off for visual diff
                ClassConfig.Set("difftool.prompt", "false");

                // Set the active diff tool
                ClassConfig.Set("diff.tool", active.Difftool);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Searches for the known diff programs and returns a list of those
        /// installed on the system. The list might be empty, if none of known
        /// diff tools are found.
        /// </summary>
        public static List<Diff> FindKnownDiffProgs()
        {
            List<Diff> diffs = new List<Diff>();
            List<Diff> candidates = new List<Diff> {
                new Diff( "p4merge", "Perforce Merge", @"Perforce\P4Merge.exe", "%1 %2" ), 
                new Diff( "WinMerge", "WinMerge", @"WinMerge\WinMergeU.exe", "%1 %2" ), 
                new Diff( "BC3", "Beyond Compare 3", @"Beyond Compare 3\BComp.exe", "%1 %2" ), 
                new Diff( "KDiff3", "KDiff3", @"KDiff3\kdiff3.exe", "%1 %2" )
            };

            // From the list of known tools ("candidates"), pick those which could be
            // found at the indicated folder locations and add them to the final list
            foreach (var d in candidates)
            {
                string path = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.ProgramFilesX86), d.Path);
                if (File.Exists(path))
                {
                    Diff diff = d;
                    diff.Path = path;
                    diffs.Add(diff);
                }
            }
            return diffs;
        }

        /// <summary>
        /// Extract internal diff program into Application data
        /// </summary>
        public static Diff GetInternalDiff()
        {
            Diff diff = new Diff();

            string appPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            ClassUtils.WriteResourceToFile(appPath, "QtCore4.dll", Properties.Resources.QtCore4);
            ClassUtils.WriteResourceToFile(appPath, "QtGui4.dll", Properties.Resources.QtGui4);
            ClassUtils.WriteResourceToFile(appPath, "QtXml4.dll", Properties.Resources.QtXml4);

            diff.Path = ClassUtils.WriteResourceToFile(appPath, "p4merge.exe", Properties.Resources.p4merge);
            diff.Difftool = "InternalP4Merge";
            diff.Args = "%1 %2";
            diff.Name = "Internal P4Merge";

            return diff;
        }
    }
}
