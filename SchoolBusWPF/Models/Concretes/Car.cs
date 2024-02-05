using SchoolBusWPF.Models.Abstracts;

namespace SchoolBusWPF.Models.Concretes
{
	public class Car : Entity
	{
        public string? Name { get; set; }
        public string? PlateNumber { get; set; }
        public int? SeatCount { get; set; }
		public Driver? Driver { get; set; }
		public int? DriverId { get; set; }
	}
}
