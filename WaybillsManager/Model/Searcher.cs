using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using WaybillsManager.Model.Data;
using WaybillsManager.Model.Data.Entities;

namespace WaybillsManager.Model
{
	internal static class Searcher
	{
		public static void Search(IList<Waybill> values, SearchFormValues searchValues)
		{
			ICollectionView view = CollectionViewSource.GetDefaultView(values);

			if (view == null)
				return;

			List<Func<Waybill, bool>> filters = new List<Func<Waybill, bool>>();

			if (searchValues.Period is int period)
				filters.Add(w => w.Date.Year == period);

			if (searchValues.Date is DateOnly date)
				filters.Add(w => w.Date == date);

			if (searchValues.Number is int number)
				filters.Add(w => w.Number == number);

			if (searchValues.CarMap is string carMap)
				filters.Add(w => w.Car.Name.Contains(carMap));

			if (searchValues.StateNumber is string stateNumber)
				filters.Add(w => w.CarStateNumber.Number.Contains(stateNumber));

			if (searchValues.Driver is string driver)
				filters.Add(w => w.Driver.Name.Contains(driver));

			string startPoint = searchValues.StartPoint;
			string endPoint = searchValues.EndPoint;

			if ((startPoint != null && startPoint!=string.Empty) && (endPoint != null && endPoint != string.Empty))
			{
				filters.Add(w => w.Route.EndPoint != null && w.Route.StartPoint.Name.Contains(startPoint) && w.Route.EndPoint.Name.Contains(endPoint));
			}
			else
			{
				if (startPoint != null && startPoint != string.Empty)
					filters.Add(w => w.Route.StartPoint.Name.Contains(startPoint));
				if (endPoint != null && endPoint != string.Empty)
					filters.Add(w=>w.Route.EndPoint!=null && w.Route.EndPoint.Name.Contains(endPoint));
			}
			
			if (filters.Count == 0)
				return;

			view.Filter = obj =>
			{
				Waybill waybill = (Waybill)obj;

				foreach (Func<Waybill, bool> filter in filters)
				{
					if (!filter(waybill))
						return false;
				}

				return true;
			};
		}

		public static void Cancel(IList<Waybill> values)
		{
			ICollectionView view = CollectionViewSource.GetDefaultView(values);

			view.Filter = null;
		}
	}
}
