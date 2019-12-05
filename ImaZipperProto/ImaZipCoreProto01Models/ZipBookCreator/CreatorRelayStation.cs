using System;
using System.Collections.Generic;
using System.Text;
using Reactive.Bindings;
using ZipBookCreator;

namespace HalationGhost.WinApps.ImaZip.ZipBookCreator
{
	public class CreatorRelayStation
	{
		public ReactivePropertySlim<CreateProcess> ProcessState { get; private set; }

		public CreatorRelayStation()
		{
			this.ProcessState = new ReactivePropertySlim<CreateProcess>(CreateProcess.Stopping);
		}
	}
}
