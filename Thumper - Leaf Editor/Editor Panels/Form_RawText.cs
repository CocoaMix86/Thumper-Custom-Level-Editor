using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_RawText : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Form Construction
        public Form_RawText(dynamic _load, FileInfo filepath)
        {
            loadedfile = filepath;
            InitializeComponent();
            textEditor.Language = FastColoredTextBoxNS.Text.Language.JSON;
            textEditor.Text = JsonConvert.SerializeObject(_load, Formatting.Indented);
            textEditor.ClearUndo();
            textEditor.SetSelectedLine(-1);
            textEditor.TextChanged += textEditor_TextChanged;
        }
        #endregion
        #region Variables
        public bool EditorIsSaved = true;
        private FileInfo loadedfile { get => LoadedFile; set => LoadedFile = value; }
        public static FileInfo LoadedFile;
        #endregion
        #region Event Handlers
        private void textEditor_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            SaveCheckAndWrite(false);
        }
        #endregion
        #region Methods
        public void Save()
        {
            SaveCheckAndWrite(true, true);
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
                    MessageBox.Show("JSON failed to parse in file. Changes not saved.", "Thumper Custom Level Editor");
                    return;
                }
                //denote editor tab is not saved
                this.Text = LoadedFile.Name + " [Raw]";
                //write JSON to file
                //TCLE.WriteFileLock(TCLE.lockedfiles[LoadedFile], _saveJSON);
                TCLE.WriteFileLock(TCLE.lockedfiles.First(x => x.Key.FullName == LoadedFile.FullName).Value, _saveJSON);

                if (playsound) TCLE.PlaySound("UIsave");

                foreach (WeifenLuo.WinFormsUI.Docking.IDockContent? dock in TCLE.Documents.Where(x => x.DockHandler.TabText.StartsWith(LoadedFile.Name))) {
                    if (dock.GetType() == typeof(Form_MasterEditor))
                        (dock as Form_MasterEditor).Reload();
                }
            }
        }
        #endregion
    }
}