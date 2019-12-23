using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SharpCompress.Archives;

namespace HalationGhost.WinApps.ImaZip
{
	public class SourceItem
	{
		#region staticメソッド

		private static List<string> TargetExtensions = new List<string>() { ".jpg", ".jpeg", ".png", ".bmp" };

		public static bool IsTargetFile(IArchiveEntry entry)
		{
			if (entry.IsDirectory)
				return false;

			return SourceItem.IsTargetFile(entry.Key);
		}

		public static bool IsTargetFile(string filePath)
		{
			var extension = Path.GetExtension(filePath);

			return SourceItem.TargetExtensions.Any(e => e == extension);
		}

		#endregion

		#region プロパティ

		public ImageSourceType ItemKind { get; } = ImageSourceType.None;

		public string ItemPath { get; } = string.Empty;

		public string FileName { get; } = string.Empty;

		public List<SourceItem> Children { get; } = new List<SourceItem>();

		public ImageSpecification ImageSpecification { get; set; } = null;

		public DistributionRule DistributionRule { get; private set; } = null;

		#endregion

		public void CreateDistributionRule()
			=> this.DistributionRule = new DistributionRule(this.Children);

		#region コンストラクタ

		public SourceItem(ImageSpecification imageSpec) : this(ImageSourceType.File, imageSpec.ImageFilePath)
		{
			this.ImageSpecification = imageSpec;
		}

		public SourceItem(ImageSourceType kind, string path) : base()
		{
			this.ItemKind = kind;
			this.ItemPath = path;
			this.FileName = Path.GetFileName(path);
		}

		#endregion
	}
}
