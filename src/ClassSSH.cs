using System;
using System.IO;

namespace GitForce
{
    /// <summary>
    /// Manage SSH access.
    /// This class should be instantiated on Linux OS only.
    /// </summary>
    public class ClassSSH
    {
        private readonly string pathHelper;

        /// <summary>
        /// Constructor class function, create batch file helper in the temp space
        /// </summary>
        public ClassSSH()
        {
            string pathHelpertLong = ClassUtils.WriteResourceToFile(Path.GetTempPath(), "git_ssh_helper.sh", Properties.Resources.git_ssh_helper);
            pathHelper = ClassUtils.GetShortPathName(pathHelpertLong);
            // Make the batch file executable: this trick will only work with Mono
            File.SetAttributes(pathHelper, (FileAttributes)((uint) File.GetAttributes (pathHelper) | 0x80000000));
            App.PrintLogMessage("SSH helper path:" + pathHelper, MessageType.Error);
            ClassUtils.AddEnvar("GIT_SSH", pathHelper);
        }

        /// <summary>
        /// Destructor for the class make sure the resources are properly disposed of
        /// </summary>
        ~ClassSSH()
        {
            // No real harm done if we fail to remove temp files. The next time GitForce is
            // run we will reuse the same files, so the temp folder will not grow.
            try
            {
                // Note: We leave these files in to allow secondary application instances to co-exist
                //File.Delete(pathHelper);
            }
            catch (Exception ex)
            {
                // Never show UI from a finalizer: see the same catch in ClassPutty. The log call
                // is guarded because an exception escaping a finalizer terminates the process.
                try { App.PrintLogMessage("SSH cleanup: " + ex.Message, MessageType.Error); }
                catch (Exception) { }
            }
        }
    }
}
