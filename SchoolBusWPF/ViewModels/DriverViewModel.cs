﻿using Microsoft.EntityFrameworkCore;
using SCHOOL_BUS.Commands;
using SchoolBusWPF.Models.Concretes;
using SchoolBusWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SchoolBusWPF.ViewModels
{
    public partial class DriverViewModel : ViewModelBase
    {
        private Driver _driverData;
        public Driver DriverData
        {
            get { return _driverData; }
            set
            {
                _driverData = value;
                OnPropertyChanged(nameof(DriverData));
            }
        }

        private ObservableCollection<Driver> _drivers = [];
        public ObservableCollection<Driver> Drivers
        {
            get { return _drivers; }
            set
            {
                _drivers = value;
                OnPropertyChanged(nameof(Drivers));
            }
        }

        private ObservableCollection<Car> _cars = [];
        public ObservableCollection<Car> Cars
        {
            get { return _cars; }
            set
            {
                _cars = value;
                OnPropertyChanged(nameof(Cars));
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

        private string? _userName;
        [Required]
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
        [Required]
        public string? Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                ValidateProperty(nameof(Password), value, "Password is required.");

                if (!string.IsNullOrEmpty(Password) && !Regex.IsMatch(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$"))
                    AddError(nameof(Password), "Password is not valid.");
            }
        }

        private string? _phoneNumber;
        [Required]
        public string? PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
                ValidateProperty(nameof(PhoneNumber), value, "Phone number is required.");

                if (!string.IsNullOrEmpty(PhoneNumber) && PhoneNumber.Length < 9)
                    AddError(nameof(PhoneNumber), "Phone number is not valid.");
            }
        }

        private string? _licence;
        [Required]
        public string? Licence
        {
            get { return _licence; }
            set
            {
                _licence = value;
                OnPropertyChanged(nameof(Licence));
                ValidateProperty(nameof(Licence), value, "Licence is required.");
            }
        }

        private Car? _car;
        [Required]
        public Car? Car
        {
            get { return _car; }
            set
            {
                _car = value;
                OnPropertyChanged(nameof(Car));
                ValidateProperty(nameof(Car), value, "Car is required.");
            }
        }

        public DriverViewModel()
        {
            Drivers = new ObservableCollection<Driver>([.. _dbContext.Drivers]);
            Cars = new ObservableCollection<Car>([.. _dbContext.Cars]);

            OpenPopupCommand = new RelayCommand(OpenPopup);
            SaveChangesCommand = new RelayCommand(SaveChanges, CanSaveChanges);
            ClosePopupCommand = new RelayCommand(ClosePopup);
            UpdateEntityCommand = new RelayCommand(UpdateEntity);
        }

        public override void OpenPopup(object obj)
        {
            try
            {
                if (obj is null || obj is not Driver objAsDriver)
                    DriverData = new();
                else
                    DriverData = objAsDriver;

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
                if (Car is null)
                    return;

                var objAsDriver = (obj as Driver)!;

                if (!IsUpdate && DriverExists(UserName))
                {
                    Console.WriteLine($"The driver with \"{UserName}\" user name is already available");
                    return;
                }

                if (objAsDriver.Id == 0)
                {
                    var driver = new Driver()
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        UserName = UserName,
                        Password = PasswordService.EncodePassword(Password),
                        PhoneNumber = $"+994 {PhoneNumber?[..2]} {PhoneNumber?.Substring(2, 3)} {PhoneNumber?.Substring(5, 2)} {PhoneNumber?.Substring(7, 2)}",
                        Licence = Licence,
                        Car = Car,
                    };

                    _dbContext.Drivers.Add(driver);
                }
                else
                {
                    var driver = _dbContext.Drivers.FirstOrDefault(d => d.Id == objAsDriver.Id)!;

                    driver.FirstName = FirstName;
                    driver.LastName = LastName;
                    driver.UserName = UserName;
                    driver.Password = Password;
                    driver.PhoneNumber = PhoneNumber;
                    driver.Licence = Licence;
                    driver.Car = Car;

                    _dbContext.Entry(driver).State = EntityState.Modified;
                }

                _dbContext.SaveChanges();
                Drivers = new ObservableCollection<Driver>([.. _dbContext.Drivers]);
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
            Licence = string.Empty;
            Car = null;

            IsUpdate = false;
            ClearAllErrors();
        }

        public override void UpdateEntity(object obj)
        {
            var objAsDriver = (obj as Driver)!;

            FirstName = objAsDriver.FirstName;
            LastName = objAsDriver.LastName;
            UserName = objAsDriver.UserName;
            Password = PasswordService.DecodePassword(objAsDriver.Password);
            PhoneNumber = $"{objAsDriver.PhoneNumber?.Substring(5, 2)}{objAsDriver.PhoneNumber?.Substring(8, 3)}{objAsDriver.PhoneNumber?.Substring(12, 2)}{objAsDriver.PhoneNumber?.Substring(15, 2)}";
            Licence = objAsDriver.Licence;
            Car = objAsDriver.Car;

            IsUpdate = true;
            OpenPopup(obj);
        }

        public override void DeleteEntity(object obj)
        {
            var objAsDriver = (obj as Driver)!;
            var driver = _dbContext.Drivers.FirstOrDefault(d => d.Id == objAsDriver.Id)!;

            _dbContext.Drivers.Remove(driver);
            _dbContext.SaveChanges();

            Drivers = new ObservableCollection<Driver>([.. _dbContext.Drivers]);
        }

        private bool DriverExists(string? username)
        {
            return _dbContext.Drivers.Any(d => d.UserName == username);
        }

        public void SearchData(string pattern)
        {
            Drivers = new ObservableCollection<Driver>([.. _dbContext.Drivers.Where(d => d.FirstName!.Contains(pattern.ToLower()) || d.LastName!.Contains(pattern.ToLower()))]);
        }
    }
}
