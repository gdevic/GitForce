﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Manage various diff programs and diff execution
    /// </summary>
    public class ClassDiff
    {
        // Common diff utilities:
        //
        // We bundle together Windows and Linux utilities
        // Since we build this app in 32-bit mode, on 64-bit OS Windows Program Files will return a (x86) folder variant
        private static readonly string ProgramFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        private static readonly string ProgramFilesX64 = ProgramFilesX86.Contains(" (x86)") ? ProgramFilesX86.Replace(" (x86)", "") : ProgramFilesX86;
        private static readonly List<AppHelper> Candidates = new List<AppHelper> {
                //   Config    Short name        Path                                                         Arguments
                // Windows OS (x32):
                new AppHelper( "p4merge",        Path.Combine(ProgramFilesX86,@"Perforce\P4Merge.exe"),       "%1 %2" ),
                new AppHelper( "WinMerge",       Path.Combine(ProgramFilesX86,@"WinMerge\WinMergeU.exe"),     "/e /x /u %1 %2" ),
                new AppHelper( "BC3",            Path.Combine(ProgramFilesX86,@"Beyond Compare 3\BComp.com"), "%1 %2" ),
                new AppHelper( "BC4",            Path.Combine(ProgramFilesX86,@"Beyond Compare 4\BComp.com"), "%1 %2" ),
                new AppHelper( "KDiff3",         Path.Combine(ProgramFilesX86,@"KDiff3\kdiff3.exe"),          "%1 %2" ),
                new AppHelper( "Meld",           Path.Combine(ProgramFilesX64,@"Meld\Meld.exe"),              "%1 %2" ),
                // Windows OS (x64):
                new AppHelper( "p4merge",        Path.Combine(ProgramFilesX64,@"Perforce\P4Merge.exe"),       "%1 %2" ),
                new AppHelper( "WinMerge",       Path.Combine(ProgramFilesX64,@"WinMerge\WinMergeU.exe"),     "/e /x /u %1 %2" ),
                new AppHelper( "BC3",            Path.Combine(ProgramFilesX64,@"Beyond Compare 3\BComp.com"), "%1 %2" ),
                new AppHelper( "BC4",            Path.Combine(ProgramFilesX64,@"Beyond Compare 4\BComp.com"), "%1 %2" ),
                new AppHelper( "KDiff3",         Path.Combine(ProgramFilesX64,@"KDiff3\kdiff3.exe"),          "%1 %2" ),
                new AppHelper( "Meld",           Path.Combine(ProgramFilesX64,@"Meld\Meld.exe"),              "%1 %2" ),

                // Linux OS:
                new AppHelper( "KDiff3",         @"/usr/bin/kdiff3",   "%1 %2" ),
                new AppHelper( "TKDiff",         @"/usr/bin/tkdiff",   "%1 %2" ),
                new AppHelper( "Meld",           @"/usr/bin/meld",     "%1 %2" ),
                new AppHelper( "xxdiff",         @"/usr/bin/xxdiff",   "%1 %2" ),
                new AppHelper( "Diffuse",        @"/usr/bin/diffuse",  "%1 %2" ),
                new AppHelper( "BCompare",       @"/usr/bin/bcompare", "%1 %2" ),
        };

        private List<AppHelper> diff = new List<AppHelper>();

        /// <summary>
        /// Init code to be called on the application startup.
        /// Return false if no diff utility was found and user wanted to quit the app.
        /// </summary>
        public bool Initialize()
        {
            // Verify the application default diff utility
            AppHelper app = new AppHelper(Properties.Settings.Default.DiffAppHelper);
            if (File.Exists(app.Path))
            {
                Configure(app);
                return true;
            }

            // Search for any of the predefined tools
            diff = GetDetected();

            // If none of the pre-set diff apps are present, show the missing diff dialog
            // and return with its selection of whether to continue or quit the app
            if (diff.Count == 0)
            {
                FormDiffMissing formDiffMissing = new FormDiffMissing();
                return formDiffMissing.ShowDialog() == DialogResult.OK;
            }

            // Otherwise, at least one diff app is present, select it as default
            Properties.Settings.Default.DiffAppHelper = diff[0].ToString();

            Configure(diff[0]);
            return true;
        }

        /// <summary>
        /// Configure a given application helper to be a Git diff utility
        /// </summary>
        public static void Configure(AppHelper app)
        {
            // Configure application only if it is valid
            if (app.Name != string.Empty)
            {
                string path = app.Path.Replace('\\', '/');
                string usr = app.Args.
                    Replace("%1", "$LOCAL").
                    Replace("%2", "$REMOTE");
                string arg = "'" + path + "' " + usr;
                ClassConfig.SetGlobal("difftool." + app.Name + ".path", path);
                ClassConfig.SetGlobal("difftool." + app.Name + ".cmd", arg);

                // TODO: This might be an option: Set our default tool to be the Git gui tool?
                // ClassConfig.SetGlobal("diff.guitool", app.Name);
            }
        }

        /// <summary>
        /// Return a proper diff command.
        /// This function is called from the actual menu item to diff files.
        /// </summary>
        public static string GetDiffCmd()
        {
            // Get the application default visual diff utility
            AppHelper app = new AppHelper(Properties.Settings.Default.DiffAppHelper);
            string cmd = string.Format(" --tool={0} --no-prompt ", app.Name);

            return cmd;
        }

        /// <summary>
        /// Returns a list of detected diff application helpers
        /// </summary>
        public static List<AppHelper> GetDetected()
        {
            return AppHelper.Scan(Candidates);
        }
    }
}
