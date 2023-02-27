using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaybillsManager.Model.Data.Entities
{

	[Table("RoutePoints")]
	public class RoutePoint: ICloneable
	{
		public int Id { get; set; }

		[MaxLength(32)]
		public string Name { get; set; }

		public object Clone()
		{
			return new RoutePoint()
			{
				Id = Id,
				Name = Name
			};
		}
	}
}
