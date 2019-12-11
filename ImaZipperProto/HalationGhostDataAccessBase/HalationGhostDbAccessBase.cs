using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace HalationGhost.WinApps.DatabaseAccesses
{
	public abstract class HalationGhostDbAccessBase : IDisposable
	{
		protected DbConnection Connection { get; private set; } = null;

		public DbTransaction BeginTransaction()
			=> HalationGhostDbAccessBase.helper.GetTransaction(this.Connection);

		#region IDisposable Support

		private bool disposedValue = false; // 重複する呼び出しを検出するには

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

		public HalationGhostDbAccessBase() : base()
		{
			this.initializeConnection();
		}

		private static DbConnectionSetting connectionSetting = null;

		private static IDbAccessHelper helper = null;

		private void initializeConnection()
		{
			if (HalationGhostDbAccessBase.connectionSetting == null)
			{
				HalationGhostDbAccessBase.connectionSetting = new DbConnectionSettingLoader().Load();
				if (HalationGhostDbAccessBase.connectionSetting == null)
					throw new Exception("DBの接続設定ファイルがLoadできません。");
			}

			var num = this.getConnectionNumber();

			if ((HalationGhostDbAccessBase.helper == null) || (!num.HasValue))
				HalationGhostDbAccessBase.helper = DbAccessHelperFactory.CreateHelper(HalationGhostDbAccessBase.connectionSetting, num);

			this.Connection = HalationGhostDbAccessBase.helper.GetConnection();
		}

		protected virtual int? getConnectionNumber()
			=> null;

		#endregion
	}
}
