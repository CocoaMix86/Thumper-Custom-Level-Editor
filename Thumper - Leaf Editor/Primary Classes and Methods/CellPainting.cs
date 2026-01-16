using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thumper_Custom_Level_Editor.Editor_Panels;

namespace Thumper_Custom_Level_Editor.Primary_Classes_and_Methods
{
    public static class CellPainting
    {
        public static int FrozenColumnOffset => Form_LeafEditor.FrozenColumnOffset;
        private static Pen PenCorn = new(Brushes.CornflowerBlue, 3);
        private static Pen PenRed = new(Brushes.Red, 3);
        private static Pen PenGreen = new(Brushes.Green, 3);
        private static Pen PenVioletThick = new(Brushes.Violet, 3);
        private static StringFormat CellFormat = new(StringFormatFlags.NoWrap) { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
        private static StringFormat CellFormatVert = new(StringFormatFlags.NoWrap) { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center, FormatFlags = (StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft) };

        public static void SetCellBorders(DataGridViewCellPaintingEventArgs e, DataGridView trackEditor, List<Sequencer_Object> SequencerObjects)
        {
            ///If showing grid AND connected bars
            if (Properties.Settings.Default.LeafOptionShowGrid && Properties.Settings.Default.LeafOptionConnectBars) {
                e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                //if previous cell value is different than this cell, put in a divider
                //otherwise remove left border to "merge" cells
                if (e.Value != null && e.Value.ToString() != trackEditor[e.ColumnIndex - 1, e.RowIndex].Value?.ToString())
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.Outset;
                else if (e.Value != null)
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.Single;
            }
            ///If showing grid and NOT connected bats
            else if (Properties.Settings.Default.LeafOptionShowGrid && !Properties.Settings.Default.LeafOptionConnectBars) {
                e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.Single;
            }
            ///If NOT showing grid AND connected bars
            else if (!Properties.Settings.Default.LeafOptionShowGrid && Properties.Settings.Default.LeafOptionConnectBars) {
                e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                if (e.Value != null && e.Value.ToString() != trackEditor[e.ColumnIndex - 1, e.RowIndex].Value?.ToString())
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.Outset;
                else
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            }
            ///If NOT showing grid and NOT connected bars
            else if (!Properties.Settings.Default.LeafOptionShowGrid && !Properties.Settings.Default.LeafOptionConnectBars) {
                e.AdvancedBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
            }

            e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.Outset;
            //draw thick border top and bottom for the outer lane rows to make it more obvious they are grouped.
            if (SequencerObjects[e.RowIndex].friendly_lane is "lane left 2") {
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.InsetDouble;
            }
            else if (SequencerObjects[e.RowIndex].friendly_lane is "lane right 2") {
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.InsetDouble;
            }
        }
        
        public static void DrawPlaybackHeaders(DataGridViewCellPaintingEventArgs e, int PlaybackStart, int PlaybackEnd, bool PlaybackLoop)
        {
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

        public static void DrawValues(DataGridViewCellPaintingEventArgs e, DataGridView trackEditor, List<Sequencer_Object> SequencerObjects)
        {
            //if cell is selected, skip all the fancy painting
            if (trackEditor[e.ColumnIndex, e.RowIndex].Selected) {
                return;
            }
            //grey out the track if disabled
            else if (SequencerObjects[e.RowIndex].ReadOnly) {
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
                if (SequencerObjects[e.RowIndex][e.ColumnIndex].Value != null)
                    e.Graphics.FillRectangle(SequencerObjects[e.RowIndex].HighlightBrush, e.CellBounds.Left - 2, e.CellBounds.Top, e.CellBounds.Width + 4, e.CellBounds.Height);
            }
        }

        public static void DrawInterpEase(DataGridViewCellPaintingEventArgs e, List<Sequencer_Object> SequencerObjects)
        {
            //paint notifier circles for changed interp and ease
            if (Properties.Settings.Default.LeafOptionEaseDots) {
                if (SequencerObjects[e.RowIndex][e.ColumnIndex].Interpolation != "Linear") {
                    e.Graphics.FillEllipse(Brushes.Black, e.CellBounds.Right - (e.CellBounds.Width / 2) - 6, e.CellBounds.Top - 1, 7, 7);
                    e.Graphics.FillEllipse(Brushes.Red, e.CellBounds.Right - (e.CellBounds.Width / 2) - 5, e.CellBounds.Top - 1, 5, 5);
                }
                if (SequencerObjects[e.RowIndex][e.ColumnIndex].Ease != "Ease In Out") {
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

        public static void DrawCellValues(DataGridViewCellPaintingEventArgs e, DataGridView trackEditor, List<Sequencer_Object> SequencerObjects)
        {
            if ((e.PaintParts & DataGridViewPaintParts.ContentForeground) != 0 && e.Value != null) {
                //skips a bunch of objects since they display their values differently
                if (e.RowIndex == -1)
                    goto skipchecks;
                if (SequencerObjects[e.RowIndex].category == "!!PLAY SAMPLE" && Properties.Settings.Default.LeafOptionShowWave)
                    return;
                else if (SequencerObjects[e.RowIndex].trait_type is "kTraitColor")
                    return;
                else if ((Properties.Settings.Default.LeafOptionThinBars && SequencerObjects[e.RowIndex].friendly_lane == "lane center" && SequencerObjects[e.RowIndex].expandlanes == false))
                    return;
                else if (Properties.Settings.Default.LeafOptionConnectBars && e.ColumnIndex >= FrozenColumnOffset && e.Value.ToString() == trackEditor[e.ColumnIndex - 1, e.RowIndex].Value?.ToString())
                    return;

                Color _c = SequencerObjects[e.RowIndex].highlight_color;
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
    }
}
