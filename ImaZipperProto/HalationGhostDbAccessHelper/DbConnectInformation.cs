using System.Runtime.Serialization;

namespace HalationGhost.WinApps.DatabaseAccesses
{
	#region 列挙型

	/// <summary>
	/// 接続先のDBを表す列挙型。
	/// </summary>
	public enum DatabaseType
	{
		/// <summary>
		/// DB種類無しを表します。（初期化用）
		/// </summary>
		None,
		/// <summary>
		/// SQLiteを表します。
		/// </summary>
		SQLite
	}

	#endregion

	/// <summary>
	/// DBへの接続情報を表します。
	/// </summary>
	[DataContract]
	public class DbConnectInformation
	{
		#region プロパティ

		/// <summary>
		/// DBへの接続情報を識別するための番号を取得・設定します。
		/// </summary>
		[DataMember]
		public int Number { get; set; } = 0;

		/// <summary>
		/// 接続するDBの種類を取得・設定します。
		/// </summary>
		[DataMember]
		public DatabaseType DbType { get; set; } = DatabaseType.None;

		/// <summary>
		/// 接続先DBのデータソースを取得・設定します。
		/// </summary>
		[DataMember]
		public string DataSource { get; set; } = string.Empty;

		#endregion
	}
}
