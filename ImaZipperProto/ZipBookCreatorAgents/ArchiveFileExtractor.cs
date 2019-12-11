using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HalationGhost.WinApps.ImaZip.ImageFileSettings;
using SharpCompress.Archives;
using ZipBookCreator;

namespace HalationGhost.WinApps.ImaZip.ZipBookCreator
{
	internal class ArchiveFileExtractor
	{
		private ZipFileSettings settings = null;
		private CreatorRelayStation relayStation = null;

		internal async Task<bool> StartExtractAsync(ZipFileSettings zipSettings, CreatorRelayStation creatorRelayStation)
		{
			this.settings = zipSettings;
			this.relayStation = creatorRelayStation;

			if (!await this.createWorkFolderAsync())
				return false;

			if (await this.hasExtractErrorAsync())
				return false;

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
				await Task.Run(() => Directory.Delete(this.settings.WorkRootFolderPath, true))
					.ConfigureAwait(false);

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
			return await Task.Run(() => ArchiveFactory.Open(filePath))
				.ConfigureAwait(false);
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

						entry.WriteToDirectory(dirPath);
					}
				}
			}).ConfigureAwait(false);
		}

		private string createParentDirectory(string value, string extractPath)
		{
			var folders = value.Split(new char[] { Path.DirectorySeparatorChar }).ToList();
			var parentDirName = folders.LastOrDefault();
			var extractDirPath = Path.Combine(extractPath, parentDirName);

			if (!Directory.Exists(extractDirPath))
				Directory.CreateDirectory(extractDirPath);

			return extractDirPath;
		}
	}
}
