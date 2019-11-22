using System;
using System.Collections.Generic;
using System.Text;
using HalationGhost.WinApps.Services.CommonDialogs.DialogSettings;
using HalationGhost.WinApps.Services.CommonDialogs.InnerService;

namespace HalationGhost.WinApps.Services.CommonDialogs
{
	/// <summary>
	/// コモンダイアログ表示サービスを表します。
	/// </summary>
	public class CommonDialogService : ICommonDialogService
	{
		#region メソッド

		/// <summary>
		/// コモンダイアログを表示します。
		/// </summary>
		/// <param name="settings">表示するコモンダイアログを表すCommonDialogSettingBase。</param>
		/// <returns>コモンダイアログの操作結果を表すbool。</returns>
		public bool ShowDialog(CommonDialogSettingBase settings)
		{
			var service = new InnerServiceFactory().CreateInnerService(settings);
			if (service == null)
				return false;
			
			return service.ShowDialog(settings);
		}

		#endregion
	}
}
