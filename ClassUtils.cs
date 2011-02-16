using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace git4win
{
    /// <summary>
    /// Contains various utility functions
    /// </summary>
    public static class ClassUtils
    {
        /// <summary>
        /// Writes binary resource to a temporary file
        /// </summary>
        public static string WriteResourceToFile(string pathName, string fileName, byte[] buffer)
        {
            string path = Path.Combine(pathName, fileName);
            try
            {
                using (var sw = new BinaryWriter(File.Open(path, FileMode.Create)))
                {
                    sw.Write(buffer);
                }
            }
            catch (Exception ex)
            {
                App.Execute.Add(ex.Message);
            }
            return path;
        }
    }
}
