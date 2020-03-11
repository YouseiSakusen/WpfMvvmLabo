using System;
using System.Collections.Generic;

namespace DapperSample
{
	/// <summary>キャラクター用リポジトリを表します。</summary>
	public interface ICharacterRepository : IDisposable
	{
		/// <summary>ID順にトップ10のキャラクターを取得します。</summary>
		/// <returns>取得したキャラクターを表すIEnumerable<dynamic>。</returns>
		public IEnumerable<dynamic> GetTopIdCharacters();
	}
}
