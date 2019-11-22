using System.Reflection;
using HalationGhost.WinApps.ImaZip.AppSettings;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace HalationGhost.WinApps.ImaZip
{
	/// <summary>
	/// MainWindowのViewModelを表します。
	/// </summary>
	public class MainWindowViewModel : HalationGhostViewModelBase
	{
		#region プロパティ

		/// <summary>
		/// MainWindowのタイトルを取得します。
		/// </summary>
		public ReactivePropertySlim<string> Title { get; }

		/// <summary>
		/// メニュー選択ボタンをドロップダウンするボタンのEnabledを取得します。
		/// </summary>
		public ReactivePropertySlim<bool> MenuSelectButtonEnabled { get; }

		#endregion

		#region コマンド

		/// <summary>
		/// 設定ViewqをLoadするコマンド。
		/// </summary>
		public ReactiveCommand<string> LoadSettingViews { get; }

		/// <summary>
		/// 設定ViewをLoadします。
		/// </summary>
		/// <param name="param">LoadするViewの種類を表す文字列。</param>
		private void onLoadSettingViews(string param)
		{
			this.regionManager.RequestNavigate("FileListArea", "ZipFileListPanel");

			switch (param)
			{
				case "AppendToZip":
					break;
				case "AppendToZipWithSave":
					break;
				case "CreateNewZip":
					break;
			}

			this.MenuSelectButtonEnabled.Value = false;
		}

		#endregion

		#region オーバーライド

		/// <summary>
		/// 自分自身をDisposeします。
		/// </summary>
		/// <param name="disposing">Dispose中かを表すbool。</param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			foreach (var region in this.regionManager.Regions)
			{
				region.RemoveAll();
			}

			// アプリケーション設定を保存
			AppSettingsService.SaveSettings((ImaZipCoreProto01Settings)this.appSettings);
		}

		#endregion

		#region コンストラクタ

		/// <summary>
		/// Prismのリージョンマネージャを表します。
		/// </summary>
		private IRegionManager regionManager = null;

		/// <summary>
		/// ImaZipのアプリケーション設定を表します。
		/// </summary>
		private IImaZipCoreProto01Settings appSettings = null;

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="regMan">Prismのリージョンマネージャを表すIRegionManager。</param>
		public MainWindowViewModel(IRegionManager regMan, IImaZipCoreProto01Settings imaZipSettings)
		{
			this.regionManager = regMan;
			this.appSettings = imaZipSettings;

			this.Title = new ReactivePropertySlim<string>(this.getApplicationTitle())
				.AddTo(this.disposable);

			this.MenuSelectButtonEnabled = new ReactivePropertySlim<bool>(true)
				.AddTo(this.disposable);

			this.LoadSettingViews = this.MenuSelectButtonEnabled
				.ToReactiveCommand<string>()
				.WithSubscribe(p => this.onLoadSettingViews(p))
				.AddTo(this.disposable);
		}

		/// <summary>
		/// MainWindowのタイトル文字列を取得します。
		/// </summary>
		/// <returns>MainWindowのタイトル文字列。</returns>
		private string getApplicationTitle()
		{
			var ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();

			return "ImaZipper Proto Ver." + ver;
		}

		#endregion
	}
}
