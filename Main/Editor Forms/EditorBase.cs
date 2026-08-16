using System.ComponentModel;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public class EditorBase : DockContent
    {
        public EditorBase() { }
        public EditorBase(FileInfo _filetolock, bool rawtext = false, bool nolock = false)
        {
            if (_filetolock == null)
                return;
            NoLock = nolock;
            RawText = rawtext;
            WorkingFile = _filetolock;
        }

        public bool SimpleLoad;
        public bool LogUndo = true;
        public bool EditorIsLoading;
        public bool EditorIsRandomizing;
        public bool EditorIsMoving;
        public bool EditorIsFinding;
        public bool EditorIsPasting;
        public bool EditorIsInterpolating;
        public bool EditorIsTuning;
        public bool EditorIsProcessing => (EditorIsLoading || EditorIsRandomizing || EditorIsMoving || EditorIsFinding || EditorIsPasting || EditorIsInterpolating || EditorIsTuning);

        public List<SaveState> UndoList = new();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RawText { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool NoLock { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileInfo WorkingFile { 
            get => _workfile; 
            set {
                if (!NoLock && value != null && _workfile != null) {
                    TCLE.Documents.Remove(_workfile.Name + (RawText ? "-raw" : ""));
                }
                if (value == null) {
                    TCLE.Documents.Remove(_workfile.Name + (RawText ? "-raw" : ""));
                    FileLock?.Close();
                }

                _workfile = value;

                if (value != null && !NoLock) {
                    FileLock?.Close();
                    TCLE.Documents.TryAdd(value.Name + (RawText ? "-raw" : ""), this);
                    try {
                        FileLock = new FileStream(_workfile.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    } catch (Exception) {
                        FileLock = null;
                    }
                }
            } 
        }
        private FileInfo _workfile;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileStream FileLock { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Saved { get; set; } = true;

        protected override string GetPersistString()
        {
            return base.GetPersistString() + ";" + (this.TabText ?? this.Text).Replace("*", "");
        }

        public virtual void ColorFormElements() { }
        public virtual void Save(bool playsound) { }
        public virtual FileInfo SaveAs(bool FileIsNew, string InitialDir) { return null; }
        public virtual void Copy() { }
        public virtual void Cut() { }
        public virtual void Paste() { }
        public virtual object GetProperties() { return null; }
        public virtual void PerformUndo(int undolistindex) { }
        public virtual void SaveCheckAndWrite(bool IsSaved, string Reason, bool playsound = false) { }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (this.WorkingFile is null)
                return;

            if (TCLE.GlobalLastGate == this)
                TCLE.GlobalLastGate = null;
            if (TCLE.GlobalLastLvl == this)
                TCLE.GlobalLastLvl = null;
            if (TCLE.GlobalLastMaster == this)
                TCLE.GlobalLastMaster = null;

            FileLock?.Close();
            TCLE.Documents.Remove(WorkingFile?.Name + (RawText ? "-raw" : ""));
            if (WorkingFile.Extension.Equals(".lvl", StringComparison.OrdinalIgnoreCase))
                TCLE.Documents.Remove(WorkingFile?.Name + " [Sequencer]");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                FileLock?.Dispose();
                FileLock = null;
            }
            base.Dispose(disposing);
        }

        public void ClearFileLock()
        {
            TCLE.Documents.Remove(_workfile.Name + (RawText ? "-raw" : ""));
            FileLock?.Close();
        }
    }

    public class CustomFloatWindow : FloatWindow
    {
        public CustomFloatWindow(DockPanel dockPanel, DockPane pane)
            : base(dockPanel, pane)
        {
            FormBorderStyle = FormBorderStyle.Sizable;
            this.Enter += Float_Enter;
            this.GotFocus += Float_Enter;
        }

        public CustomFloatWindow(DockPanel dockPanel, DockPane pane, Rectangle bounds)
            : base(dockPanel, pane, bounds)
        {
            FormBorderStyle = FormBorderStyle.Sizable;
            this.Enter += Float_Enter;
            this.GotFocus += Float_Enter;
        }

        public void Float_Enter(object sender, EventArgs e)
        {
            IDockContent focus = this.NestedPanes.Select(x => x.ActiveContent).FirstOrDefault();
            if (focus != null)
                TCLE.GlobalActiveDocument = (EditorBase?)focus;
        }
    }

    public class CustomFloatWindowFactory : DockPanelExtender.IFloatWindowFactory
    {
        public FloatWindow CreateFloatWindow(DockPanel dockPanel, DockPane pane, Rectangle bounds)
        {
            return new CustomFloatWindow(dockPanel, pane, bounds);
        }

        public FloatWindow CreateFloatWindow(DockPanel dockPanel, DockPane pane)
        {
            return new CustomFloatWindow(dockPanel, pane);
        }
    }
}
