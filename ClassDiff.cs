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
        public struct TDiff
        {
            /// <summary>
            /// Git configuration name for diff tool
            /// </summary>
            public string difftool;

            /// <summary>
            /// User-friendly name of the diff utility
            /// </summary>
            public string name;

            /// <summary>
            /// Full path/name to the diff utility
            /// </summary>
            public string path;

            /// <summary>
            /// Arguments needed to execute a 2-file diff
            /// </summary>
            public string args;

            public TDiff(string DiffTool, string Name, string Path, string Args)
            {
                difftool = DiffTool;
                name = Name;
                path = Path;
                args = Args;
            }
        }

        /// <summary>
        /// List of diff programs recognized by the application
        /// </summary>
        public List<TDiff> diffs = null;
        
        /// <summary>
        /// Init code to be called on the application startup.
        /// </summary>
        public bool Initialize()
        {
            // Find selected diff programs installed and add them to the list
            diffs = FindKnownDiffProgs();

            // If none of preset diff apps are present, extract our internal one
            if (diffs.Count == 0)
                diffs.Add(GetInternalDiff());

            // Assign the active diff utility or the first one on the list
            string activeName = Properties.Settings.Default.DiffActiveName;
            if (string.IsNullOrWhiteSpace(activeName))
                activeName = diffs[0].name;

            TDiff active = diffs[0];
            foreach (var d in diffs)
                if (d.name == activeName)
                    active = d;

            Properties.Settings.Default.DiffActiveName = active.name;

            // Lastly, configure the git by setting all optional diffs and the active one
            return Configure(diffs, active);
        }

        /// <summary>
        /// Configure git's external diff application by adding all tools from the given
        /// list into git's difftool sections and picking the active as the current app to use
        /// when doing the visual diff.
        /// Returns false if the list is empty, making no changes to the git config.
        /// </summary>
        public static bool Configure(List<TDiff> diffs, TDiff active)
        {
            // Set up global git config with each given diff program
            if (diffs.Count > 0)
            {
                foreach (var d in diffs)
                {
                    // All all diff tools to git global config as sections we can select from
                    string path = d.path.Replace('\\', '/');
                    string usr = d.args.Replace("%1", "$LOCAL").Replace("%2", "$REMOTE");
                    string arg = "'" + path + "' " + usr;
                    ClassConfig.Set("difftool." + d.difftool + ".cmd", arg);
                }

                // Make sure the prompt will be off for visual diff
                ClassConfig.Set("difftool.prompt", "false");

                // Set the active diff tool
                ClassConfig.Set("diff.tool", active.difftool);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Searches for the known diff programs and returns a list of those
        /// installed on the system. The list might be empty, if none of known
        /// diff tools are found.
        /// </summary>
        public static List<TDiff> FindKnownDiffProgs()
        {
            List<TDiff> diffs = new List<TDiff>();
            List<TDiff> candidates = new List<TDiff>()
            {
                { new TDiff( "p4merge", "Perforce Merge", @"Perforce\P4Merge.exe", "%1 %2" )},
                { new TDiff( "WinMerge", "WinMerge", @"WinMerge\WinMergeU.exe", "%1 %2" )},
                { new TDiff( "BC3", "Beyond Compare 3", @"Beyond Compare 3\BComp.exe", "%1 %2" )},
                { new TDiff( "KDiff3", "KDiff3", @"KDiff3\kdiff3.exe", "%1 %2" )}
            };

            // From the list of known tools ("candidates"), pick those which could be
            // found at the indicated folder locations and add them to the final list
            foreach (var d in candidates)
            {
                string path = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.ProgramFilesX86), d.path);
                if (File.Exists(path))
                {
                    TDiff diff = d;
                    diff.path = path;
                    diffs.Add(diff);
                }
            }
            return diffs;
        }

        /// <summary>
        /// Extract internal diff program into Application data
        /// </summary>
        public static TDiff GetInternalDiff()
        {
            TDiff diff = new TDiff();

            WriteResourceToFile(global::git4win.Properties.Resources.QtCore4, "QtCore4.dll");
            WriteResourceToFile(global::git4win.Properties.Resources.QtGui4, "QtGui4.dll");
            WriteResourceToFile(global::git4win.Properties.Resources.QtXml4, "QtXml4.dll");
            diff.path = WriteResourceToFile(global::git4win.Properties.Resources.p4merge, "p4merge.exe");
            diff.difftool = "InternalP4Merge";
            diff.args = "%1 %2";
            diff.name = "Internal P4Merge";

            return diff;
        }

        /// <summary>
        /// Writes binary resource to Application Data file
        /// </summary>
        private static string WriteResourceToFile(byte[] buffer, string filename)
        {
            string path = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), filename);
            try
            {
                using (var sw = new BinaryWriter(File.Open(path, FileMode.Create)))
                {
                    sw.Write(buffer);
                }
            }
            catch { };
            return path;
        }
    }
}
