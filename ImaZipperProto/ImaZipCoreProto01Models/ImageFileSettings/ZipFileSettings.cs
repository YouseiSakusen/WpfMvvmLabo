using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace HalationGhost.WinApps.ImaZip.ImageFileSettings
{
	/// <summary>作成するZipファイルの設定情報を表します。</summary>
	public class ZipFileSettings : BindableModelBase
	{
		private const string WORK_ROOT_FOLDER_NAME = "WorkTemp";

		#region プロパティ

		public int? ID { get; set; } = null;

		/// <summary>イメージファイルのソースファイルを取得します。</summary>
		public ObservableCollection<ImageSource> ImageSources { get; } = new ObservableCollection<ImageSource>();

		/// <summary>イメージファイルの展開先フォルダのパスを取得・設定します。</summary>
		public ReactivePropertySlim<string> ImageFilesExtractedFolder { get; set; }

		public string EXTRACT_FOLDER
		{
			get { return this.ImageFilesExtractedFolder.Value; }
			set { this.ImageFilesExtractedFolder.Value = value; }
		}

		/// <summary>設定情報の状態を取得します。</summary>
		public ReactivePropertySlim<bool> IsComplete { get; }

		public DateTime? InsertDate { get; set; } = null;

		public string WorkRootFolderPath { get; private set; } = string.Empty;

		/// <summary>フォルダ名の連番桁数を取得・設定します。</summary>
		public ReactivePropertySlim<int?> FolderNameSequenceDigit { get; set; }

		public int? FOLDER_NAME_SEQ
		{
			get { return this.FolderNameSequenceDigit.Value; }
			set { this.FolderNameSequenceDigit.Value = value; }
		}

		/// <summary>フォルダ名のテンプレートを取得・設定します。</summary>
		public ReactivePropertySlim<string> FolderNameTemplate { get; set; }

		public string FOLDER_NAME_TEMPLATE
		{
			get { return this.FolderNameTemplate.Value; }
			set { this.FolderNameTemplate.Value = value; }
		}

		/// <summary>ファイル名のテンプレートを取得・設定します。</summary>
		public ReactivePropertySlim<string> FileNameTemplate { get; set; }

		public string FILE_NAME_TEMPLATE
		{
			get { return this.FileNameTemplate.Value; }
			set { this.FileNameTemplate.Value = value; }
		}

		#endregion

		#region メソッド

		/// <summary>イメージソースのパスを重複を削除して追加します。</summary>
		/// <param name="sourcePaths">イメージソースに追加するパスを表すList<string>。</param>
		/// <param name="sourceType">追加するイメージソースの種類を表すImageSourceType列挙型の内の1つ。</param>
		public void MergeToViewModels(List<string> sourcePaths, ImageSourceType sourceType)
		{
			sourcePaths
				.Where(p => this.ImageSources.All(s => s.Path.Value != p))
				.ToList()
				.ForEach(p => this.ImageSources.Add(new ImageSource(p, sourceType)));
		}

		public long GetExtractPathFreeSpace()
		{
			var driveName = Path.GetPathRoot(this.ImageFilesExtractedFolder.Value)
				.ToList()
				.FirstOrDefault()
				.ToString();
			var drive = new DriveInfo(driveName);

			if (drive.IsReady)
				return drive.AvailableFreeSpace;
			else
				return 0;
		}

		public string GetVolumeRootFolderName(int volumeNumber)
		{
			return this.FolderNameTemplate.Value
				.Replace("?", volumeNumber.ToString()
					.PadLeft(this.FolderNameSequenceDigit.Value.Value, '0'));
		}

		public string GetImageFileName(int volumeNumber, int fileSequence, int totalFileCountLength)
		{
			var fileName = this.FileNameTemplate.Value
					.Replace("?", volumeNumber.ToString()
						.PadLeft(this.FolderNameSequenceDigit.Value.Value, '0'));

			return fileName.Replace("*", fileSequence.ToString()
											.PadLeft(totalFileCountLength, '0'));
		}

		#endregion

		#region イベントハンドラ

		/// <summary>イメージソースのCollectionChangedイベントハンドラ。</summary>
		/// <param name="sender">イベントのソース。</param>
		/// <param name="e">イベントデータを格納しているNotifyCollectionChangedEventArgs。</param>
		private void ImageSources_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			=> this.updateSettingState();

		/// <summary>設定情報の状態を更新します。</summary>
		private void updateSettingState()
		{
			if (this.ImageSources.Count == 0)
			{
				this.settingComplete.Value = false;
				this.WorkRootFolderPath = string.Empty;
			}
			else
			{
				this.settingComplete.Value = 0 < this.ImageFilesExtractedFolder.Value.Length;
				this.WorkRootFolderPath = Path.Combine(this.ImageFilesExtractedFolder.Value, ZipFileSettings.WORK_ROOT_FOLDER_NAME);
			}
		}

		#endregion

		#region コンストラクタ

		/// <summary>設定情報が完全かを表します。</summary>
		private ReactivePropertySlim<bool> settingComplete { get; set; }

		/// <summary>デフォルトコンストラクタ。</summary>
		public ZipFileSettings()
		{
			this.settingComplete = new ReactivePropertySlim<bool>(false)
				.AddTo(this.Disposable);
			this.ImageSources.CollectionChanged += this.ImageSources_CollectionChanged;

			this.ImageFilesExtractedFolder = new ReactivePropertySlim<string>(string.Empty)
				.AddTo(this.Disposable);
			this.ImageFilesExtractedFolder.Subscribe(_ => this.updateSettingState());

			this.IsComplete = this.settingComplete
				.AddTo(this.Disposable);
			this.FolderNameSequenceDigit = new ReactivePropertySlim<int?>(2)
				.AddTo(this.Disposable);
			this.FolderNameTemplate = new ReactivePropertySlim<string>("?巻")
				.AddTo(this.Disposable);
			this.FileNameTemplate = new ReactivePropertySlim<string>("?_*")
				.AddTo(this.Disposable);
		}

		#endregion
	}
}
