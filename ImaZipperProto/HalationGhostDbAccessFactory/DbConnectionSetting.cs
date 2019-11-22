using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HalationGhost.WinApps.DatabaseAccesses
{
	/// <summary>
	/// DBの接続設定を表します。
	/// </summary>
	[DataContract]
	public class DbConnectionSetting
	{
		#region プロパティ

		/// <summary>
		/// 接続するDBの番号を取得・設定します。
		/// </summary>
		[DataMember]
		public int TargetNumber { get; set; } = 0;

		/// <summary>
		/// DBの接続設定を取得・設定します。
		/// </summary>
		[DataMember]
		public List<DbConnectInformation> ConnectInformations { get; set; } = new List<DbConnectInformation>();

		#endregion
	}
}
