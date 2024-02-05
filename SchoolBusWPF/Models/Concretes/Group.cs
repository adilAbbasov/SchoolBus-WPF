using SchoolBusWPF.Models.Abstracts;

namespace SchoolBusWPF.Models.Concretes
{
	public class Group : Entity
	{
		public string? Title { get; set; }
		public virtual IEnumerable<Student>? Students { get; set; }
	}
}
