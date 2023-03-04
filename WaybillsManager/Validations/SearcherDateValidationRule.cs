using System;
using System.Windows.Controls;

namespace WaybillsManager.Validations
{
	class SearcherDateValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo ci)
		{
			if (value == null)
				return new ValidationResult(false, this);

			string valueStr = value.ToString();

			if (valueStr == string.Empty || DateOnly.TryParse(valueStr, out _))
				return new ValidationResult(true, this);

			return new ValidationResult(false, this);
		}
	}
}
