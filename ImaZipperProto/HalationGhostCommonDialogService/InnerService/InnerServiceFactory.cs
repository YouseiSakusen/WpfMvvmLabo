using HalationGhost.WinApps.Services.CommonDialogs.DialogSettings;

namespace HalationGhost.WinApps.Services.CommonDialogs.InnerService
{
	/// <summary>
	/// CommonDialogService内で使用するサービスのファクトリを表します。
	/// </summary>
	public class InnerServiceFactory
	{
		#region メソッド

		/// <summary>
		/// CommonDialogService内で使用するサービスを生成します。
		/// </summary>
		/// <param name="settings">コモンダイアログの設定を表すCommonDialogSettingBase。</param>
		/// <returns>CommonDialogService内で使用するサービスを表すICommonDialogService。</returns>
		public ICommonDialogService CreateInnerService(CommonDialogSettingBase settings)
		{
			switch (settings)
			{
				case OpenFileDialogSettings o:
				case SaveFileDialogSettings s:
					return new FileDialogService();
				case WinApiFolderPickerDialogSettings p:
					return new WinApiFolderPickerDialogService();
			}

			return null;
		}

		#endregion
	}
}
