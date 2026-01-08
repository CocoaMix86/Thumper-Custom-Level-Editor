
namespace Thumper_Custom_Level_Editor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomizeWorkspace));
            this.colorDialog1 = new ColorDialog();
            this.toolstripCustomize = new ToolStrip();
            this.btnCustomizeApply = new ToolStripButton();
            this.tabControl1 = new TabControl();
            this.tabUIColors = new TabPage();
            this.propertyGridUIColors = new PropertyGrid();
            this.tabSeq = new TabPage();
            this.treeObjects = new TreeViewEx();
            this.imageList1 = new ImageList(this.components);
            this.txtSearch = new TextBox();
            this.tabAudio = new TabPage();
            this.checkMuteApp = new CheckBox();
            this.tabPage1 = new TabPage();
            this.propertyGridKeyBinds = new PropertyGrid();
            this.toolTip1 = new ToolTip(this.components);
            this.toolstripCustomize.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabUIColors.SuspendLayout();
            this.tabSeq.SuspendLayout();
            this.tabAudio.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;
            this.colorDialog1.SolidColorOnly = true;
            // 
            // toolstripCustomize
            // 
            this.toolstripCustomize.BackColor = Color.FromArgb(40, 40, 40);
            this.toolstripCustomize.Dock = DockStyle.Bottom;
            this.toolstripCustomize.GripStyle = ToolStripGripStyle.Hidden;
            this.toolstripCustomize.Items.AddRange(new ToolStripItem[] { this.btnCustomizeApply });
            this.toolstripCustomize.Location = new Point(0, 526);
            this.toolstripCustomize.Name = "toolstripCustomize";
            this.toolstripCustomize.Size = new Size(496, 25);
            this.toolstripCustomize.TabIndex = 106;
            this.toolstripCustomize.Text = "toolStrip1";
            // 
            // btnCustomizeApply
            // 
            this.btnCustomizeApply.BackColor = Color.FromArgb(0, 192, 0);
            this.btnCustomizeApply.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnCustomizeApply.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnCustomizeApply.ForeColor = SystemColors.ControlText;
            this.btnCustomizeApply.Image = (Image)resources.GetObject("btnCustomizeApply.Image");
            this.btnCustomizeApply.ImageTransparentColor = Color.Magenta;
            this.btnCustomizeApply.Margin = new Padding(196, 1, 0, 2);
            this.btnCustomizeApply.Name = "btnCustomizeApply";
            this.btnCustomizeApply.Size = new Size(91, 22);
            this.btnCustomizeApply.Text = "Apply Changes";
            this.btnCustomizeApply.Click += this.btnCustomizeApply_Click;
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = TabAlignment.Left;
            this.tabControl1.Controls.Add(this.tabUIColors);
            this.tabControl1.Controls.Add(this.tabSeq);
            this.tabControl1.Controls.Add(this.tabAudio);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.tabControl1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.tabControl1.HotTrack = true;
            this.tabControl1.ItemSize = new Size(30, 120);
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(496, 526);
            this.tabControl1.SizeMode = TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 107;
            this.tabControl1.DrawItem += this.tabControl1_DrawItem;
            // 
            // tabUIColors
            // 
            this.tabUIColors.BackColor = Color.FromArgb(55, 55, 55);
            this.tabUIColors.Controls.Add(this.propertyGridUIColors);
            this.tabUIColors.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            this.tabUIColors.Location = new Point(124, 4);
            this.tabUIColors.Name = "tabUIColors";
            this.tabUIColors.Padding = new Padding(3);
            this.tabUIColors.Size = new Size(368, 518);
            this.tabUIColors.TabIndex = 0;
            this.tabUIColors.Text = "UI Theme";
            // 
            // propertyGridUIColors
            // 
            this.propertyGridUIColors.BackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridUIColors.CategoryForeColor = Color.White;
            this.propertyGridUIColors.CategorySplitterColor = Color.FromArgb(46, 46, 46);
            this.propertyGridUIColors.DisabledItemForeColor = Color.FromArgb(127, 255, 255, 255);
            this.propertyGridUIColors.Dock = DockStyle.Fill;
            this.propertyGridUIColors.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.propertyGridUIColors.HelpBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridUIColors.HelpBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridUIColors.HelpForeColor = Color.White;
            this.propertyGridUIColors.HelpVisible = false;
            this.propertyGridUIColors.LineColor = Color.FromArgb(46, 46, 46);
            this.propertyGridUIColors.Location = new Point(3, 3);
            this.propertyGridUIColors.Margin = new Padding(4, 3, 4, 3);
            this.propertyGridUIColors.Name = "propertyGridUIColors";
            this.propertyGridUIColors.PropertySort = PropertySort.Categorized;
            this.propertyGridUIColors.RightToLeft = RightToLeft.No;
            this.propertyGridUIColors.SelectedItemWithFocusBackColor = Color.FromArgb(113, 96, 232);
            this.propertyGridUIColors.SelectedItemWithFocusForeColor = Color.White;
            this.propertyGridUIColors.Size = new Size(362, 512);
            this.propertyGridUIColors.TabIndex = 122;
            this.propertyGridUIColors.ToolbarVisible = false;
            this.propertyGridUIColors.ViewBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridUIColors.ViewBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridUIColors.ViewForeColor = Color.White;
            // 
            // tabSeq
            // 
            this.tabSeq.BackColor = Color.FromArgb(55, 55, 55);
            this.tabSeq.Controls.Add(this.treeObjects);
            this.tabSeq.Controls.Add(this.txtSearch);
            this.tabSeq.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.tabSeq.Location = new Point(124, 4);
            this.tabSeq.Name = "tabSeq";
            this.tabSeq.Size = new Size(368, 337);
            this.tabSeq.TabIndex = 3;
            this.tabSeq.Text = "Sequencer Colors";
            // 
            // treeObjects
            // 
            this.treeObjects.BackColor = Color.FromArgb(31, 31, 31);
            this.treeObjects.BorderStyle = BorderStyle.None;
            this.treeObjects.Dock = DockStyle.Fill;
            this.treeObjects.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.treeObjects.ForeColor = Color.White;
            this.treeObjects.FullRowSelect = true;
            this.treeObjects.HideSelection = false;
            this.treeObjects.ImageKey = "other";
            this.treeObjects.ImageList = this.imageList1;
            this.treeObjects.ItemHeight = 16;
            this.treeObjects.LineColor = Color.White;
            this.treeObjects.Location = new Point(0, 22);
            this.treeObjects.Margin = new Padding(4, 3, 4, 3);
            this.treeObjects.Name = "treeObjects";
            this.treeObjects.SelectedImageKey = "other";
            this.treeObjects.ShowNodeToolTips = true;
            this.treeObjects.ShowRootLines = false;
            this.treeObjects.Size = new Size(368, 315);
            this.treeObjects.TabIndex = 101;
            this.treeObjects.NodeMouseDoubleClick += this.treeObjects_NodeMouseDoubleClick;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = ColorDepth.Depth32Bit;
            this.imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            this.imageList1.TransparentColor = Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "category");
            this.imageList1.Images.SetKeyName(1, "none");
            this.imageList1.Images.SetKeyName(2, "samp");
            this.imageList1.Images.SetKeyName(3, "fav");
            this.imageList1.Images.SetKeyName(4, "play");
            this.imageList1.Images.SetKeyName(5, "BARS - MULTI");
            this.imageList1.Images.SetKeyName(6, "BARS");
            this.imageList1.Images.SetKeyName(7, "BOSS EFFECTS");
            this.imageList1.Images.SetKeyName(8, "BOSS SHIELDS");
            this.imageList1.Images.SetKeyName(9, "CAMERA");
            this.imageList1.Images.SetKeyName(10, "CONTROLLER");
            this.imageList1.Images.SetKeyName(11, "DECORATIVE MILLIPEDES");
            this.imageList1.Images.SetKeyName(12, "DISSONANT_BURSTS");
            this.imageList1.Images.SetKeyName(13, "EXPERIMENTAL");
            this.imageList1.Images.SetKeyName(14, "FX");
            this.imageList1.Images.SetKeyName(15, "GAMMA");
            this.imageList1.Images.SetKeyName(16, "INTROFLOW");
            this.imageList1.Images.SetKeyName(17, "JUMPSSPIKES");
            this.imageList1.Images.SetKeyName(18, "LOOP TRACK VOLUME");
            this.imageList1.Images.SetKeyName(19, "MILLIPEDES");
            this.imageList1.Images.SetKeyName(20, "PLAY SAMPLE");
            this.imageList1.Images.SetKeyName(21, "POST PROCESSING");
            this.imageList1.Images.SetKeyName(22, "RAIL COLORS");
            this.imageList1.Images.SetKeyName(23, "RINGS");
            this.imageList1.Images.SetKeyName(24, "SENTRY");
            this.imageList1.Images.SetKeyName(25, "SKYBOX_COLORS");
            this.imageList1.Images.SetKeyName(26, "SMOKE FX");
            this.imageList1.Images.SetKeyName(27, "TENTACLES");
            this.imageList1.Images.SetKeyName(28, "THUMPS");
            this.imageList1.Images.SetKeyName(29, "TRACK EFFECTS");
            this.imageList1.Images.SetKeyName(30, "TRACK FX");
            this.imageList1.Images.SetKeyName(31, "WIN & MISC");
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = Color.FromArgb(56, 56, 56);
            this.txtSearch.BorderStyle = BorderStyle.FixedSingle;
            this.txtSearch.Dock = DockStyle.Top;
            this.txtSearch.ForeColor = Color.White;
            this.txtSearch.Location = new Point(0, 0);
            this.txtSearch.Margin = new Padding(4, 3, 4, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new Size(368, 22);
            this.txtSearch.TabIndex = 100;
            this.txtSearch.Text = "Search Objects (Ctrl+;)";
            this.txtSearch.TextChanged += this.txtSearch_TextChanged;
            // 
            // tabAudio
            // 
            this.tabAudio.BackColor = Color.FromArgb(55, 55, 55);
            this.tabAudio.Controls.Add(this.checkMuteApp);
            this.tabAudio.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            this.tabAudio.Location = new Point(124, 4);
            this.tabAudio.Name = "tabAudio";
            this.tabAudio.Padding = new Padding(3);
            this.tabAudio.Size = new Size(368, 337);
            this.tabAudio.TabIndex = 1;
            this.tabAudio.Text = "Audio";
            // 
            // checkMuteApp
            // 
            this.checkMuteApp.AutoSize = true;
            this.checkMuteApp.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.checkMuteApp.ForeColor = Color.White;
            this.checkMuteApp.Location = new Point(40, 32);
            this.checkMuteApp.Name = "checkMuteApp";
            this.checkMuteApp.Size = new Size(192, 19);
            this.checkMuteApp.TabIndex = 0;
            this.checkMuteApp.Text = "Mute application sound effects";
            this.checkMuteApp.UseVisualStyleBackColor = true;
            this.checkMuteApp.CheckedChanged += this.checkMuteApp_CheckedChanged;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = Color.FromArgb(55, 55, 55);
            this.tabPage1.Controls.Add(this.propertyGridKeyBinds);
            this.tabPage1.Location = new Point(124, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.Size = new Size(368, 337);
            this.tabPage1.TabIndex = 4;
            this.tabPage1.Text = "Key Binds";
            // 
            // propertyGridKeyBinds
            // 
            this.propertyGridKeyBinds.BackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridKeyBinds.CategoryForeColor = Color.White;
            this.propertyGridKeyBinds.CategorySplitterColor = Color.FromArgb(46, 46, 46);
            this.propertyGridKeyBinds.DisabledItemForeColor = Color.FromArgb(127, 255, 255, 255);
            this.propertyGridKeyBinds.Dock = DockStyle.Fill;
            this.propertyGridKeyBinds.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.propertyGridKeyBinds.HelpBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridKeyBinds.HelpBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridKeyBinds.HelpForeColor = Color.White;
            this.propertyGridKeyBinds.HelpVisible = false;
            this.propertyGridKeyBinds.LineColor = Color.FromArgb(46, 46, 46);
            this.propertyGridKeyBinds.Location = new Point(3, 3);
            this.propertyGridKeyBinds.Margin = new Padding(4, 3, 4, 3);
            this.propertyGridKeyBinds.Name = "propertyGridKeyBinds";
            this.propertyGridKeyBinds.PropertySort = PropertySort.Categorized;
            this.propertyGridKeyBinds.RightToLeft = RightToLeft.No;
            this.propertyGridKeyBinds.SelectedItemWithFocusBackColor = Color.FromArgb(113, 96, 232);
            this.propertyGridKeyBinds.SelectedItemWithFocusForeColor = Color.White;
            this.propertyGridKeyBinds.Size = new Size(362, 331);
            this.propertyGridKeyBinds.TabIndex = 123;
            this.propertyGridKeyBinds.ToolbarVisible = false;
            this.propertyGridKeyBinds.ViewBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridKeyBinds.ViewBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridKeyBinds.ViewForeColor = Color.White;
            // 
            // CustomizeWorkspace
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(55, 55, 55);
            this.ClientSize = new Size(496, 551);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolstripCustomize);
            this.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomizeWorkspace";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.Text = "Customize Workspace";
            this.toolstripCustomize.ResumeLayout(false);
            this.toolstripCustomize.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabUIColors.ResumeLayout(false);
            this.tabSeq.ResumeLayout(false);
            this.tabSeq.PerformLayout();
            this.tabAudio.ResumeLayout(false);
            this.tabAudio.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ToolStrip toolstripCustomize;
        private System.Windows.Forms.ToolStripButton btnCustomizeApply;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabUIColors;
        private System.Windows.Forms.TabPage tabAudio;
        public System.Windows.Forms.CheckBox checkMuteApp;
        private System.Windows.Forms.ToolTip toolTip1;
        private TabPage tabSeq;
        private TreeViewEx treeObjects;
        private TextBox txtSearch;
        public PropertyGrid propertyGridUIColors;
        private TabPage tabPage1;
        public PropertyGrid propertyGridKeyBinds;
        private ImageList imageList1;
    }
}