using System;
using System.Reflection;
using System.Windows;
using HalationGhost.WinApps.ImaZip.AppSettings;
using HalationGhost.WinApps.Services;
using HalationGhost.WinApps.Services.CommonDialogs;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;

namespace HalationGhost.WinApps.ImaZip
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		#region オーバーライドメソッド

		/// <summary>
		/// ViewModelLocatorを設定します。
		/// </summary>
		protected override void ConfigureViewModelLocator()
		{
			base.ConfigureViewModelLocator();

			ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(vt =>
			{
				var viewName = vt.FullName;
				var asmName = vt.GetTypeInfo().Assembly.FullName;
				var vmName = $"{viewName}ViewModel, {asmName}";

				return Type.GetType(vmName);
			});
		}

		/// <summary>
		/// Prismのモジュールを設定します。
		/// </summary>
		/// <param name="moduleCatalog">モジュールカタログを表すIModuleCatalog。</param>
		protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
		{
			moduleCatalog.AddModule<ImaZipSettingPanelsModule>();
			moduleCatalog.AddModule<HalationGhostMessageBoxServiceModule>();
		}

		/// <summary>
		/// シェルを作成します。
		/// </summary>
		/// <returns>作成したシェルを表すWindow。</returns>
		protected override Window CreateShell()
		{
			return Container.Resolve<MainWindow>();
		}

		/// <summary>
		/// DIコンテナへ型を登録します。
		/// </summary>
		/// <param name="containerRegistry">登録用のDIコンテナを表すIContainerRegistry。</param>
		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterDialogWindow<ImaZipCoreProto01DialogWindow>();
			containerRegistry.RegisterSingleton<ICommonDialogService, CommonDialogService>();
			containerRegistry.RegisterInstance(AppSettingsService.LoadSettings());
		}

		#endregion
	}
}
