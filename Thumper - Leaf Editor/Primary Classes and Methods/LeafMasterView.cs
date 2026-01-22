using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thumper_Custom_Level_Editor.Editor_Panels;

namespace Thumper_Custom_Level_Editor.Primary_Classes_and_Methods
{
    ///you're welcome
    ///Berry owes me a fursona
    ///

    public static class LeafMasterView
    {
        public static Bitmap LeafDrawing;
        //
        public static int Width = 40;
        public static int Height = 20;
        public static int Gap = 1;
        public static int Middle;
        //
        public static List<Sequencer_Object> Lanes = new();
        //Lane colors
        //public static SolidBrush BrushLane = new(Color.Red);
        public static SolidBrush BrushLane = new(Color.FromArgb(27, 19, 27));
        public static Color ColorDefaultRail = Color.FromArgb(147, 255, 80);
        public static Pen PenRailDefault = new(ColorDefaultRail, 2);
        public static SolidBrush LaneOuter = new(Color.FromArgb(148, 184, 202));
        public static Pen PenLaneOuter = new(LaneOuter, 1);
        public static SolidBrush TrackOuter = new(Color.FromArgb(49, 63, 86));
        public static Pen PenTrackOuter = new(TrackOuter, 3);
        //Thump colors
        public static SolidBrush BrushThumpInner = new(Color.White);
        public static SolidBrush BrushThumpOutter = new(Color.FromArgb(148, 129, 239));
        //
        public static List<Pen> PenRailColors;

        public static void MasterViewBegin(PictureBox pic, List<Sequencer_Object> SequencerObjects, LeafProperties Leaf, int CellWidth, int CellHeight)
        {
            if (Leaf == null || Leaf.ParentEditor.EditorIsProcessing)
                return;
            pic.Size = new(Width * Leaf.beats, (Height + Gap) * 7);
            LeafDrawing = new(pic.Width, pic.Height);
            Middle = (pic.Height / 2) - (CellHeight / 2);
            using (Graphics g = Graphics.FromImage(LeafDrawing)) {
                //initialize variables needed
                GetRailColors(SequencerObjects, Leaf);
                GetLanes(SequencerObjects);
                //Begin drawing
                g.TranslateTransform(-1 * (Width * Form_LeafEditor.FrozenColumnOffset), 0);
                DrawLanes(g, SequencerObjects, Leaf);
                DrawThumps(g, SequencerObjects, Leaf);
            }

            pic.Image = LeafDrawing;
            pic.Location = new(0, (pic.Parent.Height / 2) - (pic.Height / 2));
            //return LeafDrawing;
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
            for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {

            }

            if (SequencerObjects.FirstOrDefault(x => x.param_path is "thump_rails.ent" or "thump_boss_bonus.ent" or "thump_checkpoint.ent" or "thump_rails_fast_activat.ent") is Sequencer_Object seq) {
                for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                    if (seq[beat].InGameValue == 1) {
                        DrawThumpIcon(g, beat, Middle);
                    }
                }
            }
            if (SequencerObjects.FirstOrDefault(x => x.param_path is "thump_rails.a01" or "thump_boss_bonus.a01" or "thump_checkpoint.a01" or "thump_rails_fast_activat.a01") is Sequencer_Object seq2) {
                for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                    if (seq2[beat].InGameValue == 1) {
                        DrawThumpIcon(g, beat, Middle - Height*2 - Gap*2);
                    }
                }
            }
            if (SequencerObjects.FirstOrDefault(x => x.param_path is "thump_rails.a02" or "thump_boss_bonus.a02" or "thump_checkpoint.a02" or "thump_rails_fast_activat.a02") is Sequencer_Object seq3) {
                for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                    if (seq3[beat].InGameValue == 1) {
                        DrawThumpIcon(g, beat, Middle - Height - Gap);
                    }
                }
            }
            if (SequencerObjects.FirstOrDefault(x => x.param_path is "thump_rails.z01" or "thump_boss_bonus.z01" or "thump_checkpoint.z01" or "thump_rails_fast_activat.z01") is Sequencer_Object seq4) {
                for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                    if (seq4[beat].InGameValue == 1) {
                        DrawThumpIcon(g, beat, Middle + Height + Gap);
                    }
                }
            }
            if (SequencerObjects.FirstOrDefault(x => x.param_path is "thump_rails.z02" or "thump_boss_bonus.z02" or "thump_checkpoint.z02" or "thump_rails_fast_activat.z02") is Sequencer_Object seq5) {
                for (int beat = 0; beat < Leaf.BeatsAndFrozen; beat++) {
                    if (seq5[beat].InGameValue == 1) {
                        DrawThumpIcon(g, beat, Middle + Height*2 + Gap*2);
                    }
                }
            }
        }
        public static void DrawThumpIcon(Graphics g, int beat, int offset)
        {
            g.FillRectangle(BrushThumpOutter, new Rectangle((beat * Width) + (Width / 3), offset + 2, Width - ((Width / 3)*2), Height - 4));
            g.FillRectangle(BrushThumpInner, new Rectangle((beat * Width) + (Width / 3) + 2, offset + 4, Width - ((Width / 3)*2) - 4, Height - 8));
        }
    }
}
