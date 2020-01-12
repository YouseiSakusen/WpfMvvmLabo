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
		Stopping,
		/// <summary>
		/// 完了を表します。
		/// </summary>
		Completed,
		/// <summary>
		/// 失敗して実行中止を表します。
		/// </summary>
		Failure
	}

	/// <summary>
	/// ZipBookの作成結果を表す列挙型。
	/// </summary>
	public enum ResultDetail
	{
		/// <summary>
		/// 結果無しを表します。
		/// </summary>
		ResultNone,
		/// <summary>
		/// 作成成功を表します。
		/// </summary>
		Success,
		/// <summary>
		/// イメージソース展開先フォルダ無しを表します。
		/// </summary>
		ImageExtractFolderNotFound
	}

	/// <summary>
	/// イメージソースの状態を表す列挙型。
	/// </summary>
	public enum ImageSourceState
	{
		/// <summary>
		/// 通常を表します。
		/// </summary>
		Normal,
		/// <summary>
		/// ソースファイルが存在しないことを表します。
		/// </summary>
		SourceFileNotFound,
		/// <summary>
		/// 解凍先フォルダが既に存在することを表します。
		/// </summary>
		ExtractFolderExisted
	}

	//public enum ImageDirection
	//{
	//	/// <summary>
	//	/// 形無し
	//	/// </summary>
	//	NoDirection,
	//	/// <summary>
	//	/// 正方形を表します。
	//	/// </summary>
	//	Square,
	//	/// <summary>
	//	/// 横長の長方形を表します。
	//	/// </summary>
	//	Landscape,
	//	/// <summary>
	//	/// 縦長の長方形を表します。
	//	/// </summary>
	//	Portrait
	//}
}