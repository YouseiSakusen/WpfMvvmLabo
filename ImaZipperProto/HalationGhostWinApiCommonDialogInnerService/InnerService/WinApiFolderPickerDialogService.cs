using System;
using System.Collections.Generic;
using System.Text;
using HalationGhost.WinApps.Services.CommonDialogs.DialogSettings;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace HalationGhost.WinApps.Services.CommonDialogs.InnerService
{
	public class WinApiFolderPickerDialogService : ICommonDialogService
	{
		#region メソッド

		public bool ShowDialog(CommonDialogSettingBase settings)
		{
			var ret = CommonFileDialogResult.None;
			var dialogSettings = settings as WinApiFolderPickerDialogSettings;

			using (var dialog = this.createDialog(dialogSettings))
			{
				ret = dialog.ShowDialog();

				if (ret == CommonFileDialogResult.Ok)
					this.setReturnValues(dialog, dialogSettings);
			}

			return ret == CommonFileDialogResult.Ok;
		}

		private CommonOpenFileDialog createDialog(WinApiFolderPickerDialogSettings settings)
		{
			return new CommonOpenFileDialog()
			{
				IsFolderPicker = true,
				Multiselect = settings.Multiselect,
				InitialDirectory = settings.InitialDirectory,
				Title = settings.Title
			};
		}

		private void setReturnValues(CommonOpenFileDialog dialog, WinApiFolderPickerDialogSettings settings)
		{
			if (settings.Multiselect)
			{
				settings.FolderPaths.Clear();
				settings.FolderPaths.AddRange(dialog.FileNames);
			}
			else
			{
				settings.FolderPaths.Clear();
				settings.FolderPath = dialog.FileName;
			}
		}

		#endregion
	}
}
