using System;
using System.Collections.Generic;
using System.Text;

namespace HalationGhost.WinApps.Services.CommonDialogs.DialogSettings
{
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
