using Microsoft.EntityFrameworkCore;
using SCHOOL_BUS.Commands;
using SchoolBusWPF.Models.Concretes;
using System.Collections.ObjectModel;

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
        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
                ValidateStartDate();
            }
        }

        private void ValidateStartDate()
        {
            ClearErrors(nameof(StartDate));

            if (StartDate is null)
                AddError(nameof(StartDate), "Start date is required.");

            if(StartDate >= EndDate)
                AddError(nameof(StartDate), "Start date is not valid.");
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
                ValidateEndDate();
            }
        }

        private void ValidateEndDate()
        {
            ClearErrors(nameof(EndDate));

            if (EndDate is null)
                AddError(nameof(EndDate), "End date is required.");

            if (EndDate <= StartDate)
                AddError(nameof(EndDate), "End date is not valid.");
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
            return !HasErrors;
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
            ClearAllErrors();
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
