using HalationGhost.WinApps.Services.CommonDialogs.DialogSettings;

namespace HalationGhost.WinApps.Services.CommonDialogs
{
	/// <summary>
	/// コモンダイアログサービスインタフェース。
	/// </summary>
	public interface ICommonDialogService
	{
		#region メソッド

		/// <summary>
		/// コモンダイアログを表示します。
		/// </summary>
		/// <param name="settings">表示するコモンダイアログを表すCommonDialogSettingBase。</param>
		/// <returns>コモンダイアログの操作結果を表すbool。</returns>
		bool ShowDialog(CommonDialogSettingBase settings);

		#endregion
	}
}
