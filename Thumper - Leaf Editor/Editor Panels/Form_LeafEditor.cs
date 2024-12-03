using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_LeafEditor : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Form Construction
        public Form_LeafEditor(dynamic load = null, FileInfo filepath = null)
        {
            InitializeComponent();
            LoadQuickValues();
            leaftoolsToolStrip.Renderer = new ToolStripOverride();
            leafToolStrip.Renderer = new ToolStripOverride();
            trackEditor.MouseWheel += new MouseEventHandler(trackEditor_MouseWheel);
            DropDownMenuScrollWheelHandler.Enable(true);
            TCLE.InitializeTracks(trackEditor, true);

            if (load != null)
                LoadLeaf(load, filepath);
        }
        #endregion

        #region Variables
        public bool EditorIsSaved = true;

        private int _beats => (int)numericUpDown_LeafLength.Value;

        private int _selecttrack = 0;
        public FileInfo loadedleaf
        {
            get => LoadedLeaf;
            set {
                if (LoadedLeaf != value) {
                    LoadedLeaf = value;
                    if (!LoadedLeaf.Exists) {
                        LoadedLeaf.CreateText();
                    }
                    TCLE.lockedfiles.Add(loadedleaf, new FileStream(LoadedLeaf.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
                }
            }
        }
        private static FileInfo LoadedLeaf;
        private dynamic leafjson;
        private bool loadingleaf = false;
        private bool controldown = false;
        private bool shiftdown = false;
        private bool altdown = false;
        private bool rightclickdown = false;
        private int leafeditorcell = 0;
        private bool randomizing = false;
        private bool ismoving = false;
        private int hscrollposition = 0;

        private List<Sequencer_Object> _tracks = new();
        private static HashSet<Object_Params> _objects => TCLE.LeafObjects;
        private static Dictionary<string, string> objectcolors => TCLE.ObjectColors;
        //public List<string> _tracklane = new() { ".a01", ".a02", ".ent", ".z01", ".z02" };
        private Dictionary<string, string> _tracklanefriendly = new() { { "a01", "lane left 2" }, { "a02", "lane left 1" }, { "ent", "lane center" }, { "z01", "lane right 1" }, { "z02", "lane right 2" } };
        private List<string> lanenames = new() { "left", "center", "right" };
        private Dictionary<string, string> kTraitTooltips = new() { { "kTraitBool", "BOOL: accepts values 1 (on) or 0 (off)." }, { "kTraitAction", "ACTION: accepts values 1 (activate)." }, { "kTraitFloat", "FLOAT: accepts decimal values from -32000.0000 to 32000.0000." }, { "kTraitInt", "INT: accepts integer (no decimal) values from -32000 to 32000." }, { "kTraitColor", "COLOR: accepts an integer representation of an ARGB color. Use the color wheel button to insert colors." } };
        private List<Sequencer_Object> clipboardtracks = new();
        private List<SaveState> _undolistleaf = new();
        #endregion
        #region EventHandlers
        ///        ///
        /// EVENTS ///
        ///        ///

        ///LEAF EDITOR PANEL LOCATION CHANGED
        private void panelLeaf_LocationChanged(object sender, EventArgs e)
        {
            //stretches panel to fit window
            panelLeaf.Size = new Size(this.Size.Width - panelLeaf.Location.X - 15, panelLeaf.Size.Height);
        }

        ///
        ///TRACKBAR ZOOM AND SCROLLING
        ///DETECT SCROLL
        private void trackEditor_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            vscrollbarTrackEditor_Resize();
        }
        private void trackEditor_Scroll(object sender, ScrollEventArgs e)
        {
            if (controldown) {
                trackEditor.Scroll -= trackEditor_Scroll;
                trackEditor.HorizontalScrollingOffset = e.OldValue;
                trackEditor.Scroll += trackEditor_Scroll;
            }
        }
        private void btnLeafZoom_Click(object sender, EventArgs e)
        {
            TCLE.PlaySound("UIselect");
            panelZoom.Visible = !panelZoom.Visible;
        }
        private void trackZoom_Scroll(object sender, EventArgs e)
        {
            int display = trackEditor.FirstDisplayedScrollingColumnIndex;
            if (display == -1)
                return;
            for (int i = 0; i < _beats; i++) {
                trackEditor.Columns[i].Width = trackZoom.Value;
            }
            //hScrollBarTrackEditor.Visible = !(trackEditor.DisplayedColumnCount(false) == trackEditor.ColumnCount);
            //hScrollBarTrackEditor.Maximum = (trackEditor.ColumnCount - trackEditor.DisplayedColumnCount(true) + 10);
            if (trackEditor.ColumnCount > 1) {
                trackEditor.Scroll -= trackEditor_Scroll;
                trackEditor.FirstDisplayedScrollingColumnIndex = display + 1;
                trackEditor.FirstDisplayedScrollingColumnIndex = display;
                trackEditor.Scroll += trackEditor_Scroll;
            }
        }
        private void trackZoomVert_Scroll(object sender, EventArgs e)
        {
            trackEditor.Scroll -= trackEditor_Scroll;
            int display = trackEditor.FirstDisplayedScrollingRowIndex;
            if (display == -1)
                return;
            for (int i = 0; i < trackEditor.Rows.Count; i++) {
                trackEditor.Rows[i].Height = trackZoomVert.Value;
            }
            vscrollbarTrackEditor_Resize();
            trackEditor.FirstDisplayedScrollingRowIndex = display;
            trackEditor.Scroll += trackEditor_Scroll;
        }
        private void trackEditor_Resize(object sender, EventArgs e)
        {
            vscrollbarTrackEditor_Resize();
            //hScrollBarTrackEditor.Visible = !(trackEditor.DisplayedColumnCount(false) == trackEditor.ColumnCount);
            //hScrollBarTrackEditor.Maximum = (trackEditor.ColumnCount - trackEditor.DisplayedColumnCount(true) + 10);
        }
        private void vscrollbarTrackEditor_Resize()
        {
            vScrollBarTrackEditor.Visible = !(trackEditor.DisplayedRowCount(false) == trackEditor.RowCount);
            vScrollBarTrackEditor.Maximum = trackEditor.RowCount - trackEditor.DisplayedRowCount(false) + 10;
        }
        private void vScrollBarTrackEditor_VisibleChanged(object sender, EventArgs e)
        {
            if (vScrollBarTrackEditor.Visible) {
                trackEditor.Location = new Point(vScrollBarTrackEditor.Location.X + 15, trackEditor.Location.Y);
                //trackEditor.Width -= 15;
                trackEditor.Width = panelLeaf.Width - trackEditor.Location.X - 5;
            }
            else {
                trackEditor.Location = new Point(vScrollBarTrackEditor.Location.X, trackEditor.Location.Y);
                //trackEditor.Width += 15;
                trackEditor.Width = panelLeaf.Width - trackEditor.Location.X - 5;
            }
        }

        private void trackEditor_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            trackEditor.Focus();
            int scollrowindex = trackEditor.FirstDisplayedScrollingRowIndex;
            int horiz = trackZoom.Value;
            int vert = trackZoomVert.Value;
            int scrollLines = SystemInformation.MouseWheelScrollLines;

            //handle horizontal and vertical scroll
            if (!controldown && !shiftdown) {
                if (trackEditor.FirstDisplayedScrollingRowIndex == -1 || trackEditor.FirstDisplayedScrollingColumnIndex == -1)
                    return;
                //handle horizontal scroll
                if (leafeditorcell != -1) {
                    trackEditor.HorizontalScrollingOffset = trackEditor.HorizontalScrollingOffset + (e.Delta * -1) < 0 ? 0 : trackEditor.HorizontalScrollingOffset + (e.Delta * -1);
                }
                //handle vertical scroll
                else {
                    if (e.Delta > 0) {
                        trackEditor.FirstDisplayedScrollingRowIndex = Math.Max(0, scollrowindex - scrollLines);
                    }
                    else if (e.Delta < 0) {
                        trackEditor.FirstDisplayedScrollingRowIndex = Math.Min(trackEditor.RowCount - 1, scollrowindex + scrollLines);
                    }
                    vScrollBarTrackEditor.Value = trackEditor.FirstDisplayedScrollingRowIndex;
                }
            }
            //handle zoom scroll
            else {
                if (controldown && e.Delta < 0) {
                    trackZoom.Value = Math.Max(1, horiz - scrollLines);
                }
                else if (controldown && e.Delta > 0) {
                    trackZoom.Value = Math.Min(100, horiz + scrollLines);
                }
                if (shiftdown && e.Delta < 0) {
                    trackZoomVert.Value = Math.Max(1, vert - scrollLines);
                }
                else if (shiftdown && e.Delta > 0) {
                    trackZoomVert.Value = Math.Min(100, vert + scrollLines);
                }
            }
        }
        private void vScrollBarTrackEditor_Scroll(object sender, ScrollEventArgs e)
        {
            if (trackEditor.FirstDisplayedScrollingRowIndex != -1)
                trackEditor.FirstDisplayedScrollingRowIndex = e.NewValue;
        }
        ///
        /// 

        ///DATAGRIDVIEW - TRACK EDITOR
        private void trackEditor_SelectionChanged(object sender, EventArgs e)
        {
            bool enable = trackEditor.SelectedCells.Count > 0;
            btnTrackUp.Enabled = enable;
            btnTrackDown.Enabled = enable;
            btnTrackCopy.Enabled = enable;
            btnTrackDelete.Enabled = enable;
            btnTrackClear.Enabled = enable;
        }
        //Row changed
        private void trackEditor_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (ismoving)
                return;
            _selecttrack = e.RowIndex;
            ShowRawTrackData(trackEditor.Rows[e.RowIndex]);
            List<string> _params = new();

            try {
                //if track is a multi-lane object, split param_path from lane so both values can be used to update their dropdown boxes
                if (lanenames.Any(x => _tracks[_selecttrack].friendly_param.Contains(x))) {
                    _params = _tracks[_selecttrack].friendly_param.Split(new string[] { ", " }, StringSplitOptions.None).ToList();
                }
                else
                    _params = new List<string>() { _tracks[_selecttrack].friendly_param, "center" };
                //set all controls to their values stored in _tracks
                dropObjects.SelectedIndex = dropObjects.FindStringExact(_tracks[_selecttrack].friendly_type);
                dropParamPath.SelectedIndex = dropParamPath.FindStringExact(_params[0]);
                //needs a different selection method if it's a sample
                if (_tracks[_selecttrack].param_path == "play")
                    dropTrackLane.SelectedIndex = dropTrackLane.FindStringExact(_tracks[_selecttrack].obj_name.Replace(".samp", ""));
                else if (_params.Count >= 2)
                    dropTrackLane.SelectedIndex = dropTrackLane.FindStringExact(_params[1]);
                else //track lane only uses param[0]
                    dropTrackLane.SelectedIndex = dropTrackLane.FindStringExact(_params[0]);
                txtTrait.Text = _tracks[_selecttrack].trait_type;
                btnTrackColorDialog.BackColor = Color.FromArgb(int.Parse(_tracks[_selecttrack].highlight_color));
                //remove event handlers from a few controls so they don't trigger when their values change
                NUD_TrackHighlight.ValueChanged -= NUD_TrackHighlight_ValueChanged;
                txtDefault.ValueChanged -= txtDefault_ValueChanged;
                dropLeafStep.SelectedIndexChanged -= dropLeafStep_SelectedIndexChanged;
                dropLeafInterp.SelectedIndexChanged -= dropLeafInterp_SelectedIndexChanged;
                //set values from _tracks
                NUD_TrackHighlight.Value = (decimal)_tracks[_selecttrack].highlight_value;
                btnTrackColorDialog.BackColor = Color.FromArgb(int.Parse(_tracks[_selecttrack].highlight_color));
                txtDefault.Value = (decimal)_tracks[_selecttrack]._default;
                dropLeafStep.SelectedItem = _tracks[_selecttrack].step;
                dropLeafInterp.SelectedItem = _tracks[_selecttrack].default_interp;
                txtDefault.Enabled = true;
                dropLeafInterp.Enabled = true;
                dropLeafStep.Enabled = true;
                btnTrackApply.Enabled = true;
                //re-add event handlers
                NUD_TrackHighlight.ValueChanged += NUD_TrackHighlight_ValueChanged;
                txtDefault.ValueChanged += txtDefault_ValueChanged;
                dropLeafStep.SelectedIndexChanged += dropLeafStep_SelectedIndexChanged;
                dropLeafInterp.SelectedIndexChanged += dropLeafInterp_SelectedIndexChanged;

                toolTip1.SetToolTip(txtTrait, kTraitTooltips[txtTrait.Text]);
            }
            catch { }
        }

        //cell input sanitization
        private void trackEditor_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //e.CellStyle.Font = new Font("Consolas", 7);
        }

        //Cell value changed
        private void trackEditor_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
                return;
            if (trackEditor.IsCurrentCellInEditMode)
                CellValueChanged(e.RowIndex, e.ColumnIndex);
        }
        private void CellValueChanged(int rowindex, int columnindex, bool setnull = false)
        {
            List<DataGridViewRow> edited = new();
            try {
                bool changes = false;
                object _val = null;
                if (setnull)
                    _val = null;
                else if (Decimal.TryParse(trackEditor[columnindex, rowindex].EditedFormattedValue?.ToString(), out decimal _valtoset))
                    _val = TCLE.TruncateDecimal(_valtoset, 3);
                //iterate over each cell in the selection
                foreach (DataGridViewCell _cell in trackEditor.SelectedCells) {
                    if (_cell.ReadOnly)
                        continue;
                    //if cell does not have the value, set it
                    if (_cell.Value != _val) {
                        _cell.Value = _val;
                        changes = true;
                    }

                    if (!edited.Contains(_cell.OwningRow))
                        edited.Add(_cell.OwningRow);

                    TrackUpdateHighlightingSingleCell(_cell, _tracks[_cell.RowIndex]);
                }
                //sets flag that leaf has unsaved changes
                if (changes) {
                    if (trackEditor.SelectedCells.Count > 1)
                        SaveLeaf(false, $"{trackEditor.SelectedCells.Count} beats value set: {_val ?? "empty"}", $"{_tracks[rowindex].friendly_type} {_tracks[rowindex].friendly_param}");
                    else
                        SaveLeaf(false, $"Beat {columnindex} value set: {_val ?? "empty"}", $"{_tracks[rowindex].friendly_type} {_tracks[rowindex].friendly_param}");
                }
            }
            catch { }

            foreach (DataGridViewRow r in edited)
                GenerateDataPoints(r, _tracks[r.Index]);
            ShowRawTrackData(trackEditor.Rows[rowindex]);
        }

        private void trackEditor_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        //Cell click, insert values if track is BOOL
        private void trackEditor_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;
            DataGridView dgv = (DataGridView)sender;
            if (e.Button == MouseButtons.Left && btnLeafAutoPlace.Checked) {
                if (_tracks[e.RowIndex].trait_type is "kTraitBool" or "kTraitAction")
                    if (dgv[e.ColumnIndex, e.RowIndex].Value == null) {
                        dgv[e.ColumnIndex, e.RowIndex].Value = 1m;
                        CellValueChanged(e.RowIndex, e.ColumnIndex);
                    }
            }
        }
        private void trackEditor_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;
            DataGridView dgv = sender as DataGridView;
            if (e.Button == MouseButtons.Right) {
                if (dgv[e.ColumnIndex, e.RowIndex].Selected == false && dgv[e.ColumnIndex, e.RowIndex].Value != null) {
                    dgv[e.ColumnIndex, e.RowIndex].Value = null;
                    TrackUpdateHighlightingSingleCell(dgv[e.ColumnIndex, e.RowIndex], _tracks[e.RowIndex]);
                    GenerateDataPoints(dgv.Rows[e.RowIndex], _tracks[e.RowIndex]);
                    SaveLeaf(false, "Deleted single cell", $"{_tracks[e.RowIndex].friendly_type} {_tracks[e.RowIndex].friendly_param}");
                }
                else if (dgv[e.ColumnIndex, e.RowIndex].Selected) {
                    if (dgv[e.ColumnIndex, e.RowIndex].Value == null && dgv.SelectedCells.Count == 1)
                        return;
                    CellValueChanged(e.RowIndex, e.ColumnIndex, true);
                    //_undolistleaf.RemoveAt(1);
                }
            }
        }
        private void trackEditor_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            leafeditorcell = e.ColumnIndex;
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;
            DataGridView dgv = sender as DataGridView;
            if (Control.MouseButtons == MouseButtons.Right) {
                if (dgv[e.ColumnIndex, e.RowIndex].Selected == false && dgv[e.ColumnIndex, e.RowIndex].Value != null) {
                    dgv[e.ColumnIndex, e.RowIndex].Value = null;
                    TrackUpdateHighlightingSingleCell(dgv[e.ColumnIndex, e.RowIndex], _tracks[e.RowIndex]);
                    GenerateDataPoints(dgv.Rows[e.RowIndex], _tracks[e.RowIndex]);
                    SaveLeaf(false, "Deleted single cell", $"{_tracks[e.RowIndex].friendly_type} {_tracks[e.RowIndex].friendly_param}");
                }
                else if (dgv[e.ColumnIndex, e.RowIndex].Selected == true) {
                    dgv[e.ColumnIndex, e.RowIndex].Value = null;
                    CellValueChanged(e.RowIndex, e.ColumnIndex, true);
                    //_undolistleaf.RemoveAt(1);
                }
            }
        }
        //Keypress Backspace - clear selected cells
        private void trackEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back) {
                _logundo = false;
                CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex, true);
                _logundo = true;
                SaveLeaf(false, "Deleted cell values", $"{_tracks[_selecttrack].friendly_type} {_tracks[_selecttrack].friendly_param}");
            }
            e.Handled = true;
        }
        private void trackEditor_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            controldown = e.Control;
            shiftdown = e.Shift;
            altdown = e.Alt;
            hscrollposition = trackEditor.HorizontalScrollingOffset;
            ///Keypress Delete - clear selected cellss
            //delete cell value if Delete key is pressed
            if (e.KeyCode == Keys.Delete) {
                _logundo = false;
                CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex, true);
                _logundo = true;
                SaveLeaf(false, "Deleted cell values", $"{_tracks[_selecttrack].friendly_type} {_tracks[_selecttrack].friendly_param}");
            }
            else if (controldown) {
                ///copies selected cells
                if (e.KeyCode == Keys.C) {
                    DataObject d = trackEditor.GetClipboardContent();
                    Clipboard.SetDataObject(d, true);
                    e.Handled = true;
                }
                ///cut and copies selected cells
                if (e.KeyCode == Keys.X) {
                    DataObject d = trackEditor.GetClipboardContent();
                    Clipboard.SetDataObject(d, true);
                    _logundo = false;
                    CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex, true);
                    e.Handled = true;
                    _logundo = true;
                    SaveLeaf(false, "Cut cells", $"");
                }
                ///pastes cell data from clipboard
                if (e.KeyCode == Keys.V) {
                    //get content on clipboard to string and then split it to rows
                    string s = Clipboard.GetText().Replace("\r\n", "\n");
                    string[] copiedrows = s.Split('\n');
                    //set ints so we don't have to always call rowindex, columnindex
                    int row = trackEditor.CurrentCell.RowIndex;
                    int col = trackEditor.CurrentCell.ColumnIndex;
                    List<DataGridViewRow> edited = new();
                    for (int _line = 0; _line < copiedrows.Length; _line++) {
                        //if paste will go outside grid bounds, skip
                        if (row + _line >= trackEditor.RowCount)
                            break;
                        //split row into individual cells
                        string[] cells = copiedrows[_line].Split('\t');
                        for (int i = 0; i < cells.Length; i++) {
                            //if paste will go outside grid bounds, skip
                            if (col + i >= trackEditor.ColumnCount)
                                break;
                            //don't paste if cell is blank
                            if (!string.IsNullOrEmpty(cells[i])) {
                                trackEditor[col + i, row + _line].Value = decimal.Parse(cells[i]);
                                TrackUpdateHighlightingSingleCell(trackEditor[col + i, row + _line], _tracks[row + _line]);
                            }
                            if (!edited.Contains(trackEditor[col + i, row + _line].OwningRow))
                                edited.Add(trackEditor[col + i, row + _line].OwningRow);
                        }
                    }
                    foreach (DataGridViewRow r in edited)
                        GenerateDataPoints(r, _tracks[r.Index]);
                    SaveLeaf(false, $"Pasted cells", $"");
                }
            }

            else if (altdown) {
                if (e.KeyCode is Keys.Right or Keys.Left or Keys.Up or Keys.Down) {
                    e.Handled = true;
                    //this is used for indexing if shifting left/down or right/up
                    int indexdirection = e.KeyCode is Keys.Right or Keys.Down ? 1 : -1;
                    bool leftright = e.KeyCode is Keys.Left or Keys.Right;
                    bool shifted = false;
                    //sort cells in selection based on column. depends on direction, reverse collection.
                    //this processing order is important so cells dont overwrite each other when moving
                    IOrderedEnumerable<DataGridViewCell> dgvcc = (indexdirection == -1) ? trackEditor.SelectedCells.Cast<DataGridViewCell>().OrderBy(c => leftright ? c.ColumnIndex : c.RowIndex) : trackEditor.SelectedCells.Cast<DataGridViewCell>().OrderByDescending(c => leftright ? c.ColumnIndex : c.RowIndex);
                    trackEditor.ClearSelection();
                    //iterate over each in the selection
                    foreach (DataGridViewCell dgvc in dgvcc) {
                        //check if at left/right edges
                        if ((leftright && dgvc.ColumnIndex + indexdirection < trackEditor.ColumnCount && dgvc.ColumnIndex + indexdirection > -1) || (!leftright && dgvc.RowIndex + indexdirection < trackEditor.RowCount && dgvc.RowIndex + indexdirection > -1)) {
                            shifted = true;
                            trackEditor[dgvc.ColumnIndex + (leftright ? indexdirection : 0), dgvc.RowIndex + (!leftright ? indexdirection : 0)].Value = dgvc.Value;
                            //select the newly moved cell
                            trackEditor[dgvc.ColumnIndex + (leftright ? indexdirection : 0), dgvc.RowIndex + (!leftright ? indexdirection : 0)].Selected = true;
                            TrackUpdateHighlightingSingleCell(trackEditor[dgvc.ColumnIndex + (leftright ? indexdirection : 0), dgvc.RowIndex + (!leftright ? indexdirection : 0)], _tracks[dgvc.RowIndex + (!leftright ? indexdirection : 0)]);
                            //clear the current cell since it moved
                            dgvc.Value = null;
                            TrackUpdateHighlightingSingleCell(dgvc, _tracks[dgvc.RowIndex]);
                        }
                        else {
                            foreach (DataGridViewCell dgvcell in dgvcc)
                                dgvcell.Selected = true;
                            break;
                        }
                    }
                    if (shifted)
                        SaveLeaf(false, $"Shifted selected cells {(e.KeyCode == Keys.Left ? "left" : "right")}", $"");
                }
            }

            if (trackEditor.CurrentCell != null) {
                if (e.KeyData == TCLE.defaultkeybinds["quick1"]) {
                    trackEditor.CurrentCell.Value = NUDquick1.Value;
                    CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
                }
                else if (e.KeyData == TCLE.defaultkeybinds["quick2"]) {
                    trackEditor.CurrentCell.Value = NUDquick2.Value;
                    CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
                }
                else if (e.KeyData == TCLE.defaultkeybinds["quick3"]) {
                    trackEditor.CurrentCell.Value = NUDquick3.Value;
                    CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
                }
                else if (e.KeyData == TCLE.defaultkeybinds["quick4"]) {
                    trackEditor.CurrentCell.Value = NUDquick4.Value;
                    CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
                }
                else if (e.KeyData == TCLE.defaultkeybinds["quick5"]) {
                    trackEditor.CurrentCell.Value = NUDquick5.Value;
                    CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
                }
                else if (e.KeyData == TCLE.defaultkeybinds["quick6"]) {
                    trackEditor.CurrentCell.Value = NUDquick6.Value;
                    CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
                }
                else if (e.KeyData == TCLE.defaultkeybinds["quick7"]) {
                    trackEditor.CurrentCell.Value = NUDquick7.Value;
                    CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
                }
                else if (e.KeyData == TCLE.defaultkeybinds["quick8"]) {
                    trackEditor.CurrentCell.Value = NUDquick8.Value;
                    CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
                }
                else if (e.KeyData == TCLE.defaultkeybinds["quick9"]) {
                    trackEditor.CurrentCell.Value = NUDquick9.Value;
                    CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
                }
            }
        }
        private void trackEditor_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            controldown = e.Control;
            shiftdown = e.Shift;
            altdown = e.Alt;
        }
        private void AllowArrowMovement(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode is Keys.Right or Keys.Left or Keys.Up or Keys.Down) {
                if (trackEditor.IsCurrentCellInEditMode) {
                    if (string.IsNullOrEmpty((string)trackEditor.CurrentCell.EditedFormattedValue)) {
                        trackEditor.CurrentCell.Value = null;
                        trackEditor.CancelEdit();
                        if (e.KeyCode is Keys.Right or Keys.Left)
                            trackEditor.EndEdit();
                    }
                }
            }
        }
        private void trackEditor_Click(object sender, EventArgs e)
        {
            if (trackEditor.IsCurrentCellInEditMode) {
                if (string.IsNullOrEmpty((string)trackEditor.CurrentCell.EditedFormattedValue)) {
                    trackEditor.CurrentCell.Value = null;
                    trackEditor.CancelEdit();
                    trackEditor.EndEdit();
                }
            }
        }
        //Clicking row headers to select the row
        private void trackEditor_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (trackEditor.FirstDisplayedScrollingColumnIndex == -1)
                return;
            trackEditor.CurrentCell = trackEditor.Rows[e.RowIndex].Cells[trackEditor.FirstDisplayedScrollingColumnIndex];
        }

        private void trackEditor_RowHeadersWidthChanged(object sender, EventArgs e)
        {
            trackEditor_Resize(null, null);
        }
        ///LEAF LENGTH
        private void numericUpDown_LeafLength_ValueChanged(object sender, EventArgs e)
        {
            string data = trackEditor.ColumnCount.ToString();

            if (_beats > trackEditor.ColumnCount) {
                trackEditor.ColumnCount = _beats;
                TCLE.GenerateColumnStyle(trackEditor, _beats);
            }
            else
                trackEditor.ColumnCount = _beats;
            //set cell zoom
            trackZoom_Scroll(null, null);
            //make sure new cells follow the time sig
            TrackTimeSigHighlighting();
            //sets flag that leaf has unsaved changes
            SaveLeaf(false, "Leaf length", $"{data} -> {_beats}");
        }
        ///DROPDOWN OBJECTS
        private void dropObjects_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!panelLeaf.Enabled)
                return;
            if (dropObjects.SelectedIndex == -1) {
                dropParamPath.SelectedIndex = -1;
                return;
            }
            //when an object is chosen, unlock the param_path options and set datasource
            dropParamPath.DataSource = _objects.Where(obj => obj.category == dropObjects.Text).Select(obj => obj.param_displayname).ToList();
            //switch index back and forth to trigger event
            dropParamPath.SelectedIndex = -1;
            dropParamPath.SelectedIndex = 0;
            dropParamPath.Enabled = true;

            if ((string)dropObjects.SelectedValue == "PLAY SAMPLE") {
                label11.Text = "Samples";
                TCLE.LvlReloadSamples();
                dropTrackLane.DataSource = null;
                dropTrackLane.DataSource = TCLE.LvlSamples.Select(x => x.obj_name).ToArray();
                dropTrackLane.SelectedIndex = -1;
            }
            else {
                label11.Text = "Lane";
                dropTrackLane.DataSource = new BindingSource(_tracklanefriendly, null);
                dropTrackLane.ValueMember = "Key";
                dropTrackLane.DisplayMember = "Value";
                //set default lane to 'middle'
                dropTrackLane.SelectedIndex = 2;
            }
        }
        ///DROPDOWN PARAM_PATHS
        private void dropParamPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropParamPath.SelectedIndex == -1) {
                dropTrackLane.Enabled = false;
                btnTrackApply.Enabled = false;
                return;
            }

            if (dropParamPath.SelectedIndex != -1 && dropParamPath.Enabled) {
                //if (_tracks[trackEditor.CurrentRow?.Index ?? 0].highlight_color == null)
                btnTrackColorDialog.BackColor = Color.FromArgb(int.Parse(objectcolors.TryGetValue(dropParamPath.Text, out string value) ? value : "-8355585"));
                //if the param_path is .ent, enable lane choice
                if (_objects.First(obj => obj.param_displayname == dropParamPath.Text).param_path.EndsWith(".ent") || (string)dropObjects.SelectedValue == "PLAY SAMPLE") {
                    dropTrackLane.Enabled = true;
                }
                //else set lane to middle and enable 'Apply' button
                else {
                    dropTrackLane.Enabled = false;
                }
                btnTrackApply.Enabled = true;
            }
        }
        ///DROPDOWN TRACK LANE
        private void dropTrackLane_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if the the selected param path can be set to a lane, keep Apply button disabled until a lane is selected
            if (dropTrackLane.Enabled && dropTrackLane.SelectedIndex != -1) {
                btnTrackApply.Enabled = true;
            }
        }
        ///DROPDOWN TIMESIGS
        private void dropTimeSig_SelectedIndexChanged(object sender, EventArgs e)
        {
            TrackTimeSigHighlighting();
            //sets flag that leaf has unsaved changes
            SaveLeaf(false, "Time signature", "");
        }
        private void dropTimeSig_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                string s = dropTimeSig.Text;
                // if item exists, select it. if it does not exist, add it.
                if (!dropTimeSig.Items.Contains(s)) {
                    dropTimeSig.Items.Add(s);
                }
                dropTimeSig.SelectedItem = s;
            }
        }
        ///NUMERIC_UPDOWN TRACK HIGHLIGHT VALUE
        private void NUD_TrackHighlight_ValueChanged(object sender, EventArgs e)
        {
            string data = _tracks[_selecttrack].highlight_value.ToString();
            _tracks[trackEditor.CurrentRow.Index].highlight_value = (float)NUD_TrackHighlight.Value;
            TrackUpdateHighlighting(trackEditor.CurrentRow, _tracks[_selecttrack]);
            //sets flag that leaf has unsaved changes
            SaveLeaf(false, $"Track hilighting value {data} -> {NUD_TrackHighlight.Value}", $"{_tracks[_selecttrack].friendly_type} {_tracks[_selecttrack].friendly_param}");
        }
        ///LEAF - NEW
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((!EditorIsSaved && MessageBox.Show("Current leaf is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || EditorIsSaved) {
                leafsaveAsToolStripMenuItem_Click(null, null);
            }
        }
        ///LEAF - SAVE FILE
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if _loadedlvl is somehow not set, force Save As instead
            if (loadedleaf == null) {
                ///_mainform.toolstripLeafSaveAs.PerformClick();
                return;
            }
            else
                //write contents direct to file without prompting save dialog
                WriteLeaf();
        }
        ///LEAF - SAVE AS
        private void leafsaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using SaveFileDialog sfd = new();
            //filter .txt only
            sfd.Filter = "Thumper Leaf File (*.leaf)|*.leaf";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                /*
                if (sender == null)
                    loadedleaf = null;
                //separate path and filename
                string storePath = Path.GetDirectoryName(sfd.FileName);
                string tempFileName = Path.GetFileName(sfd.FileName);
                if (!tempFileName.EndsWith(".txt"))
                    tempFileName += ".txt";
                //check if user input "leaf_", and deny save if so
                if (Path.GetFileName(sfd.FileName).Contains("leaf_")) {
                    MessageBox.Show("File not saved. Do not include 'leaf_' in your file name.", "File not saved");
                    return;
                }
                if (File.Exists($@"{storePath}\leaf_{tempFileName}")) {
                    MessageBox.Show("That file name exists already.", "File not saved");
                    return;
                }
                */
                loadedleaf = new FileInfo(sfd.FileName);
                WriteLeaf(true);
                //after saving new file, refresh the workingfolder
                ///_mainform.btnWorkRefresh.PerformClick();
            }
        }
        private void WriteLeaf(bool clearundo = false)
        {
            //serialize JSON object to a string, and write it to the file
            JObject _save = LeafBuildSave(LoadedLeaf.Name);
            if (!TCLE.lockedfiles.ContainsKey(loadedleaf)) {
                TCLE.lockedfiles.Add(loadedleaf, new FileStream(LoadedLeaf.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
            }
            TCLE.WriteFileLock(TCLE.lockedfiles[loadedleaf], _save);
            SaveLeaf(true, "Saved", "", true);
            this.Text = LoadedLeaf.Name;
            //update beat counts in loaded lvl if need be
            ///if (_mainform._loadedlvl != null)
                ///_mainform.btnLvlRefreshBeats_Click(null, null);
            if (clearundo)
                ClearReloadUndo(_save);
        }
        ///LEAF - LOAD FILE
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((!EditorIsSaved && MessageBox.Show("Current leaf is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || EditorIsSaved) {
                using OpenFileDialog ofd = new();
                ofd.Filter = "Thumper Leaf File (*.leaf)|*.leaf";
                ofd.Title = "Load a Thumper Leaf file";
                ofd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK) {
                    //storing the filename in temp so it doesn't overwrite _loadedlvl in case it fails the check in LoadLvl()
                    FileInfo filepath = new FileInfo(TCLE.CopyToWorkingFolderCheck(ofd.FileName, TCLE.WorkingFolder.FullName));
                    if (filepath == null)
                        return;
                    //load json from file into _load. The regex strips any comments from the text.
                    dynamic _load = TCLE.LoadFileLock(filepath.FullName);
                    LoadLeaf(_load, filepath);
                }
            }
        }
        /// LEAF - LOAD TEMPLATE
        private void leafTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((!EditorIsSaved && MessageBox.Show("Current leaf is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || EditorIsSaved) {
                using OpenFileDialog ofd = new();
                ofd.Filter = "Thumper Leaf File (*.leaf)|*.leaf";
                ofd.Title = "Load a Thumper Leaf file";
                //set folder to the templates location
                ofd.InitialDirectory = $@"{AppDomain.CurrentDomain.BaseDirectory}templates";
                if (ofd.ShowDialog() == DialogResult.OK) {
                    object _load = TCLE.LoadFileLock(ofd.FileName);
                    LoadLeaf(_load, new FileInfo("template"));
                }
            }
        }
        ///DEFAULT TRACK VALUE CHANGED
        private void txtDefault_ValueChanged(object sender, EventArgs e)
        {
            string data = $"{_tracks[_selecttrack]._default}";
            _tracks[trackEditor.CurrentRow.Index]._default = (float)txtDefault.Value;
            //sets flag that leaf has unsaved changes
            if (!randomizing)
                SaveLeaf(false, $"Default value {data} -> {txtDefault.Value}", $"{_tracks[_selecttrack].friendly_type} {_tracks[_selecttrack].friendly_param}");
        }
        ///STEP CHANGED
        private void dropLeafStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            string data = $"{_tracks[_selecttrack].step}";
            _tracks[trackEditor.CurrentRow.Index].step = dropLeafStep.Text;
            if (!randomizing)
                SaveLeaf(false, $"Step value {data} -> {dropLeafStep.Text}", $"{_tracks[_selecttrack].friendly_type} {_tracks[_selecttrack].friendly_param}");
        }
        ///INTERP CHANGED
        private void dropLeafInterp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string data = $"{_tracks[_selecttrack].default_interp}";
            _tracks[trackEditor.CurrentRow.Index].default_interp = dropLeafInterp.Text;
            if (!randomizing)
                SaveLeaf(false, $"Interp value {data} -> {dropLeafInterp.Text}", $"{_tracks[_selecttrack].friendly_type} {_tracks[_selecttrack].friendly_param}");
        }
        #endregion
        #region Buttons
        ///         ///
        /// BUTTONS ///
        ///         ///

        private void btnRawImport_Click(object sender, EventArgs e)
        {
            if (loadedleaf == null)
                return;
            try {
                TrackRawImport(trackEditor.CurrentRow, JObject.Parse($"{{{richRawTrackData.Text}}}"));
                TrackUpdateHighlighting(trackEditor.CurrentRow, _tracks[_selecttrack]);
                GenerateDataPoints(trackEditor.CurrentRow, _tracks[_selecttrack]);
            }
            catch (Exception) {
                MessageBox.Show($"Invalid format or characters in raw data. Please fix.", "Import error");
            }
            TCLE.PlaySound("UIkpaste");
        }

        private void btnTrackDelete_Click(object sender, EventArgs e)
        {
            bool _empty = true;
            string data = $"{_tracks[_selecttrack].friendly_type} {_tracks[_selecttrack].friendly_param}";
            List<DataGridViewRow> selectedrows = trackEditor.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().ToList();
            //iterate over current row to see if any cells have data
            List<DataGridViewCell> filledcells = selectedrows.SelectMany(x => x.Cells.Cast<DataGridViewCell>()).Where(x => x.Value != null).ToList();
            if (filledcells.Count > 0)
                _empty = false;
            //if row is not empty, show confirmation box. Otherwise just delete the row
            if ((!_empty && MessageBox.Show("Some cells in the selected tracks have data. Are you sure you want to delete?", "Confirm?", MessageBoxButtons.YesNo) == DialogResult.Yes) || _empty) {
                try {
                    foreach (DataGridViewRow dgvr in selectedrows) {
                        _tracks.RemoveAt(dgvr.Index);
                        trackEditor.Rows.Remove(dgvr);
                    }
                    //sets flag that leaf has unsaved changes
                    TCLE.PlaySound("UIobjectremove");
                    SaveLeaf(false, "Delete track", data);
                }
                catch { }
            }
            //disable elements if there are no tracks
            if (_tracks.Count == 0) {
                dropObjects.Enabled = false;
                dropParamPath.Enabled = false;
                btnTrackApply.Enabled = false;
                btnTrackColorDialog.Enabled = false;
                NUD_TrackHighlight.Enabled = false;

                btnTrackDelete.Enabled = false;
                btnTrackUp.Enabled = false;
                btnTrackDown.Enabled = false;
                btnTrackClear.Enabled = false;
            }
        }

        private void btnTrackAdd_Click(object sender, EventArgs e)
        {
            dropObjects.Enabled = true;
            dropParamPath.Enabled = true;

            btnTrackDelete.Enabled = true;
            btnTrackUp.Enabled = true;
            btnTrackDown.Enabled = true;
            btnTrackClear.Enabled = true;

            _tracks.Add(new Sequencer_Object() {
                highlight_color = null,
                highlight_value = 1
            });
            trackEditor.Rows.Add(new DataGridViewRow() {
                Height = trackZoomVert.Value,
                ReadOnly = true,
                HeaderCell = new DataGridViewRowHeaderCell() { Value = "(apply a track object)" },
                DefaultCellStyle = new DataGridViewCellStyle() { SelectionBackColor = Color.FromArgb(40, 40, 40), SelectionForeColor = Color.Black }
            });
            trackEditor.CurrentCell = trackEditor.Rows[_tracks.Count - 1].Cells[0];
            //disable Apply button if object is not set
            //dropObjects.SelectedIndex = 0;
            if (dropObjects.SelectedIndex == -1 || dropParamPath.SelectedIndex == -1)
                btnTrackApply.Enabled = false;
            else btnTrackApply.Enabled = true;
            //sets flag that leaf has unsaved changes
            if (!randomizing) {
                TCLE.PlaySound("UIobjectadd");
                SaveLeaf(false, "Add new track", "");
            }
        }

        private void btnTrackUp_Click(object sender, EventArgs e)
        {
            ismoving = true;
            List<Tuple<Sequencer_Object, DataGridViewRow, int>> _selectedtracks = new();
            DataGridView dgv = trackEditor;
            try {
                //finds each distinct row across all selected cells
                List<DataGridViewRow> selectedrows = dgv.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().ToList();
                selectedrows.Sort((row, row2) => row.Index.CompareTo(row2.Index));
                List<DataGridViewCell> selectedcells = dgv.SelectedCells.Cast<DataGridViewCell>().ToList();

                foreach (DataGridViewRow dgvr in selectedrows) {
                    //check if one of the rows is the top row. If it is, stop
                    if (dgvr.Index == 0)
                        return;
                    //this makes sure the row has at least 1 value in it. Otherwise the rowindex gets lost for somereason
                    if (dgvr.Cells[0].Value == null)
                        dgvr.Cells[0].Value = 1.907m;
                    _selectedtracks.Add(new Tuple<Sequencer_Object, DataGridViewRow, int>(_tracks[dgvr.Index], dgvr, dgvr.Index));
                }
                //iterate over rows and shift them up 1 index
                foreach (Tuple<Sequencer_Object, DataGridViewRow, int> _newtrack in _selectedtracks) {
                    _tracks.Remove(_newtrack.Item1);
                    dgv.Rows.Remove(_newtrack.Item2);
                    _tracks.Insert(_newtrack.Item3 - 1, _newtrack.Item1);
                    dgv.Rows.Insert(_newtrack.Item3 - 1, _newtrack.Item2);

                    if ((decimal)_newtrack.Item2.Cells[0].Value == 1.907m)
                        _newtrack.Item2.Cells[0].Value = null;
                }
                //clear selected cells and shift them up
                dgv.CurrentCell = selectedrows[0].Cells[0];
                dgv.ClearSelection();
                foreach (DataGridViewCell cell in selectedcells) {
                    dgv[cell.ColumnIndex, cell.RowIndex].Selected = true;
                }
                //sets flag that leaf has unsaved changes
                SaveLeaf(false, "Move track up", $"{_tracks[_selectedtracks[0].Item3 - 1].friendly_type} {_tracks[_selectedtracks[0].Item3 - 1].friendly_param}");
            }
            catch (Exception ex) { MessageBox.Show("Something unexpected happened. Show this error to the dev.\n" + ex, "Track move error"); }
            ismoving = false;
        }

        private void btnTrackDown_Click(object sender, EventArgs e)
        {
            ismoving = true;
            List<Tuple<Sequencer_Object, DataGridViewRow, int>> _selectedtracks = new();
            DataGridView dgv = trackEditor;
            try {
                //finds each distinct row across all selected cells
                List<DataGridViewRow> selectedrows = dgv.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().ToList();
                selectedrows.Sort((row, row2) => row2.Index.CompareTo(row.Index));
                var selectedcells = dgv.SelectedCells.Cast<DataGridViewCell>().Select(cell => new { cell.ColumnIndex, cell.RowIndex }).ToList();
                foreach (DataGridViewRow dgvr in selectedrows) {
                    //check if one of the rows is the top row. If it is, stop
                    if (dgvr.Index >= dgv.RowCount - 1)
                        return;
                    //this makes sure the row has at least 1 value in it. Otherwise the rowindex gets lost for somereason
                    if (dgvr.Cells[0].Value == null)
                        dgvr.Cells[0].Value = 1.907m;
                    _selectedtracks.Add(new Tuple<Sequencer_Object, DataGridViewRow, int>(_tracks[dgvr.Index], dgvr, dgvr.Index));
                }
                //iterate over rows and shift them up 1 index
                foreach (Tuple<Sequencer_Object, DataGridViewRow, int> _newtrack in _selectedtracks) {
                    _tracks.Remove(_newtrack.Item1);
                    dgv.Rows.Remove(_newtrack.Item2);
                    _tracks.Insert(_newtrack.Item3 + 1, _newtrack.Item1);
                    dgv.Rows.Insert(_newtrack.Item3 + 1, _newtrack.Item2);

                    if ((decimal)_newtrack.Item2.Cells[0].Value == 1.907m)
                        _newtrack.Item2.Cells[0].Value = null;
                }
                //clear selected cells and shift them up
                dgv.CurrentCell = selectedrows[0].Cells[0];
                dgv.ClearSelection();
                foreach (var cell in selectedcells) {
                    dgv[cell.ColumnIndex, cell.RowIndex + 1].Selected = true;
                }
                //sets flag that leaf has unsaved changes
                SaveLeaf(false, "Move track down", $"{_tracks[_selectedtracks[0].Item3 + 1].friendly_type} {_tracks[_selectedtracks[0].Item3 + 1].friendly_param}");
            }
            catch (Exception ex) { MessageBox.Show("Something unexpected happened. Show this error to the dev.\n" + ex, "Track move error"); }
            ismoving = false;
        }

        private void btnTrackCopy_Click(object sender, EventArgs e)
        {
            DataGridView dgv = trackEditor;
            clipboardtracks.Clear();
            try {
                List<DataGridViewRow> selectedrows = dgv.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().ToList();
                selectedrows.Sort((row, row2) => row.Index.CompareTo(row2.Index));
                foreach (DataGridViewRow dgvr in selectedrows) {
                    clipboardtracks.Add(_tracks[dgvr.Index].Clone());
                }
                btnTrackPaste.Enabled = true;
            }
            catch (Exception ex) { MessageBox.Show("something went wrong with copying. Show this error to the dev.\n\n" + ex); }
            TCLE.PlaySound("UIkcopy");
        }

        private void btnTrackPaste_Click(object sender, EventArgs e)
        {
            DataGridView dgv = trackEditor;
            try {
                int _index = trackEditor.CurrentRow?.Index ?? -1;
                //check if copied row is longer than the leaf beat length
                int lastbeat;
                List<JProperty> jj = ((JObject)clipboardtracks[0].data_points).Properties().ToList();
                if (jj.Count <= 1)
                    lastbeat = 1;
                else
                    lastbeat = int.Parse(jj.Last().Name) + 1;
                if (lastbeat > numericUpDown_LeafLength.Value) {
                    DialogResult _paste = MessageBox.Show("Copied track is longer than this leaf's beat count. Do you want to extend this leaf's beat count?\nYES = extend leaf and paste\nNO = paste, do not extend leaf\nCANCEL = do not paste", "Pasting leaf track", MessageBoxButtons.YesNoCancel);
                    //YES = extend the leaf and then paste
                    if (_paste == DialogResult.Yes)
                        numericUpDown_LeafLength.Value = lastbeat;
                    //NO = do not extend leaf and then paste
                    //CANCEL = do nothing
                    else if (_paste == DialogResult.Cancel)
                        return;
                }
                //add copied Sequencer_Object to main _tracks list
                foreach (Sequencer_Object _newtrack in clipboardtracks) {
                    _index++;
                    _tracks.Insert(_index, _newtrack.Clone());
                    dgv.Rows.Insert(_index);
                    DataGridViewRow r = dgv.Rows[_index];
                    try {
                        //set the headercell names
                        ChangeTrackName(r);
                        //pass _griddata per row to be imported to the DGV
                        TrackRawImport(r, _newtrack.data_points);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("something went wrong with pasting. Show this error to the dev.\n\n" + ex);
            }

            TCLE.PlaySound("UIkpaste");
            SaveLeaf(false, "Pasted tracks", "");
        }

        private void btnTrackClear_Click(object sender, EventArgs e)
        {
            //finds each distinct row across all selected cells
            List<DataGridViewRow> selectedrows = trackEditor.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().ToList();
            if (MessageBox.Show($"{selectedrows.Count} rows selected.\nAre you sure you want to clear them?", "Confirm?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            //then get all cells in the rows that have values
            List<DataGridViewCell> filledcells = selectedrows.SelectMany(x => x.Cells.Cast<DataGridViewCell>()).Where(x => x.Value != null).ToList();
            if (filledcells.Count == 0)
                return;
            //select all of them
            foreach (DataGridViewCell dgvc in filledcells) {
                dgvc.Selected = true;
            }
            //then set a single one to null. The "cellvaluechanged" event will handle the rest
            CellValueChanged(filledcells[0].RowIndex, filledcells[0].ColumnIndex, true);

            TCLE.PlaySound("UIdataerase");
            SaveLeaf(false, $"Cleared {selectedrows.Count} track(s)", $"");
        }

        private void btnTrackApply_Click(object sender, EventArgs e)
        {
            Object_Params objmatch = _objects.First(obj => obj.category == dropObjects.Text && obj.param_displayname == dropParamPath.Text);
            //fill object properties on the form
            txtTrait.Text = objmatch.trait_type;
            toolTip1.SetToolTip(txtTrait, kTraitTooltips[txtTrait.Text]);
            //enable track highlighting tools
            btnTrackColorDialog.Enabled = true;
            NUD_TrackHighlight.Enabled = true;
            //add track to list and populate with values
            _tracks[_selecttrack] = new Sequencer_Object() {
                obj_name = objmatch.obj_name,
                friendly_type = objmatch.category,
                param_path = objmatch.param_path,
                friendly_param = objmatch.param_displayname,
                _default = float.Parse(objmatch.def),
                step = objmatch.step,
                trait_type = objmatch.trait_type,
                highlight_color = $"{btnTrackColorDialog.BackColor.ToArgb()}",
                highlight_value = 1,
                footer = objmatch.footer,
                default_interp = "Linear"
            };
            Sequencer_Object _seqobj = _tracks[_selecttrack];
            DataGridViewRow trackrowapplied = trackEditor.Rows[_selecttrack];
            trackrowapplied.HeaderCell.Style.BackColor = TCLE.Blend(Color.FromArgb(int.Parse(_seqobj.highlight_color)), Color.Black, 0.4);
            trackrowapplied.ReadOnly = false;
            trackrowapplied.DefaultCellStyle = null;
            //alter the data if it's a sample object being added. Save the sample name instead
            if ((string)dropObjects.SelectedValue == "PLAY SAMPLE")
                _seqobj.obj_name = dropTrackLane.SelectedValue?.ToString() + ".samp";
            //if lane is not middle, edit the param_path and friendly_param to match
            if (_seqobj.param_path.Contains(".ent")) {
                _seqobj.param_path = _seqobj.param_path.Replace(".ent", $".{dropTrackLane.SelectedValue}");
                _seqobj.friendly_param += ", " + dropTrackLane.Text;
            }
            //change row header to reflect what the track is
            GenerateDataPoints(trackrowapplied, _seqobj);
            ChangeTrackName(trackrowapplied);
            if (!randomizing) {
                TrackUpdateHighlighting(trackrowapplied, _seqobj);
                TCLE.PlaySound("UIobjectadd");
                SaveLeaf(false, "Applied Object settings", $"{_seqobj.friendly_type} {_seqobj.friendly_param}");
            }
        }
        ///Sets highlighting color of current track
        private void btnTrackColorDialog_Click(object sender, EventArgs e)
        {
            TCLE.PlaySound("UIcoloropen");
            TCLE.colorDialogNew.Color = btnTrackColorDialog.BackColor;
            if (TCLE.colorDialogNew.ShowDialog() == DialogResult.OK) {
                Color selectedcolor = TCLE.colorDialogNew.Color;
                btnTrackColorDialog.BackColor = selectedcolor;
                trackEditor.CurrentRow.HeaderCell.Style.BackColor = TCLE.Blend(selectedcolor, Color.Black, 0.4);
                _tracks[_selecttrack].highlight_color = selectedcolor.ToArgb().ToString();
                //sets flag that leaf has unsaved changes
                TCLE.PlaySound("UIcolorapply");
                SaveLeaf(false, "Changed track color", $"{_tracks[trackEditor.CurrentRow.Index].friendly_type} {_tracks[trackEditor.CurrentRow.Index].friendly_param}");
            }
            //call method to update coloring of track
            TrackUpdateHighlighting(trackEditor.CurrentRow, _tracks[_selecttrack]);
        }
        /// This grabs the highlighting color from each track and then exports them into a file for use in later imports
        private void btnTrackColorExport_Click(object sender, EventArgs e)
        {
            string _out = "";
            //iterate over each track in the editor, writing its highlighting color to the _out string
            foreach (Sequencer_Object seq in _tracks) {
                _out += seq.highlight_color.ToString() + '\n';
            }
            using SaveFileDialog sfd = new();
            sfd.Filter = "Thumper Color Profile (*.color)|*.color";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                //save _out to file with .color extension
                File.WriteAllText(sfd.FileName, _out);
            }
        }
        /// Imports colors from file to the current loaded leaf
        private void btnTrackColorImport_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Filter = "Thumper Color Profile (*.color)|*.color";
            ofd.FilterIndex = 1;
            ofd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (ofd.ShowDialog() == DialogResult.OK) {
                //import all colors in the file to this array
                string[] _colors = File.ReadAllLines(ofd.FileName);
                //then iterate over each track in the editor, applying the colors in the array in order
                for (int x = 0; x < _tracks.Count && x < _colors.Length; x++) {
                    _tracks[x].highlight_color = _colors[x];
                    //call this method to update the colors once the value has been assigned
                    TrackUpdateHighlighting(trackEditor.Rows[x], _tracks[x]);
                }
                SaveLeaf(false, "Imported colors", "");
            }
        }

        private void btnLEafInterpLinear_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedCellCollection _cells = trackEditor.SelectedCells;
            //interpolation requires 2 cells only
            if (_cells.Count != 2) {
                MessageBox.Show("Interpolation works with only 2 cells selected", "Interpolation error");
                return;
            }
            //check if cells are in the same row
            if (_cells[0].RowIndex != _cells[1].RowIndex) {
                MessageBox.Show("Interpolation works only if selected cells are in the same row", "Interpolation error");
                return;
            }

            List<DataGridViewCell> _listcell = new() { _cells[0], _cells[1] };
            _listcell.Sort((cell1, cell2) => cell1.ColumnIndex.CompareTo(cell2.ColumnIndex));

            //basic math to figure out the rate of change across the amount of beats between selections
            decimal _start = (decimal?)_listcell[0].Value ?? 0;
            decimal _end = (decimal?)_listcell[1].Value ?? 0;
            decimal _inc = _start;
            int _beats = _listcell[1].ColumnIndex - _listcell[0].ColumnIndex;
            decimal _diff = TCLE.TruncateDecimal((_end - _start) / _beats, 3);

            for (int x = 1; x < _beats; x++) {
                _inc += _diff;
                //if interpolating for Color, remove the decimals
                if (_tracks[_listcell[0].RowIndex].trait_type == "kTraitColor")
                    _inc = Math.Truncate(_inc);
                trackEditor[_listcell[0].ColumnIndex + x, _listcell[0].RowIndex].Value = _inc;
            }
            //recolor cells after populating
            TrackUpdateHighlighting(trackEditor.Rows[_listcell[0].RowIndex], _tracks[_listcell[0].RowIndex]);
            GenerateDataPoints(trackEditor.Rows[_listcell[0].RowIndex], _tracks[_listcell[0].RowIndex]);
            ShowRawTrackData(trackEditor.Rows[_listcell[0].RowIndex]);
            TCLE.PlaySound("UIinterpolate");
            SaveLeaf(false, $"Interpolated cells {_listcell[0].ColumnIndex} -> {_listcell[1].ColumnIndex}", $"{_tracks[trackEditor.CurrentRow.Index].friendly_type} {_tracks[trackEditor.CurrentRow.Index].friendly_param}");
        }

        private void btnLeafColors_Click(object sender, EventArgs e)
        {
            //do nothing if no cells selected
            if (trackEditor.SelectedCells.Count == 0)
                return;
            TCLE.PlaySound("UIcoloropen");
            if (TCLE.colorDialogNew.ShowDialog() == DialogResult.OK) {
                TCLE.PlaySound("UIcolorapply");
                trackEditor.SelectedCells[0].Value = (decimal)TCLE.colorDialogNew.Color.ToArgb();
                CellValueChanged(trackEditor.SelectedCells[0].RowIndex, trackEditor.SelectedCells[0].ColumnIndex);
            }
        }

        private void btnLeafSplit_Click(object sender, EventArgs e)
        {
            //do nothing if no cells selected
            if (trackEditor.SelectedCells.Count == 0)
                return;
            if (trackEditor.SelectedCells.Count > 1) {
                MessageBox.Show("Select only 1 cell to be the split point", "Leaf split error");
                return;
            }
            //split leaf into 2 leafs
            int splitindex = trackEditor.CurrentCell.ColumnIndex;
            if (MessageBox.Show($"Split this leaf at beat {splitindex}?\nTHIS CHANGE CANNOT BE UNDONE!", "Split leaf", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            //create file renaming dialog and show it
            FileInfo newfilename;
            using SaveFileDialog sfd = new();
            sfd.Filter = "Thumper Leaf File (*.leaf)|*.leaf";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                newfilename = new FileInfo(sfd.FileName);
                newfilename.CreateText();
            }
            else
                return;

            ///SPLIT THAT LEAF
            //build the leaf JSON so we can manipulate it
            JObject _leafsplitbefore = LeafBuildSave(LoadedLeaf.Name);
            //enumerate over each sequencer object and it's values to figure out which ones to keep
            foreach (JObject seq_obj in _leafsplitbefore["seq_objs"].Cast<JObject>()) {
                //data_points contains a list of all data points. By getting Properties() of it,
                //each point becomes its own index
                List<JProperty> data_points = ((JObject)seq_obj["data_points"]).Properties().ToList();
                //iterate over each data point. If it's less than the splitindex, add it to a new list
                JObject newdata = new();
                foreach (JProperty data_point in data_points) {
                    if (int.Parse(data_point.Name) < splitindex)
                        newdata.Add(data_point.Name, data_point.Value);
                }
                seq_obj.Remove("data_points");
                seq_obj.Add("data_points", newdata);
            }
            //set new beat count
            _leafsplitbefore.Remove("beat_cnt");
            _leafsplitbefore.Add("beat_cnt", splitindex);
            //write data back to file
            TCLE.WriteFileLock(TCLE.lockedfiles[LoadedLeaf], _leafsplitbefore);

            ///repeat all above for after split file
            JObject _leafsplitafter = LeafBuildSave(newfilename.Name);
            foreach (JObject seq_obj in _leafsplitafter["seq_objs"].Cast<JObject>()) {
                List<JProperty> data_points = ((JObject)seq_obj["data_points"]).Properties().ToList();
                JObject newdata = new();
                foreach (JProperty data_point in data_points) {
                    if (int.Parse(data_point.Name) >= splitindex)
                        //shift beats back to starting position
                        newdata.Add((int.Parse(data_point.Name) - splitindex).ToString(), data_point.Value);
                }
                seq_obj.Remove("data_points");
                seq_obj.Add("data_points", newdata);
            }
            //set new beat count
            _leafsplitafter.Remove("beat_cnt");
            _leafsplitafter.Add("beat_cnt", numericUpDown_LeafLength.Value - splitindex);
            //write data back to file
            newfilename.CreateText().Write(JsonConvert.SerializeObject(_leafsplitafter, Formatting.Indented));

            TCLE.PlaySound("UIleafsplit");
            //load new leaf that was just split
            TCLE.OpenFile(TCLE.Instance, newfilename);

            //update beat counts in loaded lvl if need be
            ///if (_mainform._loadedlvl != null)
                ///_mainform.btnLvlRefreshBeats.PerformClick();
        }

        private void btnLeafObjRefresh_Click(object sender, EventArgs e)
        {
            ///TCLE.ImportObjects();
            TCLE.PlaySound("UIrefresh");
        }

        private void lblRawData_Click(object sender, EventArgs e)
        {
            if (panelRawData.Height > 20) {
                panelRawData.Height = 20;
                panelRawData.Location = new Point(panelRawData.Location.X, panelLeaf.Height - 20);
                lblRawData.Text = "▲";
                trackEditor.Height += 48;
                foreach (Control c in panelRawData.Controls)
                    c.Visible = false;
                lblRawData.Visible = true;
            }
            else {
                foreach (Control c in panelRawData.Controls)
                    c.Visible = true;
                panelRawData.Height = 48;
                panelRawData.Location = new Point(panelRawData.Location.X, panelLeaf.Height - 68);
                lblRawData.Text = "▼";
                trackEditor.Height -= 48;
            }
        }

        private void btnRevertLeaf_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Revert all changes to last save?", "Revert changes", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SaveLeaf(true, "Revert to last save", "Revert");
            LoadLeaf(leafjson, LoadedLeaf);
            TCLE.PlaySound("UIrevertnew");
        }

        private void btnUndoLeaf_Click(object sender, EventArgs e)
        {
            UndoFunction(1);
        }
        private void btnUndoLeaf_DropDownOpening(object sender, EventArgs e)
        {
            ///btnUndoLeaf.DropDown = CreateUndoMenu(_undolistleaf);
        }

        private void btnLeafAutoPlace_Click(object sender, EventArgs e)
        {
            TCLE.PlaySound("UIselect");
        }

        private void btnLeafRandom_Click(object sender, EventArgs e)
        {
            randomizing = true;
            btnTrackAdd_Click(null, null);
            bool rando = true;
            while (rando) {
                dropObjects.SelectedIndex = TCLE.rng.Next(0, dropObjects.Items.Count);
                if (dropObjects.Text == "PLAY SAMPLE") {
                    dropParamPath.SelectedIndex = 0;
                    if (dropTrackLane.Items.Count > 0)
                        dropTrackLane.SelectedIndex = TCLE.rng.Next(0, dropTrackLane.Items.Count);
                }
                else
                    dropParamPath.SelectedIndex = TCLE.rng.Next(0, dropParamPath.Items.Count);
                if (_tracks.Any(x => (x.friendly_param ?? "").Split(',')[0] == dropParamPath.Text))
                    rando = false;
            }
            btnTrackApply_Click(null, null);

            TCLE.PlaySound("UIaddrandom");
            do {
                RandomizeRowValues(trackEditor.CurrentRow, _tracks[_selecttrack]);
            } while (trackEditor.CurrentRow.Cells.Cast<DataGridViewCell>().Where(x => x.Value != null).ToList().Count == 0);
            ShowRawTrackData(trackEditor.CurrentRow);
            randomizing = false;
            SaveLeaf(false, "Added random object", $"{_tracks.Last().friendly_type} {_tracks.Last().friendly_param}");
        }

        private void btnLeafRandomValues_Click(object sender, EventArgs e)
        {
            if (trackEditor.CurrentRow?.Index is -1 or null)
                return;

            if (MessageBox.Show("Assign random values to the current selected track?", "Confirm randomization", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                TCLE.PlaySound("UIaddrandom");
                do {
                    RandomizeRowValues(trackEditor.CurrentRow, _tracks[_selecttrack]);
                } while (trackEditor.CurrentRow.Cells.Cast<DataGridViewCell>().Where(x => x.Value != null).ToList().Count == 0);
                ShowRawTrackData(trackEditor.CurrentRow);
                SaveLeaf(false, "Set random values", $"{_tracks[trackEditor.CurrentRow.Index].friendly_type} {_tracks[trackEditor.CurrentRow.Index].friendly_param}");
            }
        }

        /// These buttons exist on the Workingfolder panel
        private void btnLeafPanelNew_Click(object sender, EventArgs e)
        {
            ///_mainform.toolstripLeafNew.PerformClick();
        }
        private void btnLeafPanelTemplate_Click(object sender, EventArgs e)
        {
            ///_mainform.toolstripLeafTemplate.PerformClick();
        }
        #endregion

        #region Methods
        ///         ///
        /// METHODS ///
        ///         ///

        public bool _logundo = true;
        public void SaveLeaf(bool save, string changereason, string changedetails, bool playsound = false)
        {
            //skip method if leaf is loading
            if (loadingleaf)
                return;

            //make the beeble emote
            TCLE.beeble.MakeFace();
            EditorIsSaved = save;
            if (!save) {
                SaveLeafColors(true, Color.Maroon);
                if (_logundo) {
                    _undolistleaf.Insert(0, new SaveState() {
                        reason = $"{changereason} [{changedetails}]",
                        savestate = LeafBuildSave((loadedleaf != null) ? LoadedLeaf.Name : "", true)
                    });
                }
            }
            else {
                SaveLeafColors(false, Color.FromArgb(40, 40, 40));
                ///btnRevertLeaf.Enabled = false;
                if (playsound) TCLE.PlaySound("UIsave");
            }
        }
        public static void SaveLeafColors(bool enabled, Color color)
        {
            /*
            btnSaveLeaf.Enabled = enabled;
            btnRevertLeaf.Enabled = leafjson != null;
            btnRevertLeaf.ToolTipText = leafjson != null ? "Revert changes to last save" : "You cannot revert with no file saved";
            toolstripTitleLeaf.BackColor = color;
            */
        }

        public void InitializeLeafStuff()
        {
            dropParamPath.SelectedIndexChanged += dropParamPath_SelectedIndexChanged;
        }

        ///Import raw text from rich text box to selected row
        public void TrackRawImport(DataGridViewRow r, JObject _rawdata)
        {
            if (_tracks.Count == 0)
                return;
            //_rawdata contains a list of all data points. By getting Properties() of it,
            //each point becomes its own index
            List<JProperty> data_points = _rawdata.Properties().ToList();
            //check if the last data point is beyond the beat count. If it is, it will crash or not be included in the track editor
            //Ask the user if they want to expand the leaf to accomadate the data point
            if (data_points.Count > 0 && int.Parse((data_points.Last()).Name) >= r.Cells.Count) {
                if (MessageBox.Show($"Your last data point is beyond the leaf's beat count. Do you want to lengthen the leaf? If you do not, the data point will be left out.\nObject: {r.HeaderCell.Value}\nData point: {data_points.Last()}", "Leaf too short", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    numericUpDown_LeafLength.Value = int.Parse((data_points.Last()).Name) + 1;
            }
            //iterate over each data point, and fill cells
            foreach (JProperty data_point in data_points) {
                try {
                    r.Cells[int.Parse(data_point.Name)].Value = TCLE.TruncateDecimal((decimal)data_point.Value, 3);
                }
                catch (ArgumentOutOfRangeException) { }
            }

            TrackUpdateHighlighting(r, _tracks[r.Index]);
        }
        ///Updates row headers to be the Object and Param_Path
        public void ChangeTrackName(DataGridViewRow r)
        {
            r.HeaderCell.Style.BackColor = TCLE.Blend(Color.FromArgb(int.Parse(_tracks[r.Index].highlight_color)), Color.Black, 0.4);
            if (_tracks[r.Index].friendly_type == "PLAY SAMPLE")
                //show the sample name instead
                r.HeaderCell.Value = _tracks[r.Index].friendly_type + " (" + _tracks[r.Index].obj_name + ")";
            else
                r.HeaderCell.Value = _tracks[r.Index].friendly_type + " (" + _tracks[r.Index].friendly_param + ")";
        }
        ///Takes values in a row and puts in them in the rich text box, condensed
        public static void GenerateDataPoints(DataGridViewRow dgvr, Sequencer_Object _seqobj)
        {
            //iterate over each cell of the selected row
            string allcellvalues = String.Join(",", dgvr.Cells.Cast<DataGridViewCell>().Where(x => x.Value is not null or "").Select(x => $"{x.ColumnIndex}:{x.Value}"));
            object jobj = JsonConvert.DeserializeObject($"{{{allcellvalues}}}");
            _seqobj.data_points = jobj;
        }
        public void ShowRawTrackData(DataGridViewRow dgvr)
        {
            string allcellvalues = String.Join(",", dgvr.Cells.Cast<DataGridViewCell>().Where(x => x.Value is not null or "").Select(x => $"{x.ColumnIndex}:{x.Value}"));
            richRawTrackData.Text = allcellvalues;
        }
        ///Updates column highlighting in the DGV based on time sig
        public void TrackTimeSigHighlighting()
        {
            bool _switch = true;
            //grab the first part of the time sig. This represents how many beats are in a bar
            //tryparse to see if it fails.
            if (!int.TryParse(dropTimeSig.Text.Split('/')[0], out int timesigbeats))
                return;
            for (int i = 0; i < _beats; i++) {
                //whenever `i` is a multiple of the time sig, switch colors
                if (i % timesigbeats == 0)
                    _switch = !_switch;
                trackEditor.Columns[i].DefaultCellStyle.BackColor = _switch ? Color.FromArgb(40, 40, 40) : Color.FromArgb(30, 30, 30);
            }
        }
        ///Updates cell highlighting in the DGV
        public static void TrackUpdateHighlighting(DataGridViewRow r, Sequencer_Object _seqobj)
        {
            //iterate over all cells in the row
            r.HeaderCell.Style.BackColor = TCLE.Blend(Color.FromArgb(int.Parse(_seqobj.highlight_color)), Color.Black, 0.4);
            foreach (DataGridViewCell dgvc in r.Cells) {
                TrackUpdateHighlightingSingleCell(dgvc, _seqobj);
            }
        }
        public static void TrackUpdateHighlightingSingleCell(DataGridViewCell dgvc, Sequencer_Object _seqobj)
        {
            dgvc.Style = null;
            if (dgvc.Value == null)
                return;

            //if it is kTraitColor, color the background differently
            if (_seqobj.trait_type == "kTraitColor") {
                dgvc.Style.BackColor = Color.FromArgb((int)Math.Truncate(double.Parse(dgvc.Value.ToString())));
                return;
            }

            //if the cell value is greater than the criteria of the row, highlight it with that row's color
            if (Math.Abs(Decimal.Parse(dgvc.Value.ToString())) >= (decimal)_seqobj.highlight_value) {
                dgvc.Style.BackColor = Color.FromArgb(int.Parse(_seqobj.highlight_color));
            }
            //change cell font color so text is readable on dark/light backgrounds
            Color _c = dgvc.Style.BackColor;
            if (_c.R < 150 && _c.G < 150 && _c.B < 150)
                dgvc.Style.ForeColor = Color.White;
            else
                dgvc.Style.ForeColor = Color.Black;
        }

        ///Update DGV from _tracks
        public void LoadLeaf(dynamic _load, FileInfo filepath, bool resetundolist = true)
        {
            if (_load == null)
                return;
            //reset flag in case it got stuck previously
            loadingleaf = false;
            bool loadfail = false;
            string loadfailmessage = "";
            //detect if file is actually Leaf or not
            if ((string)_load["obj_type"] != "SequinLeaf") {
                MessageBox.Show($"{filepath.Name} does not appear to be a leaf file.\n'obj_type' was not SequinLeaf.", "Thumper Custom Level Editor");
                return;
            }
            //check if it has a name
            //important for some leaf objects
            if (_load["obj_name"] == null) {
                MessageBox.Show("Leaf missing obj_name parameter. Please set it in the txt file and then reload.", "Thumper Custom Level Editor");
                return;
            }
            //check for template or regular file
            if (filepath.Name == "template") {
                loadedleaf = null;
            }
            else {
                loadedleaf = filepath;
            }

            this.Text = filepath.Name;
            //set flag that load is in progress. This skips SaveLeaf() method
            loadingleaf = true;
            //clear existing tracks
            _tracks.Clear();
            trackEditor.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            //set beat_cnt and time_sig
            int _leaflength = (int?)_load["beat_cnt"] ?? 1;
            numericUpDown_LeafLength.Value = _leaflength > 0 ? (_leaflength > 255 ? 255 : _leaflength) : 1;
            //set timesig for highlighting
            string _time_sig = (string)_load["time_sig"] ?? "4/4";
            if (!dropTimeSig.Items.Contains(_time_sig)) {
                dropTimeSig.Items.Add(_time_sig);
            }
            dropTimeSig.Enabled = true;
            dropTimeSig.SelectedIndex = dropTimeSig.FindStringExact(_time_sig);
            //each object in the seq_objs[] list becomes a track
            foreach (dynamic seq_obj in _load["seq_objs"]) {
                Sequencer_Object _s = new() {
                    obj_name = seq_obj["obj_name"],
                    trait_type = seq_obj["trait_type"],
                    data_points = seq_obj["data_points"],
                    step = seq_obj["step"],
                    _default = seq_obj["default"],
                    footer = seq_obj["footer"].GetType() == typeof(JArray) ? String.Join(",", ((JArray)seq_obj["footer"]).ToList()) : ((string)seq_obj["footer"]).Replace("[", "").Replace("]", ""),
                    //if the leaf has definitions for these, add them. If not, set to defaults
                    param_path = seq_obj.ContainsKey("param_path_hash") ? $"0x{(string)seq_obj["param_path_hash"]}" : (string)seq_obj["param_path"],
                    highlight_value = (int?)seq_obj["editor_data"]?[1] ?? 1,
                    default_interp = ((string)seq_obj["default_interp"]) != null ? ((string)seq_obj["default_interp"]).Replace("kTraitInterp", "") : "Linear"
                };
                //if object is a .samp, set the friendly_param and friendly_type since they don't exist in _objects
                if (_s.param_path == "play") {
                    _s.friendly_type = "PLAY SAMPLE";
                    _s.friendly_param = _s.param_path;
                }
                //otherwise, search _objects for the friendly names for display purposes
                else {
                    try {
                        string reg_param = Regex.Replace(_s.param_path, "[.].*", ".ent");
                        Object_Params objmatch = _objects.First(obj => obj.param_path == reg_param && obj.obj_name == _s.obj_name.Replace((string)_load["obj_name"], "leafname"));
                        _s.friendly_param = objmatch.param_displayname ?? "";
                        _s.friendly_type = objmatch.category ?? "";
                    } catch (Exception) {
                        loadfail = true;
                        loadfailmessage += $"{_s.obj_name} : {_s.param_path}\n";
                    }
                }
                _s.highlight_color = /*(string)seq_obj["editor_data"]?[0] ??*/ objectcolors.TryGetValue(_s.friendly_param, out string value) ? value : "-8355585";
                //if an object can be multi-lane, it will be an .ent. Check for "." to detect this
                if (_s.param_path.Contains('.'))
                    //get the index of the lane from _tracklane to get the item from dropTrackLane, and append that to the friendly_param
                    _s.friendly_param += $", {_tracklanefriendly[_s.param_path.Split('.')[1]]}";
                //finally, add the completed seq_obj to tracks
                _tracks.Add(_s);
            }
            //clear the DGV and prep for new data points
            trackEditor.SuspendLayout();
            trackEditor.Rows.Clear();
            trackEditor.RowCount = _tracks.Count;

            trackEditor.RowHeadersVisible = true;
            trackEditor.Resize -= trackEditor_Resize;
            trackEditor.RowHeadersWidthChanged -= trackEditor_RowHeadersWidthChanged;
            //foreach row, import data points associated with it
            foreach (DataGridViewRow r in trackEditor.Rows) {
                try {
                    //set the headercell names
                    ChangeTrackName(r);
                    //pass _griddata per row to be imported to the DGV
                    TrackRawImport(r, _tracks[r.Index].data_points);
                }
                catch (Exception) { }
            }
            trackEditor.Resize += trackEditor_Resize;
            trackEditor.RowHeadersWidthChanged += trackEditor_RowHeadersWidthChanged;
            trackEditor_RowHeadersWidthChanged(null, null);
            if (loadfail) {
                MessageBox.Show($"Could not find obj_name or param_path for these items:\n{loadfailmessage}");
            }
            trackEditor.ResumeLayout();
            //enable a bunch of elements now that a leaf is loaded.
            EnableLeafButtons(true);
            //re-set the zoom level
            trackZoom_Scroll(null, null);
            trackZoomVert_Scroll(null, null);
            //set scrollbar positions (if set last time this leaf was open)
            trackEditor.RowHeadersWidth = trackEditor.RowHeadersWidth;
            trackEditor.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            try {
                trackEditor.Scroll -= trackEditor_Scroll;
                trackEditor.FirstDisplayedScrollingRowIndex = 0;
                trackEditor.FirstDisplayedScrollingColumnIndex = 0;
                trackEditor.Scroll += trackEditor_Scroll;
            }
            catch {
                trackEditor.Scroll += trackEditor_Scroll;
            }

            loadingleaf = false;
            //clear undo list and reset the leafjson to the new leaf
            if (resetundolist) {
                ClearReloadUndo(_load);
                SaveLeaf(true, "", "");
            }
            else {
                //set save flag to true, since it just barely loaded
                SaveLeafColors(true, Color.Maroon);
            }
        }

        private void EnableLeafButtons(bool enable)
        {
            dropObjects.Enabled = enable;
            dropParamPath.Enabled = enable;
            btnTrackColorDialog.Enabled = enable;
            NUD_TrackHighlight.Enabled = enable;
            btnTrackDelete.Enabled = _tracks.Count > 0;
            btnTrackUp.Enabled = _tracks.Count > 1;
            btnTrackDown.Enabled = _tracks.Count > 1;
            btnTrackClear.Enabled = _tracks.Count > 0;
            btnTrackCopy.Enabled = _tracks.Count > 0;
            btnTrackColorExport.Enabled = btnTrackColorImport.Enabled = _tracks.Count > 0;
            if (!enable) {
                txtDefault.Enabled = false;
                dropLeafInterp.Enabled = false;
                dropLeafStep.Enabled = false;
                btnTrackApply.Enabled = false;
            }
        }

        public JObject LeafBuildSave(string _leafname, bool skiprevertsave = false)
        {
            ///start building JSON output
            JObject _save = new() {
                { "obj_type", "SequinLeaf" },
                { "obj_name", _leafname }
            };

            JArray seq_objs = new();
            foreach (Sequencer_Object seq_obj in _tracks) {
                //skip blank tracks
                if (seq_obj.friendly_param == null)
                    continue;
                JObject s = new();
                //if saving a leaf as a new name, obj_name's have to be updated, otherwise it saves with the old file's name
                if (seq_obj.obj_name.Contains(".leaf") || string.IsNullOrEmpty(seq_obj.obj_name))
                    seq_obj.obj_name = (string)_save["obj_name"];
                s.Add("obj_name", seq_obj.obj_name.Replace("leafname", (string)_save["obj_name"]));
                //write param_path or param_path_hash
                if (seq_obj.param_path.StartsWith("0x"))
                    s.Add("param_path_hash", seq_obj.param_path.Replace("0x", ""));
                else
                    s.Add("param_path", seq_obj.param_path);
                s.Add("trait_type", seq_obj.trait_type);
                s.Add("default_interp", $"kTraitInterp{seq_obj.default_interp}");
                s.Add("data_points", seq_obj.data_points);
                ///end
                //add the rest of the keys to this seq_obj
                s.Add("step", seq_obj.step);
                s.Add("default", seq_obj._default);
                s.Add("footer", seq_obj.footer);
                s.Add("editor_data", new JArray() { new object[] { seq_obj.highlight_color, seq_obj.highlight_value } });

                seq_objs.Add(s);
            }
            //add all seq_objs to the overall leaf
            _save.Add("seq_objs", seq_objs);
            //end leaf with final keys
            _save.Add("beat_cnt", (int)numericUpDown_LeafLength.Value);
            _save.Add("time_sig", dropTimeSig.Text);
            ///end building JSON output
            if (!skiprevertsave)
                leafjson = _save;
            return _save;
        }

        private void ResetLeaf()
        {
            leafjson = null;
            _tracks.Clear();
            trackEditor.Rows.Clear();
            this.Text = "Leaf Editor";
            dropObjects.Enabled = dropParamPath.Enabled = btnTrackApply.Enabled = false;
            //
            SaveLeaf(true, "New", "");
        }

        private void QuickValueChanged(object sender, EventArgs e)
        {
            File.WriteAllText($@"{TCLE.AppLocation}\templates\quickvalues.txt", $"{NUDquick1.Value}\n{NUDquick2.Value}\n{NUDquick3.Value}\n{NUDquick4.Value}\n{NUDquick5.Value}\n{NUDquick6.Value}\n{NUDquick7.Value}\n{NUDquick8.Value}\n{NUDquick9.Value}");
        }
        private void LoadQuickValues()
        {
            if (!File.Exists($@"{TCLE.AppLocation}\templates\quickvalues.txt"))
                return;
            string[] _load = File.ReadAllLines($@"{TCLE.AppLocation}\templates\quickvalues.txt");

            NUDquick1.Value = decimal.TryParse(_load[0], out decimal result) ? result : 1;
            NUDquick2.Value = decimal.TryParse(_load[1], out result) ? result : 1;
            NUDquick3.Value = decimal.TryParse(_load[2], out result) ? result : 1;
            NUDquick4.Value = decimal.TryParse(_load[3], out result) ? result : 1;
            NUDquick5.Value = decimal.TryParse(_load[4], out result) ? result : 1;
            NUDquick6.Value = decimal.TryParse(_load[5], out result) ? result : 1;
            NUDquick7.Value = decimal.TryParse(_load[6], out result) ? result : 1;
            NUDquick8.Value = decimal.TryParse(_load[7], out result) ? result : 1;
            NUDquick9.Value = decimal.TryParse(_load[8], out result) ? result : 1;
        }

        /// <summary>
        /// UNDO FUNCTIONS
        /// </summary>
        private readonly ToolStripDropDownMenu undomenu = new() {
            BackColor = Color.FromArgb(40, 40, 40),
            ShowCheckMargin = false,
            ShowImageMargin = false,
            ShowItemToolTips = false,
            MaximumSize = new Size(2000, 500)
        };
        private ToolStripDropDown CreateUndoMenu(List<SaveState> undolist)
        {
            undomenu.Items.Clear();

            foreach (SaveState s in undolist) {
                ToolStripMenuItem tmsi = new() {
                    Text = s.reason
                };
                tmsi.MouseEnter += undoMenu_MouseEnter;
                tmsi.Click += undoItem_Click;
                tmsi.BackColor = Color.FromArgb(40, 40, 40);
                tmsi.ForeColor = Color.White;
                undomenu.Items.Add(tmsi);
            }
            return undomenu;
        }
        private static void undoMenu_MouseEnter(object sender, EventArgs e)
        {
            Color backcolor = Color.FromArgb(40, 40, 40);
            ToolStrip parent = ((ToolStripMenuItem)sender).Owner;
            for (int x = parent.Items.Count - 1; x >= 0; x--) {
                parent.Items[x].BackColor = backcolor;
                if (parent.Items[x] == sender)
                    backcolor = Color.Maroon;
            }
        }
        private void undoItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tmsi = (ToolStripMenuItem)sender;
            int index = tmsi.Owner.Items.IndexOf(tmsi);

            if (tmsi.Owner.Items.Count == 1 && tmsi.Owner.Items[0].Text.Contains("No changes"))
                return;

            UndoFunction(index + 1);
            TCLE.PlaySound("UIrevertchanges");
        }
        private void UndoFunction(int undoindex)
        {
            if (undoindex >= _undolistleaf.Count) {
                LoadLeaf(_undolistleaf.Last().savestate, LoadedLeaf, false);
                _undolistleaf.RemoveRange(0, _undolistleaf.Count - 1);
            }
            else {
                LoadLeaf(_undolistleaf[undoindex].savestate, LoadedLeaf, false);
                _undolistleaf.RemoveRange(0, undoindex);
            }
        }
        private void ClearReloadUndo(dynamic _load)
        {
            _undolistleaf.Clear();
            leafjson = _load;
            _undolistleaf.Insert(0, new SaveState() {
                reason = $"No changes",
                savestate = leafjson
            });
        }
        ///
        ///
        ///


        public static void RandomizeRowValues(DataGridViewRow dgvr, Sequencer_Object _seqobj)
        {
            Random rng = new();
            int rngchance;
            int rnglimit;
            int randomtype = 0;
            decimal valueiftrue = 0;

            if ((_seqobj.trait_type is "kTraitBool" or "kTraitAction") || (_seqobj.param_path is "visibla01" or "visibla02" or "visible" or "visiblz01" or "visiblz02")) {
                valueiftrue = 1;
                rngchance = 10;
                rnglimit = 9;
                if (_seqobj.obj_name == "sentry.spn") {
                    rngchance = 55;
                    rnglimit = 54;
                }
            }
            else if (_seqobj.trait_type == "kTraitColor") {
                randomtype = 7;
                rngchance = 10;
                rnglimit = 8;
            }
            else {
                rngchance = 10;
                rnglimit = 9;
                if (_seqobj.param_path == "sequin_speed")
                    randomtype = 2;
                else if (_seqobj.obj_name == "fade.pp")
                    randomtype = 3;
                else if (_seqobj.friendly_type == "CAMERA")
                    randomtype = 4;
                else if (_seqobj.friendly_type == "GAMMA")
                    randomtype = 5;
                else
                    randomtype = 6;
            }
            foreach (DataGridViewCell dgvc in dgvr.Cells) {
                switch (randomtype) {
                    case 2:
                        valueiftrue = TCLE.TruncateDecimal((decimal)(rng.NextDouble() * 100) + 0.01m, 3) % 4;
                        break;
                    case 3:
                        valueiftrue = TCLE.TruncateDecimal((decimal)rng.NextDouble(), 3);
                        break;
                    case 4:
                        valueiftrue = TCLE.TruncateDecimal((decimal)(rng.NextDouble() * 100), 3) * (rng.Next(0, 1) == 0 ? 1 : -1);
                        break;
                    case 5:
                        valueiftrue = TCLE.TruncateDecimal((decimal)(rng.NextDouble() * 100), 3);
                        break;
                    case 6:
                        valueiftrue = TCLE.TruncateDecimal((decimal)(rng.NextDouble() * 1000), 3) % 200 * (rng.Next(0, 1) == 0 ? 1 : -1);
                        break;
                    case 7:
                        valueiftrue = Color.FromArgb(rng.Next(256), rng.Next(256), rng.Next(256)).ToArgb();
                        break;
                    default:
                        break;
                }

                dgvc.Value = rng.Next(0, rngchance) >= rnglimit ? valueiftrue : null;
            }
            TrackUpdateHighlighting(dgvr, _seqobj);
            GenerateDataPoints(dgvr, _seqobj);
        }
        #endregion
    }
}
