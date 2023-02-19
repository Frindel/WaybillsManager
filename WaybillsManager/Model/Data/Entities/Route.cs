using System.ComponentModel.DataAnnotations.Schema;

namespace WaybillsManager.Model.Data.Entitys
{
	[Table("Routes")]
	public class Route
	{
		public int Id { get; set; }

		public RoutePoint StartPoint { get; set; }

		public RoutePoint? EndPoint {get; set;}
	}
}
