using System;

namespace WaybillsManager.View.Form
{
	[AttributeUsage(AttributeTargets.Class)]
	internal class FormTypeAttribute : Attribute
	{
		public string FormTypeName { get; }

		public FormTypeAttribute(string formTypeName)
		{
			FormTypeName = formTypeName;
		}
	}
}
