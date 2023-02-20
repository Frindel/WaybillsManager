using System.Windows;

namespace WaybillsManager
{
	public partial class AppWindow : Window
	{
		public AppWindow()
		{
			InitializeComponent();

			AppLoader.Initialization();

			DataContext = new ViewModel.AppWindowViewModel();
		}
	}
}
