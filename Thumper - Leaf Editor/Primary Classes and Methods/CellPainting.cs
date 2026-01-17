using System.Drawing.Drawing2D;
using Thumper_Custom_Level_Editor.Editor_Panels;

namespace Thumper_Custom_Level_Editor.Primary_Classes_and_Methods
{
    public static class CellPainting
    {
        public static int FrozenColumnOffset => Form_LeafEditor.FrozenColumnOffset;
        private static int IconWidth = 16;
        private static int IconHeight = 16;
        private static Pen PenCorn = new(Brushes.CornflowerBlue, 3);
        private static Pen PenRed = new(Brushes.Red, 3);
        private static Pen PenGreen = new(Brushes.Green, 3);
        private static Pen PenVioletThick = new(Brushes.Violet, 3);
        private static Pen PenWhite3 = new(Brushes.White, 3);
        private static Pen PenWhite2 = new(Brushes.White, 2);
        private static Pen PenGreen6 = new(Brushes.Green, 6);
        private static Pen PenBlack6 = new(Brushes.Black, 6);
        private static Pen PenRed6 = new(Brushes.Red, 6);
        private static Pen PenRowBorder = new(new SolidBrush(Color.FromArgb(10, 10, 10)), 2);
        private static StringFormat CellFormat = new(StringFormatFlags.NoWrap) { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
        private static StringFormat CellFormatVert = new(StringFormatFlags.NoWrap) { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center, FormatFlags = (StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft) };

        public static void SetCellBorders(DataGridViewCellPaintingEventArgs e, DataGridView trackEditor)
        {
            ///If showing grid AND connected bars
            if (Properties.Settings.Default.LeafOptionShowGrid && Properties.Settings.Default.LeafOptionConnectBars) {
                //if previous cell value is different than this cell, put in a divider
                //otherwise remove left border to "merge" cells
                if (e.Value != null && e.Value.ToString() == trackEditor[e.ColumnIndex - 1, e.RowIndex].Value?.ToString()) {
                }
                else if (e.Value != null)
                    e.Graphics.DrawLine(PenWhite2, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
                else
                    e.Graphics.DrawLine(Pens.Black, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
            }
            ///If showing grid and NOT connected bats
            else if (Properties.Settings.Default.LeafOptionShowGrid && !Properties.Settings.Default.LeafOptionConnectBars) {
                e.Graphics.DrawLine(Pens.Black, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
            }
            ///If NOT showing grid AND connected bars
            else if (!Properties.Settings.Default.LeafOptionShowGrid && Properties.Settings.Default.LeafOptionConnectBars) {
                //if previous cell value is different than this cell, put in a divider
                //otherwise remove left border to "merge" cells
                if (e.Value != null && e.Value.ToString() == trackEditor[e.ColumnIndex - 1, e.RowIndex].Value?.ToString()) {
                }
                else if (e.Value != null)
                    e.Graphics.DrawLine(PenWhite2, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
            }
            ///If NOT showing grid and NOT connected bars
            else if (!Properties.Settings.Default.LeafOptionShowGrid && !Properties.Settings.Default.LeafOptionConnectBars) {
                //paint nothing
            }

            e.Graphics.DrawLine(PenRowBorder, e.CellBounds.Left - 2, e.CellBounds.Top, e.CellBounds.Right + 4, e.CellBounds.Top);
            e.Graphics.DrawLine(PenRowBorder, e.CellBounds.Left - 2, e.CellBounds.Bottom, e.CellBounds.Right + 4, e.CellBounds.Bottom);
        }
        
        public static void DrawPlaybackHeaders(DataGridViewCellPaintingEventArgs e, int PlaybackStart, int PlaybackEnd, bool PlaybackLoop)
        {
            if (e.ColumnIndex < FrozenColumnOffset)
                return;
            if (e.ColumnIndex == PlaybackStart + FrozenColumnOffset || e.ColumnIndex - 1 == PlaybackEnd + FrozenColumnOffset) {
                Point p1 = new(e.CellBounds.Left + /*(e.CellBounds.Width / 2)*/ -6, e.CellBounds.Top);
                Point p2 = new(e.CellBounds.Left + /*(e.CellBounds.Width / 2)*/ +6, e.CellBounds.Top);
                Point p3 = new(e.CellBounds.Left /*+ (e.CellBounds.Width / 2)*/, e.CellBounds.Top + 10);
                if (e.ColumnIndex == PlaybackStart + FrozenColumnOffset)
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

        public static void DrawColors(DataGridViewCellPaintingEventArgs e, DataGridView trackEditor, List<Sequencer_Object> SequencerObjects)
        {
            //if cell is selected, skip all the fancy painting
            if (trackEditor[e.ColumnIndex, e.RowIndex].Selected) {
                e.Graphics.FillRectangle(Brushes.LightSkyBlue, e.CellBounds);
                return;
            }
            //grey out the track if disabled
            if (SequencerObjects[e.RowIndex].ReadOnly) {
                e.Graphics.FillRectangle(Brushes.Gray, e.CellBounds);
            }
            //if visual option "Thin Bars" and the row is collapsed, paint the rectangles as thin bars instead of taking up the whole cell.
            else if (Properties.Settings.Default.LeafOptionThinBars && SequencerObjects[e.RowIndex].friendly_lane == "lane center" && SequencerObjects[e.RowIndex].expandlanes == false) {
                if (SequencerObjects[e.RowIndex - 2][e.ColumnIndex].Value != null)
                    e.Graphics.FillRectangle(SequencerObjects[e.RowIndex].HighlightBrush, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width, e.CellBounds.Height / 5);
                if (SequencerObjects[e.RowIndex - 1][e.ColumnIndex].Value != null)
                    e.Graphics.FillRectangle(SequencerObjects[e.RowIndex].HighlightBrush, e.CellBounds.Left, e.CellBounds.Top + e.CellBounds.Height / 5, e.CellBounds.Width, e.CellBounds.Height / 5);
                if (SequencerObjects[e.RowIndex][e.ColumnIndex].Value != null)
                    e.Graphics.FillRectangle(SequencerObjects[e.RowIndex].HighlightBrush, e.CellBounds.Left, e.CellBounds.Top + (e.CellBounds.Height / 5 * 2), e.CellBounds.Width, e.CellBounds.Height / 5);
                if (SequencerObjects[e.RowIndex + 1][e.ColumnIndex].Value != null)
                    e.Graphics.FillRectangle(SequencerObjects[e.RowIndex].HighlightBrush, e.CellBounds.Left, e.CellBounds.Top + (e.CellBounds.Height / 5 * 3), e.CellBounds.Width, e.CellBounds.Height / 5);
                if (SequencerObjects[e.RowIndex + 2][e.ColumnIndex].Value != null) {
                    e.Graphics.FillRectangle(SequencerObjects[e.RowIndex].HighlightBrush, e.CellBounds.Left, e.CellBounds.Top + (e.CellBounds.Height / 5 * 4), e.CellBounds.Width, e.CellBounds.Height / 5);
                }
            }
            //if a color object, convert the cell value to ARGB and use that
            else if (SequencerObjects[e.RowIndex].trait_type is "kTraitColor") {
                if (SequencerObjects[e.RowIndex][e.ColumnIndex].Value != null)
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(Convert.ToInt32(e.Value))), e.CellBounds);
            }
            //paint the whole cell with the highlighting color
            else if (SequencerObjects[e.RowIndex].obj_name != "_TuningLayerX" && SequencerObjects[e.RowIndex].category != "PLAY SAMPLE") {
                if (e.Value != null && Math.Abs((decimal)e.Value) >= (decimal)SequencerObjects[e.RowIndex].defaultvalue)
                    e.Graphics.FillRectangle(SequencerObjects[e.RowIndex].HighlightBrush, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width, e.CellBounds.Height);
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

        public static void DrawPlaybackBars(DataGridViewCellPaintingEventArgs e, int PlaybackStart, int PlaybackEnd, bool PlaybackLoop, string LoadedLeaf)
        {
            if (e.ColumnIndex == PlaybackStart + FrozenColumnOffset) {
                e.Graphics.DrawLine(PenCorn, new Point(e.CellBounds.Left, e.CellBounds.Top), new Point(e.CellBounds.Left, e.CellBounds.Bottom));
            }

            if (e.ColumnIndex == PlaybackEnd + FrozenColumnOffset) {
                e.Graphics.DrawLine(PlaybackLoop ? PenGreen : PenRed, new Point(e.CellBounds.Right - 3, e.CellBounds.Top), new Point(e.CellBounds.Right - 3, e.CellBounds.Bottom));
            }

            if (Playback.IsPlaying && Playback.GlobalCurrentLeaf == LoadedLeaf && e.ColumnIndex == Playback.PlaybackBeat + FrozenColumnOffset - (Playback.GlobalCurrentOffset / 100)) {
                e.Graphics.DrawLine(PenVioletThick, new Point(e.CellBounds.Left + (int)(e.CellBounds.Width * Playback.PlaybackSubBeat), e.CellBounds.Top), new Point(e.CellBounds.Left + (int)(e.CellBounds.Width * Playback.PlaybackSubBeat), e.CellBounds.Bottom));
            }
        }

        public static void DrawCellValues(DataGridViewCellPaintingEventArgs e, DataGridView trackEditor, Sequencer_Object seq = null)
        {
            if ((e.PaintParts & DataGridViewPaintParts.ContentForeground) != 0 && e.Value != null) {
                //skips a bunch of objects since they display their values differently
                if (e.RowIndex == -1)
                    goto skipchecks;
                if (seq.category == "!!PLAY SAMPLE" && Properties.Settings.Default.LeafOptionShowWave)
                    return;
                else if (seq.trait_type is "kTraitColor")
                    return;
                else if ((Properties.Settings.Default.LeafOptionThinBars && seq.friendly_lane == "lane center" && seq.expandlanes == false))
                    return;
                else if (Properties.Settings.Default.LeafOptionConnectBars && e.ColumnIndex >= FrozenColumnOffset && e.Value.ToString() == trackEditor[e.ColumnIndex - 1, e.RowIndex].Value?.ToString())
                    return;

                Color _c = seq.highlight_color;
                //Tests highlight color contrast. If low, text color is set to white.
                if (_c.R < 150 && _c.G < 150 && _c.B < 150)
                    e.CellStyle.ForeColor = Color.White;
                else
                    e.CellStyle.ForeColor = Color.Black;
                skipchecks:
                string cellText = e.Value.ToString();
                //if using vertical text, string width needs to be tested against cell height instead of width
                //hence why this is in 2 blocks that do almost identical things
                if (Properties.Settings.Default.LeafOptionVerticalCells) {
                    for (int fontSize = 1; fontSize < 25; fontSize++) {
                        Font font = new("Consolas", fontSize);
                        Size textSize = TextRenderer.MeasureText(cellText, font);
                        //if font is within cell bounds, try font size +1. Or cap it at 24.
                        if (textSize.Width > e.CellBounds.Height + 2 || textSize.Height > e.CellBounds.Width || fontSize == 24) {
                            if (fontSize - 1 != 0)
                                font = new Font("Consolas", fontSize - 1);
                            e.CellStyle.Font = font;
                            e.Graphics.DrawString(cellText, font, new SolidBrush(e.CellStyle.ForeColor), e.CellBounds, CellFormatVert);
                            break;
                        }
                    }
                }
                else {
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
        }

        public static void CellPaintIcons(DataGridViewCellPaintingEventArgs e, Form_LeafEditor Leaf, Sequencer_Object seq = null)
        {
            if (e.RowIndex != -1 && seq.obj_name == "_TuningLayerX" && e.ColumnIndex is 1 or 2)
                return;
            //get dimensions
            int x = e.CellBounds.Left + ((e.CellBounds.Width - IconWidth) / 2);
            int y = e.CellBounds.Top + ((e.CellBounds.Height - IconHeight) / 2);
            //paint the image
            //Object Toggle
            if (e.ColumnIndex == 0) {
                if (e.RowIndex == -1) {
                    e.Graphics.DrawImage(Leaf.GlobalDisable ? Properties.Resources.icon_toggle_off : Properties.Resources.icon_toggle_on, new Rectangle(x, y, IconWidth, IconHeight));
                }
                else {
                    e.Graphics.DrawImage(seq.enabled ? Properties.Resources.icon_toggle_on : Properties.Resources.icon_toggle_off, new Rectangle(x, y, IconWidth, IconHeight));
                    Leaf.trackEditor[e.ColumnIndex, e.RowIndex].Selected = false;
                }
            }
            //Audio Mute/Unmute
            else if (e.ColumnIndex == 1) {
                if (e.RowIndex == -1) {
                    e.Graphics.DrawImage(Leaf.GlobalMute ? Properties.Resources.icon_audio_mute : Properties.Resources.icon_audio, new Rectangle(x, y, IconWidth, IconHeight));
                }
                else {
                    e.Graphics.DrawImage(seq.mute ? Properties.Resources.icon_audio_mute : Properties.Resources.icon_audio, new Rectangle(x, y, IconWidth, IconHeight));
                    Leaf.trackEditor[e.ColumnIndex, e.RowIndex].Selected = false;
                }
            }
            //Lane Expand
            else if (e.ColumnIndex == 2) {
                if (e.RowIndex == -1)
                    e.Graphics.DrawImage(Properties.Settings.Default.LeafOptionShowLane ? Properties.Resources.icon_lanesgray : Properties.Resources.icon_lanes, new Rectangle(x, y, IconWidth, IconHeight));
                else if (seq.friendly_lane == "lane center") {
                    e.Graphics.DrawImage(Properties.Settings.Default.LeafOptionShowLane ? Properties.Resources.icon_lanesgray : Properties.Resources.icon_lanes, new Rectangle(x, y, IconWidth, IconHeight));
                    Leaf.trackEditor[e.ColumnIndex, e.RowIndex].Selected = false;
                }
            }
        }

        private static SolidBrush CellPaintingPen = new(Color.FromArgb(60, 60, 60));
        private static SolidBrush CellPaintingPenBright = new(Color.FromArgb(100, 100, 100));
        private static SolidBrush CellPaintingColor = new(Color.Black);
        public static DataGridViewCell HoverCell { get; set; }
        public static List<int> SelectedRows = new();
        ///Paints rounded rectangles for the frozen columns
        public static void CellPaintFancy(DataGridViewCellPaintingEventArgs e, DataGridView trackEditor, Sequencer_Object seq = null)
        {
            //skip header row
            if (e.RowIndex == -1)
                return;
            Rectangle bounds = e.CellBounds;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //column -1 is row headers
            if (e.ColumnIndex is -1) {
                e.Graphics.FillRectangle(Brushes.Black, e.CellBounds);
                CellPaintingColor.Color = TCLE.Blend(seq.highlight_color, Color.Black, 0.4);
                bounds.X += 2;
                bounds.Y += 2;
                bounds.Width -= 4;
                bounds.Height -= 4;
                //Tuning Layers get an indent
                if (seq.obj_name == "_TuningLayerX") {
                    bounds.X += 20;
                    bounds.Width -= 20;
                }
                //if row has a selected cell, highlight it, using a brighter color and white outline
                if (SelectedRows.Contains(e.RowIndex)) {
                    e.Graphics.FillRoundedRectangle(Brushes.White, new Rectangle(bounds.X - 1, bounds.Y - 1, bounds.Width + 2, bounds.Height + 2), 5);
                    CellPaintingColor.Color = TCLE.Blend(seq.highlight_color, Color.Black, 0.8);
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
                if (seq.friendly_lane == "lane left 2") {
                    bounds.Height += 4;
                    e.Graphics.FillRoundedRectangle(CellPaintingPen, bounds, 4);
                }
                else if (seq.friendly_lane == "lane right 2") {
                    bounds.Y -= 2;
                    e.Graphics.FillRoundedRectangle(CellPaintingPen, bounds, 4);
                    //this rectangle is needed to square off the top of the above rounded rectangle
                    e.Graphics.FillRectangle(CellPaintingPen, new Rectangle(bounds.X, bounds.Y, bounds.Width, 5));
                }
                else if (seq.friendly_lane is "lane left 1" or "lane right 1" || (seq.expandlanes && seq.friendly_lane is "lane center")) {
                    bounds.Height += 3;
                    bounds.Y -= 3;
                    e.Graphics.FillRectangle(CellPaintingPen, bounds);
                }
                else
                    e.Graphics.FillRoundedRectangle(trackEditor[e.ColumnIndex, e.RowIndex] == HoverCell ? CellPaintingPenBright : CellPaintingPen, bounds, 4);
            }
        }

        public static void DrawLaneEnds(DataGridViewCellPaintingEventArgs e, Sequencer_Object seq, List<Sequencer_Object> SequencerObjects)
        {
            if (seq.friendly_param == "lane center") {
                if (seq[e.ColumnIndex].InGameValue == 1) {
                    if (seq[e.ColumnIndex - 1].InGameValue == 0) {
                        if (SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane left 1")?[e.ColumnIndex].InGameValue == 1) {
                            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left - (e.CellBounds.Width / 3), e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
                            e.Graphics.DrawLine(PenGreen6, e.CellBounds.Left - (e.CellBounds.Width / 3) + 6, e.CellBounds.Top, e.CellBounds.Left + 6, e.CellBounds.Bottom);
                        }
                        if (SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane right 1")?[e.ColumnIndex].InGameValue == 1) {
                            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left - (e.CellBounds.Width / 3), e.CellBounds.Bottom, e.CellBounds.Left, e.CellBounds.Top);
                            e.Graphics.DrawLine(PenGreen6, e.CellBounds.Left - (e.CellBounds.Width / 3) + 6, e.CellBounds.Bottom, e.CellBounds.Left + 6, e.CellBounds.Top);
                        }
                    }
                }
                else {
                    if (seq[e.ColumnIndex - 1].InGameValue == 1) {
                        if (SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane left 1")?[e.ColumnIndex].InGameValue == 1) {
                            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left, e.CellBounds.Bottom, e.CellBounds.Left + (e.CellBounds.Width / 3), e.CellBounds.Top);
                            e.Graphics.DrawLine(PenRed6, e.CellBounds.Left - 6, e.CellBounds.Bottom, e.CellBounds.Left + (e.CellBounds.Width / 3) - 6, e.CellBounds.Top);
                        }
                        if (SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane right 1")?[e.ColumnIndex].InGameValue == 1) {
                            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left + (e.CellBounds.Width / 3), e.CellBounds.Bottom);
                            e.Graphics.DrawLine(PenRed6, e.CellBounds.Left - 6, e.CellBounds.Top, e.CellBounds.Left + (e.CellBounds.Width / 3) - 6, e.CellBounds.Bottom);
                        }
                    }
                }
            }
            else if (seq.friendly_param == "lane left 1") {
                if (seq[e.ColumnIndex].InGameValue == 1) {
                    if (seq[e.ColumnIndex - 1].InGameValue == 0) {
                        if (SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane left 2")?[e.ColumnIndex].InGameValue == 1) {
                            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left - (e.CellBounds.Width / 3), e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
                            e.Graphics.DrawLine(PenGreen6, e.CellBounds.Left - (e.CellBounds.Width / 3) + 6, e.CellBounds.Top, e.CellBounds.Left + 6, e.CellBounds.Bottom);
                        }
                        if (SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane center")?[e.ColumnIndex].InGameValue == 1) {
                            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left - (e.CellBounds.Width / 3), e.CellBounds.Bottom, e.CellBounds.Left, e.CellBounds.Top);
                            e.Graphics.DrawLine(PenGreen6, e.CellBounds.Left - (e.CellBounds.Width / 3) + 6, e.CellBounds.Bottom, e.CellBounds.Left + 6, e.CellBounds.Top);
                        }
                    }
                }
                else {
                    if (seq[e.ColumnIndex - 1].InGameValue == 1) {
                        if (SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane left 2")?[e.ColumnIndex].InGameValue == 1) {
                            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left, e.CellBounds.Bottom, e.CellBounds.Left + (e.CellBounds.Width / 3), e.CellBounds.Top);
                            e.Graphics.DrawLine(PenRed6, e.CellBounds.Left - 6, e.CellBounds.Bottom, e.CellBounds.Left + (e.CellBounds.Width / 3) - 6, e.CellBounds.Top);
                        }
                        if (SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane center")?[e.ColumnIndex].InGameValue == 1) {
                            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left + (e.CellBounds.Width / 3), e.CellBounds.Bottom);
                            e.Graphics.DrawLine(PenRed6, e.CellBounds.Left - 6, e.CellBounds.Top, e.CellBounds.Left + (e.CellBounds.Width / 3) - 6, e.CellBounds.Bottom);
                        }
                    }
                }
            }
            else if (seq.friendly_param == "lane left 2") {
                if (seq[e.ColumnIndex].InGameValue == 1) {
                    if (seq[e.ColumnIndex - 1].InGameValue == 0) {
                        if (SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane left 1")?[e.ColumnIndex].InGameValue == 1) {
                            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left - (e.CellBounds.Width / 3), e.CellBounds.Bottom, e.CellBounds.Left, e.CellBounds.Top);
                            e.Graphics.DrawLine(PenGreen6, e.CellBounds.Left - (e.CellBounds.Width / 3) + 6, e.CellBounds.Bottom, e.CellBounds.Left + 6, e.CellBounds.Top);
                        }
                    }
                }
                else {
                    if (seq[e.ColumnIndex - 1].InGameValue == 1) {
                        if (SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane left 1")?[e.ColumnIndex].InGameValue == 1) {
                            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left + (e.CellBounds.Width / 3), e.CellBounds.Bottom);
                            e.Graphics.DrawLine(PenRed6, e.CellBounds.Left - 6, e.CellBounds.Top, e.CellBounds.Left + (e.CellBounds.Width / 3) - 6, e.CellBounds.Bottom);
                        }
                    }
                }
            }
            else if (seq.friendly_param == "lane right 1") {
                if (seq[e.ColumnIndex].InGameValue == 1) {
                    if (seq[e.ColumnIndex - 1].InGameValue == 0) {
                        if (SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane center")?[e.ColumnIndex].InGameValue == 1) {
                            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left - (e.CellBounds.Width / 3), e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
                            e.Graphics.DrawLine(PenGreen6, e.CellBounds.Left - (e.CellBounds.Width / 3) + 6, e.CellBounds.Top, e.CellBounds.Left + 6, e.CellBounds.Bottom);
                        }
                        if (SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane right 2")?[e.ColumnIndex].InGameValue == 1) {
                            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left - (e.CellBounds.Width / 3), e.CellBounds.Bottom, e.CellBounds.Left, e.CellBounds.Top);
                            e.Graphics.DrawLine(PenGreen6, e.CellBounds.Left - (e.CellBounds.Width / 3) + 6, e.CellBounds.Bottom, e.CellBounds.Left + 6, e.CellBounds.Top);
                        }
                    }
                }
                else {
                    if (seq[e.ColumnIndex - 1].InGameValue == 1) {
                        if (SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane center")?[e.ColumnIndex].InGameValue == 1) {
                            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left, e.CellBounds.Bottom, e.CellBounds.Left + (e.CellBounds.Width / 3), e.CellBounds.Top);
                            e.Graphics.DrawLine(PenRed6, e.CellBounds.Left - 6, e.CellBounds.Bottom, e.CellBounds.Left + (e.CellBounds.Width / 3) - 6, e.CellBounds.Top);
                        }
                        if (SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane right 2")?[e.ColumnIndex].InGameValue == 1) {
                            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left + (e.CellBounds.Width / 3), e.CellBounds.Bottom);
                            e.Graphics.DrawLine(PenRed6, e.CellBounds.Left - 6, e.CellBounds.Top, e.CellBounds.Left + (e.CellBounds.Width / 3) - 6, e.CellBounds.Bottom);
                        }
                    }
                }
            }
            else if (seq.friendly_param == "lane right 2") {
                if (seq[e.ColumnIndex].InGameValue == 1) {
                    if (seq[e.ColumnIndex - 1].InGameValue == 0) {
                        if (SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane right 1")?[e.ColumnIndex].InGameValue == 1) {
                            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left - (e.CellBounds.Width / 3), e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
                            e.Graphics.DrawLine(PenGreen6, e.CellBounds.Left - (e.CellBounds.Width / 3) + 6, e.CellBounds.Top, e.CellBounds.Left + 6, e.CellBounds.Bottom);
                        }
                    }
                }
                else {
                    if (seq[e.ColumnIndex - 1].InGameValue == 1) {
                        if (SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane right 1")?[e.ColumnIndex].InGameValue == 1) {
                            e.Graphics.DrawLine(PenBlack6, e.CellBounds.Left, e.CellBounds.Bottom, e.CellBounds.Left + (e.CellBounds.Width / 3), e.CellBounds.Top);
                            e.Graphics.DrawLine(PenRed6, e.CellBounds.Left - 6, e.CellBounds.Bottom, e.CellBounds.Left + (e.CellBounds.Width / 3) - 6, e.CellBounds.Top);
                        }
                    }
                }
            }
        }

        public static void DrawTurnAngles(DataGridViewCellPaintingEventArgs e, Sequencer_Object seq)
        {
            if (e.Value != null) {
                Pen ArrowPen = new(new SolidBrush(TCLE.Blend(seq.highlight_color, Color.Black, 0.4)), 8) { EndCap = LineCap.Triangle};
                e.Graphics.DrawLine(ArrowPen, e.CellBounds.Left, e.CellBounds.Top + (e.CellBounds.Height / 2), e.CellBounds.Left + (e.CellBounds.Width / 2), e.CellBounds.Top + (e.CellBounds.Height / 2));

                // Convert the angle from degrees to radians, as Math.Cos and Math.Sin use radians
                double angleRadians = (double)(decimal)e.Value * (Math.PI / 180.0);
                // Calculate the end point coordinates
                float endX = (e.CellBounds.Left + (e.CellBounds.Width / 2)) + (float)(20 * Math.Cos(angleRadians));
                float endY = (e.CellBounds.Top + (e.CellBounds.Height / 2)) - (float)(20 * Math.Sin(angleRadians));

                ArrowPen.CustomEndCap = new AdjustableArrowCap(2, 1);
                e.Graphics.DrawLine(ArrowPen, e.CellBounds.Left + (e.CellBounds.Width / 2), e.CellBounds.Top + (e.CellBounds.Height / 2), endX, endY);
            }
        }
    }
}
