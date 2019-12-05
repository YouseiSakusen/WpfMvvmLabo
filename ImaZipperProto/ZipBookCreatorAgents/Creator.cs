using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HalationGhost.WinApps.ImaZip.ImageFileSettings;

namespace HalationGhost.WinApps.ImaZip.ZipBookCreator
{
	public class Creator
	{
		public Task CreateZipBookAsync(string zipSettingId, CreatorRelayStation relayStation)
		{
			var settings = new ImageSourceAgent().GetZipFileSettings(zipSettingId);
			if (settings == null)
				return Task.CompletedTask;

			var extractor = new ArchiveFileExtractor();
			Task.Run(async () => await extractor.ExtractArchivesAsync(settings, relayStation))
				.ConfigureAwait(false);

			return Task.CompletedTask;
		}
	}
}
