using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Add a set of repositories to the workspace
    /// </summary>
    public partial class FormNewRepoScanAdd : Form
    {
        private readonly List<string> _dirs;
        private bool _enableAdd;

        public FormNewRepoScanAdd(List<string> dirs)
        {
            InitializeComponent();
            _dirs = dirs;
        }

        /// <summary>
        /// Start scanning at the time the form is first shown
        /// </summary>
        private void FormNewRepoScanAddShown(object sender, EventArgs e)
        {
            _enableAdd = true;

            // Create each of the repos for the selected directories
            foreach (var d in _dirs)
            {
                if (_enableAdd == false)
                    return;
                try
                {
                    textRepo.Text = d;
                    Application.DoEvents();

                    Directory.SetCurrentDirectory(d);
                    if (ClassGit.Run("init").Success() == false)
                        throw new ClassException("init failed.");
                    App.Repos.Add(d);
                }
                catch (Exception ex)
                {
                    App.Log.Print("FormNewRepoScanAddShown: Unable to add repo: " + ex.Message);
                    App.PrintStatusMessage(ex.Message);
                }
            }
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Cancel adding repos
        /// </summary>
        private void BtCancelClick(object sender, EventArgs e)
        {
            _enableAdd = false;
            DialogResult = DialogResult.OK;
        }
    }
}
