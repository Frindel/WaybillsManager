using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaybillsManager.Model.Data.Entities
{
	[Table("IdentityCards")]
	public class IdentityCard: ICloneable
	{
		public int Id { get; set; }

		public Driver Driver { get; set; }

		[MaxLength(32)]
		public string Number { get; set; }

		public object Clone ()
		{
			return new IdentityCard()
			{
				Id = Id,
				Number = Number,
				Driver = (Driver)Driver.Clone()
			};
		}
	}
}
