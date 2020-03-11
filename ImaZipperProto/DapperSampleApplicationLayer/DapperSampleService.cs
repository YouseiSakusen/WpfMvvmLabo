using System.Collections.Generic;

namespace DapperSample
{
	/// <summary>サンプルアプリ用のサービスを表します。</summary>
	public class DapperSampleService : IDapperSampleService
	{
		/// <summary>ID順でトップ10のキャラクターをコンソールに表示します。</summary>
		/// <param name="console">表示するコンソールを表すConsoleBuffer。</param>
		public void ShowTopIdCharacters(ConsoleBuffer console)
		{
			IEnumerable<dynamic> characters = null;

			using (var repository = this.factory.CreateCharacterRepository())
			{
				characters = repository.GetTopIdCharacters();
			}

			foreach (var chara in characters)
			{
				console.AppendLineToBuffer($"ID: {chara.ID}  名前: {chara.CHARACTER_NAME}  かな: {chara.KANA}  " +
					$"誕生日: {chara.BIRTHDAY}  所属: {chara.ORGANIZATION_NAME}  斬魄刀: {chara.ZANPAKUTOU_NAME}");
			}
		}

		/// <summary>リポジトリのファクトリを表します。</summary>
		private IRepositoryFactory factory = null;

		/// <summary>コンストラクタ。</summary>
		/// <param name="repositoryFactory"></param>
		public DapperSampleService(IRepositoryFactory repositoryFactory)
		{
			this.factory = repositoryFactory;
		}
	}
}