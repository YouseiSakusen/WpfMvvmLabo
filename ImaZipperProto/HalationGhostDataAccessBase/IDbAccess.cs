using System.Data.Common;

namespace HalationGhost.WinApps.DatabaseAccesses
{
	/// <summary>DBアクセスインタフェースを表します。</summary>
	public interface IDbAccess
	{
		/// <summary>トランザクションを開始します。</summary>
		/// <returns>DBのトランザクションを表すDbTransaction。</returns>
		public DbTransaction BeginTransaction();
	}
}
