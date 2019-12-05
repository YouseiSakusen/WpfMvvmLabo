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

namespace HalationGhost.WinApps.ImaZip.ZipBookCreator
{
	public class MainWindowViewModel : HalationGhostViewModelBase
	{
		#region ContentRenderedイベント

		public AsyncReactiveCommand ContentRendered { get; }

		private Task onContentRendered()
		{
			var args = Environment.GetCommandLineArgs();
			if (args.Length <= 1)
				return Task.CompletedTask;

			return new Creator().CreateZipBookAsync(args[1], this.relayStation);
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
