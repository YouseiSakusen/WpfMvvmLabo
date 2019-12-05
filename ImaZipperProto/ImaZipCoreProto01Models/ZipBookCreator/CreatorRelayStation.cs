using System;
using System.Collections.Generic;
using System.Text;
using Reactive.Bindings;
using ZipBookCreator;

namespace HalationGhost.WinApps.ImaZip.ZipBookCreator
{
	public class CreatorRelayStation
	{
		#region プロパティ

		public ReactivePropertySlim<CreateProcess> ProcessState { get; private set; }

		public ResultDetail CreateResult { get; set; } = ResultDetail.ResultNone;

		#endregion

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

		public CreatorRelayStation()
		{
			this.ProcessState = new ReactivePropertySlim<CreateProcess>(CreateProcess.Stopping);
		}
	}
}
