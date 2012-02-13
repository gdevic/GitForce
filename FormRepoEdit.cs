using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GitForce.Repo.Edit.Panels;

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
        private readonly Dictionary<string, UserControl> _panels = new Dictionary<string, UserControl> {
            { "User", new ControlUser() },
            { "Gitignore", new ControlGitignore() },
            { "Gitconfig", new ControlGitconfig() },
        };

        /// <summary>
        /// Settings panel that is currently on the top
        /// </summary>
        private UserControl _current;

        /// <summary>
        /// Current repo that we are editing
        /// </summary>
        private readonly ClassRepo _repo;

        public FormRepoEdit(ClassRepo repo)
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);
            _repo = repo;

            // Get all local configuration strings and assign various panel controls.
            // This is placed first, before initializing the user panels, so that the
            // strings are accessible to individual panels should they need to use them.
            // TODO: This is not needed for now
            //string[] config = _repo.Run("config --local --list -z").Split('\0');

            // Add all user panels to the base repo edit panel; call their init
            foreach (KeyValuePair<string, UserControl> key in _panels)
            {
                panel.Controls.Add(key.Value);
                (key.Value as IRepoSettings).Init(_repo);
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
            if (panelName != null && _panels.ContainsKey(panelName))
            {
                if (_current != null)
                    (_current as IRepoSettings).Focus(false);
                _current = _panels[panelName];
                _current.BringToFront();
                (_current as IRepoSettings).Focus(true);
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
            (_current as IRepoSettings).ApplyChanges(_repo);
        }

        /// <summary>
        /// User clicked on the OK button - apply all modified settings before
        /// returning from the dialog
        /// </summary>
        private void BtOkClick(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, UserControl> key in _panels)
                (key.Value as IRepoSettings).ApplyChanges(_repo);
        }
    }
}
