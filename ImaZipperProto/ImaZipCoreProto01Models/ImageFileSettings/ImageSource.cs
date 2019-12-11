using System;
using System.Collections.Generic;
using System.IO;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ZipBookCreator;

namespace HalationGhost.WinApps.ImaZip.ImageFileSettings
{
	/// <summary>
	/// 画像のソースを表します。
	/// </summary>
	public class ImageSource : BindableModelBase
	{
		#region プロパティ

		/// <summary>
		/// ImageSourceのパスを取得・設定します。
		/// </summary>
		public ReactivePropertySlim<string> Path { get; set; }

		public string IMAGE_SOURCE_PATH
		{
			get { return this.Path.Value; }
			set { this.Path.Value = value; }
		}

		/// <summary>
		/// ImageSourceの種類を取得します。
		/// </summary>
		public ReactivePropertySlim<ImageSourceType> SourceKind { get; set; }

		public ImageSourceType SOURCE_KIND
		{
			get { return this.SourceKind.Value; }
			set { this.SourceKind.Value = value; }
		}

		/// <summary>
		/// リスト内の並び順を取得・設定します。
		/// </summary>
		public int ListOrder { get; set; } = 0;

		/// <summary>
		/// イメージファイルが存在しないかを取得します。
		/// </summary>
		public bool IsNotExists
		{
			get
			{
				if (string.IsNullOrEmpty(this.Path.Value))
					return true;

				switch (this.SourceKind.Value)
				{
					case ImageSourceType.File:
						if (!File.Exists(this.Path.Value))
						{
							this.State = ImageSourceState.SourceFileNotFound;
							return true;
						}
						break;
					case ImageSourceType.Folder:
						if (!Directory.Exists(this.Path.Value))
						{
							this.State = ImageSourceState.SourceFileNotFound;
							return true;
						}
						break;
				}

				return false;
			}
		}

		public ImageSourceState State { get; set; } = ImageSourceState.Normal;

		private string fileNameWithoutExt;

		public string FileNameWithoutExtension
		{
			get { return fileNameWithoutExt; }
		}

		public string ExtractedRootDirectory { get; set; } = string.Empty;

		public List<SourceItem> ExtractedItems { get; } = new List<SourceItem>();

		#endregion

		private void onSetPath(string value)
		{
			this.fileNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(value);
		}

		#region コンストラクタ

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="sourcePath">ImageSourceのパスを表す文字列。</param>
		/// <param name="sourceType">ImageSourceの種類を表すImageSourceType列挙型の内の1つ。</param>
		public ImageSource(string sourcePath, ImageSourceType sourceType) : this()
		{
			this.Path.Value = sourcePath;
			this.SourceKind.Value = sourceType;
		}

		/// <summary>
		/// デフォルトコンストラクタ。
		/// </summary>
		public ImageSource()
		{
			this.Path = new ReactivePropertySlim<string>(string.Empty)
				.AddTo(this.Disposable);
			this.Path.Subscribe(p => this.onSetPath(p));

			this.SourceKind = new ReactivePropertySlim<ImageSourceType>(ImageSourceType.None)
				.AddTo(this.Disposable);
		}

		#endregion
	}
}
