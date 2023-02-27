using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WaybillsManager.Model.Data.Entities;

namespace WaybillsManager.Converters
{
	internal class RouteConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Route route = (Route)value;
			if (route.EndPoint == null)
				return route.StartPoint.Name;
			return $"{route.StartPoint.Name} - {route.EndPoint.Name}";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
