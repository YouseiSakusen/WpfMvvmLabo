using HalationGhost.WinApps.DatabaseAccesses.Sqlite;

namespace HalationGhost.WinApps.DatabaseAccesses
{
	/// <summary>IDbAccessHelperのファクトリを表します。</summary>
	public static class DbAccessHelperFactory
	{
		#region メソッド

		/// <summary>IDbAccessHelperを生成します。</summary>
		/// <param name="setting">DBの接続設定を表すDbConnectionSetting。</param>
		/// <param name="targetNumber">接続先DBの番号を表すint?。</param>
		/// <returns>DBへアクセスするためのIDbAccessHelper。</returns>
		public static IDbAccessHelper CreateHelper(DbConnectionSetting setting, int? targetNumber)
		{
			var num = targetNumber;

			if (!num.HasValue)
				num = setting.TargetNumber;

			var info = setting.ConnectInformations.Find(c => c.Number == num);
			if (info == null)
				return null;

			switch (info.DbType)
			{
				case DatabaseType.SQLite:
					return new SqliteAccessHelper(info);
			}

			return null;
		}

		#endregion
	}
}
