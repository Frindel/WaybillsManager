using Microsoft.Xaml.Behaviors.Core;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using WaybillsManager.Model;
using WaybillsManager.Model.Data.Entities;
using WaybillsManager.View.Form;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace WaybillsManager.ViewModel
{
	public class AddWaybillViewModel : BindableBase
	{
		private Action _closeWindow;
		private FormBase _form;

		public Waybill Waybill { get; }

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
				// проверка возможности добавить путевку
				if (WaybillsStorage.Get().Contains(Waybill))
				{
					MessageBox.Show("Путевка с данным номером в данном году уже существует.",
						"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				//// добавление путевки
				//WaybillsStorage.Get().AddWaybillAsync(Waybill, _form);
				WaybillsStorage.Get().AddWaybillAsync(Waybill);

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

		public AddWaybillViewModel(Action closeWindow, Waybill waybill)
		{
			_closeWindow = closeWindow;

			// создание шаблона для заполнения путевки
			Waybill = waybill;

			waybill.Date = DateOnly.FromDateTime(DateTime.Now);

			waybill.CarStateNumber = new CarStateNumber();
			waybill.Car = new Car();
			waybill.IdentityCard = new IdentityCard()
			{
				Driver = new Driver()
			};
			waybill.Route = new Route()
			{
				StartPoint = new RoutePoint(),
				EndPoint = new RoutePoint()
			};

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
