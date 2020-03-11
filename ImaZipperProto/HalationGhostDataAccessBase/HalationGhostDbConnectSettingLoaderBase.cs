using System.IO;
using HalationGhost.WinApps.Utilities;

namespace HalationGhost.WinApps.DatabaseAccesses
{
	public class HalationGhostDbConnectSettingLoaderBase
	{
		#region プロパティ

		public string FolderName { get; set; } = string.Empty;

		public string SettingFileName { get; set; } = string.Empty;

		#endregion

		/// <summary>接続設定を読み込みます。</summary>
		/// <returns>接続設定を表すDbConnectionSetting。</returns>
		internal DbConnectionSetting Load()
		{
			return SerializeUtility.DeserializeFromFile<DbConnectionSetting>(this.getSettingFilePath());
		}

		/// <summary>接続設定ファイルのパスを取得します。</summary>
		/// <returns>接続設定ファイルのパスを表す文字列。</returns>
		protected virtual string getSettingFilePath()
		{
			var execPath = AssemblyUtility.GetExecutingPath();

			if (string.IsNullOrEmpty(this.FolderName))
				return Path.Combine(execPath, this.SettingFileName);
			else
				return Path.Combine(execPath, this.FolderName, this.SettingFileName);
		}

		#region コンストラクタ

		public HalationGhostDbConnectSettingLoaderBase(string folderName, string settingFileName)
		{
			this.FolderName = folderName;
			this.SettingFileName = settingFileName;
		}

		public HalationGhostDbConnectSettingLoaderBase() : base() { }

		#endregion
	}
}
