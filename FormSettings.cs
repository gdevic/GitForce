using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GitForce.Settings.Panels;

namespace GitForce
{
    /// <summary>
    /// Define an interface for option panels; at minimum they need to
    /// implement function that initializes and sets the changed values
    /// </summary>
    interface IUserSettings
    {
        void Init(string[] config);
        void Focus(bool focused);
        void ApplyChanges();
    }

    public sealed partial class FormSettings : Form
    {
        /// <summary>
        /// Get all global configuration strings and assign various panel controls.
        /// This is placed first, before initializing the user panels, so that the
        /// strings are accessible to individual panels should they need to use them.
        /// </summary>
        private readonly string[] config = ClassGit.Run("config --global --list -z").ToString().Split('\0');

        /// <summary>
        /// Create a lookup from panel names (which are set in each corresponding node
        /// of an option set tree view) to the actual option panel instance:
        /// </summary>
        private readonly Dictionary<string, UserControl> panels = new Dictionary<string, UserControl> {
            { "Global", new ControlGlobal() },
            { "User", new ControlUser() },
            { "Status", new ControlStatus() },
            { "Commits", new ControlCommits() },
            { "Gitignore", new ControlGitignore() },
            { "Gitconfig", new ControlGitconfig() },
            { "Files", new ControlFiles() },
            { "ViewEdit", new ControlViewEdit() },
            { "Diff", new ControlDiff() },
            { "Merge", new ControlMerge() },
            { "DoubleClick", new ControlDoubleClick() },
            { "Options", new ControlOptions() },
            { "Aliases", new ControlAliases() },
            { "Ssl", new ControlSsl() },
            { "Specifications", new ControlSpecifications() },
        };

        /// <summary>
        /// Settings panel that is currently on the top
        /// </summary>
        private UserControl current;

        public FormSettings()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            Cursor.Current = Cursors.WaitCursor;
            // Add all user panels to the base options panel; call their init
            foreach (KeyValuePair<string, UserControl> key in panels)
            {
                panel.Controls.Add(key.Value);
                (key.Value as IUserSettings).Init(config);
                key.Value.Dock = DockStyle.Fill;
            }

            // Expand the tree and select the first node
            treeSections.ExpandAll();
            treeSections.SelectedNode = treeSections.Nodes[0].Nodes[0];
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormSettingsFormClosing(object sender, FormClosingEventArgs e)
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
                    (current as IUserSettings).Focus(false);
                current = panels[panelName];
                current.BringToFront();
                (current as IUserSettings).Focus(true);
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
            (current as IUserSettings).ApplyChanges();
        }

        /// <summary>
        /// User clicked on the OK button - apply all modified settings before
        /// returning from the dialog
        /// </summary>
        private void BtOkClick(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, UserControl> key in panels)
                (key.Value as IUserSettings).ApplyChanges();
        }
    }

    /// <summary>
    /// This class is a static helper class that simply marks a tag field
    /// of a calling control with a non-empty value. It should be called
    /// on text/value/state changed event of a control when the set operation
    /// may be expensive (such are calling git to set configuration parameters)
    /// </summary>
    public static class ControlDirtyHelper
    {
        public static void ControlDirty(object sender, EventArgs e)
        {
            (sender as Control).Tag = "1";
        }
    }
}
