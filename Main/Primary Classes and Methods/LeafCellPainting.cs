using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;
using Thumper_Custom_Level_Editor.Properties;

namespace Thumper_Custom_Level_Editor.Primary_Classes_and_Methods
{
    public static class LeafCellPainting
    {
        public static int FrozenColumnOffset => EditorLeaf.FrozenColumnOffset;
        public static int IconWidth = 16;
        public static int IconHeight = 16;
        public static Pen PenCorn = new(Brushes.CornflowerBlue, 3);
        public static Pen PenRed = new(Brushes.Red, 3);
        public static Pen PenGreen = new(Brushes.Green, 3);
        public static Pen PenVioletThick = new(Brushes.Violet, 3);
        public static Pen PenWhite3 = new(Brushes.White, 3);
        public static Pen PenWhite2 = new(Brushes.White, 2);
        public static Pen PenGreen6 = new(Brushes.Green, 6);
        public static Pen PenBlack6 = new(Brushes.Black, 6);
        public static Pen PenRed6 = new(Brushes.Red, 6);
        public static Pen PenRowBorder = new(new SolidBrush(Color.FromArgb(10, 10, 10)), 2);
        public static SolidBrush SelectionColor = new(Color.FromArgb(180, Color.LightSkyBlue));
        public static StringFormat CellFormat = new(StringFormatFlags.NoWrap) { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
        public static StringFormat CellFormatVert = new(StringFormatFlags.NoWrap) { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center, FormatFlags = (StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft) };

        public static void SetCellBorders(DataGridViewCellPaintingEventArgs e, DataGridView trackEditor)
        {
            bool showGrid = Settings.Default.LeafOptionShowGrid;
            bool connectBars = Settings.Default.LeafOptionConnectBars;
            //DrawDivider is true if any of these evaluate true
            // > connected bars enabled
            // > current value is null
            // > current value and previous value are NOT the same
            bool drawDivider = !connectBars || e.Value == null || !Equals(e.Value, trackEditor[e.ColumnIndex - 1, e.RowIndex].Value);
            if (showGrid && drawDivider) {
                //Use a white line if the current value is different than previous
                e.Graphics.DrawLine(e.Value != null ? PenWhite2 : Pens.Black, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
            }
            //paint the row top and bottom borders
            e.Graphics.DrawLine(PenRowBorder, e.CellBounds.Left - 2, e.CellBounds.Top, e.CellBounds.Right + 4, e.CellBounds.Top);
            e.Graphics.DrawLine(PenRowBorder, e.CellBounds.Left - 2, e.CellBounds.Bottom, e.CellBounds.Right + 4, e.CellBounds.Bottom);
        }
        
        public static void DrawPlaybackHeaders(DataGridViewCellPaintingEventArgs e, int PlaybackStart, int PlaybackEnd, bool PlaybackLoop)
        {
            if (e.ColumnIndex < FrozenColumnOffset)
                return;
            if (e.ColumnIndex == PlaybackStart || e.ColumnIndex - 1 == PlaybackEnd) {
                Point p1 = new(e.CellBounds.Left + /*(e.CellBounds.Width / 2)*/ -6, e.CellBounds.Top);
                Point p2 = new(e.CellBounds.Left + /*(e.CellBounds.Width / 2)*/ +6, e.CellBounds.Top);
                Point p3 = new(e.CellBounds.Left /*+ (e.CellBounds.Width / 2)*/, e.CellBounds.Top + 10);
                if (e.ColumnIndex == PlaybackStart)
                    e.Graphics.FillPolygon(Brushes.CornflowerBlue, new[] { p1, p2, p3 });
                else
                    e.Graphics.FillPolygon(PlaybackLoop ? Brushes.Green : Brushes.Red, new[] { p1, p2, p3 });
            }
            /*if (PlaybackEnd != -2 && e.ColumnIndex == PlaybackEnd + FrozenColumnOffset && e.ColumnIndex == trackEditor.ColumnCount - 1) {
                Point p1 = new(e.CellBounds.Right + -6, e.CellBounds.Top);
                Point p2 = new(e.CellBounds.Right + +6, e.CellBounds.Top);
                Point p3 = new(e.CellBounds.Right, e.CellBounds.Top + 10);
                e.Graphics.FillPolygon(PlaybackLoop ? Brushes.Green : Brushes.Red, new[] { p1, p2, p3 });
            }*/
        }

        public static void DrawColors(DataGridViewCellPaintingEventArgs e, List<Sequencer_Object> SequencerObjects)
        {
            Sequencer_Object seq = SequencerObjects[e.RowIndex];
            //grey out the track if disabled
            if (seq.ReadOnly) {
                e.Graphics.FillRectangle(Brushes.Gray, e.CellBounds);
            }
            //if visual option "Thin Bars" and the row is collapsed, paint the rectangles as thin bars instead of taking up the whole cell.
            else if (Properties.Settings.Default.LeafOptionThinBars && seq.FriendlyLane == "lane center" && seq.ExpandLanesInEditor == false) {
                if (SequencerObjects[e.RowIndex - 2][e.ColumnIndex].Value != null)
                    e.Graphics.FillRectangle(seq.HighlightBrush, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width, e.CellBounds.Height / 5);
                if (SequencerObjects[e.RowIndex - 1][e.ColumnIndex].Value != null)
                    e.Graphics.FillRectangle(seq.HighlightBrush, e.CellBounds.Left, e.CellBounds.Top + e.CellBounds.Height / 5, e.CellBounds.Width, e.CellBounds.Height / 5);
                if (SequencerObjects[e.RowIndex][e.ColumnIndex].Value != null)
                    e.Graphics.FillRectangle(seq.HighlightBrush, e.CellBounds.Left, e.CellBounds.Top + (e.CellBounds.Height / 5 * 2), e.CellBounds.Width, e.CellBounds.Height / 5);
                if (SequencerObjects[e.RowIndex + 1][e.ColumnIndex].Value != null)
                    e.Graphics.FillRectangle(seq.HighlightBrush, e.CellBounds.Left, e.CellBounds.Top + (e.CellBounds.Height / 5 * 3), e.CellBounds.Width, e.CellBounds.Height / 5);
                if (SequencerObjects[e.RowIndex + 2][e.ColumnIndex].Value != null) {
                    e.Graphics.FillRectangle(seq.HighlightBrush, e.CellBounds.Left, e.CellBounds.Top + (e.CellBounds.Height / 5 * 4), e.CellBounds.Width, e.CellBounds.Height / 5);
                }
            }
            //if a color object, convert the cell value to ARGB and use that
            else if (seq.Default.TraitType is DefaultSequencerObject.Trait.Color) {
                if (SequencerObjects[e.RowIndex][e.ColumnIndex].Value != null)
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(Convert.ToInt32(e.Value))), e.CellBounds);
            }
            //paint the whole cell with the highlighting color
            else if (seq.ObjName != "_TuningLayerX" && seq.Default.Category != "PLAY SAMPLE") {
                if (e.Value != null && Math.Abs((decimal)e.Value) >= (decimal)seq.highlight_value)
                    e.Graphics.FillRectangle(seq.HighlightBrush, e.CellBounds.Left - 1, e.CellBounds.Top, e.CellBounds.Width + 2, e.CellBounds.Height);
            }
        }

        public static void DrawSelection(DataGridViewCellPaintingEventArgs e, DataGridView trackEditor)
        {
            //if cell is selected
            if (trackEditor[e.ColumnIndex, e.RowIndex].Selected) {
                e.Graphics.FillRectangle(SelectionColor, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width, e.CellBounds.Height);
            }
        }

        public static void DrawInterpEase(DataGridViewCellPaintingEventArgs e, Sequencer_Object seq)
        {
            //paint notifier circles for changed interp and ease
            if (Properties.Settings.Default.LeafOptionEaseDots) {
                if (seq[e.ColumnIndex].Interpolation != "Linear") {
                    e.Graphics.FillEllipse(Brushes.Black, e.CellBounds.Right - (e.CellBounds.Width / 2) - 6, e.CellBounds.Top - 1, 7, 7);
                    e.Graphics.FillEllipse(Brushes.Red, e.CellBounds.Right - (e.CellBounds.Width / 2) - 5, e.CellBounds.Top - 1, 5, 5);
                }
                if (seq[e.ColumnIndex].Ease != "Ease In Out") {
                    e.Graphics.FillEllipse(Brushes.Black, e.CellBounds.Right - (e.CellBounds.Width / 2), e.CellBounds.Top - 1, 7, 7);
                    e.Graphics.FillEllipse(Brushes.Blue, e.CellBounds.Right - (e.CellBounds.Width / 2), e.CellBounds.Top - 1, 5, 5);
                }
            }
        }

        public static void DrawPlaybackBars(DataGridViewCellPaintingEventArgs e, int PlaybackStart, int PlaybackEnd, bool PlaybackLoop)
        {
            if (e.ColumnIndex == PlaybackStart) {
                e.Graphics.DrawLine(PenCorn, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
            }

            if (e.ColumnIndex == PlaybackEnd) {
                e.Graphics.DrawLine(PlaybackLoop ? PenGreen : PenRed, e.CellBounds.Right - 3, e.CellBounds.Top, e.CellBounds.Right - 3, e.CellBounds.Bottom);
            }
        }

        public static void DrawText(DataGridViewCellPaintingEventArgs e, Sequencer_Object seq = null)
        {
            if (e.Value is null or "")
                return;
            //skips a bunch of objects since they display their values differently
            if (e.RowIndex == -1)
                goto skipchecks;
            if (seq.Default.Category == "!!PLAY SAMPLE" && Properties.Settings.Default.LeafOptionShowWave)
                return;
            else if (seq.Default.TraitType is DefaultSequencerObject.Trait.Color)
                return;
            else if ((Properties.Settings.Default.LeafOptionThinBars && seq.FriendlyLane == "lane center" && seq.ExpandLanesInEditor == false))
                return;
            else if (Properties.Settings.Default.LeafOptionConnectBars && e.ColumnIndex > FrozenColumnOffset && (decimal?)e.Value == (decimal?)seq[e.ColumnIndex - 1].Value)
                return;

            //Tests highlight color contrast. If low, text color is set to white.
            if (seq.HighlightColor.R < 150 && seq.HighlightColor.G < 150 && seq.HighlightColor.B < 150)
                e.CellStyle.ForeColor = Color.White;
            else
                e.CellStyle.ForeColor = Color.Black;
        skipchecks:
            using var BrushTextColor = new SolidBrush(e.CellStyle.ForeColor);
            string cellText = e.Value.ToString();
            //if using vertical text, string width needs to be tested against cell height instead of width
            //hence why this is in 2 blocks that do almost identical things
            if (Properties.Settings.Default.LeafOptionVerticalCells) {
                using Font font = new(TCLE.ImportedFonts.Families[0], 10);
                SizeF RealSize = e.Graphics.MeasureString(cellText, font);
                Rectangle bounds = e.CellBounds;
                if (seq?.FriendlyParam is "turn" or "turn_auto")
                    bounds = new(e.CellBounds.Left, e.CellBounds.Top + (e.CellBounds.Height / 2), e.CellBounds.Width, e.CellBounds.Height / 2);
                float WidthScaleRatio = (bounds.Height + 4) / RealSize.Width;
                float HeightScaleRatio = (bounds.Width + 4) / RealSize.Height;
                float ScaleFontSize = font.Size * ((HeightScaleRatio < WidthScaleRatio) ? HeightScaleRatio : WidthScaleRatio);
                e.Graphics.DrawString(cellText, new Font(TCLE.ImportedFonts.Families[0], ScaleFontSize, GraphicsUnit.Pixel), BrushTextColor, bounds, CellFormatVert);
            }
            else {
                using Font font = new(TCLE.ImportedFonts.Families[0], 10);
                SizeF RealSize = e.Graphics.MeasureString(cellText, font);
                Rectangle bounds = e.CellBounds;
                if (seq?.FriendlyParam is "turn" or "turn_auto")
                    bounds = new(e.CellBounds.Left, e.CellBounds.Top + (e.CellBounds.Height / 2), e.CellBounds.Width, e.CellBounds.Height / 2);
                float HeightScaleRatio = (bounds.Height + 4) / RealSize.Height;
                float WidthScaleRatio = (bounds.Width + 4) / RealSize.Width;
                float ScaleFontSize = font.Size * ((HeightScaleRatio < WidthScaleRatio) ? HeightScaleRatio : WidthScaleRatio);
                e.Graphics.DrawString(cellText, new Font(TCLE.ImportedFonts.Families[0], ScaleFontSize, GraphicsUnit.Pixel), BrushTextColor, bounds, CellFormat);
            }
        }

        public static void CellPaintIcons(DataGridViewCellPaintingEventArgs e, EditorLeaf Leaf, Sequencer_Object seq = null)
        {
            if (e.RowIndex != -1 && seq.ObjName == "_TuningLayerX" && e.ColumnIndex is 1 or 2)
                return;
            //get dimensions
            int x = e.CellBounds.Left + ((e.CellBounds.Width - IconWidth) / 2);
            int y = e.CellBounds.Top + ((e.CellBounds.Height - IconHeight) / 2);
            //paint the image
            //Object Toggle
            switch (e.ColumnIndex) {
                case -1:
                    if (seq != null && Properties.Settings.Default.LeafOptionCategoryIcon && seq.Default.CategoryIcon != null) {
                        e.Graphics.DrawImage(seq.Default.CategoryIcon, e.CellBounds.Left + 10, y, 16, 16);
                    }
                    break;
                case 0:
                    if (e.RowIndex == -1) {
                        e.Graphics.DrawImage(Leaf.GlobalDisable ? Resources.icon_toggle_off : Resources.icon_toggle_on, x, y, IconWidth, IconHeight);
                    }
                    else {
                        e.Graphics.DrawImage(seq.EnabledInEditor ? Resources.icon_toggle_on : Resources.icon_toggle_off, x, y, IconWidth, IconHeight);
                        Leaf.trackEditor[e.ColumnIndex, e.RowIndex].Selected = false;
                    }
                    break;
                //Audio Mute/Unmute
                case 1:
                    if (e.RowIndex == -1) {
                        e.Graphics.DrawImage(Leaf.GlobalMute ? Resources.icon_audio_mute : Resources.icon_audio, x, y, IconWidth, IconHeight);
                    }
                    else {
                        e.Graphics.DrawImage(seq.MuteInEditor ? Resources.icon_audio_mute : Resources.icon_audio, x, y, IconWidth, IconHeight);
                        Leaf.trackEditor[e.ColumnIndex, e.RowIndex].Selected = false;
                    }
                    break;
                //Lane Expand
                case 2:
                    if (e.RowIndex == -1)
                        e.Graphics.DrawImage(Settings.Default.LeafOptionShowLane ? Resources.icon_lanesgray : Resources.icon_lanes, x, y, IconWidth, IconHeight);
                    else if (seq.FriendlyLane == "lane center") {
                        e.Graphics.DrawImage(Settings.Default.LeafOptionShowLane ? Resources.icon_lanesgray : Resources.icon_lanes, x, y, IconWidth, IconHeight);
                        Leaf.trackEditor[e.ColumnIndex, e.RowIndex].Selected = false;
                    }
                    break;
            }
        }

        private static SolidBrush CellPaintingPen = new(Color.FromArgb(60, 60, 60));
        private static SolidBrush CellPaintingPenBright = new(Color.FromArgb(100, 100, 100));
        private static SolidBrush CellPaintingColor = new(Color.Black);
        public static DataGridViewCell HoverCell { get; set; }
        ///Paints rounded rectangles for the frozen columns
        public static void CellPaintFancy(DataGridViewCellPaintingEventArgs e, DataGridView trackEditor, List<int> SelectedRows, Sequencer_Object seq = null)
        {
            //skip header row
            if (e.RowIndex == -1)
                return;
            Rectangle bounds = e.CellBounds;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //column -1 is row headers
            if (e.ColumnIndex is -1) {
                e.Graphics.FillRectangle(Brushes.Black, e.CellBounds);
                CellPaintingColor.Color = UtilMath.Blend(seq.HighlightColor, Color.Black, 0.4);
                bounds.X += 2;
                bounds.Y += 2;
                bounds.Width -= 4;
                bounds.Height -= 4;
                //Tuning Layers get an indent
                if (seq.ObjName == "_TuningLayerX") {
                    bounds.X += 20;
                    bounds.Width -= 20;
                }
                //if row has a selected cell, highlight it, using a brighter color and white outline
                if (SelectedRows.Contains(e.RowIndex)) {
                    e.Graphics.FillRoundedRectangle(Brushes.White, new Rectangle(bounds.X - 1, bounds.Y - 1, bounds.Width + 2, bounds.Height + 2), 5);
                    CellPaintingColor.Color = UtilMath.Blend(seq.HighlightColor, Color.Black, 0.8);
                }
                e.Graphics.FillRoundedRectangle(CellPaintingColor, bounds, 5);
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
            }
            //colums 0 and 1 are Enable and Mute
            else if (e.ColumnIndex is 0 or 1) {
                e.Graphics.FillRectangle(Brushes.Black, e.CellBounds);
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
                e.Graphics.FillRectangle(Brushes.Black, new(e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width - 2, e.CellBounds.Height));
                bounds.X += 1;
                bounds.Y += 1;
                bounds.Width -= 6;
                bounds.Height -= 2;
                if (seq.FriendlyLane == "lane left 2") {
                    bounds.Height += 4;
                    e.Graphics.FillRoundedRectangle(CellPaintingPen, bounds, 4);
                }
                else if (seq.FriendlyLane == "lane right 2") {
                    bounds.Y -= 2;
                    e.Graphics.FillRoundedRectangle(CellPaintingPen, bounds, 4);
                    //this rectangle is needed to square off the top of the above rounded rectangle
                    e.Graphics.FillRectangle(CellPaintingPen, new Rectangle(bounds.X, bounds.Y, bounds.Width, 5));
                }
                else if (seq.FriendlyLane is "lane left 1" or "lane right 1" || (seq.ExpandLanesInEditor && seq.FriendlyLane is "lane center")) {
                    bounds.Height += 3;
                    bounds.Y -= 3;
                    e.Graphics.FillRectangle(CellPaintingPen, bounds);
                }
                else
                    e.Graphics.FillRoundedRectangle(trackEditor[e.ColumnIndex, e.RowIndex] == HoverCell ? CellPaintingPenBright : CellPaintingPen, bounds, 4);
            }
        }

        public static void DrawLaneEnds(DataGridViewCellPaintingEventArgs e, Sequencer_Object seq, Dictionary<string, Sequencer_Object> lanes)
        {
            switch (seq.FriendlyParam) {
                case "lane center":
                    if (seq[e.ColumnIndex].InGameValue == 1) {
                        if (seq[e.ColumnIndex - 1].InGameValue == 0) {
                            if (lanes.GetValueOrDefault("lane left 1")?[e.ColumnIndex].InGameValue == 1)
                                DrawLaneStartUp(e);
                            if (lanes.GetValueOrDefault("lane right 1")?[e.ColumnIndex].InGameValue == 1)
                                DrawLaneStartDown(e);
                        }
                    }
                    else {
                        if (seq[e.ColumnIndex - 1].InGameValue == 1) {
                            if (lanes.GetValueOrDefault("lane left 1")?[e.ColumnIndex].InGameValue == 1)
                                DrawLaneEndUp(e);
                            if (lanes.GetValueOrDefault("lane right 1")?[e.ColumnIndex].InGameValue == 1)
                                DrawLaneEndDown(e);
                        }
                    }
                    break;
                case "lane left 1":
                    if (seq[e.ColumnIndex].InGameValue == 1) {
                        if (seq[e.ColumnIndex - 1].InGameValue == 0) {
                            if (lanes.GetValueOrDefault("lane left 2")?[e.ColumnIndex].InGameValue == 1)
                                DrawLaneStartUp(e);
                            if (lanes.GetValueOrDefault("lane center")?[e.ColumnIndex].InGameValue == 1)
                                DrawLaneStartDown(e);
                        }
                    }
                    else {
                        if (seq[e.ColumnIndex - 1].InGameValue == 1) {
                            if (lanes.GetValueOrDefault("lane left 2")?[e.ColumnIndex].InGameValue == 1)
                                DrawLaneEndUp(e);
                            if (lanes.GetValueOrDefault("lane center")?[e.ColumnIndex].InGameValue == 1)
                                DrawLaneEndDown(e);
                        }
                    }
                    break;
                case "lane left 2":
                    if (seq[e.ColumnIndex].InGameValue == 1) {
                        if (seq[e.ColumnIndex - 1].InGameValue == 0) {
                            if (lanes.GetValueOrDefault("lane left 1")?[e.ColumnIndex].InGameValue == 1)
                                DrawLaneStartDown(e);
                        }
                    }
                    else {
                        if (seq[e.ColumnIndex - 1].InGameValue == 1) {
                            if (lanes.GetValueOrDefault("lane left 1")?[e.ColumnIndex].InGameValue == 1)
                                DrawLaneEndDown(e);
                        }
                    }
                    break;
                case "lane right 1":
                    if (seq[e.ColumnIndex].InGameValue == 1) {
                        if (seq[e.ColumnIndex - 1].InGameValue == 0) {
                            if (lanes.GetValueOrDefault("lane center")?[e.ColumnIndex].InGameValue == 1)
                                DrawLaneStartUp(e);
                            if (lanes.GetValueOrDefault("lane right 2")?[e.ColumnIndex].InGameValue == 1)
                                DrawLaneStartDown(e);
                        }
                    }
                    else {
                        if (seq[e.ColumnIndex - 1].InGameValue == 1) {
                            if (lanes.GetValueOrDefault("lane center")?[e.ColumnIndex].InGameValue == 1)
                                DrawLaneEndUp(e);
                            if (lanes.GetValueOrDefault("lane right 2")?[e.ColumnIndex].InGameValue == 1)
                                DrawLaneEndDown(e);
                        }
                    }
                    break;
                case "lane right 2":
                    if (seq[e.ColumnIndex].InGameValue == 1) {
                        if (seq[e.ColumnIndex - 1].InGameValue == 0) {
                            if (lanes.GetValueOrDefault("lane right 1")?[e.ColumnIndex].InGameValue == 1)
                                DrawLaneStartUp(e);
                        }
                    }
                    else {
                        if (seq[e.ColumnIndex - 1].InGameValue == 1) {
                            if (lanes.GetValueOrDefault("lane right 1")?[e.ColumnIndex].InGameValue == 1)
                                DrawLaneEndUp(e);
                        }
                    }
                    break;
            }
        }
        public static void DrawLaneStartUp(DataGridViewCellPaintingEventArgs e)
        {
            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left - (e.CellBounds.Width / 3), e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
            e.Graphics.DrawLine(PenGreen6, e.CellBounds.Left - (e.CellBounds.Width / 3) + 6, e.CellBounds.Top, e.CellBounds.Left + 6, e.CellBounds.Bottom);
        }
        public static void DrawLaneStartDown(DataGridViewCellPaintingEventArgs e)
        {
            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left - (e.CellBounds.Width / 3), e.CellBounds.Bottom, e.CellBounds.Left, e.CellBounds.Top);
            e.Graphics.DrawLine(PenGreen6, e.CellBounds.Left - (e.CellBounds.Width / 3) + 6, e.CellBounds.Bottom, e.CellBounds.Left + 6, e.CellBounds.Top);
        }
        public static void DrawLaneEndUp(DataGridViewCellPaintingEventArgs e)
        {
            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left, e.CellBounds.Bottom, e.CellBounds.Left + (e.CellBounds.Width / 3), e.CellBounds.Top);
            e.Graphics.DrawLine(PenRed6, e.CellBounds.Left - 6, e.CellBounds.Bottom, e.CellBounds.Left + (e.CellBounds.Width / 3) - 6, e.CellBounds.Top);
        }
        public static void DrawLaneEndDown(DataGridViewCellPaintingEventArgs e)
        {
            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left + (e.CellBounds.Width / 3), e.CellBounds.Bottom);
            e.Graphics.DrawLine(PenRed6, e.CellBounds.Left - 6, e.CellBounds.Top, e.CellBounds.Left + (e.CellBounds.Width / 3) - 6, e.CellBounds.Bottom);
        }

        public static Pen ArrowHighlight = new(Brushes.White, 5) { EndCap = LineCap.Triangle, CustomEndCap = new AdjustableArrowCap(3, 1) };
        public static Dictionary<Color, Pen> TurnArrowPenCache = new();
        public static Pen GetArrowPen(Color color)
        {
            if (!TurnArrowPenCache.TryGetValue(color, out Pen pen)) {
                TurnArrowPenCache[color] = new(new SolidBrush(UtilMath.Blend(color, Color.Black, 0.2)), 5) { 
                    Color = UtilMath.Blend(color, Color.Black, 0.2),
                    EndCap = LineCap.Triangle,
                    CustomEndCap = new AdjustableArrowCap(3, 1)
                };
                pen = TurnArrowPenCache[color];
            }
            return pen; 
        }
        public static void DrawTurnAngles(DataGridViewCellPaintingEventArgs e, Sequencer_Object seq)
        {
            if (e.Value != null) {
                Pen ArrowPen = GetArrowPen(seq.HighlightColor);
                //ArrowPen.CustomEndCap = new AdjustableArrowCap(3, 1);
                //e.Graphics.DrawLine(ArrowPen, e.CellBounds.Left + (e.CellBounds.Width / 2), e.CellBounds.Bottom, e.CellBounds.Left + (e.CellBounds.Width / 2), e.CellBounds.Top + (e.CellBounds.Height / 2));

                // Convert the angle from degrees to radians, as Math.Cos and Math.Sin use radians
                double angleRadians = (double)(decimal)e.Value * (Math.PI / 180.0);
                // Calculate the end point coordinates
                float endX = (e.CellBounds.Left + (e.CellBounds.Width / 2)) + (float)((Math.Min(e.CellBounds.Width / 2, e.CellBounds.Height / 3)) * Math.Cos(angleRadians));
                float endY = (e.CellBounds.Top + (e.CellBounds.Height / 3)) - (float)((Math.Min(e.CellBounds.Width / 2, e.CellBounds.Height / 3)) * Math.Sin(angleRadians));

                decimal angle = UtilMath.mod((decimal)e.Value, 360);
                if ((decimal)e.Value > 0) {
                    if (angle is (> 0 and <= 45) or (> 315))
                        e.Graphics.DrawLine(ArrowHighlight, e.CellBounds.Left + (e.CellBounds.Width / 2), e.CellBounds.Top + (e.CellBounds.Height / 3) - 1, endX, endY - 1);
                    else if (angle is (> 45 and <= 135))
                        e.Graphics.DrawLine(ArrowHighlight, e.CellBounds.Left + (e.CellBounds.Width / 2) - 1, e.CellBounds.Top + (e.CellBounds.Height / 3), endX - 1, endY);
                    else if (angle is (> 135 and <= 225))
                        e.Graphics.DrawLine(ArrowHighlight, e.CellBounds.Left + (e.CellBounds.Width / 2), e.CellBounds.Top + (e.CellBounds.Height / 3) + 1, endX, endY + 1);
                    else if (angle is (> 225 and <= 315))
                        e.Graphics.DrawLine(ArrowHighlight, e.CellBounds.Left + (e.CellBounds.Width / 2) + 1, e.CellBounds.Top + (e.CellBounds.Height / 3), endX + 1, endY);
                }
                else if ((decimal)e.Value < 0) {
                    if (angle is (> 0 and <= 45) or (> 315))
                        e.Graphics.DrawLine(ArrowHighlight, e.CellBounds.Left + (e.CellBounds.Width / 2), e.CellBounds.Top + (e.CellBounds.Height / 3) + 1, endX, endY + 1);
                    else if (angle is (> 45 and <= 135))
                        e.Graphics.DrawLine(ArrowHighlight, e.CellBounds.Left + (e.CellBounds.Width / 2) + 1, e.CellBounds.Top + (e.CellBounds.Height / 3), endX + 1, endY);
                    else if (angle is (> 135 and <= 225))
                        e.Graphics.DrawLine(ArrowHighlight, e.CellBounds.Left + (e.CellBounds.Width / 2), e.CellBounds.Top + (e.CellBounds.Height / 3) - 1, endX, endY - 1);
                    else if (angle is (> 225 and <= 315))
                        e.Graphics.DrawLine(ArrowHighlight, e.CellBounds.Left + (e.CellBounds.Width / 2) - 1, e.CellBounds.Top + (e.CellBounds.Height / 3), endX - 1, endY);
                }
                e.Graphics.DrawLine(ArrowPen, e.CellBounds.Left + (e.CellBounds.Width / 2), e.CellBounds.Top + (e.CellBounds.Height / 3), endX, endY);
            }
        }

        public static void DrawLaneDividers(DataGridViewCellPaintingEventArgs e, string lane)
        {
            if (lane == "a01") {
                e.Graphics.DrawLine(PenVioletThick, e.CellBounds.Left - 3, e.CellBounds.Top, e.CellBounds.Right + 3, e.CellBounds.Top);
            }
            else if (lane == "z02") {
                e.Graphics.DrawLine(PenVioletThick, e.CellBounds.Left - 3, e.CellBounds.Bottom - 2, e.CellBounds.Right + 3, e.CellBounds.Bottom - 2);
            }
        }
    }
}
