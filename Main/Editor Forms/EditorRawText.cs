using FastColoredTextBoxNS.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeifenLuo.WinFormsUI.Docking;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class EditorRawText : EditorBase
    {
        #region Form Construction
        public EditorRawText(string _load, FileInfo filepath) : base(filepath, true)
        {
            InitializeComponent();
            ColorFormElements();
            textEditor.Language = FastColoredTextBoxNS.Text.Language.JSON;
            textEditor.Text = _load;
            textEditor.ClearUndo();
            textEditor.SetSelectedLine(-1);
            textEditor.TextChanged += textEditor_TextChanged;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!IsSaved()) {
                if (MessageBox.Show("File not saved. Are you sure you want to close it and discard changes?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No) {
                    e.Cancel = true;
                }
            }
        }

        public override void ColorFormElements()
        {
            this.BackColor = Properties.Settings.Default.ColorRawBG;
            textEditor.BackColor = Properties.Settings.Default.ColorRawBG;
            textEditor.ForeColor = Properties.Settings.Default.ColorRawText;
        }
        #endregion
        #region Variables
        public bool EditorIsSaved = true;

        #endregion
        #region Event Handlers
        private void textEditor_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            SaveCheckAndWrite(false);
        }
        #endregion
        #region Methods
        public bool IsSaved()
        {
            return EditorIsSaved;
        }

        public override object GetProperties()
        {
            return null;
        }

        public override void Save(bool playsound = true)
        {
            SaveCheckAndWrite(true, playsound);
        }

        public override FileInfo SaveAs(bool FileIsNew, string InitialDir) { return null;  }

        public void Reload()
        {
            dynamic _load = UtilFile.LoadFileLock(this.WorkingFile);
            textEditor.TextChanged -= textEditor_TextChanged;
            textEditor.Text = JsonConvert.SerializeObject(_load, Formatting.Indented);
            textEditor.ClearUndo();
            textEditor.SetSelectedLine(-1);
            textEditor.TextChanged += textEditor_TextChanged;

            EditorIsSaved = true;
            this.Text = this.WorkingFile.Name + " [Raw]";
            this.Invalidate();
        }

        public void SaveCheckAndWrite(bool IsSaved, bool playsound = false)
        {
            //make the beeble emote
            TCLE.MainBeeble.MakeFace();

            EditorIsSaved = IsSaved;
            if (!IsSaved) {
                //denote editor tab is not saved
                this.Text = this.WorkingFile.Name + " [Raw]*";
            }
            else {
                //build the JSON to write to file
                JObject _saveJSON = new();
                try {
                    _saveJSON = JObject.Parse(textEditor.Text);
                }
                catch (Exception ex) {
                    MessageBox.Show($"JSON failed to parse in file. Changes not saved.\n\n{ex}", "Thumper Custom Level Editor");
                    return;
                }
                //denote editor tab is saved
                this.Text = this.WorkingFile.Name + " [Raw]";
                //write JSON to file
                UtilFile.WriteFileLock(this.FileLock, _saveJSON);

                if (playsound) UtilAudio.PlaySound("UIsave");

                foreach (EditorBase document in TCLE.Documents.Values.Where(x => x.WorkingFile.Name == this.WorkingFile.Name)) {
                    document.GetType().GetMethod("Reload").Invoke(document, null);
                }
            }
        }

        public override void Copy()
        {
            Clipboard.SetText(textEditor.SelectedText);
        }

        public override void Cut()
        {
            Clipboard.SetText(textEditor.SelectedText);
            textEditor.SelectedText = "";
        }

        public override void Paste()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}