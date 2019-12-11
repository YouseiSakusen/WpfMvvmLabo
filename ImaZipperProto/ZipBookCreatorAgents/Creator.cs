using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HalationGhost.WinApps.ImaZip.ImageFileSettings;

namespace HalationGhost.WinApps.ImaZip.ZipBookCreator
{
	public class Creator
	{
		public async Task CreateZipBookAsync(string zipSettingId, CreatorRelayStation relayStation)
		{
			// zip作成対象の情報を取得
			var settings = new ImageSourceAgent().GetZipFileSettings(zipSettingId);
			if (settings == null)
				return;

			// アーカイブファイルを展開
			var extractor = new ArchiveFileExtractor();
			if (!await extractor.StartExtractAsync(settings, relayStation))
				return;

			foreach (var imgSrc in settings.ImageSources)
			{
				if (imgSrc.IsNotExists)
					continue;

				await extractor.ExtractImageSourceAsync(imgSrc);
			}

			// ソースファイルをチェック
			new SourceFileCollector().CheckSourceFiles(settings);
		}
	}
}
