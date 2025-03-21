using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor.Other_Forms
{
    public partial class DragDropItemList : Form
    {
        public string Items;
        public DataGridView OwnerDGV;

        public DragDropItemList(string _itemtype, DataGridView _owner)
        {
            InitializeComponent();
            OwnerDGV = _owner;
            Items = _itemtype;
            Populate();
        }

        private void DragDropItemList_Load(object sender, EventArgs e)
        {

        }

        private static SolidBrush ClearColor = new SolidBrush(Color.Black);
        private static SolidBrush BrushWhite = new SolidBrush(Color.White);
        private void lvlLeafList_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            e.Handled = true;
            Rectangle bounds = e.RowBounds;
            bounds.X += 2;
            bounds.Y += 2;
            bounds.Width -= 4;
            bounds.Height -= 4;
            e.Graphics.FillRectangle(ClearColor, e.RowBounds);
            DataGridView dgv = sender as DataGridView;

            if (dgv.Rows[e.RowIndex].Selected)
                e.Graphics.FillRoundedRectangle(BrushWhite, new Rectangle(bounds.X - 1, bounds.Y - 1, bounds.Width + 2, bounds.Height + 2), 8);
            e.Graphics.FillRoundedRectangle(new SolidBrush(TCLE.Blend(e.InheritedRowStyle.BackColor, Color.Black, (dgv.Rows[e.RowIndex].Selected ? 1 : 0.6))), bounds, 8);

            e.PaintCells(e.RowBounds, DataGridViewPaintParts.ContentForeground);
        }

        private void lvlLeafList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.Handled = true;
            if (e.RowIndex == -1)
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
            else {
                if (e.ColumnIndex is 0) {
                    if (Items is "leaf")
                        e.Graphics.DrawImage(Properties.Resources.editor_leaf, e.CellBounds.X, e.CellBounds.Y, 16, 16);
                }
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentBackground);
                e.Paint(e.CellBounds, DataGridViewPaintParts.Focus);
            }
        }

        private void dgvPathsList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private Rectangle dragBoxFromMouseDown;
        private List<string> RowsToMove;
        private int rowIndexFromMouseDown;
        private void lvlLeafList_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                // If the mouse moves outside the rectangle, start the drag.
                if (RowsToMove == null && dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y)) {
                    // Proceed with the drag and drop, passing in the list item.
                    RowsToMove = dgvPathsList.SelectedCells.Cast<DataGridViewCell>().Select(x => x.Value.ToString()).ToList();
                    ///dgvPathsList.ClearSelection();
                    //RowToMove.DefaultCellStyle.BackColor = SelectColor;
                    DragDropEffects dropEffect = OwnerDGV.DoDragDrop(RowsToMove, DragDropEffects.Copy);
                    RowsToMove = null;
                }
            }
        }
        private void lvlLeafList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = dgvPathsList.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1) {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void lvlLeafPaths_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        #region Methods
        public void Populate()
        {
            if (Items == "path") {
                dgvPathsList.RowTemplate.DefaultCellStyle.BackColor = Color.DarkBlue;
                this.Text = "Add Tunnel/Path";
                TCLE.LvlPaths.Sort();
                foreach (string _s in TCLE.LvlPaths)
                    dgvPathsList.Rows.Add(_s);
            }
        }
        #endregion
    }
}
