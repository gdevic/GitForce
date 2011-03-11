using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Git4Win
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
            foreach (var w in db)
            {
                string[] s = w.Split(' ');
                if (!string.IsNullOrEmpty(s[0]))
                {
                    Geometry g = new Geometry
                    {
                        Location = new Point(int.Parse(s[1]), int.Parse(s[2])),
                        Size = new Size(int.Parse(s[3]), int.Parse(s[4]))
                    };
                    wnd.Add(s[0], g);
                }
            }            
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
            if(wnd.Count==0)
                LoadGeometryDatabase();

            // Find the form in our cache and assign its location and size
            if(wnd.TryGetValue(name, out g))
            {
                form.Location = g.Location;
                form.Size = g.Size;
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
