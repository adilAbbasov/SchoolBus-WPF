using SchoolBusWPF.Models.Abstracts;

namespace SchoolBusWPF.Models.Concretes
{
    public class Ride : Entity
    {
        public string? FullName { get; set; }
        public string? Type { get; set; }
        public Car? Car { get; set; }
        public int? CarId { get; set; }
        public Driver? Driver { get; set; }
        public int? DriverId { get; set; }
        public virtual IEnumerable<Student>? Students { get; set; }
    }
}
