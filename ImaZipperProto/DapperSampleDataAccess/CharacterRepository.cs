using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	}
}
