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

		#region プロパティ

		public ImageSourceType ItemKind { get; set; } = ImageSourceType.None;

		public string ItemPath { get; set; } = string.Empty;

		#endregion

		public SourceItem(ImageSourceType kind, string path) : base()
		{
			this.ItemKind = kind;
			this.ItemPath = path;
		}
	}
}
