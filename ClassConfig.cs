using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace git4win
{
    /// <summary>
    /// Class Config gets and sets values to either global configuration
    /// depot or a local repository
    /// </summary>
    public static class ClassConfig
    {
        public static string Run(string cmd, ClassRepo repo = null)
        {
            if (repo == null)
                return App.Git.Run("config --global " + cmd);
            else
                return repo.Run("config --local " + cmd);
        }

        /// <summary>
        /// Sets or removes a configuration value. If the value is null or empty string,
        /// the value will be unset.
        /// </summary>
        /// <param name="key">Key to set, example "user.name"</param>
        /// <param name="value">Value to set the key to, if empty, the key will be removed</param>
        /// <param name="repo">Optional repository to set the local key instead of a global</param>
        public static void Set(string key, string value, ClassRepo repo = null)
        {
            string setkey = (value==null || value.Length==0)? "--unset " : "";
            string val = (value == null || value.Length == 0) ? "" : " \"" + value + "\"";
            string cmd = setkey + key + val;

            if (repo == null)
                App.Git.Run("config --global " + cmd);
            else
                repo.Run("config --local " + cmd);
        }

        /// <summary>
        /// Gets a value for the given configuration key.
        /// The value will have removed newlines for easier handling.
        /// </summary>
        /// <param name="key">Key to get a value of, example "user.name"</param>
        /// <param name="repo">Optional repository from which to get the value</param>
        /// <returns>String containing the result of the get operation</returns>
        public static string Get(string key, ClassRepo repo = null)
        {
            if (repo == null)
                return App.Git.Run("config --global --get " + key).Replace("\n", "");
            else
                return repo.Run("config --local --get " + key).Replace("\n", "");
        }
    }
}
