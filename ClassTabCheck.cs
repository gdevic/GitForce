using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace GitForce
{
    /// <summary>
    /// Contains code to check files for TABs and EOL spaces.
    /// Implements the optional functionality enabled by a checkbox in the Settings->Files
    /// </summary>
    static class ClassTabCheck
    {
        /// <summary>
        /// If user checked a preference to scan for TABS, and the file extension matches,
        /// check the file(s) for TABs and EOL's spaces
        /// </summary>
        public static void CheckForTabs(List<string> files)
        {
            // Only check if the user setting enables the functionality
            if (!Properties.Settings.Default.WarnOnTabs)
                return;

            // Contains the final list of files that have TABs or EOL spaces
            List<string> xfiles;

            // Wrap the file checks with a performance diagnostics so we can track how long it takes to parse all files
            Stopwatch timer = new Stopwatch();
            timer.Start();
            {
                // Create a Regex expression corresponding to each type of file extension to match
                string[] extList = Properties.Settings.Default.WarnOnTabsExt.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                List<Regex> regexes = new List<Regex>();
                foreach (string sFileMask in extList)
                {
                    string expression = sFileMask.Trim().Replace(".", "[.]").Replace("*", ".*").Replace("?", ".") + "$";
                    regexes.Add(new Regex(expression));
                }

                xfiles = CheckTabsInFiles(files, regexes);
            }
            timer.Stop();
            App.PrintLogMessage("TabCheck: elapsed: " + timer.ElapsedMilliseconds + " ms", MessageType.Debug);

            // Print all files that have TABs or EOL spaces
            if (xfiles.Count > 0)
            {
                // Although it is a warning, internally we use a message type "Error" so the output will print in
                // red color and grab the attention
                App.PrintStatusMessage("WARNING: The following files contain TABs or EOL spaces:", MessageType.Error);
                foreach (string xfile in xfiles)
                    App.PrintStatusMessage(xfile, MessageType.Error);
            }
        }

        /// <summary>
        /// Check a list of files, filtered by a list of Regex expressions, for TABs or EOL spaces
        /// Returns a subset of files that contain TABs or EOL spaces
        /// </summary>
        private static List<string> CheckTabsInFiles(List<string> files, List<Regex> regexes)
        {
            List<string> xfiles = new List<string>();

            // Filter which files to check by using a regular expression of each file name
            foreach (string file in files)
            {
                foreach (Regex regex in regexes)
                {
                    // This file is to be checked using this particular regular expression
                    if (regex.IsMatch(file))
                    {
                        App.PrintLogMessage("TabCheck: " + file, MessageType.Debug);
                        if (CheckTabsInFile(file))
                            xfiles.Add(file);
                    }
                }
            }
            return xfiles;
        }

        /// <summary>
        /// Check if a file contains TAB characters or EOL spaces
        /// Assumes the file is a readable text file
        /// </summary>
        private static bool CheckTabsInFile(string file)
        {
            string[] lines;
            try
            {
                // Read the target file to check and separate into individual lines
                // If anything's wrong, this call will throw exceptions
                lines = File.ReadAllLines(file);
            }
            catch (Exception ex)
            {
                App.PrintStatusMessage(ex.Message, MessageType.Error);
                return false;
            }

            // This site compares several methods that could be used to quickly scan strings:
            // http://cc.davelozinski.com/c-sharp/fastest-way-to-check-if-a-string-occurs-within-a-string
            // Surprisingly, the fastest method seems to be the most basic indexed approach
            foreach (string line in lines)
            {
                bool eolspace = false;
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '\t')
                        return true;
                    eolspace = line[i] == ' ';
                }
                if (eolspace)
                    return true;
            }
            return false;
        }
    }
}
