using System.Data;
using Reactive.Bindings;
using static Dapper.SqlMapper;

namespace DapperSample
{
	public class ReactiveSlimStringTypeHandler : TypeHandler<ReactivePropertySlim<string>>
	{
		public override ReactivePropertySlim<string> Parse(object value)
		{
			return new ReactivePropertySlim<string>(value.ToString());
		}

		public override void SetValue(IDbDataParameter parameter, ReactivePropertySlim<string> value)
		{
			parameter.DbType = DbType.String;
			parameter.Value = value.Value;
		}
	}
}
