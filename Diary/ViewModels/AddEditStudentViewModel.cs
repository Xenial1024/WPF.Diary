using Diary.Commands;
using Diary.Models.Domains;
using Diary.Models.Wrappers;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Diary.Views;

namespace Diary.ViewModels
{
    class AddEditStudentViewModel : ViewModelBase
    {
        private readonly Repository _repository = new Repository();
        private StudentWrapper _student;
        private bool _isUpdate;
        private int _selectedGroupId;
        private ObservableCollection<Group> _group;
        private readonly char[] _charsToDelete = { ' ', ',' };

        public AddEditStudentViewModel(StudentWrapper student = null)
        {
            CloseCommand = new RelayCommand(Close);
            ConfirmCommand = new RelayCommand(Confirm);

            if (student == null)
            {
                Student = new StudentWrapper();
            }
            else
            {
                Student = student;
                IsUpdate = true;
            }

            InitGroups();
        }

        public ICommand CloseCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public StudentWrapper Student
        {
            get => _student; 
            set
            {
                _student = value;
                OnPropertyChanged();
            }
        }
        public bool IsUpdate
        {
            get => _isUpdate;
            set
            {
                _isUpdate = value;
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

        private bool ValidateGrades(string subjectGrades, string subjectName, AddEditStudentView view)
        {
            if (!string.IsNullOrWhiteSpace(subjectGrades))
            {
                string cleanedGrades = subjectGrades.Trim(_charsToDelete).Replace(" ", ",");
                cleanedGrades = System.Text.RegularExpressions.Regex.Replace(cleanedGrades, @"(,)+", ",");
                if (!System.Text.RegularExpressions.Regex.IsMatch(cleanedGrades, @"^[0-6]([,\s][0-6])*$"))
                {
                    _ = view.ShowMessageAsync("Błąd", $"Oceny w przedmiocie {subjectName} muszą być liczbami całkowitymi od 0 do 6, oddzielonymi spacją lub przecinkiem.");
                    return false;
                }
            }
            return true;
        }

        private async void Confirm(object obj)
        {
            AddEditStudentView addEditStudentView = obj as AddEditStudentView;

            if (!Student.IsValid)
            {
                await addEditStudentView.ShowMessageAsync("Błąd", "Nie wprowadzono wymaganych danych.");
                return;
            }

            if (!ValidateGrades(Student.Math, "Matematyka", addEditStudentView) ||
                !ValidateGrades(Student.Physics, "Fizyka", addEditStudentView) ||
                !ValidateGrades(Student.PolishLang, "Język polski", addEditStudentView) ||
                !ValidateGrades(Student.ForeignLang, "Język obcy", addEditStudentView) ||
                !ValidateGrades(Student.Technology, "Technologia", addEditStudentView))
            {
                return;
            }

            if (IsUpdate)
                UpdateStudent();
            else
                AddStudent();

            CloseWindow(obj as Window);
        }

        private void UpdateStudent() => _repository.UpdateStudent(Student);

        private void AddStudent() => _repository.AddStudent(Student);

        private void Close(object obj) => CloseWindow(obj as Window);

        private void CloseWindow(Window window) => window.Close();

        private void InitGroups()
        {
            var groups = _repository.GetGroups();
            groups.Insert(0, new Group { Id = 0, Name = "-- brak --" });
            Groups = new ObservableCollection<Group>(groups);
            SelectedGroupId = Student.Group.Id;
        }
    }
}
