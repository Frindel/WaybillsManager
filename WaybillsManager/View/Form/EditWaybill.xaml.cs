using System.Windows.Controls;
using WaybillsManager.Model.Data.Entities;
using WaybillsManager.ViewModel;

namespace WaybillsManager.View.Form
{
	[FormType("EditWaybill")]
	public partial class EditWaybill : FormBase
	{
		public EditWaybill(Waybill editingWaybill) : base(editingWaybill)
		{
			InitializeComponent();

			DataContext = new EditWaybillViewModel(Close, editingWaybill);
		}
	}
}
