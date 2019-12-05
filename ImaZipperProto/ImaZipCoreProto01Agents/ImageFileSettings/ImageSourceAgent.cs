using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HalationGhost.WinApps.ImaZip.AppSettings;
using HalationGhost.WinApps.Utilities;

namespace HalationGhost.WinApps.ImaZip.ImageFileSettings
{
	/// <summary>
	/// イメージソースのエージェントを表します。
	/// </summary>
	public class ImageSourceAgent
	{
		#region zipファイル作成開始

		/// <summary>
		/// zipファイルの作成を開始します。
		/// </summary>
		/// <param name="settings">zipファイル設定を表すZipFileSettings。</param>
		/// <param name="appSettings">アプリの設定を表すIImaZipCoreProto01Settings。</param>
		/// <returns>zipファイルの作成開始Task。</returns>
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

		/// <summary>
		/// zipファイル設定を保存します。
		/// </summary>
		/// <param name="settings">保存するzipファイル設定を表すZipFileSettings。</param>
		/// <returns>zipファイル設定IDを表すlong?。</returns>
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

		public ZipFileSettings GetZipFileSettings(string settingId)
		{
			using (var da = new ImageSourceDataAccess())
			{
				var setting = da.GetZipFileSettings(settingId);

				return setting;
			}
		}
	}
}
