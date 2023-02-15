using System.ComponentModel.DataAnnotations;

namespace WaybillsManager.Model.Data.Entitys
{
	internal class RoutePoint
	{
		public int Id { get; set; }

		[MaxLength(32)]
		public string Name { get; set; }
	}
}
