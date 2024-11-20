using Diary.Commands;
using Diary.Models.Domains;
using Diary.Models.Wrappers;
using MahApps.Metro.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Diary.Views;
using System.Linq;
using System.Windows.Media.Converters;


namespace Diary.ViewModels
{
    public class AddEditStudentViewModel : ViewModelBase
    {
        private Repository _repository = new Repository();

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


        private StudentWrapper _student;
        public StudentWrapper Student
        {
            get { return _student; }
            set
            {
                _student = value;
                OnPropertyChanged();
            }
        }

        private bool _isUpdate;
        public bool IsUpdate
        {
            get { return _isUpdate; }
            set
            {
                _isUpdate = value;
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

        private char[] _charsToDelete = { ' ', ',' };
        private async void Confirm(object obj)
        {
            ConfirmCommand = new RelayCommand(Confirm);
            AddEditStudentView addEditStudentView = obj as AddEditStudentView;
            if (!Student.IsValid)
            {
                await addEditStudentView.ShowMessageAsync("Błąd", "Nie wprowadzono wymaganych danych.");
                return;
            }
            //if (!int.TryParse(Student.Math, out _))
            if (!System.Text.RegularExpressions.Regex.IsMatch(Student.Math, @"^[0-6 ,]+$"))
            {
                await addEditStudentView.ShowMessageAsync("Błąd", "Ocena musi być liczbą całkowitą od 0 do 6. Oceny mogą być oddzielone spacją lub przecinkiem.");
                return;
            }
            Student.Math = Student.Math.Trim(_charsToDelete);
            Student.Math = Student.Math.Replace(' ', ',');
            Student.Math = System.Text.RegularExpressions.Regex.Replace(Student.Math, @"(,)+", ",");// Usunięcie nadmiarowych przecinków
            if (IsUpdate)
                UpdateStudent();
            else
                AddStudent();

            CloseWindow(obj as Window);
        }

        private void UpdateStudent()
        {
            _repository.UpdateStudent(Student);
        }

        private void AddStudent()
        {
            _repository.AddStudent(Student);
        }

        private void Close(object obj)
        {
            CloseWindow(obj as Window);
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }     

        private void InitGroups()
        {
            var groups = _repository.GetGroups();
            groups.Insert(0, new Group { Id = 0, Name = "-- brak --" });

            Groups = new ObservableCollection<Group>(groups);

            SelectedGroupId = Student.Group.Id;
        }
    }
}
