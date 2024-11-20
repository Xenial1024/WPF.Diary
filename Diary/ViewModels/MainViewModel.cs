using Caliburn.Micro;
using Diary.Commands;
using Diary.Models;
using Diary.Models.Domains;
using Diary.Models.Wrappers;
using Diary.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        MetroWindow metroWindow = Application.Current.MainWindow as MetroWindow; 
        private Repository _repository = new Repository(); 
        public MainViewModel()
        {
            OpenDialogIfConnectionToDatabaseFailed();
            
            AddStudentCommand = new RelayCommand(AddEditStudent);
            EditStudentCommand = new RelayCommand(AddEditStudent, 
                CanEditDeleteStudent);
            DeleteStudentCommand = new AsyncRelayCommand(DeleteStudent, 
                CanEditDeleteStudent);
            RefreshStudentsCommand = new RelayCommand(RefreshStudents);

            
            RefreshDiary();
            InitGroups();

        }        

        void OpenSettingsWindow()
        {
            Settings settings = new Settings();
            settings.Show();
        }

        private async void OpenDialogIfConnectionToDatabaseFailed()
        {
            if (!IsConnectionToDatabaseValid())
            {
                var dialog = await metroWindow.ShowMessageAsync(
                "Błąd połączenia",
                "Nie udało się połączyć z bazą danych. Czy chcesz zmienić ustawienia?",
                MessageDialogStyle.AffirmativeAndNegative);

                if (dialog != MessageDialogResult.Affirmative)
                {
                    
                }
            }
        }

        public ICommand AddStudentCommand { get; set; }
        public ICommand EditStudentCommand { get; set; }
        public ICommand DeleteStudentCommand { get; set; }
        public ICommand RefreshStudentsCommand { get; set; }

        bool IsConnectionToDatabaseValid()
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

            catch (Exception)
            {
                return false;
            }
        }


        private StudentWrapper _selectedStudent;
        public StudentWrapper SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<StudentWrapper> _students;
        public ObservableCollection<StudentWrapper> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged();
            }
        }

        private int _selectedGroupId;

        public int SelectedGroupId
        {
            get { return _selectedGroupId; }
            set
            {
                _selectedGroupId = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Group> _group;
        public ObservableCollection<Group> Groups
        {
            get { return _group; }
            set
            {
                _group = value;
                OnPropertyChanged();
            }
        }


        private void RefreshStudents(object obj)
        {
            RefreshDiary();
        }


        private bool CanEditDeleteStudent(object obj)
        {
            return SelectedStudent != null;
        }

        private async Task DeleteStudent(object obj)
        {
            
            var dialog = await metroWindow.ShowMessageAsync(
                "Usuwanie ucznia", 
                $"Czy na pewno chcesz usunąć ucznia {SelectedStudent.FirstName} {SelectedStudent.LastName}?", 
                MessageDialogStyle.AffirmativeAndNegative);

            if (dialog != MessageDialogResult.Affirmative)
                return;

            _repository.DeleteStudent(SelectedStudent.Id);

            RefreshDiary();
        }

        private void AddEditStudent(object obj)
        {
            var addEditStudentWindow = new AddEditStudentView(obj as StudentWrapper);
            addEditStudentWindow.Closed += AddEditStudentWindow_Closed;
            addEditStudentWindow.ShowDialog();
        }

        private void AddEditStudentWindow_Closed(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void InitGroups()
        {
            var groups = _repository.GetGroups();
            groups.Insert(0, new Group { Id = 0, Name = "Wszystkie" });

            Groups = new ObservableCollection<Group>(groups);

            SelectedGroupId = 0;
        }

        private void RefreshDiary()
        {
            Students = new ObservableCollection<StudentWrapper>(
                _repository.GetStudents(SelectedGroupId));
        }

    }
}
