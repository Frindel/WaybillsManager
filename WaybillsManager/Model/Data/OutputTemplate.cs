using System;
using System.IO;

namespace WaybillsManager.Model.Data
{
	// шаблон вывода
	public class OutputTemplate
	{
		// название шаблона
		public string FileName { get; set; }

		// URL адрес шаблона
		public string URL { get; set; }

		private OutputTemplate(string fileName, string url)
		{
			FileName = fileName;
			URL = url;
		}

		public static OutputTemplate GetTemplateByUrl(string url)
		{
			//todo: проверка на наличие нужных закладок в документе
			try
			{
				FileInfo info = new FileInfo(url);
				string fileName = info.Name;

				return new OutputTemplate(fileName, url);
			}
			catch(Exception)
			{
				return null;
			}
		}
	}
}
