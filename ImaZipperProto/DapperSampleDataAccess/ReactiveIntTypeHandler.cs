using System.Data;
using Reactive.Bindings;
using static Dapper.SqlMapper;

namespace DapperSample
{
	public class ReactiveIntTypeHandler : TypeHandler<ReactiveProperty<int>>
	{
		public override ReactiveProperty<int> Parse(object value)
		{
			return new ReactiveProperty<int>((int)value);
		}

		public override void SetValue(IDbDataParameter parameter, ReactiveProperty<int> value)
		{
			parameter.DbType = DbType.Int32;
			parameter.Value = value.Value;
		}
	}
}
