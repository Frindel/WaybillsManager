using System.Linq;
using System.Windows;
using WaybillsManager.Model;
using WaybillsManager.Model.Data.Entities;
using WaybillsManager.View;
using WaybillsManager.View.Form;

namespace WaybillsManager.ViewModel
{
	internal class AppWindowViewModel
	{
		private FormController _formController;

		public WaybillsStorage Storage { get; set; }

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

				if (MessageBox.Show("Вы уверены, что хотите удалить путевку?", "Удаление путевки", MessageBoxButton.YesNo) == MessageBoxResult.No)
					return;

				Storage.RemoveWaybillAsync(waybill as Waybill);
			},
			obj => obj is Waybill waybill && (!_formController.OpenForms.ContainsKey("EditWaybill") || _formController.OpenForms["EditWaybill"].Where(f => f?.Waybill.Id == waybill.Id).FirstOrDefault() == null));
		}

		public RelayCommand WriteWaybill
		{
			get => new RelayCommand(obj=>
			{
				Waybill waybill = (Waybill)obj;
				
				_formController.DisplayForm(new WriteWaybill(waybill));
			},
			obj => obj is Waybill waybill && (!_formController.OpenForms.ContainsKey("EditWaybill") || _formController.OpenForms["EditWaybill"].Where(f => f?.Waybill.Id == waybill.Id).FirstOrDefault() == null));
		}

		public AppWindowViewModel()
		{
			// создание контроллера форм
			_formController = FormController.GetController();

			Storage = WaybillsStorage.Get();
		}
	}
}
