using Microsoft.EntityFrameworkCore;
using SCHOOL_BUS.Commands;
using SchoolBusWPF.Models.Concretes;
using SchoolBusWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace SchoolBusWPF.ViewModels
{
    public class DriverViewModel : ViewModelBase
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

        private string? _firstName = "";
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));

                Validate(nameof(FirstName), value);
            }
        }

        private string? _lastName = "";
        [Required(ErrorMessage = "Last name is required")]
        public string? LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));

                Validate(nameof(LastName), value);
            }
        }

        private string? _userName = "";
        [Required(ErrorMessage = "User name is required")]
        public string? UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));

                Validate(nameof(UserName), value);
            }
        }

        private string? _password = "";
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Invalid input.")]
        public string? Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));

                Validate(nameof(Password), value);
            }
        }

        private string? _phoneNumber = "";
        [Required(ErrorMessage = "Phone number is required")]
        [Length(9, 9, ErrorMessage = "Phone number length is incorrect")]
        public string? PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));

                Validate(nameof(PhoneNumber), value);
            }
        }

        private string? _licence = "";
        [Required(ErrorMessage = "License is required")]
        public string? Licence
        {
            get { return _licence; }
            set
            {
                _licence = value;
                OnPropertyChanged(nameof(Licence));

                Validate(nameof(Licence), value);
            }
        }

        private Car? _car;
        [Required(ErrorMessage = "Car is required")]
        public Car? Car
        {
            get { return _car; }
            set
            {
                _car = value;
                OnPropertyChanged(nameof(Car));

                Validate(nameof(Car), value);
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
            DeleteEntityCommand = new RelayCommand(DeleteEntity);
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
            return Validator.TryValidateObject(this, new ValidationContext(this), null);
        }

        public override void SaveChanges(object obj)
        {
            try
            {
                if (obj is not Driver objAsDriver || Car is null)
                    return;

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
                    var driver = _dbContext.Drivers.FirstOrDefault(d => d.Id == objAsDriver.Id);

                    if (driver is null)
                        return;

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
        }

        public override void UpdateEntity(object obj)
        {
            if (obj is null || obj is not Driver objAsDriver)
                return;

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
            if (obj is null || obj is not Driver objAsDriver)
                return;

            var driver = _dbContext.Drivers.FirstOrDefault(d => d.Id == objAsDriver.Id);

            if (driver is null)
                return;

            _dbContext.Drivers.Remove(driver);
            _dbContext.SaveChanges();
            Drivers = new ObservableCollection<Driver>([.. _dbContext.Drivers]);
        }

        private bool DriverExists(string? username)
        {
            return _dbContext.Drivers.Any(d => d.UserName == username);
        }
    }
}
