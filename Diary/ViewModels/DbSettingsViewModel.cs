using Diary.Commands;
using Diary.Models;
using Diary.Views;
using MahApps.Metro.Controls.Dialogs;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModels
{
    class DbSettingsViewModel : ViewModelBase
    {
        //Tymczasowa krotka stworzona po to, żeby w przypadku zamknięcia okna ustawień w inny sposób niż przyciskiem "Zatwierdź", ustawienia w Settings.settings nie zostały zapisane. Dopiero po kliknięciu "Zatwierdź" dane z tej krotki zostaną przeniesione do Settings.settings.
        static (string serverAddress, string serverName, string databaseName, string username, string password, bool areSystemCredentialsUsed) _settingsBeforeConfirmation;

        public static string ServerAddress
        {
            get => _settingsBeforeConfirmation.serverAddress; 
            set => _settingsBeforeConfirmation.serverAddress = value; 
        }
        public static string ServerName
        {
            get => _settingsBeforeConfirmation.serverName; 
            set => _settingsBeforeConfirmation.serverName = value; 
        }
        public static string DatabaseName
        {
            get => _settingsBeforeConfirmation.databaseName;
            set => _settingsBeforeConfirmation.databaseName = value;
        }
        public static string Username
        {
            get => _settingsBeforeConfirmation.username;
            set => _settingsBeforeConfirmation.username = value;
        }
        public static string Password
        {
            get => _settingsBeforeConfirmation.password;
            set => _settingsBeforeConfirmation.password = value;
        }
        public static bool AreSystemCredentialsUsed
        {
            get => _settingsBeforeConfirmation.areSystemCredentialsUsed;
            set => _settingsBeforeConfirmation.areSystemCredentialsUsed = value; 
        }

        DbSettings _dbSettings;
        readonly DbSettingsView _dbSettingsView;

        private static void RestoreLastSettings()
        {
            ServerAddress = Settings.Default.ServerAddress;
            ServerName = Settings.Default.ServerName;
            DatabaseName = Settings.Default.DatabaseName;
            Username = Settings.Default.Username;
            Password = Settings.Default.Password;
            AreSystemCredentialsUsed = Settings.Default.AreSystemCredentialsUsed;
        }

        public DbSettingsViewModel(DbSettings dbSettings, DbSettingsView view)
        {
            RestoreLastSettings();
            Instance = this; 
            _dbSettingsView = view;
            DbSettings = dbSettings ?? new DbSettings();
            ResetServerAddressCommand = new RelayCommand(ResetServerAddress);
            ResetServerNameCommand = new RelayCommand(ResetServerName);
            ResetDatabaseNameCommand = new RelayCommand(ResetDatabaseName);
            CloseCommand = new RelayCommand(Close);
            ConfirmCommand = new RelayCommand(Confirm);
            ShowMessageAboutInvalidDataCommand = new RelayCommand(ShowMessageAboutInvalidData);
            _dbSettingsView.Closing += DbSettingsView_Closing;
        }
        public static DbSettingsViewModel Instance { get; private set; }
        public DbSettings DbSettings
        {
            get => _dbSettings;
            set
            {
                _dbSettings = value;
                OnPropertyChanged();
            }
        }
        public ICommand ResetServerAddressCommand { get; set; }
        public ICommand ResetServerNameCommand { get; set; }
        public ICommand ResetDatabaseNameCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand ShowMessageAboutInvalidDataCommand { get; set; }

        private void DbSettingsView_Closing(object sender, CancelEventArgs e)
        {
            if (!MainViewModel.WasConnectionToDatabaseValid)
                Application.Current.Shutdown();
        }

        private void ResetServerAddress(object obj)
        {
            ServerAddress = "(local)";
            OnStaticPropertyChanged(nameof(ServerAddress));
        }

        private void ResetServerName(object obj)
        {
            ServerName = "SQLEXPRESS";
            OnStaticPropertyChanged(nameof(ServerName));
        }

        private void ResetDatabaseName(object obj)
        {
            DatabaseName = "Diary";
            OnStaticPropertyChanged(nameof(DatabaseName));
        }

        private async void ShowMessageAboutInvalidData(object obj)
        => await _dbSettingsView.ShowMessageAsync("Błąd", "Nie wprowadzono wymaganych danych.");

        private void Confirm(object obj)
        {
            Settings.Default.Save(); 
            if (!DbSettings.IsEverySettingValid())
                ShowMessageAboutInvalidData(null);
            else
            {
                Settings.Default.ServerAddress = ServerAddress;
                Settings.Default.ServerName = ServerName;
                Settings.Default.DatabaseName = DatabaseName;
                Settings.Default.Username = Username;
                Settings.Default.Password = Password;
                Settings.Default.AreSystemCredentialsUsed = AreSystemCredentialsUsed;
                Settings.Default.Save();
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
        }
        private void Close(object obj)
        {
            if (!MainViewModel.WasConnectionToDatabaseValid)
                Application.Current.Shutdown();
            CloseWindow(obj as Window);
        }
        private void CloseWindow(Window window) => window.Close();
    }
}
