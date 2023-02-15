using System.ComponentModel.DataAnnotations;

namespace WaybillsManager.Model.Data.Entitys
{
	public class CarStateNumber
	{
		public int Id { get; set; }

		[MaxLength(12)]
		public string Number { get; set; }

		public Car Car { get; set; }
	}
}
