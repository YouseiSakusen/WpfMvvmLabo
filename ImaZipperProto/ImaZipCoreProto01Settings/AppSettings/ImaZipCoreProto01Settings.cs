using System.Runtime.Serialization;

namespace HalationGhost.WinApps.ImaZip.AppSettings
{
	/// <summary>
	/// ImaZipCoreProto01のアプリケーション設定を表します。
	/// </summary>
	[DataContract]
	public class ImaZipCoreProto01Settings : IImaZipCoreProto01Settings
	{
		#region プロパティ

		/// <summary>
		/// ソースファイルを指定する際のファイルを開くダイアログ.Filterを取得・設定します。
		/// </summary
		[DataMember]
		public string SourceFileSelectedFilter { get; set; } = string.Empty;

		/// <summary>
		/// ソースファイルを指定する際のファイルを開くダイアログ.InitialDirectoryを取得・設定します。
		/// </summary>
		[DataMember]
		public string SourceFileInitialDirectoryPath { get; set; } = string.Empty;

		/// <summary>
		/// CreatorExeファイル名を取得・設定します。
		/// </summary>
		[DataMember]
		public string CreatorExeFileName { get; set; } = string.Empty;

		#endregion
	}
}
