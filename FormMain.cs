using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using git4win.FormMain_RightPanels;
using git4win.FormMain_LeftPanels;

namespace git4win
{
    public partial class FormMain : Form
    {
        // Left panels
        private static readonly PanelView PanelView = new PanelView();

        // Right panels
        private static readonly PanelRepos PanelRepos = new PanelRepos();
        private static readonly PanelCommits PanelCommits = new PanelCommits();
        private static readonly PanelRevlist PanelRevlist = new PanelRevlist();
        private static readonly PanelBranches PanelBranches = new PanelBranches();

        private static readonly Dictionary<string, UserControl> PanelsR = new Dictionary<string, UserControl> {
            { "Repos", PanelRepos },
            { "Commits", PanelCommits },
            { "Revisions", PanelRevlist },
            { "Branches", PanelBranches },
        };

        /// <summary>
        /// This is the main entry point to the application main form. Doing all the initialization here.
        /// </summary>
        public FormMain()
        {
            InitializeComponent();

            // Restore the application's initial position and size
            if (WindowState == FormWindowState.Normal)
            {
                Location = Properties.Settings.Default.FormMainLocation;
                Size = Properties.Settings.Default.FormMainClientSize;
            }

            // Initialize panels
            // Left pane:
            splitContainer2.Panel1.Controls.Add(PanelView);
            PanelView.Dock = DockStyle.Fill;

            // Right set of panes:
            foreach (UserControl control in PanelsR.Select(uc => uc.Value))
            {
                splitContainer2.Panel2.Controls.Add(control);
                control.Dock = DockStyle.Fill;
            }
            // Current right panel view is kept in Properties.Settings.Default.viewRightPanel (string)
            ChangeRightPanel(Properties.Settings.Default.viewRightPanel);

            // Form Execute is already created but it is hidden. Show it depending on the last state
            // which is kept in Properties.Settings.Default.ShowExecuteWindow (bool)
            menuViewExecuteWindow.Checked = Properties.Settings.Default.ShowExecuteWindow;
            App.Execute.Show(menuViewExecuteWindow.Checked);
            // We prevent Execute form closing by getting a FormClosing event within which we set "e.Cancel" to True
            App.Execute.FormClosing += MenuShowExecuteWindow;

            // Add all callback handlers
            App.Log = LogFunction;              // Log, to add strings to the app log window
            App.Refresh += FormMainRefresh;     // Refresh, when any component wants to update the global state
            App.StatusInfo += SetInfo;          // Info message, to update status bar on the bottom of the app window
            App.StatusBusy += SetBusy;          // Busy flag set or reset

            // Load all repos. This will also call the global refresh to update all panes.
            App.Repos.Load();

            // If there is no current repo, switch the right panel view to Repos
            if (App.Repos.Current == null)
                ChangeRightPanel("Repos");
        }

        /// <summary>
        /// Capture the notification of a custom message sent by a new instance of this application,
        /// Based on:
        /// http://www.sanity-free.com/143/csharp_dotnet_single_instance_application.html
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WmShowme)
                ShowMe();
            base.WndProc(ref m);
        }

        /// <summary>
        /// Bring this form to the top of all other windows
        /// </summary>
        private void ShowMe()
        {
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;
            bool top = TopMost;     // Get our current "TopMost" value (ours will always be false though)
            TopMost = true;         // Make our form jump to the top of everything
            TopMost = top;          // Set it back to whatever it was
        }

        /// <summary>
        /// Form is closing
        /// </summary>
        private void FormMainFormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.FormMainLocation = Location;
                Properties.Settings.Default.FormMainClientSize = Size;
            }
            App.Repos.Save();
        }

        /// <summary>
        /// Exit command - route to form closing handler
        /// </summary>
        private void MenuExit(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Main File menu drop down
        /// </summary>
        private void MenuMainFileDropDownOpening(object sender, EventArgs e)
        {
            menuMainFile.DropDownItems.Clear();
            menuMainFile.DropDownItems.AddRange(PanelView.GetContextMenu(menuMainFile.DropDown, string.Empty));

            ToolStripMenuItem mExit = new ToolStripMenuItem("Exit", null, MenuExit, Keys.Alt | Keys.F4);

            menuMainFile.DropDownItems.AddRange(new ToolStripItem[] { new ToolStripSeparator(), mExit });
        }

        /// <summary>
        /// Menu "View" has been opened. Set the bullet next to the current view mode.
        /// </summary>
        private void MenuMainViewDropDownOpened(object sender, EventArgs e)
        {
            int mode = Properties.Settings.Default.viewMode;

            // Set the correct bullet
            List<ToolStripMenuItem> viewMenus = new List<ToolStripMenuItem> {
                menuView0, menuView1, menuView2, menuView3, menuView4 };
            foreach (var m in viewMenus)
                m.Checked = false;
            viewMenus[mode].Checked = true;            
        }

        /// <summary>
        /// Set the view mode by sending a menu item whose Tag contains the mode number.
        /// This function is called from a menu handlers that select view mode.
        /// </summary>
        private void ViewSetByMenuItem(object sender, EventArgs e)
        {
            PanelView.ViewSetByMenuItem(sender, e);
        }

        /// <summary>
        /// Switch the view mode to Local File View and Local Pending Changelists.
        /// Needed to be reset that way after creating a new repo.
        /// </summary>
        public void ResetViews()
        {
            PanelView.SetView(3);
            ChangeRightPanel("Commits");
        }

        /// <summary>
        /// This function is called when user changes a right panel
        /// </summary>
        private void ChangeRightPanel(string panelName)
        {
            UserControl panel = PanelsR[panelName];
            panel.BringToFront();
            Properties.Settings.Default.viewRightPanel = panelName;
        }

        private void RightPanelSelectionClick(object sender, EventArgs e)
        {
            ChangeRightPanel((sender as ToolStripItem).Tag.ToString());
        }

        /// <summary>
        /// Log function called with a message to print into the log window
        /// For performance resons, if a message is too long (?), we trim it.
        /// </summary>
        private void LogFunction(string message)
        {
            // Trim down the message if it is too long
            if (message.Length > 160)
                message = message.Substring(0, 160);

            // Prepend the current time, if that option is requested, in either 12 or 24-hr format
            if (Properties.Settings.Default.logTime)
                message = DateTime.Now.ToString
                    (Properties.Settings.Default.logTime24 ? "HH:mm:ss" : "hh:mm:ss") + " "
                    + message;

            listStatus.Items.Add(message);
            listStatus.TopIndex = listStatus.Items.Count - 1;
        }

        /// <summary>
        /// Set the info message into the main form status line (located at the bottom)
        /// </summary>
        private void SetInfo(string infoMessage)
        {
            statusInfoLabel.Text = infoMessage;
        }

        /// <summary>
        /// Set or clear the busy flag (small "stop" icon on the toolbar)
        /// </summary>
        private void SetBusy(bool isBusy)
        {
            Cursor = isBusy ? Cursors.WaitCursor : Cursors.Default;
            btCancelOperation.Enabled = isBusy;
            Application.DoEvents();     // Give app the chance to redraw the icon
        }

        /// <summary>
        /// Refresh main form items based on the new repository
        /// </summary>
        private void FormMainRefresh()
        {
            // Change the window title and display the default remote name

            StringBuilder title = new StringBuilder("git4win ");

            if (PanelBranches.GetCurrent() != "")
                title.Append("- " + PanelBranches.GetCurrent());

            if (App.Repos.Current != null && App.Repos.Current.Remotes.Current != "")
                title.Append(" : " + App.Repos.Current.Remotes.Current);

            Text = title.ToString();

            // Build the menu with the list of remote repos
            menuMainPushToRemote.Enabled = menuMainPullFromRemote.Enabled = menuMainEditRemoteRepo.Enabled = menuMainSwitchRemoteRepo.Enabled = false;

            menuMainSwitchRemoteRepo.DropDownItems.Clear();
            if (App.Repos.Current != null && App.Repos.Current.Remotes.Current != "")
            {
                List<string> remotes = App.Repos.Current.Remotes.GetListNames();
                foreach (string s in remotes)
                {
                    ToolStripMenuItem m = new ToolStripMenuItem(s);
                    m.Checked = App.Repos.Current.Remotes.Current == s;
                    m.Click += RemoteChangedEvent;
                    menuMainSwitchRemoteRepo.DropDownItems.Add(m);
                }
                RemoteChangedEvent = RemoteChanged;

                menuMainPushToRemote.Enabled = menuMainPullFromRemote.Enabled = menuMainSwitchRemoteRepo.Enabled = true;
            }

            if (App.Repos.Current != null)
                menuMainEditRemoteRepo.Enabled = true;
        }

        /// <summary>
        /// Event handler delegate and the actual event handler for switching
        /// the remote repo. Called when user selects one of the pre-built menu items
        /// </summary>
        private event EventHandler RemoteChangedEvent;
        private void RemoteChanged(object sender, EventArgs e)
        {
            App.Repos.Current.Remotes.Current = sender.ToString();
            FormMainRefresh();
        }

        /// <summary>
        /// Settings menu selected
        /// </summary>
        private void MenuOptions(object sender, EventArgs e)
        {
            FormOptions frmOptions = new FormOptions();
            frmOptions.ShowDialog();
        }

        /// <summary>
        /// Refresh Active Pane, but refresh everything
        /// </summary>
        private void MenuRefreshAll(object sender, EventArgs e)
        {
            App.Refresh();
        }

        /// <summary>
        /// Edit the list of remote repositories
        /// </summary>
        private void MenuEditRemoteRepos(object sender, EventArgs e)
        {
            FormRemoteEdit remoteEdit = new FormRemoteEdit(App.Repos.Current);
            if (remoteEdit.ShowDialog() == DialogResult.OK)
                App.Refresh();
        }

        /// <summary>
        /// Pull from a remote repository
        /// </summary>
        private void MenuRepoPull(object sender, EventArgs e)
        {
            App.Repos.Current.Run("pull " + App.Repos.Current.Remotes.Current + " " + PanelBranches.GetCurrent());
        }

        /// <summary>
        /// Push to remote repository.
        /// Use either the standard form (example: "origin master"), or the user override
        /// </summary>
        private void MenuRepoPush(object sender, EventArgs e)
        {
            string pushCmd = App.Repos.Current.Remotes.GetPushCmd();
            if (String.IsNullOrEmpty(pushCmd))
                App.Repos.Current.Run("push " + App.Repos.Current.Remotes.Current + " " + PanelBranches.GetCurrent());
            else
                App.Repos.Current.Run("push " + pushCmd);
        }

        /// <summary>
        /// Toggle the execute window between Show and Hide states. This form should not
        /// be closed for the application depends on it for cmd execution.
        /// </summary>
        private void MenuShowExecuteWindow(object sender, EventArgs e)
        {
            bool @checked = menuViewExecuteWindow.Checked;
            Properties.Settings.Default.ShowExecuteWindow = menuViewExecuteWindow.Checked = !@checked;
            App.Execute.Show(!@checked);

            // We prevent Execute form closing by servicing a FormExecute.FormClosing event with this event,
            // so disable closure by setting Cancel to true only if the caller used that type of arguments
            if (e is FormClosingEventArgs)
                (e as FormClosingEventArgs).Cancel = true;
        }

        /// <summary>
        /// Create Repository menu drop down
        /// </summary>
        private void MenuMainRepositoryDropDownOpening(object sender, EventArgs e)
        {
            menuMainRepository.DropDownItems.Clear();
            menuMainRepository.DropDownItems.AddRange(PanelRepos.GetContextMenu(menuMainRepository.DropDown));

            ToolStripMenuItem mRepos = new ToolStripMenuItem("View Repos", null, RightPanelSelectionClick, Keys.F10);
            mRepos.Tag = "Repos";
            menuMainRepository.DropDownItems.AddRange(new ToolStripItem[] { new ToolStripSeparator(), mRepos });            
        }

        /// <summary>
        /// Create Branches menu drop down
        /// </summary>
        private void MenuMainBranchDropDownOpening(object sender, EventArgs e)
        {
            menuMainBranch.DropDownItems.Clear();
            menuMainBranch.DropDownItems.AddRange(PanelBranches.GetContextMenu(menuMainBranch.DropDown, null));

            ToolStripMenuItem mRefresh = new ToolStripMenuItem("View Branches", null, RightPanelSelectionClick, Keys.F8);
            mRefresh.Tag = "Branches";
            menuMainBranch.DropDownItems.AddRange(new ToolStripItem[] { new ToolStripSeparator(), mRefresh });
        }

        /// <summary>
        /// Show the standard About box
        /// </summary>
        private void AboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            FormAbout about = new FormAbout();
            about.ShowDialog();
        }

        /// <summary>
        /// Changelist menu drop down
        /// </summary>
        private void MenuMainChangelistDropDownOpening(object sender, EventArgs e)
        {
            menuMainChangelist.DropDownItems.Clear();
            menuMainChangelist.DropDownItems.AddRange(PanelCommits.GetContextMenu(menuMainChangelist.DropDown, null));

            ToolStripMenuItem mPending = new ToolStripMenuItem("View Pending Changelists", null, RightPanelSelectionClick, Keys.F6);
            ToolStripMenuItem mSubmitted = new ToolStripMenuItem("View Submitted Changelists", null, RightPanelSelectionClick, Keys.F7);
            mPending.Tag = "Commits";
            mSubmitted.Tag = "Revisions";
            menuMainChangelist.DropDownItems.AddRange(new ToolStripItem[] { new ToolStripSeparator(), mPending, mSubmitted });
        }

        /// <summary>
        /// Select all files on the left pane
        /// </summary>
        private void MenuMainSelectAllClick(object sender, EventArgs e)
        {
            PanelView.SelectAll();
        }

        #region Status menu handlers

        /// <summary>
        /// Right-click on the status menu selected Copy command
        /// </summary>
        private void MenuStatusCopyClick(object sender, EventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (int i in listStatus.SelectedIndices)
                buffer.Append(listStatus.Items[i]).Append(Environment.NewLine);
            if (buffer.Length > 0)
                Clipboard.SetText(buffer.ToString());
        }

        /// <summary>
        /// Right-click on the status menu selected Select-All command
        /// </summary>
        private void MenuStatusSelectAllClick(object sender, EventArgs e)
        {
            for (int i = 0; i < listStatus.Items.Count; i++)
                listStatus.SetSelected(i, true);
        }

        /// <summary>
        /// Right-click on the status menu selected Clear command
        /// </summary>
        private void MenuSelectClearClick(object sender, EventArgs e)
        {
            listStatus.Items.Clear();
        }

        #endregion

        /// <summary>
        /// Manage PuTTY Keys
        /// </summary>
        private void MenuMainManageKeysClick(object sender, EventArgs e)
        {
            FormSSH formSsh = new FormSSH();
            formSsh.ShowDialog();
        }
    }
}
