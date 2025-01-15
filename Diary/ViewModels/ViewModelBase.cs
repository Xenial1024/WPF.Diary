using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Diary.ViewModels
{
    internal class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

        protected static void OnStaticPropertyChanged(string propertyName)
        => StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
    }
}
