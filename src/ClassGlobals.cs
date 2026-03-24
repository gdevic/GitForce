using System.Collections.Generic;
using System.Drawing;

namespace GitForce
{
    /// <summary>
    /// Class containing global variables used by several parts of the program.
    /// </summary>
    static class ClassGlobals
    {
        /// <summary>
        /// Application cached list of available fixed-width fonts
        /// </summary>
        public static readonly List<FontFamily> Fonts = new List<FontFamily>();

        /// <summary>
        /// List of temporary files that need to be deleted on app exit
        /// </summary>
        public static readonly List<string> TempFiles = new List<string>();

        /// <summary>
        /// The function that actually removes all temp files from the list
        /// </summary>
        public static void RemoveTempFiles()
        {
            foreach (var tempFile in TempFiles)
            {
                ClassUtils.DeleteFile(tempFile);
            }
        }
    }
}
