using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using HalationGhost.WinApps.Services.CommonDialogs.DialogSettings;

namespace HalationGhost.WinApps.Services.CommonDialogs.InnerService
{
	internal class FolderBrowserDialogService : ICommonDialogService
	{
		#region メソッド

		public bool ShowDialog(CommonDialogSettingBase settings)
		{
			var dialog = this.createDialog(settings);
			if (dialog == null)
				return false;

			if (dialog.ShowDialog() == DialogResult.OK)
				return true;
			else
				return false;
		}

		/// <summary>表示するコモンダイアログを生成します。</summary>
		/// <param name="settings">設定情報を表すIDialogSettings。</param>
		/// <returns>生成したコモンダイアログを表すCommonFileDialog。</returns>
		private FolderBrowserDialog createDialog(CommonDialogSettingBase settings)
		{
			if (settings == null)
				return null;

			var dialog = new FolderBrowserDialog();

			return dialog;
		}

		#endregion
	}
}
