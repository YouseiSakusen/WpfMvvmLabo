using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HalationGhost.WinApps.Images;
using ZipBookCreator;

namespace HalationGhost.WinApps.ImaZip
{
	public class DistributionRule
	{
		#region プロパティ

		public int TotalChildCount { get; } = 0;

		public int LandscapeFileCount { get; } = 0;

		public int TotalChildCountTextLength { get; } = 0;

		public double LandscapeRatio { get; } = 0;

		#endregion

		public DistributionRule(List<SourceItem> sourceItems)
		{
			this.TotalChildCount = sourceItems.Count;
			this.TotalChildCountTextLength = this.TotalChildCount.ToString().Length;
			
			this.LandscapeFileCount = sourceItems
				.Where(i => i.ImageSpecification.Direction == ImageDirection.Landscape)
				.Count();
			this.LandscapeRatio = (double)this.LandscapeFileCount / this.TotalChildCount;

			this.setSplitMark(sourceItems);
		}

		private void setSplitMark(List<SourceItem> sourceItems)
		{
			if (this.LandscapeRatio < 0.9)
			{
				var total = sourceItems.Count;
				var lastIndex = 5;
				if (total < 5)
					lastIndex = total;

				// 横長ファイルが90%以下の場合、先頭5ファイルは横長でも分割しない
				for (int i = 0; i < lastIndex; i++)
					sourceItems[i].IsSplit = false;

				// 横長ファイルが90%以下の場合、末尾5ファイルは横長でも分割しない
				for (int i = total - 1; i < total - 5; i--)
					sourceItems[i].IsSplit = false;
			}
		}
	}
}
