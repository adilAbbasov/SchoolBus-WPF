using Microsoft.EntityFrameworkCore;
using SCHOOL_BUS.Commands;
using SchoolBusWPF.Models.Concretes;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace SchoolBusWPF.ViewModels
{
    public class RideViewModel : ViewModelBase
    {
        private Ride _rideData;
        public Ride RideData
        {
            get { return _rideData; }
            set
            {
                _rideData = value;
                OnPropertyChanged(nameof(RideData));
            }
        }

        private ObservableCollection<Ride> _rides = [];
        public ObservableCollection<Ride> Rides
        {
            get { return _rides; }
            set
            {
                _rides = value;
                OnPropertyChanged(nameof(Rides));
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

        private ObservableCollection<Student> _selectedStudents = [];
        public ObservableCollection<Student> SelectedStudents
        {
            get { return _selectedStudents; }
            set
            {
                _selectedStudents = value;
                OnPropertyChanged(nameof(SelectedStudents));
            }
        }

        private bool _isStudentPopupOpen;
        public bool IsStudentPopupOpen
        {
            get { return _isStudentPopupOpen; }
            set
            {
                _isStudentPopupOpen = value;
                OnPropertyChanged(nameof(IsStudentPopupOpen));
            }
        }

        private bool _isInfoPopupOpen;
        public bool IsInfoPopupOpen
        {
            get { return _isInfoPopupOpen; }
            set
            {
                _isInfoPopupOpen = value;
                OnPropertyChanged(nameof(IsInfoPopupOpen));
            }
        }

        private string? _fullName;
        [Required]
        public string? FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
                ValidateProperty(nameof(FullName), value, "Full name is required.");
            }
        }

        private string? _type;
        public string? Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        private bool _isToggleChecked;
        public bool IsToggleChecked
        {
            get { return _isToggleChecked; }
            set
            {
                _isToggleChecked = value;
                Type = _isToggleChecked ? "School" : "Home";
                OnPropertyChanged(nameof(IsToggleChecked));
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
                LoadDriver();
            }
        }

        private Driver? _driver;
        public Driver? Driver
        {
            get { return _driver; }
            set
            {
                _driver = value;
                OnPropertyChanged(nameof(Driver));
            }
        }

        private int? _maxSeatCount;
        public int? MaxSeatCount
        {
            get { return _maxSeatCount; }
            set
            {
                _maxSeatCount = value;
                OnPropertyChanged(nameof(MaxSeatCount));
            }
        }

        private int? _selectedStudentCount;
        public int? SelectedStudentCount
        {
            get { return _selectedStudentCount; }
            set
            {
                _selectedStudentCount = value;
                OnPropertyChanged(nameof(SelectedStudentCount));
            }
        }

        private int? _rideId;
        public int? RideId
        {
            get { return _rideId; }
            set
            {
                _rideId = value;
                OnPropertyChanged(nameof(RideId));
            }
        }

        public ICommand OpenStudentPopupCommand { get; set; }
        public ICommand SaveStudentChangesCommand { get; set; }
        public ICommand CloseStudentPopupCommand { get; set; }
        public ICommand OpenInfoPopupCommand { get; set; }
        public ICommand SaveInfoChangesCommand { get; set; }
        public ICommand CloseInfoPopupCommand { get; set; }
        public ICommand RemoveStudentCommand { get; set; }

        public RideViewModel()
        {
            Rides = new ObservableCollection<Ride>([.. _dbContext.Rides.Include(r => r.Car).Include(r => r != null ? r.Driver : null)]);
           

            Cars = new ObservableCollection<Car>([.. _dbContext.Cars]);
            Students = new ObservableCollection<Student>([.. _dbContext.Students.Include(s => s.Group).Where(s => s.RideId == null)]);
            IsToggleChecked = false;

            OpenPopupCommand = new RelayCommand(OpenPopup);
            SaveChangesCommand = new RelayCommand(SaveChanges, CanSaveChanges);
            ClosePopupCommand = new RelayCommand(ClosePopup);
            UpdateEntityCommand = new RelayCommand(UpdateEntity);
            OpenStudentPopupCommand = new RelayCommand(OpenStudentPopup);
            SaveStudentChangesCommand = new RelayCommand(SaveStudentChanges, CanSaveStudentChanges);
            CloseStudentPopupCommand = new RelayCommand(CloseStudentPopup);
            OpenInfoPopupCommand = new RelayCommand(OpenInfoPopup);
            SaveInfoChangesCommand = new RelayCommand(SaveInfoChanges, CanSaveInfoChanges);
            CloseInfoPopupCommand = new RelayCommand(CloseInfoPopup);
            RemoveStudentCommand = new RelayCommand(RemoveStudent);
        }

        public override void OpenPopup(object obj)
        {
            try
            {
                if (obj is null || obj is not Ride objAsRide)
                    RideData = new();
                else
                    RideData = objAsRide;

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
                if (obj is not Ride objAsRide || Car is null || Driver is null)
                    return;

                if (objAsRide.Id == 0)
                {
                    var ride = new Ride()
                    {
                        FullName = FullName,
                        Type = Type,
                        CarId = Car.Id,
                        DriverId = Driver.Id,
                    };

                    _dbContext.Rides.Add(ride);
                }
                else
                {
                    var ride = _dbContext.Rides.FirstOrDefault(r => r.Id == objAsRide.Id);

                    if (ride is null)
                        return;

                    ride.FullName = FullName;
                    ride.Type = Type;
                    ride.CarId = Car.Id;
                    ride.DriverId = Driver.Id;

                    _dbContext.Entry(ride).State = EntityState.Modified;
                }

                _dbContext.SaveChanges();
                Rides = new ObservableCollection<Ride>([.. _dbContext.Rides]);
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

            FullName = string.Empty;
            IsToggleChecked = false;
            Car = null;

            ClearAllErrors();
        }

        public override void UpdateEntity(object obj)
        {
            if (obj is null || obj is not Ride objAsRide)
                return;

            FullName = objAsRide.FullName;
            IsToggleChecked = objAsRide.Type != "Home";
            Car = objAsRide.Car;

            OpenPopup(obj);
        }

        public override void DeleteEntity(object obj)
        {
            if (obj is null || obj is not Ride objAsRide)
                return;

            var ride = _dbContext.Rides.FirstOrDefault(r => r.Id == objAsRide.Id);

            if (ride is null)
                return;

            _dbContext.Rides.Remove(ride);
            _dbContext.SaveChanges();
            Rides = new ObservableCollection<Ride>([.. _dbContext.Rides]);
        }


        // Adding/Removing students to the ride

        public void OpenStudentPopup(object obj)
        {
            if (obj is null || obj is not Ride objAsRide)
                return;

            RideId = objAsRide.Id;
            MaxSeatCount = objAsRide.Car?.SeatCount;
            SelectedStudentCount = _dbContext.Students.Where(s => s.RideId == RideId).Count();
            Students = new ObservableCollection<Student>([.. _dbContext.Students.Include(s => s.Group).Where(s => s.RideId == null)]);

            IsStudentPopupOpen = true;
        }

        public int ModifyStudent(object obj)
        {
            if (obj is null || obj is not Student objAsStudent)
                throw new Exception();

            var isContain = SelectedStudents.Contains(objAsStudent);

            if (!isContain)
            {
                if (SelectedStudentCount == MaxSeatCount)
                    return 0;

                SelectedStudents.Add(objAsStudent);
                SelectedStudentCount++;

                return 1;
            }
            else
            {
                SelectedStudents.Remove(objAsStudent);
                SelectedStudentCount--;

                return -1;
            }
        }

        public void CloseStudentPopup(object obj)
        {
            IsStudentPopupOpen = false;
            SelectedStudents.Clear();
        }

        public bool CanSaveStudentChanges(object obj)
        {
            return SelectedStudents.Count > 0;
        }

        public void SaveStudentChanges(object obj)
        {
            foreach (var student in SelectedStudents)
            {
                student.RideId = RideId;
            }

            _dbContext.SaveChanges();
            CloseStudentPopup(obj);
        }


        // Removing students from the ride

        public void OpenInfoPopup(object obj)
        {
            if (obj is null || obj is not Ride objAsRide)
                return;

            RideId = objAsRide.Id;
            Students = new ObservableCollection<Student>([.. _dbContext.Students.Include(s => s.Group).Where(s => s.RideId == RideId)]);
            SelectedStudentCount = Students.Count;

            IsInfoPopupOpen = true;
        }

        public void RemoveStudent(object obj)
        {
            if (obj is null || obj is not Student objAsStudent)
                return;

            Students.Remove(objAsStudent);
            SelectedStudentCount--;
            SelectedStudents.Add(objAsStudent);
        }

        public void CloseInfoPopup(object obj)
        {
            IsInfoPopupOpen = false;
            SelectedStudents.Clear();
        }

        public bool CanSaveInfoChanges(object obj)
        {
            return SelectedStudents.Count > 0;
        }

        public void SaveInfoChanges(object obj)
        {
            foreach (var student in SelectedStudents)
            {
                student.RideId = null;
            }

            _dbContext.SaveChanges();
            CloseInfoPopup(obj);
        }

        private void LoadDriver()
        {
            if (Car is not null)
                Driver = _dbContext.Cars.Include(c => c.Driver).FirstOrDefault(c => c.Id == Car.Id)?.Driver;
            else
                Driver = null;
        }
    }
}
