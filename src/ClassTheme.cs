using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace GitForce
{
    /// <summary>
    /// Light, dark and system (following the OS app mode) color themes.
    /// The theme is resolved once, at startup, and stays the same while the program runs.
    /// Controls are designed with system colors. The light theme leaves them as they are. The dark
    /// theme replaces every known color a control carries with a dark counterpart and restyles the
    /// controls that a color alone does not change. Forms hook in through Apply(), which also
    /// themes the controls that are added to them later.
    /// </summary>
    static class ClassTheme
    {
        public const string ModeLight = "Light";
        public const string ModeDark = "Dark";
        public const string ModeSystem = "System";

        /// <summary>
        /// True when the dark palette is in effect
        /// </summary>
        public static bool IsDark { get; private set; }

        #region Palette

        /// <summary>
        /// Dark counterparts of the known colors that controls and our own code use
        /// </summary>
        private static readonly Dictionary<KnownColor, Color> darkOf = new Dictionary<KnownColor, Color>
        {
            { KnownColor.Control, Color.FromArgb(45, 45, 48) },
            { KnownColor.Window, Color.FromArgb(30, 30, 30) },
            { KnownColor.Info, Color.FromArgb(58, 58, 46) },
            { KnownColor.ControlText, Color.FromArgb(241, 241, 241) },
            { KnownColor.WindowText, Color.FromArgb(243, 243, 243) },
            { KnownColor.GrayText, Color.FromArgb(150, 150, 150) },
            { KnownColor.Black, Color.FromArgb(239, 239, 239) },
            { KnownColor.Red, Color.FromArgb(255, 110, 110) },
            { KnownColor.Green, Color.FromArgb(100, 220, 100) },
            { KnownColor.Blue, Color.FromArgb(120, 170, 255) },
            { KnownColor.Purple, Color.FromArgb(200, 140, 255) },
            { KnownColor.Brown, Color.FromArgb(222, 160, 110) },
            { KnownColor.DarkOliveGreen, Color.FromArgb(170, 205, 115) },
        };

        private static readonly Color buttonBorder = Color.FromArgb(100, 100, 106);
        private static readonly Color tabNormal = Color.FromArgb(36, 36, 38);
        private static readonly Color tabSelected = Color.FromArgb(62, 62, 66);
        private static readonly Color treeLine = Color.FromArgb(110, 110, 116);
        private static readonly Color linkNormal = Color.FromArgb(120, 170, 255);
        private static readonly Color linkActive = Color.FromArgb(165, 205, 255);
        private static readonly Color linkVisited = Color.FromArgb(200, 140, 255);

        /// <summary>
        /// Width of the check mark or radio glyph of a check box or a radio button
        /// </summary>
        private const int glyphWidth = 13;

        private static Color DarkText
        {
            get { return darkOf[KnownColor.ControlText]; }
        }

        private static Color DisabledText
        {
            get { return darkOf[KnownColor.GrayText]; }
        }

        /// <summary>
        /// Returns the color to use under the current theme for a color that was designed for the
        /// light theme. Colors outside of the palette are returned as they are.
        /// </summary>
        public static Color Map(Color c)
        {
            Color mapped;
            if (IsDark && c.IsKnownColor && darkOf.TryGetValue(c.ToKnownColor(), out mapped))
                return mapped;
            return c;
        }

        private static readonly Dictionary<Color, SolidBrush> brushes = new Dictionary<Color, SolidBrush>();

        /// <summary>
        /// Returns a cached brush of the color mapped for the current theme. The brush is owned by
        /// this class and must not be disposed by the caller.
        /// </summary>
        public static Brush GetBrush(Color c)
        {
            SolidBrush brush;
            if (!brushes.TryGetValue(c, out brush))
            {
                brush = new SolidBrush(Map(c));
                brushes.Add(c, brush);
            }
            return brush;
        }

        #endregion

        #region Theme selection

        /// <summary>
        /// Resolves the theme from the settings and installs the menu renderer. Called once at
        /// startup, before any form is created.
        /// </summary>
        public static void Init()
        {
            IsDark = Resolve();
            if (IsDark)
                ToolStripManager.Renderer = new DarkRenderer();
        }

        private static bool Resolve()
        {
            // The dark palette would replace the colors that High Contrast users chose
            try
            {
                if (SystemInformation.HighContrast)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            string theme = Properties.Settings.Default.Theme;
            if (theme == ModeDark)
                return true;
            if (theme == ModeSystem)
                return SystemUsesDarkApps();
            return false;
        }

        /// <summary>
        /// Returns true if the OS is set to the dark app mode. Windows 10 and later only.
        /// </summary>
        private static bool SystemUsesDarkApps()
        {
            if (ClassUtils.IsMono())
                return false;
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    object value = (key != null) ? key.GetValue("AppsUseLightTheme") : null;
                    return ((value is int) && ((int)value == 0));
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Applying the theme to forms and controls

        /// <summary>
        /// Window procedures that paint the background of a tab control
        /// </summary>
        private static readonly ConditionalWeakTable<TabControl, TabBackground> tabBackgrounds =
            new ConditionalWeakTable<TabControl, TabBackground>();

        /// <summary>
        /// Themes a form and all of its controls, present and added later. Call it right after
        /// InitializeComponent() of every form. It does nothing in the light theme.
        /// </summary>
        public static void Apply(Form form)
        {
            if (IsDark)
                Walk(form);
        }

        private static void Walk(Control c)
        {
            Style(c);
            c.ControlAdded -= OnControlAdded;
            c.ControlAdded += OnControlAdded;
            foreach (Control child in c.Controls)
                Walk(child);
        }

        private static void OnControlAdded(object sender, ControlEventArgs e)
        {
            Walk(e.Control);
        }

        /// <summary>
        /// Returns true for a button that is drawn with a picture. The pictures are made for the
        /// system button face, so such a button keeps it.
        /// </summary>
        private static bool HasPicture(Control c)
        {
            ButtonBase button = c as ButtonBase;
            return (button != null) &&
                ((button.Image != null) || (button.BackgroundImage != null) || (button.ImageList != null));
        }

        private static void Style(Control c)
        {
            if (HasPicture(c))
                return;

            ToolStrip strip = c as ToolStrip;
            if (strip != null)
            {
                // The system renderer would ignore the dark colors
                if (strip.RenderMode == ToolStripRenderMode.System)
                    strip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
            }
            else
            {
                // Tool strips are painted by the renderer, which has its own colors
                Color back = Map(c.BackColor);
                if (back != c.BackColor)
                    c.BackColor = back;
                Color fore = Map(c.ForeColor);
                if (fore != c.ForeColor)
                    c.ForeColor = fore;
            }
            StyleSpecial(c);
        }

        /// <summary>
        /// Styling that a color change alone does not achieve
        /// </summary>
        private static void StyleSpecial(Control c)
        {
            Button button = c as Button;
            if (button != null)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderColor = buttonBorder;
                button.UseVisualStyleBackColor = false;
            }

            // The system draws the text of a disabled control in a shade of its back color
            if ((c is Label) || (c is ButtonBase))
            {
                c.Paint -= DisabledTextPaint;
                c.Paint += DisabledTextPaint;
            }

            // A group box draws its title in the color it inherits from its parent only when the
            // color is set on the group box itself
            GroupBox group = c as GroupBox;
            if (group != null)
                group.ForeColor = DarkText;

            // A visual-style combo box ignores its colors
            ComboBox combo = c as ComboBox;
            if (combo != null)
                combo.FlatStyle = FlatStyle.Flat;

            TabPage page = c as TabPage;
            if (page != null)
                page.BackColor = darkOf[KnownColor.Control];

            // The system draws the tab headers unless the control does
            TabControl tabs = c as TabControl;
            if (tabs != null)
            {
                tabs.DrawItem -= TabDrawItem;
                tabs.DrawItem += TabDrawItem;
                tabs.DrawMode = TabDrawMode.OwnerDrawFixed;

                TabBackground background;
                if (!ClassUtils.IsMono() && !tabBackgrounds.TryGetValue(tabs, out background))
                    tabBackgrounds.Add(tabs, new TabBackground(tabs));
            }

            LinkLabel link = c as LinkLabel;
            if (link != null)
            {
                link.LinkColor = linkNormal;
                link.ActiveLinkColor = linkActive;
                link.VisitedLinkColor = linkVisited;
            }

            TreeView tree = c as TreeView;
            if (tree != null)
            {
                tree.LineColor = treeLine;
                MapNodes(tree.Nodes);
            }

            // A tree view is left out: with the native dark theme every operation on a tree with
            // tens of thousands of nodes is several times slower
            if ((c is Form) || (c is ListView) || (c is ListBox) || (c is TextBoxBase))
                ApplyNative(c);
        }

        private static void MapNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                Color back = Map(node.BackColor);
                if (back != node.BackColor)
                    node.BackColor = back;
                Color fore = Map(node.ForeColor);
                if (fore != node.ForeColor)
                    node.ForeColor = fore;
                MapNodes(node.Nodes);
            }
        }

        /// <summary>
        /// The system draws the text of a disabled control in a dark shade of its back color, which
        /// cannot be read on a dark one: draw it again in the disabled text color
        /// </summary>
        private static void DisabledTextPaint(object sender, PaintEventArgs e)
        {
            Control c = sender as Control;
            if ((c == null) || c.Enabled || (c.RightToLeft == RightToLeft.Yes))
                return;

            Rectangle fill;
            Rectangle text;
            ContentAlignment align;
            bool mnemonic;
            Label label = c as Label;
            if (label != null)
            {
                if (label.Image != null)
                    return;
                int border = (label.BorderStyle == BorderStyle.Fixed3D) ? 2 :
                    ((label.BorderStyle == BorderStyle.FixedSingle) ? 1 : 0);
                fill = Rectangle.Inflate(label.ClientRectangle, -border, -border);
                text = new Rectangle(fill.X + label.Padding.Left, fill.Y + label.Padding.Top,
                    fill.Width - label.Padding.Horizontal, fill.Height - label.Padding.Vertical);
                align = label.TextAlign;
                mnemonic = label.UseMnemonic;
            }
            else if (c is Button)
            {
                Button button = (Button)c;
                fill = text = Rectangle.Inflate(button.ClientRectangle, -1, -1);
                align = button.TextAlign;
                mnemonic = button.UseMnemonic;
            }
            else if ((c is CheckBox) || (c is RadioButton))
            {
                // Only the common layout is handled: a standard glyph on the left, then the text
                ButtonBase check = (ButtonBase)c;
                ContentAlignment glyphAt = (c is CheckBox) ? ((CheckBox)c).CheckAlign : ((RadioButton)c).CheckAlign;
                bool normal = (c is CheckBox) ? (((CheckBox)c).Appearance == Appearance.Normal) :
                    (((RadioButton)c).Appearance == Appearance.Normal);
                bool left = ((glyphAt == ContentAlignment.TopLeft) || (glyphAt == ContentAlignment.MiddleLeft) ||
                    (glyphAt == ContentAlignment.BottomLeft));
                if (!normal || !left || (check.FlatStyle == FlatStyle.System))
                    return;
                fill = new Rectangle(glyphWidth + 2, 0, c.Width - glyphWidth - 2, c.Height);
                text = new Rectangle(glyphWidth + 6, 0, c.Width - glyphWidth - 6, c.Height);
                align = check.TextAlign;
                mnemonic = check.UseMnemonic;
            }
            else
                return;

            // A transparent control shows what is behind it: fill with that
            Color back = c.BackColor;
            for (Control p = c.Parent; (back.A < 255) && (p != null); p = p.Parent)
                back = p.BackColor;
            if (back.A < 255)
                back = darkOf[KnownColor.Control];
            using (SolidBrush brush = new SolidBrush(back))
                e.Graphics.FillRectangle(brush, fill);

            TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix |
                TextFormatFlags.EndEllipsis | TextFormatFlags.WordBreak;
            flags |= AlignFlags(align);

            // An ampersand marks a mnemonic and is not part of the visible text; a doubled one is
            // a literal ampersand
            string caption = c.Text;
            if (mnemonic)
                caption = caption.Replace("&&", "\u0001").Replace("&", "").Replace("\u0001", "&");
            TextRenderer.DrawText(e.Graphics, caption, c.Font, text, DisabledText, flags);
        }

        /// <summary>
        /// Returns the text format flags that place text the way a content alignment says
        /// </summary>
        private static TextFormatFlags AlignFlags(ContentAlignment align)
        {
            TextFormatFlags flags;
            if ((align == ContentAlignment.TopLeft) || (align == ContentAlignment.TopCenter) ||
                (align == ContentAlignment.TopRight))
                flags = TextFormatFlags.Top;
            else if ((align == ContentAlignment.BottomLeft) || (align == ContentAlignment.BottomCenter) ||
                (align == ContentAlignment.BottomRight))
                flags = TextFormatFlags.Bottom;
            else
                flags = TextFormatFlags.VerticalCenter;

            if ((align == ContentAlignment.TopLeft) || (align == ContentAlignment.MiddleLeft) ||
                (align == ContentAlignment.BottomLeft))
                flags |= TextFormatFlags.Left;
            else if ((align == ContentAlignment.TopRight) || (align == ContentAlignment.MiddleRight) ||
                (align == ContentAlignment.BottomRight))
                flags |= TextFormatFlags.Right;
            else
                flags |= TextFormatFlags.HorizontalCenter;
            return flags;
        }

        private static void TabDrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabs = sender as TabControl;
            if ((tabs == null) || (e.Index < 0) || (e.Index >= tabs.TabPages.Count))
                return;
            using (SolidBrush brush = new SolidBrush((e.Index == tabs.SelectedIndex) ? tabSelected : tabNormal))
                e.Graphics.FillRectangle(brush, e.Bounds);
            TextRenderer.DrawText(e.Graphics, tabs.TabPages[e.Index].Text, tabs.Font, e.Bounds, DarkText,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        /// <summary>
        /// The system paints the area around the tab headers with a light color that a control
        /// property cannot change: erase the background with the dark color instead
        /// </summary>
        private sealed class TabBackground : NativeWindow
        {
            private readonly TabControl tabs;

            public TabBackground(TabControl tabs)
            {
                this.tabs = tabs;
                tabs.HandleCreated += OnHandleCreated;
                tabs.HandleDestroyed += OnHandleDestroyed;
                Attach();
            }

            private void Attach()
            {
                if (tabs.IsHandleCreated && (Handle == IntPtr.Zero))
                    AssignHandle(tabs.Handle);
            }

            private void OnHandleCreated(object sender, EventArgs e)
            {
                Attach();
            }

            private void OnHandleDestroyed(object sender, EventArgs e)
            {
                ReleaseHandle();
            }

            protected override void WndProc(ref Message m)
            {
                if ((m.Msg == NativeMethods.WM_ERASEBKGND) && (m.WParam != IntPtr.Zero))
                {
                    using (Graphics g = Graphics.FromHdc(m.WParam))
                    using (SolidBrush brush = new SolidBrush(darkOf[KnownColor.Control]))
                        g.FillRectangle(brush, tabs.ClientRectangle);
                    m.Result = new IntPtr(1);
                    return;
                }
                base.WndProc(ref m);
            }
        }

        #endregion

        #region Native window theming (Windows only)

        /// <summary>
        /// Dark title bar for forms and dark scroll bars and selection for lists and text boxes.
        /// These need a window handle, which may not exist yet.
        /// </summary>
        private static void ApplyNative(Control c)
        {
            if (ClassUtils.IsMono())
                return;
            c.HandleCreated -= OnHandleCreated;
            c.HandleCreated += OnHandleCreated;
            if (c.IsHandleCreated)
                SetNative(c);
        }

        private static void OnHandleCreated(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (c != null)
                SetNative(c);
        }

        private static void SetNative(Control c)
        {
            try
            {
                if (c is Form)
                {
                    int dark = 1;
                    if (NativeMethods.DwmSetWindowAttribute(c.Handle,
                        NativeMethods.DWMWA_USE_IMMERSIVE_DARK_MODE, ref dark, sizeof(int)) != 0)
                        NativeMethods.DwmSetWindowAttribute(c.Handle,
                            NativeMethods.DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1, ref dark, sizeof(int));
                }
                else
                {
                    // The items view theme is the one that draws the selected row of a list that
                    // has lost the focus in a dark color
                    string theme = (c is ListView) ? "DarkMode_ItemsView" : "DarkMode_Explorer";
                    NativeMethods.SetWindowTheme(c.Handle, theme, null);
                }
            }
            catch (Exception)
            {
                // The API does not exist on this version of Windows: the control stays light
            }
        }

        #endregion

        #region Menus and tool bars

        private sealed class DarkColors : ProfessionalColorTable
        {
            private static readonly Color bar = Color.FromArgb(45, 45, 48);
            private static readonly Color popup = Color.FromArgb(37, 37, 38);
            private static readonly Color hover = Color.FromArgb(70, 70, 76);
            private static readonly Color press = Color.FromArgb(86, 86, 94);
            private static readonly Color edge = Color.FromArgb(92, 92, 97);
            private static readonly Color check = Color.FromArgb(62, 94, 140);
            private static readonly Color separator = Color.FromArgb(74, 74, 80);

            public override Color ToolStripDropDownBackground { get { return popup; } }
            public override Color MenuBorder { get { return edge; } }
            public override Color MenuItemBorder { get { return edge; } }
            public override Color MenuItemSelected { get { return hover; } }
            public override Color MenuItemSelectedGradientBegin { get { return hover; } }
            public override Color MenuItemSelectedGradientEnd { get { return hover; } }
            public override Color MenuItemPressedGradientBegin { get { return press; } }
            public override Color MenuItemPressedGradientMiddle { get { return press; } }
            public override Color MenuItemPressedGradientEnd { get { return press; } }
            public override Color MenuStripGradientBegin { get { return bar; } }
            public override Color MenuStripGradientEnd { get { return bar; } }
            public override Color ToolStripGradientBegin { get { return bar; } }
            public override Color ToolStripGradientMiddle { get { return bar; } }
            public override Color ToolStripGradientEnd { get { return bar; } }
            public override Color ToolStripBorder { get { return bar; } }
            public override Color ToolStripContentPanelGradientBegin { get { return bar; } }
            public override Color ToolStripContentPanelGradientEnd { get { return bar; } }
            public override Color ToolStripPanelGradientBegin { get { return bar; } }
            public override Color ToolStripPanelGradientEnd { get { return bar; } }
            public override Color StatusStripGradientBegin { get { return bar; } }
            public override Color StatusStripGradientEnd { get { return bar; } }
            public override Color RaftingContainerGradientBegin { get { return bar; } }
            public override Color RaftingContainerGradientEnd { get { return bar; } }
            public override Color OverflowButtonGradientBegin { get { return bar; } }
            public override Color OverflowButtonGradientMiddle { get { return bar; } }
            public override Color OverflowButtonGradientEnd { get { return bar; } }
            public override Color ImageMarginGradientBegin { get { return popup; } }
            public override Color ImageMarginGradientMiddle { get { return popup; } }
            public override Color ImageMarginGradientEnd { get { return popup; } }
            public override Color ImageMarginRevealedGradientBegin { get { return popup; } }
            public override Color ImageMarginRevealedGradientMiddle { get { return popup; } }
            public override Color ImageMarginRevealedGradientEnd { get { return popup; } }
            public override Color SeparatorDark { get { return separator; } }
            public override Color SeparatorLight { get { return separator; } }
            public override Color GripDark { get { return edge; } }
            public override Color GripLight { get { return bar; } }
            public override Color ButtonSelectedBorder { get { return edge; } }
            public override Color ButtonSelectedHighlight { get { return hover; } }
            public override Color ButtonSelectedHighlightBorder { get { return edge; } }
            public override Color ButtonSelectedGradientBegin { get { return hover; } }
            public override Color ButtonSelectedGradientMiddle { get { return hover; } }
            public override Color ButtonSelectedGradientEnd { get { return hover; } }
            public override Color ButtonPressedBorder { get { return edge; } }
            public override Color ButtonPressedHighlight { get { return press; } }
            public override Color ButtonPressedHighlightBorder { get { return edge; } }
            public override Color ButtonPressedGradientBegin { get { return press; } }
            public override Color ButtonPressedGradientMiddle { get { return press; } }
            public override Color ButtonPressedGradientEnd { get { return press; } }
            public override Color ButtonCheckedHighlight { get { return check; } }
            public override Color ButtonCheckedHighlightBorder { get { return edge; } }
            public override Color ButtonCheckedGradientBegin { get { return check; } }
            public override Color ButtonCheckedGradientMiddle { get { return check; } }
            public override Color ButtonCheckedGradientEnd { get { return check; } }
            public override Color CheckBackground { get { return check; } }
            public override Color CheckSelectedBackground { get { return check; } }
            public override Color CheckPressedBackground { get { return check; } }
        }

        /// <summary>
        /// Draws the text and arrows of menu and tool strip items in light colors
        /// </summary>
        private sealed class DarkRenderer : ToolStripProfessionalRenderer
        {
            public DarkRenderer() : base(new DarkColors())
            {
                RoundedEdges = false;
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                e.TextColor = e.Item.Enabled ? DarkText : DisabledText;
                base.OnRenderItemText(e);
            }

            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
            {
                e.ArrowColor = e.Item.Enabled ? DarkText : DisabledText;
                base.OnRenderArrow(e);
            }

            /// <summary>
            /// The check mark is a black glyph: draw a light copy of it
            /// </summary>
            protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
            {
                if (e.Image == null)
                {
                    base.OnRenderItemCheck(e);
                    return;
                }
                using (Bitmap light = Recolor(e.Image))
                    base.OnRenderItemCheck(
                        new ToolStripItemImageRenderEventArgs(e.Graphics, e.Item, light, e.ImageRectangle));
            }

            private static Bitmap Recolor(Image source)
            {
                Bitmap bitmap = new Bitmap(source.Width, source.Height);
                ColorMatrix matrix = new ColorMatrix(new float[][]
                {
                    new float[] { 0, 0, 0, 0, 0 },
                    new float[] { 0, 0, 0, 0, 0 },
                    new float[] { 0, 0, 0, 0, 0 },
                    new float[] { 0, 0, 0, 1, 0 },
                    new float[] { 0.95f, 0.95f, 0.95f, 0, 1 },
                });
                using (Graphics g = Graphics.FromImage(bitmap))
                using (ImageAttributes attributes = new ImageAttributes())
                {
                    attributes.SetColorMatrix(matrix);
                    g.DrawImage(source, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attributes);
                }
                return bitmap;
            }
        }

        #endregion
    }
}
