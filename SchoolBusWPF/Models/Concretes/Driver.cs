using SchoolBusWPF.Models.Abstracts;

namespace SchoolBusWPF.Models.Concretes
{
	public class Driver : Entity
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string FullName
		{
			get { return $"{FirstName} {LastName}"; }
		}
		public string? UserName { get; set; }
		public string? Password { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Licence { get; set; }
		public Car? Car { get; set; }
	}
}
