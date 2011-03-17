using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Structure describing application helper programs (Diff, Merge)
    /// </summary>
    public struct AppHelper : IComparable<AppHelper>
    {
        /// <summary>
        /// User-friendly name of an application helper tool
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Full path/name to the tool
        /// </summary>
        public readonly string Path;

        /// <summary>
        /// Arguments needed when executing the tool for a particular purpose
        /// </summary>
        public readonly string Args;

        /// <summary>
        /// Constructor that simply assigns fields
        /// </summary>
        public AppHelper(string appName, string appPath, string appArgs)
        {
            Name = appName;
            Path = appPath;
            Args = appArgs;
        }

        /// <summary>
        /// Constructor that deserialize fields into a new structure
        /// </summary>
        public AppHelper(string appCombined)
        {
            Name = "";
            Path = appCombined;
            Args = "";
            string[] parts = appCombined.Split('\t');
            if(parts.Length==3)
            {
                Name = parts[0];
                Path = parts[1];
                Args = parts[2];
            }
        }

        /// <summary>
        /// ToString override returns serialized fields
        /// </summary>
        public override string ToString()
        {
            return Name + '\t' + Path + '\t' + Args;
        }

        /// <summary>
        /// Implements comparator
        /// </summary>
        public int CompareTo(AppHelper other)
        {
            return other.Name.CompareTo(this.Name);
        }

        /// <summary>
        /// Given a list of candidate programs, return only those that actually exist
        /// </summary>
        public static List<AppHelper> Scan(List<AppHelper> candidates)
        {
            return (from appHelper in candidates
                    let path = appHelper.Path
                    where File.Exists(path)
                    select appHelper).ToList();
        }
    }
}
