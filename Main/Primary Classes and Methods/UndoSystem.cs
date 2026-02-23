using Thumper_Custom_Level_Editor.Editor_Panels;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;

namespace Thumper_Custom_Level_Editor
{
    public class SaveState
    {
        public string reason { get; set; }
        public dynamic savestate { get; set; }
    }

    public static class UndoSystem
    {
        public static List<SaveState> UndoList;

        private static readonly ToolStripDropDownMenu undomenu = new() {
            BackColor = Color.FromArgb(40, 40, 40),
            ShowCheckMargin = false,
            ShowImageMargin = false,
            ShowItemToolTips = false,
            MaximumSize = new Size(2000, 500)
        };

        public static ToolStripDropDown CreateUndoMenu(List<SaveState> undolist)
        {
            undomenu.Items.Clear();
            UndoList = undolist;

            foreach (SaveState s in UndoList) {
                ToolStripMenuItem tmsi = new() {
                    Text = s.reason
                };
                tmsi.MouseEnter += undoMenu_MouseEnter;
                tmsi.Click += undoItem_Click;
                tmsi.BackColor = Color.FromArgb(40, 40, 40);
                tmsi.ForeColor = Color.White;
                undomenu.Items.Add(tmsi);
            }
            undomenu.Items.RemoveAt(undomenu.Items.Count - 1);
            return undomenu;
        }

        public static void undoMenu_MouseEnter(object sender, EventArgs e)
        {
            Color backcolor = Color.FromArgb(40, 40, 40);
            ToolStrip parent = ((ToolStripMenuItem)sender).Owner;
            for (int x = parent.Items.Count - 1; x >= 0; x--) {
                parent.Items[x].BackColor = backcolor;
                if (parent.Items[x] == sender)
                    backcolor = Color.Maroon;
            }
        }

        public static void undoItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tmsi = (ToolStripMenuItem)sender;
            int index = tmsi.Owner.Items.IndexOf(tmsi);
            UndoFunction(index + 1);
            UtilAudio.PlaySound("UIrevertchanges");
        }

        public static void UndoFunction(int undoindex)
        {
            if (TCLE.GlobalActiveDocument is EditorRawText)
                return;
            TCLE.GlobalActiveDocument.GetType().GetMethod("PerformUndo").Invoke(TCLE.GlobalActiveDocument, new object[] {undoindex});
        }

        public static void ClearReloadUndo(dynamic _load)
        {
            UndoList.Clear();
            UndoList.Insert(0, new SaveState() {
                reason = $"No changes",
                savestate = null
            });
        }
    }
}
