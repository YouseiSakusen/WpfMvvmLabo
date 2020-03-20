using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DapperSample
{
	/// <summary>キャラクター用リポジトリを表します。</summary>
	public class CharacterRepository : DapperSampleDataAccessBase, ICharacterRepository
	{
		/// <summary>護廷十三隊別にキャラクターを取得します。</summary>
		/// <returns>取得した護廷十三隊を表すList<SoulSocietyParty>。</returns>
		public List<SoulSocietyParty> GetCharactersByParty()
		{
			var sql = new StringBuilder(1000);
			sql.AppendLine(" SELECT ");
			sql.AppendLine(" 	  ORG.ID AS PartyId ");
			sql.AppendLine(" 	, ORG.ORGANIZATION_NAME AS PartyName ");
			sql.AppendLine(" 	, CHR.ID AS Id ");
			sql.AppendLine(" 	, CHR.CHARACTER_NAME AS Name ");
			sql.AppendLine(" 	, ZPT.ZANPAKUTOU_NAME AS ZanpakutouName ");
			sql.AppendLine(" 	, ZPT.BANKAI_NAME AS BankaiName ");
			sql.AppendLine(" FROM ");
			sql.AppendLine(" 	( ");
			sql.AppendLine(" 	SELECT ");
			sql.AppendLine(" 		* ");
			sql.AppendLine(" 	FROM ");
			sql.AppendLine(" 		ORGANIZATIONS ORG ");
			sql.AppendLine(" 	WHERE ");
			sql.AppendLine(" 		ORG.ID IN(3, 5, 6, 10, 11) ");
			sql.AppendLine(" 	) ORG ");
			sql.AppendLine(" 	LEFT JOIN CHARACTERS CHR ON ");
			sql.AppendLine(" 		ORG.ID = CHR.ORGANIZATION ");
			sql.AppendLine(" 	LEFT JOIN ZANPAKUTOU ZPT ON ");
			sql.AppendLine(" 		CHR.ZANPAKUTOU = ZPT.ID ");
			sql.AppendLine(" ORDER BY ");
			sql.AppendLine(" 	  ORG.ID ");
			sql.AppendLine(" 	, CHR.ID ");

			var partyDic = new Dictionary<long, SoulSocietyParty>();

			this.Connection.Query<SoulSocietyParty, BleachCharacter, SoulSocietyParty>(
				sql.ToString(),
				(party, bleachChara) =>
				{
					SoulSocietyParty partyEntry = null;

					if (!partyDic.TryGetValue(party.PartyId, out partyEntry))
					{
						partyEntry = party;
						partyDic.Add(partyEntry.PartyId, partyEntry);
					}

					partyEntry.PartyMembers.Add(bleachChara);

					return partyEntry;
				},
				splitOn: "Id"
				);

			return partyDic.Values.ToList();
		}

		/// <summary>フリガナ昇順のトップ10キャラクターを取得します。</summary>
		/// <returns>取得したキャラクターを表すList<BleachCharacter>。</returns>
		public List<BleachCharacter> GetTop10Furigana()
		{
			var sql = new StringBuilder(1000);
			sql.AppendLine(" SELECT ");
			sql.AppendLine(" 	* ");
			sql.AppendLine(" FROM ");
			sql.AppendLine(" 	( ");
			sql.AppendLine(" 	SELECT  ");
			sql.AppendLine(" 		  CHR.ID AS Id ");
			sql.AppendLine(" 		, CHR.CHARACTER_NAME AS Name ");
			sql.AppendLine(" 		, CHR.KANA AS Furigana ");
			sql.AppendLine(" 		, CHR.BIRTHDAY AS Birthday ");
			sql.AppendLine(" 		, CHR.ORGANIZATION AS OrganizationId ");
			sql.AppendLine(" 		, ORG.ORGANIZATION_NAME AS OrganizationName ");
			sql.AppendLine(" 		, CHR.ZANPAKUTOU ZanpakutouId ");
			sql.AppendLine(" 		, ZBT.ZANPAKUTOU_NAME AS ZanpakutouName ");
			sql.AppendLine(" 	FROM ");
			sql.AppendLine(" 		CHARACTERS CHR ");
			sql.AppendLine(" 		LEFT JOIN ORGANIZATIONS ORG ON ");
			sql.AppendLine(" 			CHR.ORGANIZATION = ORG.ID ");
			sql.AppendLine(" 		LEFT JOIN ZANPAKUTOU ZBT ON ");
			sql.AppendLine(" 			CHR.ZANPAKUTOU = ZBT.ID ");
			sql.AppendLine(" 	WHERE ");
			sql.AppendLine(" 		CHR.ORGANIZATION IN :ORGANIZATION ");
			sql.AppendLine(" 	ORDER BY ");
			sql.AppendLine(" 		CHR.ORGANIZATION ");
			sql.AppendLine(" 	) CHR LIMIT 10 ");

			//return this.Connection.Query<BleachCharacter>(sql.ToString(), new BleachCharacter() { Id = 10 }).ToList();
			return this.Connection.Query<BleachCharacter>(sql.ToString(), new { ORGANIZATION = new List<int>() { 3, 6, 9, 11 } }).ToList();
		}

		/// <summary>ID順でトップ10のキャラクターを取得します。</summary>
		/// <returns>ID順でトップ10のキャラクターを表すIEnumerable<dynamic>。</returns>
		public IEnumerable<dynamic> GetTop10Id()
		{
			var sql = new StringBuilder(800);
			sql.AppendLine(" SELECT ");
			sql.AppendLine(" 	* ");
			sql.AppendLine(" FROM ");
			sql.AppendLine(" 	( ");
			sql.AppendLine(" 	SELECT  ");
			sql.AppendLine(" 		  CHR.ID ");
			sql.AppendLine(" 		, CHR.CHARACTER_NAME ");
			sql.AppendLine(" 		, CHR.KANA ");
			sql.AppendLine(" 		, CHR.BIRTHDAY ");
			sql.AppendLine(" 		, CHR.ORGANIZATION ");
			sql.AppendLine(" 		, ORG.ORGANIZATION_NAME ");
			sql.AppendLine(" 		, CHR.ZANPAKUTOU ");
			sql.AppendLine(" 		, ZBT.ZANPAKUTOU_NAME ");
			sql.AppendLine(" 	FROM ");
			sql.AppendLine(" 		CHARACTERS CHR ");
			sql.AppendLine(" 		LEFT JOIN ORGANIZATIONS ORG ON ");
			sql.AppendLine(" 			CHR.ORGANIZATION = ORG.ID ");
			sql.AppendLine(" 		LEFT JOIN ZANPAKUTOU ZBT ON ");
			sql.AppendLine(" 			CHR.ZANPAKUTOU = ZBT.ID ");
			sql.AppendLine(" 	ORDER BY ");
			sql.AppendLine(" 		CHR.ID ");
			sql.AppendLine(" 	) CHR LIMIT 10 ");

			return this.Connection.Query(sql.ToString(), commandType: System.Data.CommandType.Text);
		}

		/// <summary>CHARACTERSテーブルの最大オートナンバーを取得します。</summary>
		/// <returns>CHARACTERSテーブルの最大オートナンバーを表すlong。</returns>
		public long GetCharacterSeq()
		{
			var sql = new StringBuilder(150);
			sql.AppendLine(" SELECT ");
			sql.AppendLine(" 	SEQ.seq ");
			sql.AppendLine(" FROM ");
			sql.AppendLine(" 	sqlite_sequence SEQ ");
			sql.AppendLine(" WHERE ");
			sql.AppendLine(" 	SEQ.name = :name ");

			return this.Connection.ExecuteScalar<long>(sql.ToString(), new { name = "CHARACTERS" });
		}

		/// <summary>指定した数値より大きいIDのキャラクターを取得します。</summary>
		/// <param name="minId">このパラメータに指定したより大きいIDのキャラクターを取得します。</param>
		/// <returns>指定した数値より大きいIDのキャラクターを表すTask<IEnumerable<BleachCharacter>>。</returns>
		public async Task<IEnumerable<BleachCharacter>> GetCharactersByIdOrverAsync(long minId)
		{
			var sql = new StringBuilder(1000);
			sql.AppendLine(" SELECT ");
			sql.AppendLine(" 	  CHR.ID AS Id ");
			sql.AppendLine(" 	, CHR.CHARACTER_NAME AS Name ");
			sql.AppendLine(" 	, CHR.KANA AS Furigana ");
			sql.AppendLine(" 	, ORG.ORGANIZATION_NAME AS OrganizationName ");
			sql.AppendLine(" FROM ");
			sql.AppendLine(" 	( ");
			sql.AppendLine(" 	SELECT ");
			sql.AppendLine(" 		CHR.ID ");
			sql.AppendLine(" 		, CHR.CHARACTER_NAME ");
			sql.AppendLine(" 		, CHR.KANA ");
			sql.AppendLine(" 		, CHR.ORGANIZATION ");
			sql.AppendLine(" 		, CHR.ZANPAKUTOU ");
			sql.AppendLine(" 	FROM ");
			sql.AppendLine(" 		CHARACTERS CHR ");
			sql.AppendLine(" 	WHERE ");
			sql.AppendLine(" 		CHR.ID > :Id ");
			sql.AppendLine(" 	) CHR ");
			sql.AppendLine(" 	LEFT JOIN ORGANIZATIONS ORG ON ");
			sql.AppendLine(" 		CHR.ORGANIZATION = ORG.ID ");
			sql.AppendLine(" 	LEFT JOIN ZANPAKUTOU ZPT ON ");
			sql.AppendLine(" 		CHR.ZANPAKUTOU = ZPT.ID ");
			sql.AppendLine(" ORDER BY ");
			sql.AppendLine(" 	CHR.KANA ");

			return await this.Connection.QueryAsync<BleachCharacter>(sql.ToString(), new { Id = minId });
		}

		/// <summary>キャラクターを登録します。</summary>
		/// <param name="character">登録するキャラクターを表すBleachCharacter。</param>
		/// <returns>登録件数を表すint。</returns>
		public int RegistCharacter(BleachCharacter character)
		{
			var sql = new StringBuilder(300);
			sql.AppendLine(" INSERT INTO CHARACTERS( ");
			sql.AppendLine(" 	  CHARACTER_NAME ");
			sql.AppendLine(" 	, KANA ");
			sql.AppendLine(" 	, ORGANIZATION ");
			sql.AppendLine(" ) VALUES ( ");
			sql.AppendLine(" 	  :Name ");
			sql.AppendLine(" 	, :Furigana ");
			sql.AppendLine(" 	, :OrganizationId ");
			sql.AppendLine(" ) ");

			return this.Connection.Execute(sql.ToString(), character);
		}

		/// <summary>キャラクターを登録します。（非同期）</summary>
		/// <param name="character">登録するキャラクターを表すBleachCharacter。</param>
		/// <returns>登録件数を表すint。</returns>
		public async Task<int> RegistCharacterAsync(BleachCharacter character)
		{
			var sql = new StringBuilder(300);
			sql.AppendLine(" INSERT INTO CHARACTERS( ");
			sql.AppendLine(" 	  CHARACTER_NAME ");
			sql.AppendLine(" 	, KANA ");
			sql.AppendLine(" 	, ORGANIZATION ");
			sql.AppendLine(" ) VALUES ( ");
			sql.AppendLine(" 	  :Name ");
			sql.AppendLine(" 	, :Furigana ");
			sql.AppendLine(" 	, :OrganizationId ");
			sql.AppendLine(" ) ");

			return await this.Connection.ExecuteAsync(sql.ToString(), character);
		}

		/// <summary>キャラクターを登録します。（非同期）</summary>
		/// <param name="characters">登録するキャラクターを表すList<BleachCharacter>。</param>
		/// <returns>登録件数を表すint。</returns>
		public async Task<int> RegistCharactersAsync(List<BleachCharacter> characters)
		{
			var sql = new StringBuilder(300);
			sql.AppendLine(" INSERT INTO CHARACTERS( ");
			sql.AppendLine(" 	  CHARACTER_NAME ");
			sql.AppendLine(" 	, KANA ");
			sql.AppendLine(" 	, ORGANIZATION ");
			sql.AppendLine(" ) VALUES ( ");
			sql.AppendLine(" 	  :Name ");
			sql.AppendLine(" 	, :Furigana ");
			sql.AppendLine(" 	, :OrganizationId ");
			sql.AppendLine(" ) ");

			return await this.Connection.ExecuteAsync(sql.ToString(), characters);
		}

		/// <summary>ふりがなを指定してキャラクターを削除します。（非同期）</summary>
		/// <param name="characters">削除するキャラクターを表すList<BleachCharacter>。</param>
		/// <returns>削除件数を表すint。</returns>
		public async Task<int> DeleteCharactersByKanaAsync(List<BleachCharacter> characters)
		{
			var sql = new StringBuilder(100);
			sql.AppendLine(" DELETE FROM CHARACTERS ");
			sql.AppendLine(" WHERE ");
			sql.AppendLine(" 	KANA IN :Furigana ");

			return await this.Connection.ExecuteAsync(sql.ToString(), new { Furigana = characters.Select(b => b.Furigana).ToList() });
		}
	}
}
