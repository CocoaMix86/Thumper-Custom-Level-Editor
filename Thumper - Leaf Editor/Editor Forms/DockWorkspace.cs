using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class DockWorkspace : EditorBase
    {
        private DeserializeDockContent m_deserializeDockContent;

        #region Form Construction
        public DockWorkspace(string configtoload) : base(null)
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
        }

        private EditorBase? GetContentFromPersistString(string persistString)
        {
            persistString = persistString.Split(';')[1];
            bool raw = persistString.Contains(" [Raw]");
            persistString = persistString.Replace(" [Raw]", "");

            FileInfo _topopen = ProjectExplorer.Files.FirstOrDefault(x => x.Name.Equals(persistString, StringComparison.OrdinalIgnoreCase));
            if (_topopen != null)
                return TCLE.OpenFile(_topopen, raw, true);
            return null;

            throw new NotImplementedException();
        }
        #endregion

        private void dockMain_ContentAdded(object sender, DockContentEventArgs e)
        {
            e.Content.DockHandler.TabPageContextMenuStrip = TCLE.TabRightClickMenu;
            if (TCLE.IsLoadingProject)
                return;
            dockMain.SaveAsXml($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{this.Text}.config");
        }

        private void dockMain_ContentRemoved(object sender, DockContentEventArgs e)
        {
            if (TCLE.IsLoadingProject || TCLE.Instance.Disposing)
                return;
            //EditorBase DocClosing = (EditorBase)e.Content;
            //FileInfo filetoclose = null;
            //filetoclose = DocClosing.WorkingFile;
            dockMain.SaveAsXml($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{this.Text}.config");
            ///TCLE.ProjectExplorer.FindNode(filetoclose.Name, TCLE.ProjectExplorer.treeView1.Nodes[0].Nodes).ForeColor = Properties.Settings.Default.ColorProjExpText;

        }

        private void dockMain_Enter(object sender, EventArgs e)
        {
            if (/*TCLE.DontSwitchGAD || */this.Disposing || TCLE.IsLoadingProject)
                return;

            if (dockMain.ActiveDocument != null && TCLE.GlobalActiveDocument != dockMain.ActiveDocument)
                TCLE.GlobalActiveDocument = (EditorBase?)dockMain.ActiveDocument;
            
            dockMain.SaveAsXml($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{this.Text}.config");
        }
        private void dockMain_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (/*TCLE.DontSwitchGAD || */this.Disposing || TCLE.IsLoadingProject)
                return;

            if (dockMain.ActiveDocument != null && TCLE.GlobalActiveDocument != dockMain.ActiveDocument)
                TCLE.GlobalActiveDocument = (EditorBase?)dockMain.ActiveDocument;

            dockMain.SaveAsXml($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{this.Text}.config");
        }
        private void dockMain_ActiveContentChanged(object sender, EventArgs e)
        {
            if (/*TCLE.DontSwitchGAD || */this.Disposing || TCLE.IsLoadingProject)
                return;

            if (dockMain.ActiveDocument != null && TCLE.GlobalActiveDocument != dockMain.ActiveDocument)
                TCLE.GlobalActiveDocument = (EditorBase?)dockMain.ActiveDocument;
            
            dockMain.SaveAsXml($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{this.Text}.config");
        }

        private void dockMain_ActivePaneChanged(object sender, EventArgs e)
        {
            if (/*TCLE.DontSwitchGAD || */this.Disposing || TCLE.IsLoadingProject)
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
            /*
    foreach (EditorBase doc in this.dockMain.Documents) {
        FileInfo filetoclose = null;
        if (!TCLE.Instance.Disposing) {
            filetoclose = doc.WorkingFile;
            doc.Dispose();
            if (doc.GetType() == typeof(Form_MasterEditor))
                filetoclose = (doc as Form_MasterEditor).MasterProperties.LoadedMaster;
            else if (doc.GetType() == typeof(Form_GateEditor))
                filetoclose = (doc as Form_GateEditor).loadedgate;
            else if (doc.GetType() == typeof(Form_LvlEditor))
                filetoclose = (doc as Form_LvlEditor).loadedlvl;
            else if (doc.GetType() == typeof(Form_SampleEditor))
                filetoclose = (doc as Form_SampleEditor).loadedsample;
            else if (doc.GetType() == typeof(Form_LeafEditor))
                filetoclose = (doc as Form_LeafEditor).LeafProperties.LoadedLeaf;
            else if (doc.GetType() == typeof(Form_RawText))
                filetoclose = (doc as Form_RawText).loadedfile;

            if (filetoclose == null)
                continue;

            TCLE.CloseFileLock(filetoclose);
        }
        }
            */
        }

        private void Form_WorkSpace_FormClosed(object sender, FormClosedEventArgs e)
        {
            try {
                File.Delete($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{this.Text}.config");
            } catch { }
        }
    }
}