using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Save and restore individual form's location and sizes
    /// </summary>
    static class ClassWinGeometry
    {
        private class Geometry
        {
            public Point Location;
            public Size Size;
        }

        #region Operations on a database

        /// <summary>
        /// Contains the lookup of geometry information for a particular form class
        /// </summary>
        private static readonly Dictionary<string, Geometry> wnd = new Dictionary<string, Geometry>();

        /// <summary>
        /// Loads geometry database from the program Settings.
        /// </summary>
        private static void LoadGeometryDatabase()
        {
            // Settings contains string collection of individual form information,
            // each piece of data is separated by space
            StringCollection db = Properties.Settings.Default.WindowsGeometries;
            if (db == null)
                return;

            foreach (var w in db)
            {
                // Skip a malformed entry instead of throwing: this runs from a form
                // constructor, well before there is anything around to report an error
                if (string.IsNullOrEmpty(w))
                    continue;

                string[] s = w.Split(' ');
                int x, y, width, height;
                if (s.Length != 5 || string.IsNullOrEmpty(s[0]) ||
                    !int.TryParse(s[1], out x) || !int.TryParse(s[2], out y) ||
                    !int.TryParse(s[3], out width) || !int.TryParse(s[4], out height) ||
                    width <= 0 || height <= 0)
                    continue;

                // Assign through the indexer so that a duplicated form name is tolerated
                wnd[s[0]] = new Geometry
                {
                    Location = new Point(x, y),
                    Size = new Size(width, height)
                };
            }
        }

        /// <summary>
        /// Returns true if a form placed at these bounds would be usable as it stands: it
        /// has to be reachable with the mouse and no larger than the screen it is on. A
        /// monitor disconnected since the geometry was saved, or a resolution that has
        /// dropped since, breaks one or the other.
        /// </summary>
        private static bool IsUsable(Rectangle bounds)
        {
            // Nothing to check against: leave the saved geometry alone rather than
            // moving every window to a default place
            if (Screen.AllScreens.Length == 0)
                return true;

            foreach (Screen screen in Screen.AllScreens)
            {
                // Require a usable overlap and not merely a corner pixel, so that
                // enough of the title bar remains within reach of the mouse
                Rectangle common = Rectangle.Intersect(screen.WorkingArea, bounds);
                if (common.Width >= 100 && common.Height >= 40 &&
                    bounds.Width <= screen.WorkingArea.Width && bounds.Height <= screen.WorkingArea.Height)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the bounds moved, and shrunk only as far as it has to be, so that the
        /// whole window sits inside the working area of the screen it is closest to. The
        /// size is kept wherever it still fits: dropping it would lose the layout the user
        /// arranged, and it is saved again on exit, which would make the loss permanent.
        /// </summary>
        private static Rectangle Fit(Rectangle bounds)
        {
            Rectangle work = Screen.FromRectangle(bounds).WorkingArea;
            int width = Math.Min(bounds.Width, work.Width);
            int height = Math.Min(bounds.Height, work.Height);
            int x = Math.Min(Math.Max(bounds.X, work.X), work.Right - width);
            int y = Math.Min(Math.Max(bounds.Y, work.Y), work.Bottom - height);
            return new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// Saves geometry database into the program Settings.
        /// </summary>
        public static void SaveGeometryDatabase()
        {
            StringCollection db = new StringCollection();

            // Settings contains string collection of individual form information,
            // each piece of data is separated by space
            foreach (var item in wnd)
            {
                Geometry g = item.Value;
                string s = String.Format("{0} {1} {2} {3} {4}",
                                         item.Key, g.Location.X, g.Location.Y, g.Size.Width, g.Size.Height);
                db.Add(s);
            }
            Properties.Settings.Default.WindowsGeometries = db;
        }

        #endregion

        #region Helper methods for dialog forms

        /// <summary>
        /// Restore form's location and size
        /// </summary>
        public static void Restore(Form form)
        {
            string name = form.GetType().Name;
            Geometry g;

            // If this is first invocation (hash is empty), load the window hash set
            if (wnd.Count == 0)
                LoadGeometryDatabase();

            // Find the form in our cache and assign its location and size
            if (wnd.TryGetValue(name, out g))
            {
                // Bring the saved rectangle back onto a connected screen if it no longer
                // lands on one. CenterParent is no use as a fallback here: WinForms honours
                // it only for a dialog shown with ShowDialog, so the main window and the
                // other non-modal forms would just land on the default cascade position.
                Rectangle bounds = new Rectangle(g.Location, g.Size);
                if (!IsUsable(bounds))
                    bounds = Fit(bounds);

                form.StartPosition = FormStartPosition.Manual;   // Or the assignment is ignored
                form.Location = bounds.Location;
                form.Size = bounds.Size;
                form.WindowState = FormWindowState.Normal;
            }
            else
            {
                // Form was not found in the database.
                // A good default is to center it around it's parent.
                form.StartPosition = FormStartPosition.CenterParent;
            }
        }

        /// <summary>
        /// Saves form's location and size
        /// </summary>
        public static void Save(Form form)
        {
            string name = form.GetType().Name;

            // Only save geometry if the form state is normal (not maximized or minimized)
            if (form.WindowState == FormWindowState.Normal)
            {
                // Simply update our cache
                Geometry g = new Geometry {Location = form.Location, Size = form.Size};
                if (wnd.ContainsKey(name))
                    wnd[name] = g;
                else
                    wnd.Add(name, g);
            }
        }

        #endregion
    }
}
