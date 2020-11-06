﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GitForce.Settings.Panels
{
    public partial class ControlViewEdit : UserControl, IUserSettings
    {
        public ControlViewEdit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// If the list of editors is empty, find several common Windows or Linux editors
        /// and add them to the list. This is a static function called on program startup.
        /// </summary>
        public static void AddKnownEditors()
        {
            if (String.IsNullOrEmpty(Properties.Settings.Default.EditViewPrograms))
            {
                List<string> candidates = new List<string>
                {
                    // Windows OS common editors:
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"notepad.exe"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Windows NT\Accessories\wordpad.exe"),

                    // Linux OS common GUI editors:
                    @"/usr/bin/gedit",
                    @"/usr/bin/emacs",
                };

                // Add to the list all editors that can be accessed
                List<string> editors = candidates.Where(File.Exists).ToList();

                // Save the list of editors in settings
                if (editors.Count > 0)
                    Properties.Settings.Default.EditViewPrograms = String.Join("\0", editors.ToArray());
            }
        }

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        /// <param name="options">All git global settings</param>
        public void Init(string[] options)
        {
            // Load a list of programs from the application settings
            string values = Properties.Settings.Default.EditViewPrograms;
            string[] progs = values.Split(("\0").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in progs)
                listPrograms.Items.Add(s);
        }

        /// <summary>
        /// Control received a focus (true) or lost a focus (false)
        /// </summary>
        public void Focus(bool focused)
        {
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges()
        {
            // Store a list of programs to application settings
            List<string> progs = listPrograms.Items.Cast<string>().ToList();
            Properties.Settings.Default.EditViewPrograms = String.Join("\0", progs.ToArray());
        }

        /// <summary>
        /// User clicked on Add button to add a program
        /// </summary>
        private void BtAddClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                if (!listPrograms.Items.Contains(openFileDialog.FileName))
                    listPrograms.Items.Add(openFileDialog.FileName);
        }

        /// <summary>
        /// User clicked on Remove button to remove selected programs
        /// </summary>
        private void BtRemoveClick(object sender, EventArgs e)
        {
            while (listPrograms.SelectedItems.Count > 0)
                listPrograms.Items.Remove(listPrograms.SelectedItem);
        }
    }
}
