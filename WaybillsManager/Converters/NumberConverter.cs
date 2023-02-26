using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml;

namespace WaybillsManager.Converters
{
	class NumberConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int number = (int)value;
			if (number == 0)
				return string.Empty;
			return number.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value.ToString() == string.Empty)
				return 0;

			MethodInfo parseMethod = targetType.GetMethod("Parse", new[] { typeof(string) });

			return parseMethod.Invoke(null, new object[] { value});
		}
	}
}
