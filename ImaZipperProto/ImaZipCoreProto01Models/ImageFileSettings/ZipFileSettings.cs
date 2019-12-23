using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using Dapper.Contrib.Extensions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace HalationGhost.WinApps.ImaZip.ImageFileSettings
{
	/// <summary>
	/// 作成するZipファイルの設定情報を表します。
	/// </summary>
	public class ZipFileSettings : BindableModelBase
	{
		private const string WORK_ROOT_FOLDER_NAME = "WorkTemp";

		#region プロパティ

		public int? ID { get; set; } = null;

		/// <summary>
		/// イメージファイルのソースファイルを取得します。
		/// </summary>
		public ObservableCollection<ImageSource> ImageSources { get; } = new ObservableCollection<ImageSource>();

		/// <summary>
		/// イメージファイルの展開先フォルダのパスを取得・設定します。
		/// </summary>
		public ReactivePropertySlim<string> ImageFilesExtractedFolder { get; set; }

		public string EXTRACT_FOLDER
		{
			get { return this.ImageFilesExtractedFolder.Value; }
			set { this.ImageFilesExtractedFolder.Value = value; }
		}

		/// <summary>
		/// 設定情報の状態を取得します。
		/// </summary>
		public ReactivePropertySlim<bool> IsComplete { get; }

		public DateTime? InsertDate { get; set; } = null;

		public string WorkRootFolderPath { get; private set; } = string.Empty;

		public ReactivePropertySlim<int?> FolderNameSequenceDigit { get; set; }

		public int? FOLDER_NAME_SEQ
		{
			get { return this.FolderNameSequenceDigit.Value; }
			set { this.FolderNameSequenceDigit.Value = value; }
		}

		public ReactivePropertySlim<string> FolderNameTemplate { get; set; }

		public string FOLDER_NAME_TEMPLATE
		{
			get { return this.FolderNameTemplate.Value; }
			set { this.FolderNameTemplate.Value = value; }
		}

		public ReactivePropertySlim<string> FileNameTemplate { get; set; }

		public string FILE_NAME_TEMPLATE
		{
			get { return this.FileNameTemplate.Value; }
			set { this.FileNameTemplate.Value = value; }
		}

		#endregion

		#region メソッド

		/// <summary>
		/// イメージソースのパスを重複を削除して追加します。
		/// </summary>
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

		#endregion

		#region イベントハンドラ

		/// <summary>
		/// イメージソースのCollectionChangedイベントハンドラ。
		/// </summary>
		/// <param name="sender">イベントのソース。</param>
		/// <param name="e">イベントデータを格納しているNotifyCollectionChangedEventArgs。</param>
		private void ImageSources_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			=> this.updateSettingState();

		/// <summary>
		/// 設定情報の状態を更新します。
		/// </summary>
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

		/// <summary>
		/// IDisposableの集約先を表します。
		/// </summary>
		private CompositeDisposable disposables = new CompositeDisposable();

		/// <summary>
		/// 設定情報が完全かを表します。
		/// </summary>
		private ReactivePropertySlim<bool> settingComplete { get; set; }

		/// <summary>
		/// デフォルトコンストラクタ。
		/// </summary>
		public ZipFileSettings()
		{
			this.settingComplete = new ReactivePropertySlim<bool>(false)
				.AddTo(this.disposables);
			this.ImageSources.CollectionChanged += this.ImageSources_CollectionChanged;

			this.ImageFilesExtractedFolder = new ReactivePropertySlim<string>(string.Empty)
				.AddTo(this.disposables);
			this.ImageFilesExtractedFolder.Subscribe(_ => this.updateSettingState());

			this.IsComplete = this.settingComplete
				.AddTo(this.disposables);
			this.FolderNameSequenceDigit = new ReactivePropertySlim<int?>(2)
				.AddTo(this.disposables);
			this.FolderNameTemplate = new ReactivePropertySlim<string>("?巻")
				.AddTo(this.disposables);
			this.FileNameTemplate = new ReactivePropertySlim<string>("?_*")
				.AddTo(this.disposables);
		}

		#endregion
	}
}
