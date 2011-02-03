using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win.FormOptions_Panels
{
    public partial class ControlViewEdit : UserControl, IUserSettings
    {
        public ControlViewEdit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// If the list of editors is empty, find several common Windows (OS) editors
        /// and add them to the list. This is a static function called on program startup.
        /// </summary>
        public static void AddKnownEditors()
        {
            if( String.IsNullOrEmpty(Properties.Settings.Default.EditViewPrograms))
            {
                List<string> editors = new List<string>();

                // %windir%\system32\notepad.exe
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"system32\notepad.exe");
                if (File.Exists(path))
                    editors.Add(path);

                // "%ProgramFiles%\Windows NT\Accessories\wordpad.exe"
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Windows NT\Accessories\wordpad.exe");
                if (File.Exists(path))
                    editors.Add(path);

                if (editors.Count > 0)
                    Properties.Settings.Default.EditViewPrograms = String.Join("\0", editors);
            }
        }

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        /// <param name="options">All git global settings</param>
        public void Init(string[] options)
        {
            // Load a list of programs from the application settings
            string[] progs = Properties.Settings.Default.EditViewPrograms.Split(("\0").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in progs)
                listPrograms.Items.Add(s);
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges()
        {
            // Store a list of programs to application settings
            List<string> progs = new List<string>();
            foreach (string s in listPrograms.Items)
                progs.Add(s);
            Properties.Settings.Default.EditViewPrograms = String.Join("\0", progs);
        }

        /// <summary>
        /// User clicked on Add button to add a program
        /// </summary>
        private void btAdd_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                if (!listPrograms.Items.Contains(openFileDialog.FileName))
                    listPrograms.Items.Add(openFileDialog.FileName);
        }

        /// <summary>
        /// User clicked on Remove button to remove selected programs
        /// </summary>
        private void btRemove_Click(object sender, EventArgs e)
        {
            while (listPrograms.SelectedItems.Count > 0)
                listPrograms.Items.Remove(listPrograms.SelectedItem);
        }
    }
}
