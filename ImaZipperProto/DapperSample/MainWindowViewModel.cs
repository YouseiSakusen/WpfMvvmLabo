using System.Threading.Tasks;
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

		/// <summary>Dynamic型でキャラクターを取得します。</summary>
		public ReactiveCommand GetDynamic { get; }

		/// <summary>BleachCharacter型でキャラクターを取得します。</summary>
		public ReactiveCommand GetBleachCharacter { get; }

		/// <summary>護廷十三隊別にキャラクターを取得します。</summary>
		public ReactiveCommand GetCharacterByParty { get; }

		/// <summary>キャラクターを登録します。</summary>
		public AsyncReactiveCommand InsertCharacter { get; }

		/// <summary>コンソールをクリアします。</summary>
		public ReactiveCommand ClearConsole { get; }

		/// <summary>Dynamic型で取得ボタンコマンドを実行します。</summary>
		private void onGetDynamic()
		{
			var service = (Application.Current as PrismApplication)?.Container.Resolve<IDapperSampleService>();

			service?.ShowTopIdCharacters(this.buf);
		}

		/// <summary>フリガナ順でトップ10のキャラクターをコンソールに表示します。</summary>
		private void onGetBleachCharacter()
		{
			var service = (Application.Current as PrismApplication)?.Container.Resolve<IDapperSampleService>();

			service?.ShowTopFuriganaCharacters(this.buf);
		}

		/// <summary>護廷十三隊別にキャラクターをコンソールに表示します。</summary>
		private void onGetCharacterByParty()
		{
			var service = (Application.Current as PrismApplication)?.Container.Resolve<IDapperSampleService>();

			service?.ShowCharactersByParty(this.buf);
		}

		/// <summary>キャラクターを登録します。</summary>
		/// <returns>非同期処理の結果を表すTask。</returns>
		private async Task onInsertCharacter()
		{
			var service = (Application.Current as PrismApplication)?.Container.Resolve<IDapperSampleService>();

			await service?.ShowInsertCharacterAsync(this.buf);
		}

		/// <summary>コンソールのバッファを表します。</summary>
		private ConsoleBuffer buf = new ConsoleBuffer();

		/// <summary>コンストラクタ。</summary>
		public MainWindowViewModel()
		{
			this.GetDynamic = new ReactiveCommand()
				.WithSubscribe(() => this.onGetDynamic())
				.AddTo(this.disposable);
			this.GetBleachCharacter = new ReactiveCommand()
				.WithSubscribe(() => this.onGetBleachCharacter())
				.AddTo(this.disposable);
			this.GetCharacterByParty = new ReactiveCommand()
				.WithSubscribe(() => this.onGetCharacterByParty())
				.AddTo(this.disposable);
			this.InsertCharacter = new AsyncReactiveCommand()
				.WithSubscribe(() => this.onInsertCharacter())
				.AddTo(this.disposable);
			this.ClearConsole = new ReactiveCommand()
				.WithSubscribe(() => this.buf.Clear())
				.AddTo(this.disposable);

			this.Console = this.buf.ConsoleText
				.ToReadOnlyReactivePropertySlim()
				.AddTo(this.disposable);
		}
	}
}
