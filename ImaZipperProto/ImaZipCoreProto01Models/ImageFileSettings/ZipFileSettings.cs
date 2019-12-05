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
		}

		#endregion
	}
}
