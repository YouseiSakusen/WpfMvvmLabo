using System.IO;
using System.Reflection;
using HalationGhost.WinApps.Utilities;

namespace HalationGhost.WinApps.ImaZip.AppSettings
{
	/// <summary>
	/// アプリケーション設定サービスを表します。
	/// </summary>
	public static class AppSettingsService
	{
		#region 定数

		/// <summary>
		/// 設定ファイルを格納するフォルダ名を表します。
		/// </summary>
		private const string PTH_SETTING_FOLDER_NAME = "Settings";
		/// <summary>
		/// 設定ファイル名を表します。
		/// </summary>
		private const string PTH_SETTING_FILE_NAME = "ImaZipCoreProto01Settings.xml";

		#endregion

		#region メソッド

		/// <summary>
		/// アプリケーション設定を保存します。
		/// </summary>
		/// <param name="settings">アプリケーション設定の保存先ファイルのパスを表す文字列。</param>
		/// <param name="savedPath">設定ファイルの保存先をフルパスで指定します。　※ 省略可能</param>
		public static void SaveSettings(ImaZipCoreProto01Settings settings, string savedPath = "")
		{
			var xmlPath = AppSettingsService.getSettingFilePath();
			if (!string.IsNullOrEmpty(savedPath))
				xmlPath = savedPath;

			SerializeUtility.SerializeToFile<ImaZipCoreProto01Settings>(xmlPath, settings);
		}

		/// <summary>
		/// アプリケーション設定を読み込みます。
		/// </summary>
		/// <returns>アプリケーション設定を表すIImaZipCoreProto01Settings。</returns>
		public static IImaZipCoreProto01Settings LoadSettings()
		{
			return SerializeUtility.DeserializeFromFile<ImaZipCoreProto01Settings>(AppSettingsService.getSettingFilePath());
		}

		/// <summary>
		/// 設定ファイルのパスを取得します。
		/// </summary>
		/// <returns>設定ファイルのパスを表す文字列。</returns>
		private static string getSettingFilePath()
		{
			var executingDirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			executingDirPath = Path.Combine(executingDirPath, AppSettingsService.PTH_SETTING_FOLDER_NAME);

			if (!Directory.Exists(executingDirPath))
				Directory.CreateDirectory(executingDirPath);

			return Path.Combine(executingDirPath,
								AppSettingsService.PTH_SETTING_FILE_NAME);
		}

		#endregion
	}
}
