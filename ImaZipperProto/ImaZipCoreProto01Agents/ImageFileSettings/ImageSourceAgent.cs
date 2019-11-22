using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HalationGhost.WinApps.ImaZip.ImageFileSettings
{
	public class ImageSourceAgent
	{
		public Task StartCreateZipAsync(ZipFileSettings settings)
		{
			using (var da = new ImageSourceDataAccess())
			{
				using (var tran = da.BeginTransaction())
				{
					try
					{

						tran.Commit();
					}
					catch (Exception e)
					{
						tran.Rollback();
						throw;
					}
				}
			}

			return Task.CompletedTask;
		}
	}
}
