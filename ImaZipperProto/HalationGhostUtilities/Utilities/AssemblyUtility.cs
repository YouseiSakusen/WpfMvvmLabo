using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace HalationGhost.WinApps.Utilities
{
	public static class AssemblyUtility
	{
		public static string GetExecutingPath()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		}
	}
}
