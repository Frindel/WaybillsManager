using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WaybillsManager.Model.Data.Entitys
{
	public class Car
	{
		public int Id { get; set; }

		[MaxLength(48)]
		public string Name { get; set; }

		public List<CarStateNumber> CarStateNumbers { get; set; }
	}
}
