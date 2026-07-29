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

        public List<SaveState> UndoList = new();
        public bool RawText { get; set; }
        public bool NoLock { get; set; }
        public FileInfo WorkingFile { 
            get => _workfile; 
            set {
                if (!NoLock && value != null && _workfile != null) {
                    TCLE.Documents.Remove(_workfile.Name + (RawText ? "-raw" : ""));
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
        public FileStream FileLock { get; set; }
        public bool Saved { get; set; }

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

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
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
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                FileLock?.Dispose();
                FileLock = null;
            }
            base.Dispose(disposing);
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
