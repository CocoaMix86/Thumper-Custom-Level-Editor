using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using Thumper_Custom_Level_Editor.Editor_Panels;

namespace Thumper_Custom_Level_Editor
{
    public class MasterProperties
    {
        public Form_MasterEditor parent;
        public MasterProperties(Form_MasterEditor Parent)
        {
            parent = Parent;
        }

        [CategoryAttribute("Options")]
        [DisplayName("Skybox")]
        [Description("")]
        public string skybox { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Intro Lvl")]
        [Description("This lvl will play at the beginning of your level, and whenever you restart.")]
        [Editor(typeof(LvlPicker), typeof(UITypeEditor))]
        public string introlvl { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Checkpoint Lvl")]
        [Description("This lvl will play immediately after each checkpoint.")]
        [Editor(typeof(LvlPicker), typeof(UITypeEditor))]
        public string checkpointlvl { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("BPM")]
        [Description("Beats Per Minute. If your song is at 100bpm, you'll likely want to map at either 200 or 400, so you can place objects on half note and quarter note intervals.")]
        public decimal bpm { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Rail Color")]
        [Description("Affects the rail color on the title screen.")]
        public Color rail { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Rail Glow Color")]
        [Description("Affects the rail color on the title screen.")]
        public Color railglow { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Path Color")]
        [Description("Affects the rail color on the title screen.")]
        public Color path { get; set; }

        [CategoryAttribute("Runtime")]
        [DisplayName("Beats")]
        [Description("Total number of beats across all lvls and gates included in the master.")]
        public int beats { get { return TCLE.CalculateMasterRuntime("", parent); } }

        [CategoryAttribute("Runtime")]
        [DisplayName("Runtime")]
        [Description("Calculated based on Beats and the current BPM. (Beats/BPM)")]
        public TimeSpan runtime { get { return  TimeSpan.FromMinutes(beats / (double)bpm); } }
    }

    class LvlPicker : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            string foo = value as string;
            if (svc != null && foo != null) {
                using (FileListBox form = new FileListBox(TCLE.lvlsinworkfolder)) {
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = new Point(Cursor.Position.X - 100, Cursor.Position.Y - 50);
                    if (svc.ShowDialog(form) == DialogResult.OK) {
                        return form.Value; // update object
                    }
                }
            }
            return value; // can also replace the wrapper object here
        }
    }
}
