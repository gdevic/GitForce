using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Git4Win
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
                    ClassGit.Run("init");
                    App.Repos.Add(d);
                }
                catch (Exception ex)
                {
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
