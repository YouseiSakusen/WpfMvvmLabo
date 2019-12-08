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
		#region ContentRenderedイベント

		public AsyncReactiveCommand ContentRendered { get; }

		private async Task onContentRendered()
		{
			var args = Environment.GetCommandLineArgs();
			if (args.Length <= 1)
				return;

			try
			{
				Mouse.OverrideCursor = Cursors.Wait;

				await new Creator().CreateZipBookAsync(args[1], this.relayStation);
				MessageBox.Show("完了");
			}
			finally
			{
				Mouse.OverrideCursor = null;
			}
		}

		#endregion

		#region コンストラクタ

		private CreatorRelayStation relayStation = null;

		public MainWindowViewModel()
		{
			this.relayStation = new CreatorRelayStation();

			this.ContentRendered = new AsyncReactiveCommand()
				.WithSubscribe(() => this.onContentRendered())
				.AddTo(this.disposable);
		}

		#endregion
	}
}
