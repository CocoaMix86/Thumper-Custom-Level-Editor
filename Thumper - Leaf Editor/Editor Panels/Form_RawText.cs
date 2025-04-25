using FastColoredTextBoxNS.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_RawText : DockContentEx
    {
        #region Form Construction
        public Form_RawText(string _load, FileInfo filepath)
        {
            loadedfile = filepath;
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

        public void ColorFormElements()
        {
            this.BackColor = Properties.Settings.Default.ColorRawBG;
            textEditor.BackColor = Properties.Settings.Default.ColorRawBG;
            textEditor.ForeColor = Properties.Settings.Default.ColorRawText;
        }
        #endregion
        #region Variables
        public bool EditorIsSaved = true;
        public FileInfo loadedfile
        {
            get => LoadedFile;
            set {
                if (LoadedFile != value) {
                    if (LoadedFile != null)
                        TCLE.CloseFileLock(LoadedFile);
                    LoadedFile = value;
                    if (!LoadedFile.Exists) {
                        using (StreamWriter sw = LoadedFile.CreateText()) {
                            sw.Write(' ');
                            sw.Close();
                        }
                    }
                    TCLE.AddFileLock(LoadedFile);
                }
            }
        }
        private FileInfo LoadedFile;
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

        public object GetProperties()
        {
            return null;
        }

        public void Save(bool playsound = true)
        {
            SaveCheckAndWrite(true, playsound);
        }

        public void Reload()
        {
            dynamic _load = TCLE.LoadFileLock(LoadedFile.FullName);
            textEditor.TextChanged -= textEditor_TextChanged;
            textEditor.Text = JsonConvert.SerializeObject(_load, Formatting.Indented);
            textEditor.ClearUndo();
            textEditor.SetSelectedLine(-1);
            textEditor.TextChanged += textEditor_TextChanged;

            EditorIsSaved = true;
            this.Text = LoadedFile.Name + " [Raw]";
            this.Invalidate();
        }

        public void SaveCheckAndWrite(bool IsSaved, bool playsound = false)
        {
            //make the beeble emote
            TCLE.MainBeeble.MakeFace();

            EditorIsSaved = IsSaved;
            if (!IsSaved) {
                //denote editor tab is not saved
                this.Text = LoadedFile.Name + " [Raw]*";
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
                //denote editor tab is not saved
                this.Text = LoadedFile.Name + " [Raw]";
                //write JSON to file
                //TCLE.WriteFileLock(TCLE.lockedfiles[LoadedFile], _saveJSON);
                TCLE.WriteFileLock(TCLE.lockedfiles.First(x => x.Key.FullName == LoadedFile.FullName).Value, _saveJSON);

                if (playsound) TCLE.PlaySound("UIsave");

                foreach (IDockContent document in TCLE.Documents.Where(x => x.DockHandler.TabText.StartsWith(LoadedFile.Name))) {
                    document.GetType().GetMethod("Reload").Invoke(document, null);
                }

            }
        }

        public void Copy()
        {
            Clipboard.SetText(textEditor.SelectedText);
        }

        public void Cut()
        {
            Clipboard.SetText(textEditor.SelectedText);
            textEditor.SelectedText = "";
        }
        #endregion
    }
}