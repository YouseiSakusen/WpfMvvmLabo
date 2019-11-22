using Prism.Ioc;
using HalationGhost.WinApps.ImaZip.Views;
using System.Windows;

namespace HalationGhost.WinApps.ImaZip
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		protected override Window CreateShell()
		{
			return Container.Resolve<MainWindow>();
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{

		}
	}
}
