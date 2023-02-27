using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaybillsManager.Model.Data.Entities
{

	[Table("CarStateNumbers")]
	public class CarStateNumber
	{
		public int Id { get; set; }

		[MaxLength(12)]
		public string Number { get; set; }
	}
}
