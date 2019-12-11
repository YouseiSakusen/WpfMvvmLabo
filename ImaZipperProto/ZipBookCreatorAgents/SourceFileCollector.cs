using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HalationGhost.WinApps.ImaZip.ImageFileSettings;

namespace HalationGhost.WinApps.ImaZip.ZipBookCreator
{
	internal class SourceFileCollector
	{
		internal void CheckSourceFiles(ZipFileSettings settings)
		{
			foreach (var imgSrc in settings.ImageSources)
			{

			}
		}

		private void getExtractedFiles(string targetFolderPath)
		{
			if (!Directory.Exists(targetFolderPath))
				return;

			var folder = new SourceItem(ImageSourceType.Folder, targetFolderPath);

			Directory.EnumerateDirectories(targetFolderPath).ToList().ForEach(d =>
			{
				this.getExtractedFiles(d);
			});

			foreach (var filePath in Directory.EnumerateFiles(targetFolderPath))
			{
				if (SourceItem.IsTargetFile(filePath))
					continue;


			}
		}
	}
}
