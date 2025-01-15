using Diary.Models.Wrappers;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;

namespace Diary.Views
{
    public partial class MainWindow : MetroWindow
    {

        readonly SettingsWrapper _settingsWrapper = new SettingsWrapper();

        public MainWindow()
        {
            InitializeComponent();

            _settingsWrapper = (SettingsWrapper)Resources["SettingsWrapper"];
            if (_settingsWrapper != null)
            {
                if (_settingsWrapper.IsMaximized)
                    WindowState = WindowState.Maximized;
            }
        }
    }
}
