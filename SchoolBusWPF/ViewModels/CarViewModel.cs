using Microsoft.EntityFrameworkCore;
using SCHOOL_BUS.Commands;
using SchoolBusWPF.Models.Concretes;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolBusWPF.ViewModels
{
    public class CarViewModel : ViewModelBase
    {
        private Car _carData;
        public Car CarData
        {
            get { return _carData; }
            set
            {
                _carData = value;
                OnPropertyChanged(nameof(CarData));
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

        private string? _name;
        [Required]
        public string? Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
                ValidateProperty(nameof(Name), value, "Name is required.");
            }
        }

        private string? _plateNumber;
        [Required]
        public string? PlateNumber
        {
            get { return _plateNumber; }
            set
            {
                _plateNumber = value;
                OnPropertyChanged(nameof(PlateNumber));
                ValidateProperty(nameof(PlateNumber), value, "Plate number is required.");

                if (!string.IsNullOrEmpty(PlateNumber) && PlateNumber.Length < 7)
                    AddError(nameof(PlateNumber), "Plate number is not valid.");
            }
        }

        private string? _seatCount;
        [Required]
        public string? SeatCount
        {
            get { return _seatCount; }
            set
            {
                _seatCount = value;
                OnPropertyChanged(nameof(SeatCount));
                ValidateProperty(nameof(SeatCount), value, "Seat count is required.");
            }
        }

        private Driver? _driver;
        [Required]
        public Driver? Driver
        {
            get { return _driver; }
            set
            {
                _driver = value;
                OnPropertyChanged(nameof(Driver));
                ValidateProperty(nameof(Driver), value, "Driver is required.");
            }
        }

        public CarViewModel()
        {
            Cars = new ObservableCollection<Car>([.. _dbContext.Cars]);
            Drivers = new ObservableCollection<Driver>([.. _dbContext.Drivers]);

            OpenPopupCommand = new RelayCommand(OpenPopup);
            SaveChangesCommand = new RelayCommand(SaveChanges, CanSaveChanges);
            ClosePopupCommand = new RelayCommand(ClosePopup);
            UpdateEntityCommand = new RelayCommand(UpdateEntity);
        }

        public override void OpenPopup(object obj)
        {
            try
            {
                if (obj is null || obj is not Car objAsCar)
                    CarData = new();
                else
                    CarData = objAsCar;

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
                if (Driver is null)
                    return;

                var objAsCar = (obj as Car)!;

                if (!IsUpdate && CarExists(PlateNumber))
                {
                    Console.WriteLine($"The car with \"{PlateNumber}\" user name is already available");
                    return;
                }

                if (objAsCar.Id == 0)
                {
                    var car = new Car()
                    {
                        Name = Name,
                        PlateNumber = $"{PlateNumber?[..2]} {PlateNumber?.Substring(2, 2).ToUpper()} {PlateNumber?.Substring(4, 3)}",
                        SeatCount = int.TryParse(SeatCount, out int seatCount) ? seatCount : null,
                        DriverId = Driver.Id
                    };

                    _dbContext.Cars.Add(car);
                }
                else
                {
                    var car = _dbContext.Cars.FirstOrDefault(c => c.Id == objAsCar.Id)!;

                    car.Name = Name;
                    car.PlateNumber = $"{PlateNumber?[..2]} {PlateNumber?.Substring(2, 2).ToUpper()} {PlateNumber?.Substring(4, 3)}";
                    car.SeatCount = int.TryParse(SeatCount, out int seatCount) ? seatCount : null;
                    car.DriverId = Driver.Id;

                    _dbContext.Entry(car).State = EntityState.Modified;
                }

                _dbContext.SaveChanges();
                Cars = new ObservableCollection<Car>([.. _dbContext.Cars]);
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

            Name = string.Empty;
            PlateNumber = string.Empty;
            SeatCount = string.Empty;
            Driver = null;

            IsUpdate = false;
            ClearAllErrors();
        }

        public override void UpdateEntity(object obj)
        {
            var objAsCar = (obj as Car)!;

            Name = objAsCar.Name;
            PlateNumber = $"{objAsCar.PlateNumber?[..2]}{objAsCar.PlateNumber?.Substring(3, 2)}{objAsCar.PlateNumber?.Substring(6, 3)}";
            SeatCount = objAsCar.SeatCount.ToString();
            Driver = objAsCar.Driver;

            IsUpdate = true;
            OpenPopup(obj);
        }

        public override void DeleteEntity(object obj)
        {
            var objAsCar = (obj as Car)!;
            var car = _dbContext.Cars.FirstOrDefault(c => c.Id == objAsCar.Id)!;

            _dbContext.Cars.Remove(car);
            _dbContext.SaveChanges();

            Cars = new ObservableCollection<Car>([.. _dbContext.Cars]);
        }

        private bool CarExists(string? plateNumber)
        {
            return _dbContext.Cars.Any(c => c.PlateNumber == plateNumber);
        }

        public void SearchData(string pattern)
        {
            Cars = new ObservableCollection<Car>([.. _dbContext.Cars.Where(c => c.Name!.Contains(pattern.ToLower()))]);
        }
    }
}
