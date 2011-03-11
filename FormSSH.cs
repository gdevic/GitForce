using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Git4Win
{
    public partial class FormSSH : Form
    {
        /// <summary>
        /// Keeps the actual list of passphrases in plain text format
        /// </summary>
        private readonly List<string> _phrases = new List<string>();

        /// <summary>
        /// Show passphrases in plain text format or encrypted
        /// </summary>
        private bool _isPlain;

        /// <summary>
        /// Form constructor
        /// </summary>
        public FormSSH()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            string[] keys = Properties.Settings.Default.PuTTYKeys.
                Split((",").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            listBoxKeys.Items.AddRange(keys);

            _phrases = App.Putty.GetPassPhrases();
            RefreshPf();
            RefreshRemoteHosts();
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormSshFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Sets the host URL string into a text box to add a host
        /// </summary>
        public void AddHost(string hostUrl)
        {
            textBoxHost.Text = hostUrl;
            tabControl.SelectedTab = tabRemoteKeys;
        }

        #region Local keys management

        /// <summary>
        /// Save the list of keys to load into the application properties
        /// </summary>
        private void SaveKeys()
        {
            string[] keys = listBoxKeys.Items.Cast<string>().ToArray();
            Properties.Settings.Default.PuTTYKeys = String.Join(",", keys);
        }

        /// <summary>
        /// Save the list of passphrases into the application properties
        /// </summary>
        private void SavePfs()
        {
            App.Putty.SetPassPhrases(_phrases);
        }

        /// <summary>
        /// Add a new key file to the list
        /// </summary>
        private void ByAddClick(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string file = dlg.FileName;
                if (!listBoxKeys.Items.Contains(file))
                    listBoxKeys.Items.Add(file);
                SaveKeys();
                btImport1.Enabled = btImport2.Enabled = true;
            }
        }

        /// <summary>
        /// Remove a selected key file from the list
        /// </summary>
        private void BtRemoveClick(object sender, EventArgs e)
        {
            listBoxKeys.Items.Remove(listBoxKeys.SelectedItem);
            SaveKeys();
            btImport1.Enabled = btImport2.Enabled = true;
        }

        /// <summary>
        /// Add a new passphrase to the list
        /// </summary>
        private void BtAddPClick(object sender, EventArgs e)
        {
            // For security reasons, make sure the Show button hides plain text phrases
            if (_isPlain) BtShowClick(null, null);
            _phrases.Add(textBoxInputPf.Text);
            textBoxInputPf.Text = "";
            SavePfs();
            RefreshPf();
            btImport1.Enabled = btImport2.Enabled = true;
        }

        /// <summary>
        /// Remove selected passphrase from the list
        /// </summary>
        private void BtRemovePClick(object sender, EventArgs e)
        {
            _phrases.RemoveAt(listBoxPf.SelectedIndex);
            listBoxPf.Items.Remove(listBoxPf.SelectedItem);
            SavePfs();
            btImport1.Enabled = btImport2.Enabled = true;
        }

        /// <summary>
        /// Refresh the list of passphrases
        /// </summary>
        private void RefreshPf()
        {
            listBoxPf.Items.Clear();
            listBoxPf.Items.AddRange(
                _phrases.Select(
                    item => _isPlain ?
                        item :
                        item[0] + new String('*', item.Length)).ToArray());
        }

        /// <summary>
        /// Toggle plaintext passphrases
        /// </summary>
        private void BtShowClick(object sender, EventArgs e)
        {
            btShowPf.Text = _isPlain ? "Show" : "Hide";
            _isPlain = !_isPlain;
            RefreshPf();
        }

        /// <summary>
        /// Run the PuTTY pageant process and reload all keys
        /// </summary>
        private void BtImportClick(object sender, EventArgs e)
        {
            btImport1.Enabled = btImport2.Enabled = false;
            App.Putty.RunPageantUpdateKeys();
        }

        /// <summary>
        /// Simply run the PuTTYgen utility
        /// </summary>
        private void BtPuttygenClick(object sender, EventArgs e)
        {
            App.Putty.RunPuTTYgen();
        }

        /// <summary>
        /// Disable or enable Remove passphrase button based on the selection
        /// </summary>
        private void ListBoxKeysSelectedIndexChanged(object sender, EventArgs e)
        {
            btRemove.Enabled = (sender as ListBox).SelectedItem != null;
        }

        /// <summary>
        /// Disable or enable Remove passphrase button based on the selection
        /// </summary>
        private void ListBoxPfSelectedIndexChanged(object sender, EventArgs e)
        {
            btRemovePf.Enabled = (sender as ListBox).SelectedItem != null;
        }

        /// <summary>
        /// Disable or enable Add passphrase button based on the text in the edit box
        /// </summary>
        private void TextBoxInputPfTextChanged(object sender, EventArgs e)
        {
            btAddPf.Enabled = !ClassUtils.IsNullOrWhiteSpace(textBoxInputPf.Text);
        }

        #endregion

        #region Remote keys management

        public ClassUrl.Url _remoteUrl;

        /// <summary>
        /// Load a list of remote hosts from the registry into the list box
        /// </summary>
        private void RefreshRemoteHosts()
        {
            listHosts.Items.Clear();
            try
            {
                RegistryKey SshKeys = Registry.CurrentUser;
                SshKeys = SshKeys.OpenSubKey(@"Software\SimonTatham\PuTTY\SshHostKeys");
                string[] keys = SshKeys.GetValueNames();
                SshKeys.Close();

                foreach (string key in keys)
                {
                    ClassUrl.Url url = ClassUrl.Parse(key);
                    // Decipher the registry host format using our URL parse functions,
                    // the fields align in the following way:
                    // Ex. rsa2@22:github.com
                    if (url.Ok)
                        listHosts.Items.Add(url.Path + "  Port: " + url.Host + "  Type: " + url.User);
                    else
                        listHosts.Items.Add(key);
                }
            }
            catch { }
        }

        /// <summary>
        /// Disable or enable Add Host button based on the validity of the host address
        /// </summary>
        private void TextBoxHostTextChanged(object sender, EventArgs e)
        {
            _remoteUrl = ClassUrl.Parse(textBoxHost.Text);
            btAddHost.Enabled = textBoxHost.Text.Length > 0 && _remoteUrl.Type == ClassUrl.UrlType.Ssh;
        }

        /// <summary>
        /// Request a key from the remote SSH server (host)
        /// The host name has already been validated by text changed function.
        /// </summary>
        private void BtAddHostClick(object sender, EventArgs e)
        {
            App.Putty.ImportRemoteSshKey(_remoteUrl);
            RefreshRemoteHosts();
            btAddHost.Enabled = false;
            textBoxHost.Text = "";
        }

        #endregion
    }
}
