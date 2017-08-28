using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace GitForce.Settings.Panels
{
    public partial class ControlSpecifications : UserControl, IUserSettings
    {
        private Font font;

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
            font = Properties.Settings.Default.commitFont;
        }

        /// <summary>
        /// Control received a focus (true) or lost a focus (false)
        /// </summary>
        public void Focus(bool focused)
        {
            // When this tab gets the focus, rebuild the fonts.
            // This implementation will still take a time but only when the
            // user clicks on the font tab. It is also heavily cached.
            if (focused)
                CollectAndShowFonts();
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges()
        {
            Properties.Settings.Default.commitFont = font;
            Properties.Settings.Default.commitW1 = numWrap1.Value;
            Properties.Settings.Default.commitW2 = numWrap2.Value;
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
                font = new Font(fontFamily, fontSize, FontStyle.Regular);

                labelSample.Font = font;
            }
        }

        /// <summary>
        /// Enumerate all fixed-width fonts and add them to the selection listbox
        /// This call uses caching, so it only takes time to execute the very first
        /// time it is called. Also, fonts are cached globally, so the enumeration only
        /// takes place once per program run.
        /// </summary>
        void CollectAndShowFonts()
        {
            // Load all available fonts. They are cached, so a second invocation will be quick.
            LoadGlobalFonts();

            // If we already have fonts in the listbox, return
            if (listFonts.Items.Count > 0)
                return;

            // Add fonts to the fonts listbox
            foreach (FontFamily fontFamily in ClassGlobals.Fonts)
            {
                Font fontTemplate = new Font(fontFamily, 10, FontStyle.Regular);
                listFonts.Items.Add(fontTemplate.Name);

                // Select the active font
                if (fontTemplate.Name == font.Name)
                    listFonts.SelectedIndex = listFonts.Items.Count - 1;
            }

            // Add the font sizes and select the current active font size
            List<int> sizes = new List<int> { 8, 9, 10, 11, 12, 14, 16, 18, 20 };
            listSizes.Items.Clear();
            foreach (int size in sizes)
            {
                var newSize = size;
                listSizes.Items.Add(newSize);
                if (size == (int)font.Size)
                    listSizes.SelectedIndex = listSizes.Items.Count - 1;
            }

            // Set the wrap around columns
            numWrap1.Value = Properties.Settings.Default.commitW1;
            numWrap2.Value = Properties.Settings.Default.commitW2;

            labelSample.Font = font;
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
