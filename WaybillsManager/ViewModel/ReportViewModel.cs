using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WaybillsManager.Model;
using WaybillsManager.Model.Data;
using WaybillsManager.Model.Output;

namespace WaybillsManager.ViewModel
{
	class ReportViewModel
	{
		private SettingsStorage _settings;

		private Action _closeForm;

		public DateOnly BeginDate { get; set; }
		public DateOnly EndDate { get; set; }

		public ObservableCollection<OutputTemplate> Templates { get => _settings.ReportTemplates; }

		public OutputTemplate DefaultReportTemplate { get => _settings.DefaultReportTemplate; }

		public RelayCommand Create
		{
			get => new RelayCommand(obj =>
			{
				if (!OutputOperations.WriteReport((OutputTemplate)obj, BeginDate, EndDate))
				{
					MessageBox.Show("Шаблон не найден");
					return;
				}

				_closeForm();
			},
			obj =>
			BeginDate<EndDate && obj is OutputTemplate);
		}

		public RelayCommand Close
		{
			get => new RelayCommand(_=>
			{
				_closeForm();
			});
		}

		public ReportViewModel(Action closeForm)
		{
			_closeForm = closeForm;

			_settings = SettingsStorage.GetStorage();

			BeginDate = DateOnly.FromDateTime(DateTime.Now);
			EndDate = DateOnly.FromDateTime(DateTime.Now);
		}
	}
}
