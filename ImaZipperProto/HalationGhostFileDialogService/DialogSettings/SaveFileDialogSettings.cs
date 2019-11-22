using System;
using System.Collections.Generic;
using System.Text;

namespace HalationGhost.WinApps.Services.CommonDialogs.DialogSettings
{
	public class SaveFileDialogSettings : CommonDialogSettingBase
	{
		#region プロパティ

		/// <summary>
		/// 表示するファイルのフィルタを取得・設定します。
		/// </summary>
		public string Filter { get; set; } = string.Empty;

		/// <summary>
		/// フィルタのデフォルトインデックスを取得・設定します。
		/// </summary>
		public int FilterIndex { get; set; } = 0;

		/// <summary>
		/// ダイアログで指定したファイル名を取得・設定します。
		/// </summary>
		public string FileName { get; set; } = string.Empty;

		#endregion
	}
}
