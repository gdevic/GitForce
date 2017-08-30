using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace GitForce
{
    public partial class FormHttps : Form
    {
        private readonly string netrcfilename;

        /// <summary>
        /// Form constructor
        /// </summary>
        public FormHttps()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            // Add button click handlers that will expand the list of existing fetch and push URLs
            _menuHosts.ItemClicked += MenuHostsItemClicked;

            string user = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            netrcfilename = Path.Combine(user, ClassUtils.IsMono() ? ".netrc" : "_netrc");
            App.PrintStatusMessage("Using file " + netrcfilename, MessageType.Debug);

            LoadNetrc(netrcfilename);
            PopulateNetrcView();
            SaveNetrc(netrcfilename);
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormHttpsFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Context menu used by the convenience button that list all existing remote host URLs
        /// </summary>
        private readonly ContextMenuStrip _menuHosts = new ContextMenuStrip();

        private Dictionary<string, Tuple<string, string>> netrc = new Dictionary<string, Tuple<string, string>>();

        /// <summary>
        /// Load .netrc file into internal data structure
        /// </summary>
        private void LoadNetrc(string filename)
        {
            try
            {
                string machine = null, login = null;
                using (StreamReader file = new StreamReader(filename))
                {
                    do
                    {
                        string line = file.ReadLine();

                        if (line == null)
                            break;
                        if (line == string.Empty)
                            continue;

                        List<string> keys = line.Split((" ").ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (keys.Count() != 2)
                            throw new Exception("Bad entry: " + line);

                        if (keys[0] == "machine") machine = keys[1].Trim();
                        else if (keys[0] == "login") login = keys[1].Trim();
                        else if (keys[0] == "password")
                        {
                            string password = keys[1].Trim();
                            if (string.IsNullOrEmpty(machine) || string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                                throw new Exception("Bad entry " + line);

                            if (netrc.ContainsKey(machine)) // Detect duplicate entries but continue
                                App.PrintStatusMessage("Duplicate machine entry " + machine, MessageType.Error);
                            else
                                netrc[machine] = new Tuple<string, string>(login, password);

                            machine = login = null;
                        }
                        else throw new Exception("Bad entry: " + keys[0]);

                    } while (true);
                }
            }
            catch (Exception ex)
            {
                App.PrintStatusMessage(ex.Message, MessageType.Error);
                MessageBox.Show(ex.Message, "Error reading .netrc file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Write back internal data into .netrc file
        /// </summary>
        private void SaveNetrc(string filename)
        {
            try
            {
                using (StreamWriter file = new StreamWriter(filename))
                {
                    foreach (var machine in netrc)
                    {
                        file.WriteLine("machine " + machine.Key);
                        file.WriteLine("login " + machine.Value.Item1);
                        file.WriteLine("password " + machine.Value.Item2);
                    }
                }
            }
            catch (Exception ex)
            {
                App.PrintStatusMessage(ex.Message, MessageType.Error);
                MessageBox.Show(ex.Message, "Error writing .netrc file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateNetrcView()
        {
            listHosts.Items.Clear();
            foreach (var machine in netrc)
            {
                ListViewItem item = new ListViewItem(machine.Key);
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, machine.Value.Item1));
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "*****"));
                listHosts.Items.Add(item);
            }
        }

        private void BtListHostsClick(object sender, EventArgs e)
        {
            _menuHosts.Items.Clear();
            // Pick up a list of existing remote URLs and show them in the pull-down list
            List<string> hintUrls = App.Repos.GetRemoteUrls();
            foreach (var hintUrl in hintUrls)
                _menuHosts.Items.Add(hintUrl);
            _menuHosts.Show(btListHosts, new Point(0, btListHosts.Height));
        }

        /// <summary>
        /// User clicked on one of the hosts we listed in the pull down popup menu
        /// </summary>
        private void MenuHostsItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            textBoxHost.Text = e.ClickedItem.Text;
        }
    }
}
