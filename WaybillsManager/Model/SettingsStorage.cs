using Prism.Mvvm;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using WaybillsManager.Model.Data;
using WaybillsManager.Properties;
using System.IO;

namespace WaybillsManager.Model
{
	internal class SettingsStorage : BindableBase
	{
		private static SettingsStorage _loader;

		private Settings _settings;

		private OutputTemplate _defaultWordTemplate;
		private OutputTemplate _defaultExcelTemplate;

		#region Propertys

		public string DbDirectory
		{
			get => _settings.DbDirectory;
			set
			{
				_settings.DbDirectory = value;
				RaisePropertyChanged(nameof(DbDirectory));
			}
		}

		public OutputTemplate DefaultWordTemplate
		{
			get => _defaultWordTemplate;
			set
			{
				_defaultWordTemplate = value;
				RaisePropertyChanged(nameof(DefaultWordTemplate));
			}
		}

		public OutputTemplate DefaultExcelTemplate
		{
			get => _defaultExcelTemplate;
			set
			{
				_defaultExcelTemplate = value;
				RaisePropertyChanged(nameof(DefaultExcelTemplate));
			}
		}

		public ObservableCollection<OutputTemplate> ExcelTemplates { get; }

		public ObservableCollection<OutputTemplate> WordTemplates { get; }

		#endregion

		#region Constructor

		private SettingsStorage()
		{
			_settings = Settings.Default;

			ExcelTemplates = GetOutputTemplatesCollection(_settings.ExcelTemplates);
			WordTemplates = GetOutputTemplatesCollection(_settings.WordTemplates);

			DefaultExcelTemplate = ExcelTemplates.Where(t => t.URL == _settings.DefaultExcelTemplate).FirstOrDefault();
			DefaultWordTemplate = WordTemplates.Where(t => t.URL == _settings.DefaultWordTemplate).FirstOrDefault();

			//удаление шаблона по умолчанию для Excel при его удалении из коллекции шаблонов 
			ExcelTemplates.CollectionChanged += (_, e) =>
			{
				if (e.Action == NotifyCollectionChangedAction.Remove && DefaultExcelTemplate != null && !ExcelTemplates.Contains(DefaultExcelTemplate))
					DefaultExcelTemplate = null;
			};

			//удаление шаблона по умолчанию для Word при его удалении из коллекции шаблонов 
			WordTemplates.CollectionChanged += (_, e) =>
			{
				if (e.Action == NotifyCollectionChangedAction.Remove && DefaultWordTemplate != null && !WordTemplates.Contains(DefaultWordTemplate))
					DefaultWordTemplate = null;
			};
		}

		public static SettingsStorage GetStorage()
		{
			if (_loader == null)
				_loader = new SettingsStorage();
			return _loader;
		}

		#endregion

		public void Save()
		{
			_settings.DefaultExcelTemplate = DefaultExcelTemplate?.URL ?? string.Empty;
			_settings.DefaultWordTemplate = DefaultWordTemplate?.URL ?? string.Empty;

			//перезапись шаблонов для вывода в Excel
			_settings.ExcelTemplates = new StringCollection();
			_settings.ExcelTemplates.AddRange(ExcelTemplates.Select(t => t.URL).ToArray());

			//перезапись шаблонов для вывода в Word
			_settings.WordTemplates = new StringCollection();
			_settings.WordTemplates.AddRange(WordTemplates.Select(t => t.URL).ToArray());

			_settings.Save();
		}

		public async Task SaveAsync()
		{
			await Task.Run(() =>
			{
				_settings.Save();
			});
		}

		//преобразует набор url-ов шаблонов в коллекцию объектов
		private ObservableCollection<OutputTemplate> GetOutputTemplatesCollection(ICollection urls)
		{
			ObservableCollection<OutputTemplate> templates = new ObservableCollection<OutputTemplate>();

			if (urls == null)
				return templates;

			bool hasMissingTemplate = false;

			foreach (string url in urls)
			{
				OutputTemplate template = OutputTemplate.GetTemplateByUrl(url);

				//удаление шаблона в случае его отсутствия по url или отсутствии необходимых закладок в нем
				if (template == null)
				{
					_settings.ExcelTemplates.Remove(url);
					hasMissingTemplate = true;
					continue;
				}

				templates.Add(template);
			}

			//сохранение настроек в случае отсутствия сохраненных ранее шаблонов
			if (hasMissingTemplate)
				Save();

			return templates;
		}
	}
}
