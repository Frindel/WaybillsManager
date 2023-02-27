using System.Collections.Generic;
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

		public RelayCommand RemoveWaybill
		{
			get => new RelayCommand(waybill=>
			{
				// todo: проверка отсутствия открытых форм редактирования для удаляемой путевки

				if (MessageBox.Show("Вы уверены, что хотите удалить путевку?", "Удаление путевки", MessageBoxButton.YesNo) == MessageBoxResult.No)
					return;

				Storage.RemoveWaybillAsync(waybill as Waybill);
			},
			waybill => waybill as Waybill !=null);
		}

		public AppWindowViewModel()
		{
			// создание контроллера форм
			_formController = FormController.GetController();

			Storage = WaybillsStorage.Get();
		}
	}
}
