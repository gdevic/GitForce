using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce.Settings.Panels
{
    public partial class ControlSpecifications : UserControl, IUserSettings
    {
        private Font _font;

        public ControlSpecifications()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        /// <param name="options">All git global settings</param>
        public void Init(string[] options)
        {
            _font = Properties.Settings.Default.commitFont;

            // Load all available fonts
            LoadGlobalFonts();

            // Add them to the fonts listbox
            foreach (FontFamily fontFamily in ClassGlobals.Fonts)
            {
                Font font = new Font(fontFamily, 10, FontStyle.Regular);
                listFonts.Items.Add(font.Name);

                // Select the active font
                if (font.Name == _font.Name)
                    listFonts.SelectedIndex = listFonts.Items.Count - 1;
            }

            // Add the font sizes and select the current active font size
            List<int> sizes = new List<int>() { 8, 9, 10, 11, 12, 14, 16, 18, 20 };
            foreach (int size in sizes)
            {
                listSizes.Items.Add(size);
                if (size == (int)_font.Size)
                    listSizes.SelectedIndex = listSizes.Items.Count - 1;
            }

            // Set the wrap around columns
            numWrap1.Value = Properties.Settings.Default.commitW1;
            numWrap2.Value = Properties.Settings.Default.commitW2;

            SetExampleText();
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges()
        {
            Properties.Settings.Default.commitFont = _font;
            Properties.Settings.Default.commitW1 = numWrap1.Value;
            Properties.Settings.Default.commitW2 = numWrap2.Value;
        }

        /// <summary>
        /// Sets the example text to a specific font
        /// </summary>
        private void SetExampleText()
        {
            labelSample.Font = _font;
        }

        /// <summary>
        /// User clicked on one of the font families or sizes
        /// </summary>
        private void ListFontsSelectedIndexChanged(object sender, EventArgs e)
        {
            if (listFonts.SelectedIndex >= 0 && listSizes.SelectedIndex >= 0)
            {
                FontFamily fontFamily = ClassGlobals.Fonts[listFonts.SelectedIndex];
                int fontSize = int.Parse(listSizes.SelectedItem.ToString());
                _font = new Font(fontFamily, fontSize, FontStyle.Regular);
                SetExampleText();
            }
        }

        /// <summary>
        /// Enumerates available font families that have fixed-width pitch and stores
        /// the list of such fonts into a global variable. We use global variable so
        /// the fonts are enumerated only once.
        /// </summary>
        private void LoadGlobalFonts()
        {
            if (ClassGlobals.Fonts.Count == 0)
            {
                InstalledFontCollection fontCollection = new InstalledFontCollection();
                FontFamily[] family = fontCollection.Families;
                Graphics gr = CreateGraphics();
                foreach (FontFamily fontFamily in family)
                {
                    if (fontFamily.IsStyleAvailable(FontStyle.Regular))
                    {
                        // TODO: Is there a better way in C# to extract the fixed-width fonts?
                        Font font = new Font(fontFamily, 10, FontStyle.Regular);
                        SizeF w1 = gr.MeasureString("iiii||||....", font);
                        SizeF w2 = gr.MeasureString("XXXX____ZZZZ", font);
                        if (w1 == w2)
                            ClassGlobals.Fonts.Add(fontFamily);
                    }
                }
                gr.Dispose();
            }
        }
    }
}
