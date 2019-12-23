using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using HalationGhost.WinApps.DatabaseAccesses;

namespace HalationGhost.WinApps.ImaZip.ImageFileSettings
{
	/// <summary>
	/// イメージソース用のデータアクセスを表します。
	/// </summary>
	public class ImageSourceDataAccess : HalationGhostDbAccessBase
	{
		#region zipファイル設定保存

		/// <summary>
		/// 作成するzipファイルの設定を保存します。
		/// </summary>
		/// <param name="zipFile">保存するzipファイル設定を表すZipFileSettings。</param>
		/// <returns>zipファイルの設定IDを表すlong?</returns>
		public long? SaveZipSettings(ZipFileSettings zipFile)
		{
			zipFile.InsertDate = DateTime.Now;
			this.Connection.Execute(this.getSaveZipSettingsSql(),
				new
				{
					EXTRACT_FOLDER = zipFile.ImageFilesExtractedFolder.Value,
					FOLDER_NAME_SEQ = zipFile.FolderNameSequenceDigit.Value,
					FOLDER_NAME_TEMPLATE = zipFile.FolderNameTemplate.Value,
					INSERT_DATE = zipFile.InsertDate
				});

			return this.getAutoNumber("ZIP_FILE_SETTINGS");
		}

		/// <summary>
		/// zipファイル設定を保存するSQLを取得します。
		/// </summary>
		/// <returns>zipファイル設定を保存するSQLを表す文字列。</returns>
		private string getSaveZipSettingsSql()
		{
			var sql = new StringBuilder(500);
			sql.AppendLine(" INSERT INTO ZIP_FILE_SETTINGS (  ");
			sql.AppendLine(" 	  EXTRACT_FOLDER ");
			sql.AppendLine(" 	, FOLDER_NAME_SEQ ");
			sql.AppendLine(" 	, FOLDER_NAME_TEMPLATE ");
			sql.AppendLine(" 	, INSERT_DATE ");
			sql.AppendLine(" ) VALUES (  ");
			sql.AppendLine(" 	  :EXTRACT_FOLDER ");
			sql.AppendLine(" 	, :FOLDER_NAME_SEQ ");
			sql.AppendLine(" 	, :FOLDER_NAME_TEMPLATE ");
			sql.AppendLine(" 	, :INSERT_DATE ");
			sql.AppendLine(" ) ");

			return sql.ToString();
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

		#region イメージソースリスト保存

		/// <summary>
		/// イメージソースのリストを保存します。
		/// </summary>
		/// <param name="imageSources">保存するイメージソースのリストを表すList<ImageSource>。</param>
		/// <param name="zipSettingId">zipファイル設定のIDを表すlong。</param>
		public void SaveImageSources(List<ImageSource> imageSources, long zipSettingId)
		{
			var sql = @"
INSERT INTO IMAGE_SOURCES (
	  ZIP_SETTINGS_ID
	, IMAGE_SOURCE_PATH
	, SOURCE_KIND
	, LIST_ORDER
) VALUES (
	  :ZIP_SETTINGS_ID
	, :IMAGE_SOURCE_PATH
	, :SOURCE_KIND
	, :LIST_ORDER
)
";
			var order = 1;

			imageSources.ForEach(s =>
			{
				this.Connection.Execute(sql, new
				{
					ZIP_SETTINGS_ID = zipSettingId,
					IMAGE_SOURCE_PATH = s.Path.Value,
					SOURCE_KIND = s.SourceKind.Value,
					LIST_ORDER = order
				});

				order++;
			});
		}

		#endregion

		public ZipFileSettings GetZipFileSettings(string settingId)
		{
			var sql = new StringBuilder(500);
			sql.AppendLine(" SELECT ");
			sql.AppendLine(" 	  ZFS.ID ");
			sql.AppendLine(" 	, ZFS.EXTRACT_FOLDER ");
			sql.AppendLine(" 	, ZFS.FOLDER_NAME_SEQ ");
			sql.AppendLine(" 	, ZFS.FOLDER_NAME_TEMPLATE ");
			sql.AppendLine(" 	, IMG.ZIP_SETTINGS_ID ");
			sql.AppendLine(" 	, IMG.IMAGE_SOURCE_PATH ");
			sql.AppendLine(" 	, IMG.SOURCE_KIND ");
			sql.AppendLine(" FROM ");
			sql.AppendLine(" 	ZIP_FILE_SETTINGS ZFS ");
			sql.AppendLine(" 	INNER JOIN IMAGE_SOURCES IMG ON ");
			sql.AppendLine(" 			ZFS.ID = :ID ");
			sql.AppendLine(" 		AND ZFS.ID = IMG.ZIP_SETTINGS_ID ");
			sql.AppendLine(" ORDER BY ");
			sql.AppendLine(" 	IMG.LIST_ORDER ");

			var dic = new Dictionary<int, ZipFileSettings>();

			this.Connection.Query<ZipFileSettings, ImageSource, ZipFileSettings>(
				sql.ToString(),
				(z, i) =>
				{
					ZipFileSettings entry = null;

					if (!dic.TryGetValue(z.ID.Value, out entry))
					{
						dic.Add(z.ID.Value, z);
						entry = z;
					}

					entry.ImageSources.Add(i);
					return entry;
				},
				new { ID = settingId },
				splitOn: "IMAGE_SOURCE_PATH");

			return dic.Values.ToList().FirstOrDefault();
		}

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public ImageSourceDataAccess() : base()
		{

		}
	}
}
