using System;
using System.Collections.Generic;

namespace DapperSample
{
	/// <summary>キャラクター用リポジトリを表します。</summary>
	public interface ICharacterRepository : IDisposable
	{
		/// <summary>護廷十三隊別にキャラクターを取得します。</summary>
		/// <returns>取得した護廷十三隊を表すList<SoulSocietyParty>。</returns>
		public List<SoulSocietyParty> GetCharactersByParty();

		/// <summary>フリガナ昇順のトップ10キャラクターを取得します。</summary>
		/// <returns>取得したキャラクターを表すList<BleachCharacter>。</returns>
		public List<BleachCharacter> GetTop10Furigana();

		/// <summary>ID順のトップ10キャラクターを取得します。</summary>
		/// <returns>取得したキャラクターを表すIEnumerable<dynamic>。</returns>
		public IEnumerable<dynamic> GetTop10Id();
	}
}
