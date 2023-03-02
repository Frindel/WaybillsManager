using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.Xaml.Behaviors.Core;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using WaybillsManager.Model;
using WaybillsManager.Model.Data;

namespace WaybillsManager.ViewModel
{
	class SettingsViewModel : BindableBase
	{
		private Action _closeWindow;

		private SettingsStorage _settings;

		#region Propertys
		public string DbDirectory { get => _settings.DbDirectory; set => _settings.DbDirectory = value; }

		public ObservableCollection<OutputTemplate> WaybillTemplates { get => _settings.WaybillTemplates; }

		public ObservableCollection<OutputTemplate> ReportTemplates { get => _settings.ReportTemplates; }

		public OutputTemplate DefaultWaybillTemplate { get => _settings.DefaultWaybillTemplate; set => _settings.DefaultWaybillTemplate = value; }

		public OutputTemplate DefaultReportTemplate { get => _settings.DefaultReportTemplate; set => _settings.DefaultReportTemplate = value; }
		#endregion

		#region Commands
		public ActionCommand Close
		{
			get => new ActionCommand(() =>
			{
				_closeWindow();
			});
		}

		public ActionCommand Save
		{
			get => new ActionCommand(() =>
			{
				_settings.Save();
				_closeWindow();
			});
		}

		public ActionCommand AddWaybillTemplate
		{
			get => new ActionCommand(() =>
			{
				string url = GetFileUrl(true, new CommonFileDialogFilter(string.Empty, "*.dot;*.dotx;*.xlt;*.xltx"));

				if (url != string.Empty)
					WaybillTemplates.Add(OutputTemplate.GetTemplateByUrl(url));
			});
		}

		public RelayCommand RemoveWaybillTeemplate
		{
			get => new RelayCommand(template =>
			{
				WaybillTemplates.Remove((OutputTemplate)template);
			},
			validationValue => validationValue is OutputTemplate);
		}

		public ActionCommand AddReportTemplate
		{
			get => new ActionCommand(() =>
			{
				string url = GetFileUrl(true, new CommonFileDialogFilter(string.Empty, ".xls"));

				if (url!=string.Empty)
					ReportTemplates.Add(OutputTemplate.GetTemplateByUrl(url));
			});
		}

		public RelayCommand RemoveReportTeemplate
		{
			get => new RelayCommand(template =>
			{
				ReportTemplates.Remove((OutputTemplate)template);
			},
			validationValue => validationValue is OutputTemplate);
		}

		public RelayCommand SetDbConnection
		{
			get => new RelayCommand(_ =>
			{
				string url = GetFileUrl(false);

				if (url != string.Empty)
					DbDirectory = url;
			});
		}
		#endregion

		public SettingsViewModel(Action closeWindow)
		{
			_closeWindow = closeWindow;

			_settings = SettingsStorage.GetStorage();

			// Уведомление представления при изменении значений в моделе
			_settings.PropertyChanged += (_, e) =>
			{
				if (e.PropertyName == nameof(_settings.DefaultWaybillTemplate))
					RaisePropertyChanged(nameof(DefaultWaybillTemplate));

				if (e.PropertyName == nameof(_settings.DefaultReportTemplate))
					RaisePropertyChanged(nameof(DefaultReportTemplate));

				if (e.PropertyName == nameof(_settings.DbDirectory))
					RaisePropertyChanged(nameof(DbDirectory));
			};
		}

		//возвращает файл/папку выбранные в проводнике Windows
		private string GetFileUrl(bool isFile, CommonFileDialogFilter filter = null)
		{
			CommonOpenFileDialog dialog = new CommonOpenFileDialog();

			if (!isFile)
				dialog.IsFolderPicker = true;

			if (isFile && filter != null)
				dialog.Filters.Add(filter);

			CommonFileDialogResult dialogResult = dialog.ShowDialog();

			string url = string.Empty;

			if (dialogResult == CommonFileDialogResult.Ok)
				url = dialog.FileName;

			return url;
		}
	}
}
