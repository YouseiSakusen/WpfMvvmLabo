using Prism.Ioc;
using Prism.Modularity;

namespace DapperSample
{
	public class DapperSampleApplicationLayerModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{

		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.Register<IRepositoryFactory, RepositoryFactory>();
		}
	}
}