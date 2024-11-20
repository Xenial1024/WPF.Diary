using Diary.Models.Wrappers;
using Diary.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Diary.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        Settings _settings = new Settings();
        SettingsWrapper _settingsWrapper = new SettingsWrapper();

        public MainWindow()
        {
            InitializeComponent();

            _settingsWrapper = (SettingsWrapper)Resources["SettingsWrapper"];
            if (_settingsWrapper != null)
            {
                if (_settingsWrapper.IsMaximized)
                {
                    WindowState = WindowState.Maximized;
                }
            }
            this.Closing += MainWindow_Closing;
            DataContext = new MainViewModel();
        }


        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            _settings.Show();
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_settingsWrapper != null)
                _settingsWrapper.IsMaximized = (WindowState == WindowState.Maximized);
            Application.Current.Shutdown();
        }
    }
}
