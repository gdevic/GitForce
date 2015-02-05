using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Class containing a set of repos that the program knows about.
    /// It also contains a pointer to the current, active repo.
    /// </summary>
    public class ClassRepos
    {
        /// <summary>
        /// Contains a list of repos in this workspace
        /// </summary>
        public List<ClassRepo> Repos = new List<ClassRepo>();

        /// <summary>
        /// Pointer to the default repo to switch to upon start
        /// </summary>
        public ClassRepo Default { get; set; }

        /// <summary>
        /// Pointer to the currently active repo
        /// </summary>
        public ClassRepo Current { get; private set; }

        /// <summary>
        /// ToString override returns the number of repos.
        /// </summary>
        public override string ToString()
        {
            return Repos.Count.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Load a set of repositories from a file.
        /// Returns true is loading succeeded and this class is assigned a new set of repos.
        /// Returns false if loading failed, or was cancelled. The repos in this class did not change.
        /// </summary>
        public bool Load(string fileName)
        {
            bool ret = false;
            // Wrap the opening of a repository database with an outer handler
            try
            {
                using (FileStream file = new FileStream(fileName, FileMode.Open))
                {
                    try
                    {
                        // Load list of repos and the default repo string into temporary objects
                        BinaryFormatter rd = new BinaryFormatter();
                        List<ClassRepo> newRepos = (List<ClassRepo>)rd.Deserialize(file);
                        string defaultRepo = (string)rd.Deserialize(file);

                        // WAR: Mono 2.6.7 does not support serialization of a HashSet. At the same time...
                        // Quickly check that each repo is valid (find if at least one is not)
                        bool allOK = true;
                        foreach (ClassRepo repo in newRepos)
                        {
                            allOK &= ClassUtils.DirStat(repo.Root) == ClassUtils.DirStatType.Git;
                            repo.ExpansionReset(null);
                        }

                        // If at least one repo was not valid, give the user a chance to recreate it
                        if (allOK == false)
                        {
                            FormRecreateRepos recreateRepos = new FormRecreateRepos();
                            recreateRepos.Repos = newRepos;
                            if (recreateRepos.ShowDialog() != DialogResult.OK)
                                return false;
                            newRepos = recreateRepos.Repos;
                        }

                        // Assign our object's list of repos
                        Repos = newRepos;
                        // Upon load, set the current based on the default repo
                        Default = Repos.Find(r => r.Root == defaultRepo);
                        SetCurrent(Default);

                        ret = true;
                    }
                    catch (Exception ex)
                    {
                        throw new ClassException(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                App.PrintLogMessage(ex.Message, MessageType.Error);
            }
            return ret;
        }

        /// <summary>
        /// Saves the current set of repositories to a file.
        /// Returns true if save succeeded.
        /// </summary>
        public bool Save(string fileName)
        {
            bool ret = false;
            try
            {
                using (FileStream file = new FileStream(fileName, FileMode.Create))
                {
                    try
                    {
                        BinaryFormatter wr = new BinaryFormatter();
                        wr.Serialize(file, Repos);
                        wr.Serialize(file, Default == null ? "" : Default.Root);

                        ret = true;
                    }
                    catch (Exception ex)
                    {
                        throw new ClassException(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                App.PrintLogMessage(ex.Message, MessageType.Error);
            }
            return ret;
        }

        /// <summary>
        /// Loads username/email for all repos in this workspace.
        /// Removes repos that are not valid.
        /// </summary>
        public void Refresh()
        {
            // Get the user name and email for each repo adding invalid ones to the list
            List<ClassRepo> invalidRepos = Repos.Where(r => r.Init() == false).ToList();

            // Remove all invalid repos from the workspace
            foreach (var r in invalidRepos)
            {
                App.PrintStatusMessage("Removing invalid repo " + r, MessageType.General);
                Delete(r);
            }
        }

        /// <summary>
        /// Add a new repository with the root at the given path. Create a directory if it does not exist.
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
            if (!repo.Init())
                throw new ClassException("Unable to initialize git repository!");

            Repos.Add(repo);
            App.PrintStatusMessage("Adding repo " + repo, MessageType.General);

            // If this is a very first repo, set it as default and current
            if (Repos.Count == 1)
                Current = Default = repo;

            return repo;
        }

        /// <summary>
        /// Delete a repository given by its class variable
        /// </summary>
        public void Delete(ClassRepo repo)
        {
            Repos.Remove(repo);

            // If the current has been deleted, find a new current
            if (repo == Current)
                SetCurrent(Repos.Count > 0 ? Repos[0] : null);

            // Reset the default if that one has been deleted
            if (repo == Default)
                Default = Current;
        }

        /// <summary>
        /// Set the new current repo.
        /// Given repo can be null.
        /// </summary>
        public void SetCurrent(ClassRepo repo)
        {
            Current = repo;
            if (Current != null)
                Current.Remotes.Refresh(Current);   // Refresh the list of remote repos
        }

        /// <summary>
        /// Reorder the list of repos according to the ordering given in the argument
        /// That list of strings should list each and all ClassRepo' names in the desired new order
        /// </summary>
        public void SetOrder(List<string> order)
        {
            // Since ClassRepo implements sorting, use it but provide our own delegate to compare
            // That delegate simply returns desired compare based on our input list of names
            Repos.Sort(
                delegate(ClassRepo a, ClassRepo b) {
                    int ia = order.FindIndex(x => x.Equals(a.ToString()));
                    int ib = order.FindIndex(x => x.Equals(b.ToString()));
                    return ia.CompareTo(ib);
                }
            );
        }
    }
}