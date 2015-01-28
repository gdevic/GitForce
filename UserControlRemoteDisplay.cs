using System;
using System.Windows.Forms;

namespace GitForce
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
            textUrlPush.ReadOnly = textUrlFetch.ReadOnly = textPushCmd.ReadOnly = !all;
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
            ClassRemotes.Remote repo = new ClassRemotes.Remote();
            repo.Name = textName.Text.Trim();
            repo.PushCmd = textPushCmd.Text.Trim();
            repo.Password = textPassword.Text.Trim();

            // For SSH, make sure the URL string follows the strict format
            if (_fetchUrl.Type == ClassUrl.UrlType.Ssh)
                repo.UrlFetch = ClassUrl.ToCanonical(textUrlFetch.Text.Trim());
            else
                repo.UrlFetch = textUrlFetch.Text.Trim();

            if (_pushUrl.Type == ClassUrl.UrlType.Ssh)
                repo.UrlPush = ClassUrl.ToCanonical(textUrlPush.Text.Trim());
            else
                repo.UrlPush = textUrlPush.Text.Trim();

            return repo;
        }

        /// <summary>
        /// Validate entries and return true if they are reasonably valid
        /// </summary>
        public bool IsValid()
        {
            _fetchUrl = ClassUrl.Parse(textUrlFetch.Text.Trim());
            _pushUrl = ClassUrl.Parse(textUrlPush.Text.Trim());

            // Consider valid entry if the name is ok and some combination of urls
            return textName.Text.Trim().Length > 0 && _fetchUrl.Type != ClassUrl.UrlType.Unknown;
        }

        /// <summary>
        /// This function is called when text boxes name and URL changed.
        /// It calls delegate back to the caller.
        /// </summary>
        private void SomeTextChanged(object sender, EventArgs e)
        {
            // Call the delegate and also reparse our fetch and push URLs
            AnyTextChanged(IsValid());

            // Enable SSH button if one of the URLs uses SSH connection
            btSsh.Enabled = false;
            if (_fetchUrl.Ok && _fetchUrl.Type == ClassUrl.UrlType.Ssh) btSsh.Enabled = true;
            if (_pushUrl.Ok && _pushUrl.Type == ClassUrl.UrlType.Ssh) btSsh.Enabled = true;

            textPassword.ReadOnly = !(_fetchUrl.Type == ClassUrl.UrlType.Https || _pushUrl.Type == ClassUrl.UrlType.Https);

            // WAR: Permanently disable SSH button if not on Windows OS
            btSsh.Enabled = !ClassUtils.IsMono();
        }

        /// <summary>
        /// Manage SSH keys
        /// </summary>
        private void BtSshClick(object sender, EventArgs e)
        {
            FormSSH formSsh = new FormSSH();

            // Add one of the URLs from our input into the text box of a URL to add
            if (_fetchUrl.Type == ClassUrl.UrlType.Ssh)
                formSsh.AddHost(textUrlFetch.Text.Trim());
            else
                if (_pushUrl.Type == ClassUrl.UrlType.Ssh)
                    formSsh.AddHost(textUrlPush.Text.Trim());

            // Show the dialog. A previous call to AddHost will set the "Remote Keys" tab as current,
            // with one of the host strings (URLs) added into the input text box, ready to click on Add Host.
            formSsh.ShowDialog();
        }

        /// <summary>
        /// User clicked on the "->" button to the left of the git repo address.
        /// Find the canonical web site and open it
        /// </summary>
        private void BtWwwClick(object sender, EventArgs e)
        {
            string key = ((Button) sender).Tag.ToString();
            ClassUrl.Url url = key == "Fetch" ? _fetchUrl : _pushUrl;
            // Find the generic host name
            string target = "www." + url.Host;
            // Detect some special hosts for which we can form a complete path
            if (url.Host.Contains("github"))
                target += "/" + url.Path;
            ClassUtils.OpenWebLink(target);
        }
    }
}
