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
        /// Program version number
        /// </summary>
        public override string ToString()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            return FileVersionInfo.GetVersionInfo(asm.Location).FileVersion;
        }

        /// <summary>
        /// Return the build date/time string
        /// </summary>
        public string GetBuild()
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
        private Thread ThreadCheck;

        /// <summary>
        /// Class constructor starts a new version check thread.
        /// </summary>
        public ClassVersion()
        {
            ThreadCheck = new Thread(ThreadVersionCheck);
            ThreadCheck.Start();
        }

        /// <summary>
        /// Class destructor terminates the version check thread, if it is still running.
        /// </summary>
        ~ClassVersion()
        {
            // Abort the thread. First give it a nice nudge and then simply abort it.
            ThreadCheck.Join(1000);
            ThreadCheck.Abort();
        }

        /// <summary>
        /// This function is run on a separate thread. It checks for a new version.
        /// </summary>
        private void ThreadVersionCheck()
        {
            // A lot of things can go wrong here...
            try
            {
                WebRequest request = WebRequest.Create("https://github.com/gdevic/GitForce/blob/master/Properties/AssemblyInfo.cs");
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                StringBuilder file = new StringBuilder();
                file.Append(reader.ReadToEnd());

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
                    int web_major = Convert.ToInt32(major);
                    int web_minor = Convert.ToInt32(minor);
                    int web_build = Convert.ToInt32(build);

                    // Get the current version numbers
                    string[] current = this.ToString().Split('.');
                    int this_major = Convert.ToInt32(current[0]);
                    int this_minor = Convert.ToInt32(current[1]);
                    int this_build = Convert.ToInt32(current[2]);

                    // Compare two versions and set flag if the current one is less
                    if (this_major < web_major)
                        NewVersionAvailable = true;
                    else
                        if (this_minor < web_minor)
                            NewVersionAvailable = true;
                        else
                            if (this_build < web_build)
                                NewVersionAvailable = true;

                    // By now we have log window availabe, so print out what's going on
                    if (NewVersionAvailable)
                        App.Log.Print("Version check OK - new version available!");
                    else
                        App.Log.Print("Version check OK - this version is up-to-date.");
                }
                else
                    App.Log.Print("Version check: Unable to match pattern!");
            }
            catch (Exception ex)
            {
                App.Log.Print("Version check: " + ex.Message);
            }
        }
    }
}
