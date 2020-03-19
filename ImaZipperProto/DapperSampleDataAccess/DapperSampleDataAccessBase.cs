using Dapper;
using HalationGhost.WinApps.DatabaseAccesses;

namespace DapperSample
{
	/// <summary>DapperSampleサンプルアプリのDataAccess用親クラスを表します。</summary>
	public abstract class DapperSampleDataAccessBase : HalationGhostDbAccessBase
	{
		/// <summary>Dapperのマッピング設定を初期化します。</summary>
		public static void InitializedSqlMapper()
		{
			SqlMapper.AddTypeHandler(new ReactiveSlimLongTypeHandler());
			SqlMapper.AddTypeHandler(new ReactiveSlimStringTypeHandler());
		}

		/// <summary>コンストラクタ。</summary>
		public DapperSampleDataAccessBase()
			: base(new HalationGhostDbConnectSettingLoaderBase(string.Empty, "DbConnectSetting.xml")) { }
	}
}
