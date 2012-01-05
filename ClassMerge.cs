using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace GitForce
{
    /// <summary>
    /// Manage various merge programs and merge execution
    /// </summary>
    class ClassMerge
    {
        // Common merge utilities:
        //
        // We jam together Windows and Linux utilities
        private static readonly string ProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        private static readonly List<AppHelper> Candidates = new List<AppHelper> {
                //   Config    Short name        Path                                                      Arguments
                // Windows OS:
                new AppHelper( "p4merge",        Path.Combine(ProgramFiles,@"Perforce\P4Merge.exe"),       "%1 %2 %3 %4" ),
                new AppHelper( "WinMerge",       Path.Combine(ProgramFiles,@"WinMerge\WinMergeU.exe"),     "/e /x /u %2 %3 %4" ),
                new AppHelper( "BC3",            Path.Combine(ProgramFiles,@"Beyond Compare 3\BComp.com"), "%2 %3 /mergeoutput=%4" ),
                new AppHelper( "KDiff3",         Path.Combine(ProgramFiles,@"KDiff3\kdiff3.exe"),          "%1 %2 %3 -o %4" ),

                // Linux OS:
                new AppHelper( "KDiff3",         @"/usr/bin/kdiff3",            "%1 %2 %3 %4" ),
                new AppHelper( "TKDiff",         @"/usr/bin/tkdiff",            "%2 %3 -o %4" ),
                new AppHelper( "Meld",           @"/usr/bin/meld",              "%1 %2 %3 %4" ),
                new AppHelper( "xxdiff",         @"/usr/bin/xxdiff",            "%2 %3 -M %4" ),
                new AppHelper( "Diffuse",        @"/usr/bin/diffuse",           "%1 %2 %3"    ),

                new AppHelper( "Emerge",         @"/usr/bin/emerge",            "%1 %2 %3 %4" ), // Not tested!
                new AppHelper( "vimdiff",        @"/usr/bin/vimdiff",           "%1 %2 %3 %4" ), // Not tested!
                new AppHelper( "gvimdiff",       @"/usr/bin/gvimdiff",          "%1 %2 %3 %4" ), // Not tested!
                new AppHelper( "ecmerge",        @"/usr/bin/ecmerge",           "%1 %2 %3 %4" ), // Not tested!
                new AppHelper( "tortoisemerge",  @"/usr/bin/tortoisemerge",     "%1 %2 %3 %4" ), // Not tested!
                new AppHelper( "opendiff",       @"/usr/bin/opendiff",          "%1 %2 %3 %4" ), // Not tested!
        };

        private List<AppHelper> _merge = new List<AppHelper>();

        /// <summary>
        /// Init code to be called on the application startup.
        /// Return false if no merge utility was found and user wanted to quit the app.
        /// </summary>
        public bool Initialize()
        {
            // Verify the application default merge utility
            AppHelper app = new AppHelper(Properties.Settings.Default.MergeAppHelper);
            if (File.Exists(app.Path))
            {
                Configure(app);
                return true;
            }

            // Search for any of the predefined tools
            _merge = GetDetected();

            // If none of the pre-set merge apps are present, show the missing merge dialog
            // and return with its selection of whether to continue or quit the app
            if (_merge.Count == 0)
            {
                FormMergeMissing formMergeMissing = new FormMergeMissing();
                return formMergeMissing.ShowDialog() == DialogResult.OK;
            }

            // Otherwise, at least one merge app is present, select it as default
            Properties.Settings.Default.MergeAppHelper = _merge[0].ToString();

            Configure(_merge[0]);
            return true;
        }

        /// <summary>
        /// Configure a given application helper to be a Git merge utility
        /// </summary>
        public static void Configure(AppHelper app)
        {
            // Configure application only if it is valid
            if (app.Name != string.Empty)
            {
                string path = app.Path.Replace('\\', '/');
                string usr = app.Args.
                    Replace("%1", "$BASE").
                    Replace("%2", "$LOCAL").
                    Replace("%3", "$REMOTE").
                    Replace("%4", "$MERGED");
                string arg = "'" + path + "' " + usr;
                ClassConfig.SetGlobal("mergetool." + app.Name + ".path", path);
                ClassConfig.SetGlobal("mergetool." + app.Name + ".cmd", arg);

                // TODO: This might be an option: Set our default tool to be the Git merge tool?
                // ClassConfig.SetGlobal("merge.tool", app.Name);
            }
        }

        /// <summary>
        /// Return a proper merge command.
        /// This function is called from the actual menu item to merge/resolve a file.
        /// </summary>
        public static string GetMergeCmd()
        {
            // Get the application default merge utility
            AppHelper app = new AppHelper(Properties.Settings.Default.MergeAppHelper);
            string cmd = string.Format(" --tool={0} --no-prompt ", app.Name);

            return cmd;
        }

        /// <summary>
        /// Returns a list of detected Merge application helpers
        /// </summary>
        public static List<AppHelper> GetDetected()
        {
            return AppHelper.Scan(Candidates);
        }
    }
}
