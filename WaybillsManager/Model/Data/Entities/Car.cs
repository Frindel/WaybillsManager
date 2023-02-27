using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaybillsManager.Model.Data.Entities
{
	[Table("Cars")]
	public class Car:ICloneable
	{
		public int Id { get; set; }

		[MaxLength(48)]
		public string Name { get; set; }

		public object Clone ()
		{
			return new Car()
			{
				Id = Id,
				Name = Name
			};
		}
	}
}
