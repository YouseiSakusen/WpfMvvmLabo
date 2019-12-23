using System.Drawing;
using System.Drawing.Imaging;
using ZipBookCreator;

namespace HalationGhost.WinApps.ImaZip
{
	/// <summary>
	/// イメージの情報を表します。
	/// </summary>
	public class ImageSpecification
	{
		#region プロパティ

		/// <summary>
		/// イメージファイルのパスを取得します。
		/// </summary>
		public string ImageFilePath { get; set; } = string.Empty;

		/// <summary>
		/// Image の高さ (ピクセル単位) を取得します。
		/// </summary>
		public int Height { get; } = 0;

		/// <summary>
		/// Image のファイル形式を取得します。
		/// </summary>
		public ImageFormat RawFormat { get; } = ImageFormat.Jpeg;

		/// <summary>
		/// イメージの幅と高さ (ピクセル単位) を取得します。
		/// </summary>
		public Size Size { get; } = new Size(0, 0);

		/// <summary>
		/// Image の幅 (ピクセル単位) を取得します。
		/// </summary>
		public int Width { get; } = 0;

		/// <summary>
		/// イメージの向きを取得します。
		/// </summary>
		public ImageDirection Direction { get; private set; } = ImageDirection.NoDirection;

		#endregion

		#region コンストラクタ

		public ImageSpecification(Image image, string filePath) : this(image)
		{
			this.ImageFilePath = filePath;
		}

		public ImageSpecification(Image image) : base()
		{
			this.Height = image.Height;
			this.RawFormat = image.RawFormat;
			this.Size = image.Size;
			this.Width = image.Width;

			this.setDirection();
		}

		private void setDirection()
		{
			if (this.Height == this.Width)
			{
				this.Direction = ImageDirection.Square;
			}
			else
			{
				if (this.Height < this.Width)
					this.Direction = ImageDirection.Landscape;
				else
					this.Direction = ImageDirection.Portrait;
			}
		}

		#endregion
	}
}
