using MahApps.Metro.Controls;
using System.Windows;
using Diary.ViewModels;
using Diary.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Diary.Views
{
    partial class DbSettingsView : MetroWindow
    {
        DbSettings _dbSettings;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event PropertyChangedEventHandler PropertyChanged;

        public DbSettings DbSettings
        {
            get => _dbSettings;
            set
            {
                _dbSettings = value;
                OnPropertyChanged();
            }
        }

        public DbSettingsView()
        {
            InitializeComponent();
            DataContext = new DbSettingsViewModel(new DbSettings(), this);
            EnableOrDisableUsernameAndPasswordTextBoxes(null, null);
        }

        //metody wpływające tylko na widok
        public void EnableOrDisableUsernameAndPasswordTextBoxes(object sender, RoutedEventArgs e)
        {
                tbUsername.IsEnabled = !DbSettingsViewModel.AreSystemCredentialsUsed;
                tbPassword.IsEnabled = !DbSettingsViewModel.AreSystemCredentialsUsed;
        }

        private void Reset_tbServerAddress(object sender, RoutedEventArgs e)
        => tbServerAddress.Text = "(local)";

        private void Reset_tbServerName(object sender, RoutedEventArgs e)
        => tbServerName.Text = "SQLEXPRESS";

        private void Reset_tbDatabaseName(object sender, RoutedEventArgs e)
        => tbDatabaseName.Text = "Diary";
    }
}
