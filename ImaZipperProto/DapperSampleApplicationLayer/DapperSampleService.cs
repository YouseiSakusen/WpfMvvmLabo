using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperSample
{
	/// <summary>サンプルアプリ用のサービスを表します。</summary>
	public class DapperSampleService : IDapperSampleService
	{
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

		/// <summary>Insertしたキャラクターを表示します。</summary>
		/// <param name="console">表示するコンソールを表すConsoleBuffer。</param>
		/// <returns>非同期処理の結果を表すTask。</returns>
		public async Task ShowInsertCharacterAsync(ConsoleBuffer console)
		{
			console.Clear();

			IEnumerable<BleachCharacter> newCharacters = null;

			using (var repository = this.factory.CreateCharacterRepository())
			{
				var seq = repository.GetCharacterSeq();

				using (var tran = repository.BeginTransaction())
				{
					var characters = this.createSavesCharacters();

					try
					{
						await repository.DeleteCharactersByKanaAsync(characters)
							.ContinueWith(c => repository.RegistCharactersAsync(characters))
							.ContinueWith(c => tran.CommitAsync());
					}
					catch (System.Exception)
					{
						tran.Rollback();
						throw;
					}
				}

				newCharacters = await repository.GetCharactersByIdOrverAsync(seq);
			}

			foreach (var chara in newCharacters)
			{
				console.AppendLineToBuffer($"ID: {chara.Id}  名前: {chara.Name}　ふりがな: {chara.Furigana}　所属: {chara.OrganizationName}");
			}
		}

		/// <summary>登録用のキャラクターリストを生成します。</summary>
		/// <returns>生成した登録用のキャラクターリストを表すList<BleachCharacter>。</returns>
		private List<BleachCharacter> createSavesCharacters()
		{
			return new List<BleachCharacter>()
			{
				new BleachCharacter("麒麟寺 天示郎", "きりんじ てんじろう", 14),
				new BleachCharacter("曳舟 桐生", "ひきふね きりお", 14),
				new BleachCharacter("二枚屋 王悦", "にまいや おうえつ", 14),
				new BleachCharacter("修多羅 千手丸", "しゅたら せんじゅまる", 14),
				new BleachCharacter("兵主部 一兵衛", "ひょうすべ いちべえ", 14)
			};
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