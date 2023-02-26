using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WaybillsManager.Validations
{
    class NumberIsNotNullValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo ci)
		{
			if (value == null)
				return new ValidationResult(false, this);

			int number = int.Parse(value.ToString());
			return new ValidationResult(number!=0,this);
		}
	}
}
