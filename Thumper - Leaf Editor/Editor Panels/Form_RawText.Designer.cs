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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_RawText));
            this.textEditor = new FastColoredTextBoxNS.FastColoredTextBox();
            ((System.ComponentModel.ISupportInitialize)this.textEditor).BeginInit();
            this.SuspendLayout();
            // 
            // textEditor
            // 
            this.textEditor.AccessibleDescription = "Textbox control";
            this.textEditor.AccessibleName = "Fast Colored Text Box";
            this.textEditor.AccessibleRole = AccessibleRole.Text;
            this.textEditor.AutoCompleteBracketsList = new char[]
    {
    '(',
    ')',
    '{',
    '}',
    '[',
    ']',
    '"',
    '"',
    '\'',
    '\''
    };
            this.textEditor.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\r\n^\\s*(case|default)\\s*[^:]*(?<range>:)\\s*(?<range>[^;]+);";
            this.textEditor.AutoScrollMinSize = new Size(59, 14);
            this.textEditor.BackBrush = null;
            this.textEditor.BackColor = Color.FromArgb(31, 31, 31);
            this.textEditor.CharHeight = 14;
            this.textEditor.CharWidth = 8;
            this.textEditor.DefaultMarkerSize = 8;
            this.textEditor.DisabledColor = Color.FromArgb(100, 180, 180, 180);
            this.textEditor.Dock = DockStyle.Fill;
            this.textEditor.FindForm = null;
            this.textEditor.FoldingHighlightColor = Color.LightGray;
            this.textEditor.FoldingHighlightEnabled = false;
            this.textEditor.ForeColor = Color.White;
            this.textEditor.GoToForm = null;
            this.textEditor.Hotkeys = resources.GetString("textEditor.Hotkeys");
            this.textEditor.IndentBackColor = Color.Black;
            this.textEditor.IsReplaceMode = false;
            this.textEditor.Location = new Point(0, 0);
            this.textEditor.Name = "textEditor";
            this.textEditor.Paddings = new Padding(0);
            this.textEditor.ReplaceForm = null;
            this.textEditor.SelectionColor = Color.FromArgb(60, 0, 0, 255);
            this.textEditor.ServiceColors = (FastColoredTextBoxNS.ServiceColors)resources.GetObject("textEditor.ServiceColors");
            this.textEditor.Size = new Size(284, 261);
            this.textEditor.TabIndex = 0;
            this.textEditor.Text = "text";
            this.textEditor.ToolTipDelay = 100;
            this.textEditor.Zoom = 100;
            this.textEditor.UndoRedoStateChanged += this.textEditor_UndoRedoStateChanged;
            // 
            // Form_RawText
            // 
            this.ClientSize = new Size(284, 261);
            this.Controls.Add(this.textEditor);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Name = "Form_RawText";
            this.Text = "Raw Editor";
            ((System.ComponentModel.ISupportInitialize)this.textEditor).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private FastColoredTextBoxNS.FastColoredTextBox textEditor;
    }
}