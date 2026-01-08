using System;
using System.Collections.Generic;
using System.IO;

namespace GitForce
{
    /// <summary>
    /// Class containing a set of submodules for a given repository
    /// </summary>
    [Serializable]
    public class ClassSubmodules
    {
        /// <summary>
        /// Structure describing a submodule
        /// </summary>
        [Serializable]
        public struct Submodule
        {
            /// <summary>
            /// Submodule name (path relative to repo root)
            /// </summary>
            public string Name;

            /// <summary>
            /// Full path to submodule directory
            /// </summary>
            public string Path;

            /// <summary>
            /// Remote URL for the submodule
            /// </summary>
            public string Url;

            /// <summary>
            /// Current commit SHA (from submodule status)
            /// </summary>
            public string Sha;

            /// <summary>
            /// Status code: ' '=OK, '-'=not init, '+'=different commit, 'U'=merge conflict
            /// </summary>
            public char StatusCode;

            /// <summary>
            /// True if submodule has been initialized
            /// </summary>
            public bool IsInitialized;
        }

        /// <summary>
        /// Dictionary of submodules keyed by their relative path
        /// </summary>
        private Dictionary<string, Submodule> submodules = new Dictionary<string, Submodule>();

        /// <summary>
        /// Return the list of submodule paths
        /// </summary>
        /// <returns>List of relative paths to all submodules</returns>
        public List<string> GetPaths()
        {
            return new List<string>(submodules.Keys);
        }

        /// <summary>
        /// Return the submodule at a given path
        /// </summary>
        /// <param name="path">Relative path to the submodule</param>
        /// <returns>Submodule struct, or empty struct if not found</returns>
        public Submodule Get(string path)
        {
            return submodules.ContainsKey(path) ? submodules[path] : new Submodule();
        }

        /// <summary>
        /// Check if a path is a submodule
        /// </summary>
        /// <param name="path">Relative path to check</param>
        /// <returns>True if the path is a submodule</returns>
        public bool IsSubmodule(string path)
        {
            return submodules.ContainsKey(path);
        }

        /// <summary>
        /// Return count of submodules
        /// </summary>
        public int Count => submodules.Count;

        /// <summary>
        /// Refresh the list of submodules for the given repo.
        /// Parses both .gitmodules config and git submodule status output.
        /// </summary>
        /// <param name="repo">Repository to refresh submodules for</param>
        public void Refresh(ClassRepo repo)
        {
            submodules.Clear();

            // Check if .gitmodules file exists
            string gitmodulesPath = System.IO.Path.Combine(repo.Path, ".gitmodules");
            if (!File.Exists(gitmodulesPath))
                return;

            // Parse .gitmodules to get path mappings
            // Format: submodule.NAME.path VALUE
            ExecResult configResult = repo.Run("config --file .gitmodules --get-regexp path");
            if (configResult.Success())
            {
                string[] lines = configResult.stdout.Split(
                    Environment.NewLine.ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                {
                    // Parse: "submodule.name.path actual/path"
                    int spaceIdx = line.IndexOf(' ');
                    if (spaceIdx > 0)
                    {
                        string submodulePath = line.Substring(spaceIdx + 1).Trim();
                        // Normalize path separators for the current platform
                        submodulePath = submodulePath.Replace('/', System.IO.Path.DirectorySeparatorChar);

                        Submodule sm = new Submodule
                        {
                            Name = submodulePath,
                            Path = System.IO.Path.Combine(repo.Path, submodulePath),
                            IsInitialized = false,
                            StatusCode = '-'
                        };
                        submodules[submodulePath] = sm;
                    }
                }
            }

            // Get URLs for each submodule
            // Format: submodule.NAME.url VALUE
            ExecResult urlResult = repo.Run("config --file .gitmodules --get-regexp url");
            if (urlResult.Success())
            {
                string[] lines = urlResult.stdout.Split(
                    Environment.NewLine.ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                {
                    int spaceIdx = line.IndexOf(' ');
                    if (spaceIdx > 0)
                    {
                        string key = line.Substring(0, spaceIdx);
                        string url = line.Substring(spaceIdx + 1).Trim();

                        // Extract name from key: submodule.NAME.url
                        if (key.StartsWith("submodule.") && key.EndsWith(".url"))
                        {
                            string name = key.Substring(10, key.Length - 14);
                            // Normalize for lookup
                            name = name.Replace('/', System.IO.Path.DirectorySeparatorChar);

                            // Find matching submodule and update URL
                            if (submodules.ContainsKey(name))
                            {
                                Submodule sm = submodules[name];
                                sm.Url = url;
                                submodules[name] = sm;
                            }
                        }
                    }
                }
            }

            // Get status of each submodule (SHA and status code)
            // Format: [STATUS]SHA PATH (DESCRIBE)
            // STATUS: ' '=OK, '-'=not initialized, '+'=different commit, 'U'=merge conflict
            ExecResult statusResult = repo.Run("submodule status");
            if (statusResult.Success())
            {
                string[] lines = statusResult.stdout.Split(
                    Environment.NewLine.ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                {
                    if (line.Length < 42)
                        continue;

                    char statusCode = line[0];
                    string sha = line.Substring(1, 40);
                    string rest = line.Substring(42).Trim();

                    // Path is everything up to first space or parenthesis
                    int endIdx = rest.IndexOfAny(new[] { ' ', '(' });
                    string path = endIdx > 0 ? rest.Substring(0, endIdx) : rest;
                    // Normalize path separators
                    path = path.Replace('/', System.IO.Path.DirectorySeparatorChar);

                    if (submodules.ContainsKey(path))
                    {
                        Submodule sm = submodules[path];
                        sm.Sha = sha;
                        sm.StatusCode = statusCode;
                        sm.IsInitialized = (statusCode != '-');
                        submodules[path] = sm;
                    }
                }
            }
        }
    }
}
