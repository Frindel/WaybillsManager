using System;

namespace WaybillsManager.Model.Data.Entities
{
	public class Waybill: ICloneable
	{
		public int Id { get; set; }

		public DateOnly Date { get; set; }

		public int Number { get; set; }

		public IdentityCard IdentityCard { get; set; }

		public CarStateNumber CarStateNumber { get; set; }

		public Car Car { get; set; }

		public Route Route { get; set; }

		public object Clone()
		{
			return new Waybill()
			{
				Id = Id,
				Date = Date,
				Number = Number,
				IdentityCard= (IdentityCard)IdentityCard.Clone(),
				CarStateNumber = (CarStateNumber)CarStateNumber.Clone(),
				Car = (Car)Car.Clone(),
				Route = (Route)Route.Clone()
			};
		}
	}
}
