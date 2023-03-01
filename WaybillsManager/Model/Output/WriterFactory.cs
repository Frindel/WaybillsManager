using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaybillsManager.Model.Output
{
	internal class WriterFactory
	{
		public virtual Writer GetWriter(string templateUrl)
		{
			// получение расширения файла
			string fileExt = new FileInfo(templateUrl).Extension;

			if (fileExt == string.Empty)
				return null;

			// выбор подходящей реализации IWriter
			if (fileExt == ".dot" || fileExt == ".dotx")
				return new WordWriter(templateUrl);

			if (fileExt == ".xlt" || fileExt == ".xltx")
				return new ExcelWriter(templateUrl);

			return null;
		}
	}
}
