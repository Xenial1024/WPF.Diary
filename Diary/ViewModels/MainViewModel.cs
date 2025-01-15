using Diary.Commands;
using Diary.Models.Domains;
using Diary.Models.Wrappers;
using Diary.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private readonly Repository _repository = new Repository();
        internal static bool WasConnectionToDatabaseValid; //Tymczasowe pole potrzebne po to, żeby w przypadku braku połączenia, w oknie DbSettingsView po kliknięciu przycisku "Anuluj" nie musieć czekać na timeout połączenia z bazą danych, tylko żeby aplikacja natychmiast się zamknęła.
        private StudentWrapper _selectedStudent;
        private ObservableCollection<StudentWrapper> _students;
        private int _selectedGroupId;
        private ObservableCollection<Group> _group;

        public MainViewModel()
        {
            AddStudentCommand = new RelayCommand(AddEditStudent);
            EditStudentCommand = new RelayCommand(AddEditStudent,
                CanEditDeleteStudent);
            DeleteStudentCommand = new AsyncRelayCommand(DeleteStudent,
                CanEditDeleteStudent);
            RefreshStudentsCommand = new RelayCommand(RefreshStudents);
            LoadedWindowCommand = new RelayCommand(LoadedWindow);
            SettingsCommand = new RelayCommand(OpenSettings);
            OnClosingCommand = new RelayCommand(RememberStateOfWindow);
            LoadedWindow(null);
        }

        public ICommand AddStudentCommand { get; set; }
        public ICommand EditStudentCommand { get; set; }
        public ICommand DeleteStudentCommand { get; set; }
        public ICommand RefreshStudentsCommand { get; set; }
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand OnClosingCommand { get; set; }

        public StudentWrapper SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<StudentWrapper> Students
        {
            get => _students;
            set
            {
                _students = value;
                OnPropertyChanged();
            }
        }

        public int SelectedGroupId
        {
            get => _selectedGroupId;
            set
            {
                _selectedGroupId = value;
                OnPropertyChanged();
                RefreshDiary();
            }
        }

        public ObservableCollection<Group> Groups
        {
            get => _group;
            set
            {
                _group = value;
                OnPropertyChanged();
            }
        }

        private void RememberStateOfWindow(object obj)
        {
            var windowState = obj as WindowState?;
            if (windowState.HasValue)
            {
                SettingsWrapper _settingsWrapper = new SettingsWrapper();
                _settingsWrapper.IsMaximized = windowState.Value == WindowState.Maximized;
            }
        }

        private void OpenSettings(object obj)
        {
            DbSettingsView dbSettingsView = new DbSettingsView();
            dbSettingsView.ShowDialog();
        }

        private async void LoadedWindow(object obj)
        {
            if (!IsConnectionToDatabaseValid())
            {
                if (Application.Current.MainWindow is MetroWindow metroWindow)
                {
                    var dialog = await metroWindow.ShowMessageAsync(
                "Błąd połączenia",
                "Nie udało się połączyć z bazą danych. Czy chcesz zmienić ustawienia?",
                MessageDialogStyle.AffirmativeAndNegative,
                    new MetroDialogSettings
                    {
                        AffirmativeButtonText = "Tak",
                        NegativeButtonText = "Nie"
                    });

                    if (dialog != MessageDialogResult.Affirmative)
                        Application.Current.Shutdown();
                    DbSettingsView dbSettingsView = new DbSettingsView();
                    dbSettingsView.ShowDialog();
                }
            }
            else
            {
                WasConnectionToDatabaseValid = true;
                RefreshDiary();
                InitGroups();
            }
        }

        public bool IsConnectionToDatabaseValid()
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    context.Database.Connection.Open();
                    context.Database.Connection.Close();
                    return true;
                }
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Nieudane połączenie z bazą danych: {ex.Message}"); 
                return false;
            }
        }

        private void RefreshStudents(object obj) => RefreshDiary();

        private bool CanEditDeleteStudent(object obj) =>SelectedStudent != null;

        private async Task DeleteStudent(object obj)
        {
            MetroWindow metroWindow = Application.Current.MainWindow as MetroWindow;
            var dialog = await metroWindow.ShowMessageAsync(
                "Usuwanie ucznia",
                $"Czy na pewno chcesz usunąć ucznia {SelectedStudent.FirstName} {SelectedStudent.LastName}?",
                MessageDialogStyle.AffirmativeAndNegative,
                    new MetroDialogSettings
                    {
                        AffirmativeButtonText = "Tak",
                        NegativeButtonText = "Nie"
                    }); 

            if (dialog != MessageDialogResult.Affirmative)
                return;
            _repository.DeleteStudent(SelectedStudent.Id);
            RefreshDiary();
        }

        private void AddEditStudent(object obj)
        {
            AddEditStudentView addEditStudentWindow = new AddEditStudentView(obj as StudentWrapper);
            addEditStudentWindow.Closed += AddEditStudentWindow_Closed;
            addEditStudentWindow.ShowDialog();
        }

        private void AddEditStudentWindow_Closed(object sender, EventArgs e) => RefreshDiary();

        private void InitGroups()
        {
            var groups = _repository.GetGroups();
            groups.Insert(0, new Group { Id = 0, Name = "Wszystkie" });
            Groups = new ObservableCollection<Group>(groups);
            SelectedGroupId = 0;
        }

        private void RefreshDiary() 
            => Students = new ObservableCollection<StudentWrapper>(_repository.GetStudents(SelectedGroupId));
    }
}
