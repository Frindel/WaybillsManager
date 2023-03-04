using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using WaybillsManager.Model;
using WaybillsManager.Model.Data;
using WaybillsManager.Model.Data.Entities;
using WaybillsManager.Model.Output;
using WaybillsManager.View;
using WaybillsManager.View.Form;

namespace WaybillsManager.ViewModel
{
	internal class AppWindowViewModel : BindableBase
	{
		private FormController _formController;

		private SearchFormValues _searchFormValues;

		public WaybillsStorage Storage { get; set; }

		public ObservableCollection<int> Periods { get => Storage.WaybillsPeriods; }

		public SearchFormValues SearchFormValue
		{
			get => _searchFormValues;
			set
			{
				_searchFormValues = value;
				RaisePropertyChanged(nameof(SearchFormValue));
			}
		}

		#region Hints

		public ObservableCollection<string> CarNames { get; }

		public ObservableCollection<string> StateNumbers { get; }

		public ObservableCollection<string> DriverNames { get; }

		public ObservableCollection<string> RoutePoints { get; }

		#endregion

		#region Commands

		public RelayCommand UploadDb
		{
			get => new RelayCommand(_ =>
			{
				string url = GetFileUrl(false);

				if (url != string.Empty)
					OutputOperations.UploadDb(url);
			});
		}

		public RelayCommand OpenReference
		{
			get => new RelayCommand(_ =>
			{
				try
				{
					System.Diagnostics.Process txt = new System.Diagnostics.Process();
					txt.StartInfo.FileName = "notepad.exe";
					txt.StartInfo.Arguments = $"{Environment.CurrentDirectory}\\Reference.txt";
					txt.Start();
				}
				catch (Exception e) 
				{ }
			});

		}

		public RelayCommand OpenSettings
		{
			get => new RelayCommand(_ =>
			{
				_formController.DisplayForm(new Settings());
			});
		}


		public RelayCommand AddWaybill
		{
			get => new RelayCommand(_ =>
			{
				_formController.DisplayForm(new AddWaybill());
			});
		}

		public RelayCommand EditWaybill
		{
			get => new RelayCommand(obj =>
			{
				Waybill waybill = (Waybill)obj;

				_formController.DisplayForm(new EditWaybill(waybill));
			},
			obj => obj is Waybill waybill && (!_formController.OpenForms.ContainsKey("EditWaybill") || _formController.OpenForms["EditWaybill"].Where(f => f?.Waybill.Id == waybill.Id).FirstOrDefault() == null));
		}

		public RelayCommand RemoveWaybill
		{
			get => new RelayCommand(waybill =>
			{
				// todo: проверка отсутствия открытых форм редактирования для удаляемой путевки

				if (MessageBox.Show("Вы действительно хотите удалить путевку?", "Удаление путевки", MessageBoxButton.YesNo) == MessageBoxResult.No)
					return;

				Storage.RemoveWaybillAsync(waybill as Waybill);
			},
			obj => obj is Waybill waybill && (!_formController.OpenForms.ContainsKey("EditWaybill") || _formController.OpenForms["EditWaybill"].Where(f => f?.Waybill.Id == waybill.Id).FirstOrDefault() == null));
		}

		public RelayCommand WriteWaybill
		{
			get => new RelayCommand(obj =>
			{
				Waybill waybill = (Waybill)obj;

				_formController.DisplayForm(new WriteWaybill(waybill));
			},
			obj => obj is Waybill waybill && (!_formController.OpenForms.ContainsKey("EditWaybill") || _formController.OpenForms["EditWaybill"].Where(f => f?.Waybill.Id == waybill.Id).FirstOrDefault() == null));
		}

		public RelayCommand Search
		{
			get => new RelayCommand(_ =>
			{
				Searcher.Search(Storage, _searchFormValues);
			});
		}

		public RelayCommand CancelSort
		{
			get => new RelayCommand(_ =>
			{
				Searcher.Cancel(Storage);
				CleanSearchForm();
			},
			_ => CollectionViewSource.GetDefaultView(Storage).Filter != null);
		}

		public RelayCommand Report
		{
			get => new RelayCommand(_ =>
			{
				_formController.DisplayForm(new Report());
			});
		}

		#endregion

		public AppWindowViewModel()
		{
			// создание контроллера форм
			_formController = FormController.GetController();

			Storage = WaybillsStorage.Get();

			_searchFormValues = new SearchFormValues();

			HintsStorage hints = HintsStorage.Get();

			// получение подсказок для марки машины
			CarNames = hints.Hints[typeof(Car)];

			// получение подсказок для гос. номера
			StateNumbers = hints.Hints[typeof(CarStateNumber)];

			// получение подсказок для ФИО шафера
			DriverNames = hints.Hints[typeof(Driver)];

			// получение подсказок для точек маршрута
			RoutePoints = hints.Hints[typeof(RoutePoint)];
		}

		private void CleanSearchForm()
		{
			SearchFormValue = new SearchFormValues();
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
