using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaybillsManager.Model.Data.Entities
{

	[Table("CarStateNumbers")]
	public class CarStateNumber: ICloneable
	{
		public int Id { get; set; }

		[MaxLength(12)]
		public string Number { get; set; }

		public object Clone()
		{
			return new CarStateNumber()
			{
				Id = Id,
				Number = Number
			};
		}
	}
}
