using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GitForce
{
    public partial class FormHttps : Form
    {
        /// <summary>
        /// Contains the user name / password combo string when editing the embedded password
        /// </summary>
        public string PassCombo
        {
            get { return textUsername + "\t" + textPassword; }
            set
            {
                textUsername = "";
                textPassword = "";
                string[] combo = value.Trim().Split('\t');
                if (combo.Length == 1)
                    textPassword = combo[0];
                else if (combo.Length == 2)
                {
                    textUsername = combo[0];
                    textPassword = combo[1];
                }

                if (combo.Length == 1)
                    labelSet.Text = string.Format(@"https://{0}...", textUsername);
                else
                    labelSet.Text = string.Format(@"https://{0}:<password>@...", textUsername);
            }
        }

        /// <summary>
        /// Form constructor
        /// The caller can remove "embedded" tab since that one is not applicable unless
        /// editing an actual HTTPS remote repo
        /// </summary>
        public FormHttps(bool removeEmbedded)
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            // Add button click handlers that will expand the list of existing fetch and push URLs
            _menuHosts.ItemClicked += MenuHostsItemClicked;

            string user = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            netrcfilename = Path.Combine(user, ClassUtils.IsMono() ? ".netrc" : "_netrc");
            App.PrintStatusMessage("Using file " + netrcfilename, MessageType.Debug);

            // Load the .netrc file if it exists, ignore if it does not (we will create it on save)
            if (File.Exists(netrcfilename))
            {
                LoadNetrc(netrcfilename);
                PopulateNetrcView();
            }

            if (removeEmbedded)
                tabControl.TabPages.RemoveByKey("tabEmbedded");
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormHttpsFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);

            // Write back the modified content of the .netrc file
            if (netrcDirty)
                SaveNetrc(netrcfilename);
        }

        #region Management of .netrc file

        private readonly ContextMenuStrip _menuHosts = new ContextMenuStrip();
        private readonly string netrcfilename;
        private Dictionary<string, Tuple<string, string>> netrc = new Dictionary<string, Tuple<string, string>>();
        private bool netrcDirty;

        /// <summary>
        /// Load .netrc file into internal data structure
        /// </summary>
        private void LoadNetrc(string filename)
        {
            try
            {
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
                        if ((keys.Count() != 6) || (keys[0] != "machine") || (keys[2] != "login") || (keys[4] != "password"))
                            throw new Exception("Bad entry: " + line);

                        netrc[keys[1]] = new Tuple<string, string>(keys[3], keys[5]); // [machine] = (login, password)

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
                        file.WriteLine(string.Format("machine {0} login {1} password {2}", machine.Key, machine.Value.Item1, machine.Value.Item2));
                }
            }
            catch (Exception ex)
            {
                App.PrintStatusMessage(ex.Message, MessageType.Error);
                MessageBox.Show(ex.Message, "Error writing .netrc file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Rebuilds the listview from the data stored in the internal machine dictionary
        /// </summary>
        private void PopulateNetrcView()
        {
            listHosts.BeginUpdate();
            listHosts.Items.Clear();
            foreach (var machine in netrc)
            {
                ListViewItem item = new ListViewItem(machine.Key);
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, machine.Value.Item1));
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "*****"));
                listHosts.Items.Add(item);
            }
            listHosts.EndUpdate();
            ListHostsItemSelectionChanged(null, null);
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

        /// <summary>
        /// Text in the input box has changed. Validate it and if it's a valid HTTPS URL, enable the "Add" button
        /// </summary>
        private void TextBoxHostTextChanged(object sender, EventArgs e)
        {
            // First, a simple test that allows users to paste a clone command
            textBoxHost.Text = textBoxHost.Text.Replace("git clone ", "");

            ClassUrl.Url host = ClassUrl.Parse(textBoxHost.Text);
            btAddHost.Enabled = host.Type == ClassUrl.UrlType.Https;
        }

        /// <summary>
        /// User clicked on the Add host button, prompt for user name and password and then add it
        /// </summary>
        private void BtAddHostClick(object sender, EventArgs e)
        {
            // We assume the URL in the text box is a well formatted HTTPS string since Add button would not be enabled otherwise
            ClassUrl.Url host = ClassUrl.Parse(textBoxHost.Text.Trim());
            AddEdit(host.Host); // Use only the host part of the address
        }

        /// <summary>
        /// User clicked on the Edit button after selecting a single item, open the dialog to edit user name and password
        /// </summary>
        private void BtEditClick(object sender, EventArgs e)
        {
            // We assume a single item is selected since Edit button would not be enabled otherwise
            AddEdit(listHosts.SelectedItems[0].Text);
        }

        /// <summary>
        /// Given a machine URL, let the user edit its user name and password, update internal data store and the view
        /// </summary>
        private void AddEdit(string machine)
        {
            FormHttpsAuth formHttpsAuth = new FormHttpsAuth();
            if (netrc.ContainsKey(machine))
                formHttpsAuth.PassCombo = netrc[machine].Item1 + "\t" + netrc[machine].Item2;
            if (formHttpsAuth.ShowDialog() == DialogResult.OK)
            {
                //if (netrc.ContainsKey(machine))
                //    netrc.Remove("machine");
                netrc[machine] = new Tuple<string, string>(formHttpsAuth.Username, formHttpsAuth.Password);

                netrcDirty = true;
                PopulateNetrcView();
                textBoxHost.Text = "";
            }
        }

        /// <summary>
        /// User selected (or deselected) one or more items in the view, enable or disable Remove and Edit buttons
        /// </summary>
        private void ListHostsItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            btRemoveHost.Enabled = listHosts.SelectedIndices.Count > 0; // Remove is enabled when a nonzero items are selected
            btEdit.Enabled = listHosts.SelectedIndices.Count == 1; // Edit is enabled when only one item is selected
        }

        /// <summary>
        /// User clicked on the Remove button, delete selected items
        /// </summary>
        private void BtRemoveHostClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listHosts.SelectedItems)
                netrc.Remove(item.Text);

            netrcDirty = true;
            PopulateNetrcView();
        }

        #endregion

        #region Management of embedded option tab

        private string textUsername;
        private string textPassword;

        /// <summary>
        /// User clicked on the Set button, open the dialog to enter the user name and password
        /// </summary>
        private void BtSetClick(object sender, EventArgs e)
        {
            FormHttpsAuth formHttpsAuth = new FormHttpsAuth();
            formHttpsAuth.PassCombo = PassCombo;
            if (formHttpsAuth.ShowDialog() == DialogResult.OK)
                PassCombo = formHttpsAuth.PassCombo;
        }

        /// <summary>
        /// User clicked on the Clear button, clear the user name and password
        /// </summary>
        private void BtClearClick(object sender, EventArgs e)
        {
            PassCombo = "";
        }

        #endregion
    }
}
