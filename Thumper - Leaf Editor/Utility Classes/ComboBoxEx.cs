using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace Thumper_Custom_Level_Editor
{
    public class ComboBoxEx : ComboBox
    {
        private const int CB_GETCURSEL = 0x0147;
        private int listItem = -1;
        IntPtr listBoxHandle = IntPtr.Zero;

        public event EventHandler<ListItemSelectionChangedEventArgs> ListItemSelectionChanged;

        protected virtual void OnListItemSelectionChanged(ListItemSelectionChangedEventArgs e)
            => ListItemSelectionChanged?.Invoke(this, e);

        public ComboBoxEx() { }

        protected override void WndProc(ref Message m)
        {
            int selItem = -1;
            base.WndProc(ref m);

            switch (m.Msg) {
                case CB_GETCURSEL:
                    selItem = m.Result.ToInt32();
                    break;
                default:
                    // Add Case switches to handle other events
                    break;
            }
            if (listItem != selItem) {
                listItem = selItem;
                OnListItemSelectionChanged(new ListItemSelectionChangedEventArgs(
                    listItem, listItem < 0 ? string.Empty : GetItemText(Items[listItem]))
                );
            }
        }

        public class ListItemSelectionChangedEventArgs : EventArgs
        {
            public ListItemSelectionChangedEventArgs(int idx, string text)
            {
                ItemIndex = idx;
                ItemText = text;
            }
            public int ItemIndex { get; private set; }
            public string ItemText { get; private set; }
        }
    }
}
