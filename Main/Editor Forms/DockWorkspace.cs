using System.Diagnostics;
using System.Windows.Controls;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class DockWorkspace : EditorBaseSub
    {
        private DeserializeDockContent m_deserializeDockContent;
        //Stopwatch sw = new();

        #region Form Construction
        public DockWorkspace(string configtoload)
        {
            InitializeComponent();
            this.Text = configtoload;
            this.TabText = configtoload;
            dockMain.Theme = new VS2015DarkTheme();
            dockMain.Theme.Extender.FloatWindowFactory = new CustomFloatWindowFactory();
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            if (TCLE.IsLoadingProject && !string.IsNullOrEmpty(configtoload)) {
                try {
                    dockMain.LoadFromXml($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{configtoload}.config", m_deserializeDockContent);
                } catch { }
            }

            //
            TCLE.Instance.toolStripWindowCloseWorkspace.Enabled = true;
            TCLE.Instance.toolstripWindowCloseAll.Enabled = true;
        }

        public override void ColorFormElements() { }

        private EditorBase? GetContentFromPersistString(string persistString)
        {
            persistString = persistString.Split(';')[1];
            bool raw = persistString.Contains(" [Raw]");
            persistString = persistString.Replace(" [Raw]", "");

            if (ProjectExplorer.TryGetFile(persistString, out FileInfo _toopen)) {//show the loading message
                TCLE.Instance.lblLoadingLvl.Text = $"Loading: {persistString}";
                TCLE.Instance.lblLoadingLvl.Invalidate();
                TCLE.Instance.lblLoadingLvl.Update();
                TCLE.Instance.lblLoadingLvl.Refresh();
                Application.DoEvents();
                //
                EditorBase _e = TCLE.OpenFile(_toopen, raw, true);
                return _e;
            }
            return null;
        }
        #endregion

        private void dockMain_ContentAdded(object sender, DockContentEventArgs e)
        {
            e.Content.DockHandler.TabPageContextMenuStrip = TCLE.TabRightClickMenu;
            if (TCLE.IsLoadingProject)
                return;

            TCLE.Instance.toolStripWindowCloseTab.Enabled = true;
            TCLE.Instance.toolstripWindowCloseEditors.Enabled = true;
            TCLE.Instance.toolStripMenuItem7.Enabled = true;
            TCLE.Instance.toolstripWindowCloseFiletype.Enabled = true;
            TCLE.Instance.toolstripWindowFloat.Enabled = true;
            TCLE.Instance.toolstripWindowFloatAll.Enabled = true;
            TCLE.Instance.toolstripWindowDock.Enabled = true;
        }

        private void dockMain_ContentRemoved(object sender, DockContentEventArgs e)
        {
            if (TCLE.IsLoadingProject || TCLE.Instance.Disposing)
                return;
        }

        private void dockMain_Enter(object sender, EventArgs e)
        {
            if (/*TCLE.DontSwitchGAD || */this.Disposing || TCLE.IsLoadingProject)
                return;
            if (dockMain.ActiveDocument is ProjectNotes)
                return;

            if (dockMain.ActiveDocument != null && TCLE.GlobalActiveDocument != dockMain.ActiveDocument)
                TCLE.GlobalActiveDocument = (EditorBase?)dockMain.ActiveDocument;
        }
        private void dockMain_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (/*TCLE.DontSwitchGAD || */this.Disposing || TCLE.IsLoadingProject)
                return;
            if (dockMain.ActiveDocument is ProjectNotes)
                return;

            if (dockMain.ActiveDocument != null && TCLE.GlobalActiveDocument != dockMain.ActiveDocument)
                TCLE.GlobalActiveDocument = (EditorBase?)dockMain.ActiveDocument;
        }
        private void dockMain_ActiveContentChanged(object sender, EventArgs e)
        {
            if (/*TCLE.DontSwitchGAD || */this.Disposing || TCLE.IsLoadingProject)
                return;
            if (dockMain.ActiveDocument is ProjectNotes)
                return;

            if (dockMain.ActiveDocument != null && TCLE.GlobalActiveDocument != dockMain.ActiveDocument)
                TCLE.GlobalActiveDocument = (EditorBase?)dockMain.ActiveDocument;
        }

        private void dockMain_ActivePaneChanged(object sender, EventArgs e)
        {
            if (/*TCLE.DontSwitchGAD || */this.Disposing || TCLE.IsLoadingProject)
                return;
            if (dockMain.ActiveContent is ProjectNotes)
                return;
            //if (dockMain.ActivePane == null) {
            //if (dockMain.ActiveContent == null)
            //if (dockMain.Panes.Count > 0)
            //    dockMain.Panes[0].Activate();
            if (dockMain.ActiveDocument != null && TCLE.GlobalActiveDocument != dockMain.ActiveDocument)
                TCLE.GlobalActiveDocument = (EditorBase?)dockMain.ActiveContent;
            //}
            //dockMain.SaveAsXml($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{this.Text}.config");
        }

        private void Form_WorkSpace_FormClosing(object sender, FormClosingEventArgs e)
        {
            //check for unsaved files
            if (TCLE.AnyUnsaved()) {
                if (MessageBox.Show("Some files in this workspace are unsaved. Do you still want to close this?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No) {
                    e.Cancel = true;
                    return;
                }
            }
            //When workspace closes, close the file lock on all the files inside it
            for (int x = 0; x < this.dockMain.Documents.Count(); x++) {
                if (!TCLE.Instance.Disposing)
                    this.dockMain.Documents.First().DockHandler.Close();
            }
        }

        private void Form_WorkSpace_FormClosed(object sender, FormClosedEventArgs e)
        {
            try {
                File.Delete($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{this.Text}.config");
            } catch { }

            if (TCLE.Workspaces.Count() == 1) {
                TCLE.Instance.toolStripWindowCloseTab.Enabled = false;
                TCLE.Instance.toolstripWindowCloseEditors.Enabled = false;
                TCLE.Instance.toolStripMenuItem7.Enabled = false;
                TCLE.Instance.toolstripWindowCloseFiletype.Enabled = false;
                TCLE.Instance.toolstripWindowFloat.Enabled = false;
                TCLE.Instance.toolstripWindowFloatAll.Enabled = false;
                TCLE.Instance.toolstripWindowDock.Enabled = false;
                //
                TCLE.Instance.toolStripWindowCloseWorkspace.Enabled = false;
                TCLE.Instance.toolstripWindowCloseAll.Enabled = false;
            }
        }
    }
}