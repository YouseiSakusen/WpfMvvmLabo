using System;
using System.Data.Common;

namespace HalationGhost.WinApps.DatabaseAccesses
{
	/// <summary>DBアクセスのベースクラスを表します。</summary>
	public abstract class HalationGhostDbAccessBase : IDisposable
	{
		/// <summary>データベースのコネクションを表します。</summary>
		protected DbConnection Connection { get; private set; } = null;

		/// <summary>トランザクションを開始します。</summary>
		/// <returns>DBのトランザクションを表すDbTransaction。</returns>
		public DbTransaction BeginTransaction()
			=> HalationGhostDbAccessBase.helper.GetTransaction(this.Connection);

		#region IDisposable Support

		private bool disposedValue = false; // 重複する呼び出しを検出するには

		/// <summary>このクラスを破棄します。</summary>
		/// <param name="disposing">破棄中かを表すbool。</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					this.Connection?.Dispose();
				}

				// TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
				// TODO: 大きなフィールドを null に設定します。

				disposedValue = true;
			}
		}

		// TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
		// ~HalationGhostDbAccessBase()
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

		/// <summary>デフォルトコンストラクタ。</summary>
		public HalationGhostDbAccessBase() : base()
			=> this.initializeConnection();

		/// <summary>キャッシュした接続文字列を表します。</summary>
		private static DbConnectionSetting connectionSetting = null;
		/// <summary>AbstractFactoryを表すIDbAccessHelper。</summary>
		private static IDbAccessHelper helper = null;

		/// <summary>
		/// 接続を初期化します。
		/// </summary>
		private void initializeConnection()
		{
			if (HalationGhostDbAccessBase.connectionSetting == null)
			{
				// キャッシュされていない場合は接続設定ファイルを読み込む
				HalationGhostDbAccessBase.connectionSetting = new DbConnectionSettingLoader().Load();
				if (HalationGhostDbAccessBase.connectionSetting == null)
					throw new Exception("DBの接続設定ファイルがLoadできません。");
			}

			// 接続する設定ファイル番号を取得
			var num = this.getConnectionNumber();

			if ((HalationGhostDbAccessBase.helper == null) || (!num.HasValue))
				HalationGhostDbAccessBase.helper = DbAccessHelperFactory.CreateHelper(HalationGhostDbAccessBase.connectionSetting, num);

			this.Connection = HalationGhostDbAccessBase.helper.GetConnection();
		}

		/// <summary>接続対象の設定ファイル番号を取得します。</summary>
		/// <returns>接続対象の設定ファイル番号を表すint?。</returns>
		protected virtual int? getConnectionNumber()
			=> null;

		#endregion
	}
}
