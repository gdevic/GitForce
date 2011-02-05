using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace git4win
{
    /// <summary>
    /// Class containing a set of repos that the program knows about.
    /// Also contains a pointer to the current, active repo.
    /// </summary>
    public class ClassRepos
    {
        /// <summary>
        /// Location of the repos data file containing a list of repositories.
        /// They are kept in a separate file located in the user app data directory.
        /// </summary>
        private string reposDatFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)) 
                                    + @"\git4win-repos.dat";

        /// <summary>
        /// Contains a list of repos that the program knows about
        /// </summary>
        public List<ClassRepo> repos = new List<ClassRepo>();

        /// <summary>
        /// Name of the default repo to switch to upon start
        /// </summary>
        private string default_repo = "";

        /// <summary>
        /// Pointer to the current repo
        /// </summary>
        private ClassRepo _current = null;
        public ClassRepo current
        {
            set
            {
                // Skip refreshing if a repo did not change
                if (value != _current)
                {
                    _current = value;                       // Set the new current repo
                    if (current != null)
                        current.remotes.Refresh(current);   // Refresh the list of remote repos
                    App.Refresh();                          // Multicast refresh delegate
                }
            }
            get { return _current; }
        }

        /// <summary>
        /// Load a set of repositories from a file. This is called only once at
        /// the start of execution. Setting the current repo will refresh panes.
        /// </summary>
        public void Load()
        {
            if (File.Exists(reposDatFile))
            {
                FileStream file = new FileStream(reposDatFile, FileMode.Open);
                try
                {
                    BinaryFormatter rd = new BinaryFormatter();
                    repos = new List<ClassRepo>();
                    repos = (List<ClassRepo>)rd.Deserialize(file);
                    default_repo = (string)rd.Deserialize(file);

                    // Upon load, set the current based on the default repo
                    current = repos.Find(r => r.root == default_repo);
                }
                catch (Exception ex)
                {
                    App.Log(ex.Message);
                }
                finally
                {
                    file.Close();
                    App.Refresh();
                }
            }
        }

        /// <summary>
        /// Saves the current set of repositories to a file.
        /// This is called once the program is about to terminate.
        /// </summary>
        public void Save()
        {
            try
            {
                FileStream file = new FileStream(reposDatFile, FileMode.Create);
                BinaryFormatter wr = new BinaryFormatter();
                wr.Serialize(file, repos);
                wr.Serialize(file, default_repo);
                file.Close();
            }
            catch (Exception ex)
            {
                // TODO: App is exiting, so this will not be seen. Perhaps find another way?
                App.Log(ex.Message);
            }
        }

        /// <summary>
        /// Add a new repository with the root at the given path. Create a
        /// directory if it does not exist.
        /// This function throws exceptions!
        /// </summary>
        /// <param name="root">The root path of a new repository</param>
        /// <returns>Newly created repository class or null if a repo already exists at that root directory</returns>
        public ClassRepo Add(string root)
        {
            // Detect a repository with the same root path
            if (repos.Exists(r => r.root == root))
                throw new ClassException("Repository with the same name already exists!");

            Directory.CreateDirectory(root);
            ClassRepo repo = new ClassRepo(root);
            repos.Add(repo);

            // If this is the firs repo, set it as default
            if (repos.Count == 1)
                default_repo = repo.root;
            return repo;
        }

        /// <summary>
        /// Delete a repository given by its class variable
        /// </summary>
        /// <param name="repo">Repository class to remove from repos</param>
        public void Delete(ClassRepo repo)
        {
            repos.Remove(repo);

            // If the current has been deleted, find a new current
            if (repo == current)
                current = repos.Count > 0 ? repos[0] : null;

            // Refresh since the current might not have changed
            App.Refresh();
        }

        /// <summary>
        /// Sets the default repo to be loaded on the next repo data load
        /// </summary>
        public void SetDefault(ClassRepo repo)
        {
            default_repo = repo.root;
        }

        /// <summary>
        /// Returns the default repo
        /// </summary>
        public string GetDefault()
        {
            return default_repo;
        }
    }
}
