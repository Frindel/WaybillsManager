using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaybillsManager.Model.Data.Entities
{
	[Table("IdentityCards")]
	public class IdentityCard
	{
		public int Id { get; set; }

		public Driver Driver { get; set; }

		[MaxLength(32)]
		public string Number { get; set; }
	}
}
