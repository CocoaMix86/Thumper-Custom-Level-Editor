namespace Thumper_Custom_Level_Editor.Other_Forms
{
    partial class VolumeMaster
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VolumeMaster));
            this.trackMix1 = new TrackBar();
            this.trackBar1 = new TrackBar();
            this.labelSequencer = new Label();
            this.panel1 = new Panel();
            this.panel2 = new Panel();
            this.trackBar4 = new TrackBar();
            this.trackBar5 = new TrackBar();
            this.trackBar2 = new TrackBar();
            this.trackBar3 = new TrackBar();
            this.label1 = new Label();
            this.panel3 = new Panel();
            this.trackBar6 = new TrackBar();
            this.trackBar7 = new TrackBar();
            this.trackBar8 = new TrackBar();
            this.trackBar9 = new TrackBar();
            this.label2 = new Label();
            this.panel4 = new Panel();
            this.trackBar10 = new TrackBar();
            this.trackBar11 = new TrackBar();
            this.trackBar12 = new TrackBar();
            this.trackBar13 = new TrackBar();
            this.label3 = new Label();
            this.panel5 = new Panel();
            this.trackBar14 = new TrackBar();
            this.trackBar15 = new TrackBar();
            this.label4 = new Label();
            this.panel6 = new Panel();
            this.trackBar16 = new TrackBar();
            this.trackBar18 = new TrackBar();
            this.trackBar19 = new TrackBar();
            this.label5 = new Label();
            this.panel7 = new Panel();
            this.pictureWaveL = new PictureBox();
            this.trackMasterVolume = new TrackBar();
            this.label6 = new Label();
            ((System.ComponentModel.ISupportInitialize)this.trackMix1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar1).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.trackBar4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar3).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.trackBar6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar9).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.trackBar10).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar12).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar13).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.trackBar14).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar15).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.trackBar16).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar18).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar19).BeginInit();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.pictureWaveL).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.trackMasterVolume).BeginInit();
            this.SuspendLayout();
            // 
            // trackMix1
            // 
            this.trackMix1.AutoSize = false;
            this.trackMix1.Cursor = Cursors.Hand;
            this.trackMix1.Location = new Point(11, -2);
            this.trackMix1.Margin = new Padding(5);
            this.trackMix1.Maximum = 120;
            this.trackMix1.Name = "trackMix1";
            this.trackMix1.Orientation = Orientation.Vertical;
            this.trackMix1.Size = new Size(22, 176);
            this.trackMix1.TabIndex = 44;
            this.trackMix1.Tag = "8";
            this.trackMix1.TickFrequency = 10;
            this.trackMix1.TickStyle = TickStyle.TopLeft;
            this.trackMix1.Value = 100;
            this.trackMix1.MouseDown += this.trackMix1_MouseDown;
            this.trackMix1.MouseUp += this.PlayKeyAtVolume;
            // 
            // trackBar1
            // 
            this.trackBar1.AutoSize = false;
            this.trackBar1.Cursor = Cursors.Hand;
            this.trackBar1.Location = new Point(43, -2);
            this.trackBar1.Margin = new Padding(5);
            this.trackBar1.Maximum = 120;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Orientation = Orientation.Vertical;
            this.trackBar1.Size = new Size(22, 176);
            this.trackBar1.TabIndex = 45;
            this.trackBar1.Tag = "18";
            this.trackBar1.TickFrequency = 10;
            this.trackBar1.TickStyle = TickStyle.TopLeft;
            this.trackBar1.Value = 100;
            this.trackBar1.MouseUp += this.PlayKeyAtVolume;
            // 
            // labelSequencer
            // 
            this.labelSequencer.AutoSize = true;
            this.labelSequencer.BackColor = Color.FromArgb(10, 10, 10);
            this.labelSequencer.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelSequencer.ForeColor = Color.White;
            this.labelSequencer.Location = new Point(10, 178);
            this.labelSequencer.Margin = new Padding(4, 0, 4, 0);
            this.labelSequencer.Name = "labelSequencer";
            this.labelSequencer.Size = new Size(59, 13);
            this.labelSequencer.TabIndex = 97;
            this.labelSequencer.Text = "THUMPS";
            this.labelSequencer.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = Color.Black;
            this.panel1.BorderStyle = BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.trackMix1);
            this.panel1.Controls.Add(this.trackBar1);
            this.panel1.Controls.Add(this.labelSequencer);
            this.panel1.Dock = DockStyle.Left;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Margin = new Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(82, 210);
            this.panel1.TabIndex = 99;
            // 
            // panel2
            // 
            this.panel2.BackColor = Color.Black;
            this.panel2.BorderStyle = BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.trackBar4);
            this.panel2.Controls.Add(this.trackBar5);
            this.panel2.Controls.Add(this.trackBar2);
            this.panel2.Controls.Add(this.trackBar3);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = DockStyle.Left;
            this.panel2.Location = new Point(82, 0);
            this.panel2.Margin = new Padding(4, 3, 4, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(148, 210);
            this.panel2.TabIndex = 100;
            // 
            // trackBar4
            // 
            this.trackBar4.AutoSize = false;
            this.trackBar4.Cursor = Cursors.Hand;
            this.trackBar4.Location = new Point(75, -2);
            this.trackBar4.Margin = new Padding(5);
            this.trackBar4.Maximum = 120;
            this.trackBar4.Name = "trackBar4";
            this.trackBar4.Orientation = Orientation.Vertical;
            this.trackBar4.Size = new Size(22, 176);
            this.trackBar4.TabIndex = 98;
            this.trackBar4.Tag = "7";
            this.trackBar4.TickFrequency = 10;
            this.trackBar4.TickStyle = TickStyle.TopLeft;
            this.trackBar4.Value = 100;
            this.trackBar4.MouseUp += this.PlayKeyAtVolume;
            // 
            // trackBar5
            // 
            this.trackBar5.AutoSize = false;
            this.trackBar5.Cursor = Cursors.Hand;
            this.trackBar5.Location = new Point(107, -2);
            this.trackBar5.Margin = new Padding(5);
            this.trackBar5.Maximum = 120;
            this.trackBar5.Name = "trackBar5";
            this.trackBar5.Orientation = Orientation.Vertical;
            this.trackBar5.Size = new Size(22, 176);
            this.trackBar5.TabIndex = 99;
            this.trackBar5.Tag = "20";
            this.trackBar5.TickFrequency = 10;
            this.trackBar5.TickStyle = TickStyle.TopLeft;
            this.trackBar5.Value = 100;
            this.trackBar5.MouseUp += this.PlayKeyAtVolume;
            // 
            // trackBar2
            // 
            this.trackBar2.AutoSize = false;
            this.trackBar2.Cursor = Cursors.Hand;
            this.trackBar2.Location = new Point(11, -2);
            this.trackBar2.Margin = new Padding(5);
            this.trackBar2.Maximum = 120;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Orientation = Orientation.Vertical;
            this.trackBar2.Size = new Size(22, 176);
            this.trackBar2.TabIndex = 44;
            this.trackBar2.Tag = "1";
            this.trackBar2.TickFrequency = 10;
            this.trackBar2.TickStyle = TickStyle.TopLeft;
            this.trackBar2.Value = 100;
            this.trackBar2.MouseUp += this.PlayKeyAtVolume;
            // 
            // trackBar3
            // 
            this.trackBar3.AutoSize = false;
            this.trackBar3.Cursor = Cursors.Hand;
            this.trackBar3.Location = new Point(43, -2);
            this.trackBar3.Margin = new Padding(5);
            this.trackBar3.Maximum = 120;
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Orientation = Orientation.Vertical;
            this.trackBar3.Size = new Size(22, 176);
            this.trackBar3.TabIndex = 45;
            this.trackBar3.Tag = "19";
            this.trackBar3.TickFrequency = 10;
            this.trackBar3.TickStyle = TickStyle.TopLeft;
            this.trackBar3.Value = 100;
            this.trackBar3.MouseUp += this.PlayKeyAtVolume;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = Color.FromArgb(10, 10, 10);
            this.label1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.ForeColor = Color.White;
            this.label1.Location = new Point(33, 178);
            this.label1.Margin = new Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(77, 13);
            this.label1.TabIndex = 97;
            this.label1.Text = "BAR / RING";
            this.label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BackColor = Color.Black;
            this.panel3.BorderStyle = BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.trackBar6);
            this.panel3.Controls.Add(this.trackBar7);
            this.panel3.Controls.Add(this.trackBar8);
            this.panel3.Controls.Add(this.trackBar9);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = DockStyle.Left;
            this.panel3.Location = new Point(230, 0);
            this.panel3.Margin = new Padding(4, 3, 4, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(148, 210);
            this.panel3.TabIndex = 101;
            // 
            // trackBar6
            // 
            this.trackBar6.AutoSize = false;
            this.trackBar6.Cursor = Cursors.Hand;
            this.trackBar6.Location = new Point(75, -2);
            this.trackBar6.Margin = new Padding(5);
            this.trackBar6.Maximum = 120;
            this.trackBar6.Name = "trackBar6";
            this.trackBar6.Orientation = Orientation.Vertical;
            this.trackBar6.Size = new Size(22, 176);
            this.trackBar6.TabIndex = 98;
            this.trackBar6.Tag = "11";
            this.trackBar6.TickFrequency = 10;
            this.trackBar6.TickStyle = TickStyle.TopLeft;
            this.trackBar6.Value = 100;
            this.trackBar6.MouseUp += this.PlayKeyAtVolume;
            // 
            // trackBar7
            // 
            this.trackBar7.AutoSize = false;
            this.trackBar7.Cursor = Cursors.Hand;
            this.trackBar7.Location = new Point(107, -2);
            this.trackBar7.Margin = new Padding(5);
            this.trackBar7.Maximum = 120;
            this.trackBar7.Name = "trackBar7";
            this.trackBar7.Orientation = Orientation.Vertical;
            this.trackBar7.Size = new Size(22, 176);
            this.trackBar7.TabIndex = 99;
            this.trackBar7.Tag = "13";
            this.trackBar7.TickFrequency = 10;
            this.trackBar7.TickStyle = TickStyle.TopLeft;
            this.trackBar7.Value = 100;
            this.trackBar7.MouseUp += this.PlayKeyAtVolume;
            // 
            // trackBar8
            // 
            this.trackBar8.AutoSize = false;
            this.trackBar8.Cursor = Cursors.Hand;
            this.trackBar8.Location = new Point(11, -2);
            this.trackBar8.Margin = new Padding(5);
            this.trackBar8.Maximum = 120;
            this.trackBar8.Name = "trackBar8";
            this.trackBar8.Orientation = Orientation.Vertical;
            this.trackBar8.Size = new Size(22, 176);
            this.trackBar8.TabIndex = 44;
            this.trackBar8.Tag = "10";
            this.trackBar8.TickFrequency = 10;
            this.trackBar8.TickStyle = TickStyle.TopLeft;
            this.trackBar8.Value = 100;
            this.trackBar8.MouseUp += this.PlayKeyAtVolume;
            // 
            // trackBar9
            // 
            this.trackBar9.AutoSize = false;
            this.trackBar9.Cursor = Cursors.Hand;
            this.trackBar9.Location = new Point(43, -2);
            this.trackBar9.Margin = new Padding(5);
            this.trackBar9.Maximum = 120;
            this.trackBar9.Name = "trackBar9";
            this.trackBar9.Orientation = Orientation.Vertical;
            this.trackBar9.Size = new Size(22, 176);
            this.trackBar9.TabIndex = 45;
            this.trackBar9.Tag = "12";
            this.trackBar9.TickFrequency = 10;
            this.trackBar9.TickStyle = TickStyle.TopLeft;
            this.trackBar9.Value = 100;
            this.trackBar9.MouseUp += this.PlayKeyAtVolume;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = Color.FromArgb(10, 10, 10);
            this.label2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.ForeColor = Color.White;
            this.label2.Location = new Point(49, 178);
            this.label2.Margin = new Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(50, 13);
            this.label2.TabIndex = 97;
            this.label2.Text = "TURNS";
            this.label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.BackColor = Color.Black;
            this.panel4.BorderStyle = BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.trackBar10);
            this.panel4.Controls.Add(this.trackBar11);
            this.panel4.Controls.Add(this.trackBar12);
            this.panel4.Controls.Add(this.trackBar13);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = DockStyle.Left;
            this.panel4.Location = new Point(378, 0);
            this.panel4.Margin = new Padding(4, 3, 4, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(148, 210);
            this.panel4.TabIndex = 102;
            // 
            // trackBar10
            // 
            this.trackBar10.AutoSize = false;
            this.trackBar10.Cursor = Cursors.Hand;
            this.trackBar10.Location = new Point(75, -2);
            this.trackBar10.Margin = new Padding(5);
            this.trackBar10.Maximum = 120;
            this.trackBar10.Name = "trackBar10";
            this.trackBar10.Orientation = Orientation.Vertical;
            this.trackBar10.Size = new Size(22, 176);
            this.trackBar10.TabIndex = 98;
            this.trackBar10.Tag = "4";
            this.trackBar10.TickFrequency = 10;
            this.trackBar10.TickStyle = TickStyle.TopLeft;
            this.trackBar10.Value = 100;
            this.trackBar10.MouseUp += this.PlayKeyAtVolume;
            // 
            // trackBar11
            // 
            this.trackBar11.AutoSize = false;
            this.trackBar11.Cursor = Cursors.Hand;
            this.trackBar11.Location = new Point(107, -2);
            this.trackBar11.Margin = new Padding(5);
            this.trackBar11.Maximum = 120;
            this.trackBar11.Name = "trackBar11";
            this.trackBar11.Orientation = Orientation.Vertical;
            this.trackBar11.Size = new Size(22, 176);
            this.trackBar11.TabIndex = 99;
            this.trackBar11.Tag = "5";
            this.trackBar11.TickFrequency = 10;
            this.trackBar11.TickStyle = TickStyle.TopLeft;
            this.trackBar11.Value = 100;
            this.trackBar11.MouseUp += this.PlayKeyAtVolume;
            // 
            // trackBar12
            // 
            this.trackBar12.AutoSize = false;
            this.trackBar12.Cursor = Cursors.Hand;
            this.trackBar12.Location = new Point(11, -2);
            this.trackBar12.Margin = new Padding(5);
            this.trackBar12.Maximum = 120;
            this.trackBar12.Name = "trackBar12";
            this.trackBar12.Orientation = Orientation.Vertical;
            this.trackBar12.Size = new Size(22, 176);
            this.trackBar12.TabIndex = 44;
            this.trackBar12.Tag = "2";
            this.trackBar12.TickFrequency = 10;
            this.trackBar12.TickStyle = TickStyle.TopLeft;
            this.trackBar12.Value = 100;
            this.trackBar12.MouseUp += this.PlayKeyAtVolume;
            // 
            // trackBar13
            // 
            this.trackBar13.AutoSize = false;
            this.trackBar13.Cursor = Cursors.Hand;
            this.trackBar13.Location = new Point(43, -2);
            this.trackBar13.Margin = new Padding(5);
            this.trackBar13.Maximum = 120;
            this.trackBar13.Name = "trackBar13";
            this.trackBar13.Orientation = Orientation.Vertical;
            this.trackBar13.Size = new Size(22, 176);
            this.trackBar13.TabIndex = 45;
            this.trackBar13.Tag = "3";
            this.trackBar13.TickFrequency = 10;
            this.trackBar13.TickStyle = TickStyle.TopLeft;
            this.trackBar13.Value = 100;
            this.trackBar13.MouseUp += this.PlayKeyAtVolume;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = Color.FromArgb(10, 10, 10);
            this.label3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label3.ForeColor = Color.White;
            this.label3.Location = new Point(35, 178);
            this.label3.Margin = new Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new Size(80, 13);
            this.label3.TabIndex = 97;
            this.label3.Text = "MILLIPEDES";
            this.label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.BackColor = Color.Black;
            this.panel5.BorderStyle = BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.trackBar14);
            this.panel5.Controls.Add(this.trackBar15);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Dock = DockStyle.Left;
            this.panel5.Location = new Point(526, 0);
            this.panel5.Margin = new Padding(4, 3, 4, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new Size(82, 210);
            this.panel5.TabIndex = 103;
            // 
            // trackBar14
            // 
            this.trackBar14.AutoSize = false;
            this.trackBar14.Cursor = Cursors.Hand;
            this.trackBar14.Location = new Point(11, -2);
            this.trackBar14.Margin = new Padding(5);
            this.trackBar14.Maximum = 120;
            this.trackBar14.Name = "trackBar14";
            this.trackBar14.Orientation = Orientation.Vertical;
            this.trackBar14.Size = new Size(22, 176);
            this.trackBar14.TabIndex = 44;
            this.trackBar14.Tag = "6";
            this.trackBar14.TickFrequency = 10;
            this.trackBar14.TickStyle = TickStyle.TopLeft;
            this.trackBar14.Value = 100;
            this.trackBar14.MouseUp += this.PlayKeyAtVolume;
            // 
            // trackBar15
            // 
            this.trackBar15.AutoSize = false;
            this.trackBar15.Cursor = Cursors.Hand;
            this.trackBar15.Location = new Point(43, -2);
            this.trackBar15.Margin = new Padding(5);
            this.trackBar15.Maximum = 120;
            this.trackBar15.Name = "trackBar15";
            this.trackBar15.Orientation = Orientation.Vertical;
            this.trackBar15.Size = new Size(22, 176);
            this.trackBar15.TabIndex = 45;
            this.trackBar15.Tag = "17";
            this.trackBar15.TickFrequency = 10;
            this.trackBar15.TickStyle = TickStyle.TopLeft;
            this.trackBar15.Value = 100;
            this.trackBar15.MouseUp += this.PlayKeyAtVolume;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = Color.FromArgb(10, 10, 10);
            this.label4.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label4.ForeColor = Color.White;
            this.label4.Location = new Point(16, 178);
            this.label4.Margin = new Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new Size(48, 13);
            this.label4.TabIndex = 97;
            this.label4.Text = "JUMPS";
            this.label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BackColor = Color.Black;
            this.panel6.BorderStyle = BorderStyle.Fixed3D;
            this.panel6.Controls.Add(this.trackBar16);
            this.panel6.Controls.Add(this.trackBar18);
            this.panel6.Controls.Add(this.trackBar19);
            this.panel6.Controls.Add(this.label5);
            this.panel6.Dock = DockStyle.Left;
            this.panel6.Location = new Point(608, 0);
            this.panel6.Margin = new Padding(4, 3, 4, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new Size(118, 210);
            this.panel6.TabIndex = 104;
            // 
            // trackBar16
            // 
            this.trackBar16.AutoSize = false;
            this.trackBar16.Cursor = Cursors.Hand;
            this.trackBar16.Location = new Point(75, -2);
            this.trackBar16.Margin = new Padding(5);
            this.trackBar16.Maximum = 120;
            this.trackBar16.Name = "trackBar16";
            this.trackBar16.Orientation = Orientation.Vertical;
            this.trackBar16.Size = new Size(22, 176);
            this.trackBar16.TabIndex = 98;
            this.trackBar16.Tag = "16";
            this.trackBar16.TickFrequency = 10;
            this.trackBar16.TickStyle = TickStyle.TopLeft;
            this.trackBar16.Value = 100;
            this.trackBar16.MouseUp += this.PlayKeyAtVolume;
            // 
            // trackBar18
            // 
            this.trackBar18.AutoSize = false;
            this.trackBar18.Cursor = Cursors.Hand;
            this.trackBar18.Location = new Point(11, -2);
            this.trackBar18.Margin = new Padding(5);
            this.trackBar18.Maximum = 120;
            this.trackBar18.Name = "trackBar18";
            this.trackBar18.Orientation = Orientation.Vertical;
            this.trackBar18.Size = new Size(22, 176);
            this.trackBar18.TabIndex = 44;
            this.trackBar18.Tag = "14";
            this.trackBar18.TickFrequency = 10;
            this.trackBar18.TickStyle = TickStyle.TopLeft;
            this.trackBar18.Value = 100;
            this.trackBar18.MouseUp += this.PlayKeyAtVolume;
            // 
            // trackBar19
            // 
            this.trackBar19.AutoSize = false;
            this.trackBar19.Cursor = Cursors.Hand;
            this.trackBar19.Location = new Point(43, -2);
            this.trackBar19.Margin = new Padding(5);
            this.trackBar19.Maximum = 120;
            this.trackBar19.Name = "trackBar19";
            this.trackBar19.Orientation = Orientation.Vertical;
            this.trackBar19.Size = new Size(22, 176);
            this.trackBar19.TabIndex = 45;
            this.trackBar19.Tag = "15";
            this.trackBar19.TickFrequency = 10;
            this.trackBar19.TickStyle = TickStyle.TopLeft;
            this.trackBar19.Value = 100;
            this.trackBar19.MouseUp += this.PlayKeyAtVolume;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = Color.FromArgb(10, 10, 10);
            this.label5.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label5.ForeColor = Color.White;
            this.label5.Location = new Point(31, 178);
            this.label5.Margin = new Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new Size(57, 13);
            this.label5.TabIndex = 97;
            this.label5.Text = "SENTRY";
            this.label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel7
            // 
            this.panel7.BackColor = Color.Black;
            this.panel7.BorderStyle = BorderStyle.Fixed3D;
            this.panel7.Controls.Add(this.pictureWaveL);
            this.panel7.Controls.Add(this.trackMasterVolume);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Dock = DockStyle.Right;
            this.panel7.Location = new Point(734, 0);
            this.panel7.Margin = new Padding(4, 3, 4, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new Size(160, 210);
            this.panel7.TabIndex = 105;
            // 
            // pictureWaveL
            // 
            this.pictureWaveL.BackColor = Color.Black;
            this.pictureWaveL.BackgroundImageLayout = ImageLayout.None;
            this.pictureWaveL.BorderStyle = BorderStyle.FixedSingle;
            this.pictureWaveL.Location = new Point(42, 4);
            this.pictureWaveL.Margin = new Padding(4, 3, 4, 3);
            this.pictureWaveL.Name = "pictureWaveL";
            this.pictureWaveL.Size = new Size(110, 162);
            this.pictureWaveL.TabIndex = 152;
            this.pictureWaveL.TabStop = false;
            // 
            // trackMasterVolume
            // 
            this.trackMasterVolume.AutoSize = false;
            this.trackMasterVolume.Cursor = Cursors.Hand;
            this.trackMasterVolume.Location = new Point(11, -2);
            this.trackMasterVolume.Margin = new Padding(0);
            this.trackMasterVolume.Maximum = 120;
            this.trackMasterVolume.Name = "trackMasterVolume";
            this.trackMasterVolume.Orientation = Orientation.Vertical;
            this.trackMasterVolume.Size = new Size(22, 176);
            this.trackMasterVolume.TabIndex = 44;
            this.trackMasterVolume.TickFrequency = 10;
            this.trackMasterVolume.TickStyle = TickStyle.TopLeft;
            this.trackMasterVolume.Value = 100;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = Color.FromArgb(10, 10, 10);
            this.label6.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label6.ForeColor = Color.White;
            this.label6.Location = new Point(35, 178);
            this.label6.Margin = new Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new Size(58, 13);
            this.label6.TabIndex = 97;
            this.label6.Text = "MASTER";
            this.label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // VolumeMaster
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(31, 31, 31);
            this.ClientSize = new Size(894, 210);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Name = "VolumeMaster";
            this.Text = "Volume Mixer";
            ((System.ComponentModel.ISupportInitialize)this.trackMix1).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar1).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.trackBar4).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar5).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar2).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar3).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.trackBar6).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar7).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar8).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar9).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.trackBar10).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar11).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar12).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar13).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.trackBar14).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar15).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.trackBar16).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar18).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.trackBar19).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.pictureWaveL).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.trackMasterVolume).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private TrackBar trackMix1;
        private TrackBar trackBar1;
        private Label labelSequencer;
        private Panel panel1;
        private Panel panel2;
        private TrackBar trackBar4;
        private TrackBar trackBar5;
        private TrackBar trackBar2;
        private TrackBar trackBar3;
        private Label label1;
        private Panel panel3;
        private TrackBar trackBar6;
        private TrackBar trackBar7;
        private TrackBar trackBar8;
        private TrackBar trackBar9;
        private Label label2;
        private Panel panel4;
        private TrackBar trackBar10;
        private TrackBar trackBar11;
        private TrackBar trackBar12;
        private TrackBar trackBar13;
        private Label label3;
        private Panel panel5;
        private TrackBar trackBar14;
        private TrackBar trackBar15;
        private Label label4;
        private Panel panel6;
        private TrackBar trackBar16;
        private TrackBar trackBar18;
        private TrackBar trackBar19;
        private Label label5;
        private Panel panel7;
        private TrackBar trackMasterVolume;
        private Label label6;
        private PictureBox pictureWaveL;
    }
}