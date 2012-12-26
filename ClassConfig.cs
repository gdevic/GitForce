using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitForce
{
    /// <summary>
    /// Class Config gets and sets values of either global git configuration or a local repository
    /// </summary>
    public static class ClassConfig
    {
        /// <summary>
        /// Sets or removes a global git configuration value.
        /// If the value is null or empty string, the key will be removed (unset).
        /// </summary>
        public static void SetGlobal(string key, string value)
        {
            string setkey = string.IsNullOrEmpty(value) ? "--unset " : "";
            string val = string.IsNullOrEmpty(value) ? "" : " \"" + value + "\"";
            string cmd = setkey + key + val;

            if (ClassGit.Run("config --global " + cmd).Success() == false)
                App.PrintLogMessage("Error setting global git config parameter!", MessageType.Error);
        }

        /// <summary>
        /// Sets or removes a local git configuration value.
        /// If the value is null or empty string, the key will be removed (unset).
        /// </summary>
        public static void SetLocal(ClassRepo repo, string key, string value)
        {
            string setkey = string.IsNullOrEmpty(value) ? "--unset " : "";
            string val = string.IsNullOrEmpty(value) ? "" : " \"" + value + "\"";
            string cmd = setkey + key + val;

            if (repo.Run("config --local " + cmd).Success() == false)
                App.PrintLogMessage("Error setting local git config parameter!", MessageType.Error);
        }

        /// <summary>
        /// Returns a value of a global git configuration key
        /// </summary>
        public static string GetGlobal(string key)
        {
            ExecResult result = ClassGit.Run("config --global --get " + key);
            if (result.Success())
                return result.stdout;

            App.PrintLogMessage("Error getting global git config parameter!", MessageType.Error);
            return String.Empty;
        }

        /// <summary>
        /// Returns a value of a local git configuration key
        /// </summary>
        public static string GetLocal(ClassRepo repo, string key)
        {
            ExecResult result = repo.Run("config --local --get " + key);
            if (result.Success())
                return result.stdout;

            App.PrintLogMessage("Error getting local git config parameter!", MessageType.Error);
            return string.Empty;
        }
    }
}