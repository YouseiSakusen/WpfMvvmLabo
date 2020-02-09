using System.IO;
using HalationGhost.WinApps.Utilities;

namespace HalationGhost.WinApps.DatabaseAccesses
{
	/// <summary>DBの接続設定を取得します。</summary>
	internal class DbConnectionSettingLoader
	{
		private const string FLE_SETTING_FILE_NAME = "DbConnectSetting.xml";

		private const string FLD_SETTING_FOLDER_NAME = "Settings";

		/// <summary>接続設定を読み込みます。</summary>
		/// <returns>接続設定を表すDbConnectionSetting。</returns>
		internal DbConnectionSetting Load()
		{
			return SerializeUtility.DeserializeFromFile<DbConnectionSetting>(this.getSettingFilePath());
		}

		/// <summary>接続設定ファイルのパスを取得します。</summary>
		/// <returns>接続設定ファイルのパスを表す文字列。</returns>
		private string getSettingFilePath()
		{
			var execPath = AssemblyUtility.GetExecutingPath();

			return Path.Combine(execPath,
				DbConnectionSettingLoader.FLD_SETTING_FOLDER_NAME,
				DbConnectionSettingLoader.FLE_SETTING_FILE_NAME);
		}
	}
}
