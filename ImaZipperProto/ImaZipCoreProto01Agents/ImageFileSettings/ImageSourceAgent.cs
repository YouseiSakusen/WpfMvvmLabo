using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using HalationGhost.WinApps.ImaZip.AppSettings;
using System.IO;
using HalationGhost.WinApps.Utilities;

namespace HalationGhost.WinApps.ImaZip.ImageFileSettings
{
	public class ImageSourceAgent
	{
		#region zipファイル作成開始

		public Task StartCreateZipAsync(ZipFileSettings settings, IImaZipCoreProto01Settings appSettings)
		{
			var zipId = this.saveZipSettings(settings);
			if (!zipId.HasValue)
				return Task.CompletedTask;

			// ZipBookCreator起動
			Process.Start(new ProcessStartInfo()
			{
				UseShellExecute = true,
				FileName = Path.Combine(AssemblyUtility.GetExecutingPath(), appSettings.CreatorExeFileName),
				Arguments = zipId.Value.ToString()
			});

			return Task.CompletedTask;
		}

		private long? saveZipSettings(ZipFileSettings settings)
		{
			using (var da = new ImageSourceDataAccess())
			{
				using (var tran = da.BeginTransaction())
				{
					try
					{
						var id = da.SaveZipSettings(settings);
						if (!id.HasValue)
						{
							tran.Rollback();
							return null;
						}

						da.SaveImageSources(settings.ImageSources.ToList(), id.Value);
						tran.Commit();

						return id;
					}
					catch (Exception)
					{
						tran.Rollback();
						throw;
					}
				}
			}
		}

		#endregion
	}
}
