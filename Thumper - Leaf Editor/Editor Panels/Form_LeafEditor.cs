using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_LeafEditor : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Form Construction
        public Form_LeafEditor(dynamic load = null, FileInfo filepath = null)
        {
            InitializeComponent();
            leaftoolsToolStrip.Renderer = new ToolStripOverride();
            leafToolStrip.Renderer = new ToolStripOverride();
            trackEditor.MouseWheel += new MouseEventHandler(trackEditor_MouseWheel);
            DropDownMenuScrollWheelHandler.Enable(true);
            TCLE.DoubleBufferDGV(trackEditor, true);
            textEditor.Language = FastColoredTextBoxNS.Text.Language.JSON;

            if (load != null) {
                LoadLeaf(load, filepath);
            }
        }
        #endregion

        #region Variables
        public bool EditorIsSaved = true;
        public bool EditorIsLoading;
        public FileInfo loadedleaf
        {
            get => LoadedLeaf;
            set {
                if (LoadedLeaf != value) {
                    LoadedLeaf = value;
                    if (!LoadedLeaf.Exists) {
                        using (StreamWriter sw = LoadedLeaf.CreateText()) {
                            sw.Write(' ');
                            sw.Close();
                        }
                    }
                    TCLE.lockedfiles.Add(loadedleaf, new FileStream(LoadedLeaf.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
                }
            }
        }
        private static FileInfo LoadedLeaf;
        public LeafProperties leafProperties
        {
            get { return LeafProperties; }
            set {
                SaveCheckAndWrite(false);
                LeafProperties = value;
            }
        }
        private LeafProperties LeafProperties;
        private static decimal BPM => TCLE.dockProjectProperties.BPM;
        private IEnumerable<DataGridViewColumn> Columns => trackEditor.Columns.Cast<DataGridViewColumn>().Where(x => x.Index >= 2);
        private dynamic leafjson;
        private int CurrentRow;
        private int MouseCurrentColumn;
        private int FrozenColumnOffset = 2;
        private bool controldown;
        private bool shiftdown;
        private bool altdown;
        private bool randomizing;
        private bool ismoving;
        private bool LogUndo;
        private ObservableCollection<Sequencer_Object> SequencerObjects { get => LeafProperties.seq_objs; set => LeafProperties.seq_objs = value; }
        private Dictionary<string, string> _tracklanefriendly = new() { { "a01", "lane left 2" }, { "a02", "lane left 1" }, { "ent", "lane center" }, { "z01", "lane right 1" }, { "z02", "lane right 2" } };
        private List<string> lanenames = new() { "left", "center", "right" };
        private Dictionary<string, string> kTraitTooltips = new() { { "kTraitBool", "BOOL: accepts values 1 (on) or 0 (off)." }, { "kTraitAction", "ACTION: accepts values 1 (activate)." }, { "kTraitFloat", "FLOAT: accepts decimal values from -32000.0000 to 32000.0000." }, { "kTraitInt", "INT: accepts integer (no decimal) values from -32000 to 32000." }, { "kTraitColor", "COLOR: accepts an integer representation of an ARGB color. Use the color wheel button to insert colors." } };
        private List<Sequencer_Object> clipboardtracks = new();
        private List<SaveState> _undolistleaf = new();
        #endregion

        #region EventHandlers
        #region Scrollbars and Zoom
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
            foreach (DataGridViewColumn dgvc in Columns) {
                dgvc.Width = trackZoom.Value;
            }
            int display = trackEditor.FirstDisplayedScrollingColumnIndex;
            if (trackEditor.ColumnCount > 1 && display != -1) {
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
        }

        private void vscrollbarTrackEditor_Resize()
        {
            vScrollBarTrackEditor.Visible = !(trackEditor.DisplayedRowCount(false) == trackEditor.RowCount);
            vScrollBarTrackEditor.Maximum = trackEditor.RowCount - trackEditor.DisplayedRowCount(false) + 10;
            splitContainerLeafSide.Panel1.Refresh();
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
                if (MouseCurrentColumn != -1) {
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
        #endregion

        private void trackEditor_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex != -1 && e.RowIndex != -1)
                CellPaint(e);
        }
        private void CellPaint(DataGridViewCellPaintingEventArgs e)
        {
            e.Paint(e.CellBounds, DataGridViewPaintParts.All);
            //get dimensions
            int w = Properties.Resources.icon_play.Width;
            int h = Properties.Resources.icon_play.Height;
            int x = e.CellBounds.Left + ((e.CellBounds.Width - w) / 2);
            int y = e.CellBounds.Top + ((e.CellBounds.Height - h) / 2);
            //paint the image
            if (e.ColumnIndex == 0) {
                if (SequencerObjects[e.RowIndex].mute)
                    e.Graphics.DrawImage(Properties.Resources.icon_audio_mute, new Rectangle(x, y, w, h));
                else
                    e.Graphics.DrawImage(Properties.Resources.icon_audio, new Rectangle(x, y, w, h));
            }
            else if (e.ColumnIndex == 1) {
                if (SequencerObjects[e.RowIndex].param_path.EndsWith(".ent"))
                    e.Graphics.DrawImage(Properties.Resources.icon_lanes, new Rectangle(x, y, w, h));
            }
            e.Handled = true;
        }

        private void trackEditor_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex is 1) {
                trackEditor[e.ColumnIndex, e.RowIndex].ToolTipText = "Show/Hide left and right lanes";
            }
        }

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
            CurrentRow = e.RowIndex;
            ShowRawTrackData(trackEditor.Rows[e.RowIndex]);
            List<string> _params;

            try {
                //if track is a multi-lane object, split param_path from lane so both values can be used to update their dropdown boxes
                if (lanenames.Any(x => SequencerObjects[CurrentRow].friendly_param.Contains(x))) {
                    _params = SequencerObjects[CurrentRow].friendly_param.Split(new string[] { ", " }, StringSplitOptions.None).ToList();
                }
                else
                    _params = new List<string>() { SequencerObjects[CurrentRow].friendly_param, "center" };
                //set all controls to their values stored in _tracks
                dropObjects.SelectedIndex = dropObjects.FindStringExact(SequencerObjects[CurrentRow].friendly_type);
                dropParamPath.SelectedIndex = dropParamPath.FindStringExact(_params[0]);
                //needs a different selection method if it's a sample
                if (SequencerObjects[CurrentRow].param_path == "play")
                    dropTrackLane.SelectedIndex = dropTrackLane.FindStringExact(SequencerObjects[CurrentRow].obj_name.Replace(".samp", ""));
                else if (_params.Count >= 2)
                    dropTrackLane.SelectedIndex = dropTrackLane.FindStringExact(_params[1]);
                else //track lane only uses param[0]
                    dropTrackLane.SelectedIndex = dropTrackLane.FindStringExact(_params[0]);
                txtTrait.Text = SequencerObjects[CurrentRow].trait_type;
                btnTrackColorDialog.BackColor = Color.FromArgb(int.Parse(SequencerObjects[CurrentRow].highlight_color));
                //remove event handlers from a few controls so they don't trigger when their values change
                NUD_TrackHighlight.ValueChanged -= NUD_TrackHighlight_ValueChanged;
                txtDefault.ValueChanged -= txtDefault_ValueChanged;
                dropLeafStep.SelectedIndexChanged -= dropLeafStep_SelectedIndexChanged;
                dropLeafInterp.SelectedIndexChanged -= dropLeafInterp_SelectedIndexChanged;
                //set values from _tracks
                NUD_TrackHighlight.Value = (decimal)SequencerObjects[CurrentRow].highlight_value;
                btnTrackColorDialog.BackColor = Color.FromArgb(int.Parse(SequencerObjects[CurrentRow].highlight_color));
                txtDefault.Value = (decimal)SequencerObjects[CurrentRow]._default;
                dropLeafStep.SelectedItem = SequencerObjects[CurrentRow].step;
                dropLeafInterp.SelectedItem = SequencerObjects[CurrentRow].default_interp;
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

                    TrackUpdateHighlightingSingleCell(_cell, SequencerObjects[_cell.RowIndex]);
                }
                //sets flag that leaf has unsaved changes
                if (changes) {
                    if (trackEditor.SelectedCells.Count > 1)
                        SaveCheckAndWrite(false);
                    //SaveCheckAndWrite(false, $"{trackEditor.SelectedCells.Count} beats value set: {_val ?? "empty"}", $"{_tracks[rowindex].friendly_type} {_tracks[rowindex].friendly_param}");
                    else
                        SaveCheckAndWrite(false);
                    //SaveCheckAndWrite(false, $"Beat {columnindex} value set: {_val ?? "empty"}", $"{_tracks[rowindex].friendly_type} {_tracks[rowindex].friendly_param}");
                }
            }
            catch { }

            foreach (DataGridViewRow r in edited)
                GenerateDataPoints(r, SequencerObjects[r.Index]);
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
            if (e.ColumnIndex is 0 or 1) {
                if (e.ColumnIndex is 0)
                    SequencerObjects[e.RowIndex].mute = !SequencerObjects[e.RowIndex].mute;
                trackEditor[e.ColumnIndex, e.RowIndex].Selected = false;
                trackEditor.InvalidateCell(trackEditor[e.ColumnIndex, e.RowIndex]);
            }
            else if (e.Button == MouseButtons.Left && btnLeafAutoPlace.Checked) {
                if (SequencerObjects[e.RowIndex].trait_type is "kTraitBool" or "kTraitAction")
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
                    TrackUpdateHighlightingSingleCell(dgv[e.ColumnIndex, e.RowIndex], SequencerObjects[e.RowIndex]);
                    GenerateDataPoints(dgv.Rows[e.RowIndex], SequencerObjects[e.RowIndex]);
                    SaveCheckAndWrite(false);
                    //SaveCheckAndWrite(false, "Deleted single cell", $"{_tracks[e.RowIndex].friendly_type} {_tracks[e.RowIndex].friendly_param}");
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
            MouseCurrentColumn = e.ColumnIndex;
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;

            DataGridView dgv = sender as DataGridView;
            if (e.ColumnIndex is 0 or 1) {
                dgv[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.FromArgb(174, 161, 255);
            }
            else if (Control.MouseButtons == MouseButtons.Right) {
                if (dgv[e.ColumnIndex, e.RowIndex].Selected == false && dgv[e.ColumnIndex, e.RowIndex].Value != null) {
                    dgv[e.ColumnIndex, e.RowIndex].Value = null;
                    TrackUpdateHighlightingSingleCell(dgv[e.ColumnIndex, e.RowIndex], SequencerObjects[e.RowIndex]);
                    GenerateDataPoints(dgv.Rows[e.RowIndex], SequencerObjects[e.RowIndex]);
                    SaveCheckAndWrite(false);
                    //SaveCheckAndWrite(false, "Deleted single cell", $"{_tracks[e.RowIndex].friendly_type} {_tracks[e.RowIndex].friendly_param}");
                }
                else if (dgv[e.ColumnIndex, e.RowIndex].Selected == true) {
                    dgv[e.ColumnIndex, e.RowIndex].Value = null;
                    CellValueChanged(e.RowIndex, e.ColumnIndex, true);
                    //_undolistleaf.RemoveAt(1);
                }
            }
        }

        private void trackEditor_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;

            DataGridView dgv = sender as DataGridView;
            if (e.ColumnIndex == 0 || e.ColumnIndex == 1) {
                dgv[e.ColumnIndex, e.RowIndex].Style.BackColor = trackEditor.Rows[e.RowIndex].HeaderCell.Style.BackColor;
            }
        }
        //Keypress Backspace - clear selected cells
        private void trackEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back) {
                LogUndo = false;
                CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex, true);
                LogUndo = true;
                SaveCheckAndWrite(false);
                //SaveCheckAndWrite(false, "Deleted cell values", $"{_tracks[_selecttrack].friendly_type} {_tracks[_selecttrack].friendly_param}");
            }
            e.Handled = true;
        }
        private void trackEditor_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            controldown = e.Control;
            shiftdown = e.Shift;
            altdown = e.Alt;
            ///Keypress Delete - clear selected cellss
            //delete cell value if Delete key is pressed
            if (e.KeyCode == Keys.Delete) {
                LogUndo = false;
                CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex, true);
                LogUndo = true;
                SaveCheckAndWrite(false);
                //SaveCheckAndWrite(false, "Deleted cell values", $"{_tracks[_selecttrack].friendly_type} {_tracks[_selecttrack].friendly_param}");
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
                    LogUndo = false;
                    CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex, true);
                    e.Handled = true;
                    LogUndo = true;
                    SaveCheckAndWrite(false);
                    //SaveCheckAndWrite(false, "Cut cells", $"");
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
                                TrackUpdateHighlightingSingleCell(trackEditor[col + i, row + _line], SequencerObjects[row + _line]);
                            }
                            if (!edited.Contains(trackEditor[col + i, row + _line].OwningRow))
                                edited.Add(trackEditor[col + i, row + _line].OwningRow);
                        }
                    }
                    foreach (DataGridViewRow r in edited)
                        GenerateDataPoints(r, SequencerObjects[r.Index]);
                    SaveCheckAndWrite(false);
                    //SaveCheckAndWrite(false, $"Pasted cells", $"");
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
                            TrackUpdateHighlightingSingleCell(trackEditor[dgvc.ColumnIndex + (leftright ? indexdirection : 0), dgvc.RowIndex + (!leftright ? indexdirection : 0)], SequencerObjects[dgvc.RowIndex + (!leftright ? indexdirection : 0)]);
                            //clear the current cell since it moved
                            dgvc.Value = null;
                            TrackUpdateHighlightingSingleCell(dgvc, SequencerObjects[dgvc.RowIndex]);
                        }
                        else {
                            foreach (DataGridViewCell dgvcell in dgvcc)
                                dgvcell.Selected = true;
                            break;
                        }
                    }
                    if (shifted)
                        SaveCheckAndWrite(false);
                    //SaveCheckAndWrite(false, $"Shifted selected cells {(e.KeyCode == Keys.Left ? "left" : "right")}", $"");
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
            dropParamPath.DataSource = TCLE.LeafObjects.Where(obj => obj.category == dropObjects.Text).Select(obj => obj.param_displayname).ToList();
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
                btnTrackColorDialog.BackColor = Color.FromArgb(int.Parse(TCLE.ObjectColors.TryGetValue(dropParamPath.Text, out string value) ? value : "-8355585"));
                //if the param_path is .ent, enable lane choice
                if (TCLE.LeafObjects.First(obj => obj.param_displayname == dropParamPath.Text).param_path.EndsWith(".ent") || (string)dropObjects.SelectedValue == "PLAY SAMPLE") {
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
            SaveCheckAndWrite(false);
            //SaveCheckAndWrite(false, "Time signature", "");
        }
        private void dropTimeSig_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                string s = dropTimeSig.Text;
                // if item exists, select it. if it does not exist, add it.
                if (!TCLE.TimeSignatures.Contains(s)) {
                    TCLE.TimeSignatures.Add(s);
                }
                dropTimeSig.SelectedItem = s;
            }
        }

        ///NUMERIC_UPDOWN TRACK HIGHLIGHT VALUE
        private void NUD_TrackHighlight_ValueChanged(object sender, EventArgs e)
        {
            string data = SequencerObjects[CurrentRow].highlight_value.ToString();
            SequencerObjects[trackEditor.CurrentRow.Index].highlight_value = (float)NUD_TrackHighlight.Value;
            TrackUpdateHighlighting(trackEditor.CurrentRow, SequencerObjects[CurrentRow]);
            //sets flag that leaf has unsaved changes
            SaveCheckAndWrite(false);
            //SaveCheckAndWrite(false, $"Track hilighting value {data} -> {NUD_TrackHighlight.Value}", $"{_tracks[_selecttrack].friendly_type} {_tracks[_selecttrack].friendly_param}");
        }

        ///LEAF - NEW
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((!EditorIsSaved && MessageBox.Show("Current leaf is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || EditorIsSaved) {
                SaveAs();
            }
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
                    FileInfo filepath = new(TCLE.CopyToWorkingFolderCheck(ofd.FileName));
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
            string data = $"{SequencerObjects[CurrentRow]._default}";
            SequencerObjects[trackEditor.CurrentRow.Index]._default = (float)txtDefault.Value;
            //sets flag that leaf has unsaved changes
            if (!randomizing)
                SaveCheckAndWrite(false);
            //SaveCheckAndWrite(false, $"Default value {data} -> {txtDefault.Value}", $"{_tracks[_selecttrack].friendly_type} {_tracks[_selecttrack].friendly_param}");
        }
        ///STEP CHANGED
        private void dropLeafStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            string data = $"{SequencerObjects[CurrentRow].step}";
            SequencerObjects[trackEditor.CurrentRow.Index].step = dropLeafStep.Text;
            if (!randomizing)
                SaveCheckAndWrite(false);
            //SaveCheckAndWrite(false, $"Step value {data} -> {dropLeafStep.Text}", $"{_tracks[_selecttrack].friendly_type} {_tracks[_selecttrack].friendly_param}");
        }
        ///INTERP CHANGED
        private void dropLeafInterp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string data = $"{SequencerObjects[CurrentRow].default_interp}";
            SequencerObjects[trackEditor.CurrentRow.Index].default_interp = dropLeafInterp.Text;
            if (!randomizing)
                SaveCheckAndWrite(false);
            //SaveCheckAndWrite(false, $"Interp value {data} -> {dropLeafInterp.Text}", $"{_tracks[_selecttrack].friendly_type} {_tracks[_selecttrack].friendly_param}");
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
                TrackRawImport(trackEditor.CurrentRow, JObject.Parse($"{{{textEditor.Text}}}"));
                TrackUpdateHighlighting(trackEditor.CurrentRow, SequencerObjects[CurrentRow]);
                GenerateDataPoints(trackEditor.CurrentRow, SequencerObjects[CurrentRow]);
            }
            catch (JsonReaderException ex) {
                MessageBox.Show($"Invalid format or characters in imported data. Please fix.\n\n{ex.Message}", "Thumper Custom Editor Level");
            }
            TCLE.PlaySound("UIkpaste");
        }

        private void btnTrackDelete_Click(object sender, EventArgs e)
        {
            bool _empty = true;
            string data = $"{SequencerObjects[CurrentRow].friendly_type} {SequencerObjects[CurrentRow].friendly_param}";
            List<DataGridViewRow> selectedrows = trackEditor.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().ToList();
            //iterate over current row to see if any cells have data
            List<DataGridViewCell> filledcells = selectedrows.SelectMany(x => x.Cells.Cast<DataGridViewCell>()).Where(x => x.Value != null).ToList();
            if (filledcells.Count > 0)
                _empty = false;
            //if row is not empty, show confirmation box. Otherwise just delete the row
            if ((!_empty && MessageBox.Show("Some cells in the selected tracks have data. Are you sure you want to delete?", "Confirm?", MessageBoxButtons.YesNo) == DialogResult.Yes) || _empty) {
                try {
                    foreach (DataGridViewRow dgvr in selectedrows) {
                        SequencerObjects.RemoveAt(dgvr.Index);
                        trackEditor.Rows.Remove(dgvr);
                    }
                    //sets flag that leaf has unsaved changes
                    TCLE.PlaySound("UIobjectremove");
                    SaveCheckAndWrite(false);
                    //SaveCheckAndWrite(false, "Delete track", data);
                }
                catch { }
            }
            //disable elements if there are no tracks
            if (SequencerObjects.Count == 0) {
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

            SequencerObjects.Add(new Sequencer_Object() {
                highlight_color = null,
                highlight_value = 1
            });
            trackEditor.Rows.Add(new DataGridViewRow() {
                Height = trackZoomVert.Value,
                ReadOnly = true,
                HeaderCell = new DataGridViewRowHeaderCell() { Value = "(apply a track object)" },
                DefaultCellStyle = new DataGridViewCellStyle() { SelectionBackColor = Color.FromArgb(40, 40, 40), SelectionForeColor = Color.Black }
            });
            trackEditor.CurrentCell = trackEditor.Rows[SequencerObjects.Count - 1].Cells[0];
            //disable Apply button if object is not set
            //dropObjects.SelectedIndex = 0;
            if (dropObjects.SelectedIndex == -1 || dropParamPath.SelectedIndex == -1)
                btnTrackApply.Enabled = false;
            else btnTrackApply.Enabled = true;
            //sets flag that leaf has unsaved changes
            if (!randomizing) {
                TCLE.PlaySound("UIobjectadd");
                SaveCheckAndWrite(false);
                //SaveCheckAndWrite(false, "Add new track", "");
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
                    _selectedtracks.Add(new Tuple<Sequencer_Object, DataGridViewRow, int>(SequencerObjects[dgvr.Index], dgvr, dgvr.Index));
                }
                //iterate over rows and shift them up 1 index
                foreach (Tuple<Sequencer_Object, DataGridViewRow, int> _newtrack in _selectedtracks) {
                    SequencerObjects.Remove(_newtrack.Item1);
                    dgv.Rows.Remove(_newtrack.Item2);
                    SequencerObjects.Insert(_newtrack.Item3 - 1, _newtrack.Item1);
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
                SaveCheckAndWrite(false);
                //SaveCheckAndWrite(false, "Move track up", $"{_tracks[_selectedtracks[0].Item3 - 1].friendly_type} {_tracks[_selectedtracks[0].Item3 - 1].friendly_param}");
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
                    _selectedtracks.Add(new Tuple<Sequencer_Object, DataGridViewRow, int>(SequencerObjects[dgvr.Index], dgvr, dgvr.Index));
                }
                //iterate over rows and shift them up 1 index
                foreach (Tuple<Sequencer_Object, DataGridViewRow, int> _newtrack in _selectedtracks) {
                    SequencerObjects.Remove(_newtrack.Item1);
                    dgv.Rows.Remove(_newtrack.Item2);
                    SequencerObjects.Insert(_newtrack.Item3 + 1, _newtrack.Item1);
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
                SaveCheckAndWrite(false);
                //SaveCheckAndWrite(false, "Move track down", $"{_tracks[_selectedtracks[0].Item3 + 1].friendly_type} {_tracks[_selectedtracks[0].Item3 + 1].friendly_param}");
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
                    clipboardtracks.Add(SequencerObjects[dgvr.Index].Clone());
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
                if (lastbeat > LeafProperties.beats) {
                    DialogResult _paste = MessageBox.Show("Copied track is longer than this leaf's beat count. Do you want to extend this leaf's beat count?\nYES = extend leaf and paste\nNO = paste, do not extend leaf\nCANCEL = do not paste", "Pasting leaf track", MessageBoxButtons.YesNoCancel);
                    //YES = extend the leaf and then paste
                    if (_paste == DialogResult.Yes)
                        LeafProperties.beats = lastbeat;
                    //NO = do not extend leaf and then paste
                    //CANCEL = do nothing
                    else if (_paste == DialogResult.Cancel)
                        return;
                }
                //add copied Sequencer_Object to main _tracks list
                foreach (Sequencer_Object _newtrack in clipboardtracks) {
                    _index++;
                    SequencerObjects.Insert(_index, _newtrack.Clone());
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
            SaveCheckAndWrite(false);
            //SaveCheckAndWrite(false, "Pasted tracks", "");
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
            SaveCheckAndWrite(false);
            //SaveCheckAndWrite(false, $"Cleared {selectedrows.Count} track(s)", $"");
        }

        private void btnTrackApply_Click(object sender, EventArgs e)
        {
            Object_Params objmatch = TCLE.LeafObjects.First(obj => obj.category == dropObjects.Text && obj.param_displayname == dropParamPath.Text);
            //fill object properties on the form
            txtTrait.Text = objmatch.trait_type;
            toolTip1.SetToolTip(txtTrait, kTraitTooltips[txtTrait.Text]);
            //enable track highlighting tools
            btnTrackColorDialog.Enabled = true;
            NUD_TrackHighlight.Enabled = true;
            //add track to list and populate with values
            SequencerObjects[CurrentRow] = new Sequencer_Object() {
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
            Sequencer_Object _seqobj = SequencerObjects[CurrentRow];
            DataGridViewRow trackrowapplied = trackEditor.Rows[CurrentRow];
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
                SaveCheckAndWrite(false);
                //SaveCheckAndWrite(false, "Applied Object settings", $"{_seqobj.friendly_type} {_seqobj.friendly_param}");
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
                SequencerObjects[CurrentRow].highlight_color = selectedcolor.ToArgb().ToString();
                //sets flag that leaf has unsaved changes
                TCLE.PlaySound("UIcolorapply");
                SaveCheckAndWrite(false);
                //SaveCheckAndWrite(false, "Changed track color", $"{_tracks[trackEditor.CurrentRow.Index].friendly_type} {_tracks[trackEditor.CurrentRow.Index].friendly_param}");
            }
            //call method to update coloring of track
            TrackUpdateHighlighting(trackEditor.CurrentRow, SequencerObjects[CurrentRow]);
        }
        /// This grabs the highlighting color from each track and then exports them into a file for use in later imports
        private void btnTrackColorExport_Click(object sender, EventArgs e)
        {
            string _out = "";
            //iterate over each track in the editor, writing its highlighting color to the _out string
            foreach (Sequencer_Object seq in SequencerObjects) {
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
                for (int x = 0; x < SequencerObjects.Count && x < _colors.Length; x++) {
                    SequencerObjects[x].highlight_color = _colors[x];
                    //call this method to update the colors once the value has been assigned
                    TrackUpdateHighlighting(trackEditor.Rows[x], SequencerObjects[x]);
                }
                SaveCheckAndWrite(false);
                //SaveCheckAndWrite(false, "Imported colors", "");
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
                if (SequencerObjects[_listcell[0].RowIndex].trait_type == "kTraitColor")
                    _inc = Math.Truncate(_inc);
                trackEditor[_listcell[0].ColumnIndex + x, _listcell[0].RowIndex].Value = _inc;
            }
            //recolor cells after populating
            TrackUpdateHighlighting(trackEditor.Rows[_listcell[0].RowIndex], SequencerObjects[_listcell[0].RowIndex]);
            GenerateDataPoints(trackEditor.Rows[_listcell[0].RowIndex], SequencerObjects[_listcell[0].RowIndex]);
            ShowRawTrackData(trackEditor.Rows[_listcell[0].RowIndex]);
            TCLE.PlaySound("UIinterpolate");
            SaveCheckAndWrite(false);
            //SaveCheckAndWrite(false, $"Interpolated cells {_listcell[0].ColumnIndex} -> {_listcell[1].ColumnIndex}", $"{_tracks[trackEditor.CurrentRow.Index].friendly_type} {_tracks[trackEditor.CurrentRow.Index].friendly_param}");
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
            JObject _leafsplitbefore = BuildSave(LeafProperties);
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
            JObject _leafsplitafter = BuildSave(LeafProperties);
            _leafsplitafter.Remove("obj_name");
            _leafsplitafter.Add("obj_name", newfilename.Name);
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
            _leafsplitafter.Add("beat_cnt", LeafProperties.beats - splitindex);
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

        private void btnRevertLeaf_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Revert all changes to last save?", "Revert changes", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SaveCheckAndWrite(true);
            //SaveCheckAndWrite(true, "Revert to last save", "Revert");
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
                if (SequencerObjects.Any(x => (x.friendly_param ?? "").Split(',')[0] == dropParamPath.Text))
                    rando = false;
            }
            btnTrackApply_Click(null, null);

            TCLE.PlaySound("UIaddrandom");
            do {
                RandomizeRowValues(trackEditor.CurrentRow, SequencerObjects[CurrentRow]);
            } while (trackEditor.CurrentRow.Cells.Cast<DataGridViewCell>().Where(x => x.Value != null).ToList().Count == 0);
            ShowRawTrackData(trackEditor.CurrentRow);
            randomizing = false;
            SaveCheckAndWrite(false);
            //SaveCheckAndWrite(false, "Added random object", $"{_tracks.Last().friendly_type} {_tracks.Last().friendly_param}");
        }

        private void btnLeafRandomValues_Click(object sender, EventArgs e)
        {
            if (trackEditor.CurrentRow?.Index is -1 or null)
                return;

            if (MessageBox.Show("Assign random values to the current selected track?", "Confirm randomization", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                TCLE.PlaySound("UIaddrandom");
                do {
                    RandomizeRowValues(trackEditor.CurrentRow, SequencerObjects[CurrentRow]);
                } while (trackEditor.CurrentRow.Cells.Cast<DataGridViewCell>().Where(x => x.Value != null).ToList().Count == 0);
                ShowRawTrackData(trackEditor.CurrentRow);
                SaveCheckAndWrite(false);
                //SaveCheckAndWrite(false, "Set random values", $"{_tracks[trackEditor.CurrentRow.Index].friendly_type} {_tracks[trackEditor.CurrentRow.Index].friendly_param}");
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
        public void InitializeLeafStuff()
        {
            dropParamPath.SelectedIndexChanged += dropParamPath_SelectedIndexChanged;
        }

        ///Update DGV from _tracks
        public void LoadLeaf(dynamic _load, FileInfo filepath, bool resetundolist = true)
        {
            if (_load == null)
                return;
            //reset flag in case it got stuck previously
            EditorIsLoading = false;
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
            this.Text = LoadedLeaf.Name;
            //set flag that load is in progress. This skips Save method
            EditorIsLoading = true;

            leafProperties = new(this, filepath) {
                beats = (int?)_load["beat_cnt"] ?? 1,
                timesignature = (string)_load["time_sig"] ?? "4/4"
            };

            //clear the DGV and prep for new data
            trackEditor.Rows.Clear();

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
                        Object_Params objmatch = TCLE.LeafObjects.First(obj => obj.param_path == reg_param && obj.obj_name == _s.obj_name.Replace((string)_load["obj_name"], "leafname"));
                        _s.friendly_param = objmatch.param_displayname ?? "";
                        _s.friendly_type = objmatch.category ?? "";
                    }
                    catch (Exception) {
                        loadfail = true;
                        loadfailmessage += $"{_s.obj_name} : {_s.param_path}\n";
                    }
                }
                _s.highlight_color = /*(string)seq_obj["editor_data"]?[0] ??*/ TCLE.ObjectColors.TryGetValue(_s.friendly_param, out string value) ? value : "-8355585";
                //if an object can be multi-lane, it will be an .ent. Check for "." to detect this
                if (_s.param_path.Contains('.'))
                    //get the index of the lane from _tracklane to get the item from dropTrackLane, and append that to the friendly_param
                    _s.friendly_param += $", {_tracklanefriendly[_s.param_path.Split('.')[1]]}";
                //finally, add the completed seq_obj to tracks
                leafProperties.seq_objs.Add(_s);
            }

            trackEditor.RowHeadersVisible = true;
            //foreach row, import data points associated with it
            foreach (DataGridViewRow r in trackEditor.Rows) {
                try {
                    //set the headercell names
                    ChangeTrackName(r);
                    //pass _griddata per row to be imported to the DGV
                    TrackRawImport(r, SequencerObjects[r.Index].data_points);
                }
                catch (Exception) { }
            }
            if (loadfail) {
                MessageBox.Show($"Could not find obj_name or param_path for these items:\n{loadfailmessage}");
            }
            trackEditor.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            //enable a bunch of elements now that a leaf is loaded.
            EnableLeafButtons(true);
            //re-set the zoom level
            trackZoom_Scroll(null, null);
            trackZoomVert_Scroll(null, null);

            //set timesig for highlighting
            dropTimeSig.ComboBox.DataSource = TCLE.TimeSignatures;
            if (!TCLE.TimeSignatures.Contains(LeafProperties.timesignature)) {
                TCLE.TimeSignatures.Add(LeafProperties.timesignature);
            }
            dropTimeSig.SelectedIndex = dropTimeSig.FindStringExact(LeafProperties.timesignature);

            propertyGridLeaf.SelectedObject = LeafProperties;
            //mark that lvl is saved (just freshly loaded)
            EditorIsLoading = false;
            EditorIsSaved = true;
        }

        ///SAVE
        public void Save()
        {
            //if _loadedlvl is somehow not set, force Save As instead
            if (LoadedLeaf == null) {
                SaveAs();
            }
            else
                SaveCheckAndWrite(true, true);
        }
        ///SAVE AS
        public void SaveAs()
        {
            using SaveFileDialog sfd = new();
            //filter .txt only
            sfd.Filter = "Thumper Editor Lvl File (*.leaf)|*.leaf";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                loadedleaf = new FileInfo(sfd.FileName);
                SaveCheckAndWrite(true, true);
                //after saving new file, refresh the project explorer
                TCLE.dockProjectExplorer.CreateTreeView();
            }
        }

        public bool IsSaved()
        {
            return EditorIsSaved;
        }

        public void SaveCheckAndWrite(bool IsSaved, bool playsound = false)
        {
            if (EditorIsLoading)
                return;
            //make the beeble emote
            TCLE.MainBeeble.MakeFace();

            EditorIsSaved = IsSaved;
            if (!IsSaved) {
                //denote editor tab is not saved
                this.Text = LoadedLeaf.Name + "*";
                //add current JSON to the undo list
                leafProperties.undoItems.Add(BuildSave(leafProperties));
            }
            else {
                this.Text = LoadedLeaf.Name;
                //build the JSON to write to file
                JObject _saveJSON = BuildSave(leafProperties);
                leafProperties.revertPoint = _saveJSON;
                //write JSON to file
                TCLE.WriteFileLock(TCLE.lockedfiles[LoadedLeaf], _saveJSON);

                if (playsound) TCLE.PlaySound("UIsave");
                //find if any raw text docs are open of this gate and update them
                TCLE.FindReloadRaw(LoadedLeaf.Name);
                TCLE.FindEditorRunMethod(typeof(Form_LvlEditor), "RecalculateRuntime");
            }
        }
        ///LEAF LENGTH
        public void LeafLengthChanged()
        {
            string data = trackEditor.ColumnCount.ToString();

            if (LeafProperties.beats + FrozenColumnOffset > trackEditor.ColumnCount) {
                trackEditor.ColumnCount = LeafProperties.beats + FrozenColumnOffset;
                TCLE.GenerateColumnStyle(trackEditor.Columns.Cast<DataGridViewColumn>().Where(x => x.Index >= FrozenColumnOffset).ToList(), FrozenColumnOffset);
            }
            else
                trackEditor.ColumnCount = LeafProperties.beats + FrozenColumnOffset;
            //set cell zoom
            trackZoom_Scroll(null, null);
            //make sure new cells follow the time sig
            TrackTimeSigHighlighting();
            //sets flag that leaf has unsaved changes
            SaveCheckAndWrite(false);
            //SaveCheckAndWrite(false, "Leaf length", $"{data} -> {_beats}");
        }

        ///Import raw text from rich text box to selected row
        public void TrackRawImport(DataGridViewRow r, JObject _rawdata)
        {
            if (SequencerObjects.Count == 0)
                return;
            //_rawdata contains a list of all data points. By getting Properties() of it,
            //each point becomes its own index
            List<JProperty> data_points = _rawdata.Properties().ToList();
            //check if the last data point is beyond the beat count. If it is, it will crash or not be included in the track editor
            //Ask the user if they want to expand the leaf to accomadate the data point
            if (data_points.Count > 0 && int.Parse((data_points.Last()).Name) >= r.Cells.Count) {
                if (MessageBox.Show($"Your last data point is beyond the leaf's beat count. Do you want to lengthen the leaf? If you do not, the data point will be left out.\nObject: {r.HeaderCell.Value}\nData point: {data_points.Last()}", "Leaf too short", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    LeafProperties.beats = int.Parse((data_points.Last()).Name) + 1;
            }
            //iterate over each data point, and fill cells
            foreach (JProperty data_point in data_points) {
                try {
                    r.Cells[int.Parse(data_point.Name) + 2].Value = TCLE.TruncateDecimal((decimal)data_point.Value, 3);
                }
                catch (ArgumentOutOfRangeException) { }
            }

            TrackUpdateHighlighting(r, SequencerObjects[r.Index]);
        }
        ///Updates row headers to be the Object and Param_Path
        public void ChangeTrackName(DataGridViewRow r)
        {
            Color background = TCLE.Blend(Color.FromArgb(int.Parse(SequencerObjects[r.Index].highlight_color)), Color.Black, 0.4);
            r.HeaderCell.Style.BackColor = background;
            r.Cells[0].Style.BackColor = background;
            r.Cells[1].Style.BackColor = background;
            if (SequencerObjects[r.Index].friendly_type == "PLAY SAMPLE")
                //show the sample name instead
                r.HeaderCell.Value = SequencerObjects[r.Index].friendly_type + " (" + SequencerObjects[r.Index].obj_name + ")";
            else
                r.HeaderCell.Value = SequencerObjects[r.Index].friendly_type + " (" + SequencerObjects[r.Index].friendly_param + ")";
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
            textEditor.Text = allcellvalues;
        }
        ///Updates column highlighting in the DGV based on time sig
        public void TrackTimeSigHighlighting()
        {
            bool _switch = true;
            //grab the first part of the time sig. This represents how many beats are in a bar
            //tryparse to see if it fails.
            if (!int.TryParse(dropTimeSig.Text.Split('/')[0], out int timesigbeats))
                return;
            for (int i = 0; i < LeafProperties.beats; i++) {
                //whenever `i` is a multiple of the time sig, switch colors
                if ((i) % timesigbeats == 0)
                    _switch = !_switch;
                trackEditor.Columns[i + FrozenColumnOffset].DefaultCellStyle.BackColor = _switch ? Color.FromArgb(40, 40, 40) : Color.FromArgb(30, 30, 30);
            }
        }
        ///Updates cell highlighting in the DGV
        public static void TrackUpdateHighlighting(DataGridViewRow r, Sequencer_Object _seqobj)
        {
            Color background = TCLE.Blend(Color.FromArgb(int.Parse(_seqobj.highlight_color)), Color.Black, 0.4);
            r.HeaderCell.Style.BackColor = background;
            //iterate over all cells in the row
            foreach (DataGridViewCell dgvc in r.Cells) {
                TrackUpdateHighlightingSingleCell(dgvc, _seqobj);
            }
            r.Cells[0].Style.BackColor = background;
            r.Cells[1].Style.BackColor = background;
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

        private void EnableLeafButtons(bool enable)
        {
            dropObjects.Enabled = enable;
            dropParamPath.Enabled = enable;
            btnTrackColorDialog.Enabled = enable;
            NUD_TrackHighlight.Enabled = enable;
            btnTrackDelete.Enabled = SequencerObjects.Count > 0;
            btnTrackUp.Enabled = SequencerObjects.Count > 1;
            btnTrackDown.Enabled = SequencerObjects.Count > 1;
            btnTrackClear.Enabled = SequencerObjects.Count > 0;
            btnTrackCopy.Enabled = SequencerObjects.Count > 0;
            btnTrackColorExport.Enabled = btnTrackColorImport.Enabled = SequencerObjects.Count > 0;
            if (!enable) {
                txtDefault.Enabled = false;
                dropLeafInterp.Enabled = false;
                dropLeafStep.Enabled = false;
                btnTrackApply.Enabled = false;
            }
        }

        public static JObject BuildSave(LeafProperties _properties, bool skiprevertsave = false)
        {
            ///start building JSON output
            JObject _save = new() {
                { "obj_type", "SequinLeaf" },
                { "obj_name", _properties.FilePath.Name }
            };

            JArray seq_objs = new();
            foreach (Sequencer_Object seq_obj in _properties.seq_objs) {
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
            _save.Add("beat_cnt", _properties.beats);
            _save.Add("time_sig", _properties.timesignature);
            ///end building JSON output
            return _save;
        }

        private void ResetLeaf()
        {
            leafjson = null;
            SequencerObjects.Clear();
            trackEditor.Rows.Clear();
            this.Text = "Leaf Editor";
            dropObjects.Enabled = dropParamPath.Enabled = btnTrackApply.Enabled = false;
            //
            SaveCheckAndWrite(true);
        }

        private void QuickValueChanged(object sender, EventArgs e)
        {
            File.WriteAllText($@"{TCLE.AppLocation}\templates\quickvalues.txt", $"{NUDquick1.Value}\n{NUDquick2.Value}\n{NUDquick3.Value}\n{NUDquick4.Value}\n{NUDquick5.Value}\n{NUDquick6.Value}\n{NUDquick7.Value}\n{NUDquick8.Value}\n{NUDquick9.Value}");
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
