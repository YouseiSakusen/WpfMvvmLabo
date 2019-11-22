using System;
using System.Collections.Generic;
using System.Text;

namespace HalationGhost.WinApps.Services.CommonDialogs.DialogSettings
{
	public class OpenFileDialogSettings : SaveFileDialogSettings
	{
		#region プロパティ

		/// <summary>
		/// 複数ファイルを選択できるかを取得・設定します。
		/// </summary>
		public bool Multiselect { get; set; } = false;

		public List<string> FileNames { get; set; } = new List<string>();

		#endregion
	}
}
