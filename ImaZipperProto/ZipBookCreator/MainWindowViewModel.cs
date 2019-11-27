using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

			//return Task.Run(async () => await )
			return Task.CompletedTask;
		}

		#endregion

		public MainWindowViewModel()
		{
			this.ContentRendered = new AsyncReactiveCommand()
				.WithSubscribe(() => this.onContentRendered())
				.AddTo(this.disposable);
		}
	}
}
