using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Disposables;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace HalationGhost.WinApps.ImaZip.ImageFileSettings
{
	/// <summary>
	/// 作成するZipファイルの設定情報を表します。
	/// </summary>
	public class ZipFileSettings : BindableBase, IDisposable
	{
		#region プロパティ

		/// <summary>
		/// イメージファイルのソースファイルを取得します。
		/// </summary>
		public ObservableCollection<ImageSource> ImageSources { get; } = new ObservableCollection<ImageSource>();

		/// <summary>
		/// イメージファイルの展開先フォルダのパスを取得・設定します。
		/// </summary>
		public ReactivePropertySlim<string> ImageFilesExtractedFolder { get; set; }

		/// <summary>
		/// 設定情報の状態を取得します。
		/// </summary>
		public ReactivePropertySlim<bool> IsComplete { get; }

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
				this.settingComplete.Value = false;
			else
				this.settingComplete.Value = 0 < this.ImageFilesExtractedFolder.Value.Length;
		}

		#endregion

		#region IDisposable Support

		private bool disposedValue = false; // 重複する呼び出しを検出するには

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					this.ImageSources.CollectionChanged -= this.ImageSources_CollectionChanged;
					this.disposables.Dispose();
				}

				// TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
				// TODO: 大きなフィールドを null に設定します。

				disposedValue = true;
			}
		}

		// TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
		// ~ZipFileSettings()
		// {
		//   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
		//   Dispose(false);
		// }

		// このコードは、破棄可能なパターンを正しく実装できるように追加されました。
		public void Dispose()
		{
			// このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
			Dispose(true);
			// TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
			// GC.SuppressFinalize(this);
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
