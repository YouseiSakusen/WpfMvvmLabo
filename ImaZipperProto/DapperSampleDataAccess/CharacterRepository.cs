using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;

namespace DapperSample
{
	/// <summary>キャラクター用リポジトリを表します。</summary>
	public class CharacterRepository : DapperSampleDataAccessBase, ICharacterRepository
	{
		/// <summary>ID順でトップ10のキャラクターを取得します。</summary>
		/// <returns>ID順でトップ10のキャラクターを表すIEnumerable<dynamic>。</returns>
		public IEnumerable<dynamic> GetTopIdCharacters()
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
			sql.AppendLine(" 		, ZBT.BANKAI_NAME ");
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
