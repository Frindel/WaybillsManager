using Microsoft.Win32;
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
		public string DbConnection { get => _settings.DbConnectionString; set => _settings.DbConnectionString = value; }

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
				OpenFileDialog fileDialog = new OpenFileDialog();
				fileDialog.Filter = "(*.doc, *.docx)|*.doc;*.docx";
				fileDialog.ShowDialog();

				string url = fileDialog.FileName;

				if (url == string.Empty)
					return;

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
				OpenFileDialog fileDialog = new OpenFileDialog();
				fileDialog.Filter = "(*.xls)|*.xls";
				fileDialog.ShowDialog();

				string url = fileDialog.FileName;

				if (url == string.Empty)
					return;

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
				OpenFileDialog fileDialog = new OpenFileDialog();
				fileDialog.Filter = "(*.db)|*.db";
				fileDialog.ShowDialog();

				string url = fileDialog.FileName;

				if (url == string.Empty)
					return;

				DbConnection = url;
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

				if (e.PropertyName == nameof(_settings.DbConnectionString))
					RaisePropertyChanged(nameof(DbConnection));
			};
		}

	}
}
