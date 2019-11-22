namespace HalationGhost.WinApps.ImaZip.AppSettings
{
	/// <summary>
	/// ImaZipプロトアプリの設定を表します。
	/// </summary>
	public interface IImaZipCoreProto01Settings
	{
		#region プロパティ

		/// <summary>
		/// ソースファイルを指定する際のファイルを開くダイアログ.Filterを取得・設定します。
		/// </summary>
		public string SourceFileSelectedFilter { get; set; }

		/// <summary>
		/// ソースファイルを指定する際のファイルを開くダイアログ.InitialDirectoryを取得・設定します。
		/// </summary>
		public string SourceFileInitialDirectoryPath { get; set; }

		#endregion
	}
}
