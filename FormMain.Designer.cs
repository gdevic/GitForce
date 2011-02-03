namespace git4win
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
            this.menuMainView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainPendingChanges = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainSubmittedChanges = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainBranches = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainRepos = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuView0 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMainRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewExecuteWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainManageKeys = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMainSwitchRemoteRepo = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainEditRemoteRepo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainPullFromRemote = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainPushToRemote = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainChangelist = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainBranch = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainRepository = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusInfoLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.btChangelists = new System.Windows.Forms.ToolStripButton();
            this.btSubmitted = new System.Windows.Forms.ToolStripButton();
            this.btBranches = new System.Windows.Forms.ToolStripButton();
            this.btRepos = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.btCancelOperation = new System.Windows.Forms.ToolStripButton();
            this.btOptions = new System.Windows.Forms.ToolStripButton();
            this.btPutty = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listStatus = new System.Windows.Forms.ListBox();
            this.menuStatus = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStatusCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStatusSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSelectClear = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMain.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.SuspendLayout();
            this.menuStatus.SuspendLayout();
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
            this.helpToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(650, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip1";
            // 
            // menuMainFile
            // 
            this.menuMainFile.Name = "menuMainFile";
            this.menuMainFile.Size = new System.Drawing.Size(37, 20);
            this.menuMainFile.Text = "File";
            this.menuMainFile.DropDownOpening += new System.EventHandler(this.menuMainFile_DropDownOpening);
            // 
            // menuMainEdit
            // 
            this.menuMainEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainSelectAll});
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
            this.menuMainSelectAll.Click += new System.EventHandler(this.menuMainSelectAll_Click);
            // 
            // menuMainView
            // 
            this.menuMainView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainPendingChanges,
            this.menuMainSubmittedChanges,
            this.menuMainBranches,
            this.menuMainRepos,
            this.toolStripSeparator5,
            this.menuView0,
            this.menuView1,
            this.menuView2,
            this.menuView3,
            this.menuView4,
            this.toolStripSeparator6,
            this.menuMainRefresh,
            this.menuViewExecuteWindow});
            this.menuMainView.Name = "menuMainView";
            this.menuMainView.Size = new System.Drawing.Size(44, 20);
            this.menuMainView.Text = "View";
            this.menuMainView.DropDownOpened += new System.EventHandler(this.menuMainView_DropDownOpened);
            // 
            // menuMainPendingChanges
            // 
            this.menuMainPendingChanges.Name = "menuMainPendingChanges";
            this.menuMainPendingChanges.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.menuMainPendingChanges.Size = new System.Drawing.Size(230, 22);
            this.menuMainPendingChanges.Tag = "Commits";
            this.menuMainPendingChanges.Text = "Pending Changelists";
            this.menuMainPendingChanges.Click += new System.EventHandler(this.rightPanelSelection_Click);
            // 
            // menuMainSubmittedChanges
            // 
            this.menuMainSubmittedChanges.Name = "menuMainSubmittedChanges";
            this.menuMainSubmittedChanges.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.menuMainSubmittedChanges.Size = new System.Drawing.Size(230, 22);
            this.menuMainSubmittedChanges.Tag = "Revisions";
            this.menuMainSubmittedChanges.Text = "Submitted Changelists";
            this.menuMainSubmittedChanges.Click += new System.EventHandler(this.rightPanelSelection_Click);
            // 
            // menuMainBranches
            // 
            this.menuMainBranches.Name = "menuMainBranches";
            this.menuMainBranches.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.menuMainBranches.Size = new System.Drawing.Size(230, 22);
            this.menuMainBranches.Tag = "Branches";
            this.menuMainBranches.Text = "Branches";
            this.menuMainBranches.Click += new System.EventHandler(this.rightPanelSelection_Click);
            // 
            // menuMainRepos
            // 
            this.menuMainRepos.Name = "menuMainRepos";
            this.menuMainRepos.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.menuMainRepos.Size = new System.Drawing.Size(230, 22);
            this.menuMainRepos.Tag = "Repos";
            this.menuMainRepos.Text = "Repository";
            this.menuMainRepos.Click += new System.EventHandler(this.rightPanelSelection_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(227, 6);
            // 
            // menuView0
            // 
            this.menuView0.Name = "menuView0";
            this.menuView0.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D1)));
            this.menuView0.Size = new System.Drawing.Size(230, 22);
            this.menuView0.Tag = "0";
            this.menuView0.Text = "Git View of Local Files";
            this.menuView0.Click += new System.EventHandler(this.viewSetByMenuItem);
            // 
            // menuView1
            // 
            this.menuView1.Name = "menuView1";
            this.menuView1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D2)));
            this.menuView1.Size = new System.Drawing.Size(230, 22);
            this.menuView1.Tag = "1";
            this.menuView1.Text = "Git View of Files";
            this.menuView1.Click += new System.EventHandler(this.viewSetByMenuItem);
            // 
            // menuView2
            // 
            this.menuView2.Name = "menuView2";
            this.menuView2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D3)));
            this.menuView2.Size = new System.Drawing.Size(230, 22);
            this.menuView2.Tag = "2";
            this.menuView2.Text = "Git View of Repo";
            this.menuView2.Click += new System.EventHandler(this.viewSetByMenuItem);
            // 
            // menuView3
            // 
            this.menuView3.Name = "menuView3";
            this.menuView3.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D4)));
            this.menuView3.Size = new System.Drawing.Size(230, 22);
            this.menuView3.Tag = "3";
            this.menuView3.Text = "Local File View";
            this.menuView3.Click += new System.EventHandler(this.viewSetByMenuItem);
            // 
            // menuView4
            // 
            this.menuView4.Name = "menuView4";
            this.menuView4.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D5)));
            this.menuView4.Size = new System.Drawing.Size(230, 22);
            this.menuView4.Tag = "4";
            this.menuView4.Text = "Local Files Not in Repo";
            this.menuView4.Click += new System.EventHandler(this.viewSetByMenuItem);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(227, 6);
            // 
            // menuMainRefresh
            // 
            this.menuMainRefresh.Name = "menuMainRefresh";
            this.menuMainRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.menuMainRefresh.Size = new System.Drawing.Size(230, 22);
            this.menuMainRefresh.Text = "Refresh Active Pane";
            this.menuMainRefresh.Click += new System.EventHandler(this.menuRefreshAll);
            // 
            // menuViewExecuteWindow
            // 
            this.menuViewExecuteWindow.Name = "menuViewExecuteWindow";
            this.menuViewExecuteWindow.Size = new System.Drawing.Size(230, 22);
            this.menuViewExecuteWindow.Text = "Execute Window";
            this.menuViewExecuteWindow.Click += new System.EventHandler(this.menuShowExecuteWindow);
            // 
            // menuMainSettings
            // 
            this.menuMainSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainOptions,
            this.menuMainManageKeys,
            this.toolStripSeparator1,
            this.menuMainSwitchRemoteRepo,
            this.menuMainEditRemoteRepo,
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
            this.menuMainOptions.Click += new System.EventHandler(this.menuOptions);
            // 
            // menuMainManageKeys
            // 
            this.menuMainManageKeys.Name = "menuMainManageKeys";
            this.menuMainManageKeys.Size = new System.Drawing.Size(192, 22);
            this.menuMainManageKeys.Text = "Manage PuTTY Keys...";
            this.menuMainManageKeys.Click += new System.EventHandler(this.menuMainManageKeys_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(189, 6);
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
            this.menuMainEditRemoteRepo.Click += new System.EventHandler(this.menuEditRemoteRepos);
            // 
            // menuMainPullFromRemote
            // 
            this.menuMainPullFromRemote.Name = "menuMainPullFromRemote";
            this.menuMainPullFromRemote.Size = new System.Drawing.Size(192, 22);
            this.menuMainPullFromRemote.Text = "Pull from Remote";
            this.menuMainPullFromRemote.Click += new System.EventHandler(this.menuRepoPull);
            // 
            // menuMainPushToRemote
            // 
            this.menuMainPushToRemote.Name = "menuMainPushToRemote";
            this.menuMainPushToRemote.Size = new System.Drawing.Size(192, 22);
            this.menuMainPushToRemote.Text = "Push to Remote";
            this.menuMainPushToRemote.Click += new System.EventHandler(this.menuRepoPush);
            // 
            // menuMainChangelist
            // 
            this.menuMainChangelist.Name = "menuMainChangelist";
            this.menuMainChangelist.Size = new System.Drawing.Size(75, 20);
            this.menuMainChangelist.Text = "Changelist";
            this.menuMainChangelist.DropDownOpening += new System.EventHandler(this.menuMainChangelist_DropDownOpening);
            // 
            // menuMainBranch
            // 
            this.menuMainBranch.Name = "menuMainBranch";
            this.menuMainBranch.Size = new System.Drawing.Size(67, 20);
            this.menuMainBranch.Text = "Branches";
            this.menuMainBranch.DropDownOpening += new System.EventHandler(this.menuMainBranch_DropDownOpening);
            // 
            // menuMainRepository
            // 
            this.menuMainRepository.Name = "menuMainRepository";
            this.menuMainRepository.Size = new System.Drawing.Size(75, 20);
            this.menuMainRepository.Text = "Repository";
            this.menuMainRepository.DropDownOpening += new System.EventHandler(this.menuMainRepository_DropDownOpening);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusInfoLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 428);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(650, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusInfoLabel
            // 
            this.statusInfoLabel.Name = "statusInfoLabel";
            this.statusInfoLabel.Size = new System.Drawing.Size(49, 17);
            this.statusInfoLabel.Text = "STATUS";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator9,
            this.btChangelists,
            this.btSubmitted,
            this.btBranches,
            this.btRepos,
            this.toolStripSeparator8,
            this.btCancelOperation,
            this.btOptions,
            this.btPutty});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(650, 25);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
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
            this.btChangelists.Click += new System.EventHandler(this.rightPanelSelection_Click);
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
            this.btSubmitted.Click += new System.EventHandler(this.rightPanelSelection_Click);
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
            this.btBranches.Click += new System.EventHandler(this.rightPanelSelection_Click);
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
            this.btRepos.Click += new System.EventHandler(this.rightPanelSelection_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
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
            // 
            // btOptions
            // 
            this.btOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btOptions.Image = ((System.Drawing.Image)(resources.GetObject("btOptions.Image")));
            this.btOptions.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btOptions.ImageTransparentColor = System.Drawing.Color.Black;
            this.btOptions.Name = "btOptions";
            this.btOptions.Size = new System.Drawing.Size(23, 22);
            this.btOptions.Text = "Set git4win Options";
            this.btOptions.Click += new System.EventHandler(this.menuOptions);
            // 
            // btPutty
            // 
            this.btPutty.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btPutty.Image = ((System.Drawing.Image)(resources.GetObject("btPutty.Image")));
            this.btPutty.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btPutty.Name = "btPutty";
            this.btPutty.Size = new System.Drawing.Size(23, 22);
            this.btPutty.Text = "Manage PuTTY Keys";
            this.btPutty.Click += new System.EventHandler(this.menuMainManageKeys_Click);
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
            this.splitContainer1.Size = new System.Drawing.Size(650, 379);
            this.splitContainer1.SplitterDistance = 196;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Size = new System.Drawing.Size(650, 196);
            this.splitContainer2.SplitterDistance = 323;
            this.splitContainer2.TabIndex = 0;
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
            this.listStatus.Size = new System.Drawing.Size(650, 179);
            this.listStatus.TabIndex = 0;
            // 
            // menuStatus
            // 
            this.menuStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStatusCopy,
            this.menuStatusSelectAll,
            this.toolStripSeparator2,
            this.menuSelectClear});
            this.menuStatus.Name = "menuStatus";
            this.menuStatus.Size = new System.Drawing.Size(153, 98);
            // 
            // menuStatusCopy
            // 
            this.menuStatusCopy.Name = "menuStatusCopy";
            this.menuStatusCopy.Size = new System.Drawing.Size(152, 22);
            this.menuStatusCopy.Text = "Copy";
            this.menuStatusCopy.Click += new System.EventHandler(this.menuStatusCopy_Click);
            // 
            // menuStatusSelectAll
            // 
            this.menuStatusSelectAll.Name = "menuStatusSelectAll";
            this.menuStatusSelectAll.Size = new System.Drawing.Size(152, 22);
            this.menuStatusSelectAll.Text = "Select All";
            this.menuStatusSelectAll.Click += new System.EventHandler(this.menuStatusSelectAll_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
            // 
            // menuSelectClear
            // 
            this.menuSelectClear.Name = "menuSelectClear";
            this.menuSelectClear.Size = new System.Drawing.Size(152, 22);
            this.menuSelectClear.Text = "Clear";
            this.menuSelectClear.Click += new System.EventHandler(this.menuSelectClear_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(300, 300);
            this.MainMenuStrip = this.menuMain;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "git4win";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.menuStatus.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuMainFile;
        private System.Windows.Forms.ToolStripMenuItem menuMainView;
        private System.Windows.Forms.ToolStripMenuItem menuMainSettings;
        private System.Windows.Forms.ToolStripMenuItem menuMainChangelist;
        private System.Windows.Forms.ToolStripMenuItem menuMainBranch;
        private System.Windows.Forms.ToolStripMenuItem menuMainRepository;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox listStatus;
        private System.Windows.Forms.ToolStripMenuItem menuMainOptions;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuMainSwitchRemoteRepo;
        private System.Windows.Forms.ToolStripMenuItem menuMainEditRemoteRepo;
        private System.Windows.Forms.ToolStripMenuItem menuMainBranches;
        private System.Windows.Forms.ToolStripMenuItem menuMainRepos;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem menuMainRefresh;
        private System.Windows.Forms.ToolStripMenuItem menuMainPendingChanges;
        private System.Windows.Forms.ToolStripMenuItem menuMainSubmittedChanges;
        private System.Windows.Forms.ToolStripButton btChangelists;
        private System.Windows.Forms.ToolStripButton btSubmitted;
        private System.Windows.Forms.ToolStripButton btBranches;
        private System.Windows.Forms.ToolStripButton btRepos;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton btOptions;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel statusInfoLabel;
        private System.Windows.Forms.ToolStripMenuItem menuView0;
        private System.Windows.Forms.ToolStripMenuItem menuView1;
        private System.Windows.Forms.ToolStripMenuItem menuView2;
        private System.Windows.Forms.ToolStripMenuItem menuView3;
        private System.Windows.Forms.ToolStripMenuItem menuView4;
        private System.Windows.Forms.ToolStripMenuItem menuViewExecuteWindow;
        private System.Windows.Forms.ToolStripButton btCancelOperation;
        private System.Windows.Forms.ToolStripMenuItem menuMainPullFromRemote;
        private System.Windows.Forms.ToolStripMenuItem menuMainPushToRemote;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuMainEdit;
        private System.Windows.Forms.ToolStripMenuItem menuMainSelectAll;
        private System.Windows.Forms.ContextMenuStrip menuStatus;
        private System.Windows.Forms.ToolStripMenuItem menuStatusCopy;
        private System.Windows.Forms.ToolStripMenuItem menuStatusSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuSelectClear;
        private System.Windows.Forms.ToolStripButton btPutty;
        private System.Windows.Forms.ToolStripMenuItem menuMainManageKeys;
    }
}

