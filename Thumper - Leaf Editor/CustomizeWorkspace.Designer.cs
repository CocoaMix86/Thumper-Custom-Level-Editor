
namespace Thumper___Leaf_Editor
{
	partial class CustomizeWorkspace
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
			this.btnBGColor = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.btnMenuColor = new System.Windows.Forms.Button();
			this.btnPanelColor = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnBGColor
			// 
			this.btnBGColor.BackColor = System.Drawing.Color.White;
			this.btnBGColor.FlatAppearance.BorderColor = System.Drawing.Color.Black;
			this.btnBGColor.FlatAppearance.BorderSize = 4;
			this.btnBGColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnBGColor.Location = new System.Drawing.Point(12, 12);
			this.btnBGColor.Name = "btnBGColor";
			this.btnBGColor.Size = new System.Drawing.Size(211, 37);
			this.btnBGColor.TabIndex = 97;
			this.btnBGColor.Text = "Background Colour";
			this.btnBGColor.UseMnemonic = false;
			this.btnBGColor.UseVisualStyleBackColor = false;
			this.btnBGColor.Click += new System.EventHandler(this.btnBGColor_Click);
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.Location = new System.Drawing.Point(12, 264);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(211, 26);
			this.button1.TabIndex = 98;
			this.button1.Text = "Apply Changes";
			this.button1.UseVisualStyleBackColor = false;
			// 
			// btnMenuColor
			// 
			this.btnMenuColor.BackColor = System.Drawing.Color.White;
			this.btnMenuColor.FlatAppearance.BorderColor = System.Drawing.Color.Black;
			this.btnMenuColor.FlatAppearance.BorderSize = 4;
			this.btnMenuColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnMenuColor.Location = new System.Drawing.Point(12, 55);
			this.btnMenuColor.Name = "btnMenuColor";
			this.btnMenuColor.Size = new System.Drawing.Size(211, 37);
			this.btnMenuColor.TabIndex = 99;
			this.btnMenuColor.Text = "Menu Colour";
			this.btnMenuColor.UseMnemonic = false;
			this.btnMenuColor.UseVisualStyleBackColor = false;
			this.btnMenuColor.Click += new System.EventHandler(this.btnMenuColor_Click);
			// 
			// btnPanelColor
			// 
			this.btnPanelColor.BackColor = System.Drawing.Color.White;
			this.btnPanelColor.FlatAppearance.BorderColor = System.Drawing.Color.Black;
			this.btnPanelColor.FlatAppearance.BorderSize = 4;
			this.btnPanelColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnPanelColor.Location = new System.Drawing.Point(12, 98);
			this.btnPanelColor.Name = "btnPanelColor";
			this.btnPanelColor.Size = new System.Drawing.Size(211, 37);
			this.btnPanelColor.TabIndex = 100;
			this.btnPanelColor.Text = "Editor Panel Colour";
			this.btnPanelColor.UseMnemonic = false;
			this.btnPanelColor.UseVisualStyleBackColor = false;
			this.btnPanelColor.Click += new System.EventHandler(this.btnPanelColor_Click);
			// 
			// CustomizeWorkspace
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.ClientSize = new System.Drawing.Size(237, 299);
			this.Controls.Add(this.btnPanelColor);
			this.Controls.Add(this.btnMenuColor);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.btnBGColor);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CustomizeWorkspace";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Customize Workspace";
			this.TopMost = true;
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ColorDialog colorDialog1;
		public System.Windows.Forms.Button btnBGColor;
		public System.Windows.Forms.Button btnMenuColor;
		public System.Windows.Forms.Button btnPanelColor;
	}
}