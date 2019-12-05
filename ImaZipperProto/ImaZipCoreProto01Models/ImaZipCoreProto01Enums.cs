/// <summary>
/// イメージソースの種類を表す列挙型。
/// </summary>
public enum ImageSourceType
{
	/// <summary>
	/// 種類無しを表します。（初期化用）
	/// </summary>
	None,
	/// <summary>
	/// ファイルを表します。
	/// </summary>
	File,
	/// <summary>
	/// フォルダを表します。
	/// </summary>
	Folder
}

namespace ZipBookCreator
{
	/// <summary>
	/// ZipBookの作成過程を表す列挙型。
	/// </summary>
	public enum CreateProcess
	{
		/// <summary>
		/// 停止中を表します。
		/// </summary>
		Stopping
	}
}