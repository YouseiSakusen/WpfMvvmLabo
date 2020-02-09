using System.Data.Common;

namespace HalationGhost.WinApps.DatabaseAccesses
{
	/// <summary>AbstractFactoryを表します。</summary>
	public interface IDbAccessHelper
	{
		/// <summary>DBのConnectionを取得します。</summary>
		/// <returns>DBのConnectionを表すDbConnection。</returns>
		public DbConnection GetConnection();

		/// <summary>DBのトランザクションを取得します。</summary>
		/// <param name="connection">トランザクションを取得するDbConnection。</param>
		/// <returns>DBのトランザクションを表すDbTransaction。</returns>
		public DbTransaction GetTransaction(DbConnection connection);
	}
}
