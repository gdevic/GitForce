using System;
using System.Collections.Generic;

namespace GitForce
{
    /// <summary>
    /// Represents a named group of repositories (a "Project" folder in the Repos panel).
    /// Projects are purely visual/organizational and do not affect git operations.
    ///
    /// Serialization note: This class is serialized as part of ClassProjectLayout via BinaryFormatter.
    /// When adding new fields, mark them with [OptionalField] so that old workspace files
    /// (which lack those fields) can still be deserialized without errors.
    /// Removing or renaming existing fields will break backward compatibility.
    /// </summary>
    [Serializable]
    public class ClassProject
    {
        /// <summary>
        /// User-chosen project name (any characters allowed: spaces, slashes, etc.)
        /// </summary>
        public string Name;

        /// <summary>
        /// Ordered list of repository paths belonging to this project
        /// </summary>
        public List<string> RepoPaths = new List<string>();

        /// <summary>
        /// Whether the project node is expanded in the tree view UI
        /// </summary>
        public bool IsExpanded = true;

        public ClassProject(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
