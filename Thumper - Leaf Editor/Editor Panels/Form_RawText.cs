using FastColoredTextBoxNS.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Input;

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
        private FileInfo loadedfile { get { return LoadedFile; } set { LoadedFile = value; } }
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
            SaveCheckAndWrite(true);
        }

        public void SaveCheckAndWrite(bool IsSaved, bool playsound = false)
        {
            //make the beeble emote
            TCLE.beeble.MakeFace();

            EditorIsSaved = IsSaved;
            if (!IsSaved) {
                //denote editor tab is not saved
                if (!this.Text.EndsWith('*')) this.Text += "*";
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
                //write JSON to file
                //TCLE.WriteFileLock(TCLE.lockedfiles[LoadedFile], _saveJSON);
                TCLE.WriteFileLock(TCLE.lockedfiles.First(x => x.Key.FullName == LoadedFile.FullName).Value, _saveJSON);

                if (playsound) TCLE.PlaySound("UIsave");

                foreach (var dock in TCLE.Documents.Where(x => x.DockHandler.TabText.Contains(LoadedFile.Name))) {
                    if (dock.GetType() == typeof(Form_MasterEditor))
                        (dock as Form_MasterEditor).ReloadMaster();
                }
            }
        }
        #endregion

        private void textEditor_UndoRedoStateChanged(object sender, EventArgs e)
        {

        }
    }
}

static class extentions
{
    public static List<Variance> DetailedCompare<T>(this T val1, T val2)
    {
        List<Variance> variances = new List<Variance>();
        FieldInfo[] fi = val1.GetType().GetFields();
        foreach (FieldInfo f in fi) {
            Variance v = new Variance();
            v.Prop = f.Name;
            v.valA = f.GetValue(val1);
            v.valB = f.GetValue(val2);
            if (!Equals(v.valA, v.valB))
                variances.Add(v);

        }
        return variances;
    }


}
class Variance
{
    public string Prop { get; set; }
    public object valA { get; set; }
    public object valB { get; set; }
}