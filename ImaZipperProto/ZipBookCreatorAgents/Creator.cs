using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalationGhost.WinApps.ImaZip.ImageFileSettings;
using ZipBookCreator;

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

			Debug.WriteLine("StartExtractAsync 呼出し後");

			foreach (var imgSrc in settings.ImageSources)
			{
				if (imgSrc.IsNotExists)
					continue;

				await extractor.ExtractImageSourceAsync(imgSrc);
			}

			Debug.WriteLine("CheckSourceFiles 呼び出し前");

			//// ソースファイルをチェック
			//new SourceFileCollector().CheckSourceFiles(settings);
		}

		public async Task CreateBookZipAsync(string zipSettingId, CreatorRelayStation relayStation)
		{
			relayStation.AddLog($"************ zipファイル作成開始！ ************");

			// zip作成対象の情報を取得
			var settings = new ImageSourceAgent().GetZipFileSettings(zipSettingId);
			if (settings == null)
				return;

			// 展開エラーの有無チェック
			var extractor = new ArchiveFileExtractor(relayStation);
			if (!await Task.Run(() => extractor.HasExtractError(settings)))
				return;

			// 展開先ワークフォルダ作成
			await this.createWorkFolderAsync(settings);

			// アーカイブを展開
			await extractor.ExtractAsync(settings)
				.ContinueWith(async t => await this.createDistributesRules(settings))
				.ContinueWith(async t => await this.distributesSourceItems(settings));
		}

		private async Task createWorkFolderAsync(ZipFileSettings settings)
		{
			if (Directory.Exists(settings.WorkRootFolderPath))
				// workフォルダが存在した場合は削除
				await Task.Run(() => Directory.Delete(settings.WorkRootFolderPath, true));

			Directory.CreateDirectory(settings.WorkRootFolderPath);
			var dirInfo = new DirectoryInfo(settings.WorkRootFolderPath);
			dirInfo.Attributes |= FileAttributes.Hidden;
		}

		private async Task createDistributesRules(ZipFileSettings settings)
		{
			await Task.Run(() =>
			{
				Parallel.ForEach<ImageSource>(settings.ImageSources,
					s =>
					{
						Parallel.ForEach<SourceItem>(s.Entries, e => e.CreateDistributionRule());
					});
			});
		}


		private async Task distributesSourceItems(ZipFileSettings settings)
		{
			await Task.Run(() =>
			{
				foreach (var imgSrc in settings.ImageSources)
				{
					var roots = imgSrc.Entries.OrderBy(e => e.FileName);


				}
			});
		}
	}
}
