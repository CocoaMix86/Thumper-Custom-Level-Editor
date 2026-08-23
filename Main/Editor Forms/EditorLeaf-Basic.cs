using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class EditorLeaf
    {
        #region VARIABLES
        private static string[] LaneParams = new[] { "a01", "a02", "ent", "z01", "z02" };
        private static string[] LaneNames = new[] { "visibla01", "visibla02", "visible", "visiblz01", "visiblz02" };
        private Dictionary<string, ToolStripItem> BasicObjects = new();
        private string BasicEditorSelectedObject = "select";
        private decimal? BasicEditorClickValue = 1m;
        private ToolStripItem BasicEditorSelectedButton;
        private static Pen BasicEditorPenGrid = new(Properties.Settings.Default.ColorLeafBasicGrid, 1);
        #endregion
        #region PAINTING
        private void dgvMasterView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.Handled = true;
            if (dgvMasterView[e.ColumnIndex, e.RowIndex].Selected) {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.LightSkyBlue)), e.CellBounds);
            }
            if (Properties.Settings.Default.LeafOptionShowGrid)
                e.Graphics.DrawRectangle(BasicEditorPenGrid, e.CellBounds);

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
            trackEditor.SelectionChanged -= trackEditor_SelectionChanged;
            trackEditor.ClearSelection();
            foreach (int column in dgvMasterView.SelectedCells.Cast<DataGridViewCell>().Select(x => x.ColumnIndex).Distinct()) {
                foreach (DataGridViewRow dgvr in trackEditor.Rows) {
                    dgvr.Cells[column + FrozenColumnOffset].Selected = true;
                }
            }
            trackEditor.SelectionChanged += trackEditor_SelectionChanged;
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
                    trackZoom.Value = Math.Min(200, horiz + scrollLines);
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
            //middle click to fast select objects
            if (Control.MouseButtons == MouseButtons.Middle) {
                //get any object that exists in the sequencer that the basic editor uses
                List<Sequencer_Object> matches = SequencerObjects.Where(x => BasicObjects.ContainsKey(x.ParamPathBase)).ToList();
                //then filter to the ones that have a value set at the clicked beat
                if (matches.FirstOrDefault(x => ((e.RowIndex is 2 && x.ParamPathLane is "none") || (x.ParamPathLane == LaneParams[e.RowIndex])) && x.Cells[e.ColumnIndex + FrozenColumnOffset].Value != null) is Sequencer_Object found) {
                    //if found, click the button to select it
                    BasicObjects[found.ParamPathBase].PerformClick();
                    //the the object is a dropdown item, then we also have the click the parent button to highlight it
                    if (BasicObjects[found.ParamPathBase] is ToolStripMenuItem drop) {
                        drop.OwnerItem.PerformClick();
                        basicEditorDropDownItemClicked(null, new(drop));
                    }
                }
                return;
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
                _findseq = SequencerObjects.FirstOrDefault(x => x.ParamPath == _paramtofind);
            }
            else if (BasicEditorSelectedObject == "turn") {
                _paramtofind = "turn";
                _findseq = SequencerObjects.FirstOrDefault(x => x.ParamPath == "turn");
            }
            else {
                _paramtofind = $"{BasicEditorSelectedObject}";
                _findseq = SequencerObjects.FirstOrDefault(x => x.ParamPath == $"{BasicEditorSelectedObject.Replace(".ent", "." + LaneParams[row])}");
            }            

            if (_findseq == null) {
                DefaultSequencerObject objmatch = TCLE.LeafObjects.FirstOrDefault(x => x.Value.ParamPath == _paramtofind).Value;
                Sequencer_Object _importseq = new(LeafProperties, objmatch) {
                    ParentLeaf = LeafProperties,
                    ObjName = objmatch.Name,
                    ParamPath = objmatch.ParamPath,
                    FriendlyParam = objmatch.ParamDisplayName,
                    DefaultValue = objmatch.DefaultValue,
                    Step = objmatch.Step,
                    HighlightColor = objmatch.DefaultColor,
                    highlight_value = 0m,
                    EnabledInEditor = true
                };
                if (_importseq.ObjName == "leafname")
                    _importseq.ObjName = this.WorkingFile.Name;
                _importseq.ExpandLanesInEditor = _importseq.FriendlyLane == "none" || Properties.Settings.Default.LeafOptionShowLane;
                if (_importseq.FriendlyLane == "lane center") {
                    var _lanes = LoadMultiLanes(_importseq, SequencerObjects);
                    if (_lanes != null) {
                        SequencerObjects.AddRange(_lanes);
                        trackEditor.Rows.AddRange(_lanes.ToArray());
                    }
                }
                else {
                    SequencerObjects.Add(_importseq);
                    trackEditor.Rows.Add(_importseq);
                }
                string _e = string.Join(',', trackEditor.Rows.Cast<DataGridViewRow>().Select(x => x.Index));
                SetRowHeaderText(_importseq);
                vscrollbarTrackEditor_Resize();
                goto search;
            }

            if (setvalue == null && _findseq.Default.Category != "TRACK EFFECTS") {
                //if trying to delete a data point, this loops through all objects of the same category and removes that value on each
                //just so you don't have to switch your selection to a different object to delete it.
                foreach (Sequencer_Object seq in SequencerObjects.Where(x => x.Default.Category == _findseq.Default.Category && x.FriendlyLane == _findseq.FriendlyLane)) {
                    if (seq.DefaultValue == 1 && setvalue == null) {
                        setvalue = 0;
                    }
                    if (seq.DefaultValue == 1 && setvalue == 1) {
                        setvalue = null;
                    }

                    seq[column + FrozenColumnOffset].Value = setvalue;
                }
            }
            else {
                if (_findseq.DefaultValue == 1 && setvalue == 1) {
                    setvalue = null;
                }
                _findseq[column + FrozenColumnOffset].Value = setvalue;
            }
            SaveCheckAndWrite(false, "Set value");
        }
        #endregion
    }
}
