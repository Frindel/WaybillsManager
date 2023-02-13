using System;
using System.Windows;

namespace WaybillsManager.View.Form
{
	// значение заполненной формы
	public class FormFilledArgs : EventArgs
	{
		public object FormValue { get; set; }

		public FormFilledArgs(object value)
		{
			FormValue = value;
		}
	}


	public delegate void FormDelegate(object sender, FormFilledArgs args);

	// основа формы
	public abstract class FormBase : Window
	{
		public event FormDelegate FormFilled;

		// вызов события заполнения формы
		protected virtual void Fill<T>(T value)
		{
			if (FormFilled != null)
			{
				FormFilled.Invoke(this, new FormFilledArgs(value));
			}
		}
	}
}
