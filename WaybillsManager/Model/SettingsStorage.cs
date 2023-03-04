using Prism.Mvvm;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using WaybillsManager.Model.Data;
using WaybillsManager.Properties;
using System.IO;
using System.Windows;
using System;
using System.Reflection;

namespace WaybillsManager.Model
{
	internal class SettingsStorage : BindableBase
	{
		private static SettingsStorage _loader;

		private Settings _settings;

		private OutputTemplate _defaultWaybillTemplate;
		private OutputTemplate _defaultReportTemplate;

		private string _oldDbDirectory;

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

		public OutputTemplate DefaultWaybillTemplate
		{
			get => _defaultWaybillTemplate;
			set
			{
				_defaultWaybillTemplate = value;
				RaisePropertyChanged(nameof(DefaultWaybillTemplate));
			}
		}

		public OutputTemplate DefaultReportTemplate
		{
			get => _defaultReportTemplate;
			set
			{
				_defaultReportTemplate = value;
				RaisePropertyChanged(nameof(DefaultReportTemplate));
			}
		}

		public ObservableCollection<OutputTemplate> ReportTemplates { get; }

		public ObservableCollection<OutputTemplate> WaybillTemplates { get; }

		#endregion

		#region Constructor

		private SettingsStorage()
		{
			_settings = Settings.Default;

			ReportTemplates = GetOutputTemplatesCollection(_settings.ReportTemplates);
			WaybillTemplates = GetOutputTemplatesCollection(_settings.WaybillTemplates);

			DefaultReportTemplate = ReportTemplates.Where(t => t.URL == _settings.DefaultReportTemplate).FirstOrDefault();
			DefaultWaybillTemplate = WaybillTemplates.Where(t => t.URL == _settings.DefaultWaybillTemplate).FirstOrDefault();

			//удаление шаблона по умолчанию для Report при его удалении из коллекции шаблонов 
			ReportTemplates.CollectionChanged += (_, e) =>
			{
				if (e.Action == NotifyCollectionChangedAction.Remove && DefaultReportTemplate != null && !ReportTemplates.Contains(DefaultReportTemplate))
					DefaultReportTemplate = null;
			};

			//удаление шаблона по умолчанию для Waybill при его удалении из коллекции шаблонов 
			WaybillTemplates.CollectionChanged += (_, e) =>
			{
				if (e.Action == NotifyCollectionChangedAction.Remove && DefaultWaybillTemplate != null && !WaybillTemplates.Contains(DefaultWaybillTemplate))
					DefaultWaybillTemplate = null;
			};

			// фиксация последней сохраненной директории БД
			_oldDbDirectory = _settings.DbDirectory;
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
			_settings.DefaultReportTemplate = DefaultReportTemplate?.URL ?? string.Empty;
			_settings.DefaultWaybillTemplate = DefaultWaybillTemplate?.URL ?? string.Empty;

			//перезапись шаблонов для вывода в Report
			_settings.ReportTemplates = new StringCollection();
			_settings.ReportTemplates.AddRange(ReportTemplates.Select(t => t.URL).ToArray());

			//перезапись шаблонов для вывода в Waybill
			_settings.WaybillTemplates = new StringCollection();
			_settings.WaybillTemplates.AddRange(WaybillTemplates.Select(t => t.URL).ToArray());

			_settings.Save();

			if (_oldDbDirectory!=_settings.DbDirectory)
			{
				string path = Assembly.GetExecutingAssembly().Location.Replace(".dll", ".exe");
				System.Diagnostics.Process.Start(path);
				Application.Current.Shutdown();
			}
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
					_settings.ReportTemplates.Remove(url);
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
