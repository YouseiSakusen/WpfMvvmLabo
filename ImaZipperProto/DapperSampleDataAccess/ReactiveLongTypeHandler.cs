using System.Data;
using Reactive.Bindings;
using static Dapper.SqlMapper;

namespace DapperSample
{
	public class ReactiveLongTypeHandler : TypeHandler<ReactiveProperty<long>>
	{
		public override ReactiveProperty<long> Parse(object value)
		{
			return new ReactiveProperty<long>((long)value);
		}

		public override void SetValue(IDbDataParameter parameter, ReactiveProperty<long> value)
		{
			parameter.DbType = DbType.Int64;
			parameter.Value = value.Value;
		}
	}
}
