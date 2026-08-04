using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Windows.ApplicationModel.Activation;

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
            get => _beats;
            set {
                if (_beats == value)
                    return;
                _beats = value;
                Runtime = TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(Beats / (double)TCLE.BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");
                TCLE.CachedRuntimes[this.ParentEditor.WorkingFile.Name] = Beats;
            }
        }//> Leafs.Sum(x => x.Beats);
        private int _beats;

        [CategoryAttribute("Runtime")]
        [DisplayName("Runtime")]
        [Description("Calculated based on Beats and the current BPM. (Beats/BPM)")]
        public string Runtime { get; set; } 
    }
}
