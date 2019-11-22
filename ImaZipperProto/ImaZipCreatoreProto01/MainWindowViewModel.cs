using System.Reflection;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace HalationGhost.WinApps.ImaZip.Creator
{
	public class MainWindowViewModel : HalationGhostViewModelBase
	{
		public ReactivePropertySlim<string> Title { get; }

		#region コンストラクタ

		public MainWindowViewModel()
		{
			this.Title = new ReactivePropertySlim<string>(this.getApplicationTitle())
				.AddTo(this.disposable);
		}

		/// <summary>
		/// MainWindowのタイトル文字列を取得します。
		/// </summary>
		/// <returns>MainWindowのタイトル文字列。</returns>
		private string getApplicationTitle()
		{
			var ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();

			return "ImaZipCreator Proto Ver." + ver;
		}

		#endregion
	}
}
