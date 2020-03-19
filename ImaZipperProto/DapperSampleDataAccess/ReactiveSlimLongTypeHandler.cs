using System.Data;
using Reactive.Bindings;
using static Dapper.SqlMapper;

namespace DapperSample
{
	/// <summary>ReactivePropertySlim<long>型用ハンドラを表します。</summary>
	public class ReactiveSlimLongTypeHandler : TypeHandler<ReactivePropertySlim<long>>
	{
		/// <summary>DBから取得した値からプロパティに設定する値を取得します。</summary>
		/// <param name="value">DBから取得した値を表すobject。</param>
		/// <returns>プロパティに設定する値を表すReactivePropertySlim<long>。</returns>
		public override ReactivePropertySlim<long> Parse(object value)
			=> new ReactivePropertySlim<long>((long)value);

		/// <summary>バインド変数にマッピングします。</summary>
		/// <param name="parameter">設定するDBパラメータを表すIDbDataParameter。</param>
		/// <param name="value">バインド変数にマッピングするプロパティを表すReactivePropertySlim<long>。</param>
		public override void SetValue(IDbDataParameter parameter, ReactivePropertySlim<long> value)
		{
			parameter.DbType = DbType.Int64;
			parameter.Value = value.Value;
		}
	}
}
