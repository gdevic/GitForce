using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Extension to the standard Tab control giving us a method of hiding
    /// the tab control bar.
    /// </summary>
    public class TabEx : TabControl
    {
        [Category("Appearance")]
        [Description("Shows or hides the tab control")]
        [DefaultValue(typeof(bool),"false")]
        public bool ShowTabs { get; set; }

        public void UpdateTabState()
        {
            // We change this property to make tab redraw itself and show/hide
            // its tab control bar. That means we can only use Normal property for now.
            Appearance = ShowTabs ? TabAppearance.Normal : TabAppearance.FlatButtons;
        }

        protected override void WndProc(ref Message m)
        {
            // In design mode, show the tabs
            if(DesignMode)
                base.WndProc(ref m);
            else
            {
                // Hide the tabs if the message is TCM_ADJUSTRECT
                // and our control variable ShowTabs is false
                if (m.Msg == NativeMethods.TCM_ADJUSTRECT && !ShowTabs)
                    m.Result = (IntPtr)1;
                else
                    base.WndProc(ref m);
            }
        }
    }
}
