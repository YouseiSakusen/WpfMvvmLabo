using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SharpCompress.Archives;

namespace HalationGhost.WinApps.ImaZip
{
	public class SourceFile
	{
		private static List<string> TargetExtensions = new List<string>() { ".jpg", ".jpeg", ".png", ".bmp" };

		public static bool IsTargetFile(IArchiveEntry entry)
		{
			if (entry.IsDirectory)
				return false;

			var extension = Path.GetExtension(entry.Key);

			return SourceFile.TargetExtensions.Any(e => e == extension);
		}
	}
}
