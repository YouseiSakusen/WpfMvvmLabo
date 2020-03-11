using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Dapper;
using HalationGhost.WinApps.DatabaseAccesses;

namespace HalationGhost.WinApps.ImaZip.ImageFileSettings
{
	/// <summary>ZipFileSettingsリポジトリを表します。</summary>
	public class ZipFileSettingsRepository : HalationGhostDbAccessBase, IZipFileSettingsRepository
	{
		/// <summary>IDを指定してZipFileSettingsを取得します。</summary>
		/// <param name="id">ZipFileSettingsのIDを表す文字列。</param>
		/// <returns>IDを指定して取得したZipFileSettings。</returns>
		public ZipFileSettings GetById(string id)
		{
			var dic = new Dictionary<int, ZipFileSettings>();

			this.Connection.Query<ZipFileSettings, ImageSource, ZipFileSettings>(
				this.createGetByIdSql(id),
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
				new { ID = id },
				splitOn: "IMAGE_SOURCE_PATH");

			return dic.Values.ToList().FirstOrDefault();
		}

		/// <summary>GetByIdで呼び出すSQLを生成します。</summary>
		/// <param name="id">ZipFileSettingsのIDを表す文字列。</param>
		/// <returns>GetByIdで呼び出すSQLを表す文字列。</returns>
		private string createGetByIdSql(string settingId)
		{
			var sql = new StringBuilder(500);
			sql.AppendLine(" SELECT ");
			sql.AppendLine(" 	  ZFS.ID ");
			sql.AppendLine(" 	, ZFS.EXTRACT_FOLDER ");
			sql.AppendLine(" 	, ZFS.FOLDER_NAME_SEQ ");
			sql.AppendLine(" 	, ZFS.FOLDER_NAME_TEMPLATE ");
			sql.AppendLine(" 	, ZFS.FILE_NAME_TEMPLATE ");
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

			return sql.ToString();
		}

		/// <summary>ZipFileSettingsを保存します。</summary>
		/// <param name="settings">保存するZipFileSettings。</param>
		/// <returns>保存したZipFileSettingsのIDを表すlong?。</returns>
		public long? Save(ZipFileSettings settings)
		{
			settings.InsertDate = DateTime.Now;
			this.Connection.Execute(this.getSaveZipSettingsSql(),
				new
				{
					EXTRACT_FOLDER = settings.ImageFilesExtractedFolder.Value,
					FOLDER_NAME_SEQ = settings.FolderNameSequenceDigit.Value,
					FOLDER_NAME_TEMPLATE = settings.FolderNameTemplate.Value,
					FILE_NAME_TEMPLATE = settings.FileNameTemplate.Value,
					INSERT_DATE = settings.InsertDate
				});

			var id = this.getAutoNumber("ZIP_FILE_SETTINGS");
			this.saveImageSources(settings.ImageSources, id);

			return id;
		}

		/// <summary>zipファイル設定を保存するSQLを取得します。</summary>
		/// <returns>zipファイル設定を保存するSQLを表す文字列。</returns>
		private string getSaveZipSettingsSql()
		{
			var sql = new StringBuilder(500);
			sql.AppendLine(" INSERT INTO ZIP_FILE_SETTINGS (  ");
			sql.AppendLine(" 	  EXTRACT_FOLDER ");
			sql.AppendLine(" 	, FOLDER_NAME_SEQ ");
			sql.AppendLine(" 	, FOLDER_NAME_TEMPLATE ");
			sql.AppendLine(" 	, FILE_NAME_TEMPLATE ");
			sql.AppendLine(" 	, INSERT_DATE ");
			sql.AppendLine(" ) VALUES (  ");
			sql.AppendLine(" 	  :EXTRACT_FOLDER ");
			sql.AppendLine(" 	, :FOLDER_NAME_SEQ ");
			sql.AppendLine(" 	, :FOLDER_NAME_TEMPLATE ");
			sql.AppendLine(" 	, :FILE_NAME_TEMPLATE ");
			sql.AppendLine(" 	, :INSERT_DATE ");
			sql.AppendLine(" ) ");

			return sql.ToString();
		}

		/// <summary>イメージソースのリストを保存します。</summary>
		/// <param name="imageSources">保存するイメージソースのリストを表すList<ImageSource>。</param>
		/// <param name="zipSettingId">zipファイル設定のIDを表すlong。</param>
		private void saveImageSources(ObservableCollection<ImageSource> imageSources, long? zipSettingId)
		{
			var sql = new StringBuilder(400);
			sql.AppendLine(" INSERT INTO IMAGE_SOURCES ( ");
			sql.AppendLine(" 	  ZIP_SETTINGS_ID ");
			sql.AppendLine(" 	, IMAGE_SOURCE_PATH ");
			sql.AppendLine(" 	, SOURCE_KIND ");
			sql.AppendLine(" 	, LIST_ORDER ");
			sql.AppendLine(" ) VALUES ( ");
			sql.AppendLine(" 	  :ZIP_SETTINGS_ID ");
			sql.AppendLine(" 	, :IMAGE_SOURCE_PATH ");
			sql.AppendLine(" 	, :SOURCE_KIND ");
			sql.AppendLine(" 	, :LIST_ORDER ");
			sql.AppendLine(" 	) ");

			var order = 1;

			imageSources.ToList().ForEach(s =>
			{
				this.Connection.Execute(sql.ToString(), new
				{
					ZIP_SETTINGS_ID = zipSettingId,
					IMAGE_SOURCE_PATH = s.Path.Value,
					SOURCE_KIND = s.SourceKind.Value,
					LIST_ORDER = order
				});

				order++;
			});
		}

		/// <summary>オートナンバー型フィールドに採番されたIDを取得します。</summary>
		/// <param name="tableName">Insert先のテーブル名を表す文字列。</param>
		/// <returns>オートナンバー型フィールドに採番されたIDを表すlong?。</returns>
		private long? getAutoNumber(string tableName)
		{
			var sql = new StringBuilder(200);
			sql.AppendLine(" SELECT ");
			sql.AppendLine(" 	SSQ.seq ");
			sql.AppendLine(" FROM ");
			sql.AppendLine(" 	sqlite_sequence SSQ ");
			sql.AppendLine(" WHERE ");
			sql.AppendLine(" 	SSQ.name = :name ");

			var seq = this.Connection.QueryFirstOrDefault(sql.ToString(), new { name = tableName });

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

		public ZipFileSettingsRepository()
			: base(new HalationGhostDbConnectSettingLoaderBase("Settings", "DbConnectSetting.xml")) { }
	}
}
