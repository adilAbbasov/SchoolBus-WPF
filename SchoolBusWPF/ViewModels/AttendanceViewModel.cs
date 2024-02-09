using Microsoft.EntityFrameworkCore;
using SchoolBusWPF.Data;
using SchoolBusWPF.Models.Concretes;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SchoolBusWPF.ViewModels
{
    public class AttendanceViewModel : INotifyPropertyChanged
    {
        public readonly SchoolBusDBContext _dbContext = new();

        private ObservableCollection<Attendance> _attendances = [];
        public ObservableCollection<Attendance> Attendances
        {
            get { return _attendances; }
            set
            {
                _attendances = value;
                OnPropertyChanged(nameof(Attendances));
            }
        }

        public AttendanceViewModel()
        {
            Attendances = new ObservableCollection<Attendance>([.. _dbContext.Attendances.Include(a => a.Student).ThenInclude(s => s!.Group)]);
        }

        public void DeleteEntity(object obj)
        {
            var objAsAttendance = (obj as Attendance)!;
            var attendance = _dbContext.Attendances.FirstOrDefault(a => a.Id == objAsAttendance.Id)!;

            _dbContext.Attendances.Remove(attendance);
            _dbContext.SaveChanges();

            Attendances = new ObservableCollection<Attendance>([.. _dbContext.Attendances]);
        }

        //

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
