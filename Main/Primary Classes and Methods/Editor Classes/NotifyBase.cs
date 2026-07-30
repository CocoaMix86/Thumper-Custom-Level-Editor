using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Editor_Classes
{
    public class NotifyBase : INotifyPropertyChanged
    {
        ///
        ///Thanks to https://stackoverflow.com/questions/1315621/implementing-inotifypropertychanged-does-a-better-way-exist
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        ///
        ///
    }
}
