using System.ComponentModel.DataAnnotations;

namespace WaybillsManager.Model.Data.Entitys
{
	public class IdentityCard
	{
		public int Id { get; set; }

		public Driver Driver { get; set; }

		[MaxLength(32)]
		public string Number { get; set; }
	}
}
