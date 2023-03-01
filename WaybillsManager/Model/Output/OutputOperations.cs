using System;
using WaybillsManager.Model.Data;
using WaybillsManager.Model.Data.Entities;

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

				writer.WriteWaybill(waybill);

				return true;
			}
			catch (NotImplementedException)
			{
				return false;
			}
		}

		public static void WriteReport()
		{

		}
	}
}
