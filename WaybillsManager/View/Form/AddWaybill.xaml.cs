using WaybillsManager.Model.Data.Entitys;
using WaybillsManager.ViewModel;

namespace WaybillsManager.View.Form
{
	[FormType("AddWaybill")]
	public partial class AddWaybill : FormBase
	{
		public Waybill Waybill { get; }

		public AddWaybill()
		{
			Waybill = new Waybill();

			InitializeComponent();

			DataContext = new AddWaybillViewModel(Close, Waybill);
		}
	}
}
