using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaybillsManager.Model.Data.Entitys
{
	[Table("Cars")]
	public class Car
	{
		public int Id { get; set; }

		[MaxLength(48)]
		public string Name { get; set; }

		public List<CarStateNumber> CarStateNumbers { get; set; }
	}
}
