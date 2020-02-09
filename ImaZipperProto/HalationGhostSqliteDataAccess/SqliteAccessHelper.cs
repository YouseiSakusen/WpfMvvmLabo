using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;
using HalationGhost.WinApps.Utilities;

namespace HalationGhost.WinApps.DatabaseAccesses.Sqlite
{
	/// <summary>SQLiteデータベースアクセスヘルパを表します。</summary>
	public class SqliteAccessHelper : IDbAccessHelper
	{
		/// <summary>DBのConnectionを取得します。</summary>
		/// <returns>DBのConnectionを表すDbConnection。</returns>
		public DbConnection GetConnection()
		{
			var con = new SQLiteConnection(SqliteAccessHelper.builder.ToString());

			return con.OpenAndReturn();
		}

		/// <summary>DBのトランザクションを取得します。</summary>
		/// <param name="connection">トランザクションを取得するDbConnection。</param>
		/// <returns>DBのトランザクションを表すDbTransaction。</returns>
		public DbTransaction GetTransaction(DbConnection connection)
		{
			if (connection == null)
				return null;

			var con = connection as SQLiteConnection;
			if (con == null)
				return null;

			if (con.State != System.Data.ConnectionState.Open)
				return null;

			return con.BeginTransaction();
		}

		#region コンストラクタ

		/// <summary>
		/// DBへの接続情報を表します。
		/// </summary>
		private static DbConnectInformation connectInfo = null;
		/// <summary>
		/// DBへの接続文字列を表します。
		/// </summary>
		private static SQLiteConnectionStringBuilder builder = null;

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="connectInformation">DBへの接続情報を表すDbConnectInformation。</param>
		public SqliteAccessHelper(DbConnectInformation connectInformation)
		{
			if (SqliteAccessHelper.connectInfo != null)
				return;

			SqliteAccessHelper.connectInfo = connectInformation;

			var sqLitePath = this.createDbFilePath();
			if (!File.Exists(sqLitePath))
				throw new FileNotFoundException("データベースファイルが存在しません。", sqLitePath);

			SqliteAccessHelper.builder = new SQLiteConnectionStringBuilder() { DataSource = sqLitePath };
		}

		/// <summary>
		/// データベースファイルへのパスを生成します。
		/// </summary>
		/// <returns>データベースファイルへのパスを表す文字列。</returns>
		private string createDbFilePath()
		{
			if (Regex.IsMatch(SqliteAccessHelper.connectInfo.DataSource, "^{exePath}."))
			{
				var execPath = AssemblyUtility.GetExecutingPath();

				return Regex.Replace(SqliteAccessHelper.connectInfo.DataSource, "{exePath}", execPath);
			}
			else
			{
				return SqliteAccessHelper.connectInfo.DataSource;
			}
		}

		#endregion
	}
}
