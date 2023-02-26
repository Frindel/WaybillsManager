using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;
using System.Reflection;

namespace WaybillsManager.Validations
{
	internal class CanParseValidationRule: ValidationRule
	{
		public string ValidationType { get; set; }

		public override ValidationResult Validate(object value, System.Globalization.CultureInfo ci)
		{
			if (ValidationType==string.Empty)
				throw new NullReferenceException("ValidationType property have null value");

			Type validationType = Type.GetType($"System.{ValidationType}");

			MethodInfo parseMethod = validationType.GetMethod("Parse", new[] { typeof(string) });

			if ((validationType == typeof(TimeSpan)||validationType== typeof(DateTime))
				&& value.ToString().Split(':').Length!=3)
				return new ValidationResult(false, null);

			try
			{
				parseMethod.Invoke(null,new object[] {value.ToString()});
			}
			catch (Exception)
			{
				return new ValidationResult(false, null);
			}

			return new ValidationResult(true, null);
		}

	}
}
