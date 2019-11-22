using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace HalationGhost.WinApps.DatabaseAccesses
{
	public interface IDbAccessHelper
	{
		public DbConnection GetConnection();

		public DbTransaction GetTransaction(DbConnection connection);
	}
}
