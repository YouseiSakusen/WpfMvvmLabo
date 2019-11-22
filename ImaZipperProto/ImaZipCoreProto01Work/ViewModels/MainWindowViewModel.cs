using System.Windows;
using HalationGhost.WinApps.DatabaseAccesses;
using HalationGhost.WinApps.ImaZip.AppSettings;
using HalationGhost.WinApps.Utilities;
using Prism.Mvvm;
using Reactive.Bindings;

namespace HalationGhost.WinApps.ImaZip.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
		private string _title = "Prism Application";
		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

		public ReactiveCommand SaveSettings { get; }

		private void onSaveSettings()
		{
			var settings = new ImaZipCoreProto01Settings()
			{
				SourceFileSelectedFilter = "アーカイブファイル(*.zip,*.rar)|*.zip;*.rar"
			};

			AppSettingsService.SaveSettings(settings);
			MessageBox.Show("保存!");
		}

		public ReactiveCommand CreateDbConnectSetting { get; }

		private void onCreateDbConnectSetting()
		{
			var connectInfo = new DbConnectInformation()
			{
				DataSource = @"{exePath}\Settings\ImaZipWorkData.sqlite3",
				DbType = DatabaseType.SQLite,
				Number = 0
			};

			var setting = new DbConnectionSetting() { TargetNumber = 0 };
			setting.ConnectInformations.Add(connectInfo);

			SerializeUtility.SerializeToFile<DbConnectionSetting>(@"D:\MyDocuments\elfApp_Proto\ImaZipCoreProto01\ImaZipCoreProto01\bin\Debug\netcoreapp3.0\Settings\DbConnectSetting.xml", setting);
			MessageBox.Show("保存!");
		}

		public MainWindowViewModel()
		{
			this.SaveSettings = new ReactiveCommand()
				.WithSubscribe(() => this.onSaveSettings());
			this.CreateDbConnectSetting = new ReactiveCommand()
				.WithSubscribe(() => this.onCreateDbConnectSetting());
		}
	}
}
