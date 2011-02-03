using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win
{
    public partial class FormPuTTY : Form
    {
        /// <summary>
        /// Keeps the actual list of passphrases in plain text format
        /// </summary>
        private List<string> phrases = new List<string>();

        /// <summary>
        /// Show passphrases in plain text format or encrypted
        /// </summary>
        private bool isPlain = false;

        public FormPuTTY()
        {
            InitializeComponent();

            string[] keys = Properties.Settings.Default.PuTTYKeys.
                Split((",").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            listBoxKeys.Items.AddRange(keys);

            phrases = App.Putty.GetPassPhrases();
            RefreshPf();
        }

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
            App.Putty.SetPassPhrases(phrases);
        }

        /// <summary>
        /// Add a new key file to the list
        /// </summary>
        private void byAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string file = dlg.FileName.ToString();
                if (!listBoxKeys.Items.Contains(file))
                    listBoxKeys.Items.Add(file);
                SaveKeys();
                btImport.Enabled = true;
            }
        }

        /// <summary>
        /// Remove a selected key file from the list
        /// </summary>
        private void btRemove_Click(object sender, EventArgs e)
        {
            listBoxKeys.Items.Remove(listBoxKeys.SelectedItem);
            SaveKeys();
            btImport.Enabled = true;
        }

        /// <summary>
        /// Add a new passphrase to the list
        /// </summary>
        private void btAddP_Click(object sender, EventArgs e)
        {
            // For security reasons, make sure the Show button hides plain text phrases
            if (isPlain) btShow_Click(null, null);
            phrases.Add(textBoxInputPf.Text);
            textBoxInputPf.Text = "";
            SavePfs();
            RefreshPf();
            btImport.Enabled = true;
        }

        /// <summary>
        /// Remove selected passphrase from the list
        /// </summary>
        private void btRemoveP_Click(object sender, EventArgs e)
        {
            phrases.RemoveAt(listBoxPf.SelectedIndex);
            listBoxPf.Items.Remove(listBoxPf.SelectedItem);
            SavePfs();
            btImport.Enabled = true;
        }

        /// <summary>
        /// Refresh the list of passphrases
        /// </summary>
        private void RefreshPf()
        {
            listBoxPf.Items.Clear();
            listBoxPf.Items.AddRange(
                phrases.Select(
                    item => isPlain ? 
                        item : 
                        item[0] + new String('*', item.Length)).ToArray());
        }

        /// <summary>
        /// Toggle plaintext passphrases
        /// </summary>
        private void btShow_Click(object sender, EventArgs e)
        {
            btShowPf.Text = isPlain ? "Show" : "Hide";
            isPlain = !isPlain;
            RefreshPf();
        }

        /// <summary>
        /// Run the PuTTY pageant process and reload all keys
        /// </summary>
        private void btImport_Click(object sender, EventArgs e)
        {
            btImport.Enabled = false;
            App.Putty.RunPageantUpdateKeys();
        }

        /// <summary>
        /// Simply run the PuTTYgen utility
        /// </summary>
        private void btPuttygen_Click(object sender, EventArgs e)
        {
            App.Putty.RunPuTTYgen();
        }

        /// <summary>
        /// Disable or enable Remove passphrase button based on the selection
        /// </summary>
        private void listBoxKeys_SelectedIndexChanged(object sender, EventArgs e)
        {
            btRemove.Enabled = (sender as ListBox).SelectedItem != null;
        }

        /// <summary>
        /// Disable or enable Remove passphrase button based on the selection
        /// </summary>
        private void listBoxPf_SelectedIndexChanged(object sender, EventArgs e)
        {
            btRemovePf.Enabled = (sender as ListBox).SelectedItem!=null;
        }

        /// <summary>
        /// Disable or enable Add passphrase button based on the text in the edit box
        /// </summary>
        private void textBoxInputPf_TextChanged(object sender, EventArgs e)
        {
            btAddPf.Enabled = !string.IsNullOrWhiteSpace(textBoxInputPf.Text);
        }
    }
}
