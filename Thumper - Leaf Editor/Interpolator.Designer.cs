namespace Thumper___Leaf_Editor
{
	partial class Interpolator
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Interpolator));
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.Offset = new System.Windows.Forms.TabPage();
			this.txtOffset_In = new System.Windows.Forms.RichTextBox();
			this.btnOffset_Out = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.txtOffset_beat = new System.Windows.Forms.TextBox();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.btnSmooth_output = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txtSmoothTurn_bStart = new System.Windows.Forms.TextBox();
			this.txtSmoothTurn_bEnd = new System.Windows.Forms.TextBox();
			this.txtSmooth_angleStart = new System.Windows.Forms.TextBox();
			this.txtSmooth_angleTarget = new System.Windows.Forms.TextBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.radioReturn_start = new System.Windows.Forms.RadioButton();
			this.radioReturn_none = new System.Windows.Forms.RadioButton();
			this.label7 = new System.Windows.Forms.Label();
			this.tabConstant = new System.Windows.Forms.TabPage();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtConstant_bStart = new System.Windows.Forms.TextBox();
			this.txtConstant_bEnd = new System.Windows.Forms.TextBox();
			this.txtConstant_angle = new System.Windows.Forms.TextBox();
			this.btnConstant_output = new System.Windows.Forms.Button();
			this.radionth = new System.Windows.Forms.RadioButton();
			this.radionrow = new System.Windows.Forms.RadioButton();
			this.txtNBeats = new System.Windows.Forms.TextBox();
			this.radioNone = new System.Windows.Forms.RadioButton();
			this.tabSmooth = new System.Windows.Forms.TabControl();
			this.Offset.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.tabConstant.SuspendLayout();
			this.tabSmooth.SuspendLayout();
			this.SuspendLayout();
			// 
			// richTextBox1
			// 
			this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.richTextBox1.ForeColor = System.Drawing.Color.White;
			this.richTextBox1.Location = new System.Drawing.Point(0, 263);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(410, 220);
			this.richTextBox1.TabIndex = 1;
			this.richTextBox1.Text = "";
			// 
			// toolTip1
			// 
			this.toolTip1.AutoPopDelay = 50000;
			this.toolTip1.BackColor = System.Drawing.Color.White;
			this.toolTip1.InitialDelay = 500;
			this.toolTip1.ReshowDelay = 100;
			this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			// 
			// Offset
			// 
			this.Offset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
			this.Offset.Controls.Add(this.txtOffset_beat);
			this.Offset.Controls.Add(this.txtOffset_In);
			this.Offset.Controls.Add(this.label8);
			this.Offset.Controls.Add(this.btnOffset_Out);
			this.Offset.Location = new System.Drawing.Point(4, 22);
			this.Offset.Name = "Offset";
			this.Offset.Size = new System.Drawing.Size(402, 240);
			this.Offset.TabIndex = 4;
			this.Offset.Text = "Offset";
			// 
			// txtOffset_In
			// 
			this.txtOffset_In.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.txtOffset_In.ForeColor = System.Drawing.Color.White;
			this.txtOffset_In.Location = new System.Drawing.Point(3, 3);
			this.txtOffset_In.Name = "txtOffset_In";
			this.txtOffset_In.Size = new System.Drawing.Size(395, 203);
			this.txtOffset_In.TabIndex = 0;
			this.txtOffset_In.Text = "enter text here";
			// 
			// btnOffset_Out
			// 
			this.btnOffset_Out.BackColor = System.Drawing.Color.Gray;
			this.btnOffset_Out.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnOffset_Out.Location = new System.Drawing.Point(5, 211);
			this.btnOffset_Out.Name = "btnOffset_Out";
			this.btnOffset_Out.Size = new System.Drawing.Size(75, 23);
			this.btnOffset_Out.TabIndex = 7;
			this.btnOffset_Out.Text = "Output";
			this.btnOffset_Out.UseVisualStyleBackColor = false;
			this.btnOffset_Out.Click += new System.EventHandler(this.btnOffset_Out_Click);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.ForeColor = System.Drawing.Color.White;
			this.label8.Location = new System.Drawing.Point(86, 214);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(71, 13);
			this.label8.TabIndex = 8;
			this.label8.Text = "Beat Offset";
			// 
			// txtOffset_beat
			// 
			this.txtOffset_beat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.txtOffset_beat.ForeColor = System.Drawing.Color.White;
			this.txtOffset_beat.Location = new System.Drawing.Point(155, 211);
			this.txtOffset_beat.Name = "txtOffset_beat";
			this.txtOffset_beat.Size = new System.Drawing.Size(72, 20);
			this.txtOffset_beat.TabIndex = 9;
			this.txtOffset_beat.Text = "0";
			this.txtOffset_beat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtOffset_beat_KeyPress);
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
			this.tabPage1.Controls.Add(this.label7);
			this.tabPage1.Controls.Add(this.panel2);
			this.tabPage1.Controls.Add(this.txtSmooth_angleTarget);
			this.tabPage1.Controls.Add(this.txtSmooth_angleStart);
			this.tabPage1.Controls.Add(this.txtSmoothTurn_bEnd);
			this.tabPage1.Controls.Add(this.txtSmoothTurn_bStart);
			this.tabPage1.Controls.Add(this.label6);
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Controls.Add(this.btnSmooth_output);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(402, 240);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Smooth Transitions";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(7, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Beat Start";
			// 
			// btnSmooth_output
			// 
			this.btnSmooth_output.BackColor = System.Drawing.Color.Gray;
			this.btnSmooth_output.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSmooth_output.Location = new System.Drawing.Point(10, 140);
			this.btnSmooth_output.Name = "btnSmooth_output";
			this.btnSmooth_output.Size = new System.Drawing.Size(72, 23);
			this.btnSmooth_output.TabIndex = 6;
			this.btnSmooth_output.Text = "Output";
			this.btnSmooth_output.UseVisualStyleBackColor = false;
			this.btnSmooth_output.Click += new System.EventHandler(this.btnSmooth_output_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(107, 3);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(70, 13);
			this.label3.TabIndex = 10;
			this.label3.Text = "Start Value";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.ForeColor = System.Drawing.Color.White;
			this.label6.Location = new System.Drawing.Point(107, 41);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 13);
			this.label6.TabIndex = 12;
			this.label6.Text = "Target Value";
			// 
			// txtSmoothTurn_bStart
			// 
			this.txtSmoothTurn_bStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.txtSmoothTurn_bStart.ForeColor = System.Drawing.Color.White;
			this.txtSmoothTurn_bStart.Location = new System.Drawing.Point(10, 18);
			this.txtSmoothTurn_bStart.Name = "txtSmoothTurn_bStart";
			this.txtSmoothTurn_bStart.Size = new System.Drawing.Size(72, 20);
			this.txtSmoothTurn_bStart.TabIndex = 1;
			this.txtSmoothTurn_bStart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSmoothTurn_bStart_KeyPress);
			// 
			// txtSmoothTurn_bEnd
			// 
			this.txtSmoothTurn_bEnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.txtSmoothTurn_bEnd.ForeColor = System.Drawing.Color.White;
			this.txtSmoothTurn_bEnd.Location = new System.Drawing.Point(10, 57);
			this.txtSmoothTurn_bEnd.Name = "txtSmoothTurn_bEnd";
			this.txtSmoothTurn_bEnd.Size = new System.Drawing.Size(72, 20);
			this.txtSmoothTurn_bEnd.TabIndex = 9;
			this.txtSmoothTurn_bEnd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSmoothTurn_bEnd_KeyPress);
			// 
			// txtSmooth_angleStart
			// 
			this.txtSmooth_angleStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.txtSmooth_angleStart.ForeColor = System.Drawing.Color.White;
			this.txtSmooth_angleStart.Location = new System.Drawing.Point(110, 18);
			this.txtSmooth_angleStart.Name = "txtSmooth_angleStart";
			this.txtSmooth_angleStart.Size = new System.Drawing.Size(72, 20);
			this.txtSmooth_angleStart.TabIndex = 11;
			this.txtSmooth_angleStart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSmooth_angleStart_KeyPress);
			// 
			// txtSmooth_angleTarget
			// 
			this.txtSmooth_angleTarget.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.txtSmooth_angleTarget.ForeColor = System.Drawing.Color.White;
			this.txtSmooth_angleTarget.Location = new System.Drawing.Point(110, 56);
			this.txtSmooth_angleTarget.Name = "txtSmooth_angleTarget";
			this.txtSmooth_angleTarget.Size = new System.Drawing.Size(72, 20);
			this.txtSmooth_angleTarget.TabIndex = 13;
			this.txtSmooth_angleTarget.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSmooth_angleTarget_KeyPress);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.radioReturn_none);
			this.panel2.Controls.Add(this.radioReturn_start);
			this.panel2.Location = new System.Drawing.Point(110, 75);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(106, 39);
			this.panel2.TabIndex = 9;
			// 
			// radioReturn_start
			// 
			this.radioReturn_start.AutoSize = true;
			this.radioReturn_start.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radioReturn_start.ForeColor = System.Drawing.Color.White;
			this.radioReturn_start.Location = new System.Drawing.Point(3, 3);
			this.radioReturn_start.Name = "radioReturn_start";
			this.radioReturn_start.Size = new System.Drawing.Size(109, 17);
			this.radioReturn_start.TabIndex = 7;
			this.radioReturn_start.TabStop = true;
			this.radioReturn_start.Text = "Return to Start";
			this.radioReturn_start.UseVisualStyleBackColor = true;
			this.radioReturn_start.CheckedChanged += new System.EventHandler(this.radioReturn_start_CheckedChanged);
			// 
			// radioReturn_none
			// 
			this.radioReturn_none.AutoSize = true;
			this.radioReturn_none.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radioReturn_none.ForeColor = System.Drawing.Color.White;
			this.radioReturn_none.Location = new System.Drawing.Point(3, 18);
			this.radioReturn_none.Name = "radioReturn_none";
			this.radioReturn_none.Size = new System.Drawing.Size(100, 17);
			this.radioReturn_none.TabIndex = 9;
			this.radioReturn_none.TabStop = true;
			this.radioReturn_none.Text = "Do not return";
			this.radioReturn_none.UseVisualStyleBackColor = true;
			this.radioReturn_none.CheckedChanged += new System.EventHandler(this.radioReturn_none_CheckedChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.ForeColor = System.Drawing.Color.White;
			this.label7.Location = new System.Drawing.Point(7, 41);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(59, 13);
			this.label7.TabIndex = 15;
			this.label7.Text = "Beat End";
			// 
			// tabConstant
			// 
			this.tabConstant.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
			this.tabConstant.Controls.Add(this.radioNone);
			this.tabConstant.Controls.Add(this.txtNBeats);
			this.tabConstant.Controls.Add(this.txtConstant_angle);
			this.tabConstant.Controls.Add(this.txtConstant_bEnd);
			this.tabConstant.Controls.Add(this.txtConstant_bStart);
			this.tabConstant.Controls.Add(this.radionrow);
			this.tabConstant.Controls.Add(this.radionth);
			this.tabConstant.Controls.Add(this.btnConstant_output);
			this.tabConstant.Controls.Add(this.label2);
			this.tabConstant.Controls.Add(this.label4);
			this.tabConstant.Controls.Add(this.label5);
			this.tabConstant.Location = new System.Drawing.Point(4, 22);
			this.tabConstant.Name = "tabConstant";
			this.tabConstant.Size = new System.Drawing.Size(402, 240);
			this.tabConstant.TabIndex = 3;
			this.tabConstant.Text = "Constant";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.ForeColor = System.Drawing.Color.White;
			this.label5.Location = new System.Drawing.Point(7, 3);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 13);
			this.label5.TabIndex = 4;
			this.label5.Text = "Beat Start";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(7, 41);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(59, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "Beat End";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(7, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(39, 13);
			this.label2.TabIndex = 8;
			this.label2.Text = "Value";
			// 
			// txtConstant_bStart
			// 
			this.txtConstant_bStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.txtConstant_bStart.ForeColor = System.Drawing.Color.White;
			this.txtConstant_bStart.Location = new System.Drawing.Point(10, 18);
			this.txtConstant_bStart.Name = "txtConstant_bStart";
			this.txtConstant_bStart.Size = new System.Drawing.Size(72, 20);
			this.txtConstant_bStart.TabIndex = 5;
			this.txtConstant_bStart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtConstant_bStart_KeyPress);
			// 
			// txtConstant_bEnd
			// 
			this.txtConstant_bEnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.txtConstant_bEnd.ForeColor = System.Drawing.Color.White;
			this.txtConstant_bEnd.Location = new System.Drawing.Point(10, 57);
			this.txtConstant_bEnd.Name = "txtConstant_bEnd";
			this.txtConstant_bEnd.Size = new System.Drawing.Size(72, 20);
			this.txtConstant_bEnd.TabIndex = 7;
			this.txtConstant_bEnd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtConstant_bEnd_KeyPress);
			// 
			// txtConstant_angle
			// 
			this.txtConstant_angle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.txtConstant_angle.ForeColor = System.Drawing.Color.White;
			this.txtConstant_angle.Location = new System.Drawing.Point(10, 96);
			this.txtConstant_angle.Name = "txtConstant_angle";
			this.txtConstant_angle.Size = new System.Drawing.Size(72, 20);
			this.txtConstant_angle.TabIndex = 9;
			// 
			// btnConstant_output
			// 
			this.btnConstant_output.BackColor = System.Drawing.Color.Gray;
			this.btnConstant_output.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnConstant_output.Location = new System.Drawing.Point(10, 139);
			this.btnConstant_output.Name = "btnConstant_output";
			this.btnConstant_output.Size = new System.Drawing.Size(72, 23);
			this.btnConstant_output.TabIndex = 10;
			this.btnConstant_output.Text = "Output";
			this.btnConstant_output.UseVisualStyleBackColor = false;
			this.btnConstant_output.Click += new System.EventHandler(this.btnConstant_output_Click);
			// 
			// radionth
			// 
			this.radionth.AutoSize = true;
			this.radionth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radionth.ForeColor = System.Drawing.Color.White;
			this.radionth.Location = new System.Drawing.Point(109, 19);
			this.radionth.Name = "radionth";
			this.radionth.Size = new System.Drawing.Size(136, 17);
			this.radionth.TabIndex = 11;
			this.radionth.Text = "Skip every nth beat";
			this.radionth.UseVisualStyleBackColor = true;
			// 
			// radionrow
			// 
			this.radionrow.AutoSize = true;
			this.radionrow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radionrow.ForeColor = System.Drawing.Color.White;
			this.radionrow.Location = new System.Drawing.Point(109, 35);
			this.radionrow.Name = "radionrow";
			this.radionrow.Size = new System.Drawing.Size(145, 17);
			this.radionrow.TabIndex = 12;
			this.radionrow.Text = "Skip n beats in a row";
			this.radionrow.UseVisualStyleBackColor = true;
			// 
			// txtNBeats
			// 
			this.txtNBeats.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.txtNBeats.ForeColor = System.Drawing.Color.White;
			this.txtNBeats.Location = new System.Drawing.Point(128, 54);
			this.txtNBeats.MaxLength = 3;
			this.txtNBeats.Name = "txtNBeats";
			this.txtNBeats.Size = new System.Drawing.Size(72, 20);
			this.txtNBeats.TabIndex = 13;
			this.txtNBeats.Text = "0";
			this.txtNBeats.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNBeats_KeyPress);
			// 
			// radioNone
			// 
			this.radioNone.AutoSize = true;
			this.radioNone.Checked = true;
			this.radioNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radioNone.ForeColor = System.Drawing.Color.White;
			this.radioNone.Location = new System.Drawing.Point(109, 3);
			this.radioNone.Name = "radioNone";
			this.radioNone.Size = new System.Drawing.Size(68, 17);
			this.radioNone.TabIndex = 14;
			this.radioNone.TabStop = true;
			this.radioNone.Text = "No skip";
			this.radioNone.UseVisualStyleBackColor = true;
			// 
			// tabSmooth
			// 
			this.tabSmooth.Controls.Add(this.tabConstant);
			this.tabSmooth.Controls.Add(this.tabPage1);
			this.tabSmooth.Controls.Add(this.Offset);
			this.tabSmooth.Location = new System.Drawing.Point(0, 0);
			this.tabSmooth.Name = "tabSmooth";
			this.tabSmooth.SelectedIndex = 0;
			this.tabSmooth.Size = new System.Drawing.Size(410, 266);
			this.tabSmooth.TabIndex = 0;
			// 
			// Interpolator
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.ClientSize = new System.Drawing.Size(410, 484);
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.tabSmooth);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Interpolator";
			this.Text = "Interpolator for Thumper Editor";
			this.Load += new System.EventHandler(this.Interpolator_Load);
			this.Offset.ResumeLayout(false);
			this.Offset.PerformLayout();
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.tabConstant.ResumeLayout(false);
			this.tabConstant.PerformLayout();
			this.tabSmooth.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.TabPage Offset;
		private System.Windows.Forms.TextBox txtOffset_beat;
		private System.Windows.Forms.RichTextBox txtOffset_In;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button btnOffset_Out;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.RadioButton radioReturn_none;
		private System.Windows.Forms.RadioButton radioReturn_start;
		private System.Windows.Forms.TextBox txtSmooth_angleTarget;
		private System.Windows.Forms.TextBox txtSmooth_angleStart;
		private System.Windows.Forms.TextBox txtSmoothTurn_bEnd;
		private System.Windows.Forms.TextBox txtSmoothTurn_bStart;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnSmooth_output;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabPage tabConstant;
		private System.Windows.Forms.RadioButton radioNone;
		private System.Windows.Forms.TextBox txtNBeats;
		private System.Windows.Forms.TextBox txtConstant_angle;
		private System.Windows.Forms.TextBox txtConstant_bEnd;
		private System.Windows.Forms.TextBox txtConstant_bStart;
		private System.Windows.Forms.RadioButton radionrow;
		private System.Windows.Forms.RadioButton radionth;
		private System.Windows.Forms.Button btnConstant_output;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TabControl tabSmooth;
	}
}