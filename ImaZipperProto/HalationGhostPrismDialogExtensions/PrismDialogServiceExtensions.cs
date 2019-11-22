using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Services.Dialogs.Extensions
{
	public static class PrismDialogServiceExtensions
	{
		#region 拡張メソッド

		public static ButtonResult ShowNotify(this IDialogService dialogService, string message, string title)
		{
			var param = new DialogParameters($"Message={message}");
			param.Add("Title", title);

			var tempRet = ButtonResult.Cancel;

			dialogService.ShowDialog("NotifyMessageBox", param, r => tempRet = r.Result);

			return tempRet;
		}

		#endregion
	}
}
