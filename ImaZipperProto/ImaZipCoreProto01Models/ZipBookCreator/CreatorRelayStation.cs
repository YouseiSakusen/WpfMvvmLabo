using System;
using System.Collections.Generic;
using System.Text;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ZipBookCreator;

namespace HalationGhost.WinApps.ImaZip.ZipBookCreator
{
	public class CreatorRelayStation : BindableModelBase
	{
		#region プロパティ

		public ReactivePropertySlim<string> Log { get; }

		public ReactivePropertySlim<CreateProcess> ProcessState { get; private set; }

		public ResultDetail CreateResult { get; set; } = ResultDetail.ResultNone;

		#endregion

		#region メソッド

		public void AddLog(string logText)
		{
			var buf = new StringBuilder(this.Log.Value);
			buf.AppendLine(logText);

			this.Log.Value = buf.ToString();
		}

		public void SetCreateResult(ResultDetail detail)
		{
			this.CreateResult = detail;

			switch (detail)
			{
				case ResultDetail.Success:
					this.ProcessState.Value = CreateProcess.Completed;
					break;
				case ResultDetail.ImageExtractFolderNotFound:
					this.ProcessState.Value = CreateProcess.Failure;
					break;
			}
		}

		#endregion

		public CreatorRelayStation()
		{
			this.Log = new ReactivePropertySlim<string>(string.Empty)
				.AddTo(this.Disposable);

			this.ProcessState = new ReactivePropertySlim<CreateProcess>(CreateProcess.Stopping);
		}
	}
}
