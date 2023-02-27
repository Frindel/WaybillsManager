using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaybillsManager.Model.Data.Entities
{

	[Table("RoutePoints")]
	public class RoutePoint
	{
		public int Id { get; set; }

		[MaxLength(32)]
		public string Name { get; set; }
	}
}
