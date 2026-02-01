using System.Drawing.Drawing2D;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Windows.Networking.NetworkOperators;

namespace Thumper_Custom_Level_Editor.Primary_Classes_and_Methods
{
    ///you're welcome
    ///Berry owes me a fursona
    ///
    public class DrawData
    {
        public int x;
        public int y;
        public DrawData()
        {

        }
    }

    public static class LeafMasterView
    {
        #region Variables
        public static Bitmap LayerTrack = new(1,1);
        public static Bitmap Master = new(1, 1);
        //
        public static int Width = 40;
        public static int Height = 20;
        public static int Gap = 1;
        public static int Middle;
        public static Dictionary<string, int> OffsetsDict;
        //
        public static List<Sequencer_Object> Lanes = new();
        //Lane Colors
        public static SolidBrush BrushLane = new(Color.FromArgb(27, 19, 27));
        public static Color ColorDefaultRail = Color.FromArgb(147, 255, 80);
        public static Pen PenRailDefault = new(ColorDefaultRail, 2);
        public static SolidBrush LaneOuter = new(Color.FromArgb(148, 184, 202));
        public static Pen PenLaneOuter = new(LaneOuter, 1);
        public static SolidBrush TrackOuter = new(Color.FromArgb(49, 63, 86));
        public static Pen PenTrackOuter = new(TrackOuter, 3);
        //Thump Colors
        public static SolidBrush BrushThumpInner = new(Color.White);
        public static SolidBrush BrushThumpOutter = new(Color.FromArgb(145, 205, 242));
        //Bar Colors
        public static Pen PenBarsPoint = new(new SolidBrush(Color.Orange), 3) { EndCap = LineCap.Triangle, StartCap = LineCap.Triangle };
        public static Pen PenBarsFlat = new(new SolidBrush(Color.Orange), 3);
        //Ring Colors
        public static Pen PenRings = new(new SolidBrush(Color.SkyBlue), 3);
        //
        public static List<Pen> PenRailColors;
        #endregion
        #region Functions
        public static void InitializeAndResize(List<Sequencer_Object> SequencerObjects, LeafProperties Leaf)
        {
            if (Leaf == null || Leaf.ParentEditor.EditorIsProcessing || TCLE.IsClosing)
                return;
            //set size of picture to draw
            Size pic = new(Width * Leaf.beats, (Height * 5) + (Gap * 4) + 4);
            LayerTrack = new(pic.Width, pic.Height);
            Master = new(pic.Width, pic.Height);
            //initialize variables needed
            Middle = (pic.Height / 2) - (Height / 2);
            OffsetsDict = new() { { "a01", -1 * ((Height * 2) + (Gap * 2)) }, { "a02", -1 * (Height + Gap) }, { "ent", 0 }, { "z01", Height + Gap }, { "z02", ((Height * 2) + (Gap * 2)) } };

            Leaf.ParentEditor.dgvMasterView.ColumnCount = Leaf.beats;
            foreach (DataGridViewColumn dgvc in Leaf.ParentEditor.dgvMasterView.Columns)
                dgvc.Width = Width;
            Leaf.ParentEditor.dgvMasterView.RowCount = 5;

            DrawTrack(SequencerObjects, Leaf, false);
            DrawMaster(Leaf);
        }
        public static void DrawTrack(List<Sequencer_Object> SequencerObjects, LeafProperties Leaf, bool drawmaster = true)
        {
            GetRailColors(SequencerObjects, Leaf);
            GetLanes(SequencerObjects);
            //draw
            using (Graphics g = Graphics.FromImage(LayerTrack)) {
                g.Clear(Color.Transparent);
                g.TranslateTransform(-1 * (Width * Form_LeafEditor.FrozenColumnOffset), 0);
                DrawLanes(g, SequencerObjects, Leaf);
                DrawThumps(g, SequencerObjects, Leaf);
                DrawSpikes(g, SequencerObjects, Leaf);
                DrawBars(g, SequencerObjects, Leaf);
                DrawRings(g, SequencerObjects, Leaf);
                DrawTurns(g, SequencerObjects, Leaf);
                g.ResetTransform();                
            }
            if (drawmaster)
                DrawMaster(Leaf);
        }
        public static void DrawMaster(LeafProperties Leaf)
        {
            using (Graphics g = Graphics.FromImage(Master)) {
                g.Clear(Color.Black);
                g.DrawImage(LayerTrack, 0, 0);
            }
            Leaf.ParentEditor.dgvMasterView.Invalidate();
        }

        public static void GetRailColors(List<Sequencer_Object> SequencerObjects, LeafProperties Leaf)
        {
            PenRailColors = new();
            if (SequencerObjects.FirstOrDefault(x => x.friendly_param == "rail_color") is Sequencer_Object seq) {
                for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                    PenRailColors.Add(new(new SolidBrush(Color.FromArgb((int)seq[beat].InGameValue)), 2));
                }
            }
            else {
                for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                    PenRailColors.Add(new(new SolidBrush(Color.FromArgb(147, 255, 80)), 2));
                }
            }
        }
        public static void GetLanes(List<Sequencer_Object> SequencerObjects)
        {
            Lanes = new() { 
                SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane left 2"), 
                SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane left 1"), 
                SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane center"), 
                SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane right 1"), 
                SequencerObjects.FirstOrDefault(x => x.friendly_param == "lane right 2") 
            };
        }

        public static void DrawLanes(Graphics g, List<Sequencer_Object> SequencerObjects, LeafProperties Leaf)
        {
            for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                if (Lanes[2] == null || Lanes[2][beat].InGameValue == 1) {
                    DrawRails(g, Lanes[2], SequencerObjects, beat, Middle);
                }
                if (Lanes[1]?[beat].InGameValue == 1) {
                    DrawRails(g, Lanes[1], SequencerObjects, beat, Middle - Height - Gap);
                }
                if (Lanes[0]?[beat].InGameValue == 1) {
                    DrawRails(g, Lanes[0], SequencerObjects, beat, Middle - Height*2 - Gap*2);
                }
                if (Lanes[3]?[beat].InGameValue == 1) {
                    DrawRails(g, Lanes[3], SequencerObjects, beat, Middle + Height + Gap);
                }
                if (Lanes[4]?[beat].InGameValue == 1) {
                    DrawRails(g, Lanes[4], SequencerObjects, beat, Middle + Height*2 + Gap*2);
                }
            }
        }
        public static void DrawRails(Graphics g, Sequencer_Object seq, List<Sequencer_Object> SequencerObjects, int beat, int offset)
        {
            bool check = false;
            if (seq == null) {
                DrawRailNormal(g, beat, offset, seq);
            }

            else if (seq.friendly_param == "lane left 2") {
                check = false;
                if (seq[beat].InGameValue == 1 && seq[beat - 1]?.InGameValue == 0 && Lanes[1]?[beat].InGameValue == 1){
                    DrawRailEnds(g, beat, offset, true, false, seq);
                    check = true;
                }
                if (seq[beat].InGameValue == 1 && seq[beat + 1]?.InGameValue == 0 && Lanes[1]?[beat].InGameValue == 1){
                    DrawRailEnds(g, beat, offset, true, true, seq);
                    check = true;
                }
                if (!check)
                    DrawRailNormal(g, beat, offset, seq);
            }
            else if (seq.friendly_param == "lane right 2") {
                check = false;
                if (seq[beat].InGameValue == 1 && seq[beat - 1]?.InGameValue == 0 && Lanes[3]?[beat].InGameValue == 1) {
                    DrawRailEnds(g, beat, offset, false, false, seq);
                    check = true;
                }
                if (seq[beat].InGameValue == 1 && seq[beat + 1]?.InGameValue == 0 && Lanes[3]?[beat].InGameValue == 1) {
                    DrawRailEnds(g, beat, offset, false, true, seq);
                    check = true;
                }
                if (!check)
                    DrawRailNormal(g, beat, offset, seq);
            }
            else {
                check = false;
                if (seq[beat].InGameValue == 1 && seq[beat - 1]?.InGameValue == 0) {
                    if (Lanes[Lanes.IndexOf(seq) - 1]?[beat].InGameValue == 1 || (seq.friendly_param == "lane right 1" && Lanes[2] == null)) {
                        DrawRailEnds(g, beat, offset, false, false, seq);
                        check = true;
                    }
                    if (Lanes[Lanes.IndexOf(seq) + 1]?[beat].InGameValue == 1 || (seq.friendly_param == "lane left 1" && Lanes[2] == null)) {
                        DrawRailEnds(g, beat, offset, true, false, seq);
                        check = true;
                    }
                }
                if (seq[beat].InGameValue == 1 && seq[beat + 1]?.InGameValue == 0) {
                    if (Lanes[Lanes.IndexOf(seq) - 1]?[beat].InGameValue == 1 || (seq.friendly_param == "lane right 1" && Lanes[2] == null)) {
                        DrawRailEnds(g, beat, offset, false, true, seq);
                        check = true;
                    }
                    if (Lanes[Lanes.IndexOf(seq) + 1]?[beat].InGameValue == 1 || (seq.friendly_param == "lane left 1" && Lanes[2] == null)) {
                        DrawRailEnds(g, beat, offset, true, true, seq);
                        check = true;
                    }
                }
                if (!check)
                    DrawRailNormal(g, beat, offset, seq);
            }
        }
        public static void DrawLaneBorder(Graphics g, Point p1, Point p2)
        {
            //g.DrawLine(PenTrackOuter, p1, p2);
        }
        public static void DrawRailNormal(Graphics g, int beat, int offset, Sequencer_Object seq)
        {
            if (seq != null && (seq.friendly_param == "lane left 2" || Lanes[Lanes.IndexOf(seq) - 1]?[beat].InGameValue == 0))
                DrawLaneBorder(g, new(beat * Width, offset - 4), new((beat * Width) + Width, offset - 4));
            if (seq != null && (seq.friendly_param == "lane right 2" || Lanes[Lanes.IndexOf(seq) + 1]?[beat].InGameValue == 0))
                DrawLaneBorder(g, new(beat * Width, offset + Height + 3), new((beat * Width) + Width, offset + Height + 3));

            g.FillRectangle(BrushLane, new Rectangle(beat * Width, offset, Width, Height));
            g.FillRectangle(PenRailColors[beat].Brush, beat * Width, offset, Width, 2);
            g.FillRectangle(PenRailColors[beat].Brush, beat * Width, offset + Height - 2, Width, 2);
            //
            DrawRailGlow(g, beat, offset, null, true);
            //
            //g.DrawLine(PenLaneOuter, beat * Width, offset - 2, (beat * Width) + Width, offset - 2);
            //g.DrawLine(PenLaneOuter, beat * Width, offset + Height + 1, (beat * Width) + Width, offset + Height + 1);
        }
        public static void DrawRailEnds(Graphics g, int beat, int offset, bool right, bool close, Sequencer_Object seq)
        {
            //bottom right
            Point p1 = new(beat * Width + Width - (!right && close ? Width / 2 : -2), offset + Height - 1);
            //bottom left
            Point p2 = new(beat * Width + (!right && !close ? Width / 2 : 0), offset + Height - 1);
            //top left
            Point p3 = new(beat * Width + (right && !close ? Width / 2 : 0), offset + 1);
            //top right
            Point p4 = new(beat * Width + Width - (right && close ? Width / 2 : -2), offset + 1);
            g.FillPolygon(BrushLane, new[] { p1, p2, p3, p4 });

            if (close) {
                //draw rail color
                g.DrawLine(PenRailColors[beat], p1, p2);
                g.DrawLine(PenRailColors[beat], p3, p4);
                g.DrawLine(PenRailColors[beat], p1, p4);
                //draw lane border
                //g.DrawLine(PenLaneOuter, new(p1.X + 2, p1.Y - 2), new(p4.X + 2, p4.Y - 2));
                //g.DrawLine(PenLaneOuter, new(p4.X + 2, p4.Y - 2), new(p3.X + 2, p3.Y - 2));
            }
            else {
                g.DrawLine(PenRailColors[beat], p1, p2);
                g.DrawLine(PenRailColors[beat], p3, p4);
                g.DrawLine(PenRailColors[beat], p2, p3);
            }
            DrawRailGlow(g, beat, offset, new[] { p1, p2, p3, p4 });
        }        
        public static void DrawRailGlow(Graphics g, int beat, int offset, Point[] points, bool normal = false)
        {
            LinearGradientBrush lgb = new(new Rectangle(beat * Width, offset, Width, Height), Color.Black, Color.Black, 90);
            ColorBlend cblend = new(3) {
                Colors = new Color[3] { Color.FromArgb(50, PenRailColors[beat].Color), Color.Transparent, Color.FromArgb(50, PenRailColors[beat].Color) },
                Positions = new float[3] { 0f, 0.5f, 1f }
            };
            lgb.InterpolationColors = cblend;

            if (normal)
                g.FillRectangle(lgb, new Rectangle(beat * Width, offset, Width, Height));
            else
                g.FillPolygon(lgb, points);
        }

        public static void DrawThumps(Graphics g, List<Sequencer_Object> SequencerObjects, LeafProperties Leaf)
        {
            foreach (Sequencer_Object seq in SequencerObjects.Where(x => x.param_path.StartsWith("thump_rails") || x.param_path.StartsWith("thump_boss_bonus") || x.param_path.StartsWith("thump_checkpoint") || x.param_path.StartsWith("thump_rails_fast_activat") || x.param_path.StartsWith("grindable_with_thump")))
            {
                for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                    if (seq[beat].InGameValue == 1) {
                        DrawThumpIcon(g, beat, Middle + OffsetsDict[seq.param_path_lane], seq.param_path.StartsWith("thump_boss_bonus"));
                        if (seq.param_path.StartsWith("grindable_with_thump"))
                            DrawBarIcons(g, beat, (beat * Width) + Width, Middle + OffsetsDict[seq.param_path_lane] - 6, Middle + OffsetsDict[seq.param_path_lane] + Height + 5);
                    }
                }
            }
        }
        public static void DrawThumpIcon(Graphics g, int beat, int offset, bool boss)
        {
            g.FillRectangle(boss ? Brushes.Green : BrushThumpOutter, new Rectangle((beat * Width) + (Width / 3), offset + 2, Width - ((Width / 3)*2), Height - 4));
            g.FillRectangle(BrushThumpInner, new Rectangle((beat * Width) + (Width / 3) + 4, offset + 6, Width - ((Width / 3)*2) - 8, Height - 12));
        }

        public static void DrawSpikes(Graphics g, List<Sequencer_Object> SequencerObjects, LeafProperties Leaf)
        {
            foreach (Sequencer_Object seq in SequencerObjects.Where(x => x.category == "JUMPS/SPIKES")) {
                for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                    if (seq[beat].InGameValue == 1) {
                        bool success = int.TryParse(seq.friendly_param.Split('[')[1].Split(' ')[0], out int beats);
                        DrawSpikeIcons(g, beat, Middle + OffsetsDict[seq.param_path_lane], beats);
                    }
                }
            }
        }
        public static void DrawSpikeIcons(Graphics g, int beat, int offset, int length)
        {
            for (int drawwidth = 0; drawwidth < (Width * length) - 4; drawwidth += 16) {
                g.DrawImage(Properties.Resources.SPIKES, beat * Width + drawwidth, offset);
            }
        }

        public static void DrawBars(Graphics g, List<Sequencer_Object> SequencerObjects, LeafProperties Leaf)
        {
            foreach (Sequencer_Object seq in SequencerObjects.Where(x => x.param_path.StartsWith("grindable_still"))) {
                for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                    if (seq[beat].InGameValue == 1) {
                        DrawBarIcons(g, beat, (beat * Width) + (Width / 2), Middle + OffsetsDict[seq.param_path_lane] - 6, Middle + OffsetsDict[seq.param_path_lane] + Height + 5);
                    }
                }
            }
            foreach (Sequencer_Object seq in SequencerObjects.Where(x => x.param_path.StartsWith("center_multi"))) {
                for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                    if (seq[beat].InGameValue == 1) {
                        DrawBarIcons(g, beat, (beat * Width) + (Width / 2), Middle + OffsetsDict[seq.param_path_lane], Middle + OffsetsDict[seq.param_path_lane] + Height);
                    }
                }
            }
            foreach (Sequencer_Object seq in SequencerObjects.Where(x => x.param_path.StartsWith("left_multi"))) {
                for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                    if (seq[beat].InGameValue == 1) {
                        DrawBarIcons(g, beat, (beat * Width) + (Width / 2), Middle + OffsetsDict[seq.param_path_lane] - 6, Middle + OffsetsDict[seq.param_path_lane] + (Height / 2));
                    }
                }
            }
            foreach (Sequencer_Object seq in SequencerObjects.Where(x => x.param_path.StartsWith("right_multi"))) {
                for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                    if (seq[beat].InGameValue == 1) {
                        DrawBarIcons(g, beat, (beat * Width) + (Width / 2), Middle + OffsetsDict[seq.param_path_lane] + (Height / 2), Middle + OffsetsDict[seq.param_path_lane] + Height + 5);
                    }
                }
            }
            foreach (Sequencer_Object seq in SequencerObjects.Where(x => x.param_path.StartsWith("grindable_double"))) {
                for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                    if (seq[beat].InGameValue == 1) {
                        DrawBarIcons(g, beat, (beat * Width) + (Width / 2), Middle + OffsetsDict[seq.param_path_lane] - 6, Middle + OffsetsDict[seq.param_path_lane] + Height + 5);
                        DrawBarIcons(g, beat, (beat * Width) + Width, Middle + OffsetsDict[seq.param_path_lane] - 6, Middle + OffsetsDict[seq.param_path_lane] + Height + 5);
                    }
                }
            }
            foreach (Sequencer_Object seq in SequencerObjects.Where(x => x.param_path.StartsWith("grindable_thirds"))) {
                for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                    if (seq[beat].InGameValue == 1) {
                        DrawBarIcons(g, beat, (beat * Width) + (Width / 2), Middle + OffsetsDict[seq.param_path_lane] - 6, Middle + OffsetsDict[seq.param_path_lane] + Height + 5);
                        DrawBarIcons(g, beat, (beat * Width) + (Width / 2) + (Width / 3), Middle + OffsetsDict[seq.param_path_lane] - 6, Middle + OffsetsDict[seq.param_path_lane] + Height + 5);
                        DrawBarIcons(g, beat, (beat * Width) + (Width / 2) + (Width / 3 * 2), Middle + OffsetsDict[seq.param_path_lane] - 6, Middle + OffsetsDict[seq.param_path_lane] + Height + 5);
                    }
                }
            }
            foreach (Sequencer_Object seq in SequencerObjects.Where(x => x.param_path.StartsWith("grindable_quarters"))) {
                for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                    if (seq[beat].InGameValue == 1) {
                        DrawBarIcons(g, beat, (beat * Width) + (Width / 2), Middle + OffsetsDict[seq.param_path_lane] - 6, Middle + OffsetsDict[seq.param_path_lane] + Height + 5);
                        DrawBarIcons(g, beat, (beat * Width) + (Width / 2) + (Width / 4), Middle + OffsetsDict[seq.param_path_lane] - 6, Middle + OffsetsDict[seq.param_path_lane] + Height + 5);
                        DrawBarIcons(g, beat, (beat * Width) + (Width / 2) + (Width / 4 * 2), Middle + OffsetsDict[seq.param_path_lane] - 6, Middle + OffsetsDict[seq.param_path_lane] + Height + 5);
                        DrawBarIcons(g, beat, (beat * Width) + (Width / 2) + (Width / 4 * 3), Middle + OffsetsDict[seq.param_path_lane] - 6, Middle + OffsetsDict[seq.param_path_lane] + Height + 5);
                    }
                }
            }
        }
        public static void DrawBarIcons(Graphics g, int beat, int offsetx, int offsety, int offsety2)
        {
            //style 1 - all point //style 2 - top point //style 3 - bottom point //style 4 - no point
            g.DrawLine(PenBarsPoint, offsetx, offsety, offsetx, offsety2);
        }

        public static void DrawRings(Graphics g, List<Sequencer_Object> SequencerObjects, LeafProperties Leaf)
        {
            foreach (Sequencer_Object seq in SequencerObjects.Where(x => x.param_path.StartsWith("ducker_crak"))) {
                for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                    if (seq[beat].InGameValue == 1) {
                        DrawRingIcons(g, beat, Middle + OffsetsDict[seq.param_path_lane]);
                    }
                }
            }
        }
        public static void DrawRingIcons(Graphics g, int beat, int offset)
        {
            g.DrawArc(PenRings, beat*Width + (Width / 2), offset, Width/3, Height, -94, 188);
        }

        public static void DrawTurns(Graphics g, List<Sequencer_Object> SequencerObjects, LeafProperties Leaf)
        {
            foreach (Sequencer_Object seq in SequencerObjects.Where(x => x.param_path == "turn")) {
                for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                    if (seq[beat].InGameValue != 0)
                        DrawTurnWall(g, beat, (decimal)seq[beat].Value);
                }
            }
        }
        public static void DrawTurnWall(Graphics g, int beat, decimal value)
        {
            //turning left
            if (value > 0) {
                g.DrawImage(Properties.Resources.basiceditor_turnl, beat * Width + (Width / 2) - 8, Middle + 2);
                int OffsetBottomLane = 0;
                for (int x = 4; x >= 0; x--) {
                    if (Lanes[x]?[beat]?.InGameValue == 1) {
                        OffsetBottomLane = OffsetsDict.ElementAt(x).Value;
                        break;
                    }
                }
                g.FillRectangle(Brushes.Gray, beat * Width, Middle + OffsetBottomLane + Height, Width, 6);
                g.FillRectangle(Brushes.OrangeRed, (beat * Width) + 2, Middle + OffsetBottomLane + Height, Width - 4, 3);
                g.DrawString(value.ToString() + "°", Form_LeafEditor.TuningFont, Brushes.White, beat * Width + (Width/ 2) - 8, Middle + OffsetBottomLane + Height + 6);
            }
            //turning right
            else {
                g.DrawImage(Properties.Resources.basiceditor_turnr, beat * Width + (Width / 2) - 8, Middle + 2);
                int OffsetTopLane = 0;
                for (int x = 0; x < 5; x++) {
                    if (Lanes[x]?[beat]?.InGameValue == 1) {
                        OffsetTopLane = OffsetsDict.ElementAt(x).Value;
                        break;
                    }
                }
                g.FillRectangle(Brushes.Gray, beat * Width, Middle + OffsetTopLane - 6, Width, 6);
                g.FillRectangle(Brushes.OrangeRed, (beat * Width) + 2, Middle + OffsetTopLane - 3, Width - 4, 3);
                g.DrawString(value.ToString() + "°", Form_LeafEditor.TuningFont, Brushes.White, beat * Width + (Width / 2) - 8, Middle + OffsetTopLane - 16);
            }
        }
        #endregion
    }
}
