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
using GitForce.Main.Left.Panels;
using GitForce.Main.Right.Panels;

namespace GitForce
{
    public partial class FormMain : Form
    {
        #region Initialization

        // This simply registers repo's status refresh function at the head of the refresh chain
        private static readonly ClassStatus Status = new ClassStatus();

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
        /// Flags to use when calling SelectiveRefresh function
        /// </summary>
        [Flags]
        public enum SelectveRefreshFlags
        {
            View = 1,
            Repos = 2,
            Commits = 4,
            Revisions = 8,
            Branches = 16,
            All = 31
        }

        // Path to the default custom tools file
        private static readonly string DefaultCustomToolsFile = Path.Combine(App.AppHome, "CustomTools.xml");

        /// <summary>
        /// This is the main entry point to the application main form. Doing all the initialization here.
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

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

            // Show or hide command line
            menuViewCommandLine.Checked = cmdBox.Visible = Properties.Settings.Default.ShowCommandLine;

            // Enable SSH only if the PuTTY support class has been instantiated
            if (App.Putty != null)
            {
                btSsh.Enabled = true;
                menuMainManageKeys.Enabled = true;
            }

            // We prevent Log window form closing by getting a FormClosing event within which we set "e.Cancel" to True
            App.Log.FormClosing += LogWindowToolStripMenuItemClick;
            menuViewLogWindow.Checked = Properties.Settings.Default.ShowLogWindow;
			
            // Add all callback handlers
            App.Refresh += FormMainRefresh;         // Refresh, when any component wants to update the global state
            App.PrintStatusMessage += PrintStatus;  // Print a line of status message
            App.StatusBusy += SetBusy;              // Busy flag set or reset

            // Register toolbar file buttons with the View panel
            PanelView.RegisterToolstripFileButtons(new Dictionary
                <PanelView.FileOps, ToolStripButton>
                {
                    { PanelView.FileOps.Add, btAdd },
                    { PanelView.FileOps.Update, btUpdate },
                    { PanelView.FileOps.UpdateAll, btUpdateAll },
                    { PanelView.FileOps.Revert, btRevert },
                    { PanelView.FileOps.Delete, btDelete },
                    { PanelView.FileOps.DeleteFs, btDeleteFs },
                    { PanelView.FileOps.Edit, btEdit }
                });

            PrintStatus("GitForce version " + ClassVersion.GetVersion());

            // Load default set of repositories
            ClassWorkspace.Load(null);

            // Load custom tools
            App.CustomTools = ClassCustomTools.Load(DefaultCustomToolsFile);

            // If there is no current repo, switch the right panel view to Repos
            // Otherwise, restore the last view panel
            ChangeRightPanel(App.Repos.Current == null ? 
                "Repos" : 
                Properties.Settings.Default.viewRightPanel);

            // Initiate the first global refresh
            App.DoRefresh();
        }

        /// <summary>
        /// Form is closing
        /// </summary>
        private void FormMainFormClosing(object sender, FormClosingEventArgs e)
        {
            // Remove the print status handler
            App.PrintStatusMessage -= PrintStatus;

            // Store geometry of _this_ window
            ClassWinGeometry.Save(this);

            // Save custom tools to their default location
            App.CustomTools.Save(DefaultCustomToolsFile);

            // Close the log windown manually in order to save its geometry
            App.Log.FormClosing -= LogWindowToolStripMenuItemClick;
            App.Log.Close();

            // Save windows geometry database
            ClassWinGeometry.SaveGeometryDatabase();

            // Save current workspace
            ClassWorkspace.Save(null);

            // Remove all outstanding temp files
            ClassGlobals.RemoveTempFiles();
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

            // Add the workspace menu items
            ToolStripMenuItem mWkClear = new ToolStripMenuItem("Clear Workspace", null, WorkspaceClearMenuItem);
            ToolStripMenuItem mWkLoad = new ToolStripMenuItem("Load Workspace...", null, WorkspaceLoadMenuItem);
            ToolStripMenuItem mWkSave = new ToolStripMenuItem("Save Workspace As...", null, WorkspaceSaveMenuItem);
            ToolStripMenuItem mWkLru = new ToolStripMenuItem("Recent Workspaces", null, WorkspaceLoadLruMenuItem);
            ToolStripMenuItem mExit = new ToolStripMenuItem("Exit", null, MenuExit, Keys.Alt | Keys.F4);

            // Fill in the last recently used workspace list of items
            List<string> lru = ClassWorkspace.GetLRU();
            foreach (var file in lru)
                mWkLru.DropDownItems.Add(new ToolStripMenuItem(file, null, WorkspaceLoadLruMenuItem) { Tag = file });
            mWkLru.Enabled = lru.Count > 0;
            mWkSave.Enabled = App.Repos.Current != null;

            menuMainFile.DropDownItems.AddRange(new ToolStripItem[] {
                    new ToolStripSeparator(), 
                    mWkClear, mWkLoad, mWkSave, mWkLru,
                    new ToolStripSeparator(),
                    mExit });
        }

        /// <summary>
        /// Clear current workspace
        /// </summary>
        private void WorkspaceClearMenuItem(object sender, EventArgs e)
        {
            // Save existing workspace before zapping it
            if(MessageBox.Show("Current workspace will be cleared from all git repositories. Continue?", 
                "Clear Workspace", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)==DialogResult.Yes)
            {
                if (ClassWorkspace.Save(null))
                    ClassWorkspace.Clear();
            }
        }

        /// <summary>
        /// Select and load a specific workspace
        /// </summary>
        private void WorkspaceLoadMenuItem(object sender, EventArgs e)
        {
            if (loadWk.ShowDialog() == DialogResult.OK)
            {
                // Save existing workspace before trying to load a new one
                if (ClassWorkspace.Save(null))
                    if (ClassWorkspace.Load(loadWk.FileName))
                        App.DoRefresh();
            }
        }

        /// <summary>
        /// Load a workspace selected by the last recently used menu
        /// </summary>
        private void WorkspaceLoadLruMenuItem(object sender, EventArgs e)
        {
            string name = (sender as ToolStripMenuItem).Tag as string;

            // Save existing workspace before trying to load a new one
            if (ClassWorkspace.Save(null))
                if (ClassWorkspace.Load(name))
                    App.DoRefresh();
        }

        /// <summary>
        /// Save current workspace
        /// </summary>
        private void WorkspaceSaveMenuItem(object sender, EventArgs e)
        {
            if(saveWk.ShowDialog()==DialogResult.OK)
                ClassWorkspace.Save(saveWk.FileName);
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
        /// Print into the status pane (and the aux log window).
        /// It is ok to send null or empty string.
        /// Strings that contain Newline will be broken into separate lines.
        /// This function is thread-safe.
        /// </summary>
        private void PrintStatus(string message)
        {
            if(string.IsNullOrEmpty(message))
                return;
            if (listStatus.InvokeRequired)
                listStatus.BeginInvoke((MethodInvoker)(() => PrintStatus(message)));
            else
            {
                // Add each line of the message individually
                foreach (string line in message.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    // Prepend the current time, if that option is requested, in either 12 or 24-hr format
                    string stamp = Properties.Settings.Default.logTime
                                   ? DateTime.Now.ToString(Properties.Settings.Default.logTime24
                                   ? "HH:mm:ss"
                                   : "hh:mm:ss") + " "
                                   : "";
                    listStatus.Items.Add(stamp + line);
                }
                listStatus.TopIndex = listStatus.Items.Count - 1;

                App.PrintLogMessage(message);
            }
        }

        /// <summary>
        /// Set the info message into the main form status line (located at the bottom)
        /// </summary>
        public void SetStatusText(string infoMessage)
        {
            statusInfoLabel.Text = infoMessage;
        }

        /// <summary>
        /// Set the window title
        /// </summary>
        public void SetTitle(string title)
        {
            // Set the title and wait one time slice for it to be painted
            Text = title;
            Application.DoEvents();
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
        /// Refresh main form items, part of the global refresh chain
        /// </summary>
        private void FormMainRefresh()
        {
            // Change the window title and display the default remote name
            StringBuilder title = new StringBuilder("GitForce ");

            // Enable new version button if there is a new version available
            btNewVersion.Visible = App.Version.NewVersionAvailable;

            menuMainStash.Enabled = menuMainUnstash.Enabled = false;

            // Do specific enables based on the availability of the current repo
            if (App.Repos.Current != null)
            {
                menuMainStash.Enabled = menuMainUnstash.Enabled = true;

                title.Append("- " + App.Repos.Current.Branches.Current);

                if (App.Repos.Current.Remotes.Current != "")
                    title.Append(" : " + App.Repos.Current.Remotes.Current);
            }

            Text = title.ToString();

            // Build the menu with the list of remote repos
            menuMainPushToRemote.Enabled = menuMainPullFromRemote.Enabled = menuMainFetchFromRemote.Enabled = 
                menuMainEditRemoteRepo.Enabled = menuMainSwitchRemoteRepo.Enabled = false;
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
                            menuMainPullFromRemote.Enabled = menuMainFetchFromRemote.Enabled = btPull.Enabled = true;
                    }
                }
                menuMainSwitchRemoteRepo.Enabled = true;
            }

            if (App.Repos.Current != null)
                menuMainEditRemoteRepo.Enabled = true;
        }

        /// <summary>
        /// Selectively refreshes only specified panels
        /// </summary>
        public void SelectiveRefresh(SelectveRefreshFlags flags)
        {
            // Always refresh the class status first
            ClassStatus.Refresh();

            if ((flags & SelectveRefreshFlags.View) == SelectveRefreshFlags.View)
                PanelView.ViewRefresh();

            if ((flags & SelectveRefreshFlags.Repos) == SelectveRefreshFlags.Repos)
                PanelRepos.ReposRefresh();

            if ((flags & SelectveRefreshFlags.Commits) == SelectveRefreshFlags.Commits)
                PanelCommits.CommitsRefresh();

            if ((flags & SelectveRefreshFlags.Revisions) == SelectveRefreshFlags.Revisions)
                PanelRevlist.RevlistRefresh();

            if ((flags & SelectveRefreshFlags.Branches) == SelectveRefreshFlags.Branches)
                PanelBranches.BranchesRefresh();
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
            App.DoRefresh();
        }

        /// <summary>
        /// Edit the list of remote repositories
        /// </summary>
        private void MenuEditRemoteRepos(object sender, EventArgs e)
        {
            FormRemoteEdit remoteEdit = new FormRemoteEdit(App.Repos.Current);
            if (remoteEdit.ShowDialog() == DialogResult.OK)
                App.DoRefresh();
        }

        /// <summary>
        /// Fetch from a remote repository
        /// </summary>
        private void MenuRepoFetch(object sender, EventArgs e)
        {
            string args = App.Repos.Current.Remotes.Current + " " + App.Repos.Current.Branches.Current;
            PrintStatus("Fetch from a remote repo: " + args);
            App.Repos.Current.RunCmd("fetch " + args);
        }

        /// <summary>
        /// Pull from a remote repository
        /// </summary>
        private void MenuRepoPull(object sender, EventArgs e)
        {
            string args = App.Repos.Current.Remotes.Current + " " + App.Repos.Current.Branches.Current;
            PrintStatus("Pull from a remote repo: " + args);
            App.Repos.Current.RunCmd("pull " + args);
        }

        /// <summary>
        /// Push to remote repository.
        /// Use either the standard form (example: "origin master"), or the user override
        /// </summary>
        private void MenuRepoPush(object sender, EventArgs e)
        {
            string args = App.Repos.Current.Remotes.GetPushCmd("");
            if (String.IsNullOrEmpty(args))
                args = App.Repos.Current.Remotes.Current + " " + App.Repos.Current.Branches.Current;
            PrintStatus("Push to a remote repo: " + args);
            App.Repos.Current.RunCmd("push " + args);
        }

        /// <summary>
        /// Toggle the execute window between Show and Hide states. This form should not
        /// be closed as it accumulates all log messages even when hidden.
        /// </summary>
        private void LogWindowToolStripMenuItemClick(object sender, EventArgs e)
        {
            bool @checked = menuViewLogWindow.Checked;
            Properties.Settings.Default.ShowLogWindow = menuViewLogWindow.Checked = !@checked;
            App.Log.ShowWindow(!@checked);

            // We prevent Execute form closing by servicing a FormExecute.FormClosing event with this event,
            // so disable closure by setting Cancel to true only if the caller used that type of arguments
            if (e is FormClosingEventArgs)
                (e as FormClosingEventArgs).Cancel = true;
        }

        /// <summary>
        /// Toggle the Command Line edit box between Show and Hide states.
        /// </summary>
        private void MenuViewCommandLineClick(object sender, EventArgs e)
        {
            bool @checked = menuViewCommandLine.Checked;
            Properties.Settings.Default.ShowCommandLine = menuViewCommandLine.Checked = !@checked;
            cmdBox.Visible = !@checked;

            // This is purely cosmetic: pushes the list pane text up to cleanly reveal the tail
            listStatus.TopIndex = listStatus.Items.Count - 1;
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
            menuMainBranch.DropDownItems.AddRange(PanelBranches.GetContextMenu(menuMainBranch.DropDown));

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
        private void WebsiteClick(object sender, EventArgs e)
        {
            Process.Start((sender as ToolStripMenuItem).Tag.ToString());
        }

        /// <summary>
        /// Stops any currently executing git command thread
        /// </summary>
        private void BtCancelOperationClick(object sender, EventArgs e)
        {
            // TODO: Does this control get the thread to run on when a command is executing?
            // ClassExecute.KillJob();
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
        /// User clicked on the Stash menu item
        /// </summary>
        private void MenuMainStashClick(object sender, EventArgs e)
        {
            FormStash formStash = new FormStash();
            if (formStash.ShowDialog() == DialogResult.OK)
                App.DoRefresh();                
        }

        /// <summary>
        /// User clicked on the Unstash menu item
        /// </summary>
        private void MenuMainUnstashClick(object sender, EventArgs e)
        {
            FormUnstash formUnstash = new FormUnstash();
            if (formUnstash.ShowDialog() == DialogResult.OK)
                App.DoRefresh();
        }

        #region Custom Tools menu handlers

        /// <summary>
        /// Custom Tools menu drop down, build the menu
        /// </summary>
        private void MenuMainToolsDropDownOpening(object sender, EventArgs e)
        {
            // Add menu items that are always there, following by tools items
            menuMainTools.DropDownItems.Clear();
            menuMainTools.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItem("Customize", null, CustomizeToolMenuItemClick),
                new ToolStripMenuItem("Import", null, ImportToolMenuItemClick),
                new ToolStripMenuItem("Export", null, ExportToolMenuItemClick) });

            // Add all custom tools to the tools menu
            if(App.CustomTools.Tools.Count>0)
            {
                menuMainTools.DropDownItems.Add(new ToolStripSeparator());
                foreach (var tool in App.CustomTools.Tools)
                    menuMainTools.DropDownItems.Add(new ToolStripMenuItem(tool.Name, null, CustomToolClicked) {Tag = tool});
            }
        }

        /// <summary>
        /// User clicked on the Tools' Customize menu item
        /// </summary>
        private void CustomizeToolMenuItemClick(object sender, EventArgs e)
        {
            FormCustomizeTools formCustomizeTools = new FormCustomizeTools(App.CustomTools);
            if (formCustomizeTools.ShowDialog() == DialogResult.OK)
                App.CustomTools = formCustomizeTools.CustomTools.Copy();
        }

        /// <summary>
        /// Import a set of custom tools from a file
        /// </summary>
        private void ImportToolMenuItemClick(object sender, EventArgs e)
        {
            if(openTools.ShowDialog()==DialogResult.OK)
            {
                ClassCustomTools newTools = ClassCustomTools.Load(openTools.FileName);
                if (newTools != null)
                {
                    App.CustomTools = newTools;
                    App.PrintStatusMessage("Loaded custom tools from " + openTools.FileName);
                }
            }
        }

        /// <summary>
        /// Export current set of custom tools to a file
        /// </summary>
        private void ExportToolMenuItemClick(object sender, EventArgs e)
        {
            if (saveTools.ShowDialog() == DialogResult.OK)
            {
                if (App.CustomTools.Save(saveTools.FileName))
                    App.PrintStatusMessage("Saved custom tools to " + saveTools.FileName);
            }
        }

        /// <summary>
        /// A specific custom tool is clicked (selected).
        /// Tag contains the tool class.
        /// </summary>
        private void CustomToolClicked(object sender, EventArgs e)
        {
            // Call the panel view's custom tool handling function
            PanelView.CustomToolClicked(sender, e);
        }

        #endregion

        /// <summary>
        /// Getting started help menu
        /// </summary>
        private void GettingStartedToolStripMenuClick(object sender, EventArgs e)
        {
            ClassHelp.Handler("Getting Started");
        }

        /// <summary>
        /// User clicked on a 'new version available' button
        /// </summary>
        private void NewVersionButtonClick(object sender, EventArgs e)
        {
            ClassHelp.Handler("Download");
        }

        /// <summary>
        /// Callback on the command line text ready.
        /// We execute a custom (immediate) command which can be either a direct git
        /// command or a shell (command prompt?) command.
        /// Several commands may be separated by "&&" token. This accomodates Gerrit
        /// code review process and its shortcuts that can be easily pasted.
        /// </summary>
        private void CmdBoxTextReady(object sender, string cmd)
        {
            foreach (string command in cmd.Split(new[] {" && "}, StringSplitOptions.RemoveEmptyEntries))
            {
                // Print out the command itself
                App.PrintStatusMessage(command);
                // If the command text started with a command 'git', remove it
                string[] tokens = command.Split(' ');
                string args = String.Join(" ", tokens, 1, tokens.Count() - 1);
                // We are guaranteed to have at least one token (by the TextBoxEx control)
                string run;
                if (tokens[0].ToLower() == "git")
                {
                    // Command is a git command: execute it
                    run = ClassGit.Run(args).ToString();
                }
                else
                {
                    // Command is an arbitrary (command line type) command
                    // Use the command shell to execute it
                    run = ClassUtils.ExecuteShellCommand(tokens[0], args);
                }
                App.PrintStatusMessage(run);
            }
        }
    }
}
