using System.Data;
using Thumper_Custom_Level_Editor.Editor_Panels;

namespace Thumper_Custom_Level_Editor.Other_Forms
{
    public partial class DragDropItemList : Form
    {
        public string Items
        {
            get => _items;
            set
            {
                if (_items != value)
                {
                    _items = value;
                    Populate();
                }
            }
        }
        private string _items;
        public DataGridView OwnerDGV;
        public string DragSource;

        public DragDropItemList(string _itemtype, DataGridView _owner)
        {
            InitializeComponent();
            OwnerDGV = _owner;
            Items = _itemtype;
        }

        private void DragDropItemList_Load(object sender, EventArgs e)
        {

        }

        private static SolidBrush ClearColor = new(Color.Black);
        private static SolidBrush BrushWhite = new(Color.White);
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
            else
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentBackground);
                e.Paint(e.CellBounds, DataGridViewPaintParts.Focus);
            }
        }

        private void dgvPathsList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string cellval = dgvPathsList[e.ColumnIndex, e.RowIndex].Value.ToString();
            if (Items == "path")
            {
                if (TCLE.GlobalLastLvl != null) {
                    TCLE.GlobalLastLvl.lvlProperties.sublevel.paths.Add(cellval);
                    TCLE.GlobalLastLvl.LvlUpdatePaths(TCLE.GlobalLastLvl.lvlProperties.sublevel);
                    TCLE.GlobalLastLvl.SaveCheckAndWrite(false, "Added path/tunnel");
                }
            }
            else if (Items == "leaf")
            {
                TCLE.GlobalLastLvl?.AddFiletoLvl(ProjectExplorer.Files.FirstOrDefault(x => x.Name == cellval)?.FullName);
            }
            else if (Items == "lvl") {
                TCLE.GlobalLastGate?.AddFileToGate(ProjectExplorer.Files.FirstOrDefault(x => x.Name == cellval)?.FullName);
            }
            else if (Items == "lvlgate") {
                TCLE.GlobalLastMaster?.AddFiletoMaster(ProjectExplorer.Files.FirstOrDefault(x => x.Name == cellval)?.FullName);
            }
        }

        private Rectangle dragBoxFromMouseDown;
        private List<string> RowsToMove;
        private int rowIndexFromMouseDown;
        private void lvlLeafList_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (RowsToMove == null && dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // Proceed with the drag and drop, passing in the list item.
                    List<DataGridViewCell> SelectedRows = dgvPathsList.SelectedCells.Cast<DataGridViewCell>().ToList();
                    SelectedRows.Sort((row1, row2) => row2.RowIndex.CompareTo(row1.RowIndex));
                    RowsToMove = SelectedRows.Select(x => x.Value.ToString()).ToList();

                    TCLE.DragSource = DragSource;

                    DragDropEffects dropEffect = dgvPathsList.DoDragDrop(RowsToMove, DragDropEffects.Move);
                    RowsToMove = null;
                    TCLE.DragSource = "none";
                }
            }
        }

        private List<int> SelectedRows = new();
        private void lvlLeafList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = dgvPathsList.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown is -1)
            {
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
                return;
            }

            if (dgvPathsList.Rows[rowIndexFromMouseDown].Selected)
                SelectedRows = dgvPathsList.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
            else
                SelectedRows.Clear();

            // Remember the point where the mouse down occurred. 
            // The DragSize indicates the size that the mouse can move 
            // before a drag event should be started.                
            Size dragSize = SystemInformation.DragSize;

            // Create a rectangle using the DragSize, with the mouse position being
            // at the center of the rectangle.
            dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);

        }

        private void lvlLeafPaths_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        #region Methods
        public void Populate()
        {
            dgvPathsList.Rows.Clear();
            SelectedRows.Clear();
            //
            if (Items == "path")
            {
                btnExternal.Visible = false;
                DragSource = "PathList";
                dgvPathsList.RowTemplate.DefaultCellStyle.BackColor = Color.DarkBlue;
                this.Text = "Add Tunnel/Path";
                TCLE.LvlPaths.Sort();
                foreach (string _s in TCLE.LvlPaths)
                    dgvPathsList.Rows.Add(_s);
            }
            else if (Items == "lvl")
            {
                btnExternal.Visible = true;
                DragSource = "LvlList";
                dgvPathsList.RowTemplate.DefaultCellStyle.BackColor = Color.Green;
                this.Text = "Add Lvl";
                foreach (FileInfo lvl in ProjectExplorer.Files)
                {
                    if (lvl.Extension is ".lvl")
                        dgvPathsList.Rows.Add(lvl.Name);
                }
            }
            else if (Items == "lvlgate")
            {
                btnExternal.Visible = true;
                DragSource = "LvlGateList";
                dgvPathsList.RowTemplate.DefaultCellStyle.BackColor = Color.Green;
                this.Text = "Add Lvl/Gate";
                foreach (FileInfo lvl in ProjectExplorer.Files)
                {
                    if (lvl.Extension is ".lvl")
                    {
                        dgvPathsList.Rows.Add(lvl.Name);
                        dgvPathsList.Rows[^1].DefaultCellStyle.BackColor = Color.Green;
                    }
                    else if (lvl.Extension is ".gate")
                    {
                        dgvPathsList.Rows.Add(lvl.Name);
                        dgvPathsList.Rows[^1].DefaultCellStyle.BackColor = Color.Orange;
                    }
                }
            }
            else if (Items == "leaf")
            {
                btnExternal.Visible = true;
                DragSource = "LeafList";
                dgvPathsList.RowTemplate.DefaultCellStyle.BackColor = Color.Green;
                this.Text = "Add Leaf";
                foreach (FileInfo leaf in ProjectExplorer.Files)
                {
                    if (leaf.Extension is ".leaf")
                        dgvPathsList.Rows.Add(leaf.Name);
                }
            }
        }
        #endregion

        private void DragDropItemList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!TCLE.IsClosing)
                e.Cancel = true;
            this.Hide();
        }

        private void dgvPathsList_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPathsList.RowCount <= 0)
                return;
            foreach (int dgvr in SelectedRows)
            {
                dgvPathsList.Rows[dgvr].Selected = true;
            }
        }

        private void dgvPathsList_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || Items != "path")
                return;
            //calculate position to show the tunnel image
            Point mouse = TCLE.Instance.PointToClient(System.Windows.Forms.Cursor.Position);
            int height = mouse.Y + 75 > TCLE.Instance.Height ? TCLE.Instance.Height - 150 : mouse.Y - 75;
            //get image of tunnel
            string pathname = (string)(sender as DataGridView).Rows[e.RowIndex].Cells[0].GetEditedFormattedValue(e.RowIndex, DataGridViewDataErrorContexts.Commit);
            TCLE.Instance.pictureTunnelViewer.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject($"path_{pathname.Replace(".path", "")}");
            //show the image
            TCLE.Instance.pictureTunnelViewer.Visible = true;
            TCLE.Instance.pictureTunnelViewer.Location = new Point(mouse.X + (this.Width - this.PointToClient(System.Windows.Forms.Cursor.Position).X), height);
            TCLE.Instance.pictureTunnelViewer.BringToFront();
        }

        private void dgvPathsList_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            TCLE.Instance.pictureTunnelViewer.Visible = false;
        }
    }
}
