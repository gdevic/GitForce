using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Subclass a standard ListView to be able to set some internal flags
    /// </summary>
    public class ListViewEx : ListView
    {
        public ListViewEx()
            : base()
        {
            // Optimize drawing to avoid flicker
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }
    }
}
