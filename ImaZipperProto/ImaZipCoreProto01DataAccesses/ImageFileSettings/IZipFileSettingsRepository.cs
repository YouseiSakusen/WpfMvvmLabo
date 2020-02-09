namespace HalationGhost.WinApps.ImaZip.ImageFileSettings
{
	/// <summary>ZipFileSettingsのリポジトリインタフェース。</summary>
	public interface IZipFileSettingsRepository
	{
		/// <summary>ZipFileSettingsを保存します。</summary>
		/// <param name="settings">保存するZipFileSettings。</param>
		/// <returns>保存したZipFileSettingsのIDを表すlong?。</returns>
		public long? Save(ZipFileSettings settings);

		/// <summary>IDを指定してZipFileSettingsを取得します。</summary>
		/// <param name="id">ZipFileSettingsのIDを表す文字列。</param>
		/// <returns>IDを指定して取得したZipFileSettings。</returns>
		public ZipFileSettings GetById(string id);
	}
}
