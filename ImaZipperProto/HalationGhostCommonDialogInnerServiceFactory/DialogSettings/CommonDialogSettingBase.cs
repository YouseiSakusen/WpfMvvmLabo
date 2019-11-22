namespace HalationGhost.WinApps.Services.CommonDialogs.DialogSettings
{
	/// <summary>
	/// コモンダイアログ設定のスーパークラスを表します。
	/// </summary>
	public class CommonDialogSettingBase
	{
		#region プロパティ

		/// <summary>
		/// 初期表示ディレクトリを取得・設定します。
		/// </summary>
		public string InitialDirectory { get; set; } = string.Empty;

		/// <summary>
		/// コモンダイアログのタイトルを取得・設定します。
		/// </summary>
		public string Title { get; set; } = string.Empty;

		#endregion
	}
}
