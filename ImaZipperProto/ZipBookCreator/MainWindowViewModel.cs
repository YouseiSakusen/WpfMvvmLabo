using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HalationGhost.WinApps.ImaZip.ZipBookCreator
{
	public class MainWindowViewModel : HalationGhostViewModelBase
	{
		public ReadOnlyReactivePropertySlim<string> LogText { get; }

		#region ContentRenderedイベント

		public AsyncReactiveCommand ContentRendered { get; }

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

		private CreatorRelayStation relayStation = null;

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
