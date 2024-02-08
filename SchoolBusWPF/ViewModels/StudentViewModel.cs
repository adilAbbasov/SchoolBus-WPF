using Microsoft.EntityFrameworkCore;
using SCHOOL_BUS.Commands;
using SchoolBusWPF.Models.Concretes;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolBusWPF.ViewModels
{
    public class StudentViewModel : ViewModelBase
    {
        private Student _studentData;
        public Student StudentData
        {
            get { return _studentData; }
            set
            {
                _studentData = value;
                OnPropertyChanged(nameof(StudentData));
            }
        }

        private ObservableCollection<Student> _students = [];
        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }

        private ObservableCollection<Group> _groups = [];
        public ObservableCollection<Group> Groups
        {
            get { return _groups; }
            set
            {
                _groups = value;
                OnPropertyChanged(nameof(Groups));
            }
        }

        private string? _firstName;
        [Required]
        public string? FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
                ValidateProperty(nameof(FirstName), value, "First name is required.");
            }
        }

        private string? _lastName;
        [Required]
        public string? LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
                ValidateProperty(nameof(LastName), value, "Last name is required.");
            }
        }

        private string? _homeAddress;
        [Required]
        public string? HomeAddress
        {
            get { return _homeAddress; }
            set
            {
                _homeAddress = value;
                OnPropertyChanged(nameof(HomeAddress));
                ValidateProperty(nameof(HomeAddress), value, "Home address is required.");
            }
        }

        private string? _otherAddress;
        public string? OtherAddress
        {
            get { return _otherAddress; }
            set
            {
                _otherAddress = value;
                OnPropertyChanged(nameof(OtherAddress));
            }
        }

        private string? _parentUserName;
        [Required]
        public string? ParentUserName
        {
            get { return _parentUserName; }
            set
            {
                _parentUserName = value;
                OnPropertyChanged(nameof(ParentUserName));
                ValidateProperty(nameof(ParentUserName), value, "Parent username is required.");

                var isLoaded = LoadParent(value);
                if (!isLoaded)
                    AddError(nameof(ParentUserName), "The username you entered does not match any parent.");
            }
        }

        private Parent? _parent;
        public Parent? Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                OnPropertyChanged(nameof(Parent));
            }
        }

        private Group? _group;
        [Required]
        public Group? Group
        {
            get { return _group; }
            set
            {
                _group = value;
                OnPropertyChanged(nameof(Group));
                ValidateProperty(nameof(Group), value, "Group is required.");
            }
        }

        public StudentViewModel()
        {
            Students = new ObservableCollection<Student>([.. _dbContext.Students.Include(s => s.Group).Include(s => s.Parent)]);
            Groups = new ObservableCollection<Group>([.. _dbContext.Groups]);

            OpenPopupCommand = new RelayCommand(OpenPopup);
            SaveChangesCommand = new RelayCommand(SaveChanges, CanSaveChanges);
            ClosePopupCommand = new RelayCommand(ClosePopup);
            UpdateEntityCommand = new RelayCommand(UpdateEntity);
        }

        public override void OpenPopup(object obj)
        {
            try
            {
                if (obj is null || obj is not Student objAsStudent)
                    StudentData = new();
                else
                    StudentData = objAsStudent;

                IsPopupOpen = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override bool CanSaveChanges(object obj)
        {
			return Validator.TryValidateObject(this, new ValidationContext(this), null) && !HasErrors;
		}

        public override void SaveChanges(object obj)
        {
            try
            {
                if (Group is null || Parent is null)
                    return;

                var objAsStudent = (obj as Student)!;

                if (objAsStudent.Id == 0)
                {
                    var student = new Student()
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        HomeAddress = HomeAddress,
                        OtherAddress = OtherAddress,
                        ParentId = Parent.Id,
                        GroupId = Group.Id
                    };

                    _dbContext.Students.Add(student);
                }
                else
                {
                    var student = _dbContext.Students.FirstOrDefault(s => s.Id == objAsStudent.Id)!;

                    student.FirstName = FirstName;
                    student.LastName = LastName;
                    student.HomeAddress = HomeAddress;
                    student.OtherAddress = OtherAddress;
                    student.ParentId = Parent.Id;
                    student.GroupId = Group.Id;

                    _dbContext.Entry(student).State = EntityState.Modified;
                }

                _dbContext.SaveChanges();
                Students = new ObservableCollection<Student>([.. _dbContext.Students]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                ClosePopup(obj);
            }
        }

        public override void ClosePopup(object obj)
        {
            IsPopupOpen = false;

            FirstName = string.Empty;
            LastName = string.Empty;
            HomeAddress = string.Empty;
            OtherAddress = string.Empty;
            ParentUserName = string.Empty;
            Group = null;

            ClearAllErrors();
        }

        public override void UpdateEntity(object obj)
        {
            var objAsStudent = (obj as Student)!;

            FirstName = objAsStudent.FirstName;
            LastName = objAsStudent.LastName;
            HomeAddress = objAsStudent.HomeAddress;
            OtherAddress = objAsStudent.OtherAddress;
            ParentUserName = objAsStudent.Parent?.UserName;
            Group = objAsStudent.Group;

            OpenPopup(obj);
        }

        public override void DeleteEntity(object obj)
        {
            var objAsStudent = (obj as Student)!;
            var student = _dbContext.Students.FirstOrDefault(s => s.Id == objAsStudent.Id)!;

            _dbContext.Students.Remove(student);
            _dbContext.SaveChanges();

            Students = new ObservableCollection<Student>([.. _dbContext.Students]);
        }

        private bool LoadParent(string? parentUserName)
        {
            if (!string.IsNullOrEmpty(parentUserName))
            {
                Parent = _dbContext.Parents.FirstOrDefault(p => p.UserName == parentUserName);

                if (Parent is null)
                    return false;
            }
            else
                Parent = null;

            return true;
        }

        public void SearchData(string pattern)
        {
            Students = new ObservableCollection<Student>([.. _dbContext.Students.Include(s => s.Group).Include(s => s.Parent).Where(s => s.FirstName!.Contains(pattern.ToLower()) || s.LastName!.Contains(pattern.ToLower()))]);
        }
    }
}
