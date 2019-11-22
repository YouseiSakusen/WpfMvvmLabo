using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HalationGhost.WinApps.Utilities;

namespace HalationGhost.WinApps.DatabaseAccesses
{

	internal class DbConnectionSettingLoader
	{
		private const string FLE_SETTING_FILE_NAME = "DbConnectSetting.xml";

		private const string FLD_SETTING_FOLDER_NAME = "Settings";

		internal DbConnectionSetting Load()
		{
			return SerializeUtility.DeserializeFromFile<DbConnectionSetting>(this.getSettingFilePath());
		}

		private string getSettingFilePath()
		{
			var execPath = AssemblyUtility.GetExecutingPath();

			return Path.Combine(execPath,
				DbConnectionSettingLoader.FLD_SETTING_FOLDER_NAME,
				DbConnectionSettingLoader.FLE_SETTING_FILE_NAME);
		}
	}
}
