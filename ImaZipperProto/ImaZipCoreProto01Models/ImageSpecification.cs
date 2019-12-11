using System.Drawing;
using System.Drawing.Imaging;

namespace HalationGhost.WinApps.ImaZip
{
	/// <summary>
	/// イメージの情報を表します。
	/// </summary>
	public class ImageSpecification
	{
		#region プロパティ

		/// <summary>
		/// Image の高さ (ピクセル単位) を取得・設定します。
		/// </summary>
		public int Height { get; set; } = 0;

		/// <summary>
		/// Image のファイル形式を取得・設定します。
		/// </summary>
		public ImageFormat RawFormat { get; set; } = ImageFormat.Jpeg;

		/// <summary>
		/// イメージの幅と高さ (ピクセル単位) を取得・設定します。
		/// </summary>
		public Size Size { get; set; }

		/// <summary>
		/// Image の幅 (ピクセル単位) を取得・設定します。
		/// </summary>
		public int Width { get; set; } = 0;

		#endregion

		public ImageSpecification(Image image) : base()
		{
			this.Height = image.Height;
			this.RawFormat = image.RawFormat;
			this.Size = image.Size;
			this.Width = image.Width;
		}
	}
}
