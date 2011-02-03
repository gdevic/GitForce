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
        private ClassURL.Url fetchUrl;
        private ClassURL.Url pushUrl;

        /// <summary>
        /// Callback delegate to signal when a text in a name or URL
        /// fields have changed.
        /// </summary>
        public delegate void AnyTextChangedDelegate(bool valid);
        public AnyTextChangedDelegate AnyTextChanged = voidAnyTextChanged;
        private static void voidAnyTextChanged(bool valid) { }

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
            textUrlPush.ReadOnly = textUrlFetch.ReadOnly = btSsh.Enabled = !all;
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
            textPassword.Text = "";
            btSsh.Enabled = false;
            textPassword.ReadOnly = true;
        }

        /// <summary>
        /// Set all input fields to parameter values
        /// </summary>
        /// <param name="repo">Values to set</param
        public void Set(ClassRemotes.Remote repo)
        {
            textName.Text = repo.name;
            textUrlFetch.Text = repo.urlFetch;
            textUrlPush.Text = repo.urlPush;
            textPassword.Text = repo.password;
        }

        /// <summary>
        /// Return current values from the input fields
        /// </summary>
        /// <returns>Structure with values</returns>
        public ClassRemotes.Remote Get()
        {
            ClassRemotes.Remote repo = new ClassRemotes.Remote();
            repo.name = textName.Text;
            repo.urlFetch = textUrlFetch.Text;
            repo.urlPush = textUrlPush.Text;
            repo.password = textPassword.Text;

            return repo;
        }

        /// <summary>
        /// Validate entries and return true if they are reasonably valid
        /// </summary>
        public bool isValid()
        {
            fetchUrl = ClassURL.Parse(textUrlFetch.Text);
            pushUrl = ClassURL.Parse(textUrlPush.Text);

            // Consider valid entry if the name is ok and some combination of urls
            return textName.Text.Length > 0 && fetchUrl.type != ClassURL.UrlType.Unknown;
        }

        /// <summary>
        /// This function is called when text boxes name and URL changed.
        /// It calls delegate back to the caller.
        /// </summary>
        private void SomeTextChanged(object sender, EventArgs e)
        {
            // Call the delegate and also reparse our fetch and push URLs
            AnyTextChanged(isValid());

            btSsh.Enabled = (fetchUrl.ok && fetchUrl.type == ClassURL.UrlType.Ssh) ||
                            (pushUrl.ok && pushUrl.type == ClassURL.UrlType.Ssh);

            textPassword.ReadOnly = !(fetchUrl.type == ClassURL.UrlType.Https || pushUrl.type == ClassURL.UrlType.Https);
        }

        /// <summary>
        /// Initializes SSH remote key by running PLINK utility
        /// PLINK code has been modified to silently accept the remote public key and store
        /// it into our registry.
        /// </summary>
        private void btSsh_Click(object sender, EventArgs e)
        {
            btSsh.Enabled = false;

            // We may need to run this twice only if the host for push differs from the host for fetch
            if (fetchUrl.ok == true)
            {
                App.Putty.ImportRemoteSshKey(fetchUrl);
                MessageBox.Show("Public key from " + fetchUrl.host + " successfully added to the registry.", "SSH", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (pushUrl.ok == true && pushUrl.host != fetchUrl.host)
            {
                App.Putty.ImportRemoteSshKey(pushUrl);
                MessageBox.Show("Public key from " + pushUrl.host + " successfully added to the registry.", "SSH", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
