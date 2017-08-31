using System;
using System.Collections.Generic;
using System.Drawing;
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
        /// Flag that we are in editing mode
        /// </summary>
        private bool _isEditing = false;

        /// <summary>
        /// Context menus used by the convenience buttons that list all existing fetch and push URLs
        /// </summary>
        private readonly ContextMenuStrip _menuFetch = new ContextMenuStrip();
        private readonly ContextMenuStrip _menuPush = new ContextMenuStrip();

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

            // Add button click handlers that will expand the list of existing fetch and push URLs
            _menuFetch.ItemClicked += MenuFetchItemClicked;
            _menuPush.ItemClicked += MenuPushItemClicked;
        }

        /// <summary>
        /// Enable or disable input fields.
        /// </summary>
        /// <param name="name">Enable or disable name input field</param>
        /// <param name="all">Enable or disable all other fields except the name</param>
        public void Enable(bool name, bool all)
        {
            _isEditing = all; // When all fields are enabled, we are in editing mode
            textName.ReadOnly = !name;
            textUrlPush.ReadOnly = textUrlFetch.ReadOnly = textPushCmd.ReadOnly = !all;
            btListFetch.Enabled = btListPush.Enabled = textPassword.Enabled = btHttpsAuth.Enabled = all;
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
            btWWW1.Enabled = false;
            btWWW2.Enabled = false;
            btListFetch.Enabled = btListPush.Enabled = btHttpsAuth.Enabled = false;
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
            btWWW1.Enabled = isValidUrl(textUrlFetch.Text);
            btWWW2.Enabled = isValidUrl(textUrlPush.Text);
            textPassword.Text = repo.Password;

            // Rebuild the state variables _after_ all the URLs have been inserted
            SomeTextChanged(null, null);
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
            // Remove "git clone" substring from the input text which is commonly pasted from an online repo command
            if (textUrlFetch.Text.Trim().StartsWith("git clone"))
                textUrlFetch.Text = textUrlFetch.Text.Replace("git clone", "").Trim();
            if (textUrlPush.Text.Trim().StartsWith("git clone"))
                textUrlPush.Text = textUrlPush.Text.Replace("git clone", "").Trim();

            // Call the delegate and also reparse our fetch and push URLs
            AnyTextChanged(IsValid());

            // Change enable properties only if we are in editing mode, otherwise controls are grayed out
            if (_isEditing)
            {
                // Enable SSH button if one of the URLs uses SSH connection
                btSsh.Enabled = false;
                if (!ClassUtils.IsMono()) // Permanently disable SSH button if not on Windows OS
                {
                    if (_fetchUrl.Ok && _fetchUrl.Type == ClassUrl.UrlType.Ssh) btSsh.Enabled = true;
                    if (_pushUrl.Ok && _pushUrl.Type == ClassUrl.UrlType.Ssh) btSsh.Enabled = true;
                }
                // Enable HTTPS button if one of the URLs uses HTTPS connection
                btHttpsAuth.Enabled = false;
                if (_fetchUrl.Ok && _fetchUrl.Type == ClassUrl.UrlType.Https) btHttpsAuth.Enabled = true;
                if (_pushUrl.Ok && _pushUrl.Type == ClassUrl.UrlType.Https) btHttpsAuth.Enabled = true;

                btWWW1.Enabled = _fetchUrl.Ok;
                btWWW2.Enabled = _pushUrl.Ok;

                textPassword.ReadOnly = !(_fetchUrl.Type == ClassUrl.UrlType.Https || _pushUrl.Type == ClassUrl.UrlType.Https);
            }
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
        /// User clicked on the "web" button to the left of the git repo address.
        /// Find the canonical web site and open it
        /// </summary>
        private void BtWwwClick(object sender, EventArgs e)
        {
            string key = ((Button) sender).Tag.ToString();
            ClassUrl.Url url = key == "Fetch" ? _fetchUrl : _pushUrl;
            // Find a generic host name
            string target = "http://" + url.Host;
            // Detect some special hosts for which we can form a complete path
            if (url.Host.Contains("github"))
                target += "/" + url.Path;
            if (url.Host.Contains(".code.sf.net"))
                target = "https://sourceforge.net/projects/" + url.Name;
            ClassUtils.OpenWebLink(target);
        }

        /// <summary>
        /// Checks the given web target link and returns true if it is reasonably valid URL
        /// </summary>
        private bool isValidUrl(string target)
        {
            ClassUrl.Url url = ClassUrl.Parse(target.Trim());
            return url.Ok;
        }

        /// <summary>
        /// User clicked on the fetch help button, open a context dialog to select from the existing URLs
        /// </summary>
        private void BtFetchClicked(object sender, EventArgs e)
        {
            _menuFetch.Items.Clear();
            // Pick up a list of existing remote URLs and show them in the pull-down list
            List<string> hintUrls = App.Repos.GetRemoteUrls();
            foreach (var hintUrl in hintUrls)
                _menuFetch.Items.Add(hintUrl);
            _menuFetch.Show(btListFetch, new Point(0, btListFetch.Height));
        }

        /// <summary>
        /// User clicked on the push help button, open a context dialog to select from the existing URLs
        /// </summary>
        private void BtPushClicked(object sender, EventArgs e)
        {
            _menuPush.Items.Clear();
            // Pick up a list of existing remote URLs and show them in the pull-down list
            List<string> hintUrls = App.Repos.GetRemoteUrls();
            foreach (var hintUrl in hintUrls)
                _menuPush.Items.Add(hintUrl);
            _menuPush.Show(btListPush, new Point(0, btListPush.Height));
        }

        /// <summary>
        /// Item menu handler for the fetch help button
        /// </summary>
        void MenuFetchItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            textUrlFetch.Text = e.ClickedItem.Text;
        }

        /// <summary>
        /// Item menu handler for the push help button
        /// </summary>
        void MenuPushItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            textUrlPush.Text = e.ClickedItem.Text;
        }

        /// <summary>
        /// User clicked on the HTTPS authentication button, open a dialog to enter HTTPS credentials
        /// </summary>
        private void BtHttpsClicked(object sender, EventArgs e)
        {
            FormHttps formHttps = new FormHttps();
            formHttps.ShowDialog();
        }
    }
}
