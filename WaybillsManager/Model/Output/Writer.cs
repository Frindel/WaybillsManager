using System;
using System.Collections.Generic;
using System.Globalization;
using System.Collections;
using System.Linq;
using WaybillsManager.Model.Data.Entities;

namespace WaybillsManager.Model.Output
{
	public abstract class Writer
	{
		protected string _templateUrl;

		public Writer(string templateURL)
		{
			_templateUrl = templateURL;
		}

		public virtual void WriteWaybill(Waybill waybill)
		{
			// вывод номера
			ReplaseTemplateText("[Number]", waybill.Number.ToString());

			// вывод даты путевки
			ReplaseTemplateText("[Date]", waybill.Date.ToString("dd MMMM yyyy", CultureInfo.GetCultureInfo("ru-RU")));

			// вывод года в коротком формате
			ReplaseTemplateText("[ShortYear]", waybill.Date.ToString("yy"));

			// вывод года
			ReplaseTemplateText("[Year]", waybill.Date.ToString("yyyy"));

			// вывод месяца (буквенный формат)
			ReplaseTemplateText("[ChartsMonth]", waybill.Date.ToString("dd MMMM", CultureInfo.GetCultureInfo("ru-RU")).Split(' ')[1]);

			// выывод месяца (числовой формат)
			ReplaseTemplateText("[NumberMonth]", waybill.Date.Month.ToString());

			// вывод дня
			ReplaseTemplateText("[Day]", waybill.Date.Day.ToString());

			// вывод модели машины
			ReplaseTemplateText("[CarMap]", waybill.Car.Name);

			// вывод гос. номера
			ReplaseTemplateText("[StateNumber]", waybill.CarStateNumber.Number);

			// вывод ФИО водителя
			ReplaseTemplateText("[Driver]", waybill.Driver.Name);

			// вывод удостоверения водителя
			ReplaseTemplateText("[IdentityCard]", waybill.IdentityCard.Number);

			// выывод маршрута
			string toRoute = (waybill.Route.EndPoint == null || waybill.Route.EndPoint.Name == string.Empty) ?
				waybill.Route.StartPoint.Name : $"{waybill.Route.StartPoint.Name} - {waybill.Route.EndPoint.Name}";

			string backRoute = (waybill.Route.EndPoint == null || waybill.Route.EndPoint.Name == string.Empty) ?
				waybill.Route.StartPoint.Name : $"{waybill.Route.EndPoint.Name} - {waybill.Route.StartPoint.Name}";

			ReplaseTemplateText("[ToRoute]", toRoute);
			ReplaseTemplateText("[BackRoute]", backRoute);
		}

		public virtual void WriteReport(IList<Waybill> waybills, DateOnly startPeriod, DateOnly endPeriod)
		{
			// выборка записей, находящихся в требуемом периоде
			var suitableWaybills = waybills.Where(w => w.Date >= startPeriod && w.Date <= endPeriod).ToArray();

			WriteValuesColumn("[Numbers]",suitableWaybills.Select(w=>w.Number).ToArray());
			WriteValuesColumn("[Dates]",suitableWaybills.Select(w=>w.Date).ToArray());
			WriteValuesColumn("[CarMaps]",suitableWaybills.Select(w=>w.Car.Name).ToArray());
			WriteValuesColumn("[StateNumber]", suitableWaybills.Select(w=>w.CarStateNumber.Number).ToArray());
			WriteValuesColumn("[Drivers]", suitableWaybills.Select(w => w.Driver.Name).ToArray());
			WriteValuesColumn("[IdentityCard]", suitableWaybills.Select(w=>w.IdentityCard.Number).ToArray());
			WriteValuesColumn("[Route]", suitableWaybills
				.Select(w => (w.Route.EndPoint == null || w.Route.EndPoint.Name == string.Empty) ?
				w.Route.StartPoint.Name : $"{w.Route.StartPoint.Name} - {w.Route.EndPoint.Name}").ToArray());
		}

		protected abstract void ReplaseTemplateText(string oldText, string newText);

		protected abstract void WriteValuesColumn(string colHeader, IList values);
	}
}
