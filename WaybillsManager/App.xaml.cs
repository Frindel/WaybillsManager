using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WaybillsManager.Model;
using WaybillsManager.Properties;

namespace WaybillsManager
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			this.ShutdownMode = ShutdownMode.OnMainWindowClose;

			var settings = SettingsStorage.GetStorage();

			while (settings.DbDirectory == null || settings.DbDirectory == string.Empty || !Directory.Exists(settings.DbDirectory))
			{
				new WaybillsManager.View.Form.Settings().ShowDialog();
			}

			AppLoader.Initialization();
		}
	}
}
