using System.Windows;
using HalationGhost.WinApps;
using Prism.DryIoc;
using Prism.Ioc;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace DapperSample
{
	/// <summary>
	/// MainWindowのVMを表します。
	/// </summary>
	public class MainWindowViewModel : HalationGhostViewModelBase
	{
		/// <summary>コンソールの出力内容を取得します。</summary>
		public ReadOnlyReactivePropertySlim<string> Console { get; }

		/// <summary>Dynamic型で取得ボタンコマンドを表します。</summary>
		public ReactiveCommand GetDynamic { get; }

		/// <summary>Dynamic型で取得ボタンコマンドを実行します。</summary>
		private void onGetDynamic()
		{
			var service = (Application.Current as PrismApplication)?.Container.Resolve<IDapperSampleService>();

			service?.ShowTopIdCharacters(this.buf);
		}

		/// <summary>コンソールのバッファを表します。</summary>
		private ConsoleBuffer buf = new ConsoleBuffer();

		/// <summary>コンストラクタ。</summary>
		public MainWindowViewModel()
		{
			this.GetDynamic = new ReactiveCommand()
				.WithSubscribe(() => this.onGetDynamic())
				.AddTo(this.disposable);

			this.Console = this.buf.ConsoleText
				.ToReadOnlyReactivePropertySlim()
				.AddTo(this.disposable);
		}
	}
}
