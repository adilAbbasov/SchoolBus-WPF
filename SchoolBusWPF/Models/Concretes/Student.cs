using SchoolBusWPF.Models.Abstracts;

namespace SchoolBusWPF.Models.Concretes
{
	public class Student : Entity
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string FullName
		{
			get { return $"{FirstName} {LastName}"; }
		}
        public string? HomeAddress { get; set; }
		public string? OtherAddress { get; set; }
		public Parent? Parent { get; set; }
		public int? ParentId { get; set; }
		public Group? Group { get; set; }
		public int? GroupId { get; set; }
		public Ride? Ride { get; set; }
		public int? RideId { get; set; }
		public virtual IEnumerable<Attendance>? Attendances { get; set; }
	}
}
