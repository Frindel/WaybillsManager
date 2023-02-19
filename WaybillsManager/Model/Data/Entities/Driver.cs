using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaybillsManager.Model.Data.Entitys
{
	[Table("Drivers")]
	public class Driver
	{
		public int Id { get; set; }

		[MaxLength(48)]
		public string Name { get; set; }

		public List<IdentityCard> IdentityCards { get; set; }
	}
}
