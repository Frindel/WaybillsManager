using System.Windows.Controls;

namespace WaybillsManager.Validations
{
	class SearcherPeriodValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo ci)
		{
			if (value == null)
				return new ValidationResult(true, this);

			string valueStr = value.ToString();

			if (value == string.Empty|| uint.TryParse(valueStr, out _))
				return new ValidationResult(true, this);
			
			return new ValidationResult(false, this);
		}

	}
}
