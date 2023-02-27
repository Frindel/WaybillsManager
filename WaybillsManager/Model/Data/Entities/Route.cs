using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaybillsManager.Model.Data.Entities
{
	[Table("Routes")]
	public class Route: ICloneable
	{
		public int Id { get; set; }

		public RoutePoint StartPoint { get; set; }

		public RoutePoint? EndPoint {get; set;}

		public object Clone()
		{
			return new Route()
			{
				Id = Id,
				StartPoint = (RoutePoint)StartPoint.Clone(),
				EndPoint = (EndPoint==null)?new RoutePoint():(RoutePoint)EndPoint.Clone()
			};
		}
	}
}
