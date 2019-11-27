using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using HalationGhost.WinApps.DatabaseAccesses;

namespace HalationGhost.WinApps.ImaZip.ImageFileSettings
{
	public class ImageSourceDataAccess : HalationGhostDbAccessBase
	{
		#region zipファイル設定保存

		public long? SaveZipSettings(ZipFileSettings zipFile)
		{
			zipFile.InsertDate = DateTime.Now;
			this.Connection.Execute(this.getSaveZipSettingsSql(),
				new
				{
					EXTRACT_FOLDER = zipFile.ImageFilesExtractedFolder.Value,
					INSERT_DATE = zipFile.InsertDate
				});

			return this.getAutoNumber("ZIP_FILE_SETTINGS");
		}

		private string getSaveZipSettingsSql()
		{
			return @"
INSERT INTO ZIP_FILE_SETTINGS (
	  EXTRACT_FOLDER
	, INSERT_DATE
) VALUES (
	  :EXTRACT_FOLDER
	, :INSERT_DATE
)
";
		}

		private long? getAutoNumber(string tableName)
		{
			var sql = @"
SELECT
	SSQ.seq
FROM
	sqlite_sequence SSQ
WHERE
	SSQ.name = :name
";
			var seq = this.Connection.QueryFirstOrDefault(sql, new { name = tableName });

			if (seq == null)
			{
				return null;
			}
			else
			{
				if (seq.seq == null)
					return null;
				else
					return (long)seq.seq;
			}
		}

		#endregion

		public void SaveImageSources(List<ImageSource> imageSources, long zipSettingId)
		{
			var sql = @"
INSERT INTO IMAGE_SOURCES (
	  ZIP_SETTINGS_ID
	, IMAGE_SOURCE_PATH
	, SOURCE_KIND
) VALUES (
	:ZIP_SETTINGS_ID
  , :IMAGE_SOURCE_PATH
  , :SOURCE_KIND
)
";
			imageSources.ForEach(s =>
			{
				this.Connection.Execute(sql, new
				{
					ZIP_SETTINGS_ID = zipSettingId,
					IMAGE_SOURCE_PATH = s.Path.Value,
					SOURCE_KIND = s.SourceKind.Value
				});
			});
		}

		public ImageSourceDataAccess() : base()
		{

		}
	}
}
