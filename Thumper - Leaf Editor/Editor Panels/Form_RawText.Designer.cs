namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    partial class Form_RawText
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_RawText));
            this.textEditor = new Syncfusion.Windows.Forms.Edit.EditControl();
            ((System.ComponentModel.ISupportInitialize)this.textEditor).BeginInit();
            this.SuspendLayout();
            // 
            // textEditor
            // 
            this.textEditor.AllowZoom = true;
            this.textEditor.ChangedLinesMarkingLineColor = Color.FromArgb(255, 238, 98);
            this.textEditor.CodeSnipptSize = new Size(100, 100);
            this.textEditor.ContextChoiceBackColor = Color.FromArgb(255, 255, 255);
            this.textEditor.ContextChoiceBorderColor = Color.FromArgb(233, 166, 50);
            this.textEditor.ContextChoiceForeColor = SystemColors.InfoText;
            this.textEditor.Dock = DockStyle.Fill;
            this.textEditor.IndicatorMarginBackColor = Color.Empty;
            this.textEditor.LikeVisualStudioSearch = true;
            this.textEditor.LineNumbersColor = Color.FromArgb(0, 128, 128);
            this.textEditor.Location = new Point(0, 0);
            this.textEditor.Name = "textEditor";
            this.textEditor.RenderRightToLeft = false;
            this.textEditor.SelectionTextColor = Color.FromArgb(173, 214, 255);
            this.textEditor.ShowEndOfLine = false;
            this.textEditor.Size = new Size(539, 497);
            this.textEditor.StatusBarSettings.CoordsPanel.Width = 150;
            this.textEditor.StatusBarSettings.EncodingPanel.Width = 100;
            this.textEditor.StatusBarSettings.FileNamePanel.Width = 100;
            this.textEditor.StatusBarSettings.InsertPanel.Width = 33;
            this.textEditor.StatusBarSettings.Offcie2007ColorScheme = Syncfusion.Windows.Forms.Office2007Theme.Blue;
            this.textEditor.StatusBarSettings.Offcie2010ColorScheme = Syncfusion.Windows.Forms.Office2010Theme.Blue;
            this.textEditor.StatusBarSettings.StatusPanel.Width = 70;
            this.textEditor.StatusBarSettings.TextPanel.Width = 214;
            this.textEditor.StatusBarSettings.VisualStyle = Syncfusion.Windows.Forms.Tools.Controls.StatusBar.VisualStyle.Default;
            this.textEditor.TabIndex = 0;
            this.textEditor.Text = "editControl1";
            this.textEditor.UseXPStyleBorder = true;
            this.textEditor.VScrollMode = Syncfusion.Windows.Forms.Edit.ScrollMode.Immediate;
            this.textEditor.WrappedLinesOffset = 10;
            this.textEditor.ZoomFactor = 1F;
            // 
            // Form_RawText
            // 
            this.BackColor = Color.FromArgb(31, 31, 31);
            this.ClientSize = new Size(539, 497);
            this.Controls.Add(this.textEditor);
            this.ForeColor = Color.White;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Name = "Form_RawText";
            this.Text = "Raw Editor";
            ((System.ComponentModel.ISupportInitialize)this.textEditor).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        public Syncfusion.Windows.Forms.Edit.EditControl textEditor;
    }
}