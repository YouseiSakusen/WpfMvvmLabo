using HalationGhost.WinApps.ImaZip.ImageFileSettings;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace HalationGhost.WinApps.ImaZip
{
	public class ImaZipSettingPanelsModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{

		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<ZipFileListPanel, ZipFileListPanelViewModel>();
		}
	}
}