using Microsoft.EntityFrameworkCore;
using SCHOOL_BUS.Commands;
using SchoolBusWPF.Models.Concretes;
using SchoolBusWPF.Services;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace SchoolBusWPF.ViewModels
{
    public class ParentViewModel : ViewModelBase
    {
        private Parent _parentData;
        public Parent ParentData
        {
            get { return _parentData; }
            set
            {
                _parentData = value;
                OnPropertyChanged(nameof(ParentData));
            }
        }

        private ObservableCollection<Parent> _parents = [];
        public ObservableCollection<Parent> Parents
        {
            get { return _parents; }
            set
            {
                _parents = value;
                OnPropertyChanged(nameof(Parents));
            }
        }

        private string? _firstName;
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

        private string? _userName;
        public string? UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
                ValidateProperty(nameof(UserName), value, "User name is required.");
            }
        }

        private string? _password;
        public string? Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                ValidatePassword();
            }
        }

        private void ValidatePassword()
        {
            ClearErrors(nameof(Password));
            var pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";

            if (string.IsNullOrEmpty(Password))
                AddError(nameof(Password), "Password is required.");
            else if (!Regex.IsMatch(Password, pattern))
                AddError(nameof(Password), "Password is not valid.");
        }

        private string? _phoneNumber;
        public string? PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
                ValidatePhoneNumber();
            }
        }

        private void ValidatePhoneNumber()
        {
            ClearErrors(nameof(PhoneNumber));

            if (string.IsNullOrEmpty(PhoneNumber))
                AddError(nameof(PhoneNumber), "Phone number is required.");
            else if (PhoneNumber.Length < 9)
                AddError(nameof(PhoneNumber), "Phone number is not valid.");
        }

        public ParentViewModel()
        {
            Parents = new ObservableCollection<Parent>([.. _dbContext.Parents]);

            OpenPopupCommand = new RelayCommand(OpenPopup);
            SaveChangesCommand = new RelayCommand(SaveChanges, CanSaveChanges);
            ClosePopupCommand = new RelayCommand(ClosePopup);
            UpdateEntityCommand = new RelayCommand(UpdateEntity);
            DeleteEntityCommand = new RelayCommand(DeleteEntity);
        }

        public override void OpenPopup(object obj)
        {
            try
            {
                if (obj is null || obj is not Parent objAsParent)
                    ParentData = new();
                else
                    ParentData = objAsParent;

                IsPopupOpen = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override bool CanSaveChanges(object obj)
        {
            return !HasErrors;
        }

        public override void SaveChanges(object obj)
        {
            try
            {
                if (obj is not Parent objAsParent)
                    return;

                if (!IsUpdate && ParentExists(UserName))
                {
                    Console.WriteLine($"The parent with \"{UserName}\" user name is already available");
                    return;
                }

                if (objAsParent.Id == 0)
                {
                    var parent = new Parent()
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        UserName = UserName,
                        Password = PasswordService.EncodePassword(Password),
                        PhoneNumber = $"+994 {PhoneNumber?[..2]} {PhoneNumber?.Substring(2, 3)} {PhoneNumber?.Substring(5, 2)} {PhoneNumber?.Substring(7, 2)}"
                    };

                    _dbContext.Parents.Add(parent);
                }
                else
                {
                    var parent = _dbContext.Parents.FirstOrDefault(p => p.Id == objAsParent.Id);

                    if (parent is null)
                        return;

                    parent.FirstName = FirstName;
                    parent.LastName = LastName;
                    parent.UserName = UserName;
                    parent.Password = Password;
                    parent.PhoneNumber = $"+994 {PhoneNumber?[..2]} {PhoneNumber?.Substring(2, 3)} {PhoneNumber?.Substring(5, 2)} {PhoneNumber?.Substring(7, 2)}";

                    _dbContext.Entry(parent).State = EntityState.Modified;
                }

                _dbContext.SaveChanges();
                Parents = new ObservableCollection<Parent>([.. _dbContext.Parents]);
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
            UserName = string.Empty;
            Password = string.Empty;
            PhoneNumber = string.Empty;

            IsUpdate = false;
            ClearAllErrors();
        }

        public override void UpdateEntity(object obj)
        {
            if (obj is null || obj is not Parent objAsParent)
                return;

            FirstName = objAsParent.FirstName;
            LastName = objAsParent.LastName;
            UserName = objAsParent.UserName;
            Password = PasswordService.DecodePassword(objAsParent.Password);
            PhoneNumber = $"{objAsParent.PhoneNumber?.Substring(5, 2)}{objAsParent.PhoneNumber?.Substring(8, 3)}{objAsParent.PhoneNumber?.Substring(12, 2)}{objAsParent.PhoneNumber?.Substring(15, 2)}";

            IsUpdate = true;
            OpenPopup(obj);
        }

        public override void DeleteEntity(object obj)
        {
            if (obj is null || obj is not Parent objAsParent)
                return;

            var parent = _dbContext.Parents.FirstOrDefault(p => p.Id == objAsParent.Id);

            if (parent is null)
                return;

            _dbContext.Parents.Remove(parent);
            _dbContext.SaveChanges();
            Parents = new ObservableCollection<Parent>([.. _dbContext.Parents]);
        }

        private bool ParentExists(string? username)
        {
            return _dbContext.Parents.Any(p => p.UserName == username);
        }
    }
}
