using System;
using System.Collections.Generic;
using System.Linq;

namespace GitForce
{
    /// <summary>
    /// Manages the layout of projects and repos in the tree view.
    ///
    /// Serialization note: This class is serialized as the third object in the workspace file
    /// (after List&lt;ClassRepo&gt; and the default repo path string). Old workspace files lack this
    /// object; deserialization failure is caught and an empty layout is created instead.
    /// When adding new fields, mark them with [OptionalField] so that workspace files saved
    /// by older versions can still be loaded. See also ClassProject for the same constraint.
    ///
    /// RootOrder stores the interleaved display order using prefixed strings:
    ///   "P:ProjectName" for project entries
    ///   "R:RepoPath" for ungrouped repo entries
    /// Repos inside projects are stored in ClassProject.RepoPaths.
    /// </summary>
    [Serializable]
    public class ClassProjectLayout
    {
        /// <summary>
        /// Ordered list of root-level items. Each entry is prefixed with "P:" or "R:".
        /// </summary>
        public List<string> RootOrder = new List<string>();

        /// <summary>
        /// All projects in this layout
        /// </summary>
        public List<ClassProject> Projects = new List<ClassProject>();

        /// <summary>
        /// Find a project by name (case-sensitive)
        /// </summary>
        public ClassProject FindProject(string name)
        {
            return Projects.Find(p => p.Name == name);
        }

        /// <summary>
        /// Find the project that contains the given repo path, or null
        /// </summary>
        public ClassProject FindProjectContaining(string repoPath)
        {
            return Projects.Find(p => p.RepoPaths.Contains(repoPath));
        }

        /// <summary>
        /// Add a new project. Returns false if a project with that name already exists.
        /// </summary>
        public bool AddProject(string name)
        {
            if (FindProject(name) != null)
                return false;
            Projects.Add(new ClassProject(name));
            RootOrder.Add("P:" + name);
            return true;
        }

        /// <summary>
        /// Delete a project, freeing its repos to root level (inserted where the project was).
        /// </summary>
        public void DeleteProject(string name)
        {
            ClassProject project = FindProject(name);
            if (project == null)
                return;

            string projectEntry = "P:" + name;
            int idx = RootOrder.IndexOf(projectEntry);

            // Remove the project entry
            RootOrder.Remove(projectEntry);

            // Insert freed repos at the position where the project was
            if (idx < 0) idx = RootOrder.Count;
            foreach (string repoPath in project.RepoPaths)
            {
                string repoEntry = "R:" + repoPath;
                if (idx <= RootOrder.Count)
                    RootOrder.Insert(idx++, repoEntry);
                else
                    RootOrder.Add(repoEntry);
            }

            Projects.Remove(project);
        }

        /// <summary>
        /// Rename a project. Returns false if the new name is already taken.
        /// </summary>
        public bool RenameProject(string oldName, string newName)
        {
            if (oldName == newName)
                return true;
            if (FindProject(newName) != null)
                return false;

            ClassProject project = FindProject(oldName);
            if (project == null)
                return false;

            // Update RootOrder entry
            int idx = RootOrder.IndexOf("P:" + oldName);
            if (idx >= 0)
                RootOrder[idx] = "P:" + newName;

            project.Name = newName;
            return true;
        }

        /// <summary>
        /// Add a repo to a project. Removes it from any other project first.
        /// </summary>
        public void AddRepoToProject(string projectName, string repoPath)
        {
            // Remove from any existing project
            RemoveRepoFromProjectOnly(repoPath);

            // Remove from root order (it's now inside a project)
            RootOrder.Remove("R:" + repoPath);

            ClassProject project = FindProject(projectName);
            if (project != null && !project.RepoPaths.Contains(repoPath))
                project.RepoPaths.Add(repoPath);
        }

        /// <summary>
        /// Insert a repo into a project at a specific index. Removes it from any other project first.
        /// </summary>
        public void InsertRepoInProject(string projectName, string repoPath, int index)
        {
            // Remove from any existing project
            RemoveRepoFromProjectOnly(repoPath);

            // Remove from root order
            RootOrder.Remove("R:" + repoPath);

            ClassProject project = FindProject(projectName);
            if (project != null)
            {
                project.RepoPaths.Remove(repoPath);
                if (index > project.RepoPaths.Count) index = project.RepoPaths.Count;
                project.RepoPaths.Insert(index, repoPath);
            }
        }

        /// <summary>
        /// Remove a repo from its project and place it back at root level.
        /// If insertBeforeEntry is provided, place it at that position; otherwise append.
        /// </summary>
        public void RemoveRepoFromProject(string repoPath, string insertBeforeEntry = null)
        {
            RemoveRepoFromProjectOnly(repoPath);

            string repoEntry = "R:" + repoPath;
            if (!RootOrder.Contains(repoEntry))
            {
                if (insertBeforeEntry != null)
                {
                    int idx = RootOrder.IndexOf(insertBeforeEntry);
                    if (idx >= 0)
                        RootOrder.Insert(idx, repoEntry);
                    else
                        RootOrder.Add(repoEntry);
                }
                else
                {
                    RootOrder.Add(repoEntry);
                }
            }
        }

        /// <summary>
        /// Remove a repo from whichever project contains it (internal helper, does not add to RootOrder)
        /// </summary>
        private void RemoveRepoFromProjectOnly(string repoPath)
        {
            foreach (ClassProject project in Projects)
                project.RepoPaths.Remove(repoPath);
        }

        /// <summary>
        /// Completely remove a repo from all tracking (projects and root order).
        /// Called when a repo is deleted or becomes invalid.
        /// </summary>
        public void RemoveRepoEntirely(string repoPath)
        {
            RemoveRepoFromProjectOnly(repoPath);
            RootOrder.Remove("R:" + repoPath);
        }

        /// <summary>
        /// Insert a root-level entry at a specific position relative to a target entry.
        /// </summary>
        public void InsertAtRootLevel(string entry, string targetEntry, bool after)
        {
            RootOrder.Remove(entry);
            int idx = RootOrder.IndexOf(targetEntry);
            if (idx < 0)
            {
                RootOrder.Add(entry);
                return;
            }
            if (after) idx++;
            RootOrder.Insert(idx, entry);
        }

        /// <summary>
        /// Ensure consistency after load: remove stale entries, add missing repos.
        /// </summary>
        public void Rebuild(List<ClassRepo> repos)
        {
            HashSet<string> validPaths = new HashSet<string>(
                repos.Select(r => r.Path), StringComparer.OrdinalIgnoreCase);

            // Remove stale repo paths from projects
            foreach (ClassProject project in Projects)
                project.RepoPaths.RemoveAll(p => !validPaths.Contains(p));

            // Remove stale R: entries from RootOrder
            RootOrder.RemoveAll(entry =>
                entry.StartsWith("R:") && !validPaths.Contains(entry.Substring(2)));

            // Remove P: entries for projects that no longer exist
            HashSet<string> projectNames = new HashSet<string>(Projects.Select(p => p.Name));
            RootOrder.RemoveAll(entry =>
                entry.StartsWith("P:") && !projectNames.Contains(entry.Substring(2)));

            // Collect all repo paths that are already tracked (in RootOrder or in a project)
            HashSet<string> tracked = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string entry in RootOrder)
            {
                if (entry.StartsWith("R:"))
                    tracked.Add(entry.Substring(2));
            }
            foreach (ClassProject project in Projects)
            {
                foreach (string path in project.RepoPaths)
                    tracked.Add(path);
            }

            // Append any repos not yet tracked as ungrouped root entries
            foreach (ClassRepo repo in repos)
            {
                if (!tracked.Contains(repo.Path))
                    RootOrder.Add("R:" + repo.Path);
            }
        }
    }
}
