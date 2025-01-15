using Diary.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Diary.Models
{
    public class DbSettings : IDataErrorInfo, INotifyPropertyChanged
    {
        string _errorText;
        public event PropertyChangedEventHandler PropertyChanged;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(DbSettingsViewModel.ServerAddress):
                        if (string.IsNullOrWhiteSpace(DbSettingsViewModel.ServerAddress))
                        {
                            _errorText = "Pole \"Adres serwera\" nie może pozostać puste.";
                            return _errorText;
                        }
                        else
                        {
                            _errorText = String.Empty;
                            return string.Empty;
                        }

                    case nameof(DbSettingsViewModel.ServerName):
                        if (string.IsNullOrWhiteSpace(DbSettingsViewModel.ServerName))
                        {
                            _errorText = "Pole \"Nazwa serwera\" nie może pozostać puste.";
                            return _errorText;
                        }
                        else
                        {
                            _errorText = String.Empty;
                            return string.Empty;
                        }

                    case nameof(DbSettingsViewModel.DatabaseName):
                        if (string.IsNullOrWhiteSpace(DbSettingsViewModel.DatabaseName))
                        {
                            _errorText = "Pole \"Nazwa bazy danych\" nie może pozostać puste.";
                            return _errorText;
                        }
                        else
                        {
                            _errorText = String.Empty;
                            return string.Empty;
                        }

                    case nameof(DbSettingsViewModel.Username):
                        if (string.IsNullOrWhiteSpace(DbSettingsViewModel.Username) && !DbSettingsViewModel.AreSystemCredentialsUsed == true)
                        {
                            _errorText = "Pole \"Nazwa użytkownika\" nie może pozostać puste.";
                            return _errorText;
                        }
                        else
                        {
                            _errorText = String.Empty;
                            return string.Empty;
                        }

                    case nameof(DbSettingsViewModel.Password):
                        if (string.IsNullOrWhiteSpace(DbSettingsViewModel.Password) && !DbSettingsViewModel.AreSystemCredentialsUsed == true)
                        {
                            _errorText = "Pole \"Hasło\" nie może pozostać puste.";
                            return _errorText;
                        }
                        else
                        {
                            _errorText = String.Empty;
                            return string.Empty;
                        }
                    default:
                        return string.Empty;
                }
            }
        }
        public string Error => _errorText;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
        
        public bool IsEverySettingValid()
        {
            bool isValid = true;
            isValid &= string.IsNullOrWhiteSpace(this[nameof(DbSettingsViewModel.ServerAddress)]);
            isValid &= string.IsNullOrWhiteSpace(this[nameof(DbSettingsViewModel.ServerName)]);
            isValid &= string.IsNullOrWhiteSpace(this[nameof(DbSettingsViewModel.DatabaseName)]);
            if (DbSettingsViewModel.AreSystemCredentialsUsed)
                return isValid;
            isValid &= string.IsNullOrWhiteSpace(this[nameof(DbSettingsViewModel.Username)]);
            isValid &= string.IsNullOrWhiteSpace(this[nameof(DbSettingsViewModel.Password)]);
            return isValid;
        }
    }
}
