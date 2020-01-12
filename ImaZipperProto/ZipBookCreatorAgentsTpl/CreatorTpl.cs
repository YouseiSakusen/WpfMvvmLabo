using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using HalationGhost.WinApps.ImaZip.ImageFileSettings;

namespace HalationGhost.WinApps.ImaZip.ZipBookCreator
{
	public class CreatorTpl
	{
		private CreatorRelayStation relayStation = null;

		public async Task CreateZipBookAsync(string zipSettingId, CreatorRelayStation relay)
		{
			this.relayStation = relay;

			var settings = await new ImageSourceAgent().GetZipFileSettingsAsync(zipSettingId);
			if (settings == null)
				return;

//#if DEBUG
//			await Task.Run(() => Directory.Delete(settings.ImageFilesExtractedFolder.Value, true));
//#endif

			if (!await new ArchiveFileExtractorTpl().HasExtractError(settings))
				return;
		}
	}
}
