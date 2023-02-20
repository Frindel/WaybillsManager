using System.Collections.Generic;
using System.Reflection;
using WaybillsManager.View.Form;

namespace WaybillsManager.View
{
	public class FormController
	{
		private static FormController _controller;

		public Dictionary<string, ICollection<FormBase>> OpenForms { get; set; }

		private FormController()
		{
			OpenForms = new Dictionary<string, ICollection<FormBase>>();
		}

		public static FormController GetController()
		{
			if (_controller == null)
				_controller = new FormController();
			return _controller;
		}

		public void DisplayForm(FormBase form)
		{
			// получение названия типа формы
			string formType = form.GetType().GetCustomAttribute<FormTypeAttribute>()?.FormTypeName ?? string.Empty;

			// добавление формы в множество открытых форм
			if (formType != string.Empty)
			{
				if (!OpenForms.ContainsKey(formType))
					OpenForms.Add(formType, new List<FormBase>());

				OpenForms[formType].Add(form);
			}

			form.Closed += DeleteForm;

			form.Show();
		}

		// удаление формы из множества открытых форм при её закрытии
		private void DeleteForm(object? sender, System.EventArgs e)
		{
			FormBase form = sender as FormBase;

			if (form == null)
				return;

			// получение названия типа формы
			string formType = form.GetType().GetCustomAttribute<FormTypeAttribute>()?.FormTypeName ?? "";

			if (formType == string.Empty)
				return;

			OpenForms[formType].Remove(form);

			if (OpenForms[formType].Count == 0)
				OpenForms.Remove(formType);
		}
	}
}
