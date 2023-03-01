using System;
using System.Collections.Generic;
using System.Globalization;
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

			// вывод водителя
			ReplaseTemplateText("[Driver]", waybill.IdentityCard.Driver.Name);

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
			ReplaseTemplateText("[Driver]", waybill.IdentityCard.Driver.Name);

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

		public abstract void WriteReport(IList<Waybill> waybills, DateOnly startPeriod, DateOnly endPeriod);

		protected abstract void ReplaseTemplateText(string oldText, string newText);
	}
}
