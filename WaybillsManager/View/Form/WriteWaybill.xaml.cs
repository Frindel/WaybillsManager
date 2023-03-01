using WaybillsManager.Model.Data.Entities;
using WaybillsManager.ViewModel;

namespace WaybillsManager.View.Form
{
	public partial class WriteWaybill : FormBase
	{
		public WriteWaybill(Waybill waybill) : base(waybill)
		{
			InitializeComponent();

			DataContext = new WriteWaybillViewModel(Close, waybill);
		}
	}
}
