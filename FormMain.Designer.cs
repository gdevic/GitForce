namespace GitForce
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuMainFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMainStash = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainUnstash = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainPendingChanges = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainSubmittedChanges = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainBranches = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainRepos = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuView0 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMainRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewLogWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewCommandLine = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainManageKeys = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMainSwitchRemoteRepo = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainEditRemoteRepo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFetchFromRemote = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainPullFromRemote = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainPushToRemote = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainChangelist = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainBranch = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainRepository = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainTools = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.gettingStartedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.documentationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gitHubHomeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusInfoLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.rightTabControl = new System.Windows.Forms.TabControl();
            this.listStatus = new System.Windows.Forms.ListBox();
            this.menuStatus = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStatusCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStatusSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStatusClear = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btAdd = new System.Windows.Forms.ToolStripButton();
            this.btUpdate = new System.Windows.Forms.ToolStripButton();
            this.btUpdateAll = new System.Windows.Forms.ToolStripButton();
            this.btRevert = new System.Windows.Forms.ToolStripButton();
            this.btDelete = new System.Windows.Forms.ToolStripButton();
            this.btDeleteFs = new System.Windows.Forms.ToolStripButton();
            this.btEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.btPull = new System.Windows.Forms.ToolStripButton();
            this.btPush = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.btChangelists = new System.Windows.Forms.ToolStripButton();
            this.btSubmitted = new System.Windows.Forms.ToolStripButton();
            this.btBranches = new System.Windows.Forms.ToolStripButton();
            this.btRepos = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btCancelOperation = new System.Windows.Forms.ToolStripButton();
            this.btOptions = new System.Windows.Forms.ToolStripButton();
            this.btSsh = new System.Windows.Forms.ToolStripButton();
            this.btNewVersion = new System.Windows.Forms.ToolStripButton();
            this.timerBusy = new System.Windows.Forms.Timer(this.components);
            this.loadWk = new System.Windows.Forms.OpenFileDialog();
            this.saveWk = new System.Windows.Forms.SaveFileDialog();
            this.openTools = new System.Windows.Forms.OpenFileDialog();
            this.saveTools = new System.Windows.Forms.SaveFileDialog();
            this.cmdBox = new GitForce.TextBoxEx();
            this.menuMain.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.menuStatus.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainFile,
            this.menuMainEdit,
            this.menuMainView,
            this.menuMainSettings,
            this.menuMainChangelist,
            this.menuMainBranch,
            this.menuMainRepository,
            this.menuMainTools,
            this.menuMainHelp});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(784, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip1";
            // 
            // menuMainFile
            // 
            this.menuMainFile.Name = "menuMainFile";
            this.menuMainFile.Size = new System.Drawing.Size(37, 20);
            this.menuMainFile.Text = "File";
            this.menuMainFile.DropDownOpening += new System.EventHandler(this.MenuMainFileDropDownOpening);
            // 
            // menuMainEdit
            // 
            this.menuMainEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainSelectAll,
            this.toolStripSeparator9,
            this.menuMainStash,
            this.menuMainUnstash});
            this.menuMainEdit.Name = "menuMainEdit";
            this.menuMainEdit.Size = new System.Drawing.Size(39, 20);
            this.menuMainEdit.Text = "Edit";
            // 
            // menuMainSelectAll
            // 
            this.menuMainSelectAll.Name = "menuMainSelectAll";
            this.menuMainSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.menuMainSelectAll.Size = new System.Drawing.Size(164, 22);
            this.menuMainSelectAll.Text = "Select All";
            this.menuMainSelectAll.Click += new System.EventHandler(this.MenuMainSelectAllClick);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(161, 6);
            // 
            // menuMainStash
            // 
            this.menuMainStash.Enabled = false;
            this.menuMainStash.Name = "menuMainStash";
            this.menuMainStash.Size = new System.Drawing.Size(164, 22);
            this.menuMainStash.Text = "Stash";
            this.menuMainStash.Click += new System.EventHandler(this.MenuMainStashClick);
            // 
            // menuMainUnstash
            // 
            this.menuMainUnstash.Enabled = false;
            this.menuMainUnstash.Name = "menuMainUnstash";
            this.menuMainUnstash.Size = new System.Drawing.Size(164, 22);
            this.menuMainUnstash.Text = "Unstash";
            this.menuMainUnstash.Click += new System.EventHandler(this.MenuMainUnstashClick);
            // 
            // menuMainView
            // 
            this.menuMainView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainPendingChanges,
            this.menuMainSubmittedChanges,
            this.menuMainBranches,
            this.menuMainRepos,
            this.toolStripSeparator3,
            this.menuView0,
            this.menuView1,
            this.menuView2,
            this.menuView3,
            this.menuView4,
            this.toolStripSeparator4,
            this.menuMainRefresh,
            this.menuViewLogWindow,
            this.menuViewCommandLine});
            this.menuMainView.Name = "menuMainView";
            this.menuMainView.Size = new System.Drawing.Size(44, 20);
            this.menuMainView.Text = "View";
            this.menuMainView.DropDownOpened += new System.EventHandler(this.MenuMainViewDropDownOpened);
            // 
            // menuMainPendingChanges
            // 
            this.menuMainPendingChanges.Name = "menuMainPendingChanges";
            this.menuMainPendingChanges.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.menuMainPendingChanges.Size = new System.Drawing.Size(230, 22);
            this.menuMainPendingChanges.Tag = "Commits";
            this.menuMainPendingChanges.Text = "Pending Changelists";
            this.menuMainPendingChanges.Click += new System.EventHandler(this.RightPanelSelectionClick);
            // 
            // menuMainSubmittedChanges
            // 
            this.menuMainSubmittedChanges.Name = "menuMainSubmittedChanges";
            this.menuMainSubmittedChanges.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.menuMainSubmittedChanges.Size = new System.Drawing.Size(230, 22);
            this.menuMainSubmittedChanges.Tag = "Revisions";
            this.menuMainSubmittedChanges.Text = "Submitted Changelists";
            this.menuMainSubmittedChanges.Click += new System.EventHandler(this.RightPanelSelectionClick);
            // 
            // menuMainBranches
            // 
            this.menuMainBranches.Name = "menuMainBranches";
            this.menuMainBranches.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.menuMainBranches.Size = new System.Drawing.Size(230, 22);
            this.menuMainBranches.Tag = "Branches";
            this.menuMainBranches.Text = "Branches";
            this.menuMainBranches.Click += new System.EventHandler(this.RightPanelSelectionClick);
            // 
            // menuMainRepos
            // 
            this.menuMainRepos.Name = "menuMainRepos";
            this.menuMainRepos.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.menuMainRepos.Size = new System.Drawing.Size(230, 22);
            this.menuMainRepos.Tag = "Repos";
            this.menuMainRepos.Text = "Repository";
            this.menuMainRepos.Click += new System.EventHandler(this.RightPanelSelectionClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(227, 6);
            // 
            // menuView0
            // 
            this.menuView0.Name = "menuView0";
            this.menuView0.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D1)));
            this.menuView0.Size = new System.Drawing.Size(230, 22);
            this.menuView0.Tag = "0";
            this.menuView0.Text = "Git Status of All Files";
            this.menuView0.Click += new System.EventHandler(this.ViewSetByMenuItem);
            // 
            // menuView1
            // 
            this.menuView1.Name = "menuView1";
            this.menuView1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D2)));
            this.menuView1.Size = new System.Drawing.Size(230, 22);
            this.menuView1.Tag = "1";
            this.menuView1.Text = "Git Status";
            this.menuView1.Click += new System.EventHandler(this.ViewSetByMenuItem);
            // 
            // menuView2
            // 
            this.menuView2.Name = "menuView2";
            this.menuView2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D3)));
            this.menuView2.Size = new System.Drawing.Size(230, 22);
            this.menuView2.Tag = "2";
            this.menuView2.Text = "Git View of Repo";
            this.menuView2.Click += new System.EventHandler(this.ViewSetByMenuItem);
            // 
            // menuView3
            // 
            this.menuView3.Name = "menuView3";
            this.menuView3.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D4)));
            this.menuView3.Size = new System.Drawing.Size(230, 22);
            this.menuView3.Tag = "3";
            this.menuView3.Text = "Local File View";
            this.menuView3.Click += new System.EventHandler(this.ViewSetByMenuItem);
            // 
            // menuView4
            // 
            this.menuView4.Name = "menuView4";
            this.menuView4.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D5)));
            this.menuView4.Size = new System.Drawing.Size(230, 22);
            this.menuView4.Tag = "4";
            this.menuView4.Text = "Local Files Not in Repo";
            this.menuView4.Click += new System.EventHandler(this.ViewSetByMenuItem);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(227, 6);
            // 
            // menuMainRefresh
            // 
            this.menuMainRefresh.Name = "menuMainRefresh";
            this.menuMainRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.menuMainRefresh.Size = new System.Drawing.Size(230, 22);
            this.menuMainRefresh.Text = "Refresh Active Pane";
            this.menuMainRefresh.Click += new System.EventHandler(this.MenuRefreshAll);
            // 
            // menuViewLogWindow
            // 
            this.menuViewLogWindow.Name = "menuViewLogWindow";
            this.menuViewLogWindow.Size = new System.Drawing.Size(230, 22);
            this.menuViewLogWindow.Text = "Log Window";
            this.menuViewLogWindow.Click += new System.EventHandler(this.LogWindowToolStripMenuItemClick);
            // 
            // menuViewCommandLine
            // 
            this.menuViewCommandLine.Name = "menuViewCommandLine";
            this.menuViewCommandLine.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.menuViewCommandLine.Size = new System.Drawing.Size(230, 22);
            this.menuViewCommandLine.Text = "Command Line";
            this.menuViewCommandLine.Click += new System.EventHandler(this.MenuViewCommandLineClick);
            // 
            // menuMainSettings
            // 
            this.menuMainSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainOptions,
            this.menuMainManageKeys,
            this.toolStripSeparator5,
            this.menuMainSwitchRemoteRepo,
            this.menuMainEditRemoteRepo,
            this.menuMainFetchFromRemote,
            this.menuMainPullFromRemote,
            this.menuMainPushToRemote});
            this.menuMainSettings.Name = "menuMainSettings";
            this.menuMainSettings.Size = new System.Drawing.Size(61, 20);
            this.menuMainSettings.Text = "Settings";
            // 
            // menuMainOptions
            // 
            this.menuMainOptions.Name = "menuMainOptions";
            this.menuMainOptions.Size = new System.Drawing.Size(192, 22);
            this.menuMainOptions.Text = "Options...";
            this.menuMainOptions.Click += new System.EventHandler(this.MenuOptions);
            // 
            // menuMainManageKeys
            // 
            this.menuMainManageKeys.Enabled = false;
            this.menuMainManageKeys.Name = "menuMainManageKeys";
            this.menuMainManageKeys.Size = new System.Drawing.Size(192, 22);
            this.menuMainManageKeys.Text = "Manage SSH Keys...";
            this.menuMainManageKeys.Click += new System.EventHandler(this.MenuMainManageKeysClick);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(189, 6);
            // 
            // menuMainSwitchRemoteRepo
            // 
            this.menuMainSwitchRemoteRepo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xToolStripMenuItem});
            this.menuMainSwitchRemoteRepo.Name = "menuMainSwitchRemoteRepo";
            this.menuMainSwitchRemoteRepo.Size = new System.Drawing.Size(192, 22);
            this.menuMainSwitchRemoteRepo.Text = "Switch Remote Repo...";
            // 
            // xToolStripMenuItem
            // 
            this.xToolStripMenuItem.Name = "xToolStripMenuItem";
            this.xToolStripMenuItem.Size = new System.Drawing.Size(79, 22);
            this.xToolStripMenuItem.Text = "x";
            // 
            // menuMainEditRemoteRepo
            // 
            this.menuMainEditRemoteRepo.Name = "menuMainEditRemoteRepo";
            this.menuMainEditRemoteRepo.Size = new System.Drawing.Size(192, 22);
            this.menuMainEditRemoteRepo.Text = "Edit Remote Repos...";
            this.menuMainEditRemoteRepo.Click += new System.EventHandler(this.MenuEditRemoteRepos);
            // 
            // menuMainFetchFromRemote
            // 
            this.menuMainFetchFromRemote.Name = "menuMainFetchFromRemote";
            this.menuMainFetchFromRemote.Size = new System.Drawing.Size(192, 22);
            this.menuMainFetchFromRemote.Text = "Fetch from Remote";
            this.menuMainFetchFromRemote.Click += new System.EventHandler(this.MenuRepoFetch);
            // 
            // menuMainPullFromRemote
            // 
            this.menuMainPullFromRemote.Name = "menuMainPullFromRemote";
            this.menuMainPullFromRemote.Size = new System.Drawing.Size(192, 22);
            this.menuMainPullFromRemote.Text = "Pull from Remote";
            this.menuMainPullFromRemote.Click += new System.EventHandler(this.MenuRepoPull);
            // 
            // menuMainPushToRemote
            // 
            this.menuMainPushToRemote.Name = "menuMainPushToRemote";
            this.menuMainPushToRemote.Size = new System.Drawing.Size(192, 22);
            this.menuMainPushToRemote.Text = "Push to Remote";
            this.menuMainPushToRemote.Click += new System.EventHandler(this.MenuRepoPush);
            // 
            // menuMainChangelist
            // 
            this.menuMainChangelist.Name = "menuMainChangelist";
            this.menuMainChangelist.Size = new System.Drawing.Size(75, 20);
            this.menuMainChangelist.Text = "Changelist";
            this.menuMainChangelist.DropDownOpening += new System.EventHandler(this.MenuMainChangelistDropDownOpening);
            // 
            // menuMainBranch
            // 
            this.menuMainBranch.Name = "menuMainBranch";
            this.menuMainBranch.Size = new System.Drawing.Size(67, 20);
            this.menuMainBranch.Text = "Branches";
            this.menuMainBranch.DropDownOpening += new System.EventHandler(this.MenuMainBranchDropDownOpening);
            // 
            // menuMainRepository
            // 
            this.menuMainRepository.Name = "menuMainRepository";
            this.menuMainRepository.Size = new System.Drawing.Size(75, 20);
            this.menuMainRepository.Text = "Repository";
            this.menuMainRepository.DropDownOpening += new System.EventHandler(this.MenuMainRepositoryDropDownOpening);
            // 
            // menuMainTools
            // 
            this.menuMainTools.Name = "menuMainTools";
            this.menuMainTools.Size = new System.Drawing.Size(48, 20);
            this.menuMainTools.Text = "Tools";
            this.menuMainTools.DropDownOpening += new System.EventHandler(this.MenuMainToolsDropDownOpening);
            // 
            // menuMainHelp
            // 
            this.menuMainHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gettingStartedMenuItem,
            this.documentationMenuItem,
            this.checkForUpdatesMenuItem,
            this.gitHubHomeMenuItem,
            this.toolStripSeparator6,
            this.aboutToolStripMenuItem});
            this.menuMainHelp.Name = "menuMainHelp";
            this.menuMainHelp.Size = new System.Drawing.Size(44, 20);
            this.menuMainHelp.Text = "Help";
            // 
            // gettingStartedMenuItem
            // 
            this.gettingStartedMenuItem.Name = "gettingStartedMenuItem";
            this.gettingStartedMenuItem.Size = new System.Drawing.Size(171, 22);
            this.gettingStartedMenuItem.Text = "Getting Started";
            this.gettingStartedMenuItem.Click += new System.EventHandler(this.GettingStartedToolStripMenuClick);
            // 
            // documentationMenuItem
            // 
            this.documentationMenuItem.Name = "documentationMenuItem";
            this.documentationMenuItem.Size = new System.Drawing.Size(171, 22);
            this.documentationMenuItem.Tag = "http://gdevic.github.com/GitForce";
            this.documentationMenuItem.Text = "Documentation";
            this.documentationMenuItem.Click += new System.EventHandler(this.WebsiteClick);
            // 
            // checkForUpdatesMenuItem
            // 
            this.checkForUpdatesMenuItem.Name = "checkForUpdatesMenuItem";
            this.checkForUpdatesMenuItem.Size = new System.Drawing.Size(171, 22);
            this.checkForUpdatesMenuItem.Tag = "https://github.com/gdevic/GitForce/downloads";
            this.checkForUpdatesMenuItem.Text = "Check for Updates";
            this.checkForUpdatesMenuItem.Click += new System.EventHandler(this.WebsiteClick);
            // 
            // gitHubHomeMenuItem
            // 
            this.gitHubHomeMenuItem.Name = "gitHubHomeMenuItem";
            this.gitHubHomeMenuItem.Size = new System.Drawing.Size(171, 22);
            this.gitHubHomeMenuItem.Tag = "https://github.com/gdevic/GitForce";
            this.gitHubHomeMenuItem.Text = "GitHub Home";
            this.gitHubHomeMenuItem.Click += new System.EventHandler(this.WebsiteClick);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(168, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItemClick);
            // 
            // statusInfoLabel
            // 
            this.statusInfoLabel.Name = "statusInfoLabel";
            this.statusInfoLabel.Size = new System.Drawing.Size(156, 17);
            this.statusInfoLabel.Text = "GitForce - A Git Visual Client";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusInfoLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 480);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(784, 22);
            this.statusStrip.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 49);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listStatus);
            this.splitContainer1.Panel2.Controls.Add(this.cmdBox);
            this.splitContainer1.Size = new System.Drawing.Size(784, 431);
            this.splitContainer1.SplitterDistance = 239;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.rightTabControl);
            this.splitContainer2.Size = new System.Drawing.Size(784, 239);
            this.splitContainer2.SplitterDistance = 373;
            this.splitContainer2.TabIndex = 0;
            // 
            // rightTabControl
            // 
            this.rightTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightTabControl.Location = new System.Drawing.Point(0, 0);
            this.rightTabControl.Name = "rightTabControl";
            this.rightTabControl.SelectedIndex = 0;
            this.rightTabControl.ShowToolTips = true;
            this.rightTabControl.Size = new System.Drawing.Size(407, 239);
            this.rightTabControl.TabIndex = 0;
            // 
            // listStatus
            // 
            this.listStatus.ContextMenuStrip = this.menuStatus;
            this.listStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listStatus.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listStatus.FormattingEnabled = true;
            this.listStatus.IntegralHeight = false;
            this.listStatus.ItemHeight = 15;
            this.listStatus.Location = new System.Drawing.Point(0, 0);
            this.listStatus.Name = "listStatus";
            this.listStatus.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listStatus.Size = new System.Drawing.Size(784, 165);
            this.listStatus.TabIndex = 0;
            // 
            // menuStatus
            // 
            this.menuStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStatusCopy,
            this.menuStatusSelectAll,
            this.toolStripSeparator1,
            this.menuStatusClear});
            this.menuStatus.Name = "menuStatus";
            this.menuStatus.Size = new System.Drawing.Size(123, 76);
            // 
            // menuStatusCopy
            // 
            this.menuStatusCopy.Name = "menuStatusCopy";
            this.menuStatusCopy.Size = new System.Drawing.Size(122, 22);
            this.menuStatusCopy.Text = "Copy";
            this.menuStatusCopy.Click += new System.EventHandler(this.MenuStatusCopyClick);
            // 
            // menuStatusSelectAll
            // 
            this.menuStatusSelectAll.Name = "menuStatusSelectAll";
            this.menuStatusSelectAll.Size = new System.Drawing.Size(122, 22);
            this.menuStatusSelectAll.Text = "Select All";
            this.menuStatusSelectAll.Click += new System.EventHandler(this.MenuStatusSelectAllClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
            // 
            // menuStatusClear
            // 
            this.menuStatusClear.Name = "menuStatusClear";
            this.menuStatusClear.Size = new System.Drawing.Size(122, 22);
            this.menuStatusClear.Text = "Clear";
            this.menuStatusClear.Click += new System.EventHandler(this.MenuSelectClearClick);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btAdd,
            this.btUpdate,
            this.btUpdateAll,
            this.btRevert,
            this.btDelete,
            this.btDeleteFs,
            this.btEdit,
            this.toolStripSeparator8,
            this.btPull,
            this.btPush,
            this.toolStripSeparator7,
            this.btChangelists,
            this.btSubmitted,
            this.btBranches,
            this.btRepos,
            this.toolStripSeparator2,
            this.btCancelOperation,
            this.btOptions,
            this.btSsh,
            this.btNewVersion});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(784, 25);
            this.toolStrip.TabIndex = 3;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btAdd
            // 
            this.btAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btAdd.Enabled = false;
            this.btAdd.Image = ((System.Drawing.Image)(resources.GetObject("btAdd.Image")));
            this.btAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(23, 22);
            this.btAdd.Text = "Add file to Git";
            // 
            // btUpdate
            // 
            this.btUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btUpdate.Enabled = false;
            this.btUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btUpdate.Image")));
            this.btUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btUpdate.Name = "btUpdate";
            this.btUpdate.Size = new System.Drawing.Size(23, 22);
            this.btUpdate.Text = "Update Changelist";
            // 
            // btUpdateAll
            // 
            this.btUpdateAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btUpdateAll.Enabled = false;
            this.btUpdateAll.Image = ((System.Drawing.Image)(resources.GetObject("btUpdateAll.Image")));
            this.btUpdateAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btUpdateAll.Name = "btUpdateAll";
            this.btUpdateAll.Size = new System.Drawing.Size(23, 22);
            this.btUpdateAll.Text = "Update Changelist with All Files";
            // 
            // btRevert
            // 
            this.btRevert.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btRevert.Enabled = false;
            this.btRevert.Image = ((System.Drawing.Image)(resources.GetObject("btRevert.Image")));
            this.btRevert.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btRevert.Name = "btRevert";
            this.btRevert.Size = new System.Drawing.Size(23, 22);
            this.btRevert.Text = "Revert";
            // 
            // btDelete
            // 
            this.btDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btDelete.Enabled = false;
            this.btDelete.Image = ((System.Drawing.Image)(resources.GetObject("btDelete.Image")));
            this.btDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(23, 22);
            this.btDelete.Text = "Open for Delete";
            // 
            // btDeleteFs
            // 
            this.btDeleteFs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btDeleteFs.Enabled = false;
            this.btDeleteFs.Image = ((System.Drawing.Image)(resources.GetObject("btDeleteFs.Image")));
            this.btDeleteFs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btDeleteFs.Name = "btDeleteFs";
            this.btDeleteFs.Size = new System.Drawing.Size(23, 22);
            this.btDeleteFs.Text = "Delete from File System";
            // 
            // btEdit
            // 
            this.btEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btEdit.Enabled = false;
            this.btEdit.Image = ((System.Drawing.Image)(resources.GetObject("btEdit.Image")));
            this.btEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btEdit.Name = "btEdit";
            this.btEdit.Size = new System.Drawing.Size(23, 22);
            this.btEdit.Text = "Edit using default editor";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // btPull
            // 
            this.btPull.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btPull.Enabled = false;
            this.btPull.Image = ((System.Drawing.Image)(resources.GetObject("btPull.Image")));
            this.btPull.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btPull.Name = "btPull";
            this.btPull.Size = new System.Drawing.Size(23, 22);
            this.btPull.Text = "Pull from Remote Repo";
            this.btPull.Click += new System.EventHandler(this.MenuRepoPull);
            // 
            // btPush
            // 
            this.btPush.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btPush.Enabled = false;
            this.btPush.Image = ((System.Drawing.Image)(resources.GetObject("btPush.Image")));
            this.btPush.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btPush.Name = "btPush";
            this.btPush.Size = new System.Drawing.Size(23, 22);
            this.btPush.Text = "Push to Remote Repo";
            this.btPush.Click += new System.EventHandler(this.MenuRepoPush);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // btChangelists
            // 
            this.btChangelists.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btChangelists.Image = ((System.Drawing.Image)(resources.GetObject("btChangelists.Image")));
            this.btChangelists.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btChangelists.ImageTransparentColor = System.Drawing.Color.Black;
            this.btChangelists.Name = "btChangelists";
            this.btChangelists.Size = new System.Drawing.Size(23, 22);
            this.btChangelists.Tag = "Commits";
            this.btChangelists.Text = "View Pending Changelists";
            this.btChangelists.Click += new System.EventHandler(this.RightPanelSelectionClick);
            // 
            // btSubmitted
            // 
            this.btSubmitted.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btSubmitted.Image = ((System.Drawing.Image)(resources.GetObject("btSubmitted.Image")));
            this.btSubmitted.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btSubmitted.ImageTransparentColor = System.Drawing.Color.Black;
            this.btSubmitted.Name = "btSubmitted";
            this.btSubmitted.Size = new System.Drawing.Size(23, 22);
            this.btSubmitted.Tag = "Revisions";
            this.btSubmitted.Text = "View Submitted Changelists";
            this.btSubmitted.Click += new System.EventHandler(this.RightPanelSelectionClick);
            // 
            // btBranches
            // 
            this.btBranches.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btBranches.Image = ((System.Drawing.Image)(resources.GetObject("btBranches.Image")));
            this.btBranches.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btBranches.ImageTransparentColor = System.Drawing.Color.Black;
            this.btBranches.Name = "btBranches";
            this.btBranches.Size = new System.Drawing.Size(23, 22);
            this.btBranches.Tag = "Branches";
            this.btBranches.Text = "View Branches";
            this.btBranches.Click += new System.EventHandler(this.RightPanelSelectionClick);
            // 
            // btRepos
            // 
            this.btRepos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btRepos.Image = ((System.Drawing.Image)(resources.GetObject("btRepos.Image")));
            this.btRepos.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btRepos.ImageTransparentColor = System.Drawing.Color.Black;
            this.btRepos.Name = "btRepos";
            this.btRepos.Size = new System.Drawing.Size(23, 22);
            this.btRepos.Tag = "Repos";
            this.btRepos.Text = "View Repositories";
            this.btRepos.Click += new System.EventHandler(this.RightPanelSelectionClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btCancelOperation
            // 
            this.btCancelOperation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btCancelOperation.Enabled = false;
            this.btCancelOperation.Image = ((System.Drawing.Image)(resources.GetObject("btCancelOperation.Image")));
            this.btCancelOperation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btCancelOperation.Name = "btCancelOperation";
            this.btCancelOperation.Size = new System.Drawing.Size(23, 22);
            this.btCancelOperation.Text = "Cancel Operation";
            this.btCancelOperation.Click += new System.EventHandler(this.BtCancelOperationClick);
            // 
            // btOptions
            // 
            this.btOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btOptions.Image = ((System.Drawing.Image)(resources.GetObject("btOptions.Image")));
            this.btOptions.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btOptions.ImageTransparentColor = System.Drawing.Color.Black;
            this.btOptions.Name = "btOptions";
            this.btOptions.Size = new System.Drawing.Size(23, 22);
            this.btOptions.Text = "Set GitForce Options";
            this.btOptions.Click += new System.EventHandler(this.MenuOptions);
            // 
            // btSsh
            // 
            this.btSsh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btSsh.Enabled = false;
            this.btSsh.Image = ((System.Drawing.Image)(resources.GetObject("btSsh.Image")));
            this.btSsh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSsh.Name = "btSsh";
            this.btSsh.Size = new System.Drawing.Size(23, 22);
            this.btSsh.Text = "Manage SSH Keys";
            this.btSsh.Click += new System.EventHandler(this.MenuMainManageKeysClick);
            // 
            // btNewVersion
            // 
            this.btNewVersion.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btNewVersion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btNewVersion.Image = ((System.Drawing.Image)(resources.GetObject("btNewVersion.Image")));
            this.btNewVersion.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btNewVersion.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btNewVersion.Name = "btNewVersion";
            this.btNewVersion.Size = new System.Drawing.Size(44, 24);
            this.btNewVersion.Text = "A new version of GitForce is available!";
            this.btNewVersion.Visible = false;
            this.btNewVersion.Click += new System.EventHandler(this.NewVersionButtonClick);
            // 
            // timerBusy
            // 
            this.timerBusy.Interval = 200;
            this.timerBusy.Tick += new System.EventHandler(this.TimerBusyTick);
            // 
            // loadWk
            // 
            this.loadWk.DefaultExt = "*.giw";
            this.loadWk.Filter = "Workspace files (*.giw)|*.giw|All files (*.*)|*.*";
            this.loadWk.Title = "Load Workspace";
            // 
            // saveWk
            // 
            this.saveWk.DefaultExt = "*.giw";
            this.saveWk.Filter = "Workspace files (*.giw)|*.giw|All files (*.*)|*.*";
            this.saveWk.Title = "Save Workspace As";
            // 
            // openTools
            // 
            this.openTools.DefaultExt = "*.xml";
            this.openTools.Filter = "Custom tools files (*.xml)|*.xml|All files (*.*)|*.*";
            this.openTools.Title = "Read Custom Tools from a File";
            // 
            // saveTools
            // 
            this.saveTools.DefaultExt = "*.xml";
            this.saveTools.Filter = "Custom tools files (*.xml)|*.xml|All files (*.*)|*.*";
            this.saveTools.Title = "Save Custom Tools to a File";
            // 
            // cmdBox
            // 
            this.cmdBox.AcceptsReturn = true;
            this.cmdBox.BackColor = System.Drawing.SystemColors.Info;
            this.cmdBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cmdBox.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdBox.Location = new System.Drawing.Point(0, 165);
            this.cmdBox.Name = "cmdBox";
            this.cmdBox.Size = new System.Drawing.Size(784, 23);
            this.cmdBox.TabIndex = 1;
            this.cmdBox.TextReady += new GitForce.TextBoxEx.TextReadyEventHandler(this.CmdBoxTextReady);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 502);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(300, 300);
            this.MainMenuStrip = this.menuMain;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "GitForce";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMainFormClosing);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.menuStatus.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripMenuItem menuMainFile;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox listStatus;
        private System.Windows.Forms.ContextMenuStrip menuStatus;
        private System.Windows.Forms.ToolStripMenuItem menuStatusCopy;
        private System.Windows.Forms.ToolStripMenuItem menuStatusSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuStatusClear;
        private System.Windows.Forms.ToolStripMenuItem menuMainEdit;
        private System.Windows.Forms.ToolStripMenuItem menuMainView;
        private System.Windows.Forms.ToolStripMenuItem menuViewLogWindow;
        private System.Windows.Forms.ToolStripMenuItem menuMainSettings;
        private System.Windows.Forms.ToolStripMenuItem menuMainChangelist;
        private System.Windows.Forms.ToolStripMenuItem menuMainBranch;
        private System.Windows.Forms.ToolStripMenuItem menuMainRepository;
        private System.Windows.Forms.ToolStripMenuItem menuMainHelp;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btChangelists;
        private System.Windows.Forms.ToolStripButton btSubmitted;
        private System.Windows.Forms.ToolStripButton btBranches;
        private System.Windows.Forms.ToolStripButton btRepos;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btCancelOperation;
        private System.Windows.Forms.ToolStripButton btOptions;
        private System.Windows.Forms.ToolStripButton btSsh;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuMainSelectAll;
        private System.Windows.Forms.ToolStripMenuItem menuMainPendingChanges;
        private System.Windows.Forms.ToolStripMenuItem menuMainSubmittedChanges;
        private System.Windows.Forms.ToolStripMenuItem menuMainBranches;
        private System.Windows.Forms.ToolStripMenuItem menuMainRepos;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuView0;
        private System.Windows.Forms.ToolStripMenuItem menuView1;
        private System.Windows.Forms.ToolStripMenuItem menuView2;
        private System.Windows.Forms.ToolStripMenuItem menuView3;
        private System.Windows.Forms.ToolStripMenuItem menuView4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuMainRefresh;
        private System.Windows.Forms.ToolStripMenuItem menuMainOptions;
        private System.Windows.Forms.ToolStripMenuItem menuMainManageKeys;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem menuMainSwitchRemoteRepo;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuMainEditRemoteRepo;
        private System.Windows.Forms.ToolStripMenuItem menuMainPullFromRemote;
        private System.Windows.Forms.ToolStripMenuItem menuMainPushToRemote;
        private System.Windows.Forms.ToolStripMenuItem documentationMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripStatusLabel statusInfoLabel;
        private System.Windows.Forms.Timer timerBusy;
        private System.Windows.Forms.ToolStripMenuItem gitHubHomeMenuItem;
        private System.Windows.Forms.ToolStripButton btAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton btUpdate;
        private System.Windows.Forms.ToolStripButton btDelete;
        private System.Windows.Forms.ToolStripButton btDeleteFs;
        private System.Windows.Forms.ToolStripButton btEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton btPull;
        private System.Windows.Forms.ToolStripButton btPush;
        private System.Windows.Forms.ToolStripButton btUpdateAll;
        private System.Windows.Forms.ToolStripButton btRevert;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem menuMainStash;
        private System.Windows.Forms.ToolStripMenuItem menuMainUnstash;
        private System.Windows.Forms.OpenFileDialog loadWk;
        private System.Windows.Forms.SaveFileDialog saveWk;
        private System.Windows.Forms.ToolStripMenuItem menuMainTools;
        private System.Windows.Forms.OpenFileDialog openTools;
        private System.Windows.Forms.SaveFileDialog saveTools;
        private System.Windows.Forms.ToolStripMenuItem gettingStartedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuMainFetchFromRemote;
        private System.Windows.Forms.ToolStripMenuItem menuViewCommandLine;
        protected TextBoxEx cmdBox;
        private System.Windows.Forms.ToolStripButton btNewVersion;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesMenuItem;
        private System.Windows.Forms.TabControl rightTabControl;
    }
}

