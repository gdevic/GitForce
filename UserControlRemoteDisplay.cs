using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win
{
    public partial class RemoteDisplay : UserControl
    {
        /// <summary>
        /// Current parsing information of the URL strings
        /// </summary>
        private ClassUrl.Url _fetchUrl;
        private ClassUrl.Url _pushUrl;

        /// <summary>
        /// Callback delegate to signal when a text in a name or URL
        /// fields have changed.
        /// </summary>
        public delegate void AnyTextChangedDelegate(bool valid);
        public AnyTextChangedDelegate AnyTextChanged = VoidAnyTextChanged;
        private static void VoidAnyTextChanged(bool valid) { }

        public RemoteDisplay()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Enable or disable input fields.
        /// </summary>
        /// <param name="name">Enable or disable name input field</param>
        /// <param name="all">Enable or disable all other fields except the name</param>
        public void Enable(bool name, bool all)
        {
            textName.ReadOnly = !name;
            textUrlPush.ReadOnly = textUrlFetch.ReadOnly = textPushCmd.ReadOnly = btSsh.Enabled = !all;
            SomeTextChanged(null, null);
        }

        /// <summary>
        /// Clear all input fields
        /// </summary>
        public void Clear()
        {
            textName.Text = "";
            textUrlFetch.Text = "";
            textUrlPush.Text = "";
            textPushCmd.Text = "";
            textPassword.Text = "";
            btSsh.Enabled = false;
            textPassword.ReadOnly = true;
        }

        /// <summary>
        /// Set all input fields to parameter values
        /// </summary>
        /// <param name="repo">Values to set</param>
        public void Set(ClassRemotes.Remote repo)
        {
            textName.Text = repo.Name;
            textUrlFetch.Text = repo.UrlFetch;
            textUrlPush.Text = repo.UrlPush;
            textPushCmd.Text = repo.PushCmd;
            textPassword.Text = repo.Password;
        }

        /// <summary>
        /// Return current values from the input fields
        /// </summary>
        /// <returns>Structure with values</returns>
        public ClassRemotes.Remote Get()
        {
            // For SSH, make sure the URL string follows the strict format
            if (_fetchUrl.Type == ClassUrl.UrlType.Ssh)
                textUrlFetch.Text = ClassUrl.ToCanonical(textUrlFetch.Text);

            if (_pushUrl.Type == ClassUrl.UrlType.Ssh)
                textUrlPush.Text = ClassUrl.ToCanonical(textUrlPush.Text);

            ClassRemotes.Remote repo = new ClassRemotes.Remote();
            repo.Name = textName.Text;
            repo.UrlFetch = textUrlFetch.Text;
            repo.UrlPush = textUrlPush.Text;
            repo.PushCmd = textPushCmd.Text;
            repo.Password = textPassword.Text;

            return repo;
        }

        /// <summary>
        /// Validate entries and return true if they are reasonably valid
        /// </summary>
        public bool IsValid()
        {
            _fetchUrl = ClassUrl.Parse(textUrlFetch.Text);
            _pushUrl = ClassUrl.Parse(textUrlPush.Text);

            // Consider valid entry if the name is ok and some combination of urls
            return textName.Text.Length > 0 && _fetchUrl.Type != ClassUrl.UrlType.Unknown;
        }

        /// <summary>
        /// This function is called when text boxes name and URL changed.
        /// It calls delegate back to the caller.
        /// </summary>
        private void SomeTextChanged(object sender, EventArgs e)
        {
            // Call the delegate and also reparse our fetch and push URLs
            AnyTextChanged(IsValid());

            btSsh.Enabled = (_fetchUrl.Ok && _fetchUrl.Type == ClassUrl.UrlType.Ssh) ||
                            (_pushUrl.Ok && _pushUrl.Type == ClassUrl.UrlType.Ssh);

            textPassword.ReadOnly = !(_fetchUrl.Type == ClassUrl.UrlType.Https || _pushUrl.Type == ClassUrl.UrlType.Https);
        }

        /// <summary>
        /// Initializes SSH remote key by running PLINK utility
        /// PLINK code has been modified to silently accept the remote public key and store
        /// it into our registry.
        /// </summary>
        private void BtSshClick(object sender, EventArgs e)
        {
            btSsh.Enabled = false;

            // We may need to run this twice only if the host for push differs from the host for fetch
            if (_fetchUrl.Ok)
            {
                App.Putty.ImportRemoteSshKey(_fetchUrl);
                MessageBox.Show("Public key from " + _fetchUrl.Host + " successfully added to the registry.", "SSH", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (_pushUrl.Ok && _pushUrl.Host != _fetchUrl.Host)
            {
                App.Putty.ImportRemoteSshKey(_pushUrl);
                MessageBox.Show("Public key from " + _pushUrl.Host + " successfully added to the registry.", "SSH", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
