using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Listview that lets the user reorder items by doing drag and drop.
    /// This functionality is missing from the standard list view control.
    /// http://www.codeproject.com/Articles/4576/Drag-and-Drop-ListView-row-reordering
    /// </summary>
    public class ListViewEx : ListView
    {
        private const string reorder = "Reorder";

        private bool allowRowReorder = true;
        public bool AllowRowReorder
        {
            get
            {
                return this.allowRowReorder;
            }
            set
            {
                this.allowRowReorder = value;
                base.AllowDrop = value;
            }
        }

        public new SortOrder Sorting
        {
            get
            {
                return SortOrder.None;
            }
            set
            {
                base.Sorting = SortOrder.None;
            }
        }

        /// <summary>
        /// Internal move vs. the external drop event
        /// </summary>
        private bool internalMove = true;

        public ListViewEx()
            : base()
        {
            this.AllowRowReorder = true;

            // Optimize drawing to avoid flicker
            SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            // GD: A special case when we are dropping an item from the outside the control
            if (!internalMove)
            {
                base.OnDragDrop(e);
                return;
            }
            if (!this.AllowRowReorder)
            {
                return;
            }
            if (base.SelectedItems.Count == 0)
            {
                return;
            }
            Point cp = base.PointToClient(new Point(e.X, e.Y));
            ListViewItem dragToItem = base.GetItemAt(cp.X, cp.Y);
            if (dragToItem == null)
            {
                return;
            }
            int dropIndex = dragToItem.Index;
            if (dropIndex > base.SelectedItems[0].Index)
            {
                dropIndex++;
            }
            ArrayList insertItems = new ArrayList(base.SelectedItems.Count);
            foreach (ListViewItem item in base.SelectedItems)
            {
                insertItems.Add(item.Clone());
            }
            for (int i = insertItems.Count - 1; i >= 0; i--)
            {
                ListViewItem insertItem =
                    (ListViewItem)insertItems[i];
                base.Items.Insert(dropIndex, insertItem);
            }
            foreach (ListViewItem removeItem in base.SelectedItems)
            {
                base.Items.Remove(removeItem);
            }

            // GD: In the original code, this call was at the start of OnDragDrop() function.
            // That did not work for our purpose since we were not getting an updated list
            // in our DragDrop handler. By moving it to the end, the list gets changed first
            // and only then we get called, so we can read the reordered list.
            base.OnDragDrop(e);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            // GD: A special case when we are dropping an item from the outside the control
            if (!internalMove)
            {
                base.OnDragOver(e);
                return;
            }
            if (!this.AllowRowReorder)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            if (!e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            Point cp = base.PointToClient(new Point(e.X, e.Y));
            ListViewItem hoverItem = base.GetItemAt(cp.X, cp.Y);
            if (hoverItem == null)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            foreach (ListViewItem moveItem in base.SelectedItems)
            {
                if (moveItem.Index == hoverItem.Index)
                {
                    e.Effect = DragDropEffects.None;
                    hoverItem.EnsureVisible();
                    return;
                }
            }
            base.OnDragOver(e);
            String text = (String)e.Data.GetData(reorder.GetType());
            if (text.CompareTo(reorder) == 0)
            {
                e.Effect = DragDropEffects.Move;
                hoverItem.EnsureVisible();
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            // GD: A special case when we are dropping an item from the outside the control
            //     we use Copy effect to track that case
            internalMove = e.Effect != DragDropEffects.Copy;
            if (!internalMove)
                return;
            if (!this.AllowRowReorder)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            if (!e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            base.OnDragEnter(e);
            String text = (String)e.Data.GetData(reorder.GetType());
            if (text.CompareTo(reorder) == 0)
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            base.OnItemDrag(e);
            if (!this.AllowRowReorder)
            {
                return;
            }
            base.DoDragDrop(reorder, DragDropEffects.Move);
        }
    }
}
