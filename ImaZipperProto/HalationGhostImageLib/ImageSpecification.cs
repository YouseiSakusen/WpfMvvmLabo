using System.Drawing;
using System.Drawing.Imaging;

namespace HalationGhost.WinApps.Images
{
	#region 列挙型

	public enum ImageDirection
	{
		/// <summary>
		/// 形無し
		/// </summary>
		NoDirection,
		/// <summary>
		/// 正方形を表します。
		/// </summary>
		Square,
		/// <summary>
		/// 横長の長方形を表します。
		/// </summary>
		Landscape,
		/// <summary>
		/// 縦長の長方形を表します。
		/// </summary>
		Portrait
	}

	#endregion

	/// <summary>
	/// イメージの情報を表します。
	/// </summary>
	public class ImageSpecification
	{
		#region プロパティ

		/// <summary>
		/// イメージファイルのパスを取得します。
		/// </summary>
		public string ImageFilePath { get; } = string.Empty;

		/// <summary>
		/// Image の高さ (ピクセル単位) を取得します。
		/// </summary>
		public int Height { get; } = 0;

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

		public ImageSpecification(string filePath, int width, int height) : base()
		{
			this.ImageFilePath = filePath;
			this.Width = width;
			this.Height = height;
			this.Size = new Size(width, height);

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
