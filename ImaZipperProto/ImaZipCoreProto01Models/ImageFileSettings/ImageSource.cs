using Reactive.Bindings;
using Reactive.Bindings.Extensions;

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

		#endregion

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
			this.SourceKind = new ReactivePropertySlim<ImageSourceType>(ImageSourceType.None)
				.AddTo(this.Disposable);
		}

		#endregion
	}
}
