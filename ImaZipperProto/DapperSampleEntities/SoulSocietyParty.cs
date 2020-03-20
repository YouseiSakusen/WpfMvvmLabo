using System.Collections.Generic;

namespace DapperSample
{
	/// <summary>護廷十三隊の隊を表します。</summary>
	public class SoulSocietyParty
	{
		/// <summary>組織IDを取得・設定します。</summary>
		public long PartyId { get; set; } = 0;

		/// <summary>組織名を取得・設定します。</summary>
		public string PartyName { get; set; } = string.Empty;

		/// <summary>所属隊員を取得します。</summary>
		public List<BleachCharacter> PartyMembers { get; } = new List<BleachCharacter>();
	}
}
