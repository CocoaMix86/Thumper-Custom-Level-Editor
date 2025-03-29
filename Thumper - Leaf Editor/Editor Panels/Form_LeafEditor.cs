using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Un4seen.Bass;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_LeafEditor : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Form Construction
        ///Load LEAF
        public Form_LeafEditor(dynamic load = null, FileInfo filepath = null, bool saveonlynoload = false)
        {
            InitializeComponent();
            RenderForm();
            ColorFormElements();
            SaveOnlyNoLoad = saveonlynoload;

            if (load != null) {
                LoadLeaf(load, filepath);
                //each object in the seq_objs[] list becomes a track
                LeafProperties.seq_objs = LoadSequencer(load["seq_objs"], LeafProperties);
                if (!SaveOnlyNoLoad) {
                    LoadTracksFromSequencer(LeafProperties.seq_objs);
                    LoadEnd();
                    UndoList.Add(new SaveState() {
                        reason = "",
                        savestate = load
                    });
                }
                else {
                    EditorIsLoading = false;
                }
            }
        }
        ///Load LVL Sequencer
        public Form_LeafEditor(LvlProperties toload)
        {
            InitializeComponent();
            RenderForm();
            ColorFormElements();
            this.Icon = Properties.Resources.ico_lvl;

            if (toload != null) {
                LvlSequencer = toload;
                LoadLeaf(null, LvlSequencer.FilePath, LvlSequencer);
                LeafProperties.seq_objs = LoadSequencer(LvlSequencer.seqJSON, LeafProperties);
                LoadTracksFromSequencer(LeafProperties.seq_objs);
                LoadEnd();
            }
        }
        private void RenderForm()
        {
            leaftoolsToolStrip.Renderer = new ToolStripOverride();
            leafToolStrip.Renderer = new ToolStripOverride();
            contextMenuInterps.Renderer = new ContextMenuColors();
            contextMenuFav.Renderer = new ContextMenuColors();
            contextMenuFavClear.Renderer = new ContextMenuColors();
            contextMenuFavRemove.Renderer = new ContextMenuColors();
            trackEditor.MouseWheel += new MouseEventHandler(trackEditor_MouseWheel);
            TCLE.DoubleBufferDGV(trackEditor, true);
            textEditor.Language = FastColoredTextBoxNS.Text.Language.JSON;
            BuildObjectTree();
            trackZoom.Value = Properties.Settings.Default.ZoomHoriz;
            trackZoomVert.Value = Properties.Settings.Default.ZoomVert;
            splitContainerLeafSide.SplitterDistance = splitContainerLeafSide.Height - 60;
            splitContainerLeafSide.Panel2Collapsed = Properties.Settings.Default.LeafHideRaw;
        }
        private void Form_LeafEditor_Shown(object sender, EventArgs e)
        {
            vscrollbarTrackEditor_Resize();
            trackEditor.BringToFront();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!IsSaved()) {
                if (MessageBox.Show("File not saved. Are you sure you want to close it and discard changes?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No) {
                    e.Cancel = true;
                }
            }
        }

        public void ColorFormElements()
        {
            this.BackColor = Properties.Settings.Default.ColorLeafBG;
            trackEditor.BackgroundColor = Properties.Settings.Default.ColorLeafSeqBG;
            textEditor.BackColor = Properties.Settings.Default.ColorLeafRawBG;
            textEditor.ForeColor = Properties.Settings.Default.ColorLeafRawText;
        }
        #endregion

        #region Variables
        public bool EditorIsSaved = true;
        public bool EditorIsLoading;
        private bool SaveOnlyNoLoad;
        public FileInfo loadedleaf
        {
            get => LoadedLeaf;
            set {
                if (LoadedLeaf != value) {
                    if (LoadedLeaf != null)
                        TCLE.CloseFileLock(LoadedLeaf);
                    LoadedLeaf = value;
                    if (!LoadedLeaf.Exists) {
                        using (StreamWriter sw = LoadedLeaf.CreateText()) {
                            sw.Write(' ');
                            sw.Close();
                        }
                    }
                    TCLE.AddFileLock(LoadedLeaf);
                }
            }
        }
        private FileInfo LoadedLeaf;
        public LeafProperties leafProperties
        {
            get { return LeafProperties; }
            set {
                LeafProperties = value;
                SaveCheckAndWrite(false, "Leaf Property Change");
            }
        }
        private LeafProperties LeafProperties;
        private IEnumerable<DataGridViewColumn> Columns => trackEditor.Columns.Cast<DataGridViewColumn>().Where(x => x.Index >= FrozenColumnOffset);
        public LvlProperties LvlSequencer;
        private dynamic leafjson;
        private int CurrentRow;
        private int MouseCurrentColumn;
        private int LastRowEdit;
        private int LastColumnEdit;
        private static int FrozenColumnOffset = 3;
        private bool randomizing;
        private bool ismoving;
        private bool isfinding;
        private bool ispasting;
        private bool LogUndo = true;
        private bool GlobalMute;
        private bool GlobalDisable;
        private bool GlobalExpand;
        private bool IsInterpolating;
        private bool ZoomHasChanged;
        private bool ResetRowAfterEdit;
        private bool RightclickDown;
        private bool RightclickChanges;
        private int PlaybackStart = -1;
        private int PlaybackEnd = -1;
        private bool PlaybackLoop;
        private ObservableCollection<Sequencer_Object> SequencerObjects { get => LeafProperties?.seq_objs; set => LeafProperties.seq_objs = value; }
        private StringFormat CellFormat = new(StringFormatFlags.NoWrap) { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
        public List<SaveState> UndoList = new();
        private List<int> SelectedRows = new();
        private List<SeqDataPoint> SelectedDPs = new();
        #endregion

        #region EventHandlers
        #region Scrollbars and Zoom
        private void trackEditor_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int x = e.RowIndex; x < e.RowIndex + e.RowCount; x++)
                trackEditor.Rows[x].Height = trackZoomVert.Value;
            vscrollbarTrackEditor_Resize();
        }

        private void trackEditor_Scroll(object sender, ScrollEventArgs e)
        {
            if (ModifierKeys is Keys.Control) {
                trackEditor.Scroll -= trackEditor_Scroll;
                trackEditor.HorizontalScrollingOffset = e.OldValue;
                trackEditor.Scroll += trackEditor_Scroll;
            }
        }

        private void btnLeafZoom_Click(object sender, EventArgs e)
        {
            TCLE.PlaySound("UIselect");
            panelZoom.Visible = !panelZoom.Visible;
            panelZoom.BringToFront();
            if (!panelZoom.Visible && ZoomHasChanged) {
                ZoomHasChanged = false;
                foreach (Sequencer_Object seq in SequencerObjects)
                    seq.WaveBitmap = null;
            }
        }

        private void trackZoom_Scroll(object sender, EventArgs e)
        {
            Properties.Settings.Default.ZoomHoriz = trackZoom.Value;
            ZoomHasChanged = true;
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
            Properties.Settings.Default.ZoomVert = trackZoomVert.Value;
            ZoomHasChanged = true;
            foreach (DataGridViewRow dgvr in trackEditor.Rows) {
                dgvr.Height = trackZoomVert.Value;
            }
            int display = trackEditor.FirstDisplayedScrollingRowIndex;
            if (display != -1) {
                trackEditor.Scroll -= trackEditor_Scroll;
                vscrollbarTrackEditor_Resize();
                if (trackEditor.Rows[display + 1].Visible == true) {
                    trackEditor.FirstDisplayedScrollingRowIndex = display + 1;
                    trackEditor.FirstDisplayedScrollingRowIndex = display;
                }
                trackEditor.Scroll += trackEditor_Scroll;
            }
        }

        private void trackEditor_Resize(object sender, EventArgs e)
        {
            vscrollbarTrackEditor_Resize();
        }

        private void vscrollbarTrackEditor_Resize()
        {
            vScrollBarTrackEditor.Visible = (trackEditor.DisplayedRowCount(false) < trackEditor.Rows.Cast<DataGridViewRow>().Where(x => x.Visible).Count());
            vScrollBarTrackEditor.Maximum = trackEditor.RowCount - trackEditor.DisplayedRowCount(false) + 10;
        }

        private void trackEditor_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            trackEditor.Focus();
            int scollrowindex = trackEditor.FirstDisplayedScrollingRowIndex;
            int horiz = trackZoom.Value;
            int vert = trackZoomVert.Value;
            int scrollLines = SystemInformation.MouseWheelScrollLines;

            //handle horizontal and vertical scroll
            if (ModifierKeys is not Keys.Control and not Keys.Shift) {
                if (trackEditor.FirstDisplayedScrollingRowIndex == -1 || trackEditor.FirstDisplayedScrollingColumnIndex == -1)
                    return;
                //handle horizontal scroll
                if (MouseCurrentColumn != -1) {
                    trackEditor.HorizontalScrollingOffset = trackEditor.HorizontalScrollingOffset + (e.Delta * -1) < 0 ? 0 : trackEditor.HorizontalScrollingOffset + (e.Delta * -1);
                }
                //handle vertical scroll
                else {
                    if (e.Delta > 0) {
                        int ind = Math.Max(0, scollrowindex - scrollLines);
                        while (trackEditor.Rows[ind].Visible == false && ind > 1)
                            ind -= 1;
                        if (ind == 0) {
                            while (trackEditor.Rows[ind].Visible == false)
                                ind += 1;
                        }
                        trackEditor.FirstDisplayedScrollingRowIndex = ind;
                    }
                    else if (e.Delta < 0) {
                        int ind = Math.Min(trackEditor.RowCount - 1, scollrowindex + scrollLines);
                        while (trackEditor.Rows[ind].Visible == false && ind < trackEditor.RowCount)
                            ind += 1;
                        trackEditor.FirstDisplayedScrollingRowIndex = ind;
                    }
                    vScrollBarTrackEditor.Value = trackEditor.FirstDisplayedScrollingRowIndex;
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
                if (ModifierKeys is Keys.Shift && e.Delta < 0) {
                    trackZoomVert.Value = Math.Max(1, vert - scrollLines);
                }
                else if (ModifierKeys is Keys.Shift && e.Delta > 0) {
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

        private SolidBrush BrushGray = new(Color.Gray);
        private SolidBrush BrushRed = new(Color.Red);
        private SolidBrush BrushGreen = new(Color.Green);
        private SolidBrush BrushBlue = new(Color.Blue);
        private SolidBrush BrushBlack = new(Color.Black);
        private SolidBrush BrushCorn = new(Color.CornflowerBlue);
        private Pen PenCorn = new(new SolidBrush(Color.CornflowerBlue), 3);
        private Pen PenRed = new(new SolidBrush(Color.Red), 3);
        private Pen PenGreen = new(new SolidBrush(Color.Green), 3);
        private Pen PenViolet = new(new SolidBrush(Color.Violet), 3);
        private void trackEditor_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.Handled = true;

            if (RowPostPrePainting) {
                if (e.ColumnIndex < FrozenColumnOffset) {
                    //e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                    CellPaintFancy(e);
                    CellPaintIcons(e);
                }
                return;
            }

            if (e.RowIndex != -1 && e.ColumnIndex >= FrozenColumnOffset) {
                if (Properties.Settings.Default.LeafOptionShowGrid && Properties.Settings.Default.LeafOptionConnectBars) {
                    //if previous cell value is different than this cell, put in a divider
                    //otherwise remove left border to "merge" cells
                    if (e.Value != null && e.Value.ToString() != trackEditor[e.ColumnIndex - 1, e.RowIndex].Value?.ToString())
                        e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.Outset;
                    else if (e.Value != null)
                        e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    //same for right border
                    if (e.ColumnIndex != trackEditor.ColumnCount - 1 && e.Value != null && e.Value.ToString() != trackEditor[e.ColumnIndex + 1, e.RowIndex].Value?.ToString())
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.Outset;
                    else if (e.Value != null)
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (Properties.Settings.Default.LeafOptionShowGrid && !Properties.Settings.Default.LeafOptionConnectBars) {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.Single;
                }
                else if (!Properties.Settings.Default.LeafOptionShowGrid && Properties.Settings.Default.LeafOptionConnectBars) {
                    if (e.Value != null && e.Value.ToString() != trackEditor[e.ColumnIndex - 1, e.RowIndex].Value?.ToString())
                        e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.Outset;
                    else
                        e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    if (e.ColumnIndex != trackEditor.ColumnCount - 1 && e.Value != null && e.Value.ToString() != trackEditor[e.ColumnIndex + 1, e.RowIndex].Value?.ToString())
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.Outset;
                    else
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (!Properties.Settings.Default.LeafOptionShowGrid && !Properties.Settings.Default.LeafOptionConnectBars) {
                    e.AdvancedBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
                }

                if (SequencerObjects[e.RowIndex].friendly_lane is "lane left 2") {
                    e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.InsetDouble;
                    e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Outset;
                }
                else if (SequencerObjects[e.RowIndex].friendly_lane is "lane right 2") {
                    e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.InsetDouble;
                }
                else
                    e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Outset;
            }

            e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.SelectionBackground);
            if (e.ColumnIndex < FrozenColumnOffset) {
                CellPaintFancy(e);
                CellPaintIcons(e);
                return;
            }
            else {
                if (e.RowIndex == -1) {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
                    if (e.ColumnIndex == PlaybackStart + FrozenColumnOffset || e.ColumnIndex == PlaybackEnd + FrozenColumnOffset) {
                        Point p1 = new Point(e.CellBounds.Left + /*(e.CellBounds.Width / 2)*/ - 6, e.CellBounds.Top);
                        Point p2 = new Point(e.CellBounds.Left + /*(e.CellBounds.Width / 2)*/ + 6, e.CellBounds.Top);
                        Point p3 = new Point(e.CellBounds.Left /*+ (e.CellBounds.Width / 2)*/, e.CellBounds.Top + 10);
                        if (e.ColumnIndex == PlaybackStart + FrozenColumnOffset)
                            e.Graphics.FillPolygon(BrushCorn, new[] { p1, p2, p3 });
                        else
                            e.Graphics.FillPolygon(PlaybackLoop ? BrushGreen : BrushRed, new[] { p1, p2, p3 });
                        //e.Graphics.FillRectangle(BrushCorn, e.CellBounds);
                    }
                    return;
                }
            }

            //paint notifier circles for changed interp and ease
            ///if (e.RowIndex != -1 && e.ColumnIndex >= FrozenColumnOffset) {
                if (trackEditor[e.ColumnIndex, e.RowIndex].Selected || ((SequencerObjects[e.RowIndex].category == "PLAY SAMPLE") && Properties.Settings.Default.LeafOptionShowWave)) {

                }
                else if (SequencerObjects[e.RowIndex].editor_row.ReadOnly) {
                    e.Graphics.FillRectangle(BrushGray, e.CellBounds);
                }
                else if (Properties.Settings.Default.LeafOptionThinBars && SequencerObjects[e.RowIndex].friendly_lane == "lane center" && SequencerObjects[e.RowIndex].expandlanes == false) {
                    if (SequencerObjects[e.RowIndex - 2].data_points[e.ColumnIndex - FrozenColumnOffset].value != null)
                        e.Graphics.FillRectangle(SequencerObjects[e.RowIndex].HighlightBrush, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width, e.CellBounds.Height / 5);
                    if (SequencerObjects[e.RowIndex - 1].data_points[e.ColumnIndex - FrozenColumnOffset].value != null)
                        e.Graphics.FillRectangle(SequencerObjects[e.RowIndex].HighlightBrush, e.CellBounds.Left, e.CellBounds.Top + e.CellBounds.Height / 5, e.CellBounds.Width, e.CellBounds.Height / 5);
                    if (SequencerObjects[e.RowIndex].data_points[e.ColumnIndex - FrozenColumnOffset].value != null)
                        e.Graphics.FillRectangle(SequencerObjects[e.RowIndex].HighlightBrush, e.CellBounds.Left, e.CellBounds.Top + (e.CellBounds.Height / 5 * 2), e.CellBounds.Width, e.CellBounds.Height / 5);
                    if (SequencerObjects[e.RowIndex + 1].data_points[e.ColumnIndex - FrozenColumnOffset].value != null)
                        e.Graphics.FillRectangle(SequencerObjects[e.RowIndex].HighlightBrush, e.CellBounds.Left, e.CellBounds.Top + (e.CellBounds.Height / 5 * 3), e.CellBounds.Width, e.CellBounds.Height / 5);
                    if (SequencerObjects[e.RowIndex + 2].data_points[e.ColumnIndex - FrozenColumnOffset].value != null) {
                        e.Graphics.FillRectangle(SequencerObjects[e.RowIndex].HighlightBrush, e.CellBounds.Left, e.CellBounds.Top + (e.CellBounds.Height / 5 * 4), e.CellBounds.Width, e.CellBounds.Height / 5);
                    }
                }
                else if (SequencerObjects[e.RowIndex].trait_type is not "kTraitColor" && SequencerObjects[e.RowIndex].data_points[e.ColumnIndex - FrozenColumnOffset].value != null)
                    e.Graphics.FillRectangle(SequencerObjects[e.RowIndex].HighlightBrush, e.CellBounds);
                else if (SequencerObjects[e.RowIndex].trait_type is "kTraitColor" && SequencerObjects[e.RowIndex].data_points[e.ColumnIndex - FrozenColumnOffset].value != null)
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(Convert.ToInt32(Math.Floor((decimal)SequencerObjects[e.RowIndex].data_points[e.ColumnIndex - FrozenColumnOffset].value)))), e.CellBounds);

                if (Properties.Settings.Default.LeafOptionEaseDots) {
                    if (SequencerObjects[e.RowIndex].data_points[e.ColumnIndex - FrozenColumnOffset].interpolation != "Linear") {
                        e.Graphics.FillEllipse(BrushBlack, e.CellBounds.Right - (e.CellBounds.Width / 2) - 6, e.CellBounds.Top - 1, 7, 7);
                        e.Graphics.FillEllipse(BrushRed, e.CellBounds.Right - (e.CellBounds.Width / 2) - 5, e.CellBounds.Top - 1, 5, 5);
                    }
                    if (SequencerObjects[e.RowIndex].data_points[e.ColumnIndex - FrozenColumnOffset].ease != "Ease In Out") {
                        e.Graphics.FillEllipse(BrushBlack, e.CellBounds.Right - (e.CellBounds.Width / 2), e.CellBounds.Top - 1, 7, 7);
                        e.Graphics.FillEllipse(BrushBlue, e.CellBounds.Right - (e.CellBounds.Width / 2), e.CellBounds.Top - 1, 5, 5);
                    }
                }
            ///}

            e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~(DataGridViewPaintParts.ContentForeground | DataGridViewPaintParts.Border | DataGridViewPaintParts.Background | DataGridViewPaintParts.SelectionBackground));

            if (e.ColumnIndex == PlaybackStart + FrozenColumnOffset) {
                e.Graphics.DrawLine(PenCorn, new Point(e.CellBounds.Left, e.CellBounds.Top), new Point(e.CellBounds.Left, e.CellBounds.Bottom));
            }
            if (e.ColumnIndex == PlaybackEnd + FrozenColumnOffset) {
                e.Graphics.DrawLine(PlaybackLoop ? PenGreen : PenRed, new Point(e.CellBounds.Left, e.CellBounds.Top), new Point(e.CellBounds.Left, e.CellBounds.Bottom));
            }
            if (Playback.IsPlaying && e.ColumnIndex == Playback.PlaybackBeat + FrozenColumnOffset)
            {
                e.Graphics.DrawLine(PenViolet, new Point(e.CellBounds.Left + (int)(e.CellBounds.Width * Playback.PlaybackSubBeat), e.CellBounds.Top), new Point(e.CellBounds.Left + (int)(e.CellBounds.Width * Playback.PlaybackSubBeat), e.CellBounds.Bottom));
                //e.Graphics.DrawLine(PenViolet, new Point(e.CellBounds.Left + e.CellBounds.Width / 2, e.CellBounds.Top), new Point(e.CellBounds.Left + e.CellBounds.Width / 2, e.CellBounds.Bottom));
            }

            e.Paint(e.CellBounds, DataGridViewPaintParts.Border);

            //check if previous cell is the same value. If so, hide it
            if ((e.PaintParts & DataGridViewPaintParts.ContentForeground) != 0 && e.Value != null/* && e.ColumnIndex != -1 && e.RowIndex != -1*/) {
                //
                if (SequencerObjects[e.RowIndex].category == "PLAY SAMPLE" && Properties.Settings.Default.LeafOptionShowWave) ;
                else if (SequencerObjects[e.RowIndex].trait_type is "kTraitColor") ;
                else if ((Properties.Settings.Default.LeafOptionThinBars && SequencerObjects[e.RowIndex].friendly_lane == "lane center" && SequencerObjects[e.RowIndex].expandlanes == false)) ;
                else if (Properties.Settings.Default.LeafOptionConnectBars && e.ColumnIndex >= FrozenColumnOffset && e.Value.ToString() == trackEditor[e.ColumnIndex - 1, e.RowIndex].Value?.ToString()) ;
                else {
                    Color _c = SequencerObjects[e.RowIndex].highlight_color;
                    if (_c.R < 150 && _c.G < 150 && _c.B < 150)
                        e.CellStyle.ForeColor = Color.White;
                    else
                        e.CellStyle.ForeColor = Color.Black;
                    string cellText = e.Value.ToString();
                    for (int fontSize = 1; fontSize < 25; fontSize++) {
                        Font font = new("Consolas", fontSize);
                        Size textSize = TextRenderer.MeasureText(cellText, font);
                        if (textSize.Width > e.CellBounds.Width + 2 || textSize.Height > e.CellBounds.Height || fontSize == 24) {
                            if (fontSize - 1 != 0)
                                font = new Font("Consolas", fontSize - 1);
                            e.CellStyle.Font = font;
                            e.Graphics.DrawString(cellText, font, new SolidBrush(e.CellStyle.ForeColor), e.CellBounds, CellFormat);
                            break;
                        }
                    }
                }
            }

            //if (e.ColumnIndex is 0 or 1 or 2)
                //CellPaintIcons(e);
        }

        int w = 16;
        int h = 16;
        private void CellPaintIcons(DataGridViewCellPaintingEventArgs e)
        {
            //get dimensions
            int x = e.CellBounds.Left + ((e.CellBounds.Width - w) / 2);
            int y = e.CellBounds.Top + ((e.CellBounds.Height - h) / 2);
            //paint the image
            //Object Toggle
            if (e.ColumnIndex == 0) {
                if (e.RowIndex == -1) {
                    e.Graphics.DrawImage(GlobalDisable ? Properties.Resources.icon_toggle_off : Properties.Resources.icon_toggle_on, new Rectangle(x, y, w, h));
                }
                else {
                    e.Graphics.DrawImage(SequencerObjects[e.RowIndex].enabled ? Properties.Resources.icon_toggle_on : Properties.Resources.icon_toggle_off, new Rectangle(x, y, w, h));
                    trackEditor[e.ColumnIndex, e.RowIndex].Selected = false;
                }
            }
            //Audio Mute/Unmute
            else if (e.ColumnIndex == 1) {
                if (e.RowIndex == -1) {
                    e.Graphics.DrawImage(GlobalMute ? Properties.Resources.icon_audio_mute : Properties.Resources.icon_audio, new Rectangle(x, y, w, h));
                }
                else {
                    e.Graphics.DrawImage(SequencerObjects[e.RowIndex].mute ? Properties.Resources.icon_audio_mute : Properties.Resources.icon_audio, new Rectangle(x, y, w, h));
                    trackEditor[e.ColumnIndex, e.RowIndex].Selected = false;
                }
            }
            //Lane Expand
            else if (e.ColumnIndex == 2) {
                if (e.RowIndex == -1)
                    e.Graphics.DrawImage(Properties.Settings.Default.LeafOptionShowLane ? Properties.Resources.icon_lanesgray : Properties.Resources.icon_lanes, new Rectangle(x, y, w, h));
                else if (SequencerObjects[e.RowIndex].friendly_lane == "lane center") {
                    e.Graphics.DrawImage(Properties.Settings.Default.LeafOptionShowLane ? Properties.Resources.icon_lanesgray : Properties.Resources.icon_lanes, new Rectangle(x, y, w, h));
                    trackEditor[e.ColumnIndex, e.RowIndex].Selected = false;
                }
            }
        }

        SolidBrush CellPaintingPen = new SolidBrush(Color.FromArgb(60, 60, 60));
        SolidBrush CellPaintingBlack = new SolidBrush(Color.Black);
        SolidBrush CellPaintingWhite = new SolidBrush(Color.White);
        SolidBrush CellPaintingColor = new SolidBrush(Color.Black);
        private void CellPaintFancy(DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            Rectangle bounds = e.CellBounds;
            if (e.ColumnIndex is -1) {
                e.Graphics.FillRectangle(CellPaintingBlack, e.CellBounds);
                CellPaintingColor.Color = TCLE.Blend(SequencerObjects[e.RowIndex].highlight_color, Color.Black, 0.4);
                bounds.X += 2;
                bounds.Y += 2;
                bounds.Width -= 4;
                bounds.Height -= 4;
                //if row has a selected cell, highlight it
                if (SelectedRows.Contains(e.RowIndex))
                {
                    e.Graphics.FillRoundedRectangle(CellPaintingWhite, new Rectangle(bounds.X - 1, bounds.Y - 1, bounds.Width + 2, bounds.Height + 2), 5);
                    CellPaintingColor.Color = TCLE.Blend(SequencerObjects[e.RowIndex].highlight_color, Color.Black, 0.8);
                }
                e.Graphics.FillRoundedRectangle(CellPaintingColor, bounds, 5);
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
            }
            else if (e.ColumnIndex is 0 or 1) {
                bounds.X += 1;
                bounds.Y += 1;
                bounds.Width -= 2;
                bounds.Height -= 2;
                e.Graphics.FillRectangle(CellPaintingBlack, e.CellBounds);
                e.Graphics.FillRoundedRectangle(CellPaintingPen, bounds, 4);
            }
            else if (e.ColumnIndex is 2)
            {
                e.Graphics.FillRectangle(CellPaintingBlack, e.CellBounds);
                bounds.X += 1;
                bounds.Y += 1;
                bounds.Width -= 6;
                bounds.Height -= 2;
                if (SequencerObjects[e.RowIndex].friendly_lane == "lane left 2") {
                    bounds.Height += 4;
                    e.Graphics.FillRoundedRectangle(CellPaintingPen, bounds, 4);
                }
                else if (SequencerObjects[e.RowIndex].friendly_lane == "lane right 2") {
                    bounds.Y -= 2;
                    e.Graphics.FillRoundedRectangle(CellPaintingPen, bounds, 4);
                    //this rectangle is needed to square off the top of the above rounded rectangle
                    e.Graphics.FillRectangle(CellPaintingPen, new Rectangle(bounds.X, bounds.Y, bounds.Width, 5));
                }
                else if (SequencerObjects[e.RowIndex].friendly_lane is "lane left 1" or "lane right 1" || (SequencerObjects[e.RowIndex].expandlanes && SequencerObjects[e.RowIndex].friendly_lane is "lane center")) {
                    bounds.Height += 3;
                    bounds.Y -= 3;
                    e.Graphics.FillRectangle(CellPaintingPen, bounds);
                }
                else
                    e.Graphics.FillRoundedRectangle(CellPaintingPen, bounds, 4);
            }
            //e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.SelectionBackground | DataGridViewPaintParts.Border);
        }

        private void trackEditor_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
        }

        private bool RowPrePainting;
        private bool RowPostPrePainting;
        private void trackEditor_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            e.Handled = true;
            if (SequencerObjects[e.RowIndex].category == "PLAY SAMPLE" && TCLE.Instance.leafoptionShowWave.Checked) {
                RowPrePainting = true;
                e.PaintCells(e.RowBounds, e.PaintParts);
                RowPrePainting = false;

                if (!SequencerObjects[e.RowIndex].data_points.Any(x => x.value != null)) {
                    goto paintheader;
                }
                //setup variables to reference later when needed
                int offsetportion = (trackEditor.Columns[3].Width - trackEditor.FirstDisplayedScrollingColumnHiddenWidth) + trackEditor.RowHeadersWidth + (trackEditor.Columns[0].Width * 3) + 4;
                int columnindex = trackEditor.FirstDisplayedScrollingColumnIndex - FrozenColumnOffset + 1;
                Sequencer_Object seqref = SequencerObjects[e.RowIndex];
                SampleData samp = TCLE.ProjectSamples.FirstOrDefault(x => x.obj_name == seqref.obj_name);
                if (samp == null) {
                    goto paintheader;
                }
                //export pc file to playable file
                if (samp.wave == null) {
                    samp.CalculateRuntime();
                }
                int cellwidth = trackZoom.Value;
                samp.wave.ColorBackground = seqref.editor_row.ReadOnly ? Color.FromArgb(45, 45, 45) : seqref.highlight_color;
                //if object has no drawn wave, create it. Wave is null whenever cell sizes change
                if (seqref.WaveBitmap == null) {
                    Bitmap WaveToDraw = samp.wave.CreateBitmap((int)Math.Floor(cellwidth * samp.beats), e.RowBounds.Height - 4, -1, -1, true);
                    using (Graphics graphics = Graphics.FromImage(WaveToDraw)) {
                        graphics.DrawLine(new Pen(Color.Black, 5), 0, 0, 0, WaveToDraw.Height);
                        graphics.DrawLine(new Pen(Color.Black, 5), WaveToDraw.Width, 0, WaveToDraw.Width, WaveToDraw.Height);
                    }
                    seqref.WaveBitmap = WaveToDraw;
                }
                //once the bitmap is created, now we can do some funky stuff
                foreach (SeqDataPoint sdp in seqref.data_points.Where(x => x.value != null)) {
                    if (sdp.beat > columnindex + trackEditor.DisplayedColumnCount(true) && sdp.beat + samp.beats < columnindex)
                        continue;
                    //math to offset drawing the wave horizontally based on where the active beats are
                    e.Graphics.DrawImage(seqref.WaveBitmap, ((sdp.beat - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + 2, (int)Math.Floor(cellwidth * samp.beats), e.RowBounds.Height - 4);
                }
                RowPostPrePainting = true;
                e.PaintCells(e.RowBounds, e.PaintParts);
                RowPostPrePainting = false;
            }
            //HANDLE OBJECTS THAT LAST LONGER THAN 1 BEAT
            else if (SequencerObjects[e.RowIndex].friendly_param.Contains('[')) {
                RowPrePainting = true;
                e.PaintCells(e.RowBounds, e.PaintParts);
                RowPrePainting = false;

                if (!SequencerObjects[e.RowIndex].data_points.Any(x => x.value != null))
                    goto paintheader;
                bool success = int.TryParse(SequencerObjects[e.RowIndex].friendly_param.Split('[')[1].Split(' ')[0], out int beats);
                beats--;
                if (!success)
                    goto paintheader;
                int offsetportion = (trackEditor.Columns[3].Width - trackEditor.FirstDisplayedScrollingColumnHiddenWidth) + trackEditor.RowHeadersWidth + (trackEditor.Columns[0].Width * 3) + 4;
                int columnindex = trackEditor.FirstDisplayedScrollingColumnIndex - FrozenColumnOffset + 1 - 1;
                Sequencer_Object seqref = SequencerObjects[e.RowIndex];
                int cellwidth = trackZoom.Value;
                Color alpha = seqref.highlight_color;
                alpha = seqref.editor_row.ReadOnly ? Color.Gray : Color.FromArgb(100, alpha.R, alpha.G, alpha.B);
                //
                if (Properties.Settings.Default.LeafOptionThinBars && SequencerObjects[e.RowIndex].friendly_lane == "lane center" && SequencerObjects[e.RowIndex].expandlanes == false) {
                    int trailstop = 0;
                    foreach (SeqDataPoint sdp in SequencerObjects[e.RowIndex - 2].data_points.Where(x => x.value != null)) {
                        //don't draw trail if it already has has happened from a previous one
                        if (sdp.beat < trailstop)
                            continue;
                        if (sdp.beat > columnindex + trackEditor.DisplayedColumnCount(true) && sdp.beat + beats < columnindex) continue;
                        e.Graphics.FillRectangle(new SolidBrush(alpha), ((sdp.beat - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top, beats * cellwidth, e.RowBounds.Height / 5);
                        trailstop = sdp.beat + beats;
                    }
                    trailstop = 0;
                    foreach (SeqDataPoint sdp in SequencerObjects[e.RowIndex - 1].data_points.Where(x => x.value != null)) {
                        //don't draw trail if it already has has happened from a previous one
                        if (sdp.beat < trailstop)
                            continue;
                        if (sdp.beat > columnindex + trackEditor.DisplayedColumnCount(true) && sdp.beat + beats < columnindex) continue;
                        e.Graphics.FillRectangle(new SolidBrush(alpha), ((sdp.beat - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + e.RowBounds.Height / 5, beats * cellwidth, e.RowBounds.Height / 5);
                        trailstop = sdp.beat + beats;
                    }
                    trailstop = 0;
                    foreach (SeqDataPoint sdp in SequencerObjects[e.RowIndex].data_points.Where(x => x.value != null)) {
                        //don't draw trail if it already has has happened from a previous one
                        if (sdp.beat < trailstop)
                            continue;
                        if (sdp.beat > columnindex + trackEditor.DisplayedColumnCount(true) && sdp.beat + beats < columnindex) continue;
                        e.Graphics.FillRectangle(new SolidBrush(alpha), ((sdp.beat - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + e.RowBounds.Height / 5 * 2, beats * cellwidth, e.RowBounds.Height / 5);
                        trailstop = sdp.beat + beats;
                    }
                    trailstop = 0;
                    foreach (SeqDataPoint sdp in SequencerObjects[e.RowIndex + 1].data_points.Where(x => x.value != null)) {
                        //don't draw trail if it already has has happened from a previous one
                        if (sdp.beat < trailstop)
                            continue;
                        if (sdp.beat > columnindex + trackEditor.DisplayedColumnCount(true) && sdp.beat + beats < columnindex) continue;
                        e.Graphics.FillRectangle(new SolidBrush(alpha), ((sdp.beat - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + e.RowBounds.Height / 5 * 3, beats * cellwidth, e.RowBounds.Height / 5);
                        trailstop = sdp.beat + beats;
                    }
                    trailstop = 0;
                    foreach (SeqDataPoint sdp in SequencerObjects[e.RowIndex + 2].data_points.Where(x => x.value != null)) {
                        //don't draw trail if it already has has happened from a previous one
                        if (sdp.beat < trailstop)
                            continue;
                        if (sdp.beat > columnindex + trackEditor.DisplayedColumnCount(true) && sdp.beat + beats < columnindex) continue;
                        e.Graphics.FillRectangle(new SolidBrush(alpha), ((sdp.beat - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + e.RowBounds.Height / 5 * 4, beats * cellwidth, e.RowBounds.Height / 5);
                        trailstop = sdp.beat + beats;
                    }
                }
                else {
                    int trailstop = 0;
                    foreach (SeqDataPoint sdp in seqref.data_points.Where(x => x.value != null)) {
                        //don't draw trail if it already has has happened from a previous one
                        if (sdp.beat < trailstop)
                            continue;
                        if (sdp.beat > columnindex + trackEditor.DisplayedColumnCount(true) && sdp.beat + beats < columnindex) continue;
                        e.Graphics.FillRectangle(new SolidBrush(alpha), ((sdp.beat - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + 2, beats * cellwidth, e.RowBounds.Height - 4);
                        trailstop = sdp.beat + beats;
                    }
                }
                RowPostPrePainting = true;
                e.PaintCells(e.RowBounds, e.PaintParts);
                RowPostPrePainting = false;
            }
            else {
                e.PaintCells(e.RowBounds, DataGridViewPaintParts.All);
            }
        paintheader:
            RowPrePainting = true;
            e.PaintHeader(true);
            RowPrePainting = false;
        }

        private void trackEditor_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            LastRowEdit = e.RowIndex;
            LastColumnEdit = e.ColumnIndex;
            //ResetRowAfterEdit = true;
        }

        private void trackEditor_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            object _tempval = trackEditor[e.ColumnIndex, e.RowIndex].Value;
            //check if value to be set works with the objects type
            if (SequencerObjects[e.RowIndex].trait_type == "kTraitBool") {
                if ((decimal)_tempval is not 1 or 0)
                    _tempval = 1m;
            }
            else if (SequencerObjects[e.RowIndex].trait_type == "kTraitColor") {
                _tempval = TCLE.TruncateDecimal((decimal)_tempval, 0);
            }
            else if (SequencerObjects[e.RowIndex].trait_type == "kTraitAction") {
                if ((decimal)_tempval is not 1)
                    _tempval = 1m;
            }
            else if (SequencerObjects[e.RowIndex].trait_type == "kTraitInt") {
                _tempval = TCLE.TruncateDecimal((decimal)_tempval, 0);
            }

            trackEditor[e.ColumnIndex, e.RowIndex].Value = _tempval;
        }

        private void trackEditor_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (isfinding)
                return;
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
                return;
            if (e.ColumnIndex is 0) {
                trackEditor[e.ColumnIndex, e.RowIndex].ToolTipText = "Enable/Disable";
            }
            else if (e.ColumnIndex is 1) {
                trackEditor[e.ColumnIndex, e.RowIndex].ToolTipText = "Mute/Unmute";
            }
            else if (e.ColumnIndex is 2) {
                //only add tooltip if the object can have lanes
                if (SequencerObjects[e.RowIndex].friendly_lane != "none")
                    trackEditor[e.ColumnIndex, e.RowIndex].ToolTipText = "Show/Hide Lanes";
            }


        }

        ///DATAGRIDVIEW - TRACK EDITOR
        private void trackEditor_SelectionChanged(object sender, EventArgs e)
        {
            //do nothing if the selection is inside the frozen columns
            if (trackEditor.SelectedCells.Count == 0 || trackEditor.SelectedCells[^1].ColumnIndex - FrozenColumnOffset < 0)
                return;
            if (ResetRowAfterEdit && trackEditor.CurrentCell.ColumnIndex == LastColumnEdit) {
                ResetRowAfterEdit = false;
                trackEditor.CurrentCell = trackEditor[trackEditor.CurrentCell.ColumnIndex, LastRowEdit];
            }

            //certain actions only available with cells selected
            bool enable = trackEditor.SelectedCells.Count > 0;
            btnTrackUp.Enabled = enable;
            btnTrackDown.Enabled = enable;
            btnTrackCopy.Enabled = enable;
            btnTrackDelete.Enabled = enable;
            btnTrackClear.Enabled = enable;
            //
            SelectedRows = trackEditor.SelectedCells.Cast<DataGridViewCell>()
                .Select(cell => cell.RowIndex)
                .Distinct().ToList();
            trackEditor.Invalidate();
            //get all selected cells and display them grouped together in the propertygrid
            //this allows for mass editing
            SelectedDPs.Clear();
            foreach (DataGridViewCell dgvc in trackEditor.SelectedCells) {
                SelectedDPs.Add(SequencerObjects[dgvc.RowIndex].data_points[dgvc.ColumnIndex - FrozenColumnOffset]);
            }
            propertyGridLeaf.SelectedObjects = SelectedDPs.ToArray();
            propertyGridLeaf.Refresh();
        }

        private void trackEditor_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewTextBoxEditingControl tb = (DataGridViewTextBoxEditingControl)e.Control;
            //tb.KeyPress += new KeyPressEventHandler(cellEditingKeyPress);
            tb.PreviewKeyDown += new PreviewKeyDownEventHandler(cellEditingKeyPress);
            e.Control.PreviewKeyDown += new PreviewKeyDownEventHandler(cellEditingKeyPress);
        }

        private void cellEditingKeyPress(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ResetRowAfterEdit = true;
        }
        //Row changed
        private void trackEditor_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (ismoving || isfinding)
                return;
            CurrentRow = e.RowIndex;
            ShowRawTrackData(SequencerObjects[e.RowIndex]);
            leafProperties.selectedobj = SequencerObjects[e.RowIndex];
            propertyGridLeaf.Refresh();
        }

        //Cell value changed
        private void trackEditor_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
                return;
            if (trackEditor.IsCurrentCellInEditMode) {
                CellValueChanged(e.RowIndex, e.ColumnIndex);
            }
        }
        public void CellValueChanged(int rowindex, int columnindex, bool setnull = false)
        {
            if (IsInterpolating || ispasting)
                return;
            try {
                bool changes = false;
                object _val = null;
                if (setnull)
                    _val = null;
                else if (Decimal.TryParse(trackEditor[columnindex, rowindex].EditedFormattedValue?.ToString(), out decimal _valtoset))
                    _val = TCLE.TruncateDecimal(_valtoset, 3);
                //iterate over each cell in the selection
                List<DataGridViewCell> CellsToChange = new();
                if (trackEditor[columnindex, rowindex].Selected)
                    CellsToChange = trackEditor.SelectedCells.Cast<DataGridViewCell>().ToList();
                else
                    CellsToChange.Add(trackEditor[columnindex, rowindex]);
                foreach (DataGridViewCell _cell in CellsToChange) {
                    object _tempval = _val;

                    if (_cell.ReadOnly || !_cell.OwningRow.Visible)
                        continue;

                    if (_tempval == null) {
                        CellValueNull(_cell);
                    }
                    else {
                        //check if value to be set works with the objects type
                        if (SequencerObjects[_cell.RowIndex].trait_type == "kTraitBool") {
                            if ((decimal)_tempval is not 1 or 0)
                                _tempval = 1m;
                        }
                        else if (SequencerObjects[_cell.RowIndex].trait_type == "kTraitColor") {
                            _tempval = TCLE.TruncateDecimal((decimal)_tempval, 0);
                        }
                        else if (SequencerObjects[_cell.RowIndex].trait_type == "kTraitAction") {
                            if ((decimal)_tempval is not 1 or 0)
                                _tempval = 1m;
                        }
                        else if (SequencerObjects[_cell.RowIndex].trait_type == "kTraitInt") {
                            _tempval = TCLE.TruncateDecimal((decimal)_tempval, 0);
                        }
                        //if cell does not have the value, set it
                        if (_cell.Value != _tempval) {
                            _cell.Value = _tempval;
                            SequencerObjects[_cell.RowIndex].data_points[_cell.ColumnIndex - FrozenColumnOffset].value = (decimal?)_tempval;
                            changes = true;
                        }
                    }
                    ///else if (SequencerObjects[_cell.RowIndex].data_points[_cell.ColumnIndex - FrozenColumnOffset].value != _tempval)

                    ///TrackUpdateHighlightingSingleCell(_cell, SequencerObjects[_cell.RowIndex]);
                }
                //sets flag that leaf has unsaved changes
                if (changes) {
                    SaveCheckAndWrite(false, "Cell Value(s) Updated");
                }
            }
            catch { }
            ShowRawTrackData(SequencerObjects[rowindex]);
        }

        private bool CellValueNull(DataGridViewCell _cell)
        {
            SequencerObjects[_cell.RowIndex].data_points[_cell.ColumnIndex - FrozenColumnOffset].value = null;

            if (SequencerObjects[_cell.RowIndex].expandlanes == false && SequencerObjects[_cell.RowIndex].friendly_lane == "lane center") {
                SequencerObjects[_cell.RowIndex - 2].data_points[_cell.ColumnIndex - FrozenColumnOffset].value = null;
                SequencerObjects[_cell.RowIndex - 1].data_points[_cell.ColumnIndex - FrozenColumnOffset].value = null;
                SequencerObjects[_cell.RowIndex + 1].data_points[_cell.ColumnIndex - FrozenColumnOffset].value = null;
                SequencerObjects[_cell.RowIndex + 2].data_points[_cell.ColumnIndex - FrozenColumnOffset].value = null;
            }

            return true;
        }

        private void trackEditor_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        //Cell click, insert values if track is BOOL
        private void trackEditor_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == -1)
                return;
            ResetRowAfterEdit = false;
            DataGridView dgv = (DataGridView)sender;
            //test if column header was clicked for global disable
            if (e.RowIndex == -1 && e.ColumnIndex == 0) {
                GlobalDisable = !GlobalDisable;
                foreach (Sequencer_Object seq in SequencerObjects) {
                    seq.enabled = !GlobalDisable;
                    RowReadOnly(GlobalDisable, seq);
                }
                //invalidate the column to repaint it, so images update
                trackEditor.InvalidateColumn(0);
                TCLE.PlaySound("UIselect");
            }
            //test if column header was clicked for global mute
            else if (e.RowIndex == -1 && e.ColumnIndex == 1) {
                GlobalMute = !GlobalMute;
                foreach (Sequencer_Object seq in SequencerObjects) {
                    seq.mute = GlobalMute;
                }
                //invalidate the column to repaint it, so images update
                trackEditor.InvalidateColumn(1);
                TCLE.PlaySound("UIselect");
            }
            //test if column header was clicked for global expand
            else if (e.RowIndex == -1 && e.ColumnIndex == 2) {
                //if ShowLanes, don't alter lane visibility
                if (Properties.Settings.Default.LeafOptionShowLane)
                    return;
                GlobalExpand = !GlobalExpand;
                foreach (Sequencer_Object seq in SequencerObjects) {
                    seq.expandlanes = GlobalExpand;
                }
                //invalidate the column to repaint it, so images update
                trackEditor.InvalidateColumn(2);
                TCLE.PlaySound("UIselect");
            }
            else if (e.RowIndex == -1) {
                if (e.Button == MouseButtons.Right) {
                    if (PlaybackEnd == e.ColumnIndex - FrozenColumnOffset) {
                        if (PlaybackLoop) {
                            PlaybackLoop = false;
                            PlaybackEnd = -1;
                            trackEditor.InvalidateColumn(e.ColumnIndex);
                            trackEditor.InvalidateColumn(e.ColumnIndex - 1);
                        }
                        else {
                            PlaybackLoop = true;
                            trackEditor.InvalidateColumn(e.ColumnIndex);
                            trackEditor.InvalidateColumn(e.ColumnIndex - 1);
                        }
                    }
                    else {
                        PlaybackEnd = e.ColumnIndex - FrozenColumnOffset;
                        if (PlaybackEnd <= PlaybackStart)
                            PlaybackEnd = PlaybackStart + 1;
                        trackEditor.Invalidate();
                    }
                }
                else {
                    if (PlaybackStart == e.ColumnIndex - FrozenColumnOffset) {
                        PlaybackStart = -1;
                        trackEditor.InvalidateColumn(e.ColumnIndex);
                        trackEditor.InvalidateColumn(e.ColumnIndex - 1);
                    }
                    else {
                        PlaybackStart = e.ColumnIndex - FrozenColumnOffset;
                        if (PlaybackEnd != -1 && PlaybackEnd <= PlaybackStart)
                            PlaybackEnd = PlaybackStart + 1;
                        trackEditor.Invalidate();
                    }
                }
                return;
            }
            //test for clicks in frozen columns 0 or 1
            //unselect the cells afterwards to imitate button click
            else if (e.ColumnIndex is 0 or 1 or 2) {
                Sequencer_Object seq = SequencerObjects[e.RowIndex];
                if (e.ColumnIndex is 0) {
                    seq.enabled = !seq.enabled;
                    RowReadOnly(!seq.enabled, seq);
                }
                if (e.ColumnIndex is 1) {
                    seq.mute = !seq.mute;
                }
                if (e.ColumnIndex is 2 && seq.friendly_lane == "lane center") {
                    //if ShowLanes, don't alter lane visibility
                    if (Properties.Settings.Default.LeafOptionShowLane)
                        return;
                    FindMissingLaneObjects(seq);
                    seq.expandlanes = !seq.expandlanes;
                    SequencerObjects[seq.editor_row.Index - 2].expandlanes = seq.expandlanes;
                    SequencerObjects[seq.editor_row.Index - 1].expandlanes = seq.expandlanes;
                    SequencerObjects[seq.editor_row.Index + 1].expandlanes = seq.expandlanes;
                    SequencerObjects[seq.editor_row.Index + 2].expandlanes = seq.expandlanes;
                }
                trackEditor[e.ColumnIndex, e.RowIndex].Selected = false;
                //invalidate cell to repaint it to update the images
                trackEditor.InvalidateCell(trackEditor[e.ColumnIndex, e.RowIndex]);
                TCLE.PlaySound("UIselect");
            }
            else if (e.Button == MouseButtons.Left && btnLeafAutoPlace.Checked) {
                if (SequencerObjects[e.RowIndex].trait_type is "kTraitBool" or "kTraitAction")
                    if (dgv[e.ColumnIndex, e.RowIndex].Value == null) {
                        dgv[e.ColumnIndex, e.RowIndex].Value = 1m;
                        CellValueChanged(e.RowIndex, e.ColumnIndex);
                    }
            }

            if (e.ColumnIndex >= FrozenColumnOffset) {
                //leafProperties.selecteddatapoint = SequencerObjects[e.RowIndex].data_points[e.ColumnIndex - FrozenColumnOffset];
                //propertyGridLeaf.Refresh();
            }
        }

        private void trackEditor_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;
            DataGridView dgv = sender as DataGridView;
            if (e.ColumnIndex < FrozenColumnOffset) {
                //do nothing
            }
            else if (e.Button == MouseButtons.Right) {
                RightclickDown = true;
                if (dgv[e.ColumnIndex, e.RowIndex].Selected == false) {
                    if (trackEditor[e.ColumnIndex, e.RowIndex].Value != null) {
                        LogUndo = false;
                        RightclickChanges = CellValueNull(trackEditor[e.ColumnIndex, e.RowIndex]);
                        LogUndo = true;
                    }
                }
                else if (dgv[e.ColumnIndex, e.RowIndex].Selected) {
                    if (dgv[e.ColumnIndex, e.RowIndex].Value == null && dgv.SelectedCells.Count == 1)
                        return;
                    CellValueChanged(e.RowIndex, e.ColumnIndex, true);
                }
                ShowRawTrackData(SequencerObjects[CurrentRow]);
            }
        }

        private void trackEditor_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) {
                RightclickDown = false;
                if (RightclickChanges) {
                    SaveCheckAndWrite(false, "Delete Cell Values via right-click");
                }
                RightclickChanges = false;
            }
        }

        private void trackEditor_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            MouseCurrentColumn = e.ColumnIndex;
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;

            DataGridView dgv = sender as DataGridView;
            if (e.ColumnIndex is 0 or 1 or 2) {
                dgv[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.FromArgb(174, 161, 255);
            }
            else if (Control.MouseButtons == MouseButtons.Right) {
                RightclickDown = true;
                if (dgv[e.ColumnIndex, e.RowIndex].Selected == false) {
                    if (trackEditor[e.ColumnIndex, e.RowIndex].Value != null) {
                        LogUndo = false;
                        RightclickChanges = CellValueNull(trackEditor[e.ColumnIndex, e.RowIndex]);
                        LogUndo = true;
                    }
                }
                else if (dgv[e.ColumnIndex, e.RowIndex].Selected == true) {
                    dgv[e.ColumnIndex, e.RowIndex].Value = null;
                    CellValueChanged(e.RowIndex, e.ColumnIndex, true);
                }
                //ShowRawTrackData(SequencerObjects[CurrentRow]);
            }
        }

        private int LastRow;
        private int LastCol;
        private void trackEditor_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (Playback.IsPlaying)
                return;
            if (e.ColumnIndex < 3 || e.RowIndex == -1)
                return;
            if (SequencerObjects[e.RowIndex].category != "PLAY SAMPLE")
                return;
            Color color = trackEditor[e.ColumnIndex, e.RowIndex].Style.BackColor;
            trackEditor[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.FromArgb(174, 161, 255);
            trackEditor[e.ColumnIndex, e.RowIndex].Style.BackColor = color;
            LastRow = e.RowIndex;
            LastCol = e.ColumnIndex;
        }

        private void trackEditor_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;

            DataGridView dgv = sender as DataGridView;
            if (e.ColumnIndex is 0 or 1 or 2) {
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
                SaveCheckAndWrite(false, "Delete Cell Values");
            }
        }
        private void trackEditor_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //delete cell value if Delete key is pressed
            if (e.KeyCode == Keys.Delete) {
                LogUndo = false;
                CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex, true);
                LogUndo = true;
                SaveCheckAndWrite(false, "Delete Cell Values");
            }
            else if (e.Control) {
                if (e.KeyCode == Keys.OemSemicolon)
                    txtSearch.Focus();
            }

            else if (e.Alt) {
                if (e.KeyCode is Keys.Right or Keys.Left or Keys.Up or Keys.Down) {
                    e.Handled = true;
                    //this is used for indexing if shifting left/down or right/up
                    int indexdirection = e.KeyCode is Keys.Right or Keys.Down ? 1 : -1;
                    bool leftright = e.KeyCode is Keys.Left or Keys.Right;
                    bool shifted = false;
                    //sort cells in selection based on column. depends on direction, reverse collection.
                    //this processing order is important so cells dont overwrite each other when moving
                    IOrderedEnumerable<DataGridViewCell> dgvcc;
                    if (indexdirection == -1)
                        dgvcc = trackEditor.SelectedCells.Cast<DataGridViewCell>().OrderBy(c => leftright ? c.ColumnIndex : c.RowIndex);
                    else
                        dgvcc = trackEditor.SelectedCells.Cast<DataGridViewCell>().OrderByDescending(c => leftright ? c.ColumnIndex : c.RowIndex);

                    trackEditor.ClearSelection();
                    //iterate over each in the selection
                    foreach (DataGridViewCell dgvc in dgvcc) {
                        //check if at left/right edges
                        if ((leftright && dgvc.ColumnIndex + indexdirection < trackEditor.ColumnCount && dgvc.ColumnIndex + indexdirection > -1) || (!leftright && dgvc.RowIndex + indexdirection < trackEditor.RowCount && dgvc.RowIndex + indexdirection > -1)) {
                            shifted = true;
                            trackEditor[dgvc.ColumnIndex + (leftright ? indexdirection : 0), dgvc.RowIndex + (!leftright ? indexdirection : 0)].Value = dgvc.Value;
                            //select the newly moved cell
                            trackEditor[dgvc.ColumnIndex + (leftright ? indexdirection : 0), dgvc.RowIndex + (!leftright ? indexdirection : 0)].Selected = true;
                            ///TrackUpdateHighlightingSingleCell(trackEditor[dgvc.ColumnIndex + (leftright ? indexdirection : 0), dgvc.RowIndex + (!leftright ? indexdirection : 0)], SequencerObjects[dgvc.RowIndex + (!leftright ? indexdirection : 0)]);
                            //clear the current cell since it moved
                            dgvc.Value = null;
                            ///TrackUpdateHighlightingSingleCell(dgvc, SequencerObjects[dgvc.RowIndex]);
                        }
                        else {
                            foreach (DataGridViewCell dgvcell in dgvcc)
                                dgvcell.Selected = true;
                            break;
                        }
                    }
                    if (shifted)
                        SaveCheckAndWrite(false, "Shift Cell Values");
                    //SaveCheckAndWrite(false, $"Shifted selected cells {(e.KeyCode == Keys.Left ? "left" : "right")}", $"");
                }
            }

            if (e.KeyData == TCLE.Keybinds["Leaf Playback"]) {
                btnTrackPlayback.PerformClick();
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 0"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue0;
                CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 1"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue1;
                CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 2"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue2;
                CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 3"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue3;
                CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 4"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue4;
                CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 5"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue5;
                CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 6"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue6;
                CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 7"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue7;
                CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 8"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue8;
                CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 9"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue9;
                CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex);
            }
        }

        private void trackEditor_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (ModifierKeys is not Keys.Control and not Keys.Shift && ZoomHasChanged) {
                ZoomHasChanged = false;
                foreach (Sequencer_Object seq in SequencerObjects)
                    seq.WaveBitmap = null;
            }
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
            trackEditor.CurrentCell = trackEditor.Rows[e.RowIndex].Cells[FrozenColumnOffset];
            trackEditor.Invalidate();
        }

        private void trackEditor_RowHeadersWidthChanged(object sender, EventArgs e)
        {
            int width = trackEditor.RowHeadersWidth;
            trackEditor_Resize(null, null);
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

        private void treeObjects_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Nodes.Count > 0 || treeObjects.SelectedNode.Nodes.Count > 0)
                return;
            Object_Params objmatch = TCLE.LeafObjects.FirstOrDefault(x => x.param_displayname == e.Node.Text);
            if (e.Node.Text.EndsWith(".samp"))
                objmatch = TCLE.LeafObjects.FirstOrDefault(x => x.category == "PLAY SAMPLE");

            if (objmatch == null)
                return;


            Sequencer_Object seq = new(leafProperties) {
                obj_name = objmatch.category == "PLAY SAMPLE" ? e.Node.Text : objmatch.obj_name,
                category = objmatch.category,
                param_path = objmatch.param_path.Split('.')[0],
                friendly_param = objmatch.param_displayname,
                defaultvalue = float.Parse(objmatch.def),
                step = objmatch.step,
                trait_type = objmatch.trait_type,
                highlight_color = objmatch.defaultcolor,
                highlight_value = 0,
                footer = objmatch.footer,
                enabled = true,
                param_path_lane = objmatch.param_path.EndsWith(".ent") ? "ent" : "none",
                friendly_lane = objmatch.param_path.EndsWith(".ent") ? "lane center" : "none",
                editor_row = new DataGridViewRow()
            };
            if (seq.obj_name == "leafname")
                seq.obj_name = LeafProperties.FilePath.Name;
            if (seq.category == "AUDIO") {
                int audiochannels = SequencerObjects.Count(x => x.category == "AUDIO");
                seq.param_path = seq.param_path.Replace("x", $"{audiochannels}");
                seq.friendly_param = seq.friendly_param.Replace("x", $"{audiochannels}");
            }
            seq.expandlanes = seq.friendly_lane == "none" || Properties.Settings.Default.LeafOptionShowLane;
            SequencerObjects.Add(seq);
            trackEditor.Rows.Add(seq.editor_row);
            ChangeTrackName(seq, Properties.Settings.Default.LeafOptionShowCategory ? $"[{seq.category}] " : "");
            TrackUpdateHighlighting(seq);
            FindMissingLaneObjects(seq);
            SaveCheckAndWrite(false, "Add Object");
            TCLE.PlaySound("UIobjectadd");
        }

        private void treeObjects_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode currentNode = treeObjects.GetNodeAt(e.Location);
            if (currentNode == null) return;

            if (e.Button == MouseButtons.Right)
                treeObjects.SelectedNode = currentNode;
        }

        int sampchannel;
        float initialfreq;
        SampleData SamplePlaying = new();
        private void treeObjects_KeyDown(object sender, KeyEventArgs e)
        {
            if (treeObjects.SelectedNode.Parent.Text.EndsWith(".samp") && e.KeyCode == Keys.Space) {
                SampleData SampToPlay = TCLE.ProjectSamples.FirstOrDefault(x => x.obj_name == treeObjects.SelectedNode.Text);
                if (SampToPlay == null || SamplePlaying == SampToPlay)
                    return;

                if (SampToPlay.TempFile == null) {
                    string SampleToPlay = TCLE.PCtoAudioFile(SampToPlay);
                    if (String.IsNullOrEmpty(SampleToPlay))
                        return;
                }

                //initialize the player and load the sample
                sampchannel = Bass.BASS_StreamCreateFile($@"{SampToPlay.TempFile}", 0, 0, BASSFlag.BASS_SAMPLE_FLOAT);
                //pitch shift and pan
                Bass.BASS_ChannelGetAttribute(sampchannel, BASSAttribute.BASS_ATTRIB_FREQ, ref initialfreq);
                Bass.BASS_ChannelSetAttribute(sampchannel, BASSAttribute.BASS_ATTRIB_FREQ, initialfreq * (float)SampToPlay.pitch);
                Bass.BASS_ChannelSetAttribute(sampchannel, BASSAttribute.BASS_ATTRIB_PAN, (float)SampToPlay.pan);
                Bass.BASS_ChannelSetPosition(sampchannel, (double)SampToPlay.offset / 1000d);
                //play the sample
                if (sampchannel != 0 && Bass.BASS_ChannelPlay(sampchannel, false)) {
                    SamplePlaying = SampToPlay;
                    treeObjects.SelectedNode.ImageKey = "play";
                    treeObjects.SelectedNode.SelectedImageKey = "play";
                }
            }
        }

        private void treeObjects_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space) {
                Bass.BASS_ChannelStop(sampchannel);
                Bass.BASS_ChannelFree(sampchannel);
                SamplePlaying = null;
                treeObjects.SelectedNode.ImageKey = "none";
                treeObjects.SelectedNode.SelectedImageKey = "none";
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            BuildObjectTree();
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search Objects (Ctrl+;)") {
                txtSearch.TextChanged -= txtSearch_TextChanged;
                txtSearch.Text = "";
                txtSearch.TextChanged += txtSearch_TextChanged;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text)) {
                txtSearch.TextChanged -= txtSearch_TextChanged;
                txtSearch.Text = "Search Objects (Ctrl+;)";
                txtSearch.TextChanged += txtSearch_TextChanged;
            }
        }

        private void toolStripFavAdd_Click(object sender, EventArgs e)
        {
            if (treeObjects.SelectedNode.ImageKey != "none")
                return;
            Object_Params match = TCLE.LeafObjects.FirstOrDefault(x => x.param_displayname == treeObjects.SelectedNode.Text && x.category.ToUpper() == treeObjects.SelectedNode.Parent.Text);
            if (match != null && !TCLE.ObjectFavorites.Contains(match))
                TCLE.ObjectFavorites.Add(match);
            treeObjects.SelectedNode.ImageKey = "fav";
            treeObjects.SelectedNode.SelectedImageKey = "fav";
            treeObjects.SelectedNode.ContextMenuStrip = contextMenuFavRemove;
            BuildTreeFavorites();
            TCLE.PlaySound("UIselect");
        }

        private void toolStripFavRemove_Click(object sender, EventArgs e)
        {
            string find = treeObjects.SelectedNode.Text;
            if (treeObjects.SelectedNode.ImageKey == "fav") {
                TCLE.ObjectFavorites.RemoveWhere(x => x.param_displayname == find);
                treeObjects.SelectedNode.SelectedImageKey = "none";
                treeObjects.SelectedNode.ImageKey = "none";
                treeObjects.SelectedNode.ContextMenuStrip = contextMenuFav;
                BuildTreeFavorites();
            }
            else {
                TCLE.ObjectFavorites.RemoveWhere(x => x.param_displayname == find);
                treeObjects.SelectedNode.Remove();
                TreeNode node = FindNode(find, treeObjects.Nodes);
                if (node != null) {
                    node.SelectedImageKey = "none";
                    node.ImageKey = "none";
                    node.ContextMenuStrip = contextMenuFav;
                }
            }
            TCLE.PlaySound("UIselect");
        }

        private void toolStripFavClear_Click(object sender, EventArgs e)
        {
            TCLE.ObjectFavorites.Clear();
            TCLE.PlaySound("UIdelete");
            BuildObjectTree();
        }
        #endregion

        #region Buttons
        ///         ///
        /// BUTTONS ///
        ///         ///
        private void btnTrackAdd_Click(object sender, EventArgs e)
        {
            if (treeObjects.SelectedNode.Nodes.Count > 0)
                return;
            Object_Params objmatch = TCLE.LeafObjects.FirstOrDefault(x => x.param_displayname == treeObjects.SelectedNode.Text);
            if (objmatch == null)
                return;
            Sequencer_Object _currentseq = SequencerObjects[CurrentRow];
            if (!objmatch.param_path.EndsWith(".ent") && _currentseq.friendly_lane != "none") {
                MessageBox.Show("Due to reasons, you cannot change a multi-lane object into a non-multi-lane object. Please just add a new object.", "Thumper Custom Level Editor");
                return;
            }
            Sequencer_Object[] Lanes = SequencerObjects.Where(x => x.category == _currentseq.category && x.friendly_param == _currentseq.friendly_param).ToArray();
            for (int x = 0; x < Lanes.Length; x++) {
                Lanes[x].obj_name = objmatch.obj_name == "PLAY SAMPLE" ? treeObjects.SelectedNode.Text : objmatch.obj_name;
                Lanes[x].category = objmatch.category;
                Lanes[x].param_path = objmatch.param_path.Split('.')[0];
                Lanes[x].friendly_param = objmatch.param_displayname;
                Lanes[x].trait_type = objmatch.trait_type;
                Lanes[x].footer = objmatch.footer;
                //if the new object is not multilane, change each lane to "none"
                if (!objmatch.param_path.EndsWith(".ent")) {
                    Lanes[x].param_path_lane = "none";
                    Lanes[x].friendly_lane = "none";
                }
                else if (Lanes[x].friendly_lane == "none") {
                    Lanes[x].param_path_lane = "ent";
                    Lanes[x].friendly_lane = "lane center";
                    Lanes[x].expandlanes = Properties.Settings.Default.LeafOptionShowLane;
                }
                if (Lanes[x].obj_name == "leafname")
                    Lanes[x].obj_name = LeafProperties.FilePath.Name;
                ChangeTrackName(Lanes[x], Properties.Settings.Default.LeafOptionShowCategory ? $"[{Lanes[x].category}] " : "");
                TrackUpdateHighlighting(Lanes[x]);
            }
            FindMissingLaneObjects(SequencerObjects[CurrentRow]);
            trackEditor.InvalidateRow(_currentseq.editor_row.Index);

            SaveCheckAndWrite(false, "Add Object");
            TCLE.PlaySound("UIobjectadd");
        }

        private void btnTrackDelete_Click(object sender, EventArgs e)
        {
            if (CurrentRow < 0)
                return;
            trackEditor.SuspendLayout();
            //If multiple rows are selected, get all of them in a list. Then loop over list, deleting each one
            List<Sequencer_Object> selectedrows = trackEditor.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().Select(x => SequencerObjects[x.Index]).ToList();
            if (MessageBox.Show($"{selectedrows.Count} Sequencer objects selected.\nAre you sure you want to delete them?", "Confirm?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            while (selectedrows.Count > 0) {
                //if object is multilane, delete its other lanes too
                Sequencer_Object[] Lanes = ReturnLanesFromName(selectedrows[0], selectedrows[0].friendly_lane);
                for (int x = 0; x < Lanes.Length; x++) {
                    trackEditor.Rows.Remove(Lanes[x].editor_row);
                    SequencerObjects.Remove(Lanes[x]);
                    //this is especially useful regarding multilanes. If the multilanes were selected in selectedrows, they'll be deleted before the next loop.
                    //by removing them from selectedrows, then the for loop objindex wont index to them
                    selectedrows.Remove(Lanes[x]);
                }
            }
            trackEditor.ResumeLayout();
            trackEditor.Invalidate();
            SaveCheckAndWrite(false, "Delete Object");
            TCLE.PlaySound("UIobjectremove");

            //disable elements if there are no tracks
            EnableLeafButtons();
        }

        private void btnTrackUp_Click(object sender, EventArgs e)
        {
            //Get each sequencer object that is the same row index as the selected cells
            //multiple cells may be selected in the same row, so we Distinct() it
            //Then SelectManay to find the matching lanes for multilane objects (this will get 5 or 1 objects depending if multilane or not)
            //now we have a well ordered list of objects to move
            IEnumerable<Sequencer_Object> selectedrows = trackEditor.SelectedCells.Cast<DataGridViewCell>()
                .Select(cell => SequencerObjects[cell.RowIndex])
                .Distinct()
                .OrderBy(cell => cell.editor_row.Index);
            List<Sequencer_Object> RowsToMove = new();
            foreach (Sequencer_Object row in selectedrows) {
                if (!RowsToMove.Contains(row))
                    RowsToMove.AddRange(ReturnLanesFromName(row, row.friendly_lane));
            }
            //if already at the top, do not move up
            if (RowsToMove.FirstOrDefault().editor_row.Index == 0)
                return;

            IEnumerable<DataGridViewCell> selectedcells = trackEditor.SelectedCells.Cast<DataGridViewCell>();

            for (int x = 0; x < RowsToMove.Count; x++) {
                int currentindex = RowsToMove[x].editor_row.Index;
                //get the object above, and any lanes with it. We will need to move above all of them.
                Sequencer_Object ObjAbove = SequencerObjects[RowsToMove[x].editor_row.Index - 1];
                int Lanes = ObjAbove.friendly_lane != "none" ? 5 : 1;
                //remove the row and object
                trackEditor.SuspendLayout();
                trackEditor.Rows.Remove(RowsToMove[x].editor_row);
                SequencerObjects.Remove(RowsToMove[x]);
                if (RowsToMove[x].friendly_lane != "none") {
                    trackEditor.Rows.Remove(RowsToMove[x + 1].editor_row);
                    SequencerObjects.Remove(RowsToMove[x + 1]);
                    trackEditor.Rows.Remove(RowsToMove[x + 2].editor_row);
                    SequencerObjects.Remove(RowsToMove[x + 2]);
                    trackEditor.Rows.Remove(RowsToMove[x + 3].editor_row);
                    SequencerObjects.Remove(RowsToMove[x + 3]);
                    trackEditor.Rows.Remove(RowsToMove[x + 4].editor_row);
                    SequencerObjects.Remove(RowsToMove[x + 4]);
                }
                //reinsert object and row at appropriate index
                SequencerObjects.Insert(currentindex - Lanes, RowsToMove[x]);
                trackEditor.Rows.Insert(currentindex - Lanes, RowsToMove[x].editor_row);
                if (RowsToMove[x].friendly_lane != "none") {
                    SequencerObjects.Insert(currentindex - Lanes + 1, RowsToMove[x + 1]);
                    trackEditor.Rows.Insert(currentindex - Lanes + 1, RowsToMove[x + 1].editor_row);
                    SequencerObjects.Insert(currentindex - Lanes + 2, RowsToMove[x + 2]);
                    trackEditor.Rows.Insert(currentindex - Lanes + 2, RowsToMove[x + 2].editor_row);
                    SequencerObjects.Insert(currentindex - Lanes + 3, RowsToMove[x + 3]);
                    trackEditor.Rows.Insert(currentindex - Lanes + 3, RowsToMove[x + 3].editor_row);
                    SequencerObjects.Insert(currentindex - Lanes + 4, RowsToMove[x + 4]);
                    trackEditor.Rows.Insert(currentindex - Lanes + 4, RowsToMove[x + 4].editor_row);
                    x += 4;
                }
            }

            trackEditor.ClearSelection();
            foreach (DataGridViewCell dgvc in selectedcells) {
                trackEditor[dgvc.ColumnIndex, dgvc.RowIndex].Selected = true;
            }
            trackEditor.ResumeLayout();

            SaveCheckAndWrite(false, "Move Object(s) Up");
        }

        private Sequencer_Object[] ReturnLanesFromName(Sequencer_Object row, string lane)
        {
            Sequencer_Object[] Lanes = new Sequencer_Object[5];
            switch (lane) {
                case "lane left 2":
                    Lanes[0] = row;
                    Lanes[1] = SequencerObjects[row.editor_row.Index + 1];
                    Lanes[2] = SequencerObjects[row.editor_row.Index + 2];
                    Lanes[3] = SequencerObjects[row.editor_row.Index + 3];
                    Lanes[4] = SequencerObjects[row.editor_row.Index + 4];
                    break;
                case "lane left 1":
                    Lanes[0] = SequencerObjects[row.editor_row.Index - 1];
                    Lanes[1] = row;
                    Lanes[2] = SequencerObjects[row.editor_row.Index + 1];
                    Lanes[3] = SequencerObjects[row.editor_row.Index + 2];
                    Lanes[4] = SequencerObjects[row.editor_row.Index + 3];
                    break;
                case "lane center":
                    Lanes[0] = SequencerObjects[row.editor_row.Index - 2];
                    Lanes[1] = SequencerObjects[row.editor_row.Index - 1];
                    Lanes[2] = row;
                    Lanes[3] = SequencerObjects[row.editor_row.Index + 1];
                    Lanes[4] = SequencerObjects[row.editor_row.Index + 2];
                    break;
                case "lane right 1":
                    Lanes[0] = SequencerObjects[row.editor_row.Index - 3];
                    Lanes[1] = SequencerObjects[row.editor_row.Index - 2];
                    Lanes[2] = SequencerObjects[row.editor_row.Index - 1];
                    Lanes[3] = row;
                    Lanes[4] = SequencerObjects[row.editor_row.Index + 1];
                    break;
                case "lane right 2":
                    Lanes[0] = SequencerObjects[row.editor_row.Index - 4];
                    Lanes[1] = SequencerObjects[row.editor_row.Index - 3];
                    Lanes[2] = SequencerObjects[row.editor_row.Index - 2];
                    Lanes[3] = SequencerObjects[row.editor_row.Index - 1];
                    Lanes[4] = row;
                    break;
                case "none":
                    Lanes = new Sequencer_Object[1] { row };
                    break;
            }
            return Lanes;
        }

        private void btnTrackDown_Click(object sender, EventArgs e)
        {
            //Get each sequencer object that is the same row index as the selected cells
            //multiple cells may be selected in the same row, so we Distinct() it
            //Then SelectManay to find the matching lanes for multilane objects (this will get 5 or 1 objects depending if multilane or not)
            //now we have a well ordered list of objects to move
            IEnumerable<Sequencer_Object> selectedrows = trackEditor.SelectedCells.Cast<DataGridViewCell>()
                .Select(cell => SequencerObjects[cell.RowIndex])
                .Distinct();
            List<Sequencer_Object> RowsToMove = new();
            foreach (Sequencer_Object row in selectedrows) {
                if (!RowsToMove.Contains(row))
                    RowsToMove.AddRange(ReturnLanesFromName(row, row.friendly_lane));
            }
            ///RowsToMove = RowsToMove.OrderByDescending(cell => cell.editor_row.Index).ToList();
            //if already at the bottom, do not move down
            if (RowsToMove.Last().editor_row.Index >= trackEditor.Rows.Count - 1)
                return;

            List<DataGridViewCell> selectedcells = trackEditor.SelectedCells.Cast<DataGridViewCell>().ToList();

            for (int x = 0; x < RowsToMove.Count; x++) {
                int currentindex = RowsToMove[x].editor_row.Index;
                //get the object above, and any lanes with it. We will need to move above all of them.
                Sequencer_Object ObjBelow = SequencerObjects[RowsToMove[^1].editor_row.Index + 1];
                int Lanes = ObjBelow.friendly_lane != "none" ? 5 : 1;
                //remove the row and object
                trackEditor.Rows.Remove(RowsToMove[x].editor_row);
                SequencerObjects.Remove(RowsToMove[x]);
                if (RowsToMove[x].friendly_lane != "none") {
                    trackEditor.Rows.Remove(RowsToMove[x + 1].editor_row);
                    SequencerObjects.Remove(RowsToMove[x + 1]);
                    trackEditor.Rows.Remove(RowsToMove[x + 2].editor_row);
                    SequencerObjects.Remove(RowsToMove[x + 2]);
                    trackEditor.Rows.Remove(RowsToMove[x + 3].editor_row);
                    SequencerObjects.Remove(RowsToMove[x + 3]);
                    trackEditor.Rows.Remove(RowsToMove[x + 4].editor_row);
                    SequencerObjects.Remove(RowsToMove[x + 4]);
                }
                //reinsert object and row at appropriate index
                int objbelowindex = ObjBelow.editor_row.Index;
                SequencerObjects.Insert(objbelowindex + Lanes, RowsToMove[x]);
                trackEditor.Rows.Insert(objbelowindex + Lanes, RowsToMove[x].editor_row);
                if (RowsToMove[x].friendly_lane != "none") {
                    SequencerObjects.Insert(objbelowindex + Lanes + 1, RowsToMove[x + 1]);
                    trackEditor.Rows.Insert(objbelowindex + Lanes + 1, RowsToMove[x + 1].editor_row);
                    SequencerObjects.Insert(objbelowindex + Lanes + 2, RowsToMove[x + 2]);
                    trackEditor.Rows.Insert(objbelowindex + Lanes + 2, RowsToMove[x + 2].editor_row);
                    SequencerObjects.Insert(objbelowindex + Lanes + 3, RowsToMove[x + 3]);
                    trackEditor.Rows.Insert(objbelowindex + Lanes + 3, RowsToMove[x + 3].editor_row);
                    SequencerObjects.Insert(objbelowindex + Lanes + 4, RowsToMove[x + 4]);
                    trackEditor.Rows.Insert(objbelowindex + Lanes + 4, RowsToMove[x + 4].editor_row);
                    x += 4;
                }
            }

            trackEditor.ClearSelection();
            foreach (DataGridViewCell dgvc in selectedcells) {
                trackEditor[dgvc.ColumnIndex, dgvc.RowIndex].Selected = true;
            }

            SaveCheckAndWrite(false, "Move Object(s) Down");
        }

        private void btnTrackCopy_Click(object sender, EventArgs e)
        {
            IEnumerable<Sequencer_Object> Copied = trackEditor.SelectedCells.Cast<DataGridViewCell>()
                .Select(cell => SequencerObjects[cell.RowIndex])
                .Distinct()
                .OrderBy(cell => cell.editor_row.Index).Select(x => x.Clone());

            TCLE.ClipboardSequencer = new();
            foreach (Sequencer_Object copyseq in Copied) {
                if (copyseq.friendly_lane is not "none") {
                    Sequencer_Object lookup = TCLE.ClipboardSequencer.FirstOrDefault(x => x.obj_name == copyseq.obj_name && x.param_path == copyseq.param_path && x.param_path_lane == copyseq.param_path_lane && x.isdefault == true);
                    //if null, no object exists in SequencerObjects yet for this object or its lanes. We'll have to make it.
                    if (lookup == null) {
                        TCLE.ClipboardSequencer.Add(copyseq.CloneAsDefault("a01", "lane left 2", new DataGridViewRow()));
                        TCLE.ClipboardSequencer.Add(copyseq.CloneAsDefault("a02", "lane left 1", new DataGridViewRow()));
                        TCLE.ClipboardSequencer.Add(copyseq.CloneAsDefault("ent", "lane center", new DataGridViewRow()));
                        TCLE.ClipboardSequencer.Add(copyseq.CloneAsDefault("z01", "lane right 1", new DataGridViewRow()));
                        TCLE.ClipboardSequencer.Add(copyseq.CloneAsDefault("z02", "lane right 2", new DataGridViewRow()));
                    }
                    lookup = TCLE.ClipboardSequencer.FirstOrDefault(x => x.obj_name == copyseq.obj_name && x.param_path == copyseq.param_path && x.param_path_lane == copyseq.param_path_lane && x.isdefault == true);
                    int index = TCLE.ClipboardSequencer.IndexOf(lookup);
                    TCLE.ClipboardSequencer[index] = copyseq;
                }
                //else just add the object without needing extra lanes
                else {
                    TCLE.ClipboardSequencer.Add(copyseq);
                }
            }

            foreach (Form_LeafEditor leaf in TCLE.Documents.Where(x => x.DockHandler.TabText.Contains(".leaf")))
                leaf.btnTrackPaste.Enabled = true;
            TCLE.PlaySound("UIkcopy");
        }

        private void btnTrackPaste_Click(object sender, EventArgs e)
        {
            try {
                int _index = trackEditor.CurrentRow?.Index ?? -1;
                //check if copied row is longer than the leaf beat length
                int lastbeat = TCLE.ClipboardSequencer.First().editor_row.Cells.Count - FrozenColumnOffset;
                if (lastbeat > LeafProperties.beats) {
                    DialogResult _paste = MessageBox.Show("Copied track is longer than this leaf's beat count. Do you want to extend this leaf's beat count?\nYES = extend leaf and paste\nNO = paste, do not extend leaf\nCANCEL = do not paste", "repmuhT motsuC leveL rotidE", MessageBoxButtons.YesNoCancel);
                    //YES = extend the leaf and then paste
                    if (_paste == DialogResult.Yes)
                        LeafProperties.beats = lastbeat;
                    //NO = do not extend leaf and then paste
                    //CANCEL = do nothing
                    else if (_paste == DialogResult.Cancel)
                        return;
                }
                ispasting = true;
                //add copied Sequencer_Object to main _tracks list
                foreach (Sequencer_Object _newtrack in TCLE.ClipboardSequencer) {
                    _index++;
                    DataGridViewRow dgvr = new();
                    Sequencer_Object clone = _newtrack.Clone();
                    clone.editor_row = dgvr;
                    clone.expandlanes = GlobalExpand;
                    //need to remove beats beyond the beat count
                    for (int x = LeafProperties.beats; x < 255; x++) {
                        clone.data_points[x] = new() { Beat = x, interpolation = "Linear", ease = "Ease In Out" };
                    }
                    SequencerObjects.Insert(_index, clone);
                    trackEditor.Rows.Insert(_index, clone.editor_row);
                    try {
                        //set the headercell names
                        ///ChangeTrackName(clone, Properties.Settings.Default.LeafOptionShowCategory ? $"[{clone.category}] " : "");
                        //pass _griddata per row to be imported to the DGV
                        TrackRawImport(clone, _newtrack.data_points, LeafProperties);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("something went wrong with pasting. Show this error to the dev.\n\n" + ex);
            }

            ispasting = false;
            TCLE.PlaySound("UIkpaste");
            SaveCheckAndWrite(false, "Paste Objects");
            //SaveCheckAndWrite(false, "Pasted tracks", "");
        }

        private void btnTrackClear_Click(object sender, EventArgs e)
        {
            //finds each distinct row across all selected cells
            List<Sequencer_Object> selectedrows = trackEditor.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().Where(row => row.Visible).Select(x => SequencerObjects[x.Index]).ToList();
            if (MessageBox.Show($"{selectedrows.Count} Sequencer objects selected.\nAre you sure you want to clear them?", "Confirm?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            LogUndo = false;

            foreach (Sequencer_Object seq in selectedrows) {
                //check selected row
                if (seq.friendly_lane is not "lane center" || seq.expandlanes) {
                    seq.ClearDataPoints();
                }
                else {
                    int index = SequencerObjects.IndexOf(seq);
                    SequencerObjects[index - 2].ClearDataPoints();// = new() { value = null, Beat = x, interpolation = "Linear", ease = "Ease In Out" };
                    SequencerObjects[index - 1].ClearDataPoints();// = new() { value = null, Beat = x, interpolation = "Linear", ease = "Ease In Out" };
                    seq.ClearDataPoints();// = new() { value = null, Beat = x, interpolation = "Linear", ease = "Ease In Out" };
                    SequencerObjects[index + 1].ClearDataPoints();// = new() { value = null, Beat = x, interpolation = "Linear", ease = "Ease In Out" };
                    SequencerObjects[index + 2].ClearDataPoints();// = new() { value = null, Beat = x, interpolation = "Linear", ease = "Ease In Out" };
                }
                trackEditor.InvalidateRow(seq.editor_row.Index);
            }

            LogUndo = true;
            TCLE.PlaySound("UIdataerase");
            SaveCheckAndWrite(false, "Clear Object Values");
        }

        private void btnRawImport_Click(object sender, EventArgs e)
        {
            if (loadedleaf == null)
                return;
            try {
                TrackRawImport(SequencerObjects[CurrentRow], JObject.Parse($"{{{textEditor.Text}}}"));
                TrackUpdateHighlighting(SequencerObjects[CurrentRow]);
                TCLE.PlaySound("UIkpaste");
            }
            catch (JsonReaderException ex) {
                MessageBox.Show($"Invalid format or characters in imported data. Please fix.\n\n{ex.Message}", "Thumper Custom Editor Level");
            }
        }

        private string InterpLastUsed;
        private void btnLeafInterpLinear_ButtonClick(object sender, EventArgs e)
        {
            Interpolate(InterpLastUsed);
        }

        private void contextMenuInterps_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Examples (web link)")
                return;
            Interpolate(e.ClickedItem.Text);
        }

        private void Interpolate(string interpOption)
        {
            InterpLastUsed = interpOption;
            DataGridViewSelectedCellCollection SelectedCells = trackEditor.SelectedCells;
            //interpolation requires 2 cells only
            if (SelectedCells.Count != 2) {
                MessageBox.Show("Interpolation works with only 2 cells selected", "Interpolation error");
                return;
            }
            //check if cells are in the same row
            if (SelectedCells[0].RowIndex != SelectedCells[1].RowIndex) {
                MessageBox.Show("Interpolation works only if selected cells are in the same row", "Interpolation error");
                return;
            }
            //sort cells so they are in order according to column index
            List<DataGridViewCell> InterpCells = new() { SelectedCells[0], SelectedCells[1] };
            InterpCells.Sort((cell1, cell2) => cell1.ColumnIndex.CompareTo(cell2.ColumnIndex));
            Sequencer_Object interpobject = SequencerObjects[SelectedCells[0].RowIndex];

            //get start and end values, and how many beats separate them
            double _start = (double)((decimal?)InterpCells[0].Value ?? (decimal)interpobject.defaultvalue);
            double _end = (double)((decimal?)InterpCells[1].Value ?? (decimal)interpobject.defaultvalue);
            double max = Math.Max(_start, _end);
            double min = Math.Min(_start, _end);
            int _beats = InterpCells[1].ColumnIndex - InterpCells[0].ColumnIndex + 1;
            //initialize array = to beats, fill with linear values between 0 and 1
            //these will be transformed by the formulas below
            double[] interp = new double[_beats];
            for (int x = 0; x < interp.Length; x++) {
                interp[x] = (double)(x) / (double)(interp.Length - 1);
            }

            //depending on interp option chosen, run a different calculation per value in interp[]
            switch (interpOption) {
                case "Linear":
                    //no changes needed
                    break;
                case "Quadratic Ease In":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = interp[x] * interp[x];
                    }
                    break;
                case "Quadratic Ease Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = 1 - (1 - interp[x]) * (1 - interp[x]);
                    }
                    break;
                case "Quadratic Ease In Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = interp[x] < 0.5 ? (2 * interp[x] * interp[x]) : (1 - (Math.Pow(-2 * interp[x] + 2, 2) / 2));
                    }
                    break;
                case "Cubic Ease In":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = interp[x] * interp[x] * interp[x];
                    }
                    break;
                case "Cubic Ease Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = 1 - Math.Pow(1 - interp[x], 3);
                    }
                    break;
                case "Cubic Ease In Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = interp[x] < 0.5 ? (4 * interp[x] * interp[x] * interp[x]) : (1 - (Math.Pow(-2 * interp[x] + 2, 3) / 2));
                    }
                    break;
                case "Quartic Ease In":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = interp[x] * interp[x] * interp[x] * interp[x];
                    }
                    break;
                case "Quartic Ease Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = 1 - Math.Pow(1 - interp[x], 4);
                    }
                    break;
                case "Quartic Ease In Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = interp[x] < 0.5 ? (8 * interp[x] * interp[x] * interp[x] * interp[x]) : (1 - (Math.Pow(-2 * interp[x] + 2, 4) / 2));
                    }
                    break;
                case "Quintic Ease In":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = interp[x] * interp[x] * interp[x] * interp[x] * interp[x];
                    }
                    break;
                case "Quintic Ease Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = 1 - Math.Pow(1 - interp[x], 5);
                    }
                    break;
                case "Quintic Ease In Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = interp[x] < 0.5 ? (16 * interp[x] * interp[x] * interp[x] * interp[x]) : (1 - (Math.Pow(-2 * interp[x] + 2, 5) / 2));
                    }
                    break;
                case "Sine Ease In":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = 1 - Math.Cos((interp[x] * Math.PI) / 2);
                    }
                    break;
                case "Sine Ease Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = Math.Sin((interp[x] * Math.PI) / 2);
                    }
                    break;
                case "Sine Ease In Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = -(Math.Cos(Math.PI * interp[x]) - 1) / 2;
                    }
                    break;
            }

            //if the first cell is actually the maximum, each value needs to be flipped across the range 0 to 1
            if (_start == max) {
                for (int x = 0; x < interp.Length; x++)
                    interp[x] = 1 - interp[x];
            }

            //convert interp[] range of 0 to 1 into range between selected beats
            for (int x = 0; x < interp.Length; x++) {
                interp[x] = ((interp[x] - 0) / (1 - 0)) * (max - min) + min;
            }
            //assign new values back to the data points
            IsInterpolating = true;
            for (int x = 0; x < _beats; x++) {
                //if interpolating for Color, remove the decimals
                if (interpobject.trait_type == "kTraitColor")
                    interp[x] = Math.Truncate(interp[x]);
                interpobject.data_points[InterpCells[0].ColumnIndex + x - FrozenColumnOffset].value = TCLE.TruncateDecimal((decimal)interp[x], 3);
            }
            IsInterpolating = false;
            //recolor cells after populating
            TrackUpdateHighlighting(interpobject);
            ShowRawTrackData(interpobject);
            TCLE.PlaySound("UIinterpolate");
            SaveCheckAndWrite(false, "Interpolated");
        }

        private void exampleswebLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = "https://easings.net/", UseShellExecute = true });
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
            int splitindex = trackEditor.CurrentCell.ColumnIndex - FrozenColumnOffset;
            if (MessageBox.Show($"Split this leaf before beat {splitindex}?\nTHIS CHANGE CANNOT BE UNDONE!", "Split leaf", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            //create file renaming dialog and show it
            FileInfo SplitFile;
            using SaveFileDialog sfd = new();
            sfd.Filter = "Thumper Leaf File (*.leaf)|*.leaf";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                SplitFile = new FileInfo(sfd.FileName);
            }
            else
                return;

            //create a new LeafProperties with a copy of the split leaf's properties, so that both splits are identical
            LeafProperties LeafSplitAfter = new(new Form_LeafEditor(), SplitFile, LeafProperties.beats - splitindex) {
                timesignature = LeafProperties.timesignature
            };
            //copy objects to the new split
            foreach (Sequencer_Object seq in LeafProperties.seq_objs) {
                Sequencer_Object clone = seq.Clone();
                clone.editor_row = null;
                clone.data_points = new SeqDataPoint[255].ToList();
                LeafSplitAfter.seq_objs.Add(clone);
                //only copy datapoints after the split index
                for (int x = splitindex; x < LeafProperties.beats; x++) {
                    clone.data_points[x - splitindex] = new SeqDataPoint() {
                        Beat = seq.data_points[x].beat - splitindex,
                        value = seq.data_points[x].value,
                        ease = seq.data_points[x].ease,
                        interpolation = seq.data_points[x].interpolation,
                        Owner = clone
                    };
                    //after copying, set the value to null since this datapoint is "leaving"
                    seq.data_points[x].value = null;
                }
            }
            //save the new split
            JObject tosave = BuildSave(LeafSplitAfter);
            using (StreamWriter sw = SplitFile.CreateText()) {
                sw.Write(JsonConvert.SerializeObject(tosave, Formatting.Indented));
            }
            //reduce beat count of the leaf that was just split and save it
            LeafProperties.beats = splitindex;
            SaveCheckAndWrite(true, "");
            TCLE.PlaySound("UIleafsplit");
            ProjectExplorer.CreateTreeView();
            //load new leaf that was just split
            TCLE.OpenFile(SplitFile);
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
            SaveCheckAndWrite(true, "");
            //SaveCheckAndWrite(true, "Revert to last save", "Revert");
            LoadLeaf(leafjson, LoadedLeaf);
            TCLE.PlaySound("UIrevertnew");
        }

        private void btnLeafAutoPlace_Click(object sender, EventArgs e)
        {
            TCLE.PlaySound("UIselect");
        }

        private void btnLeafRandom_Click(object sender, EventArgs e)
        {
            randomizing = true;
            //I pick category first rather than any object, as this gives Play Sample a higher chance of being picked
            //And all the tentacles a lower chance
            List<string> categories = TCLE.LeafObjects.Select(x => x.category).Distinct().ToList();
        beginrando:
            string category = categories[TCLE.rng.Next(0, categories.Count)];
            List<Object_Params> objects = TCLE.LeafObjects.Where(x => x.category == category).ToList();
            Object_Params obj = objects[TCLE.rng.Next(0, objects.Count)];
            //check if the object exists in the leaf already. If so, pick a new one
            if (SequencerObjects.Any(x => x.category == category && x.param_path == obj.param_path))
                goto beginrando;

            Sequencer_Object seq = new(leafProperties) {
                obj_name = category == "PLAY SAMPLE" ? TCLE.ProjectSamples[TCLE.rng.Next(0, TCLE.ProjectSamples.Count)].obj_name : obj.obj_name,
                category = obj.category,
                param_path = obj.param_path.Split('.')[0],
                friendly_param = obj.param_displayname,
                defaultvalue = float.Parse(obj.def),
                step = obj.step,
                trait_type = obj.trait_type,
                highlight_color = obj.defaultcolor,
                highlight_value = 0,
                footer = obj.footer,
                enabled = true,
                param_path_lane = obj.param_path.EndsWith(".ent") ? "ent" : "none",
                friendly_lane = obj.param_path.EndsWith(".ent") ? "lane center" : "none",
                editor_row = new DataGridViewRow(),
                expandlanes = Properties.Settings.Default.LeafOptionShowLane
            };
            if (seq.obj_name == "leafname")
                seq.obj_name = LeafProperties.FilePath.Name;
            if (seq.category == "AUDIO") {
                int audiochannels = SequencerObjects.Count(x => x.category == "AUDIO");
                seq.param_path = seq.param_path.Replace("x", $"{audiochannels}");
                seq.friendly_param = seq.friendly_param.Replace("x", $"{audiochannels}");
            }
            SequencerObjects.Add(seq);
            trackEditor.Rows.Add(seq.editor_row);
            ChangeTrackName(seq, Properties.Settings.Default.LeafOptionShowCategory ? $"[{seq.category}] " : "");
            TrackUpdateHighlighting(seq);
            FindMissingLaneObjects(seq);
            //measure header and see if it's the biggest
            int tempsize = TextRenderer.MeasureText(seq.editor_row.HeaderCell.Value.ToString(), seq.editor_row.HeaderCell.Style.Font).Width;
            if (tempsize > trackEditor.RowHeadersWidth)
                trackEditor.RowHeadersWidth = tempsize;
            //fill cells with random values
            do {
                if (seq.friendly_lane == "lane center") {
                    RandomizeRowValues(SequencerObjects[^5]);
                    RandomizeRowValues(SequencerObjects[^4]);
                    RandomizeRowValues(seq);
                    RandomizeRowValues(SequencerObjects[^2]);
                    RandomizeRowValues(SequencerObjects[^1]);
                }
                else
                    RandomizeRowValues(seq);
            } while (!seq.data_points.Any(x => x.value is not null));

            trackEditor.Invalidate();
            TCLE.PlaySound("UIaddrandom");
            randomizing = false;
            SaveCheckAndWrite(false, "Added Random Object");
        }

        private void btnLeafRandomValues_Click(object sender, EventArgs e)
        {
            if (trackEditor.CurrentRow?.Index is -1 or null)
                return;


            IEnumerable<Sequencer_Object> SelectedSeq = trackEditor.SelectedCells.Cast<DataGridViewCell>()
                .Select(cell => SequencerObjects[cell.RowIndex])
                .Distinct()
                .Where(x => x.friendly_lane is "none" or "lane center");

            if (MessageBox.Show("Assign random values to the current selected Objects?", "TELdCiethovrueulsmtpoemr", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                randomizing = true;
                foreach (Sequencer_Object seq in SelectedSeq) {
                    do {
                        if (seq.friendly_lane == "lane center") {
                            RandomizeRowValues(SequencerObjects[seq.editor_row.Index - 2]);
                            RandomizeRowValues(SequencerObjects[seq.editor_row.Index - 1]);
                            RandomizeRowValues(seq);
                            RandomizeRowValues(SequencerObjects[seq.editor_row.Index + 1]);
                            RandomizeRowValues(SequencerObjects[seq.editor_row.Index + 2]);
                        }
                        else
                            RandomizeRowValues(seq);
                    } while (!seq.editor_row.Cells.Cast<DataGridViewCell>().Any(x => x.Value != null));
                }
                ShowRawTrackData(SequencerObjects[CurrentRow]);
                trackEditor.Invalidate();

                TCLE.PlaySound("UIaddrandom");
                randomizing = false;
                SaveCheckAndWrite(false, "Set Random Values");
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

        private void labelCollapsePanel_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = !splitContainer1.Panel1Collapsed;
            labelCollapsePanel.Text = splitContainer1.Panel1Collapsed ? ">" : "<";
        }

        private void labelCollapsePanel2_Click(object sender, EventArgs e)
        {
            splitContainerLeafSide.Panel2Collapsed = !splitContainerLeafSide.Panel2Collapsed;
            labelCollapsePanel2.Text = splitContainerLeafSide.Panel2Collapsed ? "^" : "v";
            Properties.Settings.Default.LeafHideRaw = splitContainerLeafSide.Panel2Collapsed;
        }
        #endregion

        #region Methods
        public static void InitializeLeafStuff()
        {
            //meh
        }

        public object GetProperties()
        {
            return LeafProperties;
        }

        ///Update DGV from _tracks
        public void LoadLeaf(dynamic _load, FileInfo filepath, LvlProperties Lvl = null)
        {
            //skip certain checks if we're loading a non-leaf sequencer
            if (filepath.Extension == ".leaf") {
                if (_load == null)
                    return;
                //reset flag in case it got stuck previously
                EditorIsLoading = false;
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
            }
            //check for template or regular file
            if (filepath.Name == "template")
                loadedleaf = null;
            else
                loadedleaf = filepath;

            //set flag that load is in progress. This skips Save method
            EditorIsLoading = true;
            if (filepath.Extension == ".leaf") {
                this.Text = LoadedLeaf.Name;
                //
                leafProperties = new(this, filepath, (int?)_load["beat_cnt"] ?? 1) {
                    SequencerType = filepath.Extension,
                    timesignature = (string)_load["time_sig"] ?? "4/4"
                };
            }
            else if (filepath.Extension == ".lvl") {
                this.Text = $"{LoadedLeaf.Name} [Sequencer]";
                //
                leafProperties = new(this, filepath, Lvl.lvlleafs.Select(x => x.beats).Sum() + Lvl.approachbeats) {
                    SequencerType = filepath.Extension,
                    timesignature = "4/4"
                };
            }


            LeafLengthChanged();
        }

        public void LoadEnd()
        {
            //finsih up setting up the leaf editor. Enable some buttons, set zoom level, etc.
            EnableLeafButtons();
            trackZoom_Scroll(null, null);

            //mark that lvl is saved (just freshly loaded)
            EditorIsLoading = false;
            EditorIsSaved = true;
            SaveCheckAndWrite(true, "");
            TrackTimeSigHighlighting();
        }

        public static ObservableCollection<Sequencer_Object> LoadSequencer(dynamic seqJSON, LeafProperties parent)
        {
            ObservableCollection<Sequencer_Object> Seq_Objs = new();
            bool loadfail = false;
            string loadfailmessage = "";

            //each object in the seq_objs[] list
            foreach (dynamic seq_obj in seqJSON) {
                Sequencer_Object _s = new(parent) {
                    obj_name = seq_obj["obj_name"],
                    trait_type = seq_obj["trait_type"],
                    step = (string)seq_obj["step"] == "True",
                    defaultvalue = seq_obj["default"],
                    footer = seq_obj["footer"].GetType() == typeof(JArray) ? String.Join(",", ((JArray)seq_obj["footer"]).ToList()) : ((string)seq_obj["footer"]).Replace("[", "").Replace("]", ""),
                    //if the leaf has definitions for these, add them. If not, set to defaults
                    param_path = seq_obj.ContainsKey("param_path_hash") ? $"0x{(string)seq_obj["param_path_hash"]}" : ((string)seq_obj["param_path"]).Split('.')[0],
                    highlight_value = (int?)seq_obj["editor_data"]?[1] ?? 0,
                    enabled = ((string)seq_obj["enabled"] ?? "True") == "True",
                    isdefault = false
                };
                if (_s.param_path.StartsWith("layer_volume"))
                    _s.param_path = "layer_volume,x";
                _s.param_path_lane = seq_obj.ContainsKey("param_path") && ((string)seq_obj["param_path"]).Contains('.') ? ((string)seq_obj["param_path"]).Split('.')[1] : "none";
                _s.friendly_lane = TCLE.TrackLaneFriendly[_s.param_path_lane];
                //if object is a .samp, set the friendly_param and friendly_type since they don't exist in _objects
                if (_s.param_path == "play") {
                    _s.category = "PLAY SAMPLE";
                    _s.friendly_param = _s.param_path;
                }
                //otherwise, search _objects for the friendly names for display purposes
                else {
                    try {
                        string reg_param = $"{_s.param_path}{(_s.param_path_lane != "none" ? ".ent" : "")}";
                        Object_Params objmatch = TCLE.LeafObjects.FirstOrDefault(obj => obj.param_path == reg_param && obj.obj_name == _s.obj_name.Replace(parent.FilePath.Name, "leafname"));
                        _s.friendly_param = objmatch?.param_displayname ?? "";
                        _s.category = objmatch?.category ?? "";
                        //set audio channel numbers on load
                        if (_s.category == "AUDIO") {
                            int audiochannels = Seq_Objs.Count(x => x.category == "AUDIO");
                            _s.param_path = _s.param_path.Replace("x", $"{audiochannels}");
                            _s.friendly_param = _s.friendly_param.Replace("x", $"{audiochannels}");
                        }
                    }
                    catch (Exception) { }
                }
                _s.highlight_color = seq_obj["editor_data"]?[0] != null ? Color.FromArgb((int)seq_obj["editor_data"][0]) : (TCLE.LeafObjects.FirstOrDefault(x => x.param_displayname == _s.friendly_param)?.defaultcolor ?? Color.Purple);
                foreach (dynamic dp in seq_obj["data_points"]) {
                    if (dp is JObject data_point) {
                        SeqDataPoint data = new() {
                            Owner = _s,
                            Beat = (int)data_point["beat"],
                            value = (decimal)data_point["value"],
                            interpolation = ((string)data_point["interp"])?.Replace("kTraitInterp", "") ?? "Linear",
                            ease = TCLE.Easings[(string)data_point["ease"] ?? "kEaseInOut"]
                        };
                        _s.data_points[data.beat] = data;
                    }
                    else {
                        SeqDataPoint data = new() {
                            Owner = _s,
                            Beat = int.Parse(((JProperty)dp).Name),
                            value = TCLE.TruncateDecimal((decimal)((JProperty)dp).Value, 3),
                            interpolation = "Linear",
                            ease = TCLE.Easings["kEaseInOut"]
                        };
                        _s.data_points[data.beat] = data;
                    }
                }
                //if object is multilane, we will add all 5 lanes at once, as defaults
                //then lookup the object and assign the initialized Sequencer Object created above in place of the default one
                if (_s.friendly_lane is not "none") {
                    //ProcessOtherLanesLast.Add(_s);
                    Sequencer_Object lookup = Seq_Objs.FirstOrDefault(x => x.obj_name == _s.obj_name && x.param_path == _s.param_path && x.param_path_lane == _s.param_path_lane && x.isdefault == true);
                    //if null, no object exists in SequencerObjects yet for this object or its lanes. We'll have to make it.
                    if (lookup == null) {
                        Seq_Objs.Add(_s.CloneAsDefault("a01", "lane left 2", new DataGridViewRow()));
                        Seq_Objs.Add(_s.CloneAsDefault("a02", "lane left 1", new DataGridViewRow()));
                        Seq_Objs.Add(_s.CloneAsDefault("ent", "lane center", new DataGridViewRow()));
                        Seq_Objs.Add(_s.CloneAsDefault("z01", "lane right 1", new DataGridViewRow()));
                        Seq_Objs.Add(_s.CloneAsDefault("z02", "lane right 2", new DataGridViewRow()));
                    }
                    lookup = Seq_Objs.FirstOrDefault(x => x.obj_name == _s.obj_name && x.param_path == _s.param_path && x.param_path_lane == _s.param_path_lane && x.isdefault == true);
                    int index = Seq_Objs.IndexOf(lookup);
                    _s.editor_row = lookup.editor_row;
                    Seq_Objs[index] = _s;
                }
                //else just add the object without needing extra lanes
                else {
                    //attach the dgv row to the object
                    _s.editor_row = new DataGridViewRow();
                    _s.expandlanes = true;
                    //finally, add the completed seq_obj to tracks
                    Seq_Objs.Add(_s);
                }
            }

            if (loadfail) {
                MessageBox.Show($"Could not find obj_name or param_path for these items:\n{loadfailmessage}");
            }
            return Seq_Objs;
        }

        public void LoadTracksFromSequencer(ObservableCollection<Sequencer_Object> Seq_Objs)
        {
            //clear the DGV and prep for new data
            trackEditor.Rows.Clear();
            trackEditor.RowHeadersVisible = true;
            foreach (Sequencer_Object seq in Seq_Objs) {
                trackEditor.Rows.Add(seq.editor_row);
                TrackRawImport(seq, seq.data_points, LeafProperties);
                RowReadOnly(!seq.enabled, seq);
            }
            TCLE.ResizeHeaders(trackEditor);
        }

        public void Reload()
        {
            dynamic _load = TCLE.LoadFileLock(LoadedLeaf.FullName);
            LvlProperties lvlProperties = null;
            if (LoadedLeaf.Extension == ".lvl") {
                lvlProperties = new(new Form_LvlEditor(), LoadedLeaf) {
                    LeafReload = true,
                    approachbeats = (int)_load["approach_beats"],
                    volume = (decimal)_load["volume"],
                    allowinput = (string)_load["input_allowed"] == "True",
                    tutorialtype = (string)_load["tutorial_type"],
                    seqJSON = _load["seq_objs"]
                };
                foreach (dynamic leaf in _load["leaf_seq"]) {
                    lvlProperties.lvlleafs.Add(new LvlLeafData() {
                        leafname = (string)leaf["leaf_name"],
                        beats = (int)leaf["beat_cnt"],
                        paths = leaf["sub_paths"].ToObject<List<string>>(),
                        id = TCLE.rng.Next(0, 1000000)
                    });
                }
            }

            LoadLeaf(_load, LoadedLeaf, lvlProperties);
            //each object in the seq_objs[] list becomes a track
            LeafProperties.seq_objs = LoadSequencer(_load["seq_objs"], LeafProperties);
            LoadTracksFromSequencer(LeafProperties.seq_objs);
            LoadEnd();
        }

        public List<SaveState> GetUndoList()
        {
            return UndoList;
        }

        public void PerformUndo(int undolistindex)
        {
            if (undolistindex > UndoList.Count - 1)
                return;
            LoadLeaf(UndoList[undolistindex].savestate, LvlSequencer?.FilePath ?? LoadedLeaf, LvlSequencer);
            LeafProperties.seq_objs = LoadSequencer(UndoList[undolistindex].savestate["seq_objs"], LeafProperties);
            LoadTracksFromSequencer(LeafProperties.seq_objs);
            LoadEnd();
            UndoList.RemoveRange(0, undolistindex);
            propertyGridLeaf.Refresh();
        }

        ///SAVE
        public void Save(bool playsound = true)
        {
            //if _loadedlvl is somehow not set, force Save As instead
            if (LoadedLeaf == null) {
                SaveAs();
            }
            else
                SaveCheckAndWrite(true, "", playsound);
        }
        ///SAVE AS
        public FileInfo SaveAs(bool isnew = false)
        {
            using SaveFileDialog sfd = new();
            //filter .txt only
            sfd.Filter = "Thumper Editor Leaf File (*.leaf)|*.leaf";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                loadedleaf = new FileInfo(sfd.FileName);

                if (LeafProperties == null) {
                    leafProperties = new(this, loadedleaf, 32) {
                        timesignature = "4/4"
                    };
                }
                else
                    leafProperties.FilePath = loadedleaf;

                SaveCheckAndWrite(true, "", true);
                if (isnew)
                    TCLE.CloseFileLock(loadedleaf);
                //after saving new file, refresh the project explorer
                ProjectExplorer.CreateTreeView();
            }
            return loadedleaf;
        }

        public bool IsSaved()
        {
            return EditorIsSaved;
        }

        public void SaveCheckAndWrite(bool IsSaved, string Reason, bool playsound = false)
        {
            if (EditorIsLoading)
                return;
            EnableLeafButtons();
            //make the beeble emote
            TCLE.MainBeeble.MakeFace();

            EditorIsSaved = IsSaved;
            JObject _saveJSON = BuildSave(leafProperties);
            //
            if (!IsSaved) {
                //denote editor tab is not saved
                this.Text = $"{LoadedLeaf.Name}{(LoadedLeaf.Extension == ".lvl" ? " [Sequencer]" : "")}" + "*";
                //update the undo list
                if (LogUndo) {
                    UndoList.Insert(0, new SaveState() {
                        reason = Reason,
                        savestate = _saveJSON
                    });
                }
            }
            else {
                this.Text = $"{LoadedLeaf.Name}{(LoadedLeaf.Extension == ".lvl" ? " [Sequencer]" : "")}";
                leafProperties.revertPoint = _saveJSON;
                //If leaf, build the JSON to write to file
                if (LoadedLeaf.Extension == ".leaf") {
                    //write JSON to file
                    TCLE.WriteFileLock(TCLE.lockedfiles[LoadedLeaf], _saveJSON);
                    TCLE.FindEditorRunMethod(typeof(Form_LvlEditor), "RecalculateRuntime");
                    if (playsound) TCLE.PlaySound("UIsave");
                }
                //else if a different sequencer, pass data back and force save
                else {
                    if (TCLE.Documents.FirstOrDefault(x => x.DockHandler.TabText.StartsWith(LoadedLeaf.Name)) is Form_LvlEditor Owner) {
                        Owner.lvlProperties.seq_objs = LeafProperties.seq_objs;
                        Owner.Save();
                    }
                }

                //find if any raw text docs are open of this gate and update them
                TCLE.FindReloadRaw(LoadedLeaf.Name);
            }
        }
        ///LEAF LENGTH
        public void LeafLengthChanged()
        {
            if (LeafProperties == null || SaveOnlyNoLoad)
                return;
            int data = trackEditor.ColumnCount - FrozenColumnOffset;

            if (LeafProperties.beats + FrozenColumnOffset > trackEditor.ColumnCount) {
                trackEditor.ColumnCount = LeafProperties.beats + FrozenColumnOffset;
                TCLE.GenerateColumnStyle(trackEditor.Columns.Cast<DataGridViewColumn>().Where(x => x.Index >= FrozenColumnOffset).ToList(), FrozenColumnOffset);
            }
            else
                trackEditor.ColumnCount = LeafProperties.beats + FrozenColumnOffset;
            //clear out data that exists beyond the beatcount
            foreach (Sequencer_Object seq in SequencerObjects)
            {
                for (int x = LeafProperties.beats; x < 255; x++)
                {
                    seq.data_points[x] = new() { Beat = x, interpolation = "Linear", ease = "Ease In Out" };
                }
            }
            //set cell zoom
            trackZoom_Scroll(null, null);
            //make sure new cells follow the time sig
            TrackTimeSigHighlighting();
            //sets flag that leaf has unsaved changes
            SaveCheckAndWrite(false, $"Leaf length changed {data} -> {LeafProperties.beats}");
            //SaveCheckAndWrite(false, "Leaf length", $"{data} -> {_beats}");
        }

        ///Import raw text from rich text box to selected row
        public static void TrackRawImport(Sequencer_Object seq, List<SeqDataPoint> data_points, LeafProperties _properties)
        {
            List<SeqDataPoint> DataNotNull = data_points.Where(x => x.value is not null && x.beat < _properties.beats).ToList();
            //iterate over each data point, and fill cells
            foreach (SeqDataPoint data_point in DataNotNull) {
                try {
                    seq.editor_row.Cells[data_point.beat + FrozenColumnOffset].Value = TCLE.TruncateDecimal(Decimal.Parse(data_point.value.ToString()), 3);
                    //seq.data_points[data_point.beat].value = TCLE.TruncateDecimal(Decimal.Parse(data_point.value.ToString()), 3);
                }
                catch (Exception ex) {
                    MessageBox.Show($"Failed to entirely parse raw text.\n\n{ex}", "Thumper Custom Level Editor");
                    break;
                }
            }

            TrackUpdateHighlighting(seq);
        }
        public static void TrackRawImport(Sequencer_Object seq, JObject _rawdata)
        {
            //_rawdata contains a list of all data points. By getting Properties() of it,
            //each point becomes its own index
            List<JProperty> data_points = _rawdata.Properties().ToList();
            //iterate over each data point, and fill cells
            foreach (JProperty data_point in data_points) {
                try {
                    seq.editor_row.Cells[int.Parse(data_point.Name) + FrozenColumnOffset].Value = TCLE.TruncateDecimal((decimal)data_point.Value, 3);
                    //seq.data_points[int.Parse(data_point.Name)].value = TCLE.TruncateDecimal((decimal)data_point.Value, 3);
                }
                catch (ArgumentOutOfRangeException) {
                    break;
                }
            }

            TrackUpdateHighlighting(seq);
        }

        ///Updates row headers to be the Object and Param_Path
        public static void ChangeTrackName(Sequencer_Object seq, string category = "")
        {
            string ShowCategory = category;
            string ShowLane = (seq.expandlanes && seq.friendly_lane != "none") ? $"{seq.friendly_param}, {seq.friendly_lane}" : seq.friendly_param;
            if (seq.category == "PLAY SAMPLE")
                //show the sample name instead
                seq.editor_row.HeaderCell.Value = $"{ShowCategory}{seq.obj_name}";
            else
                seq.editor_row.HeaderCell.Value = $"{ShowCategory}{ShowLane}";
        }

        public void ShowRawTrackData(Sequencer_Object seq)
        {
            string allcellvalues = String.Join(",", seq.data_points.Where(x => x.value is not null).Select(x => $"{x.beat}:{x.value}"));
            textEditor.Text = allcellvalues;
            textEditor.ClearUndo();
            textEditor.SetSelectedLine(-1);
        }

        ///Updates column highlighting in the DGV based on time sig
        public void TrackTimeSigHighlighting()
        {
            if (LeafProperties == null || EditorIsLoading)
                return;
            bool _switch = true;
            //grab the first part of the time sig. This represents how many beats are in a bar
            //tryparse to see if it fails.
            if (!int.TryParse(LeafProperties.timesignature.Split('/')[0], out int timesigbeats))
                return;
            for (int i = 0; i < LeafProperties.beats; i++) {
                //whenever `i` is a multiple of the time sig, switch colors
                if ((i) % timesigbeats == 0)
                    _switch = !_switch;
                trackEditor.Columns[i + FrozenColumnOffset].DefaultCellStyle.BackColor = _switch ? Properties.Settings.Default.ColorLeafTimeSig1 : Properties.Settings.Default.ColorLeafTimeSig2;
                trackEditor.Columns[i + FrozenColumnOffset].HeaderCell.Style.BackColor = _switch ? Properties.Settings.Default.ColorLeafTimeSig1 : Properties.Settings.Default.ColorLeafTimeSig2;
            }

            if (LvlSequencer != null)
                TrackLeafDividerHighlighting(LvlSequencer);
        }

        public void TrackLeafDividerHighlighting(LvlProperties Lvl)
        {
            int index = FrozenColumnOffset;

            trackEditor.Columns[index].DefaultCellStyle.BackColor = Color.LightGray;
            trackEditor.Columns[index].HeaderCell.Style.BackColor = Color.LightGray;
            trackEditor.Columns[index].HeaderCell.Style.ForeColor = Color.Black;
            trackEditor.Columns[index].HeaderText = "Approach";
            index += Lvl.approachbeats;

            foreach (LvlLeafData leaf in Lvl.lvlleafs) {
                trackEditor.Columns[index].DefaultCellStyle.BackColor = Color.LightGray;
                trackEditor.Columns[index].HeaderCell.Style.BackColor = Color.LightGray;
                trackEditor.Columns[index].HeaderCell.Style.ForeColor = Color.Black;
                trackEditor.Columns[index].HeaderText = leaf.leafname;
                //trackEditor.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                index += leaf.beats;
            }
        }
        private void trackEditor_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.FillWeight = 0.001f;
        }

        ///Updates cell highlighting in the DGV
        public static void TrackUpdateHighlighting(Sequencer_Object seq)
        {
            Color background = TCLE.Blend(seq.highlight_color, Color.Black, 0.4);
            seq.editor_row.HeaderCell.Style.BackColor = background;

            if (seq.editor_row.Cells.Count >= 3) {
                seq.editor_row.Cells[0].Style.BackColor = background;
                seq.editor_row.Cells[1].Style.BackColor = background;
                seq.editor_row.Cells[2].Style.BackColor = background;
            }
        }
        /*
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
            //if ((decimal)_seqobj.highlight_value == 0 || Math.Abs(Decimal.Parse(dgvc.Value.ToString())) >= (decimal)_seqobj.highlight_value) {
            ///dgvc.Style.BackColor = _seqobj.highlight_color;
            //}
            //change cell font color so text is readable on dark/light backgrounds
            Color _c = dgvc.Style.BackColor;
            if (_c.R < 150 && _c.G < 150 && _c.B < 150)
                dgvc.Style.ForeColor = Color.White;
            else
                dgvc.Style.ForeColor = Color.Black;
        }
        */
        private static void RowReadOnly(bool isreadonly, Sequencer_Object seq)
        {
            if (isreadonly) {
                seq.editor_row.ReadOnly = true;
                foreach (DataGridViewCell dgvc in seq.editor_row.Cells.Cast<DataGridViewCell>().Where(x => x.ColumnIndex >= FrozenColumnOffset)) {
                    dgvc.Style.BackColor = Color.Gray;
                    dgvc.Style.SelectionBackColor = Color.Gray;
                }
            }
            else {
                seq.editor_row.ReadOnly = false;
                foreach (DataGridViewCell dgvc in seq.editor_row.Cells.Cast<DataGridViewCell>().Where(x => x.ColumnIndex >= FrozenColumnOffset)) {
                    dgvc.Style = null;
                }
                TrackUpdateHighlighting(seq);
            }
        }

        private void EnableLeafButtons()
        {
            btnTrackDelete.Enabled = SequencerObjects.Count > 0;
            btnTrackUp.Enabled = SequencerObjects.Count > 1;
            btnTrackDown.Enabled = SequencerObjects.Count > 1;
            btnTrackClear.Enabled = SequencerObjects.Count > 0;
            btnTrackCopy.Enabled = SequencerObjects.Count > 0;
            btnTrackPaste.Enabled = TCLE.ClipboardSequencer.Count > 0;
        }

        public static JObject BuildSave(LeafProperties _properties, bool skiprevertsave = false)
        {
            ///start building JSON output
            JObject _save = new() {
                { "obj_type", "SequinLeaf" },
                { "obj_name", _properties.FilePath.Name }
            };

            JArray seq_objs = new();
            foreach (Sequencer_Object seq_obj in _properties.seq_objs.Where(x => !x.isdefault)) {
                //skip blank tracks
                if (seq_obj.friendly_param == null)
                    continue;
                JObject s = new();
                //if saving a leaf as a new name, obj_name's have to be updated, otherwise it saves with the old file's name
                if (seq_obj.obj_name.Contains(".leaf") || string.IsNullOrEmpty(seq_obj.obj_name))
                    seq_obj.obj_name = (string)_save["obj_name"];
                //also adjust default leafname to the actual leaf's name
                if (seq_obj.obj_name == "leafname")
                    seq_obj.obj_name = (string)_save["obj_name"];
                s.Add("obj_name", seq_obj.obj_name.Replace("leafname", (string)_save["obj_name"]));
                //write param_path or param_path_hash
                if (seq_obj.param_path.StartsWith("0x"))
                    s.Add("param_path_hash", seq_obj.param_path.Replace("0x", ""));
                else
                    s.Add("param_path", $"{seq_obj.param_path}{(seq_obj.param_path_lane != "none" ? "." + seq_obj.param_path_lane : "")}");
                s.Add("trait_type", seq_obj.trait_type);
                JArray datapoints = new();
                foreach (SeqDataPoint datapoint in seq_obj.data_points.Where(x => x != null && x.value is not null)) {
                    JObject d = new() {
                        { "beat", datapoint.beat },
                        { "value", decimal.Parse(datapoint.value.ToString()) },
                        { "interp", $"kTraitInterp{datapoint.interpolation ?? "Linear"}" },
                        { "ease", $"k{datapoint.ease?.Replace(" ", "") ?? "EaseInOut"}" }
                    };

                    datapoints.Add(d);
                }
                s.Add("data_points", datapoints);
                ///end
                //add the rest of the keys to this seq_obj
                s.Add("step", seq_obj.step.ToString());
                s.Add("default", seq_obj.defaultvalue);
                s.Add("footer", seq_obj.footer);
                s.Add("editor_data", new JArray() { new object[] { seq_obj.highlight_color.ToArgb(), seq_obj.highlight_value } });
                s.Add("enabled", seq_obj.enabled.ToString());

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
            SaveCheckAndWrite(true, "");
        }

        public void BuildObjectTree()
        {
            bool filtersearch = txtSearch.Text is not "" and not "Search Objects (Ctrl+;)";

            treeObjects.Nodes.Clear();
            //Add Favorites right at the top
            TreeNode fav = new() {
                Text = "*FAVORITES*",
                ImageKey = "fav",
                SelectedImageKey = "fav",
                ContextMenuStrip = contextMenuFavClear
            };
            treeObjects.Nodes.Add(fav);
            BuildTreeFavorites();
            if (filtersearch && fav.Nodes.Count == 0)
                fav.Remove();

            //make each category of objects its own node
            foreach (string category in TCLE.LeafObjects.Select(x => x.category).Distinct().Order()) {
                TreeNode _node = new() {
                    Text = category.ToUpper(),
                    ImageKey = "category",
                    SelectedImageKey = "category"
                };
                if (category == "PLAY SAMPLE") {
                    //samples are not stored in LeafObjects, so we loop over a different list to find them
                    //seperate samples into sub-nodes by the file they came from
                    foreach (string file in TCLE.ProjectSamples.Select(x => x.File?.Name).Distinct()) {
                        if (string.IsNullOrEmpty(file))
                            continue;
                        TreeNode sampfile = new() {
                            Text = file,
                            ImageKey = "samp",
                            SelectedImageKey = "samp"
                        };
                        foreach (SampleData samp in TCLE.ProjectSamples.Where(x => x.File?.Name == file)) {
                            TreeNode _param = new() {
                                Text = samp.obj_name,
                                ImageKey = "none",
                                SelectedImageKey = "none",
                                ToolTipText = $"Pitch: {samp.pitch}\nPan: {samp.pan}\nOffset: {samp.offset}\nSelect sample and then hold SPACE to play it",
                            };
                            if ((filtersearch && _param.Text.Contains(txtSearch.Text)) || !filtersearch)
                                sampfile.Nodes.Add(_param);
                        }
                        if ((filtersearch && sampfile.Nodes.Count != 0) || !filtersearch)
                            _node.Nodes.Add(sampfile);
                    }
                }
                else {
                    //each object becomes its own node
                    foreach (Object_Params obj in TCLE.LeafObjects.Where(x => x.category == category)) {
                        TreeNode _param = new() {
                            Text = obj.param_displayname,
                            ImageKey = TCLE.ObjectFavorites.Contains(obj) ? "fav" : "none",
                            SelectedImageKey = TCLE.ObjectFavorites.Contains(obj) ? "fav" : "none",
                            ContextMenuStrip = contextMenuFav
                        };
                        if ((filtersearch && _param.Text.Contains(txtSearch.Text)) || !filtersearch)
                            _node.Nodes.Add(_param);
                    }
                }
                if ((filtersearch && _node.Nodes.Count != 0) || !filtersearch)
                    treeObjects.Nodes.Add(_node);
            }

            if (filtersearch)
                treeObjects.ExpandAll();
        }

        private void BuildTreeFavorites()
        {
            bool filtersearch = txtSearch.Text is not "" and not "Search Objects (Ctrl+;)";

            treeObjects.Nodes[0].Nodes.Clear();
            foreach (string obj in TCLE.ObjectFavorites.Select(x => x.param_displayname).Order()) {
                TreeNode _param = new() {
                    Text = obj,
                    ImageKey = "none",
                    SelectedImageKey = "none",
                    ContextMenuStrip = contextMenuFavRemove
                };
                if ((filtersearch && _param.Text.Contains(txtSearch.Text)) || !filtersearch)
                    treeObjects.Nodes[0].Nodes.Add(_param);
            }
        }

        private static TreeNode FindNode(string search, TreeNodeCollection nodes)
        {
            TreeNode foundnode = null;
            foreach (TreeNode tn in nodes) {
                if (tn.Text == search)
                    return tn;
                foundnode = FindNode(search, tn.Nodes);
                if (foundnode != null)
                    break;
            }
            return foundnode;
        }

        #region Cut Copy Paste
        public void Copy()
        {
            ///copies selected cells
            TCLE.ClipBoardDataPoints = trackEditor.GetClipboardContent();
            TCLE.Instance.toolstripEditPaste.Enabled = true;
        }

        public void Cut()
        {
            ///cut and copies selected cells
            TCLE.ClipBoardDataPoints = trackEditor.GetClipboardContent();
            LogUndo = false;
            CellValueChanged(trackEditor.CurrentCell.RowIndex, trackEditor.CurrentCell.ColumnIndex, true);
            LogUndo = true;
            SaveCheckAndWrite(false, "Cut cells");
        }

        public void Paste()
        {
            //get content on clipboard to string and then split it to rows
            string s = TCLE.ClipBoardDataPoints.GetText().Replace("\r\n", "\n");
            string[][] copiedcells = s.Split('\n').Select(x => x.Split('\t')).ToArray();
            //set ints so we don't have to always call rowindex, columnindex
            int pastingrow = trackEditor.CurrentCell.RowIndex;
            int pastingcol = trackEditor.CurrentCell.ColumnIndex;
            int offset = 0;
            ispasting = true;
            LogUndo = false;
            for (int rowindex = 0; rowindex < copiedcells.Length; rowindex++) {
                if (pastingrow + rowindex + offset >= trackEditor.RowCount)
                    break;
                //if paste will go outside grid bounds, skip
                while (trackEditor.Rows[pastingrow + rowindex + offset].Visible == false) {
                    offset += 1;
                    if (pastingrow + rowindex + offset >= trackEditor.RowCount)
                        goto exit;
                }
                for (int cellindex = 0; cellindex < copiedcells[rowindex].Length; cellindex++) {
                    //if paste will go outside grid bounds, skip
                    if (pastingcol + cellindex >= trackEditor.ColumnCount)
                        break;
                    //don't paste if cell is blank
                    if (string.IsNullOrEmpty(copiedcells[rowindex][cellindex]))
                        continue;
                    //trackEditor[pastingcol + cellindex, pastingrow + rowindex + offset].Value = decimal.Parse(copiedcells[rowindex][cellindex]);
                    SequencerObjects[pastingrow + rowindex + offset].data_points[pastingcol + cellindex - FrozenColumnOffset].value = decimal.Parse(copiedcells[rowindex][cellindex]);
                    ///TrackUpdateHighlightingSingleCell(trackEditor[pastingcol + cellindex, pastingrow + rowindex + offset], SequencerObjects[pastingrow + rowindex + offset]);
                }
                trackEditor.InvalidateRow(pastingrow + rowindex + offset);
            }
        exit:
            ispasting = false;
            LogUndo = true;
            SaveCheckAndWrite(false, "Pasted cells");
        }
        #endregion

        public static void RandomizeRowValues(Sequencer_Object seq)
        {
            Random rng = new();
            int rngchance;
            int rnglimit;
            int randomtype = 0;
            decimal valueiftrue = 0;

            if ((seq.trait_type is "kTraitBool" or "kTraitAction") || (seq.param_path is "visibla01" or "visibla02" or "visible" or "visiblz01" or "visiblz02")) {
                valueiftrue = 1;
                rngchance = 10;
                rnglimit = 9;
                if (seq.obj_name == "sentry.spn") {
                    rngchance = 55;
                    rnglimit = 54;
                }
            }
            else if (seq.trait_type == "kTraitColor") {
                randomtype = 7;
                rngchance = 10;
                rnglimit = 8;
            }
            else {
                rngchance = 10;
                rnglimit = 9;
                if (seq.param_path == "sequin_speed")
                    randomtype = 2;
                else if (seq.obj_name == "fade.pp")
                    randomtype = 3;
                else if (seq.category == "CAMERA")
                    randomtype = 4;
                else if (seq.category == "GAMMA")
                    randomtype = 5;
                else
                    randomtype = 6;
            }
            foreach (DataGridViewCell dgvc in seq.editor_row.Cells.Cast<DataGridViewCell>().Where(x => x.ColumnIndex >= FrozenColumnOffset)) {
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

                object _out = rng.Next(0, rngchance) >= rnglimit ? valueiftrue : null;
                dgvc.Value = _out;
                seq.data_points[dgvc.ColumnIndex - FrozenColumnOffset] = new() { Owner = seq, Beat = dgvc.ColumnIndex - FrozenColumnOffset, value = (decimal?)_out, ease = "Ease In Out", interpolation = "Linear" };
            }
            TrackUpdateHighlighting(seq);
        }

        private void FindMissingLaneObjects(Sequencer_Object seq)
        {
            if (ismoving)
                return;
            //don't need to find lanes for non-multi-lanes
            if (seq.friendly_lane == "none")
                return;
            isfinding = true;
            int indexofcenter = SequencerObjects.IndexOf(seq);
            if (indexofcenter - 1 < 0 || (SequencerObjects[indexofcenter - 1].obj_name != seq.obj_name || SequencerObjects[indexofcenter - 1].param_path != seq.param_path)) {
                trackEditor.Rows.Insert(indexofcenter, 1);
                SequencerObjects.Insert(indexofcenter, seq.CloneAsDefault("a02", "lane left 1", trackEditor.Rows[indexofcenter]));
                SequencerObjects[indexofcenter].expandlanes = Properties.Settings.Default.LeafOptionShowLane;
                indexofcenter += 1;
            }
            if (indexofcenter - 2 < 0 || (SequencerObjects[indexofcenter - 2].obj_name != seq.obj_name || SequencerObjects[indexofcenter - 2].param_path != seq.param_path)) {
                trackEditor.Rows.Insert(indexofcenter - 1, 1);
                SequencerObjects.Insert(indexofcenter - 1, seq.CloneAsDefault("a01", "lane left 2", trackEditor.Rows[indexofcenter - 1]));
                SequencerObjects[indexofcenter - 1].expandlanes = Properties.Settings.Default.LeafOptionShowLane;
                indexofcenter += 1;
            }
            if (indexofcenter + 1 > SequencerObjects.Count - 1 || (SequencerObjects[indexofcenter + 1].obj_name != seq.obj_name || SequencerObjects[indexofcenter + 1].param_path != seq.param_path)) {
                trackEditor.Rows.Insert(indexofcenter + 1, 1);
                SequencerObjects.Insert(indexofcenter + 1, seq.CloneAsDefault("z01", "lane right 1", trackEditor.Rows[indexofcenter + 1]));
                SequencerObjects[indexofcenter + 1].expandlanes = Properties.Settings.Default.LeafOptionShowLane;
            }
            if (indexofcenter + 2 > SequencerObjects.Count - 1 || (SequencerObjects[indexofcenter + 2].obj_name != seq.obj_name || SequencerObjects[indexofcenter + 2].param_path != seq.param_path)) {
                trackEditor.Rows.Insert(indexofcenter + 2, 1);
                SequencerObjects.Insert(indexofcenter + 2, seq.CloneAsDefault("z02", "lane right 2", trackEditor.Rows[indexofcenter + 2]));
                SequencerObjects[indexofcenter + 2].expandlanes = Properties.Settings.Default.LeafOptionShowLane;
            }
            isfinding = false;
        }
        #endregion


        private Rectangle dragBoxFromMouseDown;
        Sequencer_Object[] RowsToMove;
        Sequencer_Object CenterLane;
        private int columnIndexFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private int previousDragOver = -1;
        private void trackEditor_MouseMove(object sender, MouseEventArgs e)
        {
            if (rowIndexFromMouseDown == -1)
                return;
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                // If the mouse moves outside the rectangle, start the drag.
                if (RowsToMove == null && dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y)) {
                    RowsToMove = ReturnLanesFromName(SequencerObjects[rowIndexFromMouseDown], SequencerObjects[rowIndexFromMouseDown].friendly_lane);
                    CenterLane = RowsToMove.Length == 5 ? RowsToMove[2] : RowsToMove[0];
                    ismoving = true;
                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = trackEditor.DoDragDrop(CenterLane.editor_row, DragDropEffects.Move);
                    RowsToMove = null;
                    ismoving = false;
                }
            }
        }

        private void trackEditor_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            columnIndexFromMouseDown = trackEditor.HitTest(e.X, e.Y).ColumnIndex;
            rowIndexFromMouseDown = trackEditor.HitTest(e.X, e.Y).RowIndex;
            if (columnIndexFromMouseDown == -1) {
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

        private void trackEditor_DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = trackEditor.PointToClient(new Point(e.X, e.Y));
            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop = trackEditor.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move) {
                if (e.Data.GetData(typeof(DataGridViewRow)) is DataGridViewRow rowToMove) {
                    if (rowIndexOfItemUnderMouseToDrop == -1)
                        return;
                    /*
                    trackEditor.SuspendLayout();
                    Sequencer_Object[] RowsToMove = ReturnLanesFromName(SequencerObjects[rowToMove.Index], SequencerObjects[rowToMove.Index].friendly_lane).Reverse().ToArray();
                    foreach (Sequencer_Object seq in RowsToMove) {
                        trackEditor.Rows.Remove(seq.editor_row);
                        SequencerObjects.Remove(seq);
                        SequencerObjects.Insert(rowIndexOfItemUnderMouseToDrop, seq);
                        trackEditor.Rows.Insert(rowIndexOfItemUnderMouseToDrop, seq.editor_row);
                    }
                    trackEditor.ResumeLayout();
                    */
                    SaveCheckAndWrite(false, "Reorder Sequencer Objects");
                }
            }
        }
        private void trackEditor_DragEnter(object sender, DragEventArgs e) => e.Effect = DragDropEffects.Move;
        private void trackEditor_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = trackEditor.PointToClient(new Point(e.X, e.Y));
            // Retrieve the node at the drop location.
            int targetRow = trackEditor.HitTest(targetPoint.X, targetPoint.Y).RowIndex;
            if (targetRow == -1)
                return;
            //check if target location exists inside the lane selection
            if (RowsToMove.Length == 5 &&
                ((RowsToMove[0].friendly_lane == "lane right 2" && targetRow >= RowsToMove[4].editor_row.Index && targetRow <= RowsToMove[0].editor_row.Index) ||
                (RowsToMove[0].friendly_lane == "lane left 2" && targetRow >= RowsToMove[0].editor_row.Index && targetRow <= RowsToMove[4].editor_row.Index)))
                return;

            if (SequencerObjects[targetRow].friendly_lane != "none" && SequencerObjects[targetRow].expandlanes) {
                return;
            }

            if (RowsToMove.Length == 5 && RowsToMove[0].friendly_lane == "lane left 2" && targetRow < RowsToMove[0].editor_row.Index)
                RowsToMove = RowsToMove.Reverse().ToArray();
            else if (RowsToMove.Length == 5 && RowsToMove[0].friendly_lane == "lane right 2" && targetRow > RowsToMove[0].editor_row.Index)
                RowsToMove = RowsToMove.Reverse().ToArray();

            if (SequencerObjects[targetRow].friendly_lane == "lane center" && targetRow > RowsToMove[0].editor_row.Index)
                targetRow += 2;
            else if (SequencerObjects[targetRow].friendly_lane == "lane center" && targetRow < RowsToMove[0].editor_row.Index)
                targetRow -= 2;


            if (RowsToMove != null && targetRow != -1 && targetRow != previousDragOver) {
                previousDragOver = targetRow;
                TCLE.Instance.toolstripLevelName.Text = $"{targetRow}";
                trackEditor.SuspendLayout();
                /*
                trackEditor.Rows.Remove(CenterLane.editor_row);
                SequencerObjects.Remove(CenterLane);
                SequencerObjects.Insert(targetRow, CenterLane);
                trackEditor.Rows.Insert(targetRow, CenterLane.editor_row);
                */
                foreach (Sequencer_Object seq in RowsToMove) {
                    trackEditor.Rows.Remove(seq.editor_row);
                    SequencerObjects.Remove(seq);
                    SequencerObjects.Insert(targetRow, seq);
                    trackEditor.Rows.Insert(targetRow, seq.editor_row);
                }

                trackEditor.ResumeLayout();
            }
            /*
            //changing the hovered node backcolor to make it obvious where the destination will be
            if (previousDragOver != targetRow && previousDragOver != -1) {
                //trackEditor.Rows[previousDragOver].DefaultCellStyle = null;
            }
            if (targetRow != -1 && targetRow != previousDragOver) {
                //trackEditor.Rows[targetRow].DefaultCellStyle.BackColor = Color.FromArgb(64, 53, 130);
                previousDragOver = targetRow;
            }
            */
        }

        private void btnTrackPlayback_Click(object sender, EventArgs e)
        {
            if (Playback.IsPlaying) {
                Playback.IsPlaying = false;
                ForceStop = true;
            }
            else {
                //timer interval twice as small as the bpm (*500ms, instead of *1000ms), so it can keep up with the Playback threading timer
                timer1.Interval = (int)((60 / TCLE.BPM) * (1000 / Playback.BeatSubdivisions));
                btnTrackPlayback.Image = Properties.Resources.icon_stop;
                Playback.Initialize();
                Playback.CreatePlaybackFromLeaf(LeafProperties, PlaybackEnd);
                Playback.Play(PlaybackStart, PlaybackLoop);
                if (Playback.IsPlaying) {
                    timer1.Enabled = true;
                }
                else {
                    Bass.BASS_ChannelFree(Playback.MidiStream);
                    TCLE.alzheimer();
                    btnTrackPlayback.Image = Properties.Resources.icon_play2;
                }
            }
        }

        private int PreviousSetColumn = 3;
        private bool ForceStop;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Playback.PlaybackBeat < 0)
                return;
            if (Playback.IsPlaying && Playback.PlaybackBeat + FrozenColumnOffset < trackEditor.ColumnCount) {
                trackEditor.InvalidateColumn(PreviousSetColumn);
                trackEditor.InvalidateColumn(PreviousSetColumn - 1);
                trackEditor.InvalidateColumn(Playback.PlaybackBeat + FrozenColumnOffset);
                PreviousSetColumn = Playback.PlaybackBeat + FrozenColumnOffset;
            }
            else {
                if (PlaybackLoop && !ForceStop)
                    return;
                ForceStop = false;
                timer1.Enabled = false;
                Playback.IsPlaying = false;
                Bass.BASS_ChannelStop(Playback.MidiStream);
                var Error = Bass.BASS_ErrorGetCode();
                Playback.SyncTimer.Dispose();
                btnTrackPlayback.Image = Properties.Resources.icon_play2;
                PreviousSetColumn = 3;
                trackEditor.Invalidate();
            }
        }
    }
}
