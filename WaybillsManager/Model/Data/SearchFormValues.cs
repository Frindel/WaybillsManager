using System;

namespace WaybillsManager.Model.Data
{
	class SearchFormValues
	{
		public int? Period { get; set; }
		public DateOnly? Date { get; set; }
		public int? Number { get; set; }
		public string CarMap { get; set; }
		public string StateNumber { get; set; }
		public string Driver { get; set; }
		public string StartPoint { get; set; }
		public string EndPoint { get; set; }
	}
}
