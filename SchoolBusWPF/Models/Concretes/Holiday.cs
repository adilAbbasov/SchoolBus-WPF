using SchoolBusWPF.Models.Abstracts;

namespace SchoolBusWPF.Models.Concretes
{
	public class Holiday : Entity
	{
		public string? Name { get; set; }
		public DateOnly? StartDate { get; set; }
		public DateOnly? EndDate { get; set; }
	}
}
