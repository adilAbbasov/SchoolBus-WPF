using SchoolBusWPF.Models.Abstracts;

namespace SchoolBusWPF.Models.Concretes
{
	public class Attendance : Entity
    {
        public bool? WillAttend { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string? Destination { get; set; }
        public Student? Student { get; set; }
        public int? StudentId { get; set; }
    }
}
