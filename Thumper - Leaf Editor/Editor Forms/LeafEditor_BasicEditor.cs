using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_LeafEditor
    {
        #region VARIABLES
        private static string[] LaneParams = new[] { "a01", "a02", "ent", "z01", "z02" };
        private static string[] LaneNames = new[] { "visibla01", "visibla02", "visible", "visiblz01", "visiblz02" };
        private string BasicEditorSelectedObject = "select";
        private decimal? BasicEditorClickValue = 1m;
        private ToolStripItem BasicEditorSelectedButton;
        #endregion
        #region PAINTING
        private void dgvMasterView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.Handled = true;
            if (dgvMasterView[e.ColumnIndex, e.RowIndex].Selected) {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.LightSkyBlue)), e.CellBounds);
            }
            if (Properties.Settings.Default.LeafOptionShowGrid)
                e.Graphics.DrawRectangle(Pens.DarkGray, e.CellBounds);
        }

        private void dgvMasterView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            e.Handled = true;
            e.PaintCells(e.RowBounds, e.PaintParts);
        }

        private void dgvMasterView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
        }
        #endregion
        #region ZOOM and SCROLL
        private void dgvMasterView_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.Width = LeafMasterView.Width;
            e.Column.FillWeight = 0.001f;
        }

        private void dgvMasterView_SelectionChanged(object sender, EventArgs e)
        {
            trackEditor.ClearSelection();
            foreach (int column in dgvMasterView.SelectedCells.Cast<DataGridViewCell>().Select(x => x.ColumnIndex).Distinct()) {
                foreach (DataGridViewRow dgvr in trackEditor.Rows) {
                    dgvr.Cells[column + FrozenColumnOffset].Selected = true;
                }
            }
            trackEditor.Invalidate();
        }
        private void dgvMasterView_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int horiz = trackZoom.Value;
            int scrollLines = SystemInformation.MouseWheelScrollLines;
            //handle horizontal scroll
            if (ModifierKeys is not Keys.Control and not Keys.Shift) {
                if (dgvMasterView.FirstDisplayedScrollingRowIndex == -1 || dgvMasterView.FirstDisplayedScrollingColumnIndex == -1)
                    return;
                //handle horizontal scroll
                if (MouseCurrentColumn != -1) {
                    dgvMasterView.HorizontalScrollingOffset = dgvMasterView.HorizontalScrollingOffset + (e.Delta * -1) < 0 ? 0 : dgvMasterView.HorizontalScrollingOffset + (e.Delta * -1);
                    dgvMasterView.Invalidate();
                }
            }
            //handle zoom scroll
            else {
                if (ModifierKeys is Keys.Control && e.Delta < 0) {
                    trackZoom.Value = Math.Max(1, horiz - scrollLines);
                }
                else if (ModifierKeys is Keys.Control && e.Delta > 0) {
                    trackZoom.Value = Math.Min(100, horiz + scrollLines);
                }
            }
        }

        private void trackEditor_Scroll_1(object sender, ScrollEventArgs e)
        {
            dgvMasterView.Scroll -= dgvMasterView_Scroll;
            dgvMasterView.HorizontalScrollingOffset = trackEditor.HorizontalScrollingOffset;
            dgvMasterView.Scroll += dgvMasterView_Scroll;
        }

        private void dgvMasterView_Scroll(object sender, ScrollEventArgs e)
        {
            trackEditor.Scroll -= trackEditor_Scroll_1;
            trackEditor.HorizontalScrollingOffset = dgvMasterView.HorizontalScrollingOffset;
            trackEditor.Scroll += trackEditor_Scroll_1;
        }
        #endregion
        #region VALUE SETTING
        private void toolstripMasterView_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStrip _ts = sender as ToolStrip;
            BasicEditorSelectedButton = e.ClickedItem;
            foreach (ToolStripItem item in _ts.Items) {
                item.BackColor = item == BasicEditorSelectedButton ? Color.FromArgb(46, 46, 46) : Color.FromArgb(35, 35, 35);
            }
            BasicEditorUpdate((string)e.ClickedItem.Tag, e.ClickedItem.Text);
        }

        private void basicEditorDropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ((ToolStripSplitButton)e.ClickedItem.OwnerItem).Image = e.ClickedItem.Image;
            BasicEditorUpdate((string)e.ClickedItem.Tag, e.ClickedItem.Text);
        }

        private void BasicEditorUpdate(string _selectobject, string _itemtext)
        {
            BasicEditorSelectedObject = _selectobject;
            dgvMasterView.MultiSelect = BasicEditorSelectedObject == "select";
            if (BasicEditorSelectedObject == "turn")
                BasicEditorClickValue = decimal.Parse(_itemtext[^5..^3]) * (_itemtext.Contains("right", StringComparison.OrdinalIgnoreCase) ? -1 : 1);
            else
                BasicEditorClickValue = 1m;
        }

        private void dgvMasterView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dgvMasterView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
        }

        private bool RegisteredSelectionChanged = false;
        private void dgvMasterView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!RegisteredSelectionChanged) {
                dgvMasterView.SelectionChanged += dgvMasterView_SelectionChanged;
                RegisteredSelectionChanged = true;
            }
            decimal? setvalue = e.Button == MouseButtons.Left ? BasicEditorClickValue : null;
            MasterViewSetValue(e.RowIndex, e.ColumnIndex, setvalue);
        }

        private void dgvMasterView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (Control.MouseButtons == MouseButtons.None)
                return;
            decimal? setvalue = Control.MouseButtons == MouseButtons.Left ? BasicEditorClickValue : null;
            MasterViewSetValue(e.RowIndex, e.ColumnIndex, setvalue);
        }

        private void MasterViewSetValue(int row, int column, decimal? setvalue)
        {
            if (BasicEditorSelectedObject == "select")
                return;
            Sequencer_Object _findseq = null;
            string _paramtofind = "";
        search:
            if (BasicEditorSelectedObject == "visible") {
                _paramtofind = LaneNames[row];
                _findseq = SequencerObjects.FirstOrDefault(x => x.param_path == _paramtofind);
            }
            else if (BasicEditorSelectedObject == "turn") {
                _paramtofind = "turn";
                _findseq = SequencerObjects.FirstOrDefault(x => x.param_path == "turn");
            }
            else {
                _paramtofind = $"{BasicEditorSelectedObject}";
                _findseq = SequencerObjects.FirstOrDefault(x => x.param_path == $"{BasicEditorSelectedObject.Replace(".ent", "." + LaneParams[row])}");
            }            

            if (_findseq == null) {
                Object_Params objmatch = TCLE.LeafObjects.FirstOrDefault(x => x.Value.param_path == _paramtofind).Value;
                Sequencer_Object _importseq = new() {
                    ParentLeaf = leafProperties,
                    obj_name = objmatch.obj_name,
                    category = objmatch.category,
                    param_path = objmatch.param_path,
                    friendly_param = objmatch.param_displayname,
                    defaultvalue = objmatch.default_value,
                    step = objmatch.step,
                    trait_type = objmatch.trait_type,
                    highlight_color = objmatch.defaultcolor,
                    highlight_value = 0,
                    footer = objmatch.footer,
                    enabled = true
                };
                if (_importseq.obj_name == "leafname")
                    _importseq.obj_name = LeafProperties.FilePath.Name;
                _importseq.expandlanes = _importseq.friendly_lane == "none" || Properties.Settings.Default.LeafOptionShowLane;
                if (_importseq.friendly_lane == "lane center") {
                    LoadMultiLanes(_importseq, SequencerObjects);
                }
                else {
                    SequencerObjects.Add(_importseq);
                    trackEditor.Rows.Add(_importseq);
                }
                ChangeTrackName(_importseq, _importseq.category);
                goto search;
            }

            if (setvalue == null && _findseq.category != "TRACK EFFECTS") {
                //if trying to delete a data point, this loops through all objects of the same category and removes that value on each
                //just so you don't have to switch your selection to a different object to delete it.
                foreach (Sequencer_Object seq in SequencerObjects.Where(x => x.category == _findseq.category && x.friendly_lane == _findseq.friendly_lane)) {
                    if (seq.defaultvalue == 1 && setvalue == null) {
                        setvalue = 0;
                    }
                    if (seq.defaultvalue == 1 && setvalue == 1) {
                        setvalue = null;
                    }

                    seq[column + FrozenColumnOffset].Value = setvalue;
                }
            }
            else {
                if (_findseq.defaultvalue == 1 && setvalue == 1) {
                    setvalue = null;
                }
                _findseq[column + FrozenColumnOffset].Value = setvalue;
            }
            SaveCheckAndWrite(false, "Set value");
        }
        #endregion
    }
}
