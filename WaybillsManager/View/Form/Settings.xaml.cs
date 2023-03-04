using System.Collections.ObjectModel;
using WaybillsManager.Model.Data;
using WaybillsManager.View.Form;
using WaybillsManager.ViewModel;
using System;
using System.Collections.Specialized;

namespace WaybillsManager.View.Form
{
	public partial class Settings : FormBase
	{
		public Settings()
		{
			InitializeComponent();
			SettingsViewModel vm = new SettingsViewModel(
				() => Close()
			);
			DataContext= vm;
		}
	}
}
