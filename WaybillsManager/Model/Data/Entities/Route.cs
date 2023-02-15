namespace WaybillsManager.Model.Data.Entitys
{
	internal class Route
	{
		public int Id { get; set; }

		public RoutePoint StartPoint { get; set; }

		public RoutePoint EndPoint {get; set;}
	}
}
