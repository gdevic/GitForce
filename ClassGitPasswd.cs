using System;
using System.IO;

namespace GitForce
{
    /// <summary>
    /// Manages git HTTPS password helper file
    /// </summary>
    public class ClassGitPasswd
    {
        /// <summary>
        /// A file that provides password for HTTPS operations
        /// </summary>
        private readonly string pathPasswordBatchHelper;

        /// <summary>
        /// Constructor creates a shell executable file that echoes the PASSWORD
        /// environment variable when called. When GIT_ASKPASS is present, Git
        /// obtains a password for its HTTPS operations by calling it.
        /// </summary>
        public ClassGitPasswd()
        {
            // WAR: Do a different kind of shell script dependent on the OS)
            if (ClassUtils.IsMono())
            {
                // Mono: Use the Shell script
                pathPasswordBatchHelper = Path.Combine(App.AppHome, "passwd.sh");
                File.WriteAllText(pathPasswordBatchHelper, "echo $PASSWORD" + Environment.NewLine);

                // Set the execute bit
                if (Exec.Run("chmod", "+x " + pathPasswordBatchHelper).Success() == false)
                    App.PrintLogMessage("ClassGitPasswd: Unable to chmod +x on " + pathPasswordBatchHelper, MessageType.Error);
            }
            else
            {
                // Windows: Use the CMD BAT script
                // Note: Using "App.AppHome" directory to host the batch helper file
                //       fails on XP where that directory has spaces in the name ("Documents and Settings")
                //       which git cannot handle in this context. Similarly, git will fail with
                //       any other path that contains a space.
                // This redirection is used to provide the password in an automated way.
                pathPasswordBatchHelper = Path.Combine(Path.GetTempPath(), "passwd.bat");
                File.WriteAllText(pathPasswordBatchHelper, "@echo %PASSWORD%" + Environment.NewLine);
                pathPasswordBatchHelper = ClassUtils.GetShortPathName(pathPasswordBatchHelper);
            }
            ClassUtils.AddEnvar("GIT_ASKPASS", pathPasswordBatchHelper);

            App.PrintLogMessage("Created HTTP password helper file: " + pathPasswordBatchHelper, MessageType.General);
        }
    }
}