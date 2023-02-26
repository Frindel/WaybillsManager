using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WaybillsManager.Validations
{
	internal class StringNotEmptyValidationRule:ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo ci)
		{
			if (value == null)
				return new ValidationResult(false, null);

			string str = value.ToString();

			return (str == string.Empty) ? new ValidationResult(false, null) : new ValidationResult(true, null);
		}
	}
}
