using System;
using System.Data;
using WaybillsManager.Model.Data;
using WaybillsManager.Model.Data.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.IO;

namespace WaybillsManager.Model.Output
{
	internal class OutputOperations
	{
		public static bool WriteWaybill(Waybill waybill, OutputTemplate template)
		{
			try
			{
				Writer writer = new WriterFactory().GetWriter(template.URL);

				if (writer == null)
					return false;

				Task.Run(()=> writer.WriteWaybill(waybill));

				return true;
			}
			catch (NotImplementedException)
			{
				return false;
			}
		}

		public static bool WriteReport(OutputTemplate template, DateOnly startPeriod, DateOnly endPeriod)
		{
			try
			{
				Writer writer = new WriterFactory().GetWriter(template.URL);

				Task.Run(()=>
				writer.WriteReport(WaybillsStorage.Get(), startPeriod, endPeriod));

				return true;
			}
			catch (RowNotInTableException)
			{
				return false;
			}
		}

		public static void UploadDb(string url)
		{
			SettingsStorage settings = SettingsStorage.GetStorage();

			File.Copy($"{settings.DbDirectory}/Waybills.db",$"{url}/Waybills.db");
		}
	}
}
