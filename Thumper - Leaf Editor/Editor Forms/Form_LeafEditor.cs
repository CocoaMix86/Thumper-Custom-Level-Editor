using ICSharpCode.TextEditor.Document;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Documents;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods;
using Un4seen.Bass;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_LeafEditor : DockContentEx
    {
        #region Form Construction
        ///Load LEAF
        public Form_LeafEditor(dynamic load = null, FileInfo filepath = null, bool saveonlynoload = false)
        {
            if (Playback.Generating) {
                LoadLeafSimple(load, filepath);
                LoadSequencer(load["seq_objs"], LeafProperties);
                return;
            }

            SaveOnlyNoLoad = saveonlynoload;
            InitializeComponent();
            RenderForm();
            ColorFormElements();

            if (load != null) {
                LoadLeaf(load, filepath);
                //each object in the seq_objs[] list becomes a track
                LoadSequencer(load["seq_objs"], LeafProperties);
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
        public Form_LeafEditor(LvlProperties toload, bool saveonlynoload = false)
        {
            SaveOnlyNoLoad = saveonlynoload;
            if (Playback.Generating) {
                LvlSequencer = toload;
                LoadLeafSimple(null, LvlSequencer.FilePath, LvlSequencer);
                LoadSequencer(LvlSequencer.seqJSON, LeafProperties);
                return;
            }

            InitializeComponent();
            RenderForm();
            ColorFormElements();
            this.Icon = Properties.Resources.ico_lvl;

            if (toload != null) {
                LvlSequencer = toload;
                LoadLeaf(null, LvlSequencer.FilePath, LvlSequencer);
                if (!SaveOnlyNoLoad) {
                    LoadSequencer(LvlSequencer.seqJSON, LeafProperties);
                    LoadTracksFromSequencer(LeafProperties.seq_objs);
                    LoadEnd();
                }
                else
                    LoadSequencer(LvlSequencer.seqJSON, LeafProperties);
            }
        }
        private void RenderForm()
        {
            if (SaveOnlyNoLoad)
                return;

            dockPanel1.Theme = TCLE.DockTheme;
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            contentMain.Controls.Add(splitContainerLeafSide);
            splitContainerLeafSide.Dock = DockStyle.Fill;
            contentObjects.Controls.Add(panelObjects);
            panelObjects.Dock = DockStyle.Fill;
            contentPropertyGrid.Controls.Add(propertyGridLeaf);
            propertyGridLeaf.Dock = DockStyle.Fill;
            //
            try {
                dockPanel1.LoadFromXml($@"{TCLE.AppLocation}\settings\layout_leaf.config", m_deserializeDockContent);
            } catch {
                contentMain.Show(dockPanel1, DockState.Document);
                contentObjects.Show(contentMain.Pane, DockAlignment.Left, 0.13);
                contentPropertyGrid.Show(contentObjects.Pane, DockAlignment.Bottom, 0.5);
            }
            //
            leaftoolsToolStrip.Renderer = new ToolStripOverride();
            leafToolStrip.Renderer = new ToolStripOverride();
            contextMenuInterps.Renderer = new ContextMenuColors();
            trackEditor.MouseWheel += new MouseEventHandler(trackEditor_MouseWheel);
            TCLE.DoubleBufferDGV(trackEditor);
            textEditor.Language = FastColoredTextBoxNS.Text.Language.JSON;
            //
            treeObjects.Tag = txtSearch.Text;
            SeqObjTreeBuilder.FilterTree(treeObjects, txtSearch.Text);
            //
            trackZoom.Value = Properties.Settings.Default.ZoomHoriz;
            trackZoomVert.Value = Properties.Settings.Default.ZoomVert;
            splitContainerLeafSide.SplitterDistance = splitContainerLeafSide.Height - 60;
            splitContainerLeafSide.Panel2Collapsed = Properties.Settings.Default.LeafHideRaw;
            //
            btnLeafAutoPlace.Checked = Properties.Settings.Default.LeafOptionAutoPlace;
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
            if (SaveOnlyNoLoad)
                return;
            this.BackColor = Properties.Settings.Default.ColorLeafBG;
            trackEditor.BackgroundColor = Properties.Settings.Default.ColorLeafSeqBG;
            textEditor.BackColor = Properties.Settings.Default.ColorLeafRawBG;
            textEditor.ForeColor = Properties.Settings.Default.ColorLeafRawText;

            foreach (var _ColorIcon in TCLE.ColorIcons)
                treeObjects.ImageList.Images.Add(_ColorIcon.Key, _ColorIcon.Value);
        }

        private void dockPanel1_ActiveContentChanged(object sender, EventArgs e)
        {
            dockPanel1.SaveAsXml($@"{TCLE.AppLocation}\settings\layout_leaf.config");
        }
        #endregion

        #region Variables
        //Static
        public static int FrozenColumnOffset = 3;
        private static int IconWidth = 16;
        private static int IconHeight = 16;
        private static SolidBrush CellPaintingPen = new(Color.FromArgb(60, 60, 60));
        private static SolidBrush CellPaintingPenBright = new(Color.FromArgb(100, 100, 100));
        private static SolidBrush CellPaintingColor = new(Color.Black);
        //private static Pen PenCorn = new(BrushCorn, 3);
        //private static Pen PenRed = new(BrushRed, 3);
        //private static Pen PenGreen = new(BrushGreen, 3);
        //private static Pen PenVioletThick = new(new SolidBrush(Color.Violet), 3);
        private static Pen PenVioletThin = new(new SolidBrush(Color.Violet), 1);
        private static Pen PenWhite = new(new SolidBrush(Color.White), 3);
        //
        //Local basic vars
        private bool SaveOnlyNoLoad;
        public bool EditorIsSaved = true;
        public bool EditorIsLoading;
        private bool EditorIsRandomizing;
        private bool EditorIsMoving;
        private bool EditorIsFinding;
        private bool EditorIsPasting;
        private bool EditorIsInterpolating;
        private bool EditorIsTuning;
        private bool EditorIsProcessing => (EditorIsLoading || EditorIsRandomizing || EditorIsMoving || EditorIsFinding || EditorIsPasting || EditorIsInterpolating || EditorIsTuning);
        private bool LogUndo = true;
        private bool GlobalMute;
        private bool GlobalDisable;
        private bool GlobalExpand;
        private bool ZoomHasChanged;
        private bool ResetRowAfterEdit;
        private bool RightclickDown;
        private bool RightclickChanges;
        private bool PlaybackLoop;
        private bool RowPrePainting;
        private bool RowPostPrePainting;
        private int CurrentRow;
        private int MouseCurrentColumn;
        private int LastRowEdit;
        private int LastColumnEdit;
        private int PlaybackStart = -2;
        private int PlaybackEnd = -2;
        private string RowPrePaintError;
        private DataGridViewCell HoverCell;
        //
        //Local custom class vars
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
        private List<Sequencer_Object> SequencerObjects { get => LeafProperties?.seq_objs; set => LeafProperties.seq_objs = value; }
        public List<SaveState> UndoList = new();
        private List<int> SelectedRows = new();
        private List<SeqDataPoint> SelectedDPs = new();
        private DeserializeDockContent m_deserializeDockContent;
        public DockContentEx contentPropertyGrid = new() {
            TabText = "Properties",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
        public DockContentEx contentMain = new() {
            TabText = "Sequencer",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
        public DockContentEx contentObjects = new() {
            TabText = "Objects",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
        #endregion

        private IDockContent GetContentFromPersistString(string persistString)
        {
            persistString = persistString.Split(';')[1];
            if (persistString is "Properties")
                return contentPropertyGrid;
            if (persistString is "Objects")
                return contentObjects;
            if (persistString is "Sequencer")
                return contentMain;

            throw new NotImplementedException();
        }

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
            int display = trackEditor.FirstDisplayedScrollingColumnIndex;
            foreach (DataGridViewColumn dgvc in Columns) {
                dgvc.Width = trackZoom.Value;
            }
            if (trackEditor.ColumnCount > 1 && display != -1 && display + 1 < trackEditor.ColumnCount - 1) {
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
            int display = trackEditor.FirstDisplayedScrollingRowIndex;
            foreach (DataGridViewRow dgvr in trackEditor.Rows) {
                dgvr.Height = trackZoomVert.Value;
            }
            if (trackEditor.RowCount > 1 && display != -1) {
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
                    trackEditor.Invalidate();
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
            if (trackEditor.FirstDisplayedScrollingRowIndex != -1 && trackEditor.Rows[e.NewValue].Visible == true)
                trackEditor.FirstDisplayedScrollingRowIndex = e.NewValue;
        }
        #endregion
        #region Trackeditor Painting
        private void trackEditor_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.Handled = true;
            //we enter this specifically after all the other row prepainting is done, so this ends up on top.
            if (RowPostPrePainting) {
                //paint the frozen column squares and their icons
                if (e.ColumnIndex < FrozenColumnOffset) {
                    CellPaintFancy(e);
                    CellPaintIcons(e);
                }
                //draw a vertical line inside tuning layer row to show where selected cell is.
                else {
                    if (trackEditor[e.ColumnIndex, e.RowIndex] == trackEditor.CurrentCell)
                        e.Graphics.DrawLine(PenVioletThin, e.CellBounds.Left + (e.CellBounds.Width / 2), e.CellBounds.Top, e.CellBounds.Left + (e.CellBounds.Width / 2), e.CellBounds.Bottom);
                }
                return;
            }

            //e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.SelectionBackground | DataGridViewPaintParts.ContentBackground);
            e.Graphics.FillRectangle(new SolidBrush(e.CellStyle.BackColor), new Rectangle(e.CellBounds.Left - 1, e.CellBounds.Top, e.CellBounds.Width + 2, e.CellBounds.Height));
            //if we're in the frozen columns or header row (-1), return after this block as there's no other special drawing to be done
            if (e.ColumnIndex < FrozenColumnOffset) {
                CellPaintFancy(e);
                CellPaintIcons(e);
                return;
            }
            if (e.RowIndex == -1) {
                //draw column headers (beat #s)
                CellPainting.DrawCellValues(e, trackEditor, SequencerObjects);
                //Drawing the playback heads, start and end point triangles that exist in the header row
                CellPainting.DrawPlaybackHeaders(e, PlaybackStart, PlaybackEnd, PlaybackLoop);
                return;
            }

            CellPainting.DrawValues(e, trackEditor, SequencerObjects);
            CellPainting.DrawInterpEase(e, SequencerObjects);
            //specifically paint border seperately so it appears above everything and cleans up edges a bit.
            CellPainting.SetCellBorders(e, trackEditor, SequencerObjects);
            e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~(DataGridViewPaintParts.ContentForeground | DataGridViewPaintParts.Border | DataGridViewPaintParts.Background | DataGridViewPaintParts.SelectionBackground | DataGridViewPaintParts.ContentBackground));
            e.Paint(e.CellBounds, DataGridViewPaintParts.Border);
            //Painting playback head and end
            CellPainting.DrawPlaybackBars(e, PlaybackStart, PlaybackEnd, PlaybackLoop, LoadedLeaf.Name);
            //This block handles font scaling to draw the value in the cell bigger/smaller
            CellPainting.DrawCellValues(e, trackEditor, SequencerObjects);
        }

        private void CellPaintIcons(DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1 && SequencerObjects[e.RowIndex].obj_name == "_TuningLayerX" && e.ColumnIndex is 1 or 2)
                return;
            //get dimensions
            int x = e.CellBounds.Left + ((e.CellBounds.Width - IconWidth) / 2);
            int y = e.CellBounds.Top + ((e.CellBounds.Height - IconHeight) / 2);
            //paint the image
            //Object Toggle
            if (e.ColumnIndex == 0) {
                if (e.RowIndex == -1) {
                    e.Graphics.DrawImage(GlobalDisable ? Properties.Resources.icon_toggle_off : Properties.Resources.icon_toggle_on, new Rectangle(x, y, IconWidth, IconHeight));
                }
                else {
                    e.Graphics.DrawImage(SequencerObjects[e.RowIndex].enabled ? Properties.Resources.icon_toggle_on : Properties.Resources.icon_toggle_off, new Rectangle(x, y, IconWidth, IconHeight));
                    trackEditor[e.ColumnIndex, e.RowIndex].Selected = false;
                }
            }
            //Audio Mute/Unmute
            else if (e.ColumnIndex == 1) {
                if (e.RowIndex == -1) {
                    e.Graphics.DrawImage(GlobalMute ? Properties.Resources.icon_audio_mute : Properties.Resources.icon_audio, new Rectangle(x, y, IconWidth, IconHeight));
                }
                else {
                    e.Graphics.DrawImage(SequencerObjects[e.RowIndex].mute ? Properties.Resources.icon_audio_mute : Properties.Resources.icon_audio, new Rectangle(x, y, IconWidth, IconHeight));
                    trackEditor[e.ColumnIndex, e.RowIndex].Selected = false;
                }
            }
            //Lane Expand
            else if (e.ColumnIndex == 2) {
                if (e.RowIndex == -1)
                    e.Graphics.DrawImage(Properties.Settings.Default.LeafOptionShowLane ? Properties.Resources.icon_lanesgray : Properties.Resources.icon_lanes, new Rectangle(x, y, IconWidth, IconHeight));
                else if (SequencerObjects[e.RowIndex].friendly_lane == "lane center") {
                    e.Graphics.DrawImage(Properties.Settings.Default.LeafOptionShowLane ? Properties.Resources.icon_lanesgray : Properties.Resources.icon_lanes, new Rectangle(x, y, IconWidth, IconHeight));
                    trackEditor[e.ColumnIndex, e.RowIndex].Selected = false;
                }
            }
        }

        Font TuningFont = new("Consolas", 8);
        ///Paints rounded rectangles for the frozen columns
        private void CellPaintFancy(DataGridViewCellPaintingEventArgs e)
        {
            //skip header row
            if (e.RowIndex == -1)
                return;
            Rectangle bounds = e.CellBounds;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //column -1 is row headers
            if (e.ColumnIndex is -1) {
                e.Graphics.FillRectangle(Brushes.Black, e.CellBounds);
                CellPaintingColor.Color = TCLE.Blend(SequencerObjects[e.RowIndex].highlight_color, Color.Black, 0.4);
                bounds.X += 2;
                bounds.Y += 2;
                bounds.Width -= 4;
                bounds.Height -= 4;
                //Tuning Layers get an indent
                if (SequencerObjects[e.RowIndex].obj_name == "_TuningLayerX") {
                    bounds.X += 20;
                    bounds.Width -= 20;
                }
                //if row has a selected cell, highlight it, using a brighter color and white outline
                if (SelectedRows.Contains(e.RowIndex)) {
                    e.Graphics.FillRoundedRectangle(Brushes.White, new Rectangle(bounds.X - 1, bounds.Y - 1, bounds.Width + 2, bounds.Height + 2), 5);
                    CellPaintingColor.Color = TCLE.Blend(SequencerObjects[e.RowIndex].highlight_color, Color.Black, 0.8);
                }
                e.Graphics.FillRoundedRectangle(CellPaintingColor, bounds, 5);
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
            }
            //colums 0 and 1 are Enable and Mute
            else if (e.ColumnIndex is 0 or 1) {
                bounds.X += 1;
                bounds.Y += 1;
                bounds.Width -= 2;
                bounds.Height -= 2;
                e.Graphics.FillRectangle(Brushes.Black, e.CellBounds);
                e.Graphics.FillRoundedRectangle(trackEditor[e.ColumnIndex, e.RowIndex] == HoverCell ? CellPaintingPenBright : CellPaintingPen, bounds, 4);
            }
            //column 2 is lanes buttons
            //special painting has to be done to make the button appear connected across 5 rows.
            else if (e.ColumnIndex is 2) {
                e.Graphics.FillRectangle(Brushes.Black, e.CellBounds);
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
                    e.Graphics.FillRoundedRectangle(trackEditor[e.ColumnIndex, e.RowIndex] == HoverCell ? CellPaintingPenBright : CellPaintingPen, bounds, 4);
            }
        }
        /*
        protected override void OnPaint(PaintEventArgs e)
        {
            if (RowPrePainting)
                return;
            base.OnPaint(e);
        }
        */
        private void trackEditor_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            //setting handled True prevents the app from performing any drawing automatically.
            //I get to handle it all, in the order I need
            e.Handled = true;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            RowPrePaintError = null;
            #region PLAY SAMPLE WAVEFORMS
            if (SequencerObjects[e.RowIndex].category == "PLAY SAMPLE" && TCLE.Instance.leafoptionShowWave.Checked) {
                RowPrePainting = true;
                e.PaintCells(e.RowBounds, e.PaintParts);
                RowPrePainting = false;

                if (!SequencerObjects[e.RowIndex].Cells.Cast<SeqDataPoint>().Any(x => x.Value != null)) {
                    goto paintheader;
                }
                //setup variables to reference later when needed
                int offsetportion = (trackEditor.Columns[3].Width - trackEditor.FirstDisplayedScrollingColumnHiddenWidth) + trackEditor.RowHeadersWidth + (trackEditor.Columns[0].Width * 3) + 4;
                int columnindex = trackEditor.FirstDisplayedScrollingColumnIndex - FrozenColumnOffset + 1;
                Sequencer_Object seqref = SequencerObjects[e.RowIndex];
                SampleData samp = TCLE.ProjectSamples.FirstOrDefault(x => x.obj_name == seqref.obj_name);
                if (samp == null) {
                    if (!SequencerObjects[e.RowIndex].HasShownError) {
                        RowPrePaintError = $@"{SequencerObjects[e.RowIndex].obj_name} does not exist in any .samp file in this project. Please add it, or remove the object in this leaf.";
                        SequencerObjects[e.RowIndex].HasShownError = true;
                    }
                    goto paintheader;
                }
                //export pc file to playable file
                if (samp.wave == null) {
                    RowPrePainting = true;
                    samp.CalculateRuntime();
                    RowPrePainting = false;
                }
                //CalculateRuntime can fail. In that case, skip drawing the waveform
                if (samp.wave != null) {
                    int cellwidth = trackZoom.Value;
                    samp.wave.ColorBackground = seqref.ReadOnly ? Color.FromArgb(45, 45, 45) : seqref.highlight_color;
                    //if object has no drawn wave, create it. Wave is null whenever cell sizes change
                    if (seqref.WaveBitmap == null) {
                        Bitmap WaveToDraw = samp.wave.CreateBitmap((int)Math.Floor(cellwidth * samp.beats), e.RowBounds.Height - 4, -1, -1, true);
                        if (WaveToDraw == null)
                            goto skipwaveform;
                        /*using (Graphics graphics = Graphics.FromImage(WaveToDraw)) {
                            graphics.DrawLine(new Pen(Color.Black, 5), 0, 0, 0, WaveToDraw.Height);
                            graphics.DrawLine(new Pen(Color.Black, 5), WaveToDraw.Width, 0, WaveToDraw.Width, WaveToDraw.Height);
                        }*/
                        seqref.WaveBitmap = WaveToDraw;
                    }
                    //once the bitmap is created, now we can do some funky stuff
                    foreach (SeqDataPoint sdp in seqref.Cells.Cast<SeqDataPoint>().Where(x => x.Value != null)) {
                        if (sdp.beat > columnindex + trackEditor.DisplayedColumnCount(true) && sdp.beat + samp.beats < columnindex)
                            continue;
                        //math to offset drawing the wave horizontally based on where the active beats are
                        //e.Graphics.FillRoundedRectangle(Brushes.White, new Rectangle(((sdp.beat - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top, (int)Math.Floor(cellwidth * samp.beats), e.RowBounds.Height), 10);
                        e.Graphics.DrawImage(seqref.WaveBitmap, ((sdp.beat - columnindex) * cellwidth) + offsetportion + 3, e.RowBounds.Top + 3, (int)Math.Floor(cellwidth * samp.beats) - 6, e.RowBounds.Height - 6);
                        e.Graphics.DrawRoundedRectangle(PenWhite, new Rectangle(((sdp.beat - columnindex) * cellwidth) + offsetportion + 2, e.RowBounds.Top + 2, (int)Math.Floor(cellwidth * samp.beats) - 4, e.RowBounds.Height - 4), 10);
                    }
                }
            skipwaveform:;
                RowPostPrePainting = true;
                e.PaintCells(e.RowBounds, e.PaintParts);
                RowPostPrePainting = false;
                if (samp.message != null) {
                    RowPrePaintError = samp.message;
                    samp.message = null;
                }
            }
            #endregion
            #region OBJECTS THAT LAST LONGER THAN 1 BEAT
            else if (SequencerObjects[e.RowIndex].friendly_param.Contains('[')) {
                RowPrePainting = true;
                e.PaintCells(e.RowBounds, e.PaintParts);
                RowPrePainting = false;

                ///if (!SequencerObjects[e.RowIndex].data_points.Any(x => x.value != null))
                ///    goto paintheader;
                bool success = int.TryParse(SequencerObjects[e.RowIndex].friendly_param.Split('[')[1].Split(' ')[0], out int beats);
                beats--;
                if (!success)
                    goto paintheader;
                int offsetportion = (trackEditor.Columns[3].Width - trackEditor.FirstDisplayedScrollingColumnHiddenWidth) + trackEditor.RowHeadersWidth + (trackEditor.Columns[0].Width * 3) + 4;
                int columnindex = trackEditor.FirstDisplayedScrollingColumnIndex - FrozenColumnOffset + 1 - 1;
                Sequencer_Object seqref = SequencerObjects[e.RowIndex];
                int cellwidth = trackZoom.Value;
                Color alpha = seqref.highlight_color;
                alpha = seqref.ReadOnly ? Color.Gray : Color.FromArgb(100, alpha.R, alpha.G, alpha.B);
                //
                if (Properties.Settings.Default.LeafOptionThinBars && SequencerObjects[e.RowIndex].friendly_lane == "lane center" && SequencerObjects[e.RowIndex].expandlanes == false) {
                    int trailstop = 0;
                    foreach (SeqDataPoint sdp in SequencerObjects[e.RowIndex - 2].Cells.Cast<SeqDataPoint>().Where(x => x.Value != null)) {
                        //don't draw trail if it already has has happened from a previous one
                        if (sdp.beat > columnindex + trackEditor.DisplayedColumnCount(true) && sdp.beat + beats < columnindex) continue;
                        if (sdp.beat < trailstop)
                            e.Graphics.FillRectangle(new SolidBrush(alpha), ((trailstop - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top, (beats - (trailstop - sdp.beat)) * cellwidth, e.RowBounds.Height / 5);
                        else
                            e.Graphics.FillRectangle(new SolidBrush(alpha), ((sdp.beat - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top, beats * cellwidth, e.RowBounds.Height / 5);
                        trailstop = sdp.beat + beats;
                    }
                    trailstop = 0;
                    foreach (SeqDataPoint sdp in SequencerObjects[e.RowIndex - 1].Cells.Cast<SeqDataPoint>().Where(x => x.Value != null)) {
                        if (sdp.beat > columnindex + trackEditor.DisplayedColumnCount(true) && sdp.beat + beats < columnindex) continue;
                        //don't draw trail if it already has has happened from a previous one
                        if (sdp.beat < trailstop)
                            e.Graphics.FillRectangle(new SolidBrush(alpha), ((trailstop - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + e.RowBounds.Height / 5, (beats - (trailstop - sdp.beat)) * cellwidth, e.RowBounds.Height / 5);
                        else
                            e.Graphics.FillRectangle(new SolidBrush(alpha), ((sdp.beat - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + e.RowBounds.Height / 5, beats * cellwidth, e.RowBounds.Height / 5);
                        trailstop = sdp.beat + beats;
                    }
                    trailstop = 0;
                    foreach (SeqDataPoint sdp in SequencerObjects[e.RowIndex].Cells.Cast<SeqDataPoint>().Where(x => x.Value != null)) {
                        if (sdp.beat > columnindex + trackEditor.DisplayedColumnCount(true) && sdp.beat + beats < columnindex) continue;
                        //don't draw trail if it already has has happened from a previous one
                        if (sdp.beat < trailstop)
                            e.Graphics.FillRectangle(new SolidBrush(alpha), ((trailstop - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + e.RowBounds.Height / 5 * 2, (beats - (trailstop - sdp.beat)) * cellwidth, e.RowBounds.Height / 5);
                        else
                            e.Graphics.FillRectangle(new SolidBrush(alpha), ((sdp.beat - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + e.RowBounds.Height / 5 * 2, beats * cellwidth, e.RowBounds.Height / 5);
                        trailstop = sdp.beat + beats;
                    }
                    trailstop = 0;
                    foreach (SeqDataPoint sdp in SequencerObjects[e.RowIndex + 1].Cells.Cast<SeqDataPoint>().Where(x => x.Value != null)) {
                        if (sdp.beat > columnindex + trackEditor.DisplayedColumnCount(true) && sdp.beat + beats < columnindex) continue;
                        //don't draw trail if it already has has happened from a previous one
                        if (sdp.beat < trailstop)
                            e.Graphics.FillRectangle(new SolidBrush(alpha), ((trailstop - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + e.RowBounds.Height / 5 * 3, (beats - (trailstop - sdp.beat)) * cellwidth, e.RowBounds.Height / 5);
                        else
                            e.Graphics.FillRectangle(new SolidBrush(alpha), ((sdp.beat - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + e.RowBounds.Height / 5 * 3, beats * cellwidth, e.RowBounds.Height / 5);
                        trailstop = sdp.beat + beats;
                    }
                    trailstop = 0;
                    foreach (SeqDataPoint sdp in SequencerObjects[e.RowIndex + 2].Cells.Cast<SeqDataPoint>().Where(x => x.Value != null)) {
                        if (sdp.beat > columnindex + trackEditor.DisplayedColumnCount(true) && sdp.beat + beats < columnindex) continue;
                        //don't draw trail if it already has has happened from a previous one
                        if (sdp.beat < trailstop)
                            e.Graphics.FillRectangle(new SolidBrush(alpha), ((trailstop - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + e.RowBounds.Height / 5 * 4, (beats - (trailstop - sdp.beat)) * cellwidth, e.RowBounds.Height / 5);
                        else
                            e.Graphics.FillRectangle(new SolidBrush(alpha), ((sdp.beat - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + e.RowBounds.Height / 5 * 4, beats * cellwidth, e.RowBounds.Height / 5);
                        trailstop = sdp.beat + beats;
                    }
                }
                else {
                    int trailstop = 0;
                    foreach (SeqDataPoint sdp in seqref.Cells.Cast<SeqDataPoint>().Where(x => x.Value != null)) {
                        //don't draw trail if it already has has happened from a previous one
                        if (sdp.beat > columnindex + trackEditor.DisplayedColumnCount(true) && sdp.beat + beats < columnindex) continue;
                        if (sdp.beat < trailstop)
                            e.Graphics.FillRectangle(new SolidBrush(alpha), ((trailstop - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + 2, (beats - (trailstop - sdp.beat)) * cellwidth, e.RowBounds.Height - 4);
                        else
                            e.Graphics.FillRectangle(new SolidBrush(alpha), ((sdp.beat - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + 2, beats * cellwidth, e.RowBounds.Height - 4);
                        trailstop = sdp.beat + beats;
                    }
                }
                RowPostPrePainting = true;
                e.PaintCells(e.RowBounds, e.PaintParts);
                RowPostPrePainting = false;
            }
            #endregion
            # region TUNINGLAYER GRAPHS
            else if (SequencerObjects[e.RowIndex].obj_name == "_TuningLayerX") {
                RowPrePainting = true;
                e.PaintCells(e.RowBounds, e.PaintParts);
                RowPrePainting = false;

                if (!SequencerObjects[e.RowIndex].Cells.Cast<SeqDataPoint>().Any(x => x.Value != null)) {
                    goto paintheader;
                }
                Sequencer_Object seqref = SequencerObjects[e.RowIndex];
                //skip drawing graphs if object disabled
                if (!seqref.enabled)
                    goto paintheader;
                List<SeqDataPoint> _datapoints = seqref.Cells.Cast<SeqDataPoint>().Where(x => x.Value != null).ToList();
                if (_datapoints.Count < 1)
                    goto paintheader;
                //setup variables to reference later when needed
                int offsetportion = (trackEditor.Columns[3].Width - trackEditor.FirstDisplayedScrollingColumnHiddenWidth) + trackEditor.RowHeadersWidth + (trackEditor.Columns[0].Width * 3) + 4;
                int columnindex = trackEditor.FirstDisplayedScrollingColumnIndex - FrozenColumnOffset + 1;
                int cellwidth = trackZoom.Value;
                float max = (float)_datapoints.Max(x => (decimal)x.Value);
                float min = (float)_datapoints.Min(x => (decimal)x.Value);
                if (max == min) {
                    max++; min--;
                }
                int startX = ((_datapoints[0].beat - columnindex) * cellwidth) + offsetportion;
                int length = cellwidth * (_datapoints[^1].beat - _datapoints[0].beat + 1);
                int endX = ((_datapoints[^1].beat - columnindex + 1) * cellwidth) + offsetportion;
                PointF[] _drawingpoints = _datapoints.Select(p => new PointF(ConvertRange(_datapoints[0].beat, _datapoints[^1].beat, startX, endX - cellwidth, p.beat) + cellwidth / 2, ConvertRange(min, max, e.RowBounds.Bottom - 7, e.RowBounds.Top + 7, (float)(decimal)p.Value))).ToArray();
                //
                e.Graphics.FillRoundedRectangle(Brushes.White, new(startX, e.RowBounds.Top, length, e.RowBounds.Height), 10);
                e.Graphics.FillRoundedRectangle(new SolidBrush(Properties.Settings.Default.ColorTuningBG), new(startX + 2, e.RowBounds.Top + 2, length - 4, e.RowBounds.Height - 4), 10);
                e.Graphics.DrawLine(new(Properties.Settings.Default.ColorTuningMaxMin, 1), startX + 3, e.RowBounds.Top + 7, endX - 3, e.RowBounds.Top + 7);
                e.Graphics.DrawLine(new(Properties.Settings.Default.ColorTuningMaxMin, 1), startX + 3, e.RowBounds.Bottom - 7, endX - 3, e.RowBounds.Bottom - 7);
                e.Graphics.DrawLine(new(Properties.Settings.Default.ColorTuningMaxMin, 1), startX + 3, e.RowBounds.Top + e.RowBounds.Height / 2, endX - 3, e.RowBounds.Top + e.RowBounds.Height / 2);
                e.Graphics.DrawString($"{max}", TuningFont, new SolidBrush(Properties.Settings.Default.ColorTuningFont), startX + 3, e.RowBounds.Top + 8);
                e.Graphics.DrawString($"{min}", TuningFont, new SolidBrush(Properties.Settings.Default.ColorTuningFont), startX + 3, e.RowBounds.Bottom - 15);
                PointF midpoint = new();
                PointF midpoint2 = new();
                Pen TuningLine = new(Properties.Settings.Default.ColorTuningLine, 3);
                SolidBrush TuningPoint = new SolidBrush(Properties.Settings.Default.ColorTuningPoint);
                //
                for (int x = 0; x < _datapoints.Count; x++) {
                    if (x == _datapoints.Count - 1) {
                        e.Graphics.FillRectangle(TuningPoint, _drawingpoints[x].X - 4, _drawingpoints[x].Y - 4, 9, 9);
                        continue;
                    }
                    float distance = _drawingpoints[x + 1].X - _drawingpoints[x].X;
                    switch ($"{_datapoints[x].Interpolation} {_datapoints[x].Ease}") {
                        case "Step Ease In":
                        case "Step Ease Out":
                        case "Step Ease In Out":
                            e.Graphics.DrawLine(TuningLine, _drawingpoints[x], new(_drawingpoints[x + 1].X, _drawingpoints[x].Y));
                            e.Graphics.DrawLine(TuningLine, new(_drawingpoints[x + 1].X, _drawingpoints[x].Y), _drawingpoints[x + 1]);
                            break;
                        case "Linear Ease In":
                        case "Linear Ease Out":
                        case "Linear Ease In Out":
                            midpoint = new PointF(_drawingpoints[x].X, _drawingpoints[x].Y);
                            midpoint2 = new PointF(_drawingpoints[x + 1].X, _drawingpoints[x + 1].Y);
                            break;
                        case "Quadratic Ease In":
                            midpoint = new PointF(_drawingpoints[x].X + (distance * 0.11f), _drawingpoints[x].Y);
                            midpoint2 = new PointF(_drawingpoints[x].X + (distance * 0.5f), _drawingpoints[x].Y);
                            break;
                        case "Quadratic Ease Out":
                            midpoint = new PointF(_drawingpoints[x].X + (distance * 0.5f), _drawingpoints[x + 1].Y);
                            midpoint2 = new PointF(_drawingpoints[x].X + (distance * 0.89f), _drawingpoints[x + 1].Y);
                            break;
                        case "Quadratic Ease In Out":
                            midpoint = new PointF(_drawingpoints[x].X + (distance * 0.45f), _drawingpoints[x].Y);
                            midpoint2 = new PointF(_drawingpoints[x].X + (distance * 0.55f), _drawingpoints[x + 1].Y);
                            break;
                        case "Cubic Ease In":
                            midpoint = new PointF(_drawingpoints[x].X + (distance * 0.32f), _drawingpoints[x].Y);
                            midpoint2 = new PointF(_drawingpoints[x].X + (distance * 0.67f), _drawingpoints[x].Y);
                            break;
                        case "Cubic Ease Out":
                            midpoint = new PointF(_drawingpoints[x].X + (distance * 0.33f), _drawingpoints[x + 1].Y);
                            midpoint2 = new PointF(_drawingpoints[x].X + (distance * 0.68f), _drawingpoints[x + 1].Y);
                            break;
                        case "Cubic Ease In Out":
                            midpoint = new PointF(_drawingpoints[x].X + (distance * 0.65f), _drawingpoints[x].Y);
                            midpoint2 = new PointF(_drawingpoints[x].X + (distance * 0.35f), _drawingpoints[x + 1].Y);
                            break;
                        case "Quartic Ease In":
                            midpoint = new PointF(_drawingpoints[x].X + (distance * 0.5f), _drawingpoints[x].Y);
                            midpoint2 = new PointF(_drawingpoints[x].X + (distance * 0.75f), _drawingpoints[x].Y);
                            break;
                        case "Quartic Ease Out":
                            midpoint = new PointF(_drawingpoints[x].X + (distance * 0.25f), _drawingpoints[x + 1].Y);
                            midpoint2 = new PointF(_drawingpoints[x].X + (distance * 0.5f), _drawingpoints[x + 1].Y);
                            break;
                        case "Quartic Ease In Out":
                            midpoint = new PointF(_drawingpoints[x].X + (distance * 0.76f), _drawingpoints[x].Y);
                            midpoint2 = new PointF(_drawingpoints[x].X + (distance * 0.24f), _drawingpoints[x + 1].Y);
                            break;
                        case "Quintic Ease In":
                            midpoint = new PointF(_drawingpoints[x].X + (distance * 0.64f), _drawingpoints[x].Y);
                            midpoint2 = new PointF(_drawingpoints[x].X + (distance * 0.78f), _drawingpoints[x].Y);
                            break;
                        case "Quintic Ease Out":
                            midpoint = new PointF(_drawingpoints[x].X + (distance * 0.22f), _drawingpoints[x + 1].Y);
                            midpoint2 = new PointF(_drawingpoints[x].X + (distance * 0.36f), _drawingpoints[x + 1].Y);
                            break;
                        case "Quintic Ease In Out":
                            midpoint = new PointF(_drawingpoints[x].X + (distance * 0.83f), _drawingpoints[x].Y);
                            midpoint2 = new PointF(_drawingpoints[x].X + (distance * 0.17f), _drawingpoints[x + 1].Y);
                            break;
                        case "Sine Ease In":
                            midpoint = new PointF(_drawingpoints[x].X + (distance * 0.12f), _drawingpoints[x].Y);
                            midpoint2 = new PointF(_drawingpoints[x].X + (distance * 0.39f), _drawingpoints[x + 1].Y);
                            break;
                        case "Sine Ease Out":
                            midpoint = new PointF(_drawingpoints[x].X + (distance * 0.61f), _drawingpoints[x + 1].Y);
                            midpoint2 = new PointF(_drawingpoints[x].X + (distance * 0.88f), _drawingpoints[x + 1].Y);
                            break;
                        case "Sine Ease In Out":
                            midpoint = new PointF(_drawingpoints[x].X + (distance * 0.37f), _drawingpoints[x].Y);
                            midpoint2 = new PointF(_drawingpoints[x].X + (distance * 0.63f), _drawingpoints[x + 1].Y);
                            break;
                    }
                    if (!_datapoints[x].Interpolation.Contains("step", StringComparison.OrdinalIgnoreCase) && _datapoints[x].Interpolation != "None")
                        e.Graphics.DrawBezier(TuningLine, _drawingpoints[x], midpoint, midpoint2, _drawingpoints[x + 1]);
                    e.Graphics.FillRectangle(TuningPoint, _drawingpoints[x].X - 4, _drawingpoints[x].Y - 4, 9, 9);
                }
                //
                RowPostPrePainting = true;
                e.PaintCells(e.RowBounds, e.PaintParts);
                RowPostPrePainting = false;
            }
            #endregion
            #region Paint Anything Else
            else {
                e.PaintCells(e.RowBounds, DataGridViewPaintParts.All);
            }
        #endregion
        paintheader:
            RowPrePainting = true;
            e.PaintHeader(true);
            RowPrePainting = false;
            if (RowPrePaintError != null) {
                MessageBox.Show(RowPrePaintError, "Lumper Eustum Tevel Cditor");
            }
        }
        #endregion

        private void trackEditor_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            LastRowEdit = e.RowIndex;
            LastColumnEdit = e.ColumnIndex;
            //ResetRowAfterEdit = true;
        }

        private void trackEditor_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //I think this doesn't have a purpose anymore
        }

        private void trackEditor_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (EditorIsProcessing)
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
            if (trackEditor.SelectedCells.Count == 0 || trackEditor.SelectedCells[^1].ColumnIndex < FrozenColumnOffset)
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
            //get all selected cells and display them grouped together in the propertygrid
            //this allows for mass editing
            trackEditor.Invalidate();
            SelectedDPs.Clear();
            foreach (DataGridViewCell dgvc in trackEditor.SelectedCells) {
                //check if index out of bounds
                if (dgvc.ColumnIndex < FrozenColumnOffset)
                    continue;
                SelectedDPs.Add(SequencerObjects[dgvc.RowIndex][dgvc.ColumnIndex]);
            }
            //update the properties panel to show the selected object
            LeafProperties.selectedobj = SequencerObjects[trackEditor.SelectedCells[^1].RowIndex];
            TCLE.dockProjectProperties.propertyGridProject.SelectedObject = GetProperties();
            TCLE.dockProjectProperties.propertyGridProject.Refresh();
            propertyGridLeaf.SelectedObjects = SelectedDPs.ToArray();
            propertyGridLeaf.Refresh();
        }

        private void trackEditor_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewTextBoxEditingControl tb = (DataGridViewTextBoxEditingControl)e.Control;
            //setup keypress events for the cells in editing mode.
            tb.PreviewKeyDown += new PreviewKeyDownEventHandler(cellEditingKeyPress);
            e.Control.PreviewKeyDown += new PreviewKeyDownEventHandler(cellEditingKeyPress);
        }

        private void cellEditingKeyPress(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ResetRowAfterEdit = true;
            //if (trackEditor.CurrentCell.RowIndex == trackEditor.RowCount - 1)
            //trackEditor_SelectionChanged(null, null);
        }
        //Row changed
        private void trackEditor_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (EditorIsProcessing)
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
                CellValueChanged(trackEditor[e.ColumnIndex, e.RowIndex]);
            }
        }
        public void CellValueChanged(DataGridViewCell StartCell, bool setnull = false)
        {
            //If certain actions going on, don't bother running this method.
            if (EditorIsProcessing) return;
            EditorIsLoading = true;

            bool _changes = false;
            object _val = null;
            if (!setnull && Decimal.TryParse(StartCell.EditedFormattedValue?.ToString(), out decimal _valtoset))
                _val = TCLE.TruncateDecimal(_valtoset, 3);

            List<DataGridViewCell> CellsToChange = new();
            if (StartCell.Selected)
                CellsToChange = trackEditor.SelectedCells.Cast<DataGridViewCell>().ToList();
            else
                CellsToChange.Add(StartCell);

            foreach (DataGridViewCell _cell in CellsToChange) {
                //skip readonly and hidden cells
                if (_cell.ReadOnly || !_cell.OwningRow.Visible)
                    continue;

                _cell.Value = _val;
            }

            EditorIsLoading = false;
            SaveCheckAndWrite(false, "Cell Value(s) Updated");
            ShowRawTrackData(SequencerObjects[StartCell.RowIndex]);            
        }

        private void CellValueNull(DataGridViewCell _cell)
        {
            SequencerObjects[_cell.RowIndex][_cell.ColumnIndex].Value = null;
            SequencerObjects[_cell.RowIndex][_cell.ColumnIndex].Interpolation = "Linear";
            SequencerObjects[_cell.RowIndex][_cell.ColumnIndex].Ease = "Ease In Out";

            if (SequencerObjects[_cell.RowIndex].expandlanes == false && SequencerObjects[_cell.RowIndex].friendly_lane == "lane center") {
                SequencerObjects[_cell.RowIndex - 2][_cell.ColumnIndex].Value = null;
                SequencerObjects[_cell.RowIndex - 2][_cell.ColumnIndex].Interpolation = "Linear";
                SequencerObjects[_cell.RowIndex - 2][_cell.ColumnIndex].Ease = "Ease In Out";
                SequencerObjects[_cell.RowIndex - 1][_cell.ColumnIndex].Value = null;
                SequencerObjects[_cell.RowIndex - 1][_cell.ColumnIndex].Interpolation = "Linear";
                SequencerObjects[_cell.RowIndex + 1][_cell.ColumnIndex].Ease = "Ease In Out";
                SequencerObjects[_cell.RowIndex + 1][_cell.ColumnIndex].Value = null;
                SequencerObjects[_cell.RowIndex + 1][_cell.ColumnIndex].Interpolation = "Linear";
                SequencerObjects[_cell.RowIndex + 1][_cell.ColumnIndex].Ease = "Ease In Out";
                SequencerObjects[_cell.RowIndex + 2][_cell.ColumnIndex].Value = null;
                SequencerObjects[_cell.RowIndex + 2][_cell.ColumnIndex].Interpolation = "Linear";
                SequencerObjects[_cell.RowIndex + 2][_cell.ColumnIndex].Ease = "Ease In Out";
            }
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
                    RowReadOnly(seq, !seq.enabled);
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
                            PlaybackEnd = -2;
                            trackEditor.Invalidate();
                        }
                        else {
                            PlaybackLoop = true;
                            trackEditor.Invalidate();
                        }
                    }
                    else {
                        PlaybackEnd = e.ColumnIndex - FrozenColumnOffset;
                        if (PlaybackEnd < PlaybackStart)
                            PlaybackEnd = PlaybackStart;
                        trackEditor.Invalidate();
                    }
                }
                else {
                    if (PlaybackStart == e.ColumnIndex - FrozenColumnOffset) {
                        PlaybackStart = -2;
                        trackEditor.Invalidate();
                    }
                    else {
                        PlaybackStart = e.ColumnIndex - FrozenColumnOffset;
                        if (PlaybackEnd != -2 && PlaybackEnd <= PlaybackStart)
                            PlaybackEnd = PlaybackStart;
                        trackEditor.Invalidate();
                    }
                }
                return;
            }
            //test for clicks in frozen columns
            //unselect the cells afterwards to imitate button click
            else if (e.ColumnIndex is 0 or 1 or 2) {
                Sequencer_Object seq = SequencerObjects[e.RowIndex];
                if (e.ColumnIndex is 0) {
                    seq.enabled = !seq.enabled;
                    RowReadOnly(seq, !seq.enabled);
                    TCLE.PlaySound("UIselect");
                }
                if (e.ColumnIndex is 1) {
                    seq.mute = !seq.mute;
                    TCLE.PlaySound("UIselect");
                }
                if (e.ColumnIndex is 2 && seq.friendly_lane == "lane center") {
                    //if ShowLanes, don't alter lane visibility
                    if (Properties.Settings.Default.LeafOptionShowLane)
                        return;
                    //FindMissingLaneObjects(seq);
                    seq.expandlanes = !seq.expandlanes;
                    SequencerObjects[seq.Index - 2].expandlanes = seq.expandlanes;
                    SequencerObjects[seq.Index - 1].expandlanes = seq.expandlanes;
                    SequencerObjects[seq.Index + 1].expandlanes = seq.expandlanes;
                    SequencerObjects[seq.Index + 2].expandlanes = seq.expandlanes;
                    TCLE.PlaySound("UIselect");
                }
                trackEditor[e.ColumnIndex, e.RowIndex].Selected = false;
                //invalidate cell to repaint it to update the images
                trackEditor.InvalidateCell(trackEditor[e.ColumnIndex, e.RowIndex]);
            }
            else if (e.Button == MouseButtons.Left && btnLeafAutoPlace.Checked) {
                if (SequencerObjects[e.RowIndex].trait_type is "kTraitBool" or "kTraitAction")
                    if (dgv[e.ColumnIndex, e.RowIndex].Value == null) {
                        dgv[e.ColumnIndex, e.RowIndex].Value = 1m;
                    }
            }

            if (e.ColumnIndex >= FrozenColumnOffset) {
                //leafProperties.selecteddatapoint = SequencerObjects[e.RowIndex][e.ColumnIndex - FrozenColumnOffset];
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
                    LogUndo = false;
                    CellValueNull(trackEditor[e.ColumnIndex, e.RowIndex]);
                    RightclickChanges = true;
                    LogUndo = true;
                    trackEditor.InvalidateCell(dgv[e.ColumnIndex, e.RowIndex]);
                }
                else if (dgv[e.ColumnIndex, e.RowIndex].Selected) {
                    if (dgv[e.ColumnIndex, e.RowIndex].Value == null && dgv.SelectedCells.Count == 1)
                        return;
                    LogUndo = false;
                    dgv[e.ColumnIndex, e.RowIndex].Value = null;
                    CellValueChanged(trackEditor[e.ColumnIndex, e.RowIndex]);
                    LogUndo = true;
                    trackEditor.InvalidateCell(dgv[e.ColumnIndex, e.RowIndex]);
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
                trackEditor.InvalidateRow(e.RowIndex);
            }
        }

        private void trackEditor_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            MouseCurrentColumn = e.ColumnIndex;
            if (e.ColumnIndex == -1 || e.RowIndex == -1) {
                HoverCell = null;
                return;
            }
            HoverCell = trackEditor[e.ColumnIndex, e.RowIndex];
            if (e.ColumnIndex < FrozenColumnOffset)
                trackEditor.InvalidateCell(HoverCell);

            DataGridView dgv = sender as DataGridView;
            if (e.ColumnIndex is 0 or 1 or 2) {
                dgv[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.FromArgb(174, 161, 255);
            }
            else if (Control.MouseButtons == MouseButtons.Right) {
                RightclickDown = true;
                if (dgv[e.ColumnIndex, e.RowIndex].Selected == false) {
                    LogUndo = false;
                    CellValueNull(trackEditor[e.ColumnIndex, e.RowIndex]);
                    RightclickChanges = true;
                    LogUndo = true;
                    trackEditor.InvalidateCell(dgv[e.ColumnIndex, e.RowIndex]);
                }
                else if (dgv[e.ColumnIndex, e.RowIndex].Selected == true) {
                    LogUndo = false;
                    dgv[e.ColumnIndex, e.RowIndex].Value = null;
                    CellValueChanged(trackEditor[e.ColumnIndex, e.RowIndex]);
                    LogUndo = true;
                }
            }
        }

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
                CellValueChanged(trackEditor[trackEditor.SelectedCells[^1].ColumnIndex, trackEditor.SelectedCells[^1].RowIndex], true);
                LogUndo = true;
                SaveCheckAndWrite(false, "Delete Cell Values");
            }
        }
        private void trackEditor_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //delete cell value if Delete key is pressed
            if (e.KeyCode == Keys.Delete) {
                LogUndo = false;
                CellValueChanged(trackEditor[trackEditor.SelectedCells[^1].ColumnIndex, trackEditor.SelectedCells[^1].RowIndex], true);
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
                    IOrderedEnumerable<SeqDataPoint> dgvcc;
                    if (indexdirection == -1)
                        dgvcc = trackEditor.SelectedCells.Cast<SeqDataPoint>().OrderBy(c => leftright ? c.ColumnIndex : c.RowIndex);
                    else
                        dgvcc = trackEditor.SelectedCells.Cast<SeqDataPoint>().OrderByDescending(c => leftright ? c.ColumnIndex : c.RowIndex);

                    LogUndo = false;
                    trackEditor.ClearSelection();
                    //iterate over each in the selection
                    foreach (SeqDataPoint dgvc in dgvcc) {
                        //check if at left/right edges
                        if ((leftright && dgvc.ColumnIndex + indexdirection < trackEditor.ColumnCount && dgvc.ColumnIndex + indexdirection >= FrozenColumnOffset) || (!leftright && dgvc.RowIndex + indexdirection < trackEditor.RowCount && dgvc.RowIndex + indexdirection > -1)) {
                            shifted = true;
                            //clone selected cell to new location
                            SequencerObjects[dgvc.RowIndex + (!leftright ? indexdirection : 0)][dgvc.ColumnIndex + (leftright ? indexdirection : 0)] = dgvc.Clone();
                            //select the newly moved cell
                            trackEditor[dgvc.ColumnIndex + (leftright ? indexdirection : 0), dgvc.RowIndex + (!leftright ? indexdirection : 0)].Selected = true;
                            //clear the current cell since it moved
                            SequencerObjects[dgvc.RowIndex][dgvc.ColumnIndex].Value = null;
                            SequencerObjects[dgvc.RowIndex][dgvc.ColumnIndex].Interpolation = "Linear";
                            SequencerObjects[dgvc.RowIndex][dgvc.ColumnIndex].Ease = "Ease In Out";
                        }
                        else {
                            foreach (DataGridViewCell dgvcell in dgvcc)
                                dgvcell.Selected = true;
                            break;
                        }
                    }
                    LogUndo = true;
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
                CellValueChanged(trackEditor[trackEditor.CurrentCell.ColumnIndex, trackEditor.CurrentCell.RowIndex]);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 1"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue1;
                CellValueChanged(trackEditor[trackEditor.CurrentCell.ColumnIndex, trackEditor.CurrentCell.RowIndex]);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 2"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue2;
                CellValueChanged(trackEditor[trackEditor.CurrentCell.ColumnIndex, trackEditor.CurrentCell.RowIndex]);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 3"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue3;
                CellValueChanged(trackEditor[trackEditor.CurrentCell.ColumnIndex, trackEditor.CurrentCell.RowIndex]);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 4"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue4;
                CellValueChanged(trackEditor[trackEditor.CurrentCell.ColumnIndex, trackEditor.CurrentCell.RowIndex]);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 5"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue5;
                CellValueChanged(trackEditor[trackEditor.CurrentCell.ColumnIndex, trackEditor.CurrentCell.RowIndex]);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 6"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue6;
                CellValueChanged(trackEditor[trackEditor.CurrentCell.ColumnIndex, trackEditor.CurrentCell.RowIndex]);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 7"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue7;
                CellValueChanged(trackEditor[trackEditor.CurrentCell.ColumnIndex, trackEditor.CurrentCell.RowIndex]);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 8"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue8;
                CellValueChanged(trackEditor[trackEditor.CurrentCell.ColumnIndex, trackEditor.CurrentCell.RowIndex]);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 9"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValue9;
                CellValueChanged(trackEditor[trackEditor.CurrentCell.ColumnIndex, trackEditor.CurrentCell.RowIndex]);
            }
        }

        private void trackEditor_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (ModifierKeys is not Keys.Control and not Keys.Shift && ZoomHasChanged) {
                ZoomHasChanged = false;
                foreach (Sequencer_Object seq in SequencerObjects)
                    seq.WaveBitmap = null;
                trackEditor.Invalidate();
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
            int lastpos = trackEditor.FirstDisplayedScrollingColumnIndex;
            if (trackEditor.FirstDisplayedScrollingColumnIndex == -1)
                return;

            if (ModifierKeys is Keys.Shift) {
                foreach (DataGridViewCell dgvc in trackEditor.Rows[e.RowIndex].Cells)
                    dgvc.Selected = true;
            }
            else {
                trackEditor.CurrentCell = trackEditor[trackEditor.CurrentCell.ColumnIndex < FrozenColumnOffset ? FrozenColumnOffset : trackEditor.CurrentCell.ColumnIndex, e.RowIndex];
                trackEditor.Invalidate();

                if (e.Button == MouseButtons.Right) {
                    contextMenuObj.Show(MousePosition.X, MousePosition.Y);
                    return;
                }
            }
            trackEditor.FirstDisplayedScrollingColumnIndex = lastpos;
        }

        private void contextMenuObj_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            toolstripObjTune.Enabled = SequencerObjects[trackEditor.CurrentRow.Index].trait_type == "kTraitFloat";
        }

        private void trackEditor_RowHeadersWidthChanged(object sender, EventArgs e)
        {
            _ = trackEditor.RowHeadersWidth;
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
            if (e.Node.Nodes.Count > 0 || treeObjects.SelectedNode.Nodes.Count > 0 || e.Button == MouseButtons.Right)
                return;
            Object_Params objmatch = TCLE.LeafObjects.FirstOrDefault(x => x.param_displayname == e.Node.Text);
            if (e.Node.Text.EndsWith(".samp"))
                objmatch = TCLE.LeafObjects.FirstOrDefault(x => x.category == "PLAY SAMPLE");

            if (objmatch == null)
                return;


            Sequencer_Object seq = new() {
                ParentLeaf = leafProperties,
                obj_name = objmatch.category == "PLAY SAMPLE" ? e.Node.Text : objmatch.obj_name,
                category = objmatch.category,
                param_path = objmatch.param_path,
                friendly_param = objmatch.param_displayname,
                defaultvalue = float.Parse(objmatch.def),
                step = objmatch.step,
                trait_type = objmatch.trait_type,
                highlight_color = objmatch.defaultcolor,
                highlight_value = 0,
                footer = objmatch.footer,
                enabled = true
            };
            if (seq.obj_name == "leafname")
                seq.obj_name = LeafProperties.FilePath.Name;
            if (seq.category == "LOOP TRACK VOLUME") {
                int audiochannels = SequencerObjects.Count(x => x.category == "LOOP TRACK VOLUME");
                seq.param_path = seq.param_path.Replace("x", $"{audiochannels}");
                seq.friendly_param = seq.friendly_param.Replace("x", $"{audiochannels}");
            }
            seq.expandlanes = seq.friendly_lane == "none" || Properties.Settings.Default.LeafOptionShowLane;

            if (seq.friendly_lane == "lane center") {
                LoadMultiLanes(seq, SequencerObjects);
            }
            else {
                SequencerObjects.Add(seq);
                trackEditor.Rows.Add(seq);
            }

            ChangeTrackName(seq, seq.category);
            //FindMissingLaneObjects(seq);
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
            if (e.KeyCode == Keys.Space && treeObjects.SelectedNode.Nodes.Count == 0 && treeObjects.SelectedNode.Text.EndsWith(".samp")) {
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
            treeObjects.Tag = txtSearch.Text;
            SeqObjTreeBuilder.FilterTree(treeObjects, txtSearch.Text);
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
            SeqObjTreeBuilder.BuildTreeFavorites(SeqObjTreeBuilder.GlobalObjectTree, "");
            SeqObjTreeBuilder.FilterTree(treeObjects, txtSearch.Text);
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
                SeqObjTreeBuilder.BuildTreeFavorites(SeqObjTreeBuilder.GlobalObjectTree, "");
            }
            else {
                TCLE.ObjectFavorites.RemoveWhere(x => x.param_displayname == find);
                treeObjects.SelectedNode.Remove();
                TreeNode node = SeqObjTreeBuilder.FindNode(find, treeObjects.Nodes);
                if (node != null) {
                    node.SelectedImageKey = "none";
                    node.ImageKey = "none";
                    node.ContextMenuStrip = contextMenuFav;
                }
            }
            SeqObjTreeBuilder.FilterTree(treeObjects, txtSearch.Text);
            TCLE.PlaySound("UIselect");
        }

        private void toolStripFavClear_Click(object sender, EventArgs e)
        {
            TCLE.ObjectFavorites.Clear();
            TCLE.PlaySound("UIdelete");
            SeqObjTreeBuilder.FilterTree(treeObjects, txtSearch.Text);
        }
        #endregion

        #region Buttons
        ///         ///
        /// BUTTONS ///
        ///         ///
        private void btnTrackAdd_Click(object sender, EventArgs e)
        {
            if (treeObjects.SelectedNode.Nodes.Count > 0 || trackEditor.SelectedCells.Count == 0)
                return;
            Object_Params objmatch = TCLE.LeafObjects.FirstOrDefault(x => x.param_displayname == treeObjects.SelectedNode.Text);
            if (objmatch == null) {
                if (treeObjects.SelectedNode.Text.EndsWith(".samp")) {
                    objmatch = TCLE.LeafObjects.FirstOrDefault(x => x.category == "PLAY SAMPLE");
                }
                else
                    return;
            }
            Sequencer_Object _currentseq = SequencerObjects[CurrentRow];
            if (!objmatch.param_path.EndsWith(".ent") && _currentseq.friendly_lane != "none") {
                MessageBox.Show("Due to reasons, you cannot change a multi-lane object into a non-multi-lane object. Please just add a new object.", "Thumper Custom Level Editor");
                return;
            }
            Sequencer_Object[] Lanes = SequencerObjects.GetRange(_currentseq.Index + _currentseq.LaneOffsetFromTop, (_currentseq.friendly_lane != "none" ? 5 : 1)).ToArray();// Where(x => x.category == _currentseq.category && x.friendly_param == _currentseq.friendly_param).ToArray();
            for (int x = 0; x < Lanes.Length; x++) {
                Lanes[x].obj_name = objmatch.category == "PLAY SAMPLE" ? treeObjects.SelectedNode.Text : objmatch.obj_name;
                Lanes[x].category = objmatch.category;
                Lanes[x].param_path = objmatch.param_path;
                Lanes[x].friendly_param = objmatch.param_displayname;
                Lanes[x].trait_type = objmatch.trait_type;
                Lanes[x].footer = objmatch.footer;
                Lanes[x].highlight_color = objmatch.defaultcolor;
                if (Lanes[x].obj_name == "leafname")
                    Lanes[x].obj_name = LeafProperties.FilePath.Name;
                ChangeTrackName(Lanes[x], Lanes[x].category);
            }
            //
            if (Lanes.Length == 1 && Lanes[0].friendly_lane != "none")
                LoadMultiLanes(Lanes[0], SequencerObjects);
            //FindMissingLaneObjects(SequencerObjects[CurrentRow]);
            trackEditor.InvalidateRow(_currentseq.Index);

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
                    trackEditor.Rows.Remove(Lanes[x]);
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
                .OrderBy(cell => cell.Index);
            List<Sequencer_Object> RowsToMove = new();
            foreach (Sequencer_Object row in selectedrows) {
                if (!RowsToMove.Contains(row))
                    RowsToMove.AddRange(ReturnLanesFromName(row, row.friendly_lane));
            }
            //if already at the top, do not move up
            if (RowsToMove.FirstOrDefault().Index == 0)
                return;

            IEnumerable<DataGridViewCell> selectedcells = trackEditor.SelectedCells.Cast<DataGridViewCell>();

            for (int x = 0; x < RowsToMove.Count; x++) {
                int currentindex = RowsToMove[x].Index;
                //get the object above, and any lanes with it. We will need to move above all of them.
                Sequencer_Object ObjAbove = SequencerObjects[RowsToMove[x].Index - 1];
                int Lanes = ObjAbove.friendly_lane != "none" ? 5 : 1;
                //remove the row and object
                trackEditor.SuspendLayout();
                trackEditor.Rows.Remove(RowsToMove[x]);
                SequencerObjects.Remove(RowsToMove[x]);
                if (RowsToMove[x].friendly_lane != "none") {
                    trackEditor.Rows.Remove(RowsToMove[x + 1]);
                    SequencerObjects.Remove(RowsToMove[x + 1]);
                    trackEditor.Rows.Remove(RowsToMove[x + 2]);
                    SequencerObjects.Remove(RowsToMove[x + 2]);
                    trackEditor.Rows.Remove(RowsToMove[x + 3]);
                    SequencerObjects.Remove(RowsToMove[x + 3]);
                    trackEditor.Rows.Remove(RowsToMove[x + 4]);
                    SequencerObjects.Remove(RowsToMove[x + 4]);
                }
                //reinsert object and row at appropriate index
                SequencerObjects.Insert(currentindex - Lanes, RowsToMove[x]);
                trackEditor.Rows.Insert(currentindex - Lanes, RowsToMove[x]);
                if (RowsToMove[x].friendly_lane != "none") {
                    SequencerObjects.Insert(currentindex - Lanes + 1, RowsToMove[x + 1]);
                    trackEditor.Rows.Insert(currentindex - Lanes + 1, RowsToMove[x + 1]);
                    SequencerObjects.Insert(currentindex - Lanes + 2, RowsToMove[x + 2]);
                    trackEditor.Rows.Insert(currentindex - Lanes + 2, RowsToMove[x + 2]);
                    SequencerObjects.Insert(currentindex - Lanes + 3, RowsToMove[x + 3]);
                    trackEditor.Rows.Insert(currentindex - Lanes + 3, RowsToMove[x + 3]);
                    SequencerObjects.Insert(currentindex - Lanes + 4, RowsToMove[x + 4]);
                    trackEditor.Rows.Insert(currentindex - Lanes + 4, RowsToMove[x + 4]);
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
                    Lanes[1] = SequencerObjects[row.Index + 1];
                    Lanes[2] = SequencerObjects[row.Index + 2];
                    Lanes[3] = SequencerObjects[row.Index + 3];
                    Lanes[4] = SequencerObjects[row.Index + 4];
                    break;
                case "lane left 1":
                    Lanes[0] = SequencerObjects[row.Index - 1];
                    Lanes[1] = row;
                    Lanes[2] = SequencerObjects[row.Index + 1];
                    Lanes[3] = SequencerObjects[row.Index + 2];
                    Lanes[4] = SequencerObjects[row.Index + 3];
                    break;
                case "lane center":
                    Lanes[0] = SequencerObjects[row.Index - 2];
                    Lanes[1] = SequencerObjects[row.Index - 1];
                    Lanes[2] = row;
                    Lanes[3] = SequencerObjects[row.Index + 1];
                    Lanes[4] = SequencerObjects[row.Index + 2];
                    break;
                case "lane right 1":
                    Lanes[0] = SequencerObjects[row.Index - 3];
                    Lanes[1] = SequencerObjects[row.Index - 2];
                    Lanes[2] = SequencerObjects[row.Index - 1];
                    Lanes[3] = row;
                    Lanes[4] = SequencerObjects[row.Index + 1];
                    break;
                case "lane right 2":
                    Lanes[0] = SequencerObjects[row.Index - 4];
                    Lanes[1] = SequencerObjects[row.Index - 3];
                    Lanes[2] = SequencerObjects[row.Index - 2];
                    Lanes[3] = SequencerObjects[row.Index - 1];
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
                .Distinct()
                .OrderByDescending(cell => cell.Index);
            List<Sequencer_Object> RowsToMove = new();
            foreach (Sequencer_Object row in selectedrows) {
                if (!RowsToMove.Contains(row))
                    RowsToMove.AddRange(ReturnLanesFromName(row, row.friendly_lane).Reverse());
            }
            ///RowsToMove = RowsToMove.OrderByDescending(cell => cell.editor_row.Index).ToList();
            //if already at the bottom, do not move down
            if (RowsToMove[0].Index >= trackEditor.Rows.Count - 1)
                return;

            List<DataGridViewCell> selectedcells = trackEditor.SelectedCells.Cast<DataGridViewCell>().ToList();

            for (int x = 0; x < RowsToMove.Count; x++) {
                int currentindex = RowsToMove[x].Index;
                //get the object above, and any lanes with it. We will need to move above all of them.
                Sequencer_Object ObjBelow = SequencerObjects[RowsToMove[x].Index + 1];
                int Lanes = ObjBelow.friendly_lane != "none" ? 5 : 1;
                //remove the row and object
                trackEditor.Rows.Remove(RowsToMove[x]);
                SequencerObjects.Remove(RowsToMove[x]);
                if (RowsToMove[x].friendly_lane != "none") {
                    trackEditor.Rows.Remove(RowsToMove[x + 1]);
                    SequencerObjects.Remove(RowsToMove[x + 1]);
                    trackEditor.Rows.Remove(RowsToMove[x + 2]);
                    SequencerObjects.Remove(RowsToMove[x + 2]);
                    trackEditor.Rows.Remove(RowsToMove[x + 3]);
                    SequencerObjects.Remove(RowsToMove[x + 3]);
                    trackEditor.Rows.Remove(RowsToMove[x + 4]);
                    SequencerObjects.Remove(RowsToMove[x + 4]);
                }
                //reinsert object and row at appropriate index
                int objbelowindex = ObjBelow.Index;
                SequencerObjects.Insert(objbelowindex + Lanes, RowsToMove[x]);
                trackEditor.Rows.Insert(objbelowindex + Lanes, RowsToMove[x]);
                if (RowsToMove[x].friendly_lane != "none") {
                    SequencerObjects.Insert(objbelowindex + Lanes, RowsToMove[x + 1]);
                    trackEditor.Rows.Insert(objbelowindex + Lanes, RowsToMove[x + 1]);
                    SequencerObjects.Insert(objbelowindex + Lanes, RowsToMove[x + 2]);
                    trackEditor.Rows.Insert(objbelowindex + Lanes, RowsToMove[x + 2]);
                    SequencerObjects.Insert(objbelowindex + Lanes, RowsToMove[x + 3]);
                    trackEditor.Rows.Insert(objbelowindex + Lanes, RowsToMove[x + 3]);
                    SequencerObjects.Insert(objbelowindex + Lanes, RowsToMove[x + 4]);
                    trackEditor.Rows.Insert(objbelowindex + Lanes, RowsToMove[x + 4]);
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
                .Select(cell => cell.RowIndex)
                .Distinct()
                .Order().Select(x => SequencerObjects[x]);

            TCLE.ClipboardSequencer = new();
            foreach (Sequencer_Object copyseq in Copied) {
                if (copyseq.friendly_lane == "lane center") {
                    ///Sequencer_Object lookup = TCLE.ClipboardSequencer.FirstOrDefault(x => x.obj_name == copyseq.obj_name && x.param_path == copyseq.param_path && x.param_path_lane == copyseq.param_path_lane && x.isdefault == true);
                    //if null, no object exists in SequencerObjects yet for this object or its lanes. We'll have to make it.
                    /*if (lookup == null) {
                        TCLE.ClipboardSequencer.Add(copyseq.CloneAsDefault("a01", "lane left 2", new DataGridViewRow()));
                        TCLE.ClipboardSequencer.Add(copyseq.CloneAsDefault("a02", "lane left 1", new DataGridViewRow()));
                        TCLE.ClipboardSequencer.Add(copyseq.CloneAsDefault("ent", "lane center", new DataGridViewRow()));
                        TCLE.ClipboardSequencer.Add(copyseq.CloneAsDefault("z01", "lane right 1", new DataGridViewRow()));
                        TCLE.ClipboardSequencer.Add(copyseq.CloneAsDefault("z02", "lane right 2", new DataGridViewRow()));
                    }*/
                    TCLE.ClipboardSequencer.Add(SequencerObjects[SequencerObjects.IndexOf(copyseq) - 2].Clone());
                    TCLE.ClipboardSequencer.Add(SequencerObjects[SequencerObjects.IndexOf(copyseq) - 1].Clone());
                    TCLE.ClipboardSequencer.Add(SequencerObjects[SequencerObjects.IndexOf(copyseq)].Clone());
                    TCLE.ClipboardSequencer.Add(SequencerObjects[SequencerObjects.IndexOf(copyseq) + 1].Clone());
                    TCLE.ClipboardSequencer.Add(SequencerObjects[SequencerObjects.IndexOf(copyseq) + 2].Clone());
                    ///lookup = TCLE.ClipboardSequencer.FirstOrDefault(x => x.obj_name == copyseq.obj_name && x.param_path == copyseq.param_path && x.param_path_lane == copyseq.param_path_lane && x.isdefault == true);
                    ///int index = TCLE.ClipboardSequencer.IndexOf(lookup);
                    ///TCLE.ClipboardSequencer[index] = copyseq;
                }
                //else just add the object without needing extra lanes
                else if (copyseq.friendly_lane == "none") {
                    TCLE.ClipboardSequencer.Add(copyseq.Clone());
                }
            }

            foreach (Form_LeafEditor leaf in TCLE.Documents.Where(x => x.DockHandler.TabText.Contains(".leaf")))
                leaf.btnTrackPaste.Enabled = true;
            TCLE.PlaySound("UIkcopy");
        }

        private void btnTrackPaste_Click(object sender, EventArgs e)
        {
            int _index = trackEditor.CurrentRow?.Index ?? -1;
            //if pasting inside a multilane object, skip index down a few rows
            switch (SequencerObjects[_index].friendly_lane) {
                case "lane left 2":
                    _index += 5;
                    break;
                case "lane left 1":
                    _index += 4;
                    break;
                case "lane center":
                    _index += 3;
                    break;
                case "lane right 1":
                    _index += 2;
                    break;
                case "lane right 2":
                case "none":
                    _index += 1;
                    break;
            }
            EditorIsPasting = true;
            //add copied Sequencer_Object to main _tracks list
            foreach (Sequencer_Object _newtrack in TCLE.ClipboardSequencer) {
                Sequencer_Object clone = _newtrack.Clone(LeafProperties.beats);
                clone.ParentLeaf = leafProperties;
                clone.expandlanes = GlobalExpand;
                SequencerObjects.Insert(_index, clone);
                trackEditor.Rows.Insert(_index, clone);
                _index++;
            }

            EditorIsPasting = false;
            TCLE.PlaySound("UIkpaste");
            LogUndo = true;
            SaveCheckAndWrite(false, "Paste Objects");
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
                trackEditor.InvalidateRow(seq.Index);
            }

            LogUndo = true;
            TCLE.PlaySound("UIdataerase");
            SaveCheckAndWrite(false, "Clear Object Values");
        }

        private void btnLeafClean_Click(object sender, EventArgs e)
        {
            List<Sequencer_Object> todelete = new();
            bool del;
            int index;
            foreach (Sequencer_Object seq in SequencerObjects) {
                del = false;
                if (seq.friendly_lane is not "none" and not "lane center")
                    continue;
                index = SequencerObjects.IndexOf(seq);

                if (seq.friendly_lane == "lane center") {
                    del = CheckObjectIfEmpty(SequencerObjects[index - 2]) &
                        CheckObjectIfEmpty(SequencerObjects[index - 1]) &
                        CheckObjectIfEmpty(SequencerObjects[index]) &
                        CheckObjectIfEmpty(SequencerObjects[index + 1]) &
                        CheckObjectIfEmpty(SequencerObjects[index + 2]);
                }
                else
                    del = CheckObjectIfEmpty(seq);

                if (del) {
                    if (seq.friendly_lane == "lane center") {
                        todelete.Add(SequencerObjects[index - 2]);
                        todelete.Add(SequencerObjects[index - 1]);
                        todelete.Add(seq);
                        todelete.Add(SequencerObjects[index + 1]);
                        todelete.Add(SequencerObjects[index + 2]);
                    }
                    else
                        todelete.Add(seq);
                }
            }

            foreach (Sequencer_Object seq in todelete) {
                trackEditor.Rows.Remove(seq);
                SequencerObjects.Remove(seq);
            }

            SaveCheckAndWrite(false, "cleaned up empty objects");
            EnableLeafButtons();
        }

        public static bool CheckObjectIfEmpty(Sequencer_Object seq)
        {
            bool dodelete = true;
            if (seq.Cells.Cast<SeqDataPoint>().Any(x => x.Value != null))
                dodelete = false;

            Object_Params baseobj = TCLE.LeafObjects.FirstOrDefault(x => x.param_path == seq.param_path && x.category == seq.category);
            if (baseobj != null) {
                if (float.Parse(baseobj.def) != seq.defaultvalue)
                    dodelete = false;
            }

            return dodelete;
        }

        private void btnRawImport_Click(object sender, EventArgs e)
        {
            if (loadedleaf == null)
                return;
            try {
                TrackRawImport(SequencerObjects[CurrentRow], JObject.Parse($"{{{textEditor.Text}}}"));
                TCLE.PlaySound("UIkpaste");
            } catch (JsonReaderException ex) {
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

        private void contextMenuInterps_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void linearToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            TCLE.Instance.pictureEasing.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject($"ease_{item.Text.Replace(" ", "_")}");
            //show the image
            TCLE.Instance.pictureEasing.Visible = true;
            TCLE.Instance.pictureEasing.Location = new Point(item.Owner.Location.X + item.Owner.Width, item.Owner.Location.Y);
            TCLE.Instance.pictureEasing.BringToFront();
        }

        private void contextMenuInterps_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            TCLE.Instance.pictureEasing.Visible = false;
        }

        private void Interpolate(string interpOption)
        {
            if (interpOption == null)
                return;
            InterpLastUsed = interpOption;
            btnLeafInterpLinear.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject($"ease_{interpOption.Replace(" ", "_")}");
            btnLeafInterpLinear.ToolTipText = $"Interpolate values between 2 selected cells in the same row.\nUse the drop down to select different easing styles.\n=======\nLast Used: {interpOption}\n";
            TCLE.Instance.pictureEasing.Visible = false;
            //
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
            double max2 = 0, max3 = 0, min2 = 0, min3 = 0;
            Color startcolor = new();
            Color endcolor = new();
            if (interpobject.trait_type == "kTraitColor") {
                startcolor = Color.FromArgb((int)_start);
                endcolor = Color.FromArgb((int)_end);
                max = Math.Max(Color.FromArgb((int)_start).R, Color.FromArgb((int)_end).R);
                max2 = Math.Max(Color.FromArgb((int)_start).G, Color.FromArgb((int)_end).G);
                max3 = Math.Max(Color.FromArgb((int)_start).B, Color.FromArgb((int)_end).B);
                min = Math.Min(Color.FromArgb((int)_start).R, Color.FromArgb((int)_end).R);
                min2 = Math.Min(Color.FromArgb((int)_start).G, Color.FromArgb((int)_end).G);
                min3 = Math.Min(Color.FromArgb((int)_start).B, Color.FromArgb((int)_end).B);
            }
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

            if (interpobject.trait_type == "kTraitColor") {
                double valR, valG, valB = 0;
                //convert interp[] range of 0 to 1 into range between selected beats
                for (int x = 0; x < interp.Length; x++) {
                    valR = ((max == startcolor.R ? 1 - interp[x] : interp[x]) / 1) * (max - min) + min;
                    valG = ((max2 == startcolor.G ? 1 - interp[x] : interp[x]) / 1) * (max2 - min2) + min2;
                    valB = ((max3 == startcolor.B ? 1 - interp[x] : interp[x]) / 1) * (max3 - min3) + min3;
                    interp[x] = Color.FromArgb((int)valR, (int)valG, (int)valB).ToArgb();
                }
            }
            else {
                //if the first cell is actually the maximum, each value needs to be flipped across the range 0 to 1
                if (_start == max) {
                    for (int x = 0; x < interp.Length; x++)
                        interp[x] = 1 - interp[x];
                }
                //convert interp[] range of 0 to 1 into range between selected beats
                for (int x = 0; x < interp.Length; x++) {
                    interp[x] = ((interp[x] - 0) / (1 - 0)) * (max - min) + min;
                }
            }
            //assign new values back to the data points
            EditorIsInterpolating = true;
            for (int x = 0; x < _beats; x++) {
                interpobject[InterpCells[0].ColumnIndex + x].Value = TCLE.TruncateDecimal((decimal)interp[x], 3);
            }
            EditorIsInterpolating = false;
            //
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
                CellValueChanged(trackEditor[trackEditor.SelectedCells[0].ColumnIndex, trackEditor.SelectedCells[0].RowIndex]);
            }
        }

        private void btnLeafSplit_Click(object sender, EventArgs e)
        {
            if (LvlSequencer != null) {
                MessageBox.Show("Not allowed to split a lvl sequencer!", "Jumper Justum Jevel Jeditor");
                return;
            }
            //do nothing if no cells selected
            if (trackEditor.SelectedCells.Count == 0)
                return;
            if (trackEditor.SelectedCells.Count > 1) {
                MessageBox.Show("Select only 1 cell to be the split point", "Leaf split error");
                return;
            }
            //split leaf into 2 leafs
            int splitindex = trackEditor.CurrentCell.ColumnIndex - FrozenColumnOffset;
            if (MessageBox.Show($"Split this leaf between beat {splitindex - 1} and {splitindex}?\nThis leaf will end at beat {splitindex - 1}. The new leaf will have all data from beat {splitindex} and onward.\nTHIS CHANGE CANNOT BE UNDONE!", "Split leaf", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            //create file renaming dialog and show it
            FileInfo SplitFile;
            using SaveFileDialog sfd = new();
            sfd.Filter = "Thumper Leaf File (*.leaf)|*.leaf";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = LoadedLeaf.DirectoryName ?? TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                SplitFile = new FileInfo(sfd.FileName);
            }
            else
                return;

            Form_LeafEditor LeafSplitAfter = (Form_LeafEditor)TCLE.OpenFile(LoadedLeaf, false, true);
            LeafSplitAfter.loadedleaf = SplitFile;
            //after setting the loadedleaf like that, it will kick this leafs file out of locked files. So we have to readd it
            TCLE.AddFileLock(LoadedLeaf);
            //remove columns from the beginning to shoft all cells backwards until they get to beat 0
            for (int x = 0; x < splitindex; x++) {
                LeafSplitAfter.trackEditor.Columns.RemoveAt(0);
            }
            /*
            foreach (Sequencer_Object seq in LeafSplitAfter.SequencerObjects) {
                //some objects need the leaf name. Change them to the new leaf's name
                if (seq.obj_name.Contains(".leaf"))
                    seq.obj_name = LeafSplitAfter.loadedleaf.Name;
                //shift data points backwards so they align at beat 0
                for (int x = splitindex; x < LeafSplitAfter.LeafProperties.beats; x++) {
                    seq[x - splitindex] = new SeqDataPoint() {
                        Value = seq[x].Value,
                        Ease = seq[x].Ease,
                        Interpolation = seq[x].Interpolation,
                    };
                    //after copying, set the value to null since this datapoint is "leaving"
                    seq[x].Value = null;
                }
            }*/
            //reduce split leafs beat count and save
            LeafSplitAfter.LeafProperties.beats = LeafProperties.beats - splitindex;
            LeafSplitAfter.SaveCheckAndWrite(true, "");
            LeafSplitAfter.Dispose();

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

        private void btnLeafAutoPlace_Click(object sender, EventArgs e)
        {
            TCLE.PlaySound("UIselect");
            Properties.Settings.Default.LeafOptionAutoPlace = btnLeafAutoPlace.Checked;
            Properties.Settings.Default.Save();
            foreach (Form_LeafEditor leaf in TCLE.Documents.Where(x => x.GetType() == typeof(Form_LeafEditor))) {
                leaf.btnLeafAutoPlace.Checked = Properties.Settings.Default.LeafOptionAutoPlace;
            }
        }

        private void btnLeafRandom_Click(object sender, EventArgs e)
        {
            EditorIsRandomizing = true;
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

            Sequencer_Object seq = new() {
                ParentLeaf = leafProperties,
                obj_name = category == "PLAY SAMPLE" ? TCLE.ProjectSamples[TCLE.rng.Next(0, TCLE.ProjectSamples.Count)].obj_name : obj.obj_name,
                category = obj.category,
                param_path = obj.param_path,
                friendly_param = obj.param_displayname,
                defaultvalue = float.Parse(obj.def),
                step = obj.step,
                trait_type = obj.trait_type,
                highlight_color = obj.defaultcolor,
                highlight_value = 0,
                footer = obj.footer,
                enabled = true,
                expandlanes = Properties.Settings.Default.LeafOptionShowLane
            };
            if (seq.obj_name == "leafname")
                seq.obj_name = LeafProperties.FilePath.Name;
            if (seq.category == "LOOP TRACK VOLUME") {
                int audiochannels = SequencerObjects.Count(x => x.category == "LOOP TRACK VOLUME");
                seq.param_path = seq.param_path.Replace("x", $"{audiochannels}");
                seq.friendly_param = seq.friendly_param.Replace("x", $"{audiochannels}");
            }

            if (seq.friendly_lane == "lane center") {
                LoadMultiLanes(seq, SequencerObjects);
            }
            else {
                SequencerObjects.Add(seq);
                trackEditor.Rows.Add(seq);
            }
            ChangeTrackName(seq, seq.category);
            //FindMissingLaneObjects(seq);

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
            } while (!seq.Cells.Cast<SeqDataPoint>().Any(x => x.Value is not null));

            trackEditor.Invalidate();
            TCLE.PlaySound("UIaddrandom");
            EditorIsRandomizing = false;
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
                EditorIsRandomizing = true;
                foreach (Sequencer_Object seq in SelectedSeq) {
                    do {
                        if (seq.friendly_lane == "lane center") {
                            RandomizeRowValues(SequencerObjects[seq.Index - 2]);
                            RandomizeRowValues(SequencerObjects[seq.Index - 1]);
                            RandomizeRowValues(seq);
                            RandomizeRowValues(SequencerObjects[seq.Index + 1]);
                            RandomizeRowValues(SequencerObjects[seq.Index + 2]);
                        }
                        else
                            RandomizeRowValues(seq);
                    } while (!seq.Cells.Cast<SeqDataPoint>().Any(x => x.Value != null));
                }
                ShowRawTrackData(SequencerObjects[CurrentRow]);
                trackEditor.Invalidate();

                TCLE.PlaySound("UIaddrandom");
                EditorIsRandomizing = false;
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
                leafProperties = new(this, filepath, Lvl.lvlleafs.Select(x => x.beats).Sum() + Lvl.approachbeats + (Lvl.lvlleafs.Count(x => x.beats == -1) * 2)) {
                    SequencerType = filepath.Extension,
                    timesignature = "4/4"
                };
            }
            trackEditor.Rows.Clear();

            LeafLengthChanged();
        }

        public void LoadLeafSimple(dynamic _load, FileInfo filepath, LvlProperties Lvl = null)
        {
            LoadedLeaf = filepath;
            //set flag that load is in progress. This skips Save method
            EditorIsLoading = true;
            if (filepath.Extension == ".leaf") {
                leafProperties = new(this, filepath, (int?)_load["beat_cnt"] ?? 1) {
                    SequencerType = filepath.Extension,
                    timesignature = (string)_load["time_sig"] ?? "4/4"
                };
            }
            else if (filepath.Extension == ".lvl") {
                leafProperties = new(this, filepath, Lvl.lvlleafs.Select(x => x.beats).Sum() + Lvl.approachbeats + (Lvl.lvlleafs.Count(x => x.beats == -1) * 2)) {
                    SequencerType = filepath.Extension,
                    timesignature = "4/4"
                };
            }
        }

        public void LoadEnd()
        {
            //finsih up setting up the leaf editor. Enable some buttons, set zoom level, etc.
            trackZoom_Scroll(null, null);
            foreach (Sequencer_Object seq in SequencerObjects) {
                //update visual row properties
                ChangeTrackName(seq, seq.category);
            }

            //mark that lvl is saved (just freshly loaded)
            EditorIsLoading = false;
            EditorIsSaved = true;
            SaveCheckAndWrite(true, "", false);
            TrackTimeSigHighlighting();
            trackEditor.Invalidate();
        }

        public static void LoadSequencer(dynamic seqJSON, LeafProperties ParentLeaf)
        {
            List<Sequencer_Object> LoadedObjects = new();

            //each object in the seq_objs[] list
            foreach (dynamic seq_obj in seqJSON) {
                Sequencer_Object ObjectToImport = new() {
                    ParentLeaf = ParentLeaf,
                    obj_name = ((string)seq_obj["obj_name"]),
                    trait_type = seq_obj["trait_type"],
                    step = (string)seq_obj["step"] == "True",
                    defaultvalue = seq_obj["default"],
                    footer = seq_obj["footer"].GetType() == typeof(JArray) ? String.Join(",", ((JArray)seq_obj["footer"]).ToList()) : ((string)seq_obj["footer"]).Replace("[", "").Replace("]", ""),
                    //if the leaf has definitions for these, add them. If not, set to defaults
                    param_path = seq_obj.ContainsKey("param_path_hash") ? $"0x{(string)seq_obj["param_path_hash"]}" : ((string)seq_obj["param_path"]),
                    highlight_value = (int?)seq_obj["editor_data"]?[1] ?? 0,
                    enabled = ((string)seq_obj["enabled"] ?? "True") == "True",
                    isdefault = false
                };
                ObjectToImport.highlight_color = seq_obj["editor_data"]?[0] != null ? Color.FromArgb((int)seq_obj["editor_data"][0]) : (TCLE.LeafObjects.FirstOrDefault(x => x.param_displayname == ObjectToImport.friendly_param)?.defaultcolor ?? Color.Purple);

                //if object is a layer volume, we "reset" its index to x so it can be renumbered in case its out of order.
                if (ObjectToImport.param_path.StartsWith("layer_volume"))
                    ObjectToImport.param_path = "layer_volume,x";
                //if the object is a tuning layer, handle it here
                if (ObjectToImport.obj_name == "_TuningLayerX") {
                    ObjectToImport.friendly_param = ObjectToImport.param_path;
                    ObjectToImport.category = "";
                }
                //if object is a .samp, set category and friendly_param since they don't exist in LeafObjects
                else if (ObjectToImport.obj_name.EndsWith(".samp") && ObjectToImport.param_path == "play") {
                    ObjectToImport.category = "PLAY SAMPLE";
                    ObjectToImport.friendly_param = "play";
                }
                //otherwise, search LeafObjects for the friendly names for display purposes
                else {
                    try {
                        string normalizeParam = $"{ObjectToImport.param_path.Replace(".a02", ".ent").Replace(".a01", ".ent").Replace(".z01", ".ent").Replace(".z02", ".ent")}";
                        Object_Params objmatch = TCLE.LeafObjects.FirstOrDefault(obj => obj.param_path == normalizeParam && obj.obj_name == ObjectToImport.obj_name.Replace(ParentLeaf.FilePath.Name, "leafname"));
                        ObjectToImport.friendly_param = objmatch?.param_displayname ?? "";
                        ObjectToImport.category = objmatch?.category ?? "";
                        //set audio channel numbers on load
                        if (ObjectToImport.category == "LOOP TRACK VOLUME") {
                            int audiochannels = LoadedObjects.Count(x => x.category == "LOOP TRACK VOLUME");
                            ObjectToImport.param_path = ObjectToImport.param_path.Replace("x", $"{audiochannels}");
                            ObjectToImport.friendly_param = ObjectToImport.friendly_param.Replace("x", $"{audiochannels}");
                        }
                    } catch (Exception) { }
                }
                //deal with multilanes
                //if object is multilane, we will add all 5 lanes at once, as defaults
                //then lookup the object and assign the initialized Sequencer Object created above in place of the default one
                if (ObjectToImport.friendly_lane is not "none") {
                    LoadMultiLanes(ObjectToImport, LoadedObjects);
                    ObjectToImport.expandlanes = Properties.Settings.Default.LeafOptionShowLane;
                    //ParentLeaf.trackEditor.Rows.Add(ObjectToImport);
                }
                else {
                    ObjectToImport.expandlanes = true;
                    LoadedObjects.Add(ObjectToImport);
                    ParentLeaf.trackEditor.Rows.Add(ObjectToImport);
                }
                //import data points to the row cells.
                LoadDataPoints(ObjectToImport, seq_obj);
                RowReadOnly(ObjectToImport, ObjectToImport.enabled);
            }

            //return Seq_Objs;
            ParentLeaf.seq_objs = LoadedObjects;
        }

        public static void LoadDataPoints(Sequencer_Object ObjectToImport, dynamic seq_obj)
        {
            //There are 2 methods here for backwards compat
            foreach (dynamic dp in seq_obj["data_points"]) {
                //modern data point. The save data includes interp and ease
                if (dp is JObject data_point) {
                    SeqDataPoint data = new() {
                        Interpolation = ((string)data_point["interp"])?.Replace("kTraitInterp", "") ?? "Linear",
                        Ease = TCLE.Easings[(string)data_point["ease"] ?? "kEaseInOut"]
                    };
                    if ((int)data_point["beat"] >= ObjectToImport.ParentLeaf.Beats)
                        continue;
                    ObjectToImport[(int)data_point["beat"] + FrozenColumnOffset] = data;
                    ObjectToImport[(int)data_point["beat"] + FrozenColumnOffset].Value = (decimal)data_point["value"];
                }
                //old data point. The save includes Value only.
                else {
                    SeqDataPoint data = new() {
                        //Beat = int.Parse(((JProperty)dp).Name),
                        Interpolation = "Linear",
                        Ease = TCLE.Easings["kEaseInOut"]
                    };
                    if (int.Parse(((JProperty)dp).Name) >= ObjectToImport.ParentLeaf.Beats)
                        continue;
                    ObjectToImport[data.beat + FrozenColumnOffset] = data;
                    ObjectToImport[data.beat + FrozenColumnOffset].Value = TCLE.TruncateDecimal((decimal)((JProperty)dp).Value, 3);
                }
            }
        }

        public static void LoadMultiLanes(Sequencer_Object ObjectToImport, List<Sequencer_Object> LoadedObjects)
        {
            Sequencer_Object lookup = LoadedObjects.FirstOrDefault(x => x.obj_name == ObjectToImport.obj_name && x.param_path == ObjectToImport.param_path && x.param_path_lane == ObjectToImport.param_path_lane && x.isdefault == true);
            //if null, no object exists in SequencerObjects yet for this object or its lanes. We'll have to make it.
            if (lookup == null) {
                LoadedObjects.Add(ObjectToImport.CloneAsLane(".a01", Properties.Settings.Default.LeafOptionShowLane));
                ObjectToImport.ParentLeaf.trackEditor.Rows.Add(LoadedObjects[^1]); LoadedObjects[^1].expandlanes = Properties.Settings.Default.LeafOptionShowLane;
                LoadedObjects.Add(ObjectToImport.CloneAsLane(".a02", Properties.Settings.Default.LeafOptionShowLane));
                ObjectToImport.ParentLeaf.trackEditor.Rows.Add(LoadedObjects[^1]); LoadedObjects[^1].expandlanes = Properties.Settings.Default.LeafOptionShowLane;
                LoadedObjects.Add(ObjectToImport.CloneAsLane(".ent", Properties.Settings.Default.LeafOptionShowLane));
                ObjectToImport.ParentLeaf.trackEditor.Rows.Add(LoadedObjects[^1]); LoadedObjects[^1].expandlanes = Properties.Settings.Default.LeafOptionShowLane;
                LoadedObjects.Add(ObjectToImport.CloneAsLane(".z01", Properties.Settings.Default.LeafOptionShowLane));
                ObjectToImport.ParentLeaf.trackEditor.Rows.Add(LoadedObjects[^1]); LoadedObjects[^1].expandlanes = Properties.Settings.Default.LeafOptionShowLane;
                LoadedObjects.Add(ObjectToImport.CloneAsLane(".z02", Properties.Settings.Default.LeafOptionShowLane));
                ObjectToImport.ParentLeaf.trackEditor.Rows.Add(LoadedObjects[^1]); LoadedObjects[^1].expandlanes = Properties.Settings.Default.LeafOptionShowLane;

                lookup = LoadedObjects.FirstOrDefault(x => x.obj_name == ObjectToImport.obj_name && x.param_path == ObjectToImport.param_path && x.param_path_lane == ObjectToImport.param_path_lane && x.isdefault == true);
            }
            int index = LoadedObjects.IndexOf(lookup);
            LoadedObjects[index] = ObjectToImport;
            ObjectToImport.ParentLeaf.trackEditor.Rows.RemoveAt(index);
            ObjectToImport.ParentLeaf.trackEditor.Rows.Insert(index, LoadedObjects[index]);
        }

        public void LoadTracksFromSequencer(List<Sequencer_Object> Seq_Objs)
        {
            //clear the DGV and prep for new data
            trackEditor.Rows.Clear();
            trackEditor.RowHeadersVisible = true;
            foreach (Sequencer_Object seq in Seq_Objs) {
                trackEditor.Rows.Add(seq);
                RowReadOnly(seq, !seq.enabled);
            }
            TCLE.ResizeHeaders(trackEditor);
        }


        private void toolstripObjTune_Click(object sender, EventArgs e)
        {
            AddSequencerLayer(trackEditor.CurrentRow.Index);
        }
        public void AddSequencerLayer(int index)
        {
            Sequencer_Object seq = new() {
                ParentLeaf = leafProperties,
                obj_name = "_TuningLayerX",
                category = "",
                param_path = "⮝ Tuning Layer X",
                friendly_param = "⮝ Tuning Layer X",
                defaultvalue = 0,
                step = false,
                trait_type = "",
                highlight_color = Color.FromArgb(40, 40, 40),
                highlight_value = 0,
                footer = "",
                enabled = true
            };

            int tuninglayers = SequencerObjects.Count(x => x.obj_name == "_TuningLayerX");
            seq.param_path = seq.param_path.Replace("X", $"{tuninglayers}");
            seq.friendly_param = seq.friendly_param.Replace("X", $"{tuninglayers}");

            seq.expandlanes = seq.friendly_lane == "none" || Properties.Settings.Default.LeafOptionShowLane;
            SequencerObjects.Insert(index + 1, seq);
            trackEditor.Rows.Insert(index + 1, seq);
            ChangeTrackName(seq, "");
            SaveCheckAndWrite(false, "Add Object");
            TCLE.PlaySound("UIobjectadd");
        }

        private void toolstripObjConvert_Click(object sender, EventArgs e)
        {
            Sequencer_Object seq = SequencerObjects[trackEditor.CurrentRow.Index].Clone();
            seq.obj_name = "_TuningLayerX";
            seq.category = "";
            seq.param_path = "⮝ Tuning Layer X";
            seq.friendly_param = "⮝ Tuning Layer X";
            seq.defaultvalue = 0;
            seq.step = false;
            seq.trait_type = "";
            seq.highlight_color = Color.FromArgb(40, 40, 40);
            seq.highlight_value = 0;
            seq.footer = "";
            seq.enabled = true;

            int tuninglayers = SequencerObjects.Count(x => x.obj_name == "_TuningLayerX");
            seq.param_path = seq.param_path.Replace("X", $"{tuninglayers}");
            seq.friendly_param = seq.friendly_param.Replace("X", $"{tuninglayers}");

            SequencerObjects.Insert(trackEditor.CurrentRow.Index + 1, seq);
            trackEditor.Rows.Insert(trackEditor.CurrentRow.Index + 1, seq);

            ChangeTrackName(seq, "");
            TCLE.PlaySound("UIinterpolatewindow");

            CalculateTuningLayers(leafProperties, seq);
            SaveCheckAndWrite(false, "Converted object to tuning layer");
        }

        public List<SaveState> GetUndoList() => UndoList;
        public void PerformUndo(int undolistindex)
        {
            if (undolistindex > UndoList.Count - 1)
                return;
            bool _trackNotSaved = EditorIsSaved;
            //track which objects are expanded
            List<Sequencer_Object> _expanded = SequencerObjects.Where(x => x.expandlanes == true).ToList();
            //
            LoadLeaf(UndoList[undolistindex].savestate, LvlSequencer?.FilePath ?? LoadedLeaf, LvlSequencer);
            LoadSequencer(UndoList[undolistindex].savestate["seq_objs"], LeafProperties);
            LoadTracksFromSequencer(LeafProperties.seq_objs);
            LoadEnd();
            UndoList.RemoveRange(0, undolistindex);
            propertyGridLeaf.Refresh();
            //restore expanded lanes
            foreach (Sequencer_Object seq in SequencerObjects) {
                if (_expanded.Any(x => x.obj_name == seq.obj_name && x.friendly_lane == seq.friendly_lane && x.friendly_param == seq.friendly_param))
                    seq.expandlanes = true;
            }

            if (!_trackNotSaved) {
                EditorIsSaved = false;
                if (!this.Text.EndsWith("*"))
                    this.Text += '*';
            }
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
        public FileInfo SaveAs(bool isnew = false, string startpath = null)
        {
            using SaveFileDialog sfd = new();
            //filter .txt only
            sfd.Filter = "Thumper Editor Leaf File (*.leaf)|*.leaf";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = startpath ?? TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                loadedleaf = new FileInfo(sfd.FileName);
                EditorIsLoading = true;
                if (LeafProperties == null) {
                    leafProperties = new(this, loadedleaf, isnew ? 32 : leafProperties.beats) {
                        timesignature = "4/4"
                    };
                } //else
                  //leafProperties.FilePath = loadedleaf;
                EditorIsLoading = false;
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
            if (EditorIsLoading || Playback.Generating)
                return;
            //make the beeble emote
            TCLE.MainBeeble.MakeFace();

            EditorIsSaved = IsSaved;
            JObject _saveJSON = BuildSave(leafProperties);
            //
            if (!IsSaved) {
                //denote editor tab is not saved
                this.Text = $"{LoadedLeaf.Name}{(LoadedLeaf.Extension.Equals(".lvl", StringComparison.OrdinalIgnoreCase) ? " [Sequencer]" : "")}" + "*";
                //update the undo list
                if (LogUndo) {
                    UndoList.Insert(0, new SaveState() {
                        reason = Reason,
                        savestate = _saveJSON
                    });
                }
            }
            else {
                this.Text = $"{LoadedLeaf.Name}{(LoadedLeaf.Extension.Equals(".lvl", StringComparison.OrdinalIgnoreCase) ? " [Sequencer]" : "")}";
                //leafProperties.revertPoint = _saveJSON;
                //If leaf, build the JSON to write to file
                if (LoadedLeaf.Extension == ".leaf") {
                    //write JSON to file
                    TCLE.WriteFileLock(TCLE.lockedfiles[LoadedLeaf], _saveJSON);
                    //need to update leaf beat count in every lvl that references this file
                    if (LeafProperties.BeatsChangedSinceSave) {
                        foreach (FileInfo lvl in ProjectExplorer.Files.Where(x => x.Extension.Equals(".lvl", StringComparison.OrdinalIgnoreCase))) {
                            dynamic _loadfile = TCLE.LoadFileLock(lvl.FullName);
                            //if load fails, skip
                            if (_loadfile == null)
                                continue;
                            bool changes = false;
                            //some files may be lock loaded, so we use different writing methods for those
                            //also force editor to reload the document
                            foreach (dynamic leafseq in _loadfile["leaf_seq"]) {
                                if (leafseq["leaf_name"] == LoadedLeaf.Name) {
                                    leafseq["beat_cnt"] = LeafProperties.beats;
                                    changes = true;
                                }
                            }
                            if (changes)
                                TCLE.WriteFileLock(new FileStream(lvl.FullName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite), _loadfile);
                        }
                        TCLE.FindEditorRunMethod(typeof(Form_LvlEditor), "RecalculateRuntime");
                    }
                    if (playsound) TCLE.PlaySound("UIsave");
                }
                //else if a different sequencer, pass data back and force save
                else {
                    LvlSequencer.seq_objs = LeafProperties.seq_objs;
                    /*
                    if (TCLE.Documents.FirstOrDefault(x => x.GetType() == typeof(Form_LvlEditor) && (x as Form_LvlEditor).LoadedLvl.Name == LoadedLeaf.Name) is Form_LvlEditor Owner) {
                        Owner.lvlProperties.seq_objs = LeafProperties.seq_objs;
                        Owner.Save(false);
                    }*/
                }

                //find if any raw text docs are open of this leaf and update them
                TCLE.FindReloadRaw(LoadedLeaf.Name);
                LeafProperties.BeatsChangedSinceSave = false;
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
                TCLE.GenerateColumnStyle(Columns, FrozenColumnOffset);
            }
            else
                trackEditor.ColumnCount = LeafProperties.beats + FrozenColumnOffset;
            //set cell zoom
            trackZoom_Scroll(null, null);
            //make sure new cells follow the time sig
            TrackTimeSigHighlighting();
            //sets flag that leaf has unsaved changes
            SaveCheckAndWrite(false, $"Leaf length changed {data} -> {LeafProperties.beats}");
        }

        ///Import raw text from rich text box to selected row
        public static void TrackRawImport(Sequencer_Object seq, List<SeqDataPoint> data_points, LeafProperties _properties)
        {
            List<SeqDataPoint> DataNotNull = data_points.Where(x => x.Value is not null && x.beat < _properties.beats).ToList();
            //iterate over each data point, and fill cells
            foreach (SeqDataPoint data_point in DataNotNull) {
                try {
                    seq.Cells[data_point.beat + FrozenColumnOffset].Value = TCLE.TruncateDecimal(Decimal.Parse(data_point.Value.ToString()), 3);
                    //seq[data_point.beat].Value = TCLE.TruncateDecimal(Decimal.Parse(data_point.value.ToString()), 3);
                } catch (Exception ex) {
                    MessageBox.Show($"Failed to entirely parse raw text.\n\n{ex}", "Thumper Custom Level Editor");
                    break;
                }
            }
        }
        public static void TrackRawImport(Sequencer_Object seq, JObject _rawdata)
        {
            //_rawdata contains a list of all data points. By getting Properties() of it,
            //each point becomes its own index
            List<JProperty> data_points = _rawdata.Properties().ToList();
            //iterate over each data point, and fill cells
            foreach (JProperty data_point in data_points) {
                try {
                    seq[int.Parse(data_point.Name)].Value = TCLE.TruncateDecimal((decimal)data_point.Value, 3);
                    //seq[int.Parse(data_point.Name)].Value = TCLE.TruncateDecimal((decimal)data_point.Value, 3);
                } catch (ArgumentOutOfRangeException) {
                    break;
                }
            }
        }

        ///Updates row headers to be the Object and Param_Path
        public static void ChangeTrackName(Sequencer_Object seq, string category = "")
        {
            string ShowCategory = Properties.Settings.Default.LeafOptionShowCategory ? $"[{category}] " : "";
            string ShowLane = (seq.expandlanes && seq.friendly_lane != "none") ? $"{seq.friendly_param}, {seq.friendly_lane}" : seq.friendly_param;
            if (seq.category == "PLAY SAMPLE")
                //show the sample name instead
                seq.HeaderCell.Value = $"{ShowCategory}{seq.obj_name}";
            else if (seq.obj_name == "_TuningLayerX")
                seq.HeaderCell.Value = $"  {seq.param_path}";
            else
                seq.HeaderCell.Value = $"{ShowCategory}{ShowLane}";
        }

        public void ShowRawTrackData(Sequencer_Object seq)
        {
            string allcellvalues = String.Join(",", seq.Cells.Cast<SeqDataPoint>().Where(x => x.Value is not null).Select(x => $"{x.beat}:{x.Value}"));
            textEditor.Text = allcellvalues;
            textEditor.ClearUndo();
            textEditor.SetSelectedLine(-1);
        }

        ///Updates column highlighting in the DGV based on time sig
        public void TrackTimeSigHighlighting()
        {
            if (LeafProperties == null || EditorIsLoading || SaveOnlyNoLoad)
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
                index += leaf.beats != -1 ? leaf.beats : 1;
            }
        }
        private void trackEditor_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.FillWeight = 0.001f;
        }

        private static void RowReadOnly(Sequencer_Object seq, bool setreadonly)
        {
            if (setreadonly) {
                seq.ReadOnly = true;
                foreach (DataGridViewCell dgvc in seq.Cells.Cast<DataGridViewCell>().Where(x => x.ColumnIndex >= FrozenColumnOffset)) {
                    dgvc.Style.BackColor = Color.Gray;
                    dgvc.Style.SelectionBackColor = Color.Gray;
                }
            }
            else {
                seq.ReadOnly = false;
                foreach (DataGridViewCell dgvc in seq.Cells.Cast<DataGridViewCell>().Where(x => x.ColumnIndex >= FrozenColumnOffset)) {
                    dgvc.Style = null;
                }
            }
        }

        public void EnableLeafButtons()
        {
            if (Playback.Generating)
                return;
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
                if (seq_obj.obj_name == "leafname" || seq_obj.obj_name.Contains(".leaf") || string.IsNullOrEmpty(seq_obj.obj_name))
                    seq_obj.obj_name = _properties.FilePath.Name;
                s.Add("obj_name", seq_obj.obj_name);
                //write param_path or param_path_hash
                if (seq_obj.param_path.StartsWith("0x"))
                    s.Add("param_path_hash", seq_obj.param_path.Replace("0x", ""));
                else
                    s.Add("param_path", $"{seq_obj.param_path}");
                s.Add("trait_type", seq_obj.trait_type);
                //
                JArray datapoints = new();
                for (int _in = 0; _in < _properties.beats; _in++) {
                    if (seq_obj[_in]?.Value == null)
                        continue;
                    JObject d = new() {
                        { "beat", seq_obj[_in].beat },
                        { "value", (decimal)seq_obj[_in].Value },
                        { "interp", $"kTraitInterp{seq_obj[_in].Interpolation ?? "Linear"}" },
                        { "ease", $"k{seq_obj[_in].Ease?.Replace(" ", "") ?? "EaseInOut"}" }
                    };

                    datapoints.Add(d);
                }
                s.Add("data_points", datapoints);
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

        #region Cut Copy Paste
        public void Copy()
        {
            if (textEditor.Focused)
                return;
            ///copies selected cells
            IEnumerable<SeqDataPoint> _selected = trackEditor.SelectedCells.Cast<SeqDataPoint>();
            List<SeqDataPoint> lanecells = new();
            foreach (SeqDataPoint dgvc in _selected) {
                if (SequencerObjects[dgvc.RowIndex].friendly_lane == "lane center" && SequencerObjects[dgvc.RowIndex].expandlanes == false) {
                    lanecells.Add((SeqDataPoint)trackEditor[dgvc.ColumnIndex, dgvc.RowIndex - 2]);
                    lanecells.Add((SeqDataPoint)trackEditor[dgvc.ColumnIndex, dgvc.RowIndex - 1]);
                    lanecells.Add((SeqDataPoint)trackEditor[dgvc.ColumnIndex, dgvc.RowIndex + 1]);
                    lanecells.Add((SeqDataPoint)trackEditor[dgvc.ColumnIndex, dgvc.RowIndex + 2]);
                }
            }
            _selected = lanecells.Concat(_selected).OrderBy(x => x.RowIndex).ThenBy(x => x.ColumnIndex);
            TCLE.ClipboardDataPoints = _selected.Select(x => x.Clone()).ToList();
            TCLE.PlaySound("UIkcopy");
        }

        public void Cut()
        {
            if (textEditor.Focused)
                return;
            Copy();
            LogUndo = false;
            CellValueChanged(trackEditor[trackEditor.CurrentCell.ColumnIndex, trackEditor.CurrentCell.RowIndex], true);
            LogUndo = true;
            SaveCheckAndWrite(false, "Cut cells");
        }

        public void Paste()
        {
            if (textEditor.Focused)
                return;

            EditorIsPasting = true;
            LogUndo = false;
            int pastingrow = trackEditor.CurrentCell.RowIndex;
            if (SequencerObjects[pastingrow].friendly_lane == "lane center" && SequencerObjects[pastingrow].expandlanes == false && TCLE.ClipboardDataPoints.Count >= 5)
                pastingrow -= 2;
            int pastingcol = trackEditor.CurrentCell.ColumnIndex;
            int rowoffset = TCLE.ClipboardDataPoints.First().OriginalRow;
            int coloffset = TCLE.ClipboardDataPoints.First().OriginalColumn;

            List<Sequencer_Object> pastedrows = new();

            foreach (SeqDataPoint sdp in TCLE.ClipboardDataPoints) {
                //if copied beat is pasted outside rowcount, break, because all beats after it will also be outside the bounds
                if (pastingrow + (sdp.OriginalRow - rowoffset) >= SequencerObjects.Count)
                    break;
                //if copied beat is pasted beyond beatcount, skip it
                if (pastingcol + (sdp.OriginalColumn - coloffset) >= leafProperties.beats)
                    continue;
                SeqDataPoint clone = sdp.Clone();
                SequencerObjects[pastingrow + (sdp.OriginalRow - rowoffset)][pastingcol + (sdp.OriginalColumn - coloffset)] = clone;
                if (!pastedrows.Contains(SequencerObjects[pastingrow + (sdp.OriginalRow - rowoffset)]))
                    pastedrows.Add(SequencerObjects[pastingrow + (sdp.OriginalRow - rowoffset)]);
            }

            foreach (Sequencer_Object _seq in pastedrows) {
                CalculateTuningLayers(LeafProperties, _seq);
            }

            EditorIsPasting = false;
            LogUndo = true;
            SaveCheckAndWrite(false, "Pasted cells");
            trackEditor.Invalidate();
        }
        #endregion

        public static void RandomizeRowValues(Sequencer_Object seq)
        {
            Random rng = new();
            int rngchance;
            int rnglimit;
            int randomtype = 0;
            decimal? valueiftrue = 0;

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
            foreach (SeqDataPoint dgvc in seq.Cells.Cast<SeqDataPoint>().Where(x => x.ColumnIndex >= FrozenColumnOffset)) {
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
                //dgvc.Value = _out;
                dgvc.Value = (decimal?)_out;// new() { ParentSeqObj = seq, Value = (decimal?)_out, Ease = "Ease In Out", Interpolation = "Linear" };
                dgvc.Ease = "Ease In Out";
                dgvc.Interpolation = "Linear";
            }
        }
        /*
        private void FindMissingLaneObjects(Sequencer_Object seq)
        {
            if (EditorIsMoving)
                return;
            //don't need to find lanes for non-multi-lanes
            if (seq.friendly_lane == "none")
                return;
            EditorIsFinding = true;

            if (seq.Lanes[1] is null) {
                Sequencer_Object clone = seq.CloneAsLane("a02", Properties.Settings.Default.LeafOptionShowLane);
                trackEditor.Rows.Insert(seq.Index, clone);
                SequencerObjects.Insert(seq.Index, clone);
            }
            if (seq.Lanes[3] is null) {
                Sequencer_Object clone = seq.CloneAsLane("z01", Properties.Settings.Default.LeafOptionShowLane);
                trackEditor.Rows.Insert(seq.Index + 1, clone);
                SequencerObjects.Insert(seq.Index + 1, clone);
            }
            if (seq.Lanes[0] is null) {
                Sequencer_Object clone = seq.CloneAsLane("a01", Properties.Settings.Default.LeafOptionShowLane);
                trackEditor.Rows.Insert(seq.Index - 1, clone);
                SequencerObjects.Insert(seq.Index - 1, clone);
            }
            if (seq.Lanes[4] is null) {
                Sequencer_Object clone = seq.CloneAsLane("z02", Properties.Settings.Default.LeafOptionShowLane);
                trackEditor.Rows.Insert(seq.Index + 2, clone);
                SequencerObjects.Insert(seq.Index + 2, clone);
            }

            EditorIsFinding = false;
        }*/

        public static void CalculateTuningLayers(LeafProperties _properties, Sequencer_Object seq)
        {
            if (_properties.ParentEditor.EditorIsProcessing)
                return;

            if (_properties.ParentEditor.EditorIsLoading || (seq.Index == 0 && seq.obj_name == "_TuningLayerX"))
                return;
            int count = 1;
            List<Sequencer_Object> TuningLayers = new();
            while (_properties.seq_objs[seq.Index - count].obj_name == "_TuningLayerX") {
                count++;
            }
            Sequencer_Object Main = _properties.seq_objs[seq.Index - count];
            count = 1;
            while (Main.Index + count < _properties.seq_objs.Count && _properties.seq_objs[Main.Index + count].obj_name == "_TuningLayerX") {
                TuningLayers.Add(_properties.seq_objs[Main.Index + count]);
                count++;
            }

            _properties.ParentEditor.LogUndo = false;
            _properties.ParentEditor.EditorIsTuning = true;
            Sequencer_Object _temp2 = new() { ParentLeaf = _properties };
            _properties.ParentEditor.trackEditor.Rows.Add(_temp2);

            foreach (Sequencer_Object _layer in TuningLayers) {
                Sequencer_Object _temp = new() { ParentLeaf = _properties };
                _properties.ParentEditor.trackEditor.Rows.Add(_temp);
                SeqDataPoint[] _datapoints = _layer.Cells.Cast<SeqDataPoint>().Where(x => x.Value != null).ToArray();

                for (int n = 0; n < _datapoints.Length - 1; n++) {
                    //sort cells so they are in order according to column index
                    List<SeqDataPoint> InterpCells = new() { _datapoints[n], _datapoints[n + 1] };
                    InterpCells.Sort((cell1, cell2) => cell1.beat.CompareTo(cell2.beat));
                    //get start and end values, and how many beats separate them
                    double _start = (double)(decimal)InterpCells[0].Value;
                    double _end = (double)(decimal)InterpCells[1].Value;
                    double max = Math.Max(_start, _end);
                    double min = Math.Min(_start, _end);
                    int _beats = InterpCells[1].beat - InterpCells[0].beat + 1;
                    //initialize array = to beats, fill with linear values between 0 and 1
                    //these will be transformed by the formulas below
                    double[] interp = new double[_beats];
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = (double)(x) / (double)(interp.Length - 1);
                    }
                    //change interpolation formula based on settings on the datapoint
                    switch ($"{InterpCells[0].Interpolation} {InterpCells[0].Ease}") {
                        case "Linear Ease In":
                        case "Linear Ease Out":
                        case "Linear Ease In Out":
                            break;
                        case "Step Ease In":
                        case "Step Ease Out":
                        case "Step Ease In Out":
                            for (int x = 0; x < interp.Length; x++) {
                                interp[x] = 0;
                            }
                            interp[^1] = 1;
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
                    //write the datapoints to a temp object to store them
                    for (int x = 0; x < _beats; x++) {
                        _temp[InterpCells[0].beat + x + FrozenColumnOffset].Value = TCLE.TruncateDecimal((decimal)interp[x], 3);
                    }
                }
                //transfer temp data points to another temp as the first one will be cleared
                //this second one with sum together the tuning layers
                for (int m = FrozenColumnOffset; m < _temp2.Cells.Count; m++) {
                    if (_temp2[m].Value == null)
                        _temp2[m].Value = 0m;
                    _temp2[m].Value = (decimal)_temp2[m].Value + (decimal)(_temp[m].Value ?? 0m);
                }
                _properties.ParentEditor.trackEditor.Rows.Remove(_temp);
            }
            //write temp2 to the real object
            for (int m = FrozenColumnOffset; m < Main.Cells.Count; m++) {
                Main[m].Value = (decimal)(_temp2[m].Value ?? 0m);
            }
            _properties.ParentEditor.trackEditor.Rows.Remove(_temp2);
            _properties.ParentEditor.LogUndo = true;
            _properties.ParentEditor.EditorIsTuning = false;
        }

        public static float ConvertRange(float originalStart, float originalEnd, float newStart, float newEnd, float value) // value to convert
        {
            float scale = (float)(newEnd - newStart) / (originalEnd - originalStart);
            return (newStart + ((value - originalStart) * scale));
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
                    EditorIsMoving = true;
                    // Proceed with the drag and drop, passing in the list item.                    
                    _ = trackEditor.DoDragDrop(CenterLane, DragDropEffects.Move);
                    RowsToMove = null;
                    EditorIsMoving = false;
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
                if (e.Data.GetData(typeof(DataGridViewRow)) is DataGridViewRow) {
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
            if (RowsToMove == null)
                return;
            e.Effect = DragDropEffects.Move;
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = trackEditor.PointToClient(new Point(e.X, e.Y));
            // Retrieve the node at the drop location.
            int targetRow = trackEditor.HitTest(targetPoint.X, targetPoint.Y).RowIndex;
            if (targetRow == -1)
                return;
            //check if target location exists inside the lane selection
            if (RowsToMove.Length == 5 &&
                ((RowsToMove[0].friendly_lane == "lane right 2" && targetRow >= RowsToMove[4].Index && targetRow <= RowsToMove[0].Index) ||
                (RowsToMove[0].friendly_lane == "lane left 2" && targetRow >= RowsToMove[0].Index && targetRow <= RowsToMove[4].Index)))
                return;

            if (SequencerObjects[targetRow].friendly_lane != "none" && SequencerObjects[targetRow].expandlanes) {
                return;
            }

            if (RowsToMove.Length == 5 && RowsToMove[0].friendly_lane == "lane left 2" && targetRow < RowsToMove[0].Index)
                RowsToMove = RowsToMove.Reverse().ToArray();
            else if (RowsToMove.Length == 5 && RowsToMove[0].friendly_lane == "lane right 2" && targetRow > RowsToMove[0].Index)
                RowsToMove = RowsToMove.Reverse().ToArray();

            if (SequencerObjects[targetRow].friendly_lane == "lane center" && targetRow > RowsToMove[0].Index)
                targetRow += 2;
            else if (SequencerObjects[targetRow].friendly_lane == "lane center" && targetRow < RowsToMove[0].Index)
                targetRow -= 2;


            if (RowsToMove != null && targetRow != -1 && targetRow != previousDragOver) {
                previousDragOver = targetRow;
                trackEditor.SuspendLayout();
                /*
                trackEditor.Rows.Remove(CenterLane.editor_row);
                SequencerObjects.Remove(CenterLane);
                SequencerObjects.Insert(targetRow, CenterLane);
                trackEditor.Rows.Insert(targetRow, CenterLane.editor_row);
                */
                foreach (Sequencer_Object seq in RowsToMove) {
                    trackEditor.Rows.Remove(seq);
                    SequencerObjects.Remove(seq);
                    SequencerObjects.Insert(targetRow, seq);
                    trackEditor.Rows.Insert(targetRow, seq);
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
                Playback.Initialize("leaf");
                Playback.CreatePlaybackFromLeaf(LeafProperties, PlaybackEnd);
                Playback.Play(PlaybackStart, LeafProperties.beats, PlaybackLoop);
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
                trackEditor.Invalidate();
                //trackEditor.InvalidateColumn(PreviousSetColumn);
                //trackEditor.InvalidateColumn(PreviousSetColumn - 1);
                //trackEditor.InvalidateColumn(Playback.PlaybackBeat + FrozenColumnOffset);
                //PreviousSetColumn = Playback.PlaybackBeat + FrozenColumnOffset;
            }
            else {
                if (PlaybackLoop && !ForceStop)
                    return;
                ForceStop = false;
                timer1.Enabled = false;
                btnTrackPlayback.Image = Properties.Resources.icon_play2;
                Playback.StopPlayback();
                PreviousSetColumn = 3;
                trackEditor.Invalidate();
            }
        }
    }
}
