using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SCHOOL_BUS.Commands;
using SchoolBusWPF.Models.Concretes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace SchoolBusWPF.ViewModels
{
    public class HolidayViewModel : ViewModelBase
    {
        private Holiday _holidayData;
        public Holiday HolidayData
        {
            get { return _holidayData; }
            set
            {
                _holidayData = value;
                OnPropertyChanged(nameof(HolidayData));
            }
        }

        private ObservableCollection<Holiday> _holidays = [];
        public ObservableCollection<Holiday> Holidays
        {
            get { return _holidays; }
            set
            {
                _holidays = value;
                OnPropertyChanged(nameof(Holidays));
            }
        }

        private string? _name = "";
        [Required(ErrorMessage = "Name is required")]
        public string? Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));

                Validate(nameof(Name), value);
            }
        }

        private DateTime? _startDate;
        [Required(ErrorMessage = "First date is required")]
        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));

                if (StartDate > EndDate)
                {
                    var startDateErrors = new List<string>()
                    {
                        "Start date can not be after the end date"
                    };

                    Errors.Add(nameof(StartDate), startDateErrors);
                    //ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartDate)));
                }

                Validate(nameof(StartDate), value);
            }
        }

        private DateTime? _endDate;
        [Required(ErrorMessage = "Last date is required")]
        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));

                if (EndDate < StartDate)
                {
                    var endDateErrors = new List<string>()
                    {
                        "End date can not be before the start date"
                    };

                    Errors.Add(nameof(EndDate), endDateErrors);
                    //ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndDate)));
                }

                Validate(nameof(EndDate), value);
            }
        }

        public HolidayViewModel()
        {
            Holidays = new ObservableCollection<Holiday>([.. _dbContext.Holidays]);

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
                if (obj is null || obj is not Holiday objAsHoliday)
                    HolidayData = new();
                else
                    HolidayData = objAsHoliday;

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
                if (obj is not Holiday objAsHoliday)
                    return;

                if (!IsUpdate && HolidayExists(Name))
                {
                    Console.WriteLine($"The holiday with \"{Name}\" name is already available");
                    return;
                }

                if (objAsHoliday.Id == 0)
                {
                    var holiday = new Holiday()
                    {
                        Name = Name,
                        StartDate = DateOnly.FromDateTime(StartDate ?? new DateTime()),
                        EndDate = DateOnly.FromDateTime(EndDate ?? new DateTime())
                    };

                    _dbContext.Holidays.Add(holiday);
                }
                else
                {
                    var holiday = _dbContext.Holidays.FirstOrDefault(h => h.Id == objAsHoliday.Id);

                    if (holiday is null)
                        return;

                    holiday.Name = Name;
                    holiday.StartDate = DateOnly.FromDateTime(StartDate ?? new DateTime());
                    holiday.EndDate = DateOnly.FromDateTime(EndDate ?? new DateTime());

                    _dbContext.Entry(holiday).State = EntityState.Modified;
                }

                _dbContext.SaveChanges();
                Holidays = new ObservableCollection<Holiday>([.. _dbContext.Holidays]);
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
            StartDate = null;
            EndDate = null;

            IsUpdate = false;
        }

        public override void UpdateEntity(object obj)
        {
            if (obj is null || obj is not Holiday objAsHoliday)
                return;

            Name = objAsHoliday.Name;
            StartDate = objAsHoliday.StartDate.HasValue ? objAsHoliday.StartDate.Value.ToDateTime(new TimeOnly(0, 0)) : null;
            EndDate = objAsHoliday.EndDate.HasValue ? objAsHoliday.EndDate.Value.ToDateTime(new TimeOnly(0, 0)) : null;

            IsUpdate = true;
            OpenPopup(obj);
        }

        public override void DeleteEntity(object obj)
        {
            if (obj is null || obj is not Holiday objAsHoliday)
                return;

            var holiday = _dbContext.Holidays.FirstOrDefault(h => h.Id == objAsHoliday.Id);

            if (holiday is null)
                return;

            _dbContext.Holidays.Remove(holiday);
            _dbContext.SaveChanges();
            Holidays = new ObservableCollection<Holiday>([.. _dbContext.Holidays]);
        }

        private bool HolidayExists(string? name)
        {
            return _dbContext.Holidays.Any(h => h.Name == name);
        }
    }
}
