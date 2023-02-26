using System;

namespace WaybillsManager.Model.Data.Entitys
{
	public class Waybill
	{
		public int Id { get; set; }

		public DateOnly Date { get; set; }

		public int Number { get; set; }

		public IdentityCard IdentityCard { get; set; }

		public CarStateNumber CarStateNumber { get; set; }

		public Car Car { get; set; }

		public Route Route { get; set; }
	}
}
