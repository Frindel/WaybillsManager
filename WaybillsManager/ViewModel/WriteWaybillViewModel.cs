using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using WaybillsManager.Model;
using WaybillsManager.Model.Data;
using System;
using WaybillsManager.Model.Data.Entities;
using WaybillsManager.Model.Output;
using System.Windows;

namespace WaybillsManager.ViewModel
{
	class WriteWaybillViewModel : BindableBase
	{
		private SettingsStorage _settings;

		private Action _closeForm;

		private Waybill _writingWaybill;

		public ObservableCollection<OutputTemplate> Templates { get => _settings.WordTemplates; }

		public OutputTemplate DefaultWaybillTemplate { get=> _settings.DefaultWordTemplate; }

		public RelayCommand WriteWaybill
		{
			get => new RelayCommand(obj =>
			{
				OutputTemplate template = (OutputTemplate)obj;

				if (!OutputOperations.WriteWaybill(_writingWaybill, template))
				{
					MessageBox.Show("Шаблон не найден");
					return;
				}

				_closeForm();
			},
			obj => obj is OutputTemplate);
		}

		public WriteWaybillViewModel(Action closeForm, Waybill waybill)
		{
			_settings = SettingsStorage.GetStorage();

			_writingWaybill = waybill;

			_closeForm = closeForm;
		}
	}
}
