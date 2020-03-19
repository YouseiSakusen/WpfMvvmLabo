using Prism.Ioc;
using Prism.Modularity;

namespace DapperSample
{
	/// <summary>DapperSampleApplicationLayerモジュールを表します。</summary>
	public class DapperSampleApplicationLayerModule : IModule
	{
		/// <summary>モジュールを初期化します。</summary>
		/// <param name="containerProvider"></param>
		public void OnInitialized(IContainerProvider containerProvider)
			=> DapperSampleDataAccessBase.InitializedSqlMapper();

		/// <summary>DIコンテナへ型を登録します。</summary>
		/// <param name="containerRegistry">登録用のDIコンテナを表すIContainerRegistry。</param>
		public void RegisterTypes(IContainerRegistry containerRegistry)
			=> containerRegistry.Register<IRepositoryFactory, RepositoryFactory>();
	}
}