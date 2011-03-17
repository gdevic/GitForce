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

            ClassGit.Run("config --global " + cmd);
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

            repo.Run("config " + cmd);
        }

        /// <summary>
        /// Returns a value of a global git configuration key
        /// </summary>
        public static string GetGlobal(string key)
        {
            return ClassGit.Run("config --global --get " + key).Replace("\n", "");
        }

        /// <summary>
        /// Returns a value of a local git configuration key
        /// </summary>
        public static string GetLocal(ClassRepo repo, string key)
        {
            return repo.Run("config --get " + key).Replace("\n", "");
        }
    }
}
