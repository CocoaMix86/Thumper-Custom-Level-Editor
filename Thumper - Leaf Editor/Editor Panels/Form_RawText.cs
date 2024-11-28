using FastColoredTextBoxNS.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
                //if file is not locked, lock it
                if (!TCLE.lockedfiles.ContainsKey(LoadedFile)) {
                    TCLE.lockedfiles.Add(LoadedFile, new FileStream(LoadedFile.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
                }
                //write JSON to file
                TCLE.WriteFileLock(TCLE.lockedfiles[LoadedFile], _saveJSON);

                if (playsound) TCLE.PlaySound("UIsave");
            }
        }
        #endregion

        private void textEditor_UndoRedoStateChanged(object sender, EventArgs e)
        {

        }
    }
}
