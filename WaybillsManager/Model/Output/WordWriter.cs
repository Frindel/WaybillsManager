using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaybillsManager.Model.Data.Entities;
using Word = Microsoft.Office.Interop.Word;

namespace WaybillsManager.Model.Output
{
	internal class WordWriter : Writer
	{
		private Word.Application _wordApp;

		public WordWriter(string templateUrl): base(templateUrl)
		{
		}

		public override void WriteWaybill(Waybill waybill)
		{
			Word.Application app = new Word.Application();
			
			// открытие шаблона
			app.Documents.Open(_templateUrl);

			// настройка отображения окна Microsoft Word
			app.Visible = true;
			app.WindowState = Word.WdWindowState.wdWindowStateMaximize;

			_wordApp = app;
			
			// замена установленных значений
			base.WriteWaybill(waybill);
		}

		public override void WriteReport(IList<Waybill> waybills, DateOnly startPeriod, DateOnly endPeriod)
		{
			throw new NotImplementedException();
		}

		protected override void ReplaseTemplateText(string oldText, string newText)
		{
			Word.Find find = _wordApp.Selection.Find;

			find.Text = oldText;
			find.Replacement.Text = newText;

			find.Execute(Replace: Word.WdReplace.wdReplaceAll);
		}
	}
}
