using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Git4Win.Main.Left.Panels;
using Git4Win.Main.Right.Panels;

namespace Git4Win
{
    public partial class FormMain : Form
    {
        #region Initialization

        /// <summary>
        /// Location of the repos data file containing a list of repositories.
        /// </summary>
        private readonly string _reposDataFile;

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

        // Left panels
        private static readonly PanelView PanelView = new PanelView();

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
                Size = Properties.Settings.Default.FormMainSize;
            }

            // WAR: On Linux, remove status bar resizing grip (since it does not work under X)
            if (ClassUtils.IsMono())
                statusStrip.SizingGrip = false;

            // Initialize panels
            // Left panel:
            splitContainer2.Panel1.Controls.Add(PanelView);
            PanelView.Dock = DockStyle.Fill;

            // Right set of panels:
            foreach (UserControl control in PanelsR.Select(uc => uc.Value))
            {
                splitContainer2.Panel2.Controls.Add(control);
                control.Dock = DockStyle.Fill;
            }

            // Enable SSH only if the PuTTY support class has been instantiated
            if (App.Putty != null)
            {
                btSsh.Enabled = true;
                menuMainManageKeys.Enabled = true;
            }

            // We prevent Execute form closing by getting a FormClosing event within which we set "e.Cancel" to True
            App.Log.FormClosing += LogWindowToolStripMenuItemClick;
            menuViewLogWindow.Checked = Properties.Settings.Default.ShowLogWindow;
			
            // Add all callback handlers
            App.Refresh += FormMainRefresh;         // Refresh, when any component wants to update the global state
            App.PrintStatusMessage += PrintStatus;  // Print a line of status message
            App.StatusBusy += SetBusy;              // Busy flag set or reset

            // Register toolbar file buttons with the View panel
            PanelView.RegisterToolstripFileButtons(new Dictionary
                <PanelView.FileOps, ToolStripButton>()
                {
                    { PanelView.FileOps.Add, btAdd },
                    { PanelView.FileOps.Update, btUpdate },
                    { PanelView.FileOps.UpdateAll, btUpdateAll },
                    { PanelView.FileOps.Revert, btRevert },
                    { PanelView.FileOps.Delete, btDelete },
                    { PanelView.FileOps.DeleteFs, btDeleteFs },
                    { PanelView.FileOps.Edit, btEdit }
                });

            // Load default set of repositories
            _reposDataFile = Path.Combine(App.AppHome, "repos.dat");
            App.Repos.WorkspaceLoad(_reposDataFile);

            // If there is no current repo, switch the right panel view to Repos
            // Otherwise, restore the last view panel
            ChangeRightPanel(App.Repos.Current == null ? 
                "Repos" : 
                Properties.Settings.Default.viewRightPanel);

            // In the status label, initially show the version
            statusInfoLabel.Text = String.Format("Version {0}, {1}",
                Assembly.GetExecutingAssembly().GetName().Version,
                new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime.ToLongTimeString());

            // Initiate the first global refresh
            App.Refresh();
        }

        /// <summary>
        /// Form is closing
        /// </summary>
        private void FormMainFormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.FormMainLocation = Location;
                Properties.Settings.Default.FormMainSize = Size;
            }

            // Save windows geometry database
            ClassWinGeometry.SaveGeometryDatabase();

            // Save current workspace
            App.Repos.WorkspaceSave(_reposDataFile);
        }

        /// <summary>
        /// Exit command - route to form closing handler
        /// </summary>
        private void MenuExit(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        /// <summary>
        /// Main File menu drop down
        /// </summary>
        private void MenuMainFileDropDownOpening(object sender, EventArgs e)
        {
            menuMainFile.DropDownItems.Clear();
            menuMainFile.DropDownItems.AddRange(PanelView.GetContextMenu(menuMainFile.DropDown));

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
        /// Print into the status pane (and the aux log window)
        /// </summary>
        private void PrintStatus(string message)
        {
            // Prepend the current time, if that option is requested, in either 12 or 24-hr format
            if (Properties.Settings.Default.logTime)
                message = DateTime.Now.ToString
                    (Properties.Settings.Default.logTime24 ? "HH:mm:ss" : "hh:mm:ss") + " "
                    + message;

            listStatus.Items.Add(message);
            listStatus.TopIndex = listStatus.Items.Count - 1;

            App.Log.Print(message);
        }

        /// <summary>
        /// Set the info message into the main form status line (located at the bottom)
        /// </summary>
        public void SetStatusText(string infoMessage)
        {
            statusInfoLabel.Text = infoMessage;
        }

        /// <summary>
        /// Set or clear the busy flag (small "stop" icon on the toolbar)
        /// Use timer to turn the GUI elements off to avoid rapid on/off cycles
        /// caused by a number of successive toggles. Store the true state of busy-ness
        /// in the timerTick.Tag, so when the tick triggers, it will know what was the
        /// true busy state.
        /// </summary>
        private void SetBusy(bool isBusy)
        {
            // If the signal is to clear the busy flag, arm the timer to do it few ms later
            if( isBusy==false)
            {
                timerBusy.Interval = 300;
                timerBusy.Enabled = true;
            }
            else
            {
                Cursor = Cursors.WaitCursor;
                btCancelOperation.Enabled = true;
                Application.DoEvents();     // Give app the chance to redraw the icon
            }
            timerBusy.Tag = isBusy;
        }

        /// <summary>
        /// Busy signal timer tick handler
        /// </summary>
        private void TimerBusyTick(object sender, EventArgs e)
        {
            if((bool)timerBusy.Tag==false)
            {
                Cursor = Cursors.Default;
                btCancelOperation.Enabled = false;
                timerBusy.Enabled = false;
                Application.DoEvents();     // Give app the chance to redraw the icon
            }
        }

        /// <summary>
        /// Refresh main form items based on the new repository
        /// </summary>
        private void FormMainRefresh()
        {
            // Change the window title and display the default remote name

            StringBuilder title = new StringBuilder("Git4Win ");

            if (PanelBranches.GetCurrent() != "")
                title.Append("- " + PanelBranches.GetCurrent());

            if (App.Repos.Current != null && App.Repos.Current.Remotes.Current != "")
                title.Append(" : " + App.Repos.Current.Remotes.Current);

            Text = title.ToString();

            // Build the menu with the list of remote repos
            menuMainPushToRemote.Enabled = menuMainPullFromRemote.Enabled = menuMainEditRemoteRepo.Enabled = menuMainSwitchRemoteRepo.Enabled = false;
            btPull.Enabled = btPush.Enabled = false;

            menuMainSwitchRemoteRepo.DropDownItems.Clear();
            if (App.Repos.Current != null && App.Repos.Current.Remotes.Current != "")
            {
                List<string> remotes = App.Repos.Current.Remotes.GetListNames();
                foreach (string s in remotes)
                {
                    // Create a new menu items for each remote repository
                    ToolStripMenuItem m = new ToolStripMenuItem(s, null, RemoteChanged) {Checked = false};
                    menuMainSwitchRemoteRepo.DropDownItems.Add(m);

                    // For the current repository, add a checkmark and enable corresponding
                    // menus for push and pull both in the menu and tool box buttons
                    if (App.Repos.Current.Remotes.Current == s)
                    {
                        m.Checked = true;

                        if (!string.IsNullOrEmpty(App.Repos.Current.Remotes.Get(s).UrlPush))
                            menuMainPushToRemote.Enabled = btPush.Enabled = true;

                        if (!string.IsNullOrEmpty(App.Repos.Current.Remotes.Get(s).UrlFetch))
                            menuMainPullFromRemote.Enabled = btPull.Enabled = true;
                    }
                }
                menuMainSwitchRemoteRepo.Enabled = true;
            }

            if (App.Repos.Current != null)
                menuMainEditRemoteRepo.Enabled = true;
        }

        /// <summary>
        /// Switch the remote repo. The new repo name is given as sender name.
        /// </summary>
        private void RemoteChanged(object sender, EventArgs e)
        {
            PrintStatus("Changed remote repository to " + sender);
            App.Repos.Current.Remotes.Current = sender.ToString();
            FormMainRefresh();
        }

        /// <summary>
        /// Settings menu selected
        /// </summary>
        private void MenuOptions(object sender, EventArgs e)
        {
            FormSettings frmOptions = new FormSettings();
            frmOptions.ShowDialog();
        }

        /// <summary>
        /// Refresh Active Pane (do a global refresh for now)
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
            string args = App.Repos.Current.Remotes.Current + " " + PanelBranches.GetCurrent();
            PrintStatus("Pull from a remote repo: " + args);
            App.Repos.Current.Run("pull " + args);
        }

        /// <summary>
        /// Push to remote repository.
        /// Use either the standard form (example: "origin master"), or the user override
        /// </summary>
        private void MenuRepoPush(object sender, EventArgs e)
        {
            string args = App.Repos.Current.Remotes.GetPushCmd("");
            if (String.IsNullOrEmpty(args))
                args = App.Repos.Current.Remotes.Current + " " + PanelBranches.GetCurrent();
            PrintStatus("Push to a remote repo: " + args);
            App.Repos.Current.Run("push " + args);
        }

        /// <summary>
        /// Toggle the execute window between Show and Hide states. This form should not
        /// be closed as it accumulates all log messages even when hidden.
        /// </summary>
        private void LogWindowToolStripMenuItemClick(object sender, EventArgs e)
        {
            bool @checked = menuViewLogWindow.Checked;
            Properties.Settings.Default.ShowLogWindow = menuViewLogWindow.Checked = !@checked;
            App.Log.Show(!@checked);

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

            ToolStripMenuItem mRepos = new ToolStripMenuItem("View Repos", null, RightPanelSelectionClick, Keys.F10) {Tag = "Repos"};
            menuMainRepository.DropDownItems.AddRange(new ToolStripItem[] { mRepos });
        }

        /// <summary>
        /// Create Branches menu drop down
        /// </summary>
        private void MenuMainBranchDropDownOpening(object sender, EventArgs e)
        {
            menuMainBranch.DropDownItems.Clear();
            menuMainBranch.DropDownItems.AddRange(PanelBranches.GetContextMenu(menuMainBranch.DropDown, null));

            ToolStripMenuItem mRefresh = new ToolStripMenuItem("View Branches", null, RightPanelSelectionClick, Keys.F8) {Tag = "Branches"};
            menuMainBranch.DropDownItems.AddRange(new ToolStripItem[] { new ToolStripSeparator(), mRefresh });
        }

        /// <summary>
        /// Changelist menu drop down
        /// </summary>
        private void MenuMainChangelistDropDownOpening(object sender, EventArgs e)
        {
            // Add the menu items from the commit pane followed menu items from the revisions pane
            menuMainChangelist.DropDownItems.Clear();
            menuMainChangelist.DropDownItems.AddRange(PanelCommits.GetContextMenu(menuMainChangelist.DropDown, null));

            // Add the revision list menu only if the revlist right pane is active
            if (Properties.Settings.Default.viewRightPanel == "Revisions")
                menuMainChangelist.DropDownItems.AddRange(PanelRevlist.GetContextMenu(menuMainChangelist.DropDown));

            ToolStripMenuItem mPending = new ToolStripMenuItem("View Pending Changelists", null, RightPanelSelectionClick, Keys.F6) {Tag = "Commits"};
            ToolStripMenuItem mSubmitted = new ToolStripMenuItem("View Submitted Changelists", null, RightPanelSelectionClick, Keys.F7) {Tag = "Revisions"};
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
        /// Manage SSH Keys
        /// </summary>
        private void MenuMainManageKeysClick(object sender, EventArgs e)
        {
            FormSSH formSsh = new FormSSH();
            formSsh.ShowDialog();
        }

        /// <summary>
        /// User clicked on a Website menu item, open the website
        /// </summary>
        private void WebsiteToolStripMenuItemClick(object sender, EventArgs e)
        {
            Process.Start((sender as ToolStripMenuItem).Tag.ToString());
        }

        /// <summary>
        /// User clicked on a User's Manual menu item, open the embedded PDF
        /// </summary>
        private void UsersManualToolStripMenuItemClick(object sender, EventArgs e)
        {
            string appPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
// TODO: Users manual
//            appPath = ClassUtils.WriteResourceToFile(appPath, "Git4WinUsersManual.pdf", Properties.Resources.Git4WinUsersManual);
//            Process.Start(appPath);
        }

        /// <summary>
        /// Stops any currently executing git command thread
        /// </summary>
        private void BtCancelOperationClick(object sender, EventArgs e)
        {
            ClassExecute.KillJob();
        }

        /// <summary>
        /// Show the standard About box
        /// </summary>
        private void AboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            FormAbout about = new FormAbout();
            about.ShowDialog();
        }
    }
}
