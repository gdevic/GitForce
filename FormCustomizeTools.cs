using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Customize tools, main form.
    /// </summary>
    public partial class FormCustomizeTools : Form
    {
        /// <summary>
        /// Private copy of the custom toolset
        /// </summary>
        public readonly ClassCustomTools CustomTools;

        public FormCustomizeTools(ClassCustomTools customTools)
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            CustomTools = customTools.Copy();
            RefreshList();
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormCustomizeToolsFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Reload the list of tools, set the current tool and show the information
        /// Also set all button enables accordingly.
        /// </summary>
        private void RefreshList()
        {
            int sel = listTools.SelectedIndex;
            listTools.Items.Clear();
            foreach (var t in CustomTools.Tools)
                listTools.Items.Add(t.Name);
            if (listTools.Items.Count > 0)
                listTools.SelectedIndex = (sel >= 0 ? sel : 0);
            AdjustEnables();
        }

        /// <summary>
        /// Based on the number of tools in the list and the index of a selected tool,
        /// adjust all button enables.
        /// Also, fill in the information from the selected tool into this form.
        /// </summary>
        private void AdjustEnables()
        {
            ClassTool tool = new ClassTool();
            btEdit.Enabled = btRemove.Enabled = btUp.Enabled = btDown.Enabled = listTools.Items.Count > 0;
            if (listTools.Items.Count > 0)
            {
                int sel = listTools.SelectedIndex;
                tool = CustomTools.Tools[sel];

                if (sel == 0)
                    btUp.Enabled = false;
                if (sel == listTools.Items.Count - 1)
                    btDown.Enabled = false;
            }

            textCmd.Text = tool.Cmd;
            textArgs.Text = tool.Args;
            textDir.Text = tool.Dir;
            textDesc.Text = tool.Desc;
            checkAddToContextMenu.Checked = tool.Checks[0];
            checkConsoleApp.Checked = tool.Checks[1];
            checkWriteToStatus.Checked = tool.Checks[2];
            checkCloseUponExit.Checked = tool.Checks[3];
            checkRefresh.Checked = tool.Checks[4];
            checkPrompt.Checked = tool.Checks[5];
            checkBrowse.Checked = tool.Checks[6];            
        }

        /// <summary>
        /// User clicked on the Add button to add a new tool.
        /// </summary>
        private void BtAddClick(object sender, EventArgs e)
        {
            FormEditTools formEditTools = new FormEditTools(new ClassTool());
            if (formEditTools.ShowDialog() == DialogResult.OK)
            {
                int sel = listTools.SelectedIndex >= 0 ? listTools.SelectedIndex : 0;
                CustomTools.Tools.Insert(sel, formEditTools.Tool);
                RefreshList();
            }
        }

        /// <summary>
        /// User clicked on the Edit button to edit selected tool.
        /// </summary>
        private void BtEditClick(object sender, EventArgs e)
        {
            int sel = listTools.SelectedIndex;
            FormEditTools formEditTools = new FormEditTools(CustomTools.Tools[sel]);
            if (formEditTools.ShowDialog() == DialogResult.OK)
            {
                CustomTools.Tools[sel] = formEditTools.Tool;
                RefreshList();
            }
        }

        /// <summary>
        /// User clicked on the Remove button to remove a selected tool.
        /// </summary>
        private void BtRemoveClick(object sender, EventArgs e)
        {
            if(MessageBox.Show("This will permanently remove the tool. Proceed?", "Remove tool", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
            {
                int sel = listTools.SelectedIndex;
                CustomTools.Tools.RemoveAt(sel);
                RefreshList();
            }
        }

        /// <summary>
        /// User clicked on the Up button to reorder tool: move selected tool up one slot.
        /// </summary>
        private void BtUpClick(object sender, EventArgs e)
        {
            int sel = listTools.SelectedIndex;
            if (sel > 0)
            {
                CustomTools.Tools.Insert(sel - 1, CustomTools.Tools[sel]);
                CustomTools.Tools.RemoveAt(sel + 1);
                listTools.SelectedIndex = sel - 1;
                RefreshList();
            }
        }

        /// <summary>
        /// User clicked on the Down button to reorder tool: move selected tool down one slot.
        /// </summary>
        private void BtDownClick(object sender, EventArgs e)
        {
            int sel = listTools.SelectedIndex;
            if (sel < listTools.Items.Count - 1)
            {
                CustomTools.Tools.Insert(sel + 2, CustomTools.Tools[sel]);
                CustomTools.Tools.RemoveAt(sel);
                listTools.SelectedIndex = sel + 1;
                RefreshList();
            }
        }

        /// <summary>
        /// User clicked with a mouse on an item in the list of tools, select the item.
        /// </summary>
        private void ListToolsSelectedIndexChanged(object sender, EventArgs e)
        {
            AdjustEnables();
        }
    }
}
