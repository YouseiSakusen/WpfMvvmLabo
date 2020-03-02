using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace HalationGhost.WinApps.ImaZip.ZipBookCreator
{
	/// <summary>zipファイル作成アプリのMainWindowを表します。</summary>
	public class MainWindowViewModel : HalationGhostViewModelBase
	{
		/// <summary>ログを取得します。</summary>
		public ReadOnlyReactivePropertySlim<string> LogText { get; }

		#region ContentRenderedイベント

		/// <summary>ContentRenderedイベントCommand</summary>
		public AsyncReactiveCommand ContentRendered { get; }

		/// <summary>ContentRenderedイベントハンドラ。</summary>
		/// <returns>実行したTask。</returns>
		private async Task onContentRenderedAsync()
		{
			var args = Environment.GetCommandLineArgs();
			if (args.Length <= 1)
				return;

			var watch = new Stopwatch();
			watch.Start();

			//try
			//{
			//	Mouse.OverrideCursor = Cursors.Wait;

			//await new Creator().CreateZipBookAsync(args[1], this.relayStation);

			await new Creator().CreateBookZipAsync(args[1], this.relayStation);

			//await new CreatorTpl().CreateZipBookAsync(args[1], this.relayStation);

			watch.Stop();

			this.relayStation.AddLog($"実行時間 {watch.ElapsedMilliseconds} [ms]");
			this.relayStation.AddLog($"************ CreateBook Finished! ************");

			//Debug.WriteLine($"************ CreateBook Finished! ************");
			//MessageBox.Show("完了");
			//}
			//finally
			//{
			//	Mouse.OverrideCursor = null;
			//}
		}

		#endregion

		#region コンストラクタ

		/// <summary>Modelの情報を中継します。</summary>
		private CreatorRelayStation relayStation = null;

		/// <summary>コンストラクタ。</summary>
		public MainWindowViewModel()
		{
			this.relayStation = new CreatorRelayStation();

			this.LogText = this.relayStation.Log
				.ToReadOnlyReactivePropertySlim()
				.AddTo(this.disposable);

			this.ContentRendered = new AsyncReactiveCommand()
				.WithSubscribe(() => this.onContentRenderedAsync())
				.AddTo(this.disposable);
		}

		#endregion
	}
}
