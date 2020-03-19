using System.Collections.Generic;

namespace DapperSample
{
	/// <summary>サンプルアプリ用のサービスを表します。</summary>
	public class DapperSampleService : IDapperSampleService
	{
		/// <summary>フリガナ順でトップ10のキャラクターをコンソールに表示します。</summary>
		/// <param name="console">表示するコンソールを表すConsoleBuffer。</param>
		public void ShowTopFuriganaCharacters(ConsoleBuffer console)
		{
			console.Clear();

			List<BleachCharacter> characters = null;

			using (var repository = this.factory.CreateCharacterRepository())
			{
				characters = repository.GetTop10Furigana();
			}

			foreach (var chara in characters)
			{
				console.AppendLineToBuffer($"ID: {chara.Id}  名前: {chara.Name}  かな: {chara.Furigana}  " +
					$"誕生日: {chara.Birthday}  所属: {chara.OrganizationName}  斬魄刀: {chara.ZanpakutouName}");


			}
		}

		/// <summary>ID順でトップ10のキャラクターをコンソールに表示します。</summary>
		/// <param name="console">表示するコンソールを表すConsoleBuffer。</param>
		public void ShowTopIdCharacters(ConsoleBuffer console)
		{
			console.Clear();

			IEnumerable<dynamic> characters = null;

			using (var repository = this.factory.CreateCharacterRepository())
			{
				characters = repository.GetTop10Id();
			}

			foreach (var chara in characters)
			{
				console.AppendLineToBuffer($"ID: {chara.ID}  名前: {chara.CHARACTER_NAME}  かな: {chara.KANA}  " +
					$"誕生日: {chara.BIRTHDAY}  所属: {chara.ORGANIZATION_NAME}  斬魄刀: {chara.ZANPAKUTOU_NAME}");
			}
		}

		/// <summary>護廷十三隊別にキャラクターをコンソールに表示します。</summary>
		/// <param name="console">表示するコンソールを表すConsoleBuffer。</param>
		public void ShowCharactersByParty(ConsoleBuffer console)
		{
			console.Clear();

			List<SoulSocietyParty> parties = null;

			using (var repository = this.factory.CreateCharacterRepository())
			{
				parties = repository.GetCharactersByParty();
			}

			foreach (var party in parties)
			{
				console.AppendLineToBuffer($"隊ID: {party.PartyId}　隊名: {party.PartyName}");

				foreach (var chara in party.PartyMembers)
				{
					console.AppendLineToBuffer($"\tID: {chara.Id}  名前: {chara.Name}　斬魄刀: {chara.ZanpakutouName}　卍解: {chara.BankaiName}");
				}
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