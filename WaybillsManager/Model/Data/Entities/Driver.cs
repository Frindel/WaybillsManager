using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WaybillsManager.Model.Data.Entitys
{
	public class Driver
	{
		public int Id { get; set; }

		[MaxLength(48)]
		public string Name { get; set; }

		public List<IdentityCard> IdentityCards { get; set; }
	}
}
