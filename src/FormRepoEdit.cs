using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ControlGitconfig = GitForce.Repo.Edit.Panels.ControlGitconfig;
using ControlGitignore = GitForce.Repo.Edit.Panels.ControlGitignore;
using ControlUser = GitForce.Repo.Edit.Panels.ControlUser;
using ControlLocal = GitForce.Repo.Edit.Panels.ControlLocal;
using ControlRemotes = GitForce.Repo.Edit.Panels.ControlRemotes;

namespace GitForce
{
    /// <summary>
    /// Define an interface for option panels; at minimum they need to
    /// implement function that initializes and sets the changed values
    /// </summary>
    interface IRepoSettings
    {
        void Init(ClassRepo repo);
        void Focus(bool focused);
        void ApplyChanges(ClassRepo repo);
    }

    public partial class FormRepoEdit : Form
    {
        /// <summary>
        /// Create a lookup from panel names (which are set in each corresponding node
        /// of an setting tree view) to the actual repo edit panel instance:
        /// </summary>
        private readonly Dictionary<string, UserControl> panels = new Dictionary<string, UserControl> {
            { "Local", new ControlLocal() },
            { "User", new ControlUser() },
            { "Gitignore", new ControlGitignore() },
            { "Gitconfig", new ControlGitconfig() },
            { "Remotes", new ControlRemotes() },
        };

        /// <summary>
        /// Settings panel that is currently on the top
        /// </summary>
        private UserControl current;

        /// <summary>
        /// Current repo that we are editing
        /// </summary>
        private readonly ClassRepo repo;

        public FormRepoEdit(ClassRepo targetRepo)
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);
            repo = targetRepo;

            // Add all user panels to the base repo edit panel; call their init
            foreach (KeyValuePair<string, UserControl> key in panels)
            {
                panel.Controls.Add(key.Value);
                (key.Value as IRepoSettings).Init(repo);
                key.Value.Dock = DockStyle.Fill;
            }

            // Expand the tree and select the first node
            treeSections.ExpandAll();
            treeSections.SelectedNode = treeSections.Nodes[0].Nodes[0];
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormRepoEditFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Selecting the tree node brings up the corresponding user control
        /// </summary>
        private void TreeSectionsAfterSelect(object sender, TreeViewEventArgs e)
        {
            string panelName = e.Node.Tag as string;
            if (panelName != null && panels.ContainsKey(panelName))
            {
                if (current != null)
                    (current as IRepoSettings).Focus(false);
                current = panels[panelName];
                current.BringToFront();
                (current as IRepoSettings).Focus(true);
            }
        }

        /// <summary>
        /// User clicked on the Apply button - store all changes to the current
        /// setting panel, but not to any other. Hitting OK will store all panels.
        /// Hitting Cancel will void all changes in any panel done since the last
        /// time Apply button was pressed.
        /// </summary>
        private void BtApplyClick(object sender, EventArgs e)
        {
            (current as IRepoSettings).ApplyChanges(repo);
        }

        /// <summary>
        /// User clicked on the OK button - apply all modified settings before
        /// returning from the dialog
        /// </summary>
        private void BtOkClick(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, UserControl> key in panels)
                (key.Value as IRepoSettings).ApplyChanges(repo);
        }
    }
}
