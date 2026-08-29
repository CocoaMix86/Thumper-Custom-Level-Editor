using System.ComponentModel;
using Thumper_Custom_Level_Editor.Editor_Panels;

namespace Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Editor_Classes
{
    public class EditorPropertiesBase
    {
        public EditorPropertiesBase() { }

        [Browsable(false)]
        public EditorBase ParentEditor;

        [CategoryAttribute("General")]
        [DisplayName("File Path")]
        [Description("The full filepath to this file.")]
        public string Filepath => this.ParentEditor.WorkingFile.FullName;

        [CategoryAttribute("Runtime")]
        [DisplayName("Beats")]
        [Description("Total number of beats this item spans.")]
        public int Beats { 
            get {
                if (ProjectExplorer.Files.TryGetValue(ParentEditor?.WorkingFile?.Name, out ProjectItem _item)) {
                    if (_beats != _item.Runtime) {
                        _beats = _item.Runtime;
                        Runtime = TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(Beats / (double)TCLE.BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");
                    }
                }
                return _beats;
            }/*
            set {
                if (_beats == value)
                    return;
                _beats = value;
                Runtime = TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(Beats / (double)TCLE.BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");
                if (ParentEditor != null && this.ParentEditor?.WorkingFile?.Name != null)
                    TCLE.CachedRuntimes[this.ParentEditor.WorkingFile.Name] = Beats;
            }*/
        }//> Leafs.Sum(x => x.Beats);
        private int _beats;

        [CategoryAttribute("Runtime")]
        [DisplayName("Runtime")]
        [Description("Calculated based on Beats and the current BPM. (Beats/BPM)")]
        public string _showruntime => Runtime;
        [Browsable(false)]
        public string Runtime { get; set; } 
    }
}
