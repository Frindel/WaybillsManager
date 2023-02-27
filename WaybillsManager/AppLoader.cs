using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WaybillsManager.Model;
using WaybillsManager.Model.Data;
using WaybillsManager.View;
using WaybillsManager.Model.Data.Entities;

namespace WaybillsManager
{
	public static class AppLoader
	{
		public static void Initialization()
		{
			// создание хранилища путевок
			WaybillsStorage.Get(200);

			// создание источников подсказок
			HintsStorage hints = HintsStorage.Get();

			// получение марок машин
			hints.SetGetHintsFunc(typeof(Car), () =>
			{
				using (ApplicationContext context = new ApplicationContext())
				{
					return context.Cars.Select(d => d.Name).ToArray();
				}
			});
			
			// получение гос. номеров машин
			hints.SetGetHintsFunc(typeof(CarStateNumber), () =>
			{
				using (ApplicationContext context = new ApplicationContext())
				{
					return context.CarStateNumbers.Select(csn => csn.Number).ToArray();
				}
			});

			// получение ФИО водителей
			hints.SetGetHintsFunc(typeof(Driver), () => 
			{
				using (ApplicationContext context = new ApplicationContext())
				{
					return context.Drivers.Select(d => d.Name).ToArray();
				}
			});

			// получение удостоверений водителей
			hints.SetGetHintsFunc(typeof(IdentityCard), () =>
			{
				using (ApplicationContext context = new ApplicationContext())
				{
					return context.IdentityCards.Select(ic => ic.Number).ToArray();
				}
			});

			// получение точек маршрута
			hints.SetGetHintsFunc(typeof(RoutePoint), () =>
			{
				using (ApplicationContext context = new ApplicationContext())
				{
					return context.RoutePoints.Select(rp=>rp.Name).ToArray();
				}
			});
		}

	}
}
