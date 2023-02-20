using System.Collections.Generic;
using WaybillsManager.Model;
using WaybillsManager.Model.Data.Entitys;
using WaybillsManager.View;
using WaybillsManager.View.Form;

namespace WaybillsManager.ViewModel
{
	internal class AppWindowViewModel
	{
		private FormController _formController;

		public WaybillsStorage Storage { get; }

		public RelayCommand OpenSettings
		{
			get => new RelayCommand(_ =>
			{
				_formController.DisplayForm(new Settings());
			});
		}

		List<Waybill> waybills = new List<Waybill>();
		public AppWindowViewModel()
		{
			// создание контроллера форм
			_formController = FormController.GetController();

			// создание хранилища путевок
			Storage = new WaybillsStorage(800);
		}
	}
}
