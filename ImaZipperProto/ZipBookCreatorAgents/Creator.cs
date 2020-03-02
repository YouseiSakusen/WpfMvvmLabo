using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HalationGhost.WinApps.Images;
using HalationGhost.WinApps.ImaZip.ImageFileSettings;

namespace HalationGhost.WinApps.ImaZip.ZipBookCreator
{
	/// <summary>zipファイルの作成処理を表します。</summary>
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

		/// <summary>Modelの情報をVMに中継します。</summary>
		private CreatorRelayStation relay = null;

		/// <summary>ブックデータを格納したzipファイルを作成します。</summary>
		/// <param name="zipSettingId">作成するzipファイル情報の登録済みIDを表す文字列。</param>
		/// <param name="relayStation">Modelの情報をVMに中継するためのCreatorRelayStation。</param>
		/// <returns>非同期処理の実行結果を表すTask。</returns>
		public async Task CreateBookZipAsync(string zipSettingId, CreatorRelayStation relayStation)
		{
			this.relay = relayStation;
			relayStation.AddLog($"************ zipファイル作成開始！ ************");

			// zip作成対象の情報を取得
			var settings = new ZipFileSettingsRepository().GetById(zipSettingId);
			if (settings == null)
				return;

//#if DEBUG
//			await Task.Run(() => Directory.Delete(settings.ImageFilesExtractedFolder.Value, true));
//#endif

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
				var seq = 1;

				foreach (var imgSrc in settings.ImageSources)
				{
					imgSrc.Entries
						.OrderBy(e => e.FileName)
						.ToList()
						.ForEach(e =>
						{
							this.createDestinationFolder(e, settings, seq);
							this.moveImageFiles(e, settings, seq);
							seq++;
						});
				}

				this.relay.AddLog($"************ 配置まですべて完了 ************");
			});
		}

		private void createDestinationFolder(SourceItem volumeRoot, ZipFileSettings settings, int volumeNumber)
		{
			if (!string.IsNullOrEmpty(volumeRoot.DestinationFolderPath))
				return;

			volumeRoot.DestinationFolderPath = Path.Combine(settings.ImageFilesExtractedFolder.Value, settings.GetVolumeRootFolderName(volumeNumber));
			if (!Directory.Exists(volumeRoot.DestinationFolderPath))
				Directory.CreateDirectory(volumeRoot.DestinationFolderPath);
		}

		private void moveImageFiles(SourceItem rootItem, ZipFileSettings settings, int volumeNumber)
		{
			var seq = 1;
			var fileCountLength = rootItem.Children.Count.ToString().Length;

			foreach (var imageItem in rootItem.Children.OrderBy(c => c.FileName))
			{
				if (!File.Exists(imageItem.ItemPath))
					continue;

				var destinationPath = this.getImageFileName(settings, volumeNumber, seq, fileCountLength, imageItem, rootItem);

				if (!imageItem.IsSplit)
				{
					File.Move(imageItem.ItemPath, destinationPath);
					seq++;
				}
				else
				{
					seq++;
					var leftImagePath = this.getImageFileName(settings, volumeNumber, seq, fileCountLength, imageItem, rootItem);
					ImageFile.SplitVerticalToFile(imageItem.ItemPath, leftImagePath, destinationPath);
					seq++;
				}
			}
		}

		private string getImageFileName(ZipFileSettings settings,
										int volumeNumber,
										int pageSequence,
										int fileCountLength,
										SourceItem imageItem,
										SourceItem rootItem)
		{
			var newFileName = settings.GetImageFileName(volumeNumber,
														pageSequence,
														fileCountLength) + Path.GetExtension(imageItem.ItemPath);

			return Path.Combine(rootItem.DestinationFolderPath, newFileName);
		}
	}
}
