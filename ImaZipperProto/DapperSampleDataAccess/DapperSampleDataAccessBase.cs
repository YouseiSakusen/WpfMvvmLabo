using System;
using System.Collections.Generic;
using System.Text;
using HalationGhost.WinApps.DatabaseAccesses;

namespace DapperSample
{
	public abstract class DapperSampleDataAccessBase : HalationGhostDbAccessBase
	{
		public DapperSampleDataAccessBase()
			: base(new HalationGhostDbConnectSettingLoaderBase(string.Empty, "DbConnectSetting.xml")) { }
	}
}
