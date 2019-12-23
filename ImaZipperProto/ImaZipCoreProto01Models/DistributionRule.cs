using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZipBookCreator;

namespace HalationGhost.WinApps.ImaZip
{
	public class DistributionRule
	{
		#region プロパティ

		public int TotalChildCount { get; } = 0;

		public int LandscapeFileCount { get; } = 0;

		#endregion

		public DistributionRule(List<SourceItem> sourceItems)
		{
			this.TotalChildCount = sourceItems.Count;
			this.LandscapeFileCount = sourceItems
				.Where(i => i.ImageSpecification.Direction == ImageDirection.Landscape)
				.Count();
		}
	}
}
