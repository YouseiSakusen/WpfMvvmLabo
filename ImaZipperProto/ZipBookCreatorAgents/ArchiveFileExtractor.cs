using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using HalationGhost.WinApps.ImaZip.ImageFileSettings;
using ZipBookCreator;

namespace HalationGhost.WinApps.ImaZip.ZipBookCreator
{
	internal class ArchiveFileExtractor
	{
		internal async Task<bool> ExtractArchivesAsync(ZipFileSettings settings, CreatorRelayStation relayStation)
		{
			if (! (await this.createWorkFolderAsync(settings, relayStation)))
				return false;

			foreach (var imgSrc in settings.ImageSources)
			{
				if (!File.Exists(imgSrc.Path.Value))
					continue;


			}

			return true;
		}

		private async Task<bool> createWorkFolderAsync(ZipFileSettings settings, CreatorRelayStation relayStation)
		{
			if (!Directory.Exists(settings.ImageFilesExtractedFolder.Value))
			{
				relayStation.SetCreateResult(ResultDetail.ImageExtractFolderNotFound);
				return false;
			}

			if (Directory.Exists(settings.WorkRootFolderPath))
				await Task.Run(() => Directory.Delete(settings.WorkRootFolderPath, true));

			Directory.CreateDirectory(settings.WorkRootFolderPath);

			return true;
		}
	}
}
