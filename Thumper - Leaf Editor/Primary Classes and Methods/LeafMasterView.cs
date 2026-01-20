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
        public static int Width = 40;
        public static int Height = 20;
        public static int Gap = 3;
        public static int Middle;
        //
        public static List<Sequencer_Object> Lanes = new();
        //Lane colors
        public static SolidBrush BrushLane = new(Color.FromArgb(27, 19, 27));
        public static Color ColorDefaultRail = Color.FromArgb(147, 255, 80);
        public static Pen PenRailDefault = new(ColorDefaultRail, 2);
        public static SolidBrush LaneOuter = new(Color.FromArgb(148, 184, 202));
        public static SolidBrush TrackOuter = new(Color.FromArgb(48, 48, 48));
        //Thump colors
        public static SolidBrush BrushThumpInner = new(Color.White);
        public static SolidBrush BrushThumpOutter = new(Color.FromArgb(148, 129, 239));
        //
        public static List<Pen> PenRailColors;

        public static void MasterViewBegin(Graphics g, List<Sequencer_Object> SequencerObjects, LeafProperties Leaf, int CellWidth, int CellHeight)
        {
            if (Leaf.ParentEditor.EditorIsProcessing)
                return;
            //clear the screen
            g.Clear(Color.Black);
            //initialize variables needed
            //Width = CellWidth;
            //Height = CellHeight;
            Middle = (int)(g.VisibleClipBounds.Height / 2) - (CellHeight / 2);
            GetRailColors(SequencerObjects, Leaf);
            GetLanes(SequencerObjects);
            //Begin drawing
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TranslateTransform(-1 * (Width * Form_LeafEditor.FrozenColumnOffset), 0);
            DrawLanes(g, SequencerObjects, Leaf);
            DrawThumps(g, SequencerObjects, Leaf);
        }

        public static void GetRailColors(List<Sequencer_Object> SequencerObjects, LeafProperties Leaf)
        {
            PenRailColors = new();
            if (SequencerObjects.FirstOrDefault(x => x.friendly_param == "rail_color") is Sequencer_Object seq) {
                for (int beat = 0; beat < Leaf.beats; beat++) {
                    PenRailColors.Add(new(new SolidBrush(Color.FromArgb((int)seq[beat].InGameValue)), 2));
                }
            }
            else {
                for (int beat = 0; beat < Leaf.beats; beat++) {
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
            for (int beat = 0; beat < Leaf.beats; beat++) {
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
                DrawRailNormal(g, beat, offset);
            }

            else if (seq.friendly_param == "lane left 2") {
                check = false;
                if (seq[beat].InGameValue == 1 && seq[beat - 1]?.InGameValue == 0 && Lanes[1]?[beat].InGameValue == 1){
                    DrawRailOpenRight(g, beat, offset);
                    check = true;
                }
                if (seq[beat].InGameValue == 1 && seq[beat + 1]?.InGameValue == 0 && Lanes[1]?[beat].InGameValue == 1){
                    DrawRailCloseRight(g, beat, offset);
                    check = true;
                }
                if (!check)
                    DrawRailNormal(g, beat, offset);
            }
            else if (seq.friendly_param == "lane right 2") {
                check = false;
                if (seq[beat].InGameValue == 1 && seq[beat - 1]?.InGameValue == 0 && Lanes[3]?[beat].InGameValue == 1){
                    DrawRailOpenLeft(g, beat, offset);
                    check = true;
                }
                if (seq[beat].InGameValue == 1 && seq[beat + 1]?.InGameValue == 0 && Lanes[3]?[beat].InGameValue == 1){
                    DrawRailCloseLeft(g, beat, offset);
                    check = true;
                }
                if (!check)
                    DrawRailNormal(g, beat, offset);
            }
            else {
                check = false;
                if (seq[beat].InGameValue == 1 && seq[beat - 1]?.InGameValue == 0) {
                    if (Lanes[Lanes.IndexOf(seq) - 1]?[beat].InGameValue == 1){
                        DrawRailOpenLeft(g, beat, offset);
                        check = true;
                    }
                    if (Lanes[Lanes.IndexOf(seq) + 1]?[beat].InGameValue == 1){
                        DrawRailOpenRight(g, beat, offset);
                        check = true;
                    }
                }
                if (seq[beat].InGameValue == 1 && seq[beat + 1]?.InGameValue == 0) {
                    if (Lanes[Lanes.IndexOf(seq) - 1]?[beat].InGameValue == 1){
                        DrawRailCloseLeft(g, beat, offset);
                        check = true;
                    }
                    if (Lanes[Lanes.IndexOf(seq) + 1]?[beat].InGameValue == 1){
                        DrawRailCloseRight(g, beat, offset);
                        check = true;
                    }
                }
                if (!check)
                    DrawRailNormal(g, beat, offset);
            }
        }
        public static void DrawRailNormal(Graphics g, int beat, int offset)
        {
            g.FillRectangle(BrushLane, new Rectangle(beat * Width, offset, Width, Height));
            g.DrawLine(PenRailColors[beat], beat * Width, offset, (beat * Width) + Width, offset);
            g.DrawLine(PenRailColors[beat], beat * Width, offset + Height, (beat * Width) + Width, offset + Height);

            LinearGradientBrush lgb = new(new Rectangle(beat * Width, offset, Width, Height), Color.Black, Color.Black, 90);
            ColorBlend cblend = new(3) {
                Colors = new Color[3] { Color.FromArgb(40, PenRailColors[beat].Color), Color.Transparent, Color.FromArgb(40, PenRailColors[beat].Color) },
                Positions = new float[3] { 0f, 0.5f, 1f }
            };
            lgb.InterpolationColors = cblend;

            g.FillRectangle(lgb, new Rectangle(beat * Width, offset, Width, Height));
        }
        public static void DrawRailOpenLeft(Graphics g, int beat, int offset)
        {
            Point p1 = new(beat * Width, offset);
            Point p2 = new((beat * Width) + Width, offset);
            Point p3 = new((beat * Width) + Width, offset + Height);
            Point p4 = new((beat * Width) + (Width / 2), offset + Height);
            g.FillPolygon(BrushLane, new[] { p1, p2, p3, p4 });

            g.DrawLine(PenRailColors[beat], beat * Width, offset, beat * Width + (Width / 2), offset + Height);
            g.DrawLine(PenRailColors[beat], (beat * Width) + (Width / 2), offset + Height, (beat * Width) + Width, offset + Height);
            g.DrawLine(PenRailColors[beat], beat * Width, offset, (beat * Width) + Width, offset);

            DrawRailGlow(g, beat, offset, new[] { p1, p2, p3, p4 });
        }
        public static void DrawRailOpenRight(Graphics g, int beat, int offset)
        {
            Point p1 = new(beat * Width, offset + Height);
            Point p2 = new((beat * Width) + (Width / 2), offset);
            Point p3 = new((beat * Width) + Width, offset);
            Point p4 = new((beat * Width) + Width, offset + Height);
            g.FillPolygon(BrushLane, new[] { p1, p2, p3, p4 });

            g.DrawLine(PenRailColors[beat], beat * Width, offset + Height, beat * Width + (Width / 2), offset);
            g.DrawLine(PenRailColors[beat], (beat * Width) + (Width / 2), offset, (beat * Width) + Width, offset);
            g.DrawLine(PenRailColors[beat], beat * Width, offset + Height, (beat * Width) + Width, offset + Height);

            DrawRailGlow(g, beat, offset, new[] { p1, p2, p3, p4 });
        }
        public static void DrawRailCloseLeft(Graphics g, int beat, int offset)
        {
            Point p1 = new(beat * Width, offset);
            Point p2 = new((beat * Width) + Width, offset);
            Point p3 = new((beat * Width) + (Width / 2), offset + Height);
            Point p4 = new((beat * Width), offset + Height);
            g.FillPolygon(BrushLane, new[] { p1, p2, p3, p4 });

            g.DrawLine(PenRailColors[beat], beat * Width + (Width / 2), offset + Height, (beat * Width) + Width, offset);
            g.DrawLine(PenRailColors[beat], beat * Width, offset, (beat * Width) + Width, offset);
            g.DrawLine(PenRailColors[beat], beat * Width, offset + Height, (beat * Width) + (Width / 2), offset + Height);

            DrawRailGlow(g, beat, offset, new[] { p1, p2, p3, p4 });
        }
        public static void DrawRailCloseRight(Graphics g, int beat, int offset)
        {
            Point p1 = new(beat * Width, offset);
            Point p2 = new((beat * Width) + (Width / 2), offset);
            Point p3 = new((beat * Width) + Width, offset + Height);
            Point p4 = new((beat * Width), offset + Height);
            g.FillPolygon(BrushLane, new[] { p1, p2, p3, p4 });

            g.DrawLine(PenRailColors[beat], beat * Width + (Width / 2), offset, (beat * Width) + Width, offset + Height);
            g.DrawLine(PenRailColors[beat], beat * Width, offset, (beat * Width) + (Width / 2), offset);
            g.DrawLine(PenRailColors[beat], beat * Width, offset + Height, (beat * Width) + Width, offset + Height);

            DrawRailGlow(g, beat, offset, new[] { p1, p2, p3, p4 });
        }
        public static void DrawRailGlow(Graphics g, int beat, int offset, Point[] points)
        {
            LinearGradientBrush lgb = new(new Rectangle(beat * Width, offset, Width, Height), Color.Black, Color.Black, 90);
            ColorBlend cblend = new(3) {
                Colors = new Color[3] { Color.FromArgb(50, PenRailColors[beat].Color), Color.Transparent, Color.FromArgb(50, PenRailColors[beat].Color) },
                Positions = new float[3] { 0f, 0.5f, 1f }
            };
            lgb.InterpolationColors = cblend;

            g.FillPolygon(lgb, points);
        }

        public static void DrawThumps(Graphics g, List<Sequencer_Object> SequencerObjects, LeafProperties Leaf)
        {
            if (SequencerObjects.FirstOrDefault(x => x.param_path == "thump_rails.ent") is Sequencer_Object seq) {
                for (int beat = 0; beat < Leaf.beats; beat++) {
                    if (seq[beat].InGameValue == 1) {
                        DrawThumpIcon(g, beat, Middle);
                    }
                }
            }
            if (SequencerObjects.FirstOrDefault(x => x.param_path == "thump_rails.a01") is Sequencer_Object seq2) {
                for (int beat = 0; beat < Leaf.beats; beat++) {
                    if (seq2[beat].InGameValue == 1) {
                        DrawThumpIcon(g, beat, Middle - Height*2 - Gap*2);
                    }
                }
            }
            if (SequencerObjects.FirstOrDefault(x => x.param_path == "thump_rails.a02") is Sequencer_Object seq3) {
                for (int beat = 0; beat < Leaf.beats; beat++) {
                    if (seq3[beat].InGameValue == 1) {
                        DrawThumpIcon(g, beat, Middle - Height - Gap);
                    }
                }
            }
            if (SequencerObjects.FirstOrDefault(x => x.param_path == "thump_rails.z01") is Sequencer_Object seq4) {
                for (int beat = 0; beat < Leaf.beats; beat++) {
                    if (seq4[beat].InGameValue == 1) {
                        DrawThumpIcon(g, beat, Middle + Height + Gap);
                    }
                }
            }
            if (SequencerObjects.FirstOrDefault(x => x.param_path == "thump_rails.z02") is Sequencer_Object seq5) {
                for (int beat = 0; beat < Leaf.beats; beat++) {
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
