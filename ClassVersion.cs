using System;
using System.Diagnostics;
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
        public volatile bool MessageAlreadySent = false;

        /// <summary>
        /// Thread handle for function that checks for a new version.
        /// </summary>
        private readonly Thread threadCheck;
        private readonly Thread altThreadCheck;

        /// <summary>
        /// Web request object that tries to fetch text from a target website
        /// </summary>
#if !DEBUG
        private readonly WebRequest request = null;
        private readonly WebRequest altRequest = null;
#endif

        /// <summary>
        /// Class constructor starts a new version check thread.
        /// </summary>
        public ClassVersion()
        {
#if !DEBUG
            // Create a web request object
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)0x00000C00; // SecurityProtocolType.Tls12;
            request = WebRequest.Create("https://sourceforge.net/projects/gitforce/files/");
            request.Timeout = 5000;

            string query = "?v=" + GetVersion() + (ClassUtils.IsMono() ? "&r=Mono" : "&r=.NET") + "&u=" + Environment.UserName;
            altRequest = WebRequest.Create("http://baltazarstudios.com/uc/GitForce/index.php" + query);
            altRequest.Timeout = 5000;

            // Create and start the thread to check for the new version
            threadCheck = new Thread(() => ThreadVersionCheck(request));
            threadCheck.Start();

            altThreadCheck = new Thread(() => ThreadVersionCheck(altRequest));
            altThreadCheck.Start();
#endif
        }

        /// <summary>
        /// Terminates the version check thread, if it is still running.
        /// </summary>
        ~ClassVersion()
        {
#if !DEBUG
            // Abort the web request
            if (request != null) request.Abort();
            if (altRequest != null) altRequest.Abort();
            // Abort the threads. First give it a nice nudge and then simply abort them.
            threadCheck.Join(10); threadCheck.Abort();
            altThreadCheck.Join(10); altThreadCheck.Abort();
#endif
        }

        /// <summary>
        /// This function is run on a separate thread. It checks for a new version.
        /// </summary>
        private void ThreadVersionCheck(WebRequest request)
        {
            // A lot of things can go wrong here...
            try
            {
                StringBuilder answer = new StringBuilder();
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        answer.Append(reader.ReadToEnd());
                    }
                }
                ParseNewVersionResponse(answer);
            }
            catch (Exception ex)
            {
                App.PrintStatusMessage("Version check: " + ex.Message, MessageType.Error);
            }
        }

        /// <summary>
        /// Parses the response string from new version check sites
        /// </summary>
        private void ParseNewVersionResponse(StringBuilder answer)
        {
            if (MessageAlreadySent) // Print only one message for multiple site's checks
                return;
            // Search for the version string, for example:
            // [assembly: AssemblyFileVersion(&quot;1.0.11&quot;)]
            string sPattern = @"GitForce-(?<major>\d+).(?<minor>\d+).(?<build>\d+).exe";
            Regex r = new Regex(sPattern, RegexOptions.Compiled);
            if (r.IsMatch(answer.ToString()))
            {
                // Get the version numbers from the latest version checked in github
                string major = r.Match(answer.ToString()).Result("${major}");
                string minor = r.Match(answer.ToString()).Result("${minor}");
                string build = r.Match(answer.ToString()).Result("${build}");
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
                else if (thisMinor < webMinor)
                    NewVersionAvailable = true;
                else if (thisBuild < webBuild)
                    NewVersionAvailable = true;

                // By now we have log window availabe, so print out what's going on
                if (NewVersionAvailable)
                    App.PrintStatusMessage("**** A new version of GitForce is available! ****", MessageType.NewVersion);
                else
                    App.PrintStatusMessage("Version check OK - this version is up-to-date.", MessageType.General);
                MessageAlreadySent = true;
            }
            else
                App.PrintStatusMessage("Version check: Unable to match pattern!", MessageType.Error);
        }
    }
}