using System;
using System.Collections.Generic;
using System.Text;

namespace HalationGhost.WinApps.Services.CommonDialogs.DialogSettings
{
	public class WinApiFolderPickerDialogSettings : CommonDialogSettingBase
	{
		/// <summary>
		/// 複数ファイルを選択できるかを取得・設定します。
		/// </summary>
		public bool Multiselect { get; set; } = false;

		public string FolderPath { get; set; } = string.Empty;

		public List<string> FolderPaths { get; } = new List<string>();
	}
}
