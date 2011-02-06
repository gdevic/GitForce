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
        private readonly string _reposDataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)) 
                                    + @"\git4win-repos.dat";

        /// <summary>
        /// Contains a list of repos that the program knows about
        /// </summary>
        public List<ClassRepo> Repos = new List<ClassRepo>();

        /// <summary>
        /// Name of the default repo to switch to upon start
        /// </summary>
        private string _defaultRepo = "";

        public ClassRepo Current { get; private set; }

        /// <summary>
        /// Load a set of repositories from a file. This is called only once at
        /// the start of execution. Setting the current repo will refresh panes.
        /// </summary>
        public void Load()
        {
            if (File.Exists(_reposDataFile))
            {
                FileStream file = new FileStream(_reposDataFile, FileMode.Open);
                try
                {
                    BinaryFormatter rd = new BinaryFormatter();
                    Repos = new List<ClassRepo>();
                    Repos = (List<ClassRepo>)rd.Deserialize(file);
                    _defaultRepo = (string)rd.Deserialize(file);

                    // Upon load, set the current based on the default repo
                    SetCurrent(Repos.Find(r => r.Root == _defaultRepo));
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
                FileStream file = new FileStream(_reposDataFile, FileMode.Create);
                BinaryFormatter wr = new BinaryFormatter();
                wr.Serialize(file, Repos);
                wr.Serialize(file, _defaultRepo);
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
            if (Repos.Exists(r => r.Root == root))
                throw new ClassException("Repository with the same name already exists!");

            Directory.CreateDirectory(root);
            ClassRepo repo = new ClassRepo(root);
            Repos.Add(repo);

            // If this is the firs repo, set it as default
            if (Repos.Count == 1)
                _defaultRepo = repo.Root;
            return repo;
        }

        /// <summary>
        /// Delete a repository given by its class variable
        /// </summary>
        /// <param name="repo">Repository class to remove from repos</param>
        public void Delete(ClassRepo repo)
        {
            Repos.Remove(repo);

            // If the current has been deleted, find a new current
            if (repo == Current)
                SetCurrent(Repos.Count > 0 ? Repos[0] : null);

            // Refresh since the current might not have changed
            App.Refresh();
        }

        public void SetCurrent(ClassRepo repo)
        {
            Current = repo;                        // Set the new current repo
            if (Current != null)
                Current.Remotes.Refresh(Current);   // Refresh the list of remote repos
            App.Refresh();                          // Multicast refresh delegate
        }

        /// <summary>
        /// Sets the default repo to be loaded on the next repo data load
        /// </summary>
        public void SetDefault(ClassRepo repo)
        {
            _defaultRepo = repo.Root;
        }

        /// <summary>
        /// Returns the default repo
        /// </summary>
        public string GetDefault()
        {
            return _defaultRepo;
        }
    }
}
