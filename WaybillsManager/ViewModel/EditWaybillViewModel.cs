using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WaybillsManager.Model;
using WaybillsManager.Model.Data.Entities;

namespace WaybillsManager.ViewModel
{
	internal class EditWaybillViewModel: BindableBase
	{
		private Action _closeWindow;

		private DateOnly _initialDate;
		private int _initialNumber;
		
		public Waybill Waybill { get; set; }

		public RelayCommand Close
		{
			get => new RelayCommand(_ =>
			{
				_closeWindow();
			});
		}

		public RelayCommand Add
		{
			get => new RelayCommand(_ =>
			{
				var a = WaybillsStorage.Get().Contains(Waybill);
				// проверка возможности добавить путевку
				if (!(Waybill.Date == _initialDate && Waybill.Number == _initialNumber) && WaybillsStorage.Get().Contains(Waybill))
				{
					MessageBox.Show("Путевка с данным номером в данном году уже существует.",
						"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				//// добавление путевки
				//WaybillsStorage.Get().AddWaybillAsync(Waybill, _form);
				//WaybillsStorage.Get().AddWaybillAsync(Waybill);
				WaybillsStorage.Get().EditWaybilllAsync(Waybill);

				// закрытие формы
				_closeWindow();
			},
			value => value == null || !(bool)value);
		}
		#region Hints
		
		public ObservableCollection<string> CarNames { get; }

		public ObservableCollection<string> StateNumbers { get; }

		public ObservableCollection<string> DriverNames { get; }

		public ObservableCollection<string> IdentityCards { get; }

		public ObservableCollection<string> RoutePoints { get; }

		#endregion

		public Car Car { get; } = new Car() { Name = "test" };

		public EditWaybillViewModel(Action closeForm, Waybill waybill)
		{
			_closeWindow = closeForm;

			Waybill = (Waybill)waybill.Clone();

			_initialDate = Waybill.Date;
			_initialNumber = Waybill.Number;

			HintsStorage hints = HintsStorage.Get();

			// получение подсказок для марки машины
			CarNames = hints.Hints[typeof(Car)];

			// получение подсказок для гос. номера
			StateNumbers = hints.Hints[typeof(CarStateNumber)];

			// получение подсказок для ФИО шафера
			DriverNames = hints.Hints[typeof(Driver)];

			// получение подсказок для удостоверения
			IdentityCards = hints.Hints[typeof(IdentityCard)];

			// получение подсказок для точек маршрута
			RoutePoints = hints.Hints[typeof(RoutePoint)];
		}

	}
}
