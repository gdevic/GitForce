﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce.Settings.Panels
{
    public partial class ControlAliases : UserControl, IUserSettings
    {
        public ControlAliases()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        /// <param name="options">All git global settings</param>
        public void Init(string[] options)
        {
            foreach (string s in options)
            {
                // Populate text box with aliases...
                try
                {
                    string[] def = s.Split('\n');       // Definition
                    string[] sec = def[0].Split('.');   // Section

                    switch (sec[0])
                    {
                        case "alias":
                            textBoxAliases.Text += sec[1] + "=" + def[1] + Environment.NewLine;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    App.PrintLogMessage(ex.Message, MessageType.Error);
                }
            }

            // Add the dirty (modified) value changed helper
            textBoxAliases.TextChanged += ControlDirtyHelper.ControlDirty;
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
            if (textBoxAliases.Tag != null)
            {
                // Remove all aliases and then rebuild them
                if (ClassGit.Run("config --remove-section alias").Success() == false)
                    App.PrintLogMessage("ClassAliases: Error removing section aliases", MessageType.Error);

                foreach (string[] def in
                    textBoxAliases.Lines.Select(s => s.Trim().Split('=')).Where(def => def.Length == 2))
                {
                    ClassConfig.SetGlobal("alias." + def[0].Trim(), def[1].Trim());
                }

                textBoxAliases.Tag = null;
            }
        }
    }
}