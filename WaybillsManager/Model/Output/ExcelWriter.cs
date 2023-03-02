using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using WaybillsManager.Model.Data.Entities;
using Excel = Microsoft.Office.Interop.Excel;

namespace WaybillsManager.Model.Output
{
	internal class ExcelWriter : Writer
	{
		private Excel.Worksheet _worksheetWaybill;

		private Excel.Worksheet _worksheetReport;

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
			foreach (Excel.Worksheet worksheet in app.Worksheets)
			{
				// получение работчей страницы
				_worksheetWaybill = worksheet;

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

			foreach (Excel.Worksheet worksheet in app.Worksheets)
			{
				// получение работчей страницы
				_worksheetReport = worksheet;

				base.WriteReport(waybills, startPeriod, endPeriod);
			}
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

		protected override void WriteValuesColumn(string colHeader, IList values)
		{
			Excel.Range cell = _worksheetReport.Cells.Find(colHeader);
			
			// проверка существавания ячейки удовлетворяющей условию
			if (!(cell is Excel.Range))
				return;
			
			Excel.Borders cellBorders = cell.Borders;

			int columnIndex = cell.Column;
			int rowIndex = cell.Row;

			foreach (var value in values)
			{
				Excel.Range curRange = _worksheetReport.Cells[rowIndex, columnIndex];

				curRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = cell.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle;
				curRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = cell.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle;
				curRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = cell.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle;
				curRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = cell.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle;

				curRange.Value = value.ToString();

				rowIndex++;
			}
		}
	}
}
