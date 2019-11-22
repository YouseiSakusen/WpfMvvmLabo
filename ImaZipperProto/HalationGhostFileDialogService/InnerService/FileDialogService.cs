using System.Linq;
using HalationGhost.WinApps.Services.CommonDialogs.DialogSettings;
using Microsoft.Win32;

namespace HalationGhost.WinApps.Services.CommonDialogs.InnerService
{
	/// <summary>
	/// ファイル保存、ファイルを開くコモンダイアログ用のサービスを表します。
	/// </summary>
	internal class FileDialogService : ICommonDialogService
	{
		#region メソッド

		/// <summary>
		/// コモンダイアログを表示します。
		/// </summary>
		/// <param name="settings">表示するコモンダイアログを表すCommonDialogSettingBase。</param>
		/// <returns>コモンダイアログの操作結果を表すbool。</returns>
		public bool ShowDialog(CommonDialogSettingBase settings)
		{
			var dialog = this.createDialog(settings);
			if (dialog == null)
				return false;

			var ret = dialog.ShowDialog();
			if (ret.HasValue)
			{
				this.setReturnValues(dialog, settings);
				return ret.Value;
			}
			else
			{
				return false;
			}
		}

		private FileDialog createDialog(CommonDialogSettingBase settings)
		{
			if (settings == null)
				return null;

			FileDialog dialog = null;

			switch (settings)
			{
				case OpenFileDialogSettings o:
					dialog = new OpenFileDialog();
					break;
				case SaveFileDialogSettings s:
					dialog = new SaveFileDialog();
					break;
				default:
					return null;
			}

			var saveSettings = settings as SaveFileDialogSettings;

			dialog.Filter = saveSettings.Filter;
			dialog.FilterIndex = saveSettings.FilterIndex;
			dialog.InitialDirectory = saveSettings.InitialDirectory;
			dialog.Title = saveSettings.Title;

			if (settings is OpenFileDialogSettings)
				((OpenFileDialog)dialog).Multiselect = ((OpenFileDialogSettings)settings).Multiselect;

			return dialog;
		}

		/// <summary>戻り値を設定します。</summary>
		/// <param name="dialog">表示したダイアログを表すFileDialog。</param>
		/// <param name="settings">設定情報を表すIDialogSettings。</param>
		private void setReturnValues(FileDialog dialog, CommonDialogSettingBase settings)
		{
			switch (settings)
			{
				case OpenFileDialogSettings o:
					var openDialog = dialog as OpenFileDialog;
					o.FileName = openDialog.FileName;
					o.FileNames = openDialog.FileNames.ToList();
					break;
				case SaveFileDialogSettings s:
					var saveDialog = dialog as SaveFileDialog;
					s.FileName = saveDialog.FileName;
					break;
				default:
					break;
			}
		}

		#endregion
	}
}
