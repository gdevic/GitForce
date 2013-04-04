using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace GitForce
{
    /// <summary>
    /// This class implements the version related functions.
    /// It also checks for a new version of the program off the web.
    /// </summary>
    class ClassVersion
    {
        /// <summary>
        /// Returns the program version number
        /// </summary>
        public static string GetVersion()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            return FileVersionInfo.GetVersionInfo(asm.Location).FileVersion;
        }

        /// <summary>
        /// Return the build date/time string
        /// </summary>
        public static string GetBuild()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            return FileVersionInfo.GetVersionInfo(asm.Location).ProductName;
        }

        /// <summary>
        /// Set to true if a new version is available.
        /// This should really be interlocking, but volatile will do with bools.
        /// </summary>
        public volatile bool NewVersionAvailable = false;

        /// <summary>
        /// Thread handle for function that checks for a new version.
        /// </summary>
        //private readonly Thread threadCheck;

        /// <summary>
        /// Class constructor starts a new version check thread.
        /// </summary>
        public ClassVersion()
        {
            // TODO: Need to find a new home for release builds
            //threadCheck = new Thread(ThreadVersionCheck);
            //threadCheck.Start();
        }

        /// <summary>
        /// Terminates the version check thread, if it is still running.
        /// TODO: This never worked consistently
        /// </summary>
        ~ClassVersion()
        {
            // Abort the thread. First give it a nice nudge and then simply abort it.
            //threadCheck.Join(1);
            //threadCheck.Abort();
        }

        /// <summary>
        /// This function is run on a separate thread. It checks for a new version.
        /// </summary>
        private void ThreadVersionCheck()
        {
            // A lot of things can go wrong here...
            try
            {
                StringBuilder file = new StringBuilder();
                WebRequest request = WebRequest.Create("https://github.com/gdevic/GitForce/blob/master/Properties/AssemblyInfo.cs");
                request.Timeout = 1000;
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        file.Append(reader.ReadToEnd());
                    }
                }

                // Search for the version string, for example:
                // [assembly: AssemblyFileVersion(&quot;1.0.11&quot;)]
                string sPattern = @"AssemblyFileVersion\(&quot;(?<major>\d+).(?<minor>\d+).(?<build>\d+)&quot;\)";
                Regex r = new Regex(sPattern, RegexOptions.Compiled);
                if (r.IsMatch(file.ToString()))
                {
                    // Get the version numbers from the latest version checked in github
                    string major = r.Match(file.ToString()).Result("${major}");
                    string minor = r.Match(file.ToString()).Result("${minor}");
                    string build = r.Match(file.ToString()).Result("${build}");
                    int webMajor = Convert.ToInt32(major);
                    int webMinor = Convert.ToInt32(minor);
                    int webBuild = Convert.ToInt32(build);

                    // Get the current version numbers
                    string[] current = GetVersion().Split('.');
                    int thisMajor = Convert.ToInt32(current[0]);
                    int thisMinor = Convert.ToInt32(current[1]);
                    int thisBuild = Convert.ToInt32(current[2]);

                    // Compare two versions and set flag if the current one is less
                    if (thisMajor < webMajor)
                        NewVersionAvailable = true;
                    else
                        if (thisMinor < webMinor)
                            NewVersionAvailable = true;
                        else
                            if (thisBuild < webBuild)
                                NewVersionAvailable = true;

                    // By now we have log window availabe, so print out what's going on
                    if (NewVersionAvailable)
                        App.PrintStatusMessage("**** A new version of GitForce is available! ****", MessageType.NewVersion);
                    else
                        App.PrintStatusMessage("Version check OK - this version is up-to-date.", MessageType.General);
                }
                else
                    App.PrintStatusMessage("Version check: Unable to match pattern!", MessageType.Error);
            }
            catch (Exception ex)
            {
                App.PrintStatusMessage("Version check: " + ex.Message, MessageType.Error);
            }
        }
    }
}