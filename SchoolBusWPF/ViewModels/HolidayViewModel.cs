using Microsoft.EntityFrameworkCore;
using SCHOOL_BUS.Commands;
using SchoolBusWPF.Models.Concretes;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

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

        private DateTime? _startDate;
        [Required]
        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
                ValidateProperty(nameof(StartDate), value, "Start date is required.");

                ClearErrors(nameof(StartDate));
                ClearErrors(nameof(EndDate));

                if (StartDate >= EndDate)
                    AddError(nameof(StartDate), "Start date is not valid.");
            }
        }

        private DateTime? _endDate;
        [Required]
        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
                ValidateProperty(nameof(EndDate), value, "End date is required.");

                ClearErrors(nameof(EndDate));
                ClearErrors(nameof(StartDate));

                if (EndDate <= StartDate)
                    AddError(nameof(EndDate), "End date is not valid.");
            }
        }

        public HolidayViewModel()
        {
            Holidays = new ObservableCollection<Holiday>([.. _dbContext.Holidays]);

            OpenPopupCommand = new RelayCommand(OpenPopup);
            SaveChangesCommand = new RelayCommand(SaveChanges, CanSaveChanges);
            ClosePopupCommand = new RelayCommand(ClosePopup);
            UpdateEntityCommand = new RelayCommand(UpdateEntity);
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
            return Validator.TryValidateObject(this, new ValidationContext(this), null) && !HasErrors;
        }

        public override void SaveChanges(object obj)
        {
            try
            {
                var objAsHoliday = (obj as Holiday)!;

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
                    var holiday = _dbContext.Holidays.FirstOrDefault(h => h.Id == objAsHoliday.Id)!;

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
            ClearAllErrors();
        }

        public override void UpdateEntity(object obj)
        {
            var objAsHoliday = (obj as Holiday)!;

            Name = objAsHoliday.Name;
            StartDate = objAsHoliday.StartDate.HasValue ? objAsHoliday.StartDate.Value.ToDateTime(new TimeOnly(0, 0)) : null;
            EndDate = objAsHoliday.EndDate.HasValue ? objAsHoliday.EndDate.Value.ToDateTime(new TimeOnly(0, 0)) : null;

            IsUpdate = true;
            OpenPopup(obj);
        }

        public override void DeleteEntity(object obj)
        {
            var objAsHoliday = (obj as Holiday)!;
            var holiday = _dbContext.Holidays.FirstOrDefault(h => h.Id == objAsHoliday.Id)!;

            _dbContext.Holidays.Remove(holiday);
            _dbContext.SaveChanges();

            Holidays = new ObservableCollection<Holiday>([.. _dbContext.Holidays]);
        }

        private bool HolidayExists(string? name)
        {
            return _dbContext.Holidays.Any(h => h.Name == name);
        }

        public void SearchData(string pattern)
        {
            Holidays = new ObservableCollection<Holiday>([.. _dbContext.Holidays.Where(h => h.Name!.Contains(pattern.ToLower()))]);
        }
    }
}
