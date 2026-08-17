using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;
using Thumper_Custom_Level_Editor.Utility_Classes;
using Un4seen.Bass;
using WeifenLuo.WinFormsUI.Docking;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Windows.Media.Animation;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class EditorLeaf : EditorBase
    {
        #region Form Construction
        ///Load LEAF
        public EditorLeaf() { InitializeComponent(); }
        public EditorLeaf(dynamic load = null, FileInfo filepath = null, bool simpleload = false) : base(filepath, false, simpleload)
        {
            this.SimpleLoad = simpleload;
            if (this.SimpleLoad) {
                LoadLeafSimple(load);
                LoadSequencer(load["seq_objs"], _leafproperties, SimpleTrackEditor);
                return;
            }
            EditorIsLoading = true;

            Debug.WriteLine($"============START LOADING NEW LEAF: {WorkingFile.Name}============");
            InitializeComponent();
            RenderForm();
            ColorFormElements();

            if (load == null)
                return;

            LoadLeaf(load);
            LoadSequencer(load["seq_objs"], _leafproperties, trackEditor);
            LoadEnd(load);
        }
        ///Load LVL Sequencer
        public EditorLeaf(LvlProperties toload, FileInfo filepath = null, bool simpleload = false) : base(filepath, false, simpleload)
        {
            this.SimpleLoad = simpleload;
            if (this.SimpleLoad) {
                AltSequencer = toload;
                LoadLeafSimple(null);
                LoadSequencer(((LvlProperties)AltSequencer).seqJSON, LeafProperties, SimpleTrackEditor);
                return;
            }
            EditorIsLoading = true;
            InitializeComponent();
            RenderForm();
            ColorFormElements();
            this.Icon = Properties.Resources.ico_lvl;

            if (toload == null)
                return;

            AltSequencer = toload;
            LoadLeaf(null);
            LoadSequencer(((LvlProperties)AltSequencer).seqJSON, LeafProperties, trackEditor);
            LoadEnd(((LvlProperties)AltSequencer).seqJSON);
        }

        private void RenderForm()
        {
            if (SimpleLoad)
                return;
            Stopwatch sw = new();
            sw.Start();
            this.SuspendLayout();
            this.dockPanel1.SuspendLayout();
            splitContainerLeafSide.SplitterDistance = splitContainerLeafSide.Height - 5;
            splitContainerLeafSide.Panel2Collapsed = Properties.Settings.Default.LeafHideRaw;
            //
            dockPanel1.Theme = TCLE.DockTheme;
            //m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            contentMain.Controls.Add(splitContainerLeafSide);
            splitContainerLeafSide.Dock = DockStyle.Fill;
            contentObjects.Controls.Add(panelObjects);
            panelObjects.Dock = DockStyle.Fill;
            contentPropertyGrid.Controls.Add(propertyGridLeaf);
            propertyGridLeaf.Dock = DockStyle.Fill;
            contentMasterView.Controls.Add(panelMasterView);
            panelMasterView.Dock = DockStyle.Fill;
            //
            contentMain.Show(dockPanel1, DockState.Document);
            contentObjects.Show(contentMain.Pane, DockAlignment.Left, Properties.Settings.Default.ProportionLeafObjects);
            contentPropertyGrid.Show(contentObjects.Pane, DockAlignment.Bottom, Properties.Settings.Default.ProportionLeafPropertyGrid);
            contentMasterView.Show(contentMain.Pane, DockAlignment.Top, Properties.Settings.Default.ProportionLeafMasterView);
            //
            leaftoolsToolStrip.Renderer = TCLE.LeafToolStripOverride;
            toolstripMasterView.Renderer = TCLE.LeafToolStripOverride;
            leafToolStrip.Renderer = TCLE.LeafToolStripOverride;
            contextMenuInterps.Renderer = TCLE.LeafContextMenuColors;
            trackEditor.MouseWheel += new MouseEventHandler(trackEditor_MouseWheel);
            dgvMasterView.MouseWheel += new MouseEventHandler(dgvMasterView_MouseWheel);
            TCLE.DoubleBufferDGV(trackEditor);
            TCLE.DoubleBufferDGV(dgvMasterView);
            textEditor.Language = FastColoredTextBoxNS.Text.Language.JSON;
            //
            treeObjects.Tag = txtSearch.Text;
            SeqObjTreeBuilder.FilterTree(treeObjects, "");
            //
            trackZoom.Value = Properties.Settings.Default.ZoomHoriz;
            trackZoomVert.Value = Properties.Settings.Default.ZoomVert;
            LeafMasterView.Width = Properties.Settings.Default.ZoomHoriz;
            //LeafMasterView.Height = Properties.Settings.Default.ZoomVert;
            //
            btnLeafAutoPlace.Checked = Properties.Settings.Default.LeafOptionAutoPlace;
            btnLeafViewOptions.DropDown = TCLE.Instance.contextMenuLeafOptions;
            UpdateInterpTooltip(InterpLastUsed);
            this.dockPanel1.ResumeLayout();
            this.ResumeLayout();
        }

        private void Form_LeafEditor_Shown(object sender, EventArgs e)
        {
            vscrollbarTrackEditor_Resize();
            trackEditor.BringToFront();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!this.Saved) {
                if (MessageBox.Show("File not saved. Are you sure you want to close it and discard changes?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No) {
                    e.Cancel = true;
                }
            }
        }

        public override void ColorFormElements()
        {
            if (SimpleLoad)
                return;
            this.BackColor = Properties.Settings.Default.ColorLeafBG;
            trackEditor.BackgroundColor = Properties.Settings.Default.ColorLeafSeqBG;
            textEditor.BackColor = Properties.Settings.Default.ColorLeafRawBG;
            textEditor.ForeColor = Properties.Settings.Default.ColorLeafRawText;
            dgvMasterView.BackgroundColor = Properties.Settings.Default.ColorLeafBasicBG;
            BasicEditorPenGrid.Color = Properties.Settings.Default.ColorLeafBasicGrid;
            //
            BrushBG = new SolidBrush(Properties.Settings.Default.ColorTuningBG);
            BrushText = new SolidBrush(Properties.Settings.Default.ColorTuningFont);
            PenMaxMin = new Pen(Properties.Settings.Default.ColorTuningMaxMin, 1);
            TuningLine = new Pen(Properties.Settings.Default.ColorTuningLine, 3);
            TuningPoint = new SolidBrush(Properties.Settings.Default.ColorTuningPoint);
            //
            dgvMasterView.Invalidate();

            treeObjects.ImageList = TCLE.Instance.imageListCategoryIcons;
            //foreach (var _ColorIcon in TCLE.ColorIcons)
            //    treeObjects.ImageList.Images.Add(_ColorIcon.Key, _ColorIcon.Value);
        }

        private void dockPanel1_ActiveContentChanged(object sender, EventArgs e)
        {
            if (TCLE.IsLoadingProject || EditorIsLoading)
                return;
            
            Properties.Settings.Default.ProportionLeafObjects = contentObjects.Pane.NestedDockingStatus.Proportion;
            Properties.Settings.Default.ProportionLeafPropertyGrid = contentPropertyGrid.Pane.NestedDockingStatus.Proportion;
            Properties.Settings.Default.ProportionLeafMasterView = contentMasterView.Pane.NestedDockingStatus.Proportion;

        }
        #endregion

        #region Variables
        //Static
        public static int FrozenColumnOffset = 3;
        private static Pen PenVioletThin = new(new SolidBrush(Color.Violet), 2);
        private static Pen PenWhite = new(new SolidBrush(Color.White), 3);
        public static Font TuningFont = new("Consolas", 8);
        public static Dictionary<Color, SolidBrush> CellBackColorCache = new() {
            { Color.Gray, new(Color.Gray) },
            { Color.DarkGray, new(Color.DarkGray) },
            {Color.LightGray, new(Color.LightGray)  },
            { Color.Black, new(Color.Black) },
            { Color.White, new(Color.White) },
            { Color.FromArgb(40, 40, 40), new(Color.FromArgb(40, 40, 40)) }
        };
        //
        //Local basic vars
        public bool GlobalMute;
        public bool GlobalDisable;
        public bool GlobalExpand;
        private bool ZoomHasChanged;
        private bool ResetRowAfterEdit;
        private bool RightclickDown;
        private bool RightclickChanges;
        private bool PlaybackLoop;
        private bool RowCellPaintForeground;
        private int CurrentRow;
        private int MouseCurrentColumn;
        private int LastRowEdit;
        private int LastColumnEdit;
        private int PlaybackStart = -5;
        private int PlaybackEnd = -5;
        private string RowPrePaintError;
        //
        //Local custom class vars
        private LeafProperties _leafproperties;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LeafProperties LeafProperties
        {
            get { return _leafproperties; }
            set {
                _leafproperties = value;
                SaveCheckAndWrite(false, "Leaf Property Change");
            }
        }
        private IEnumerable<DataGridViewColumn> Columns => trackEditor.Columns.Cast<DataGridViewColumn>().Where(x => x.Index >= FrozenColumnOffset);
        public object AltSequencer;
        private List<Sequencer_Object> SequencerObjects => _leafproperties?.SequencerObjects;
        private List<SeqDataPoint> SelectedDPs = new();
        public List<int> SelectedRows = new();
        public Dictionary<string, Sequencer_Object> LeafLanes { get; private set; }
        private DeserializeDockContent m_deserializeDockContent;
        public EditorBaseSub contentPropertyGrid = new() {
            TabText = "Data Point Props.",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
        public EditorBaseSub contentMain = new() {
            TabText = "Advanced Editor",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
        public EditorBaseSub contentObjects = new() {
            TabText = "Objects",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
        public EditorBaseSub contentMasterView = new() {
            TabText = "Basic Editor",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = false,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
        #endregion

        private EditorBaseSub? GetContentFromPersistString(string persistString)
        {
            persistString = persistString.Split(';')[1];
            if (persistString is "Data Point Props.")
                return contentPropertyGrid;
            if (persistString is "Objects")
                return contentObjects;
            if (persistString is "Advanced Editor")
                return contentMain;
            if (persistString is "Basic Editor")
                return contentMasterView;

            return null;

            throw new NotImplementedException();
        }

        #region EventHandlers
        #region Scrollbars and Zoom
        private void trackEditor_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (EditorIsTuning || EditorIsLoading)
                return;
            vscrollbarTrackEditor_Resize();

            LeafLanes = SequencerObjects.Where(x => x.ParamPathBase.StartsWith("visibl")).ToDictionary(x => x.FriendlyParam);

            for (int x = e.RowIndex; x < e.RowIndex + e.RowCount; x++) {
                Sequencer_Object seq = SequencerObjects[x];
                seq[0].ToolTipText = "Enable/Disable";
                seq[1].ToolTipText = "Mute/Unmute";
                //only add tooltip if the object can have lanes
                if (seq.FriendlyLane != "none")
                    seq[2].ToolTipText = "Show/Hide Lanes";
                TimeSigHighlightSingleObject(seq, LeafProperties.TimeTopBeat);
            }
        }

        private void trackEditor_Scroll(object sender, ScrollEventArgs e)
        {
            if (ModifierKeys is Keys.Control) {
                trackEditor.Scroll -= trackEditor_Scroll;
                trackEditor.HorizontalScrollingOffset = e.OldValue;
                trackEditor.Scroll += trackEditor_Scroll;
            }
            dgvMasterView.HorizontalScrollingOffset = trackEditor.HorizontalScrollingOffset;
        }

        private void btnLeafZoom_Click(object sender, EventArgs e)
        {
            UtilAudio.PlaySound("UIselect");
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
            if (EditorIsLoading)
                return;
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
            LeafMasterView.Width = trackZoom.Value;
            LeafMasterView.InitializeAndResize(SequencerObjects.ToList(), _leafproperties);
        }

        private void trackZoomVert_Scroll(object sender, EventArgs e)
        {
            if (EditorIsLoading)
                return;
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
                    trackZoom.Value = Math.Min(trackZoom.Maximum, horiz + scrollLines);
                }
                if (ModifierKeys is Keys.Shift && e.Delta < 0) {
                    trackZoomVert.Value = Math.Max(1, vert - scrollLines);
                }
                else if (ModifierKeys is Keys.Shift && e.Delta > 0) {
                    trackZoomVert.Value = Math.Min(trackZoomVert.Maximum, vert + scrollLines);
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
            //e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //we enter this specifically after all the other row prepainting is done, so this ends up on top.
            if (RowCellPaintForeground) {
                PaintCellForeground(e);
                return;
            }

            e.Graphics.FillRectangle(CellBackColorCache[e.CellStyle.BackColor], new Rectangle(e.CellBounds.Left - 1, e.CellBounds.Top, e.CellBounds.Width + 2, e.CellBounds.Height));
            //e.Graphics.FillRectangle(new SolidBrush(e.CellStyle.BackColor), new Rectangle(e.CellBounds.Left - 1, e.CellBounds.Top, 5, 5));
            if (e.RowIndex == -1) {
                //draw column headers (beat #s)
                LeafCellPainting.DrawText(e);
                LeafCellPainting.CellPaintIcons(e, this);
                //Drawing the playback heads, start and end point triangles that exist in the header row
                LeafCellPainting.DrawPlaybackHeaders(e, PlaybackStart, PlaybackEnd, PlaybackLoop);
                return;
            }
            Sequencer_Object seqref = SequencerObjects[e.RowIndex];
            //if we're in the frozen columns or header row (-1), return after this block as there's no other special drawing to be done
            if (e.ColumnIndex < FrozenColumnOffset) {
                //LeafCellPainting.CellPaintFancy(e, trackEditor, SelectedRows, seqref);
                //LeafCellPainting.CellPaintIcons(e, this, seqref);
                return;
            }

            LeafCellPainting.DrawColors(e, SequencerObjects);
            LeafCellPainting.DrawInterpEase(e, seqref);
            //specifically paint border seperately so it appears above everything and cleans up edges a bit.
            LeafCellPainting.SetCellBorders(e, trackEditor);
            //
            LeafCellPainting.DrawLaneDividers(e, seqref.ParamPathLane);
            //This block handles font scaling to draw the value in the cell bigger/smaller
            if (seqref.FriendlyParam is "lane center" or "lane left 1" or "lane left 2" or "lane right 1" or "lane right 2")
                LeafCellPainting.DrawLaneEnds(e, seqref, LeafLanes);
            else if (seqref.FriendlyParam is "turn" or "turn_auto")
                LeafCellPainting.DrawTurnAngles(e, seqref);
            LeafCellPainting.DrawText(e, seqref);
            //LeafCellPainting.DrawSelection(e, trackEditor);
        }
        private void PaintCellForeground(DataGridViewCellPaintingEventArgs e)
        {
            Sequencer_Object seqref = e.RowIndex == -1 ? null : SequencerObjects[e.RowIndex];
            //paint the frozen column squares and their icons
            if (e.ColumnIndex < FrozenColumnOffset) {
                LeafCellPainting.CellPaintFancy(e, trackEditor, SelectedRows, seqref);
                LeafCellPainting.CellPaintIcons(e, this, seqref);
            }
            //draw a vertical line inside tuning layer row to show where selected cell is.
            else {
                //Painting playback head and end
                LeafCellPainting.DrawPlaybackBars(e, PlaybackStart, PlaybackEnd, PlaybackLoop);
                //
                LeafCellPainting.DrawSelection(e, trackEditor);
                //if (trackEditor[e.ColumnIndex, e.RowIndex] == trackEditor.CurrentCell && (seqref.Category == "PLAY SAMPLE" || seqref.ObjName == "_TuningLayerX"))
                    //e.Graphics.DrawLine(PenVioletThin, e.CellBounds.Left + (e.CellBounds.Width / 2), e.CellBounds.Top, e.CellBounds.Left + (e.CellBounds.Width / 2), e.CellBounds.Bottom);
            }
        }

        private void trackEditor_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            //setting handled True prevents the app from performing any drawing automatically.
            //I get to handle it all, in the order I need
            e.Handled = true;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            RowPrePaintError = null;

            if (SequencerObjects[e.RowIndex].Default.Category == "PLAY SAMPLE" && Properties.Settings.Default.LeafOptionShowWave)
                PaintRowWaveforms(e);
            else if (SequencerObjects[e.RowIndex].Default.TrailLength > 1)
                PaintRowLongObject(e);
            else if (SequencerObjects[e.RowIndex].ObjName == "_TuningLayerX")
                PaintRowTuningLayer(e);
            else
                PaintRowNormal(e);

            PaintForeground(e);
            //if (Playback.IsPlaying && e.RowIndex == SequencerObjects.Last(x => x.Visible).Index) 
                //PaintRowPlayback(e);

            RowCellPaintForeground = true;
            e.PaintHeader(true);
            RowCellPaintForeground = false;
            if (RowPrePaintError != null) {
                MessageBox.Show(RowPrePaintError, "Lumper Eustum Tevel Cditor");
            }
        }
        private void PaintForeground(DataGridViewRowPrePaintEventArgs e)
        {
            RowCellPaintForeground = true;
            e.PaintCells(e.RowBounds, e.PaintParts);
            RowCellPaintForeground = false;
        }
        private void PaintRowWaveforms(DataGridViewRowPrePaintEventArgs e)
        {
            e.PaintCells(e.RowBounds, e.PaintParts);

            if (!SequencerObjects[e.RowIndex].Cells.Cast<SeqDataPoint>().Any(x => x.Value != null)) {
                return;
            }
            //setup variables to reference later when needed
            int offsetportion = UtilMath.GetTrackOffset(trackEditor);
            int columnindex = trackEditor.FirstDisplayedScrollingColumnIndex - FrozenColumnOffset + 1;
            Sequencer_Object seqref = SequencerObjects[e.RowIndex];
            SampleData samp = TCLE.ProjectSamples[seqref.ObjName];
            if (samp == null) {
                if (!seqref.HasShownError) {
                    RowPrePaintError = $@"{seqref.ObjName} does not exist in any .samp file in this project. Please add it, or remove the object in this leaf.";
                    seqref.HasShownError = true;
                }
                return;
            }
            //export pc file to playable file
            if (samp.wave == null) {
                samp.CalculateRuntime();
            }
            //CalculateRuntime can fail. In that case, skip drawing the waveform
            if (samp.wave != null) {
                int cellwidth = trackZoom.Value;
                int wavewidth = (int)Math.Floor(cellwidth * samp.beats);
                samp.wave.ColorBackground = seqref.ReadOnly ? Color.FromArgb(45, 45, 45) : seqref.HighlightColor;
                //if object has no drawn wave, create it. Wave is null whenever cell sizes change
                if (seqref.WaveBitmap == null) {
                    Bitmap WaveToDraw = samp.wave.CreateBitmap(wavewidth, e.RowBounds.Height - 4, -1, -1, true);
                    seqref.WaveBitmap = WaveToDraw;
                }
                if (seqref.WaveBitmap != null) {
                    //once the bitmap is created, now we can do some funky stuff
                    foreach (SeqDataPoint sdp in seqref.Cells.Cast<SeqDataPoint>().Where(x => x.Value != null)) {
                        //skip drawing the waveform if its offscreen to the right
                        if (sdp.beat > columnindex + trackEditor.DisplayedColumnCount(true) || sdp.beat + samp.beats < columnindex)
                            continue;
                        //math to offset drawing the wave horizontally based on where the active beats are
                        e.Graphics.DrawImage(seqref.WaveBitmap, ((sdp.beat - columnindex) * cellwidth) + offsetportion + 3, e.RowBounds.Top + 3, wavewidth - 6, e.RowBounds.Height - 6);
                        e.Graphics.DrawRoundedRectangle(PenWhite, new Rectangle(((sdp.beat - columnindex) * cellwidth) + offsetportion + 2, e.RowBounds.Top + 2, wavewidth - 4, e.RowBounds.Height - 4), Math.Min(10, (e.RowBounds.Height - 4) / 2));
                    }
                }
            }

            if (samp.message != null) {
                RowPrePaintError = samp.message;
                samp.message = null;
            }
        }
        private void PaintRowLongObject(DataGridViewRowPrePaintEventArgs e)
        {
            //RowPrePainting = true;
            e.PaintCells(e.RowBounds, e.PaintParts);
            //RowPrePainting = false;

            ///if (!SequencerObjects[e.RowIndex].data_points.Any(x => x.value != null))
            ///    goto paintheader;
            int LengthOfObject = SequencerObjects[e.RowIndex].Default.TrailLength;
            if (LengthOfObject == 0) {
                return;
            }
            LengthOfObject--;
            int offsetportion = UtilMath.GetTrackOffset(trackEditor);
            int columnindex = trackEditor.FirstDisplayedScrollingColumnIndex - FrozenColumnOffset;
            Sequencer_Object seqref = SequencerObjects[e.RowIndex];
            int cellwidth = trackZoom.Value;
            Color alpha = seqref.HighlightColor;
            alpha = seqref.ReadOnly ? Color.Gray : Color.FromArgb(100, alpha.R, alpha.G, alpha.B);
            using SolidBrush alphaBrush = new(alpha);
            //
            if (Properties.Settings.Default.LeafOptionThinBars && seqref.FriendlyLane == "lane center" && seqref.ExpandLanesInEditor == false) {
                DrawLaneTrail(e, -2, columnindex, cellwidth, offsetportion, 0, LengthOfObject, alphaBrush);
                DrawLaneTrail(e, -1, columnindex, cellwidth, offsetportion, e.RowBounds.Height / 5, LengthOfObject, alphaBrush);
                DrawLaneTrail(e, 0, columnindex, cellwidth, offsetportion, e.RowBounds.Height / 5 * 2, LengthOfObject, alphaBrush);
                DrawLaneTrail(e, 1, columnindex, cellwidth, offsetportion, e.RowBounds.Height / 5 * 3, LengthOfObject, alphaBrush);
                DrawLaneTrail(e, 2, columnindex, cellwidth, offsetportion, e.RowBounds.Height / 5 * 4, LengthOfObject, alphaBrush);
            }
            else {
                int trailstop = 0;
                foreach (SeqDataPoint sdp in seqref.Cells.Cast<SeqDataPoint>().Where(x => x.Value != null)) {
                    //don't draw trail if it already has has happened from a previous one
                    if (sdp.beat > columnindex + trackEditor.DisplayedColumnCount(true) && sdp.beat + LengthOfObject < columnindex) continue;
                    if (sdp.beat < trailstop)
                        e.Graphics.FillRectangle(alphaBrush, ((trailstop - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + 2, (LengthOfObject - (trailstop - sdp.beat)) * cellwidth, e.RowBounds.Height - 4);
                    else
                        e.Graphics.FillRectangle(alphaBrush, ((sdp.beat - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + 2, LengthOfObject * cellwidth, e.RowBounds.Height - 4);
                    trailstop = sdp.beat + LengthOfObject;
                }
            }
        }
        private void DrawLaneTrail(DataGridViewRowPrePaintEventArgs e, int LaneOffset, int columnindex, int cellwidth, int offsetportion, int verticaloffset, int LengthOfObject, SolidBrush alphaBrush)
        {
            int trailstop = 0;
            foreach (SeqDataPoint sdp in SequencerObjects[e.RowIndex + LaneOffset].Cells) {
                if (sdp.Value == null)
                    continue;
                //skip datapoints that are offscreen
                if (sdp.beat + LengthOfObject < columnindex)
                    continue;
                if (sdp.beat > columnindex + trackEditor.DisplayedColumnCount(true))
                    break;
                //don't draw trail if it already has has happened from a previous one
                if (sdp.beat > columnindex + trackEditor.DisplayedColumnCount(true) && sdp.beat + LengthOfObject < columnindex) continue;
                if (sdp.beat < trailstop)
                    e.Graphics.FillRectangle(alphaBrush, ((trailstop - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + verticaloffset, (LengthOfObject - (trailstop - sdp.beat)) * cellwidth, e.RowBounds.Height / 5);
                else
                    e.Graphics.FillRectangle(alphaBrush, ((sdp.beat - columnindex) * cellwidth) + offsetportion, e.RowBounds.Top + verticaloffset, LengthOfObject * cellwidth, e.RowBounds.Height / 5);
                trailstop = sdp.beat + LengthOfObject;
            }
        }
        private static SolidBrush BrushBG = new(Properties.Settings.Default.ColorTuningBG);
        private static SolidBrush BrushText = new(Properties.Settings.Default.ColorTuningFont);
        private static Pen PenMaxMin = new(Properties.Settings.Default.ColorTuningMaxMin, 1);
        private static Pen TuningLine = new(Properties.Settings.Default.ColorTuningLine, 3);
        private static SolidBrush TuningPoint = new(Properties.Settings.Default.ColorTuningPoint);
        private readonly record struct BezierPreset(
            float ControlPoint1,
            bool Y1IsStart,
            float ControlPoint2,
            bool Y2IsStart);
        private static readonly Dictionary<(string Interp, string Ease), BezierPreset> BezierLookup = new()
        {
            { ("Quadratic", "Ease In"),     new(0.11f, true,  0.50f, true) },
            { ("Quadratic", "Ease Out"),    new(0.50f, false, 0.89f, false) },
            { ("Quadratic", "Ease In Out"), new(0.45f, true,  0.55f, false) },

            { ("Cubic", "Ease In"),         new(0.32f, true,  0.67f, true) },
            { ("Cubic", "Ease Out"),        new(0.33f, false, 0.68f, false) },
            { ("Cubic", "Ease In Out"),     new(0.65f, true,  0.35f, false) },

            { ("Quartic", "Ease In"),       new(0.50f, true,  0.75f, true) },
            { ("Quartic", "Ease Out"),      new(0.25f, false, 0.50f, false) },
            { ("Quartic", "Ease In Out"),   new(0.76f, true,  0.24f, false) },

            { ("Quintic", "Ease In"),       new(0.64f, true,  0.78f, true) },
            { ("Quintic", "Ease Out"),      new(0.22f, false, 0.36f, false) },
            { ("Quintic", "Ease In Out"),   new(0.83f, true,  0.17f, false) },

            { ("Sine", "Ease In"),          new(0.12f, true,  0.39f, false) },
            { ("Sine", "Ease Out"),         new(0.61f, false, 0.88f, false) },
            { ("Sine", "Ease In Out"),      new(0.37f, true,  0.63f, false) },
        };
        private void PaintRowTuningLayer(DataGridViewRowPrePaintEventArgs e)
        {
            e.PaintCells(e.RowBounds, e.PaintParts);

            Sequencer_Object seqref = SequencerObjects[e.RowIndex];
            //skip drawing graphs if object disabled
            if (!seqref.EnabledInEditor)
                return;
            List<SeqDataPoint> _datapoints = seqref.Cells.Cast<SeqDataPoint>().Where(x => x.Value != null).ToList();
            if (_datapoints.Count == 0)
                return;
            //setup variables to reference later when needed
            int offsetportion = UtilMath.GetTrackOffset(trackEditor);
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
            if (endX < 0 || startX > trackEditor.Width)
                return;
            PointF[] _drawingpoints = _datapoints.Select(p => new PointF(ConvertRange(_datapoints[0].beat, _datapoints[^1].beat, startX, endX - cellwidth, p.beat) + cellwidth / 2, ConvertRange(min, max, e.RowBounds.Bottom - 7, e.RowBounds.Top + 7, (float)(decimal)p.Value))).ToArray();
            //Draw frame and boundary lines for the graphs
            e.Graphics.FillRoundedRectangle(Brushes.White, new(startX, e.RowBounds.Top, length, e.RowBounds.Height), 10);
            e.Graphics.FillRoundedRectangle(BrushBG, new(startX + 2, e.RowBounds.Top + 2, length - 4, e.RowBounds.Height - 4), 10);
            e.Graphics.DrawLine(PenMaxMin, startX + 3, e.RowBounds.Top + 7, endX - 3, e.RowBounds.Top + 7);
            e.Graphics.DrawLine(PenMaxMin, startX + 3, e.RowBounds.Bottom - 7, endX - 3, e.RowBounds.Bottom - 7);
            e.Graphics.DrawLine(PenMaxMin, startX + 3, e.RowBounds.Top + e.RowBounds.Height / 2, endX - 3, e.RowBounds.Top + e.RowBounds.Height / 2);
            e.Graphics.DrawString($"{max}", TuningFont, BrushText, startX + 3, e.RowBounds.Top + 8);
            e.Graphics.DrawString($"{min}", TuningFont, BrushText, startX + 3, e.RowBounds.Bottom - 15);
            PointF midpoint = new();
            PointF midpoint2 = new();
            //
            for (int x = 0; x < _datapoints.Count; x++) {
                if (x == _datapoints.Count - 1) {
                    e.Graphics.FillRectangle(TuningPoint, _drawingpoints[x].X - 4, _drawingpoints[x].Y - 4, 9, 9);
                    continue;
                }
                float distance = _drawingpoints[x + 1].X - _drawingpoints[x].X;
                //calculate bezier control points to draw the proper curves
                switch (_datapoints[x].Interpolation) {
                    case ("Step"):
                        e.Graphics.DrawLine(TuningLine, _drawingpoints[x], new(_drawingpoints[x + 1].X, _drawingpoints[x].Y));
                        e.Graphics.DrawLine(TuningLine, new(_drawingpoints[x + 1].X, _drawingpoints[x].Y), _drawingpoints[x + 1]);
                        break;
                    case ("Linear"):
                        midpoint = new PointF(_drawingpoints[x].X, _drawingpoints[x].Y);
                        midpoint2 = new PointF(_drawingpoints[x + 1].X, _drawingpoints[x + 1].Y);
                        break;
                        //if not step or linear, use the bezier lookup to get the curves
                    default:
                        if (!BezierLookup.TryGetValue((_datapoints[x].Interpolation, _datapoints[x].Ease), out BezierPreset BezierPreset))
                            continue;
                        midpoint = new PointF(_drawingpoints[x].X + (distance * BezierPreset.ControlPoint1), BezierPreset.Y1IsStart ? _drawingpoints[x].Y : _drawingpoints[x + 1].Y);
                        midpoint2 = new PointF(_drawingpoints[x].X + (distance * BezierPreset.ControlPoint2), BezierPreset.Y2IsStart ? _drawingpoints[x].Y : _drawingpoints[x + 1].Y);
                        break;
                }
                if (!_datapoints[x].Interpolation.Contains("step", StringComparison.OrdinalIgnoreCase) && _datapoints[x].Interpolation != "None")
                    e.Graphics.DrawBezier(TuningLine, _drawingpoints[x], midpoint, midpoint2, _drawingpoints[x + 1]);
                e.Graphics.FillRectangle(TuningPoint, _drawingpoints[x].X - 4, _drawingpoints[x].Y - 4, 9, 9);
            }
        }
        private void PaintRowNormal(DataGridViewRowPrePaintEventArgs e)
        {
            e.PaintCells(e.RowBounds, e.PaintParts);
        }
        private void PaintRowPlayback(DataGridViewRowPrePaintEventArgs e)
        {
            /*e.Graphics.DrawLine(LeafCellPainting.PenVioletThick,
                e.RowBounds.Left + ((Playback.PlaybackBeat + FrozenColumnOffset - Playback.GlobalCurrentOffset + 7) * trackZoom.Value) + (int)(trackZoom.Value * Playback.PlaybackSubBeat) - trackEditor.HorizontalScrollingOffset,
                -130,
                e.RowBounds.Left + ((Playback.PlaybackBeat + FrozenColumnOffset - Playback.GlobalCurrentOffset + 7) * trackZoom.Value) + (int)(trackZoom.Value * Playback.PlaybackSubBeat) - trackEditor.HorizontalScrollingOffset,
                e.RowBounds.Bottom);*/
            PaintForeground(e);
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
            SelectedDPs.Clear();
            if (trackEditor.SelectedCells.Count == 0)
                return;
            foreach (DataGridViewCell dgvc in trackEditor.SelectedCells) {
                //check if index out of bounds
                if (dgvc.ColumnIndex < FrozenColumnOffset)
                    continue;
                SelectedDPs.Add(SequencerObjects[dgvc.RowIndex][dgvc.ColumnIndex]);
            }
            //update the properties panel to show the selected object
            _leafproperties.selectedobj = SequencerObjects[trackEditor.SelectedCells[^1].RowIndex];
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
            if (EditorIsProcessing || Control.MouseButtons == MouseButtons.Left)
                return;
            CurrentRow = e.RowIndex;
            ShowRawTrackData(SequencerObjects[e.RowIndex]);
            LeafProperties.selectedobj = SequencerObjects[e.RowIndex];
        }

        //Cell value changed
        private void trackEditor_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
                return;
            if (trackEditor.IsCurrentCellInEditMode) {
                ApplyCellValueChanges(trackEditor[e.ColumnIndex, e.RowIndex]);
            }
        }
        public void ApplyCellValueChanges(DataGridViewCell StartCell, bool setnull = false)
        {
            //If certain actions going on, don't bother running this method.
            if (EditorIsProcessing) return;
            EditorIsLoading = true;

            object _val = null;
            if (!setnull && Decimal.TryParse(StartCell.EditedFormattedValue?.ToString(), out decimal _valtoset))
                _val = UtilMath.TruncateDecimal(_valtoset, 3);

            List<DataGridViewCell> CellsToChange = new();
            if (StartCell.Selected)
                CellsToChange = trackEditor.SelectedCells.Cast<DataGridViewCell>().ToList();
            else
                CellsToChange.Add(StartCell);

            foreach (DataGridViewCell _cell in CellsToChange) {
                //skip readonly and hidden cells
                if (_cell.ReadOnly || !_cell.OwningRow.Visible)
                    continue;

                if (_val == null)
                    CellValueNull(_cell);
                else
                    _cell.Value = _val;
            }

            EditorIsLoading = false;
            SaveCheckAndWrite(false, "Cell Value(s) Updated");
            ShowRawTrackData(SequencerObjects[StartCell.RowIndex]);
        }

        private void CellValueNull(DataGridViewCell _cell)
        {
            ResetCell(SequencerObjects[_cell.RowIndex][_cell.ColumnIndex]);

            if (SequencerObjects[_cell.RowIndex].ExpandLanesInEditor == false && SequencerObjects[_cell.RowIndex].FriendlyLane == "lane center") {
                foreach (int laneoffset in new[] { -2, -1, 0, 1, 2}) {
                    ResetCell(SequencerObjects[_cell.RowIndex + laneoffset][_cell.ColumnIndex]);
                }
            }
        }
        private static void ResetCell(SeqDataPoint sdp)
        {
            sdp.Reset();
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
                    seq.EnabledInEditor = !GlobalDisable;
                    RowReadOnly(seq, !seq.EnabledInEditor);
                }
                //invalidate the column to repaint it, so images update
                trackEditor.InvalidateColumn(0);
                UtilAudio.PlaySound("UIselect");
            }
            //test if column header was clicked for global mute
            else if (e.RowIndex == -1 && e.ColumnIndex == 1) {
                GlobalMute = !GlobalMute;
                foreach (Sequencer_Object seq in SequencerObjects) {
                    seq.MuteInEditor = GlobalMute;
                }
                //invalidate the column to repaint it, so images update
                trackEditor.InvalidateColumn(1);
                UtilAudio.PlaySound("UIselect");
            }
            //test if column header was clicked for global expand
            else if (e.RowIndex == -1 && e.ColumnIndex == 2) {
                //if ShowLanes, don't alter lane visibility
                if (Properties.Settings.Default.LeafOptionShowLane)
                    return;
                GlobalExpand = !GlobalExpand;
                foreach (Sequencer_Object seq in SequencerObjects) {
                    seq.ExpandLanesInEditor = GlobalExpand;
                }
                //invalidate the column to repaint it, so images update
                trackEditor.InvalidateColumn(2);
                UtilAudio.PlaySound("UIselect");
            }
            else if (e.RowIndex == -1) {
                if (e.Button == MouseButtons.Right) {
                    if (PlaybackEnd == e.ColumnIndex) {
                        if (PlaybackLoop) {
                            PlaybackLoop = false;
                            PlaybackEnd = -5;
                            trackEditor.Invalidate();
                        }
                        else {
                            PlaybackLoop = true;
                            trackEditor.Invalidate();
                        }
                    }
                    else {
                        PlaybackEnd = e.ColumnIndex;
                        if (PlaybackEnd < PlaybackStart)
                            PlaybackEnd = PlaybackStart;
                        trackEditor.Invalidate();
                    }
                }
                else {
                    if (PlaybackStart == e.ColumnIndex) {
                        PlaybackStart = -5;
                        trackEditor.Invalidate();
                    }
                    else {
                        PlaybackStart = e.ColumnIndex;
                        if (PlaybackEnd > -1 && PlaybackEnd <= PlaybackStart)
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
                    seq.EnabledInEditor = !seq.EnabledInEditor;
                    RowReadOnly(seq, !seq.EnabledInEditor);
                    UtilAudio.PlaySound("UIselect");
                }
                if (e.ColumnIndex is 1) {
                    seq.MuteInEditor = !seq.MuteInEditor;
                    UtilAudio.PlaySound("UIselect");
                }
                if (e.ColumnIndex is 2 && seq.FriendlyLane == "lane center") {
                    //if ShowLanes, don't alter lane visibility
                    if (Properties.Settings.Default.LeafOptionShowLane)
                        return;
                    //FindMissingLaneObjects(seq);
                    seq.ExpandLanesInEditor = !seq.ExpandLanesInEditor;
                    SequencerObjects[seq.Index - 2].ExpandLanesInEditor = seq.ExpandLanesInEditor;
                    SequencerObjects[seq.Index - 1].ExpandLanesInEditor = seq.ExpandLanesInEditor;
                    SequencerObjects[seq.Index + 1].ExpandLanesInEditor = seq.ExpandLanesInEditor;
                    SequencerObjects[seq.Index + 2].ExpandLanesInEditor = seq.ExpandLanesInEditor;
                    UtilAudio.PlaySound("UIselect");
                }
                trackEditor[e.ColumnIndex, e.RowIndex].Selected = false;
                //invalidate cell to repaint it to update the images
                trackEditor.InvalidateCell(trackEditor[e.ColumnIndex, e.RowIndex]);
            }
            else if (e.Button == MouseButtons.Left && btnLeafAutoPlace.Checked) {
                if (SequencerObjects[e.RowIndex].Default.TraitType is DefaultSequencerObject.Trait.Bool or DefaultSequencerObject.Trait.Action)
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
                    ApplyCellValueChanges(trackEditor[e.ColumnIndex, e.RowIndex]);
                    RightclickChanges = true;
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
                if (e.RowIndex != -1)
                    trackEditor.InvalidateRow(e.RowIndex);
            }
            else if (e.Button == MouseButtons.Left) {
                //get all selected cells and display them grouped together in the propertygrid
                //this allows for mass editing
                trackEditor.Invalidate();
                SelectedDPs.Clear();
                if (trackEditor.SelectedCells.Count == 0)
                    return;
                foreach (DataGridViewCell dgvc in trackEditor.SelectedCells) {
                    //check if index out of bounds
                    if (dgvc.ColumnIndex < FrozenColumnOffset)
                        continue;
                    SelectedDPs.Add(SequencerObjects[dgvc.RowIndex][dgvc.ColumnIndex]);
                }
                //update the properties panel to show the selected object
                _leafproperties.selectedobj = SequencerObjects[trackEditor.SelectedCells[^1].RowIndex];
                TCLE.dockProjectProperties.propertyGridProject.SelectedObject = GetProperties();
                TCLE.dockProjectProperties.propertyGridProject.Refresh();
                propertyGridLeaf.SelectedObjects = SelectedDPs.ToArray();
                propertyGridLeaf.Refresh();
            }
        }

        private void trackEditor_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            MouseCurrentColumn = e.ColumnIndex;
            if (e.ColumnIndex == -1 || e.RowIndex == -1) {
                LeafCellPainting.HoverCell = null;
                return;
            }
            LeafCellPainting.HoverCell = trackEditor[e.ColumnIndex, e.RowIndex];
            if (e.ColumnIndex < FrozenColumnOffset)
                trackEditor.InvalidateCell(LeafCellPainting.HoverCell);

            DataGridView dgv = sender as DataGridView;
            if (Control.MouseButtons == MouseButtons.Right) {
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
                    ApplyCellValueChanges(trackEditor[e.ColumnIndex, e.RowIndex]);
                    LogUndo = true;
                }
            }
        }

        private void trackEditor_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
        }

        private void trackEditor_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;
            if (e.ColumnIndex < FrozenColumnOffset) {
                trackEditor.InvalidateCell(e.ColumnIndex, e.RowIndex);
            }
        }
        //Keypress Backspace - clear selected cells
        private void trackEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back) {
                LogUndo = false;
                ApplyCellValueChanges(trackEditor[trackEditor.SelectedCells[^1].ColumnIndex, trackEditor.SelectedCells[^1].RowIndex], true);
                LogUndo = true;
                SaveCheckAndWrite(false, "Delete Cell Values");
            }
        }
        private void trackEditor_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //delete cell value if Delete key is pressed
            if (e.KeyCode == Keys.Delete) {
                LogUndo = false;
                ApplyCellValueChanges(trackEditor[trackEditor.SelectedCells[^1].ColumnIndex, trackEditor.SelectedCells[^1].RowIndex], true);
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
                    int indexdirection = (e.KeyCode is Keys.Right or Keys.Down) ? 1 : -1;
                    bool leftright = e.KeyCode is Keys.Left or Keys.Right;
                    bool shifted = false;
                    //sort cells in selection based on column. depends on direction, reverse collection.
                    //this processing order is important so cells dont overwrite each other when moving
                    List<SeqDataPoint> SelectedCells;
                    if (indexdirection == -1)
                        SelectedCells = trackEditor.SelectedCells.Cast<SeqDataPoint>().OrderBy(c => leftright ? c.ColumnIndex : c.RowIndex).ToList();
                    else
                        SelectedCells = trackEditor.SelectedCells.Cast<SeqDataPoint>().OrderByDescending(c => leftright ? c.ColumnIndex : c.RowIndex).ToList();

                    LogUndo = false;
                    trackEditor.SelectionChanged -= trackEditor_SelectionChanged;
                    trackEditor.ClearSelection();
                    //iterate over each in the selection
                    foreach (SeqDataPoint sdp in SelectedCells) {
                        //check if at left/right edges
                        if ((leftright && sdp.ColumnIndex + indexdirection < trackEditor.ColumnCount && sdp.ColumnIndex + indexdirection >= FrozenColumnOffset) || (!leftright && sdp.RowIndex + indexdirection < trackEditor.RowCount && sdp.RowIndex + indexdirection > -1)) {
                            shifted = true;
                            Sequencer_Object row = SequencerObjects[sdp.RowIndex];
                            SeqDataPoint destCell = SequencerObjects[sdp.RowIndex + (!leftright ? indexdirection : 0)][sdp.ColumnIndex + (leftright ? indexdirection : 0)];
                            //clone selected cell to new location
                            destCell.Value = sdp.Value;
                            destCell.Interpolation = sdp.Interpolation;
                            destCell.Ease = sdp.Ease;
                            //select the newly moved cell
                            destCell.Selected = true;
                            //clear the current cell since it moved
                            row[sdp.ColumnIndex].Value = null;
                            row[sdp.ColumnIndex].Interpolation = "Linear";
                            row[sdp.ColumnIndex].Ease = "Ease In Out";
                        }
                    }
                    trackEditor.SelectionChanged += trackEditor_SelectionChanged;
                    LogUndo = true;
                    if (shifted)
                        SaveCheckAndWrite(false, "Shift Cell Values");
                    //reselect all cells if shift did not occur, since selection was cleared earlier
                    else {
                        foreach (DataGridViewCell dgvcell in SelectedCells)
                            dgvcell.Selected = true;
                    }
                    //SaveCheckAndWrite(false, $"Shifted selected cells {(e.KeyCode == Keys.Left ? "left" : "right")}", $"");
                }
            }

            if (e.KeyData == TCLE.Keybinds["Leaf Playback"]) {
                btnTrackPlayback.PerformClick();
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 0"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValues[0];
                ApplyCellValueChanges(trackEditor.CurrentCell);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 1"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValues[1];
                ApplyCellValueChanges(trackEditor.CurrentCell);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 2"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValues[2];
                ApplyCellValueChanges(trackEditor.CurrentCell);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 3"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValues[3];
                ApplyCellValueChanges(trackEditor.CurrentCell);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 4"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValues[4];
                ApplyCellValueChanges(trackEditor.CurrentCell);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 5"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValues[5];
                ApplyCellValueChanges(trackEditor.CurrentCell);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 6"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValues[6];
                ApplyCellValueChanges(trackEditor.CurrentCell);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 7"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValues[7];
                ApplyCellValueChanges(trackEditor.CurrentCell);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 8"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValues[8];
                ApplyCellValueChanges(trackEditor.CurrentCell);
            }
            else if (e.KeyData == TCLE.Keybinds["Quick Value 9"]) {
                trackEditor.CurrentCell.Value = TCLE.LeafQuickValues[9];
                ApplyCellValueChanges(trackEditor.CurrentCell);
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
            if (e.RowIndex == -1)
                return;
            int lastpos = trackEditor.FirstDisplayedScrollingColumnIndex;
            if (trackEditor.FirstDisplayedScrollingColumnIndex == -1)
                return;

            if (ModifierKeys is Keys.Shift) {
                foreach (DataGridViewCell dgvc in trackEditor.Rows[e.RowIndex].Cells)
                    dgvc.Selected = true;
            }
            else {
                trackEditor.CurrentCell = trackEditor[trackEditor.CurrentCell.ColumnIndex < FrozenColumnOffset ? FrozenColumnOffset : trackEditor.CurrentCell.ColumnIndex, e.RowIndex];
                trackEditor.FirstDisplayedScrollingColumnIndex = lastpos;
                trackEditor.Invalidate();

                if (e.Button == MouseButtons.Right) {
                    contextMenuObj.Show(MousePosition.X, MousePosition.Y);
                    return;
                }
            }
        }

        private void contextMenuObj_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            toolstripObjTune.Enabled = SequencerObjects[trackEditor.CurrentRow.Index].Default.TraitType is DefaultSequencerObject.Trait.Float;
        }

        private void trackEditor_RowHeadersWidthChanged(object sender, EventArgs e)
        {
            _ = trackEditor.RowHeadersWidth;
            trackEditor_Resize(null, null);

            toolstripMasterView.Width = leafToolStrip.Width + trackEditor.RowHeadersWidth + (trackEditor.Columns[0].Width * 3);
        }

        ///LEAF - NEW
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((!this.Saved && MessageBox.Show("Current leaf is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || this.Saved) {
                SaveAs();
            }
        }

        private void treeObjects_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Text == "*FAVORITES*" || e.Node.Nodes.Count > 0 || treeObjects.SelectedNode.Nodes.Count > 0 || e.Button == MouseButtons.Right)
                return;

            DefaultSequencerObject objmatch = e.Node.Tag is DefaultSequencerObject _obj ? _obj : TCLE.LeafObjects[(string)e.Node.Tag];
            if (e.Node.Text.EndsWith(".samp"))
                objmatch = TCLE.LeafObjects["sample.samp;play"];
            if (objmatch == null)
                return;

            AddToSequencer(objmatch, e.Node.Text);
            SaveCheckAndWrite(false, "Add Object");
            UtilAudio.PlaySound("UIobjectadd");
        }

        private Sequencer_Object AddToSequencer(DefaultSequencerObject ObjToAdd, string SampleName = null)
        {
            Sequencer_Object seq = new(LeafProperties, ObjToAdd) {
                ParentLeaf = LeafProperties,
                ObjName = ObjToAdd.Category == "PLAY SAMPLE" ? SampleName : ObjToAdd.Name,
                ParamPath = ObjToAdd.ParamPath,
                FriendlyParam = ObjToAdd.ParamDisplayName,
                DefaultValue = ObjToAdd.DefaultValue,
                Step = ObjToAdd.Step,
                HighlightColor = ObjToAdd.DefaultColor,
                highlight_value = 0,
                EnabledInEditor = true
            };
            if (seq.ObjName == "leafname")
                seq.ObjName = this.WorkingFile.Name;
            if (seq.Default.Category == "LOOP TRACK VOLUME") {
                int audiochannels = SequencerObjects.Count(x => x.Default.Category == "LOOP TRACK VOLUME");
                seq.ParamPath = seq.ParamPath.Replace("x", $"{audiochannels}");
                seq.FriendlyParam = seq.FriendlyParam.Replace("x", $"{audiochannels}");
            }
            seq.ExpandLanesInEditor = seq.FriendlyLane == "none" || Properties.Settings.Default.LeafOptionShowLane;

            if (!EditorIsRandomizing && SequencerObjects.Any(x => x.ObjName == seq.ObjName && x.ParamPath == seq.ParamPath)) {
                if (MessageBox.Show($"WARNING\nThis leaf already has a {seq.ObjName}-{seq.ParamPath} object. Do you still want to add another one?", "TCLEEEEEEEEEEEEEEE", MessageBoxButtons.YesNo) == DialogResult.No)
                    return null;
            }
            //if object is multilane, need to add all its lanes
            if (seq.FriendlyLane == "lane center") {
                List<Sequencer_Object> LanesToAdd = LoadMultiLanes(seq, SequencerObjects);
                SequencerObjects.AddRange(LanesToAdd);
                trackEditor.Rows.AddRange(LanesToAdd.ToArray());
            }
            else {
                SequencerObjects.Add(seq);
                trackEditor.Rows.Add(seq);
            }
            SetRowHeaderText(seq);
            return seq;
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
                SampleData SampToPlay = TCLE.ProjectSamples[treeObjects.SelectedNode.Text];
                if (SampToPlay == null || SamplePlaying == SampToPlay)
                    return;

                if (SampToPlay.TempFile == null) {
                    string SampleToPlay = UtilAudio.PCtoAudioFile(SampToPlay);
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
        #endregion

        #region Buttons
        ///         ///
        /// BUTTONS ///
        ///         ///
        private void btnTrackConvert_Click(object sender, EventArgs e)
        {
            if (treeObjects.SelectedNode.Nodes.Count > 0 || trackEditor.SelectedCells.Count == 0)
                return;
            DefaultSequencerObject objmatch = TCLE.LeafObjects[(string)treeObjects.SelectedNode.Tag];
            if (objmatch == null) {
                if (treeObjects.SelectedNode.Text.EndsWith(".samp")) {
                    objmatch = TCLE.LeafObjects["sample.samp;play"];
                }
                else
                    return;
            }
            Sequencer_Object _currentseq = SequencerObjects[CurrentRow];
            if (!objmatch.ParamPath.EndsWith(".ent") && _currentseq.FriendlyLane != "none") {
                MessageBox.Show("Due to reasons, you cannot change a multi-lane object into a non-multi-lane object. Please just add a new object.", "Thumper Custom Level Editor");
                return;
            }
            Sequencer_Object[] Lanes = SequencerObjects.GetRange(_currentseq.Index + _currentseq.LaneOffsetFromTop, (_currentseq.FriendlyLane != "none" ? 5 : 1)).ToArray();// Where(x => x.category == _currentseq.category && x.friendly_param == _currentseq.friendly_param).ToArray();
            for (int x = 0; x < Lanes.Length; x++) {
                Lanes[x].Default = objmatch;
                Lanes[x].ObjName = objmatch.Category == "PLAY SAMPLE" ? treeObjects.SelectedNode.Text : objmatch.Name;
                Lanes[x].ParamPath = objmatch.ParamPath;
                Lanes[x].FriendlyParam = objmatch.ParamDisplayName;
                Lanes[x].HighlightColor = objmatch.DefaultColor;
                if (Lanes[x].ObjName == "leafname")
                    Lanes[x].ObjName = this.WorkingFile.Name;
                SetRowHeaderText(Lanes[x]);
            }
            //
            if (Lanes.Length == 1 && Lanes[0].FriendlyLane != "none")
                LoadMultiLanes(Lanes[0], SequencerObjects);
            //FindMissingLaneObjects(SequencerObjects[CurrentRow]);
            trackEditor.InvalidateRow(_currentseq.Index);

            SaveCheckAndWrite(false, "Add Object");
            UtilAudio.PlaySound("UIobjectadd");
        }

        private void btnTrackDelete_Click(object sender, EventArgs e)
        {
            if (CurrentRow < 0)
                return;
            trackEditor.SuspendLayout();
            //If multiple rows are selected, get all of them in a list. Then loop over list, deleting each one
            List<Sequencer_Object> selectedrows = trackEditor.SelectedCells.Cast<DataGridViewCell>().Where(cell => cell.OwningRow.Visible).Select(cell => cell.RowIndex).Distinct().Select(x => SequencerObjects[x]).ToList();
            if (MessageBox.Show($"{selectedrows.Count} Sequencer objects selected.\nAre you sure you want to delete them?", "Confirm?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            while (selectedrows.Count > 0) {
                //if object is multilane, delete its other lanes too
                Sequencer_Object[] Lanes = ReturnLanesFromName(selectedrows[0], selectedrows[0].FriendlyLane);
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
            UtilAudio.PlaySound("UIobjectremove");
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
                    RowsToMove.AddRange(ReturnLanesFromName(row, row.FriendlyLane));
            }
            //if already at the top, do not move up
            if (RowsToMove.FirstOrDefault().Index == 0)
                return;

            IEnumerable<DataGridViewCell> selectedcells = trackEditor.SelectedCells.Cast<DataGridViewCell>();
            SuspendDataGrids(true);

            for (int x = 0; x < RowsToMove.Count; x++) {
                int currentindex = RowsToMove[x].Index;
                //get the object above, and any lanes with it. We will need to move above all of them.
                Sequencer_Object ObjAbove = SequencerObjects[RowsToMove[x].Index - 1];
                int Lanes = ObjAbove.FriendlyLane != "none" ? 5 : 1;
                //remove the row and object
                trackEditor.Rows.Remove(RowsToMove[x]);
                SequencerObjects.Remove(RowsToMove[x]);
                if (RowsToMove[x].FriendlyLane != "none") {
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
                if (RowsToMove[x].FriendlyLane != "none") {
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
            SuspendDataGrids(false);

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
                    RowsToMove.AddRange(ReturnLanesFromName(row, row.FriendlyLane).Reverse());
            }
            ///RowsToMove = RowsToMove.OrderByDescending(cell => cell.editor_row.Index).ToList();
            //if already at the bottom, do not move down
            if (RowsToMove[0].Index >= trackEditor.Rows.Count - 1)
                return;

            List<DataGridViewCell> selectedcells = trackEditor.SelectedCells.Cast<DataGridViewCell>().ToList();
            SuspendDataGrids(true);

            for (int x = 0; x < RowsToMove.Count; x++) {
                int currentindex = RowsToMove[x].Index;
                //get the object above, and any lanes with it. We will need to move above all of them.
                Sequencer_Object ObjBelow = SequencerObjects[RowsToMove[x].Index + 1];
                int Lanes = ObjBelow.FriendlyLane != "none" ? 5 : 1;
                //remove the row and object
                trackEditor.Rows.Remove(RowsToMove[x]);
                SequencerObjects.Remove(RowsToMove[x]);
                if (RowsToMove[x].FriendlyLane != "none") {
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
                if (RowsToMove[x].FriendlyLane != "none") {
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
            SuspendDataGrids(false);

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
                if (copyseq.FriendlyLane == "lane center") {
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
                else if (copyseq.FriendlyLane == "none") {
                    TCLE.ClipboardSequencer.Add(copyseq.Clone());
                }
            }

            foreach (EditorLeaf leaf in TCLE.Documents.Values.OfType<EditorLeaf>())
                leaf.btnTrackPaste.Enabled = true;
            UtilAudio.PlaySound("UIkcopy");
        }

        private void btnTrackPaste_Click(object sender, EventArgs e)
        {
            bool resize = true;
            int _index = trackEditor.CurrentRow?.Index ?? 0;
            if (SequencerObjects.Count > 0) {
                resize = false;
                //if pasting inside a multilane object, skip index down a few rows
                switch (SequencerObjects[_index].FriendlyLane) {
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
            }
            EditorIsPasting = true;
            //add copied Sequencer_Object to main _tracks list
            foreach (Sequencer_Object _newtrack in TCLE.ClipboardSequencer) {
                Sequencer_Object clone = _newtrack.Clone(_leafproperties.LeafLength);
                clone.ParentLeaf = LeafProperties;
                clone.ExpandLanesInEditor = GlobalExpand;
                clone.Height = trackZoomVert.Value;
                SequencerObjects.Insert(_index, clone);
                trackEditor.Rows.Insert(_index, clone);
                _index++;
            }
            if (resize)
                TCLE.ResizeHeaders(trackEditor);

            EditorIsPasting = false;
            UtilAudio.PlaySound("UIkpaste");
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
                if (seq.FriendlyLane is not "lane center" || seq.ExpandLanesInEditor) {
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
            }
            trackEditor.Invalidate();
            LogUndo = true;
            UtilAudio.PlaySound("UIdataerase");
            SaveCheckAndWrite(false, "Clear Object Values");
        }

        private void btnLeafClean_Click(object sender, EventArgs e)
        {
            //this function checks over all objects in the leaf and removes them if they have no data set
            List<Sequencer_Object> todelete = new();
            bool del;
            for (int seqindex = 0; seqindex < SequencerObjects.Count; seqindex++) {
                Sequencer_Object ObjToCheck = SequencerObjects[seqindex];
                if (ObjToCheck.FriendlyLane is not "none" and not "lane center")
                    continue;
                //if object has lanes, check all of them together. Lanes cannot exist separately from the center.
                if (ObjToCheck.FriendlyLane == "lane center") {
                    del = CheckObjectIfEmpty(SequencerObjects[seqindex - 2]) &&
                        CheckObjectIfEmpty(SequencerObjects[seqindex - 1]) &&
                        CheckObjectIfEmpty(ObjToCheck) &&
                        CheckObjectIfEmpty(SequencerObjects[seqindex + 1]) &&
                        CheckObjectIfEmpty(SequencerObjects[seqindex + 2]);
                }
                else
                    del = CheckObjectIfEmpty(ObjToCheck);

                if (del) {
                    if (ObjToCheck.FriendlyLane == "lane center") {
                        todelete.Add(SequencerObjects[seqindex - 2]);
                        todelete.Add(SequencerObjects[seqindex - 1]);
                        todelete.Add(ObjToCheck);
                        todelete.Add(SequencerObjects[seqindex + 1]);
                        todelete.Add(SequencerObjects[seqindex + 2]);
                    }
                    else
                        todelete.Add(ObjToCheck);
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
            //if any data is found, don't delete the object
            if (seq.Cells.Cast<SeqDataPoint>().Any(x => x.Value != null))
                return false;
            //locate the default object
            if (TCLE.LeafObjects.TryGetValue($"{seq.ObjName};{seq.ParamPath.Replace(seq.ParamPathLane, "ent")}", out DefaultSequencerObject? BaseObj))
            //once found, compare its default value to the leaf object's. If different, data has changed, so don't delete it.
            if (BaseObj != null && BaseObj.DefaultValue != seq.DefaultValue)
                return false;

            return true;
        }

        private void btnRawImport_Click(object sender, EventArgs e)
        {
            try {
                TrackRawImport(SequencerObjects[CurrentRow], JObject.Parse($"{{{textEditor.Text}}}"));
                UtilAudio.PlaySound("UIkpaste");
            } catch (JsonReaderException ex) {
                MessageBox.Show($"Invalid format or characters in imported data. Please fix.\n\n{ex.Message}", "Thumper Custom Editor Level");
            }
        }

        private string InterpLastUsed
        {
            get {
                return Properties.Settings.Default.LeafOptionInterp;
            }
            set {
                Properties.Settings.Default.LeafOptionInterp = value;
                Properties.Settings.Default.Save();
                foreach (EditorLeaf leaf in TCLE.Documents.Values.Where(x => x.WorkingFile.Extension.Equals(".leaf", StringComparison.OrdinalIgnoreCase))) {
                    leaf.UpdateInterpTooltip(InterpLastUsed);
                }
            }
        }
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

        private void UpdateInterpTooltip(string LastUsed)
        {
            btnLeafInterpLinear.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject($"ease_{LastUsed.Replace(" ", "_")}");
            btnLeafInterpLinear.ToolTipText = $"Interpolate values between 2 selected cells in the same row.\nUse the drop down to select different easing styles.\n=======\nLast Used: {LastUsed}\n";
        }

        private void Interpolate(string interpOption)
        {
            if (interpOption == null)
                return;
            InterpLastUsed = interpOption;
            UpdateInterpTooltip(interpOption);
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
            DataGridViewCell first = SelectedCells[0].ColumnIndex < SelectedCells[1].ColumnIndex ? SelectedCells[0] : SelectedCells[1];
            DataGridViewCell second = first == SelectedCells[0] ? SelectedCells[1] : SelectedCells[0];
            Sequencer_Object interpobject = SequencerObjects[SelectedCells[0].RowIndex];

            //get start and end values, and how many beats separate them
            double _start = (double)((decimal?)first.Value ?? (decimal)interpobject.DefaultValue);
            double _end = (double)((decimal?)second.Value ?? (decimal)interpobject.DefaultValue);
            double max = Math.Max(_start, _end);
            double min = Math.Min(_start, _end);
            double max2 = 0, max3 = 0, min2 = 0, min3 = 0;
            Color startcolor = new();
            Color endcolor = new();
            if (interpobject.Default.TraitType is DefaultSequencerObject.Trait.Color) {
                startcolor = Color.FromArgb((int)_start);
                endcolor = Color.FromArgb((int)_end);
                max = Math.Max(startcolor.R, endcolor.R);
                max2 = Math.Max(startcolor.G, endcolor.G);
                max3 = Math.Max(startcolor.B, endcolor.B);
                min = Math.Min(startcolor.R, endcolor.R);
                min2 = Math.Min(startcolor.G, endcolor.G);
                min3 = Math.Min(startcolor.B, endcolor.B);
            }
            int _beats = second.ColumnIndex - first.ColumnIndex + 1;
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

            if (interpobject.Default.TraitType is DefaultSequencerObject.Trait.Color) {
                double valR, valG, valB = 0;
                //convert interp[] range of 0 to 1 into range between selected beats
                for (int x = 0; x < interp.Length; x++) {
                    valR = (max == startcolor.R ? 1 - interp[x] : interp[x]) * (max - min) + min;
                    valG = (max2 == startcolor.G ? 1 - interp[x] : interp[x]) * (max2 - min2) + min2;
                    valB = (max3 == startcolor.B ? 1 - interp[x] : interp[x]) * (max3 - min3) + min3;
                    interp[x] = Color.FromArgb((int)valR, (int)valG, (int)valB).ToArgb();
                }
            }
            else {
                for (int x = 0; x < interp.Length; x++) {
                    //if the first cell is actually the maximum, each value needs to be flipped across the range 0 to 1
                    if (_start == max) {
                        interp[x] = 1 - interp[x];
                    }
                    //convert interp[] range of 0 to 1 into range between selected beats
                    interp[x] = interp[x] * (max - min) + min;
                }
            }
            //assign new values back to the data points
            EditorIsInterpolating = true;
            for (int x = 0; x < _beats; x++) {
                interpobject[first.ColumnIndex + x].Value = UtilMath.TruncateDecimal((decimal)interp[x], 3);
            }
            EditorIsInterpolating = false;
            //
            ShowRawTrackData(interpobject);
            UtilAudio.PlaySound("UIinterpolate");
            SaveCheckAndWrite(false, "Interpolated");
        }

        private void exampleswebLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = "https://easings.net/", UseShellExecute = true });
        }

        private void btnLeafColors_Click(object sender, EventArgs e)
        {
            //do nothing if no cells selected
            if (trackEditor.SelectedCells.Count == 0)
                return;
            UtilAudio.PlaySound("UIcoloropen");
            if (TCLE.colorDialogNew.ShowDialog() == DialogResult.OK) {
                UtilAudio.PlaySound("UIcolorapply");
                trackEditor.SelectedCells[0].Value = (decimal)TCLE.colorDialogNew.Color.ToArgb();
                ApplyCellValueChanges(trackEditor[trackEditor.SelectedCells[0].ColumnIndex, trackEditor.SelectedCells[0].RowIndex]);
            }
        }

        private void btnLeafSplit_Click(object sender, EventArgs e)
        {
            if (AltSequencer != null) {
                MessageBox.Show("Not allowed to split a lvl sequencer!", "Jumper Justum Jevel Jeditor");
                return;
            }
            if (!this.Saved) {
                MessageBox.Show("Not allowed to split a leaf with unsaved changes.", "Thump Cust Lev Edit");
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
            sfd.InitialDirectory = this.WorkingFile.DirectoryName ?? TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                SplitFile = new FileInfo(sfd.FileName);
            }
            else
                return;

            EditorIsInterpolating = true;
            try {
                File.Copy(this.WorkingFile.FullName, SplitFile.FullName);
                EditorLeaf LeafSplitAfter = (EditorLeaf)TCLE.OpenFile(SplitFile, false, true);
                //remove columns from the beginning to shift all cells backwards until they get to beat 0
                for (int x = 0; x < splitindex; x++) {
                    LeafSplitAfter.trackEditor.Columns.RemoveAt(0);
                }
                //need to rename Track Effect objects to point to the new leaf name
                foreach (Sequencer_Object _seq in LeafSplitAfter.SequencerObjects) {
                    if (_seq.ObjName == this.WorkingFile.Name)
                        _seq.ObjName = LeafSplitAfter.WorkingFile.Name;
                }
                //reduce split leafs beat count and save
                LeafSplitAfter._leafproperties.LeafLength = _leafproperties.LeafLength - splitindex;
                LeafSplitAfter.SaveCheckAndWrite(true, "");
                LeafSplitAfter.Dispose();

                //reduce beat count of the leaf that was just split and save it
                LeafProperties.LeafLength = splitindex;
                SaveCheckAndWrite(true, "");
                UtilAudio.PlaySound("UIleafsplit");
                //load new leaf that was just split
                ProjectExplorer.CreateTreeView();
                TCLE.OpenFile(SplitFile);
            } finally {
                EditorIsInterpolating = false;
            }
        }

        private void btnLeafObjRefresh_Click(object sender, EventArgs e)
        {
            ///TCLE.ImportObjects();
            UtilAudio.PlaySound("UIrefresh");
        }

        private void btnLeafAutoPlace_Click(object sender, EventArgs e)
        {
            UtilAudio.PlaySound("UIselect");
            Properties.Settings.Default.LeafOptionAutoPlace = btnLeafAutoPlace.Checked;
            Properties.Settings.Default.Save();
            foreach (EditorLeaf leaf in TCLE.Documents.Values.Where(x => x.WorkingFile.Extension.Equals("leaf", StringComparison.OrdinalIgnoreCase))) {
                leaf.btnLeafAutoPlace.Checked = Properties.Settings.Default.LeafOptionAutoPlace;
            }
        }

        private void btnLeafRandom_Click(object sender, EventArgs e)
        {
            EditorIsRandomizing = true;
            //I pick category first rather than any object, as this gives Play Sample a higher chance of being picked
            //And all the tentacles a lower chance
            List<string> categories = TCLE.LeafObjects.Select(x => x.Value.Category).Distinct().ToList();
        beginrando:
            string category = categories[TCLE.rng.Next(0, categories.Count)];
            List<DefaultSequencerObject> objects = TCLE.LeafObjects.Where(x => x.Value.Category == category).Select(x => x.Value).ToList();
            DefaultSequencerObject BaseObj = objects[TCLE.rng.Next(0, objects.Count)];
            //check if the object exists in the leaf already. If so, pick a new one
            if (SequencerObjects.Any(x => x.Default.Category == category && x.ParamPath == BaseObj.ParamPath))
                goto beginrando;

            Sequencer_Object seq = AddToSequencer(BaseObj, TCLE.ProjectSamples.ElementAt(TCLE.rng.Next(0, TCLE.ProjectSamples.Count)).Value.obj_name);
            /*Sequencer_Object seq = new(LeafProperties) {
                ParentLeaf = LeafProperties,
                ObjName = category == "PLAY SAMPLE" ? TCLE.ProjectSamples.ElementAt(TCLE.rng.Next(0, TCLE.ProjectSamples.Count)).Value.obj_name : BaseObj.obj_name,
                Category = BaseObj.category,
                ParamPath = BaseObj.param_path,
                FriendlyParam = BaseObj.param_displayname,
                DefaultValue = BaseObj.default_value,
                Step = BaseObj.step,
                TraitType = Sequencer_Object.TraitLookup[BaseObj.trait_type],
                HighlightColor = BaseObj.defaultcolor,
                highlight_value = 0,
                Footer = BaseObj.footer,
                EnabledInEditor = true,
                ExpandLanesInEditor = Properties.Settings.Default.LeafOptionShowLane
            };
            if (seq.ObjName == "leafname")
                seq.ObjName = this.WorkingFile.Name;
            if (seq.Category == "LOOP TRACK VOLUME") {
                int audiochannels = SequencerObjects.Count(x => x.Category == "LOOP TRACK VOLUME");
                seq.ParamPath = seq.ParamPath.Replace("x", $"{audiochannels}");
                seq.FriendlyParam = seq.FriendlyParam.Replace("x", $"{audiochannels}");
            }

            if (seq.FriendlyLane == "lane center") {
                LoadMultiLanes(seq, SequencerObjects, trackEditor);
            }
            else {
                SequencerObjects.Add(seq);
                trackEditor.Rows.Add(seq);
            }
            SetRowHeaderText(seq, seq.Category);*/
            //FindMissingLaneObjects(seq);

            //fill cells with random values
            do {
                if (seq.FriendlyLane == "lane center") {
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
            UtilAudio.PlaySound("UIaddrandom");
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
                .Where(x => x.FriendlyLane is "none" or "lane center");

            if (MessageBox.Show("Assign random values to the current selected Objects?", "TELdCiethovrueulsmtpoemr", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                EditorIsRandomizing = true;
                foreach (Sequencer_Object seq in SelectedSeq) {
                    do {
                        if (seq.FriendlyLane == "lane center") {
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

                UtilAudio.PlaySound("UIaddrandom");
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
        public override object GetProperties()
        {
            return _leafproperties;
        }

        public void SuspendDataGrids(bool suspend)
        {
            if (suspend) {
                trackEditor.SuspendLayout();
                trackEditor.SuspendDrawing();
                dgvMasterView.SuspendLayout();
                dgvMasterView.SuspendDrawing();
            }
            else {
                trackEditor.ResumeDrawing();
                trackEditor.ResumeLayout();
                dgvMasterView.ResumeDrawing();
                dgvMasterView.ResumeLayout();
            }
        }

        ///Update DGV from _tracks
        public void LoadLeaf(dynamic _load, bool template = false)
        {
            //skip certain checks if we're loading a non-leaf sequencer
            if (this.WorkingFile.Extension.Equals(".leaf", StringComparison.OrdinalIgnoreCase)) {
                if (_load == null)
                    return;
                //reset flag in case it got stuck previously
                EditorIsLoading = false;
                //detect if file is actually Leaf or not
                if ((string)_load["obj_type"] != "SequinLeaf") {
                    MessageBox.Show($"{this.WorkingFile.Name} does not appear to be a leaf file.\n'obj_type' was not SequinLeaf.", "Thumper Custom Level Editor");
                    return;
                }
                //check if it has a name
                //important for some leaf objects
                if (_load["obj_name"] == null) {
                    MessageBox.Show("Leaf missing obj_name parameter. Please set it in the txt file and then reload.", "Thumper Custom Level Editor");
                    return;
                }
            }
            //set flag that load is in progress. This skips Save method
            EditorIsLoading = true;
            SuspendDataGrids(true);
            if (this.WorkingFile.Extension.Equals(".leaf", StringComparison.OrdinalIgnoreCase)) {
                LeafProperties = new(this) {
                    SequencerType = this.WorkingFile.Extension,
                    TimeSignature = (string)_load["time_sig"] ?? "4/4",
                    LeafLength = (int?)_load["beat_cnt"] ?? 1,
                };
            }
            else if (this.WorkingFile.Extension.Equals(".lvl", StringComparison.OrdinalIgnoreCase)) {
                LeafProperties = new(this) {
                    SequencerType = this.WorkingFile.Extension,
                    TimeSignature = "4/4",
                    LeafLength = ((LvlProperties)AltSequencer).Leafs.Select(x => x.Beats).Sum() + ((LvlProperties)AltSequencer).ApproachBeats + (((LvlProperties)AltSequencer).Leafs.Count(x => x.Beats == -1) * 2)
                };
            }
            //check for template or regular file
            if (template)
                this.WorkingFile = null;
            //Add [sequencer] tag on tab text if its a lvl sequencer
            this.Text = $"{this.WorkingFile.Name}{(this.WorkingFile.Extension.Equals(".lvl", StringComparison.OrdinalIgnoreCase) ? " [Sequencer]" : "")}";

            trackEditor.Rows.Clear();
            LeafLengthChanged();
        }

        public DataGridView SimpleTrackEditor = new();
        public void LoadLeafSimple(dynamic _load)
        {
            //set flag that load is in progress. This skips Save method
            EditorIsLoading = true;
            if (this.WorkingFile?.Extension == ".leaf") {
                LeafProperties = new(this) {
                    SequencerType = ".leaf",
                    TimeSignature = (string)_load["time_sig"] ?? "4/4",
                    LeafLength = (int?)_load["beat_cnt"] ?? 1
                };
            }
            else if (this.WorkingFile?.Extension is ".lvl" or null) {
                LeafProperties = new(this) {
                    SequencerType = ".lvl",
                    TimeSignature = "4/4",
                    LeafLength = ((LvlProperties)AltSequencer).Leafs.Select(x => x.Beats).Sum() + ((LvlProperties)AltSequencer).ApproachBeats + (((LvlProperties)AltSequencer).Leafs.Count(x => x.Beats == -1) * 2)
                };
            }

            while (_leafproperties.BeatsAndFrozen > SimpleTrackEditor.ColumnCount)
                SimpleTrackEditor.Columns.Add(new SequencerColumn() { FillWeight = 0.001f });
        }

        public void LoadEnd(dynamic savestate)
        {
            //finish up setting up the editor. Enable some buttons, set zoom level, etc.
            LeafLanes = SequencerObjects.Where(x => x.ObjName.EndsWith(".leaf")).ToDictionary(x => x.FriendlyParam);
            trackZoom_Scroll(null, null);
            trackEditor.RowHeadersVisible = true;
            TCLE.ResizeHeaders(trackEditor);
            foreach (Sequencer_Object seq in SequencerObjects) {
                //update visual row properties
                seq.ExpandLanesInEditor = Properties.Settings.Default.LeafOptionShowLane;
                SetRowHeaderText(seq);
            }
            TrackTimeSigHighlighting();
            //
            SuspendDataGrids(false);
            trackEditor.Invalidate();
            //initialize undo base state to first load
            UndoList.Add(new SaveState() {
                Reason = "",
                State = savestate
            });
            //mark leaf is saved (just freshly loaded)
            EditorIsLoading = false;
            //SaveCheckAndWrite(true, "", false);
            //
            LeafMasterView.InitializeAndResize(SequencerObjects, _leafproperties);
            toolstripMasterView.Width = leafToolStrip.Width + trackEditor.RowHeadersWidth + (75);
        }

        public static void LoadSequencer(dynamic seqJSON, LeafProperties ParentLeaf, DataGridView dgv)
        {
            Dictionary<(string, string), Sequencer_Object> LoadedObjects = new();
            int audiochannels = 0;

            //each object in the seq_objs[] list
            foreach (dynamic seq_obj in seqJSON) {
                Sequencer_Object ObjectToImport = new(ParentLeaf, null) {
                    ParentLeaf = ParentLeaf,
                    ObjName = ((string)seq_obj["obj_name"]),
                    //TraitType = Sequencer_Object.TraitLookup[(string)seq_obj["trait_type"]],
                    Step = (string)seq_obj["step"] == "True",
                    DefaultValue = seq_obj["default"],
                    //Footer = seq_obj["footer"].GetType() == typeof(JArray) ? String.Join(",", ((JArray)seq_obj["footer"]).ToList()) : ((string)seq_obj["footer"]).Replace("[", "").Replace("]", ""),
                    //if the leaf has definitions for these, add them. If not, set to defaults
                    ParamPath = seq_obj.ContainsKey("param_path_hash") ? $"0x{(string)seq_obj["param_path_hash"]}" : ((string)seq_obj["param_path"]),
                    highlight_value = (int?)seq_obj["editor_data"]?[1] ?? 0,
                    EnabledInEditor = ((string)seq_obj["enabled"] ?? "True").Equals("true", StringComparison.OrdinalIgnoreCase),
                    IsDefault = false
                };
                ObjectToImport.CreateCells(dgv);

                //if object is a layer volume, we "reset" its index to x so it can be renumbered in case its out of order.
                if (ObjectToImport.ParamPath.StartsWith("layer_volume"))
                    ObjectToImport.ParamPath = "layer_volume,x";
                //if the object is a tuning layer, handle it here
                if (ObjectToImport.ObjName == "_TuningLayerX") {
                    ObjectToImport.FriendlyParam = ObjectToImport.ParamPath;
                    ObjectToImport.Default = TCLE.LeafObjects["_TuningLayerX;⮝ Tuning Layer X"];
                    //ObjectToImport.Category = "";
                }
                //if object is a .samp, set category and friendly_param since they don't exist in LeafObjects
                else if (ObjectToImport.ObjName.EndsWith(".samp") && ObjectToImport.ParamPath == "play") {
                    //ObjectToImport.Category = "PLAY SAMPLE";
                    ObjectToImport.FriendlyParam = "play";
                    ObjectToImport.Default = TCLE.LeafObjects["sample.samp;play"];
                }
                //otherwise, search LeafObjects for the friendly names for display purposes
                else {
                    try {
                        string normalizeParam = $"{(ObjectToImport.ObjName.EndsWith(".leaf", StringComparison.OrdinalIgnoreCase) || ObjectToImport.ObjName.EndsWith(".lvl", StringComparison.OrdinalIgnoreCase) ? "leafname" : ObjectToImport.ObjName)};{ObjectToImport.ParamPath.Replace(ObjectToImport.ParamPathLane, "ent")}";
                        if (TCLE.LeafObjects.TryGetValue(normalizeParam, out DefaultSequencerObject objmatch)) {
                            ObjectToImport.Default = objmatch;
                            ObjectToImport.FriendlyParam = objmatch.ParamDisplayName;
                            ObjectToImport.HighlightColor = seq_obj["editor_data"]?[0] is int _color ? Color.FromArgb(_color) : objmatch.DefaultColor;
                            //ObjectToImport.HighlightColor = objmatch.defaultcolor;
                        }
                        //set audio channel numbers on load
                        if (ObjectToImport.Default.Category == "LOOP TRACK VOLUME") {
                            ObjectToImport.ParamPath = ObjectToImport.ParamPath.Replace("x", $"{audiochannels}");
                            ObjectToImport.FriendlyParam = ObjectToImport.FriendlyParam.Replace("x", $"{audiochannels}");
                            audiochannels++;
                        }
                    } catch (Exception ex) {
                        Debug.WriteLine($"{ParentLeaf.LeafName} VERY BIG PROBLEM HERE {ex}");
                    }
                }
                //deal with multilanes
                //if object is multilane, we will add all 5 lanes at once, as defaults
                //then lookup the object and assign the initialized Sequencer Object created above in place of the default one
                if (ObjectToImport.FriendlyLane is not "none") {
                    LoadMultiLanes(ObjectToImport, LoadedObjects);
                }
                else {
                    ObjectToImport.ExpandLanesInEditor = true;
                    LoadedObjects.TryAdd((ObjectToImport.ObjName, ObjectToImport.ParamPath), ObjectToImport);
                }
                //this line exists to force the app to recognize the rows have proper indexes instead of -1
                //string _e = string.Join(',', dgv.Rows.Cast<DataGridViewRow>().Select(x => x.Index));
                //import data points to the row cells.
                LoadDataPoints(ObjectToImport, seq_obj["data_points"]);
            }
            //return Seq_Objs;
            ParentLeaf.SequencerObjects = LoadedObjects.Values.ToList();
            dgv.Rows.AddRange(ParentLeaf.SequencerObjects.ToArray());
        }

        public static void LoadDataPoints(Sequencer_Object ObjectToImport, JToken dataPoints)
        {
            foreach (JToken dp in dataPoints) {
                if (dp is JObject dataPoint) {
                    int beat = (int)dataPoint["beat"];
                    if (beat >= ObjectToImport.ParentLeaf.LeafLength)
                        continue;

                    SeqDataPoint data = ObjectToImport[beat + FrozenColumnOffset];
                    data.Interpolation = ((string)dataPoint["interp"])?.Replace("kTraitInterp", "") ?? "Linear";
                    data.Ease = TCLE.Easings[(string)dataPoint["ease"] ?? "kEaseInOut"];
                    data.Value = (decimal)dataPoint["value"];
                }
                else {
                    JProperty property = (JProperty)dp;
                    int beat = int.Parse(property.Name);
                    if (beat >= ObjectToImport.ParentLeaf.LeafLength)
                        continue;

                    SeqDataPoint data = ObjectToImport[beat + FrozenColumnOffset];
                    data.Value = UtilMath.TruncateDecimal((decimal)property.Value, 3);
                }
            }
        }

        public static void LoadMultiLanes(Sequencer_Object ObjectToImport, Dictionary<(string, string), Sequencer_Object> LoadedObjects)
        {
            if (LoadedObjects.TryGetValue((ObjectToImport.ObjName, ObjectToImport.ParamPath), out _)) {
                LoadedObjects[(ObjectToImport.ObjName, ObjectToImport.ParamPath)] = ObjectToImport;
                return;
            }
            //if null, no object exists in SequencerObjects yet for this object or its lanes. We'll have to make it.
            LoadedObjects.TryAdd((ObjectToImport.ObjName, $"{ObjectToImport.ParamPathBase}.a01"), ObjectToImport.CloneAsLane(".a01", Properties.Settings.Default.LeafOptionShowLane));
            //dgv.Rows.Add(LoadedObjects[^1]); if (!ObjectToImport.ParentLeaf.ParentEditor.SimpleLoad) LoadedObjects[^1].ExpandLanesInEditor = Properties.Settings.Default.LeafOptionShowLane;
            LoadedObjects.TryAdd((ObjectToImport.ObjName, $"{ObjectToImport.ParamPathBase}.a02"), ObjectToImport.CloneAsLane(".a02", Properties.Settings.Default.LeafOptionShowLane));
            LoadedObjects.TryAdd((ObjectToImport.ObjName, $"{ObjectToImport.ParamPathBase}.ent"), ObjectToImport.CloneAsLane(".ent", Properties.Settings.Default.LeafOptionShowLane));
            LoadedObjects.TryAdd((ObjectToImport.ObjName, $"{ObjectToImport.ParamPathBase}.z01"), ObjectToImport.CloneAsLane(".z01", Properties.Settings.Default.LeafOptionShowLane));
            LoadedObjects.TryAdd((ObjectToImport.ObjName, $"{ObjectToImport.ParamPathBase}.z02"), ObjectToImport.CloneAsLane(".z02", Properties.Settings.Default.LeafOptionShowLane));

            LoadedObjects[(ObjectToImport.ObjName, ObjectToImport.ParamPath)] = ObjectToImport;
            return;
        }
        public static List<Sequencer_Object> LoadMultiLanes(Sequencer_Object ObjectToImport, List<Sequencer_Object> LoadedObjects)
        {
            List<Sequencer_Object> Lanes = new(); Sequencer_Object lookup = LoadedObjects.FirstOrDefault(x => x.ParamPath == ObjectToImport.ParamPath && x.ParamPathLane == ObjectToImport.ParamPathLane && x.IsDefault == true);
            //if null, no object exists in SequencerObjects yet for this object or its lanes. We'll have to make it.
            if (lookup == null) { 
                Lanes.Add(ObjectToImport.CloneAsLane(".a01", Properties.Settings.Default.LeafOptionShowLane));
                //dgv.Rows.Add(LoadedObjects[^1]); if (!ObjectToImport.ParentLeaf.ParentEditor.SimpleLoad) LoadedObjects[^1].ExpandLanesInEditor = Properties.Settings.Default.LeafOptionShowLane;
                Lanes.Add(ObjectToImport.CloneAsLane(".a02", Properties.Settings.Default.LeafOptionShowLane));
                Lanes.Add(ObjectToImport.CloneAsLane(".ent", Properties.Settings.Default.LeafOptionShowLane));
                Lanes.Add(ObjectToImport.CloneAsLane(".z01", Properties.Settings.Default.LeafOptionShowLane));
                Lanes.Add(ObjectToImport.CloneAsLane(".z02", Properties.Settings.Default.LeafOptionShowLane));
                
                Lanes[(ObjectToImport.LaneOffsetFromTop * -1)] = ObjectToImport;
                //lookup = Lanes[(ObjectToImport.LaneOffsetFromTop * -1)];// .FirstOrDefault(x => x.ObjName == ObjectToImport.ObjName && x.ParamPath == ObjectToImport.ParamPath && x.ParamPathLane == ObjectToImport.ParamPathLane && x.IsDefault == true);
                //Lanes[Lanes.IndexOf(lookup)] = ObjectToImport;
                return Lanes;
            } else { 
                LoadedObjects[LoadedObjects.IndexOf(lookup)] = ObjectToImport;
                return null;
            }
        }

        private void toolstripObjTune_Click(object sender, EventArgs e)
        {
            AddSequencerLayer(trackEditor.CurrentRow.Index);
        }
        public void AddSequencerLayer(int index)
        {
            Sequencer_Object seq = new(LeafProperties, TCLE.LeafObjects["_TuningLayerX;⮝ Tuning Layer X"]) {
                ParentLeaf = LeafProperties,
                ObjName = "_TuningLayerX",
                ParamPath = "⮝ Tuning Layer X",
                FriendlyParam = "⮝ Tuning Layer X",
                DefaultValue = 0,
                Step = false,
                HighlightColor = Color.FromArgb(40, 40, 40),
                highlight_value = 0,
                EnabledInEditor = true
            };

            int tuninglayers = SequencerObjects.Count(x => x.ObjName == "_TuningLayerX");
            seq.ParamPath = seq.ParamPath.Replace("X", $"{tuninglayers}");
            seq.FriendlyParam = seq.FriendlyParam.Replace("X", $"{tuninglayers}");

            seq.ExpandLanesInEditor = seq.FriendlyLane == "none" || Properties.Settings.Default.LeafOptionShowLane;
            SequencerObjects.Insert(index + 1, seq);
            trackEditor.Rows.Insert(index + 1, seq);
            SetRowHeaderText(seq);
            SaveCheckAndWrite(false, "Add Object");
            UtilAudio.PlaySound("UIobjectadd");
        }

        private void toolstripObjConvert_Click(object sender, EventArgs e)
        {
            Sequencer_Object seq = SequencerObjects[trackEditor.CurrentRow.Index].Clone();
            seq.Default = TCLE.LeafObjects["_TuningLayerX;⮝ Tuning Layer X"];
            seq.ParentLeaf = _leafproperties;
            seq.ObjName = "_TuningLayerX";
            seq.ParamPath = "⮝ Tuning Layer X";
            seq.FriendlyParam = "⮝ Tuning Layer X";
            seq.DefaultValue = 0;
            seq.Step = false;
            seq.HighlightColor = Color.FromArgb(40, 40, 40);
            seq.highlight_value = 0;
            seq.EnabledInEditor = true;

            int tuninglayers = SequencerObjects.Count(x => x.ObjName == "_TuningLayerX");
            seq.ParamPath = seq.ParamPath.Replace("X", $"{tuninglayers}");
            seq.FriendlyParam = seq.FriendlyParam.Replace("X", $"{tuninglayers}");

            SequencerObjects.Insert(trackEditor.CurrentRow.Index + 1, seq);
            trackEditor.Rows.Insert(trackEditor.CurrentRow.Index + 1, seq);

            SetRowHeaderText(seq);
            UtilAudio.PlaySound("UIinterpolatewindow");

            CalculateTuningLayers(LeafProperties, seq);
            SaveCheckAndWrite(false, "Converted object to tuning layer");
        }

        public override void PerformUndo(int undolistindex)
        {
            if (undolistindex > UndoList.Count - 1)
                return;
            bool _trackNotSaved = this.Saved;
            //track which objects are expanded
            List<Sequencer_Object> _expanded = SequencerObjects.Where(x => x.ExpandLanesInEditor == true).ToList();
            List<Tuple<int, int>> _selection = trackEditor.SelectedCells.Cast<DataGridViewCell>().Select(x => new Tuple<int, int>(x.ColumnIndex, x.RowIndex)).ToList();
            //
            LoadLeaf(UndoList[undolistindex].State);
            LoadSequencer(UndoList[undolistindex].State["seq_objs"], LeafProperties, trackEditor);
            LoadEnd(UndoList[undolistindex].State);
            UndoList.RemoveRange(0, undolistindex);
            propertyGridLeaf.Refresh();
            //restore expanded lanes
            foreach (Sequencer_Object seq in SequencerObjects) {
                if (_expanded.Any(x => x.ObjName == seq.ObjName && x.FriendlyLane == seq.FriendlyLane && x.FriendlyParam == seq.FriendlyParam))
                    seq.ExpandLanesInEditor = true;
            }
            //restore selection
            foreach (Tuple<int, int> _cell in _selection) {
                trackEditor[_cell.Item1, _cell.Item2].Selected = true;
            }

            if (!_trackNotSaved) {
                this.Saved = false;
                if (!this.Text.EndsWith('*'))
                    this.Text += '*';
            }

            LeafMasterView.InitializeAndResize(SequencerObjects, LeafProperties);
        }

        ///SAVE
        public override void Save(bool playsound = true)
        {
            //if _loadedlvl is somehow not set, force Save As instead
            if (this.WorkingFile == null) {
                SaveAs();
            }
            else
                SaveCheckAndWrite(true, "", playsound);
        }
        ///SAVE AS
        public override FileInfo SaveAs(bool FileIsNew = false, string InitialDir = null)
        {
            using SaveFileDialog sfd = new();
            //filter .txt only
            sfd.Filter = "Thumper Editor Leaf File (*.leaf)|*.leaf";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = InitialDir ?? TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                this.WorkingFile = new FileInfo(sfd.FileName);
                EditorIsLoading = true;
                if (_leafproperties == null) {
                    LeafProperties = new(this) {
                        TimeSignature = "4/4",
                        LeafLength = FileIsNew ? 32 : LeafProperties.LeafLength
                    };
                } //else
                  //leafProperties.FilePath = loadedleaf;
                EditorIsLoading = false;
                SaveCheckAndWrite(true, "", true);
                this.ClearFileLock();
                //after saving new file, refresh the project explorer
                ProjectExplorer.CreateTreeView();
            }
            return this.WorkingFile;
        }

        public string GetEditorTitle() => $"{this.WorkingFile.Name}{(this.WorkingFile.Extension.Equals(".lvl", StringComparison.OrdinalIgnoreCase) ? " [Sequencer]" : "")}";
        public override void SaveCheckAndWrite(bool IsSaved, string Reason, bool playsound = false)
        {
            if (EditorIsLoading || Playback.Generating || TCLE.IsLoadingProject )
                return;
            //make the beeble emote
            TCLE.MainBeeble.MakeFace();

            JObject _saveJSON;
            //catch any issues with serializing the file
            try {
                _saveJSON = LeafProperties.ConvertToJson();
            } catch (Exception ex) {
                MessageBox.Show($"Problem saving data to file. Show this error to the dev.\n\n{ex}");
                return;
            }
            this.Saved = IsSaved;
            //
            if (!IsSaved) {
                //denote editor tab is not saved
                this.Text = GetEditorTitle() + "*";
                //update the undo list
                if (LogUndo) {
                    UndoList.Insert(0, new SaveState() {
                        Reason = Reason,
                        State = _saveJSON
                    });
                }
                LeafMasterView.DrawTrack(SequencerObjects, _leafproperties);
            }
            else {
                this.Text = GetEditorTitle();
                //leafProperties.revertPoint = _saveJSON;
                //If leaf, build the JSON to write to file
                if (this.WorkingFile.Extension.Equals(".leaf", StringComparison.OrdinalIgnoreCase)) {
                    //write JSON to file
                    UtilFile.WriteFileLock(this.FileLock, _saveJSON);
                    //need to update leaf beat count in every lvl that references this file
                    if (_leafproperties.BeatsChangedSinceSave) {
                        foreach (FileInfo lvl in ProjectExplorer.GetFilesByExtension(".lvl")) {
                            dynamic _loadfile = UtilFile.LoadFileLock(lvl);
                            //if load fails, skip
                            if (_loadfile == null)
                                continue;
                            bool changes = false;
                            //some files may be lock loaded, so we use different writing methods for those
                            //also force editor to reload the document
                            foreach (dynamic leafseq in _loadfile["leaf_seq"]) {
                                if (leafseq["leaf_name"] == this.WorkingFile.Name) {
                                    leafseq["beat_cnt"] = _leafproperties.LeafLength;
                                    changes = true;
                                }
                            }
                            if (changes)
                                UtilFile.WriteFileLock(new FileStream(lvl.FullName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite), _loadfile);
                        }
                        TCLE.FindEditorRunMethod(typeof(EditorLvl), "RecalculateRuntime");
                    }
                    if (playsound) UtilAudio.PlaySound("UIsave");
                }
                //else if a different sequencer, pass data back and force save
                else if (this.WorkingFile.Extension.Equals(".lvl", StringComparison.OrdinalIgnoreCase)) {
                    ((LvlProperties)AltSequencer).SequencerObjects = _leafproperties.SequencerObjects;
                }

                if (!EditorIsLoading && !SimpleLoad) {
                    TCLE.SaveTCL();
                }

                //find if any raw text docs are open of this leaf and update them
                TCLE.FindReloadRaw(this.WorkingFile.Name);
                _leafproperties.BeatsChangedSinceSave = false;
            }
        }
        ///LEAF LENGTH
        public static DataGridViewCellStyle DGVCS = new() {
            Format = "0.###",
            Font = EditorLeaf.TuningFont
        };
        public void LeafLengthChanged()
        {
            if (_leafproperties == null)
                return;
            int data = trackEditor.ColumnCount - FrozenColumnOffset;

            if (_leafproperties.LeafLength + FrozenColumnOffset > trackEditor.ColumnCount) {
                if (!SimpleLoad) {
                    int _width = Properties.Settings.Default.ZoomHoriz;
                    while (_leafproperties.LeafLength + FrozenColumnOffset > trackEditor.ColumnCount)
                        trackEditor.Columns.Add(new SequencerColumn() {
                            FillWeight = 0.0001f,
                            CellTemplate = new SeqDataPoint(),
                            Name = (trackEditor.ColumnCount - FrozenColumnOffset).ToString(),
                            HeaderText = (trackEditor.ColumnCount - FrozenColumnOffset).ToString(),
                            Resizable = DataGridViewTriState.False,
                            SortMode = DataGridViewColumnSortMode.NotSortable,
                            DividerWidth = 0,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                            Frozen = false,
                            MinimumWidth = 2,
                            ReadOnly = false,
                            ValueType = typeof(decimal?),
                            DefaultCellStyle = DGVCS,
                            Width = _width
                        });
                }
                else {
                    while (_leafproperties.LeafLength + FrozenColumnOffset > trackEditor.ColumnCount)
                        trackEditor.Columns.Add(new SequencerColumn() {
                            FillWeight = 0.0001f,
                            CellTemplate = new SeqDataPoint(),
                        });
                    return;
                }
                //TCLE.GenerateColumnStyle(Columns, FrozenColumnOffset);
            }
            else {
                trackEditor.ColumnCount = _leafproperties.LeafLength + FrozenColumnOffset;
            }

            dgvMasterView.ColumnCount = trackEditor.ColumnCount - FrozenColumnOffset;
            LeafMasterView.InitializeAndResize(SequencerObjects, _leafproperties);
            //set cell zoom
            trackZoom_Scroll(null, null);
            //make sure new cells follow the time sig
            TrackTimeSigHighlighting();
            //sets flag that leaf has unsaved changes
            SaveCheckAndWrite(false, $"Leaf length changed {data} -> {_leafproperties.LeafLength}");
        }

        ///Import raw text from rich text box to selected row
        public static void TrackRawImport(Sequencer_Object seq, List<SeqDataPoint> data_points, LeafProperties _properties)
        {
            List<SeqDataPoint> DataNotNull = data_points.Where(x => x.Value is not null && x.beat < _properties.LeafLength).ToList();
            //iterate over each data point, and fill cells
            foreach (SeqDataPoint data_point in DataNotNull) {
                try {
                    seq.Cells[data_point.beat + FrozenColumnOffset].Value = UtilMath.TruncateDecimal(Decimal.Parse(data_point.Value.ToString()), 3);
                    //seq[data_point.beat].Value = UtilMath.TruncateDecimal(Decimal.Parse(data_point.value.ToString()), 3);
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
                    seq[int.Parse(data_point.Name)].Value = UtilMath.TruncateDecimal((decimal)data_point.Value, 3);
                    //seq[int.Parse(data_point.Name)].Value = UtilMath.TruncateDecimal((decimal)data_point.Value, 3);
                } catch (ArgumentOutOfRangeException) {
                    break;
                }
            }
        }

        public void RefreshHeaders()
        {
            SuspendDataGrids(true);
            foreach (Sequencer_Object seq in SequencerObjects) {
                SetRowHeaderText(seq);
            }
            TCLE.ResizeHeaders(trackEditor);
            SuspendDataGrids(false);
        }

        ///Updates row headers to be the Object and Param_Path
        public static void SetRowHeaderText(Sequencer_Object seq)
        {
            string ShowCategory = Properties.Settings.Default.LeafOptionShowCategory ? $"[{seq.Default.Category}] " : "";
            ShowCategory = Properties.Settings.Default.LeafOptionCategoryIcon ? $" {ShowCategory}" : ShowCategory; 
            string ShowLane = (seq.ExpandLanesInEditor && seq.FriendlyLane != "none") ? $"{seq.FriendlyParam}, {seq.FriendlyLane}" : seq.FriendlyParam;
            if (seq.Default.Category == "PLAY SAMPLE")
                //show the sample name instead
                seq.HeaderCell.Value = $"{ShowCategory}{seq.ObjName}";
            else if (seq.ObjName == "_TuningLayerX")
                seq.HeaderCell.Value = $"  {seq.ParamPath}";
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
            if (_leafproperties == null || SimpleLoad)
                return;
            //grab the first part of the time sig. This represents how many beats are in a bar
            //tryparse to see if it fails.
            foreach (Sequencer_Object Seq in SequencerObjects)
                TimeSigHighlightSingleObject(Seq, LeafProperties.TimeTopBeat);

            if (AltSequencer != null)
                TrackLeafDividerHighlighting((LvlProperties)AltSequencer);

            trackEditor.Invalidate();
        }

        public static void TimeSigHighlightSingleObject(Sequencer_Object Seq, int timesigbeats)
        {
            int doubletime = timesigbeats * 2;
            int cellcount = Seq.Cells.Count;
            for (int x = 0; x < cellcount - FrozenColumnOffset; x++) {
                Seq.Cells[x + FrozenColumnOffset].Style.BackColor = x % doubletime < timesigbeats ? Properties.Settings.Default.ColorLeafTimeSig1 : Properties.Settings.Default.ColorLeafTimeSig2;
            }
        }

        public void TrackLeafDividerHighlighting(LvlProperties Lvl)
        {
            int index = FrozenColumnOffset;

            trackEditor.Columns[index].DefaultCellStyle.BackColor = Color.LightGray;
            trackEditor.Columns[index].HeaderCell.Style.BackColor = Color.LightGray;
            trackEditor.Columns[index].HeaderCell.Style.ForeColor = Color.Black;
            trackEditor.Columns[index].HeaderText = "Approach";
            index += Lvl.ApproachBeats;

            foreach (LvlLeafData leaf in Lvl.Leafs) {
                trackEditor.Columns[index].DefaultCellStyle.BackColor = Color.LightGray;
                trackEditor.Columns[index].HeaderCell.Style.BackColor = Color.LightGray;
                trackEditor.Columns[index].HeaderCell.Style.ForeColor = Color.Black;
                trackEditor.Columns[index].HeaderText = leaf.Leaf;
                //trackEditor.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                index += leaf.Beats != -1 ? leaf.Beats : 1;
            }
        }

        private static void RowReadOnly(Sequencer_Object seq, bool setreadonly)
        {
            if (Playback.Generating)
                return;
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
            if (Playback.Generating || this.SimpleLoad)
                return;
            btnTrackDelete.Enabled = SequencerObjects.Count > 0;
            btnTrackUp.Enabled = SequencerObjects.Count > 1;
            btnTrackDown.Enabled = SequencerObjects.Count > 1;
            btnTrackClear.Enabled = SequencerObjects.Count > 0;
            btnTrackCopy.Enabled = SequencerObjects.Count > 0;
            btnTrackPaste.Enabled = TCLE.ClipboardSequencer.Count > 0;
        }

        #region Cut Copy Paste
        public override void Copy()
        {
            if (textEditor.Focused)
                return;
            ///copies selected cells
            IEnumerable<SeqDataPoint> _selected = trackEditor.SelectedCells.Cast<SeqDataPoint>().Where(x => x.ColumnIndex >= FrozenColumnOffset);
            List<SeqDataPoint> lanecells = new();
            foreach (SeqDataPoint dgvc in _selected) {
                if (SequencerObjects[dgvc.RowIndex].FriendlyLane == "lane center" && SequencerObjects[dgvc.RowIndex].ExpandLanesInEditor == false) {
                    lanecells.Add((SeqDataPoint)trackEditor[dgvc.ColumnIndex, dgvc.RowIndex - 2]);
                    lanecells.Add((SeqDataPoint)trackEditor[dgvc.ColumnIndex, dgvc.RowIndex - 1]);
                    lanecells.Add((SeqDataPoint)trackEditor[dgvc.ColumnIndex, dgvc.RowIndex + 1]);
                    lanecells.Add((SeqDataPoint)trackEditor[dgvc.ColumnIndex, dgvc.RowIndex + 2]);
                }
            }
            _selected = lanecells.Concat(_selected).OrderBy(x => x.RowIndex).ThenBy(x => x.ColumnIndex);
            TCLE.ClipboardDataPoints = _selected.Select(x => x.Clone()).ToList();
            UtilAudio.PlaySound("UIkcopy");
        }

        public override void Cut()
        {
            if (textEditor.Focused)
                return;
            Copy();
            LogUndo = false;
            ApplyCellValueChanges(trackEditor[trackEditor.CurrentCell.ColumnIndex, trackEditor.CurrentCell.RowIndex], true);
            LogUndo = true;
            SaveCheckAndWrite(false, "Cut cells");
        }

        public override void Paste()
        {
            if (textEditor.Focused)
                return;

            EditorIsPasting = true;
            LogUndo = false;
            int pastingrow = trackEditor.CurrentCell.RowIndex;
            if (SequencerObjects[pastingrow].FriendlyLane == "lane center" && SequencerObjects[pastingrow].ExpandLanesInEditor == false && TCLE.ClipboardDataPoints.Count >= 5)
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
                if (pastingcol + (sdp.OriginalColumn - coloffset) >= LeafProperties.LeafLength + FrozenColumnOffset)
                    continue;
                SeqDataPoint target = SequencerObjects[pastingrow + (sdp.OriginalRow - rowoffset)][pastingcol + (sdp.OriginalColumn - coloffset)];
                target.Value = sdp.Value;
                target.Ease = sdp.Ease;
                target.Interpolation = sdp.Interpolation;
                if (!pastedrows.Contains(SequencerObjects[pastingrow + (sdp.OriginalRow - rowoffset)]))
                    pastedrows.Add(SequencerObjects[pastingrow + (sdp.OriginalRow - rowoffset)]);
            }

            foreach (Sequencer_Object _seq in pastedrows) {
                CalculateTuningLayers(_leafproperties, _seq);
            }

            EditorIsPasting = false;
            LogUndo = true;
            SaveCheckAndWrite(false, "Pasted cells");
            trackEditor.Invalidate();
        }
        #endregion

        public static void RandomizeRowValues(Sequencer_Object seq)
        {
            int rngchance;
            int rnglimit;
            int randomtype = 0;
            decimal? valueiftrue = 0;

            if ((seq.Default.TraitType is DefaultSequencerObject.Trait.Bool or DefaultSequencerObject.Trait.Action) || (seq.ParamPath is "visibla01" or "visibla02" or "visible" or "visiblz01" or "visiblz02")) {
                valueiftrue = 1;
                rngchance = 10;
                rnglimit = 9;
                if (seq.ObjName == "sentry.spn") {
                    rngchance = 55;
                    rnglimit = 54;
                }
            }
            else if (seq.Default.TraitType is DefaultSequencerObject.Trait.Color) {
                randomtype = 7;
                rngchance = 10;
                rnglimit = 8;
            }
            else {
                rngchance = 10;
                rnglimit = 9;
                if (seq.ParamPath == "sequin_speed")
                    randomtype = 2;
                else if (seq.ObjName == "fade.pp")
                    randomtype = 3;
                else if (seq.Default.Category == "CAMERA")
                    randomtype = 4;
                else if (seq.Default.Category == "GAMMA")
                    randomtype = 5;
                else
                    randomtype = 6;
            }
            foreach (SeqDataPoint dgvc in seq.Cells) {
                if (dgvc.ColumnIndex < FrozenColumnOffset)
                    continue;
                switch (randomtype) {
                    case 2:
                        valueiftrue = UtilMath.TruncateDecimal((decimal)(TCLE.rng.NextDouble() * 100) + 0.01m, 3) % 4;
                        break;
                    case 3:
                        valueiftrue = UtilMath.TruncateDecimal((decimal)TCLE.rng.NextDouble(), 3);
                        break;
                    case 4:
                        valueiftrue = UtilMath.TruncateDecimal((decimal)(TCLE.rng.NextDouble() * 100), 3) * (TCLE.rng.Next(2) == 0 ? 1 : -1);
                        break;
                    case 5:
                        valueiftrue = UtilMath.TruncateDecimal((decimal)(TCLE.rng.NextDouble() * 100), 3);
                        break;
                    case 6:
                        valueiftrue = UtilMath.TruncateDecimal((decimal)(TCLE.rng.NextDouble() * 1000), 3) % 200 * (TCLE.rng.Next(2) == 0 ? 1 : -1);
                        break;
                    case 7:
                        valueiftrue = Color.FromArgb(TCLE.rng.Next(256), TCLE.rng.Next(256), TCLE.rng.Next(256)).ToArgb();
                        break;
                    default:
                        break;
                }
                dgvc.Value = TCLE.rng.Next(0, rngchance) >= rnglimit ? valueiftrue : null;
                dgvc.Ease = "Ease In Out";
                dgvc.Interpolation = "Linear";
            }
        }

        public static void CalculateTuningLayers(LeafProperties _properties, Sequencer_Object seq)
        {
            if (_properties.ParentEditor.EditorIsProcessing)
                return;

            if (_properties.ParentEditor.EditorIsLoading || (seq.Index == 0 && seq.ObjName == "_TuningLayerX"))
                return;
            int count = 1;
            List<Sequencer_Object> TuningLayers = new();
            while (seq.Index - count >= 0 && _properties.SequencerObjects[seq.Index - count].ObjName == "_TuningLayerX") {
                count++;
            }
            //return if the tuning layer is at the top
            if (seq.Index - count == 0 && _properties.SequencerObjects[seq.Index - count].ObjName == "_TuningLayerX")
                return;
            Sequencer_Object TargetTuningLayer = _properties.SequencerObjects[seq.Index - count];
            count = 1;
            while (TargetTuningLayer.Index + count < _properties.SequencerObjects.Count && _properties.SequencerObjects[TargetTuningLayer.Index + count].ObjName == "_TuningLayerX") {
                TuningLayers.Add(_properties.SequencerObjects[TargetTuningLayer.Index + count]);
                count++;
            }

            try {
                _properties.ParentEditor.LogUndo = false;
                _properties.ParentEditor.EditorIsTuning = true;
                Sequencer_Object SumOfLayers = new(_properties, null);
                ((EditorLeaf)_properties.ParentEditor).trackEditor.Rows.Add(SumOfLayers);
                int _ = ((EditorLeaf)_properties.ParentEditor).trackEditor.Rows[^1].Index;

                foreach (Sequencer_Object _layer in TuningLayers) {
                    Sequencer_Object InterpolationCalc = new(_properties, null);
                    ((EditorLeaf)_properties.ParentEditor).trackEditor.Rows.Add(InterpolationCalc);
                    _ = ((EditorLeaf)_properties.ParentEditor).trackEditor.Rows[^1].Index;
                    SeqDataPoint[] _datapoints = _layer.Cells.Cast<SeqDataPoint>().Where(x => x.Value != null).ToArray();

                    for (int n = 0; n < _datapoints.Length - 1; n++) {
                        //sort cells so they are in order according to column index
                        SeqDataPoint start = _datapoints[n];
                        SeqDataPoint end = _datapoints[n + 1];
                        if (start.beat > end.beat)
                            (start, end) = (end, start);
                        //get start and end values, and how many beats separate them
                        double _start = (double)(decimal)start.Value;
                        double _end = (double)(decimal)end.Value;
                        double max = Math.Max(_start, _end);
                        double min = Math.Min(_start, _end);
                        int _beats = end.beat - start.beat + 1;
                        //initialize array = to beats, fill with linear values between 0 and 1
                        //these will be transformed by the formulas below
                        double[] interp = new double[_beats];
                        for (int x = 0; x < interp.Length; x++) {
                            interp[x] = (double)(x) / (double)(interp.Length - 1);
                        }
                        //change interpolation formula based on settings on the datapoint
                        UtilMath.CalculateTuning(interp, $"{start.Interpolation} {start.Ease}");

                        for (int x = 0; x < interp.Length; x++) {
                            //if the first cell is actually the maximum, each value needs to be flipped across the range 0 to 1
                            if (_start == max)
                                interp[x] = 1 - interp[x];
                            //convert interp[] range of 0 to 1 into range between selected beats
                            interp[x] = ((interp[x] - 0) / (1 - 0)) * (max - min) + min;
                            //write the datapoints to a temp object to store them
                            InterpolationCalc[start.beat + x + FrozenColumnOffset].Value = UtilMath.TruncateDecimal((decimal)interp[x], 3);
                        }
                    }
                    //transfer temp data points to another temp as the first one will be cleared
                    //this second one with sum together the tuning layers
                    for (int m = FrozenColumnOffset; m < SumOfLayers.Cells.Count; m++) {
                        if (SumOfLayers[m].Value == null)
                            SumOfLayers[m].Value = 0m;
                        SumOfLayers[m].Value = (decimal)SumOfLayers[m].Value + (decimal)(InterpolationCalc[m].Value ?? 0m);
                    }
                    ((EditorLeaf)_properties.ParentEditor).trackEditor.Rows.Remove(InterpolationCalc);
                }
                //write temp2 to the real object
                for (int m = FrozenColumnOffset; m < TargetTuningLayer.Cells.Count; m++) {
                    TargetTuningLayer[m].Value = (decimal)(SumOfLayers[m].Value ?? 0m);
                }
                ((EditorLeaf)_properties.ParentEditor).trackEditor.Rows.Remove(SumOfLayers);
            }
            finally {
                _properties.ParentEditor.LogUndo = true;
                _properties.ParentEditor.EditorIsTuning = false;
            }

            _properties.ParentEditor.SaveCheckAndWrite(false, "Interpolated values from tuning layer");
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
                    RowsToMove = ReturnLanesFromName(SequencerObjects[rowIndexFromMouseDown], SequencerObjects[rowIndexFromMouseDown].FriendlyLane);
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
                ((RowsToMove[0].FriendlyLane == "lane right 2" && targetRow >= RowsToMove[4].Index && targetRow <= RowsToMove[0].Index) ||
                (RowsToMove[0].FriendlyLane == "lane left 2" && targetRow >= RowsToMove[0].Index && targetRow <= RowsToMove[4].Index)))
                return;

            if (SequencerObjects[targetRow].FriendlyLane != "none" && SequencerObjects[targetRow].ExpandLanesInEditor) {
                return;
            }

            if (RowsToMove.Length == 5 && RowsToMove[0].FriendlyLane == "lane left 2" && targetRow < RowsToMove[0].Index)
                RowsToMove = RowsToMove.Reverse().ToArray();
            else if (RowsToMove.Length == 5 && RowsToMove[0].FriendlyLane == "lane right 2" && targetRow > RowsToMove[0].Index)
                RowsToMove = RowsToMove.Reverse().ToArray();

            if (SequencerObjects[targetRow].FriendlyLane == "lane center" && targetRow > RowsToMove[0].Index)
                targetRow += 2;
            else if (SequencerObjects[targetRow].FriendlyLane == "lane center" && targetRow < RowsToMove[0].Index)
                targetRow -= 2;


            if (RowsToMove != null && targetRow != -1 && targetRow != previousDragOver) {
                previousDragOver = targetRow;
                trackEditor.SuspendLayout();
                foreach (Sequencer_Object seq in RowsToMove) {
                    trackEditor.Rows.Remove(seq);
                    SequencerObjects.Remove(seq);
                    SequencerObjects.Insert(targetRow, seq);
                    trackEditor.Rows.Insert(targetRow, seq);
                }

                trackEditor.ResumeLayout();
            }
        }

        private void btnTrackPlayback_Click(object sender, EventArgs e)
        {
            if (Playback.IsPlaying) {
                Playback.IsPlaying = false;
                ForceStop = true;
            }
            else {
                //timer interval twice as small as the bpm (*500ms, instead of *1000ms), so it can keep up with the Playback threading timer
                //timer1.Interval = (int)((60 / TCLE.BPM) * (1000 / Playback.BeatSubdivisions));
                timer1.Interval = 30;
                btnTrackPlayback.Image = Properties.Resources.icon_stop;
                //
                JObject _saveJSON = LeafProperties.ConvertToJson();
                SimpleLeafProperties _leafload = UtilCreate.SimpleLeaf(_saveJSON, null);
                //
                Playback.Initialize("leaf");
                Playback.CreatePlaybackFromLeaf(_leafload, PlaybackEnd);
                Playback.Play(PlaybackStart, PlaybackEnd > -1 ? PlaybackEnd : _leafproperties.LeafLength, PlaybackLoop);
                if (Playback.IsPlaying) {
                    if (Properties.Settings.Default.LeafOptionPlaybackScroll) 
                        trackEditor.HorizontalScrollingOffset = (int)Math.Round((Math.Max(0, Playback.PlaybackBeat - Playback.GlobalCurrentOffset + Playback.PlaybackSubBeat)) * trackZoom.Value);                    
                    timer1.Enabled = true;
                }
                else {
                    Bass.BASS_ChannelFree(Playback.MidiStream);
                    TCLE.alzheimer();
                    btnTrackPlayback.Image = Properties.Resources.icon_play2;
                }
            }
        }

        private bool ForceStop;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Playback.PlaybackBeat < 0)
                return;
            if (Playback.IsPlaying /*&& Playback.PlaybackBeat + FrozenColumnOffset < trackEditor.ColumnCount*/) {
                trackEditor.PlaybackPosition = (double)(Playback.PlaybackBeat - Playback.GlobalCurrentOffset + Playback.PlaybackSubBeat);
                trackEditor.Invalidate();
                dgvMasterView.Invalidate();
                if (Properties.Settings.Default.LeafOptionPlaybackScroll) {
                    int playheadx = (int)Math.Round((Playback.PlaybackBeat - Playback.GlobalCurrentOffset + Playback.PlaybackSubBeat) * trackZoom.Value);
                    int margin = trackEditor.Width / 3;
                    if (playheadx > trackEditor.HorizontalScrollingOffset + margin)
                        trackEditor.HorizontalScrollingOffset = playheadx - margin;
                }
            }
            else {
                if (PlaybackLoop && !ForceStop)
                    return;
                ForceStop = false;
                timer1.Enabled = false;
                btnTrackPlayback.Image = Properties.Resources.icon_play2;
                Playback.StopPlayback();
                trackEditor.ResetPlayback();
                trackEditor.Invalidate();
            }
        }
    }
}
