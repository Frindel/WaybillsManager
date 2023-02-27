using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WaybillsManager
{
	public partial class AppWindow : Window
	{
		bool scrollPending;

		public AppWindow()
		{
			InitializeComponent();

			DataContext = new ViewModel.AppWindowViewModel();

			// устанавливает вниз позицию ползунка полосы прокрутки
			waybillsList.Loaded += (_, _) =>
			{
				if (waybillsList.Items.Count > 0)
				{
					var border = VisualTreeHelper.GetChild(waybillsList, 0) as Decorator;
					if (border != null)
					{
						var scroll = border.Child as ScrollViewer;
						if (scroll != null) scroll.ScrollToEnd();
					}
				}
			};
		}
	}
}
