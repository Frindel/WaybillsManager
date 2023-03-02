using System;
using System.Collections.Generic;
using WaybillsManager.Model.Data.Entities;
using Excel = Microsoft.Office.Interop.Excel;

namespace WaybillsManager.Model.Output
{
	internal class ExcelWriter : Writer
	{
		public Excel.Worksheet _worksheetWaybill;

		public ExcelWriter(string templateUrl) : base(templateUrl)
		{

		}

		public override void WriteWaybill(Waybill waybill)
		{
			var app = new Excel.Application();
			app.Workbooks.Add(_templateUrl);

			app.Visible = true;

			app.DisplayFullScreen = false;
			app.WindowState = Excel.XlWindowState.xlMaximized;

			// получение работчей страницы
			_worksheetWaybill = (Excel.Worksheet)app.ActiveSheet;

			// получение работчей страницы
			foreach (Excel.Worksheet worksheet in app.Worksheets)
			{
				base.WriteWaybill(waybill);
			}
		}

		public override void WriteReport(IList<Waybill> waybills, DateOnly startPeriod, DateOnly endPeriod)
		{
			var app = new Excel.Application();
			app.Workbooks.Add(_templateUrl);

			app.Visible = true;

			app.DisplayFullScreen = false;
			app.WindowState = Excel.XlWindowState.xlMaximized;


		}

		protected override void ReplaseTemplateText(string oldText, string newText)
		{
			// получение первой ячейки содержащий требуемое значение
			var cell = _worksheetWaybill.Cells.Find(oldText);

			// проверка существавания ячейки удовлетворяющей условию
			if (!(cell is Excel.Range))
				return;

			do
			{
				// вывод значения
				cell.Value = newText;

				// поиск следующей ячейки удовлетворяющей условию
				cell = _worksheetWaybill.Cells.FindNext();
			}
			while (cell != null);
		}
	}
}
