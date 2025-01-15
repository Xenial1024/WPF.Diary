using System.ComponentModel;

namespace Diary.Models.Wrappers

{
    class SettingsWrapper : INotifyPropertyChanged
    {
        public static SettingsWrapper Instance { get; } = new SettingsWrapper();

        public bool IsMaximized
        {
            get => Settings.Default.IsMaximized;
            set
            {
                if (Settings.Default.IsMaximized != value)
                {
                    Settings.Default.IsMaximized = value;
                    OnPropertyChanged(nameof(IsMaximized));
                    Settings.Default.Save();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}