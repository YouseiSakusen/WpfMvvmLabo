using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HalationGhost.WinApps.Images;
using HalationGhost.WinApps.ImaZip.ImageFileSettings;
using SharpCompress.Archives;
using ZipBookCreator;

namespace HalationGhost.WinApps.ImaZip.ZipBookCreator
{
	internal class ArchiveFileExtractor
	{
		private ZipFileSettings settings = null;
		private CreatorRelayStation relayStation = null;

		internal bool HasExtractError(ZipFileSettings zipSettings)
		{
			long totalSize = 0;
			var watch = Stopwatch.StartNew();
			
			Parallel.ForEach<ImageSource>(zipSettings.ImageSources, src =>
			{
				Debug.WriteLine($"容量取得：{src.Path.Value}");

				using (var archive = ArchiveFactory.Open(src.Path.Value))
				{
					src.ArchiveEntryTotalCount = archive.Entries.Where(e => !e.IsDirectory).Count();
					var entries = archive.Entries.Where(e => SourceItem.IsTargetFile(e));

					src.ArchiveEntryTargetCount = entries.Count();

					Parallel.ForEach<IArchiveEntry, long>
					(
						entries,
						() => 0,
						(entry, state, local) =>
						{
							local += entry.Size;
						//Debug.WriteLine($"容量取得(entry)：{entry.Key}");

							return local;
						},
						local => Interlocked.Add(ref totalSize, local)
					);
				}
			});

			var freeSpace = zipSettings.GetExtractPathFreeSpace();

			watch.Stop();

			// 圧縮ファイル内トータルサイズ取得：215[ms]
			// 圧縮ファイル内トータルサイズ取得(Debug.WriteLineあり)：6828[ms]
			// 圧縮ファイル内トータルサイズ取得(Debug.WriteLineなし)：221[ms]

			return totalSize < freeSpace;
		}

		internal async Task ExtractAsync(ZipFileSettings settings)
		{
			await Task.Run(() =>
			{
				var watch = Stopwatch.StartNew();
				var agent = new ImageAgent();

				Parallel.ForEach<ImageSource>(settings.ImageSources,
					s =>
					{
						s.ExtractedRootDirectory = Path.Combine(settings.WorkRootFolderPath, s.FileNameWithoutExtension);
						Directory.CreateDirectory(s.ExtractedRootDirectory);

						this.extractImageSourceAsync(s, agent);
					});

				watch.Stop();
				this.relayStation.AddLog($"展開完了！");
				this.relayStation.AddLog($"総展開時間：{watch.ElapsedMilliseconds / 1000}[sec]");
			});
		}

		//private async Task extractImageSourceAsync(ImageSource source, ImageAgent agent)
		private void extractImageSourceAsync(ImageSource source, ImageAgent agent)
		{
			//await Task.Run(async () =>
			//{
			using (var archive = ArchiveFactory.Open(source.Path.Value))
			{
				var entries = archive.Entries.Where(e => SourceItem.IsTargetFile(e));

				foreach (var e in entries)
				{
					var targetFolder = source.GetExtractedFolder(e.Key);

					e.WriteToDirectory(targetFolder.ItemPath);

					var extractedPath = this.getExtractedFilePath(e, targetFolder);
					targetFolder.Children.Add(new SourceItem(ImageFile.GetImageSpecification(extractedPath)));
					//targetFolder.Children.Add(new SourceItem(agent.GetImageSpecification(extractedPath)));
					//targetFolder.Children.Add(new SourceItem(await agent.GetImageSpecificationAsync(extractedPath)));
				}
			}
			//});
		}

		private string getExtractedFilePath(IArchiveEntry entry, SourceItem extractedFolder)
			=> Path.Combine(extractedFolder.ItemPath, Path.GetFileName(entry.Key));


		public ArchiveFileExtractor(CreatorRelayStation relayStation) : this()
		{
			this.relayStation = relayStation;
		}

		public ArchiveFileExtractor() : base() { }



		private string createParentDirectory(string value, string extractPath)
		{
			var folders = value.Split(new char[] { Path.DirectorySeparatorChar }).ToList();
			var parentDirName = folders.LastOrDefault();
			var extractDirPath = Path.Combine(extractPath, parentDirName);

			if (!Directory.Exists(extractDirPath))
				Directory.CreateDirectory(extractDirPath);

			return extractDirPath;
		}

		internal async Task<bool> StartExtractAsync(ZipFileSettings zipSettings, CreatorRelayStation creatorRelayStation)
		{
			this.settings = zipSettings;
			this.relayStation = creatorRelayStation;

			if (!await this.createWorkFolderAsync())
				return false;

			Debug.WriteLine("StartExtractAsync.createWorkFolderAsync 呼出し後");

			var watch = Stopwatch.StartNew();

			if (await this.hasExtractErrorAsync())
				return false;

			watch.Stop();
			Debug.WriteLine($"圧縮ファイル内トータルサイズ取得：{watch.ElapsedMilliseconds}[ms]");
			// 圧縮ファイル内トータルサイズ取得(Debug.WriteLineあり)：5282[ms]
			// 圧縮ファイル内トータルサイズ取得(Debug.WriteLineなし)：550[ms]

			return true;
		}

		private async Task<bool> createWorkFolderAsync()
		{
			if (!Directory.Exists(this.settings.ImageFilesExtractedFolder.Value))
			{
				relayStation.SetCreateResult(ResultDetail.ImageExtractFolderNotFound);
				return false;
			}

			if (Directory.Exists(this.settings.WorkRootFolderPath))
				await Task.Run(() => Directory.Delete(this.settings.WorkRootFolderPath, true));

			Debug.WriteLine("createWorkFolderAsync 呼出し後");

			Directory.CreateDirectory(this.settings.WorkRootFolderPath);
			var dirInfo = new DirectoryInfo(this.settings.WorkRootFolderPath);
			dirInfo.Attributes |= FileAttributes.Hidden;

			return true;
		}

		private async Task<bool> hasExtractErrorAsync()
		{
			long totalSize = 0;

			foreach (var imgSrc in this.settings.ImageSources)
			{
				if (imgSrc.IsNotExists)
					continue;

				using (var archive = await this.openArchiveAsync(imgSrc.Path.Value))
				{
					totalSize += await this.getArchiveEntrySourceSizeAsync(archive);
				}
			}

			var freeSpace = this.settings.GetExtractPathFreeSpace();

			return freeSpace < totalSize;
		}

		private async Task<IArchive> openArchiveAsync(string filePath)
		{
			Debug.WriteLine("openArchiveAsync");

			return await Task.Run(() => ArchiveFactory.Open(filePath));
		}

		private async Task<long> getArchiveEntrySourceSizeAsync(IArchive archive)
		{
			long totalSize = 0;

			await Task.Run(() =>
			{
				foreach (var entry in archive.Entries)
				{
					if (SourceItem.IsTargetFile(entry))
					{
						//Debug.WriteLine("getArchiveEntrySourceSizeAsync" + entry.Key);

						totalSize += entry.Size;
					}
				}
			}).ConfigureAwait(false);

			return totalSize;
		}

		internal async Task ExtractImageSourceAsync(ImageSource source)
		{
			source.ExtractedRootDirectory = Path.Combine(this.settings.WorkRootFolderPath, source.FileNameWithoutExtension);
			Directory.CreateDirectory(source.ExtractedRootDirectory);

			using (var archive = await this.openArchiveAsync(source.Path.Value))
			{
				await this.extractImageFilesAsync(archive, source.ExtractedRootDirectory);
			}
		}

		private async Task extractImageFilesAsync(IArchive archive, string extractDir)
		{
			await Task.Run(() =>
			{
				foreach (var entry in archive.Entries)
				{
					if (SourceItem.IsTargetFile(entry))
					{
						var dirPath = this.createParentDirectory(Path.GetDirectoryName(entry.Key), extractDir);

						Debug.WriteLine("extractImageFilesAsync" + entry.Key);
						entry.WriteToDirectory(dirPath);
					}
				}
			}).ConfigureAwait(false);
		}
	}
}
