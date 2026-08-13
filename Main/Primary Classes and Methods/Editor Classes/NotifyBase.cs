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

    /// <summary>
    /// Extension methods for <see cref="System.ComponentModel.BindingList{T}"/>.
    /// </summary>
    public static class BindingListExtensions
    {
        /// <summary>
        /// Adds the elements of the specified collection to the end of the <see cref="System.ComponentModel.BindingList{T}"/>,
        /// while only firing the <see cref="System.ComponentModel.BindingList{T}.ListChanged"/>-event once.
        /// </summary>
        /// <typeparam name="T">
        /// The type T of the values of the <see cref="System.ComponentModel.BindingList{T}"/>.
        /// </typeparam>
        /// <param name="bindingList">
        /// The <see cref="System.ComponentModel.BindingList{T}"/> to which the values shall be added.
        /// </param>
        /// <param name="collection">
        /// The collection whose elements should be added to the end of the <see cref="System.ComponentModel.BindingList{T}"/>.
        /// The collection itself cannot be null, but it can contain elements that are null,
        /// if type T is a reference type.
        /// </param>
        /// <exception cref="ArgumentNullException">values is null.</exception>
        public static void AddRange<T>(this System.ComponentModel.BindingList<T> bindingList, IEnumerable<T> collection)
        {
            // The given collection may not be null.
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            // Remember the current setting for RaiseListChangedEvents
            // (if it was already deactivated, we shouldn't activate it after adding!).
            bool oldRaiseEventsValue = bindingList.RaiseListChangedEvents;

            // Try adding all of the elements to the binding list.
            try {
                bindingList.RaiseListChangedEvents = false;

                foreach (T? value in collection)
                    bindingList.Add(value);
            }

            // Restore the old setting for RaiseListChangedEvents (even if there was an exception),
            // and fire the ListChanged-event once (if RaiseListChangedEvents is activated).
            finally {
                bindingList.RaiseListChangedEvents = oldRaiseEventsValue;

                if (bindingList.RaiseListChangedEvents)
                    bindingList.ResetBindings();
            }
        }
    }
}
