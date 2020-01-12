using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using HalationGhost.WinApps.ImaZip.ImageFileSettings;
using SharpCompress.Archives;

namespace HalationGhost.WinApps.ImaZip.ZipBookCreator
{
	internal class ArchiveFileExtractorTpl
	{
		internal async Task<bool> HasExtractError(ZipFileSettings zipSettings)
		{
			long total = 0;
			var sumBlock = new ActionBlock<IArchiveEntry>(async e =>
				await Task.Run(() => total += e.Size), new ExecutionDataflowBlockOptions()
															{ MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded });
			var extractTasks = new List<Task>();

			foreach (var imageSource in zipSettings.ImageSources)
			{
				if (imageSource.SourceKind.Value != ImageSourceType.File)
					continue;

				extractTasks.Add(Task.Run(() =>
				{
					using (var archive = ArchiveFactory.Open(imageSource.Path.Value))
					{
						imageSource.ArchiveEntryTotalCount = archive.Entries.Where(e => !e.IsDirectory).Count();

						foreach (var entry in archive.Entries)
						{
							if (SourceItem.IsTargetFile(entry))
							{
								imageSource.ArchiveEntryTargetCount++;
								sumBlock.Post(entry);
							}
						}
					}
				}));
			}

			await Task.WhenAll(extractTasks);
			sumBlock.Complete();
			await sumBlock.Completion;

			var freeSpace = zipSettings.GetExtractPathFreeSpace();

			return total < freeSpace;
		}
	}
}
