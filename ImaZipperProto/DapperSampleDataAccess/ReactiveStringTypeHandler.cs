using System.Data;
using Reactive.Bindings;
using static Dapper.SqlMapper;

namespace DapperSample
{
	public class ReactiveStringTypeHandler : TypeHandler<ReactiveProperty<string>>
	{
		public override ReactiveProperty<string> Parse(object value)
		{
			return new ReactiveProperty<string>(value.ToString());
		}

		public override void SetValue(IDbDataParameter parameter, ReactiveProperty<string> value)
		{
			parameter.DbType = DbType.String;
			parameter.Value = value.Value;
		}
	}
}
