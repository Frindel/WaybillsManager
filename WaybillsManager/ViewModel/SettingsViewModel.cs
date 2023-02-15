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

		public ObservableCollection<OutputTemplate> WordTemplates { get => _settings.WordTemplates; }

		public ObservableCollection<OutputTemplate> ExcelTemplates { get => _settings.ExcelTemplates; }

		public OutputTemplate DefaultWordTemplate { get => _settings.DefaultWordTemplate; set => _settings.DefaultWordTemplate = value; }

		public OutputTemplate DefaultExcelTemplate { get => _settings.DefaultExcelTemplate; set => _settings.DefaultExcelTemplate = value; }
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

		public ActionCommand AddWordTemplate
		{
			get => new ActionCommand(() =>
			{
				string url = GetFileUrl(true, new CommonFileDialogFilter(string.Empty, "*.doc;*.docx"));

				if (url != string.Empty)
					WordTemplates.Add(OutputTemplate.GetTemplateByUrl(url));
			});
		}

		public RelayCommand RemoveWordTeemplate
		{
			get => new RelayCommand(template =>
			{
				WordTemplates.Remove((OutputTemplate)template);
			},
			validationValue => validationValue is OutputTemplate);
		}

		public ActionCommand AddExcelTemplate
		{
			get => new ActionCommand(() =>
			{
				string url = GetFileUrl(true, new CommonFileDialogFilter(string.Empty, ".xls"));

				if (url!=string.Empty)
					ExcelTemplates.Add(OutputTemplate.GetTemplateByUrl(url));
			});
		}

		public RelayCommand RemoveExcelTeemplate
		{
			get => new RelayCommand(template =>
			{
				ExcelTemplates.Remove((OutputTemplate)template);
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
				if (e.PropertyName == nameof(_settings.DefaultWordTemplate))
					RaisePropertyChanged(nameof(DefaultWordTemplate));

				if (e.PropertyName == nameof(_settings.DefaultExcelTemplate))
					RaisePropertyChanged(nameof(DefaultExcelTemplate));

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
