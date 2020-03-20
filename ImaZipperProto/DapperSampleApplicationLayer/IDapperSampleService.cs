using System.Threading.Tasks;

namespace DapperSample
{
	/// <summary>サンプルアプリ用のサービスインタフェースを表します。</summary>
	public interface IDapperSampleService
	{
		/// <summary>ID順でトップ10のキャラクターをコンソールに表示します。</summary>
		/// <param name="console">表示するコンソールを表すConsoleBuffer。</param>
		public void ShowTopIdCharacters(ConsoleBuffer console);

		/// <summary>フリガナ順でトップ10のキャラクターをコンソールに表示します。</summary>
		/// <param name="console">表示するコンソールを表すConsoleBuffer。</param>
		public void ShowTopFuriganaCharacters(ConsoleBuffer console);

		/// <summary>護廷十三隊別にキャラクターをコンソールに表示します。</summary>
		/// <param name="console">表示するコンソールを表すConsoleBuffer。</param>
		public void ShowCharactersByParty(ConsoleBuffer console);

		/// <summary>Insertしたキャラクターを表示します。</summary>
		/// <param name="console">表示するコンソールを表すConsoleBuffer。</param>
		/// <returns>非同期処理の結果を表すTask。</returns>
		public Task ShowInsertCharacterAsync(ConsoleBuffer console);
	}
}
