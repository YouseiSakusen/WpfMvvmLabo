using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using HalationGhost.WinApps.ImaZip.AppSettings;
using HalationGhost.WinApps.Services.CommonDialogs;
using HalationGhost.WinApps.Services.CommonDialogs.DialogSettings;
using Prism.Services.Dialogs;
using Prism.Services.Dialogs.Extensions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace HalationGhost.WinApps.ImaZip.ImageFileSettings
{
	/// <summary>
	/// ソースファイルリストを追加するViewを表します。
	/// </summary>
	public class ZipFileListPanelViewModel : HalationGhostViewModelBase
	{
		#region プロパティ

		public ReadOnlyReactiveCollection<ImageSourceViewModel> ImageSources { get; }

		public ReactivePropertySlim<string> ImageFilesExtractedFolderPath { get; set; }

		public ReadOnlyReactiveCollection<int> FolderNameSequenceDigits { get; }

		public ReactiveProperty<int?> FolderNameSequenceDigit { get; set; }

		public ReactiveProperty<string> FolderNameTemplate { get; set; }

		public ReactiveProperty<string> FileNameTemplate { get; set; }

		#endregion

		#region コマンド

		#region イメージソースを追加

		public ReactiveCommand<string> AddImageSource { get; }

		private void onAddImageSource(string param)
		{
			var settings = this.createCommonDialogSettings(param);

			if (this.commonDialogService.ShowDialog(settings))
			{
				switch (param)
				{
					case "Archives":
						var openFileSetting = settings as OpenFileDialogSettings;
						this.zipSettings.MergeToViewModels(openFileSetting.FileNames, ImageSourceType.File);
						break;
					case "Folders":
						break;
				}
			}
		}

		private CommonDialogSettingBase createCommonDialogSettings(string param)
		{
			switch (param)
			{
				case "Archives":
					return new OpenFileDialogSettings()
					{
						Filter = this.appSettings.SourceFileSelectedFilter,
						InitialDirectory = this.appSettings.SourceFileInitialDirectoryPath,
						Multiselect = true
					};
				case "Folders":
					return new WinApiFolderPickerDialogSettings();
			}

			return null;
		}

		#endregion

		public ReactiveCommand<SelectionChangedEventArgs> SelectionChanged { get; }

		private void onSelectionChanged(SelectionChangedEventArgs e)
		{
			var selItems = this.ImageSources.Where(s => s.IsSelected.Value == true).ToList();
			if (selItems.Count == 0)
			{
				this.deleteEnabled.Value = false;
				this.moveUpEnabled.Value = false;
				this.moveDownEnabled.Value = false;

				return;
			}

			this.deleteEnabled.Value = true;

			if (this.ImageSources.Count == 0)
			{
				this.moveUpEnabled.Value = false;
				this.moveDownEnabled.Value = false;

				return;
			}

			this.moveUpEnabled.Value = this.ImageSources.First().Path.Value != selItems.First().Path.Value;
			this.moveDownEnabled.Value = this.ImageSources.Last().Path.Value != selItems.Last().Path.Value;
		}

		public ReactiveCommand DeleteSource { get; }

		private void onDeleteSource()
		{

		}

		public ReactiveCommand MoveUp { get; }

		private void onMoveUp()
		{

		}

		public ReactiveCommand MoveDown { get; }

		private void onMoveDown()
		{

		}

		public ReactiveCommand SelectExtractFolder { get; }

		private void onSelectExtractFolder()
		{
			var settings = new WinApiFolderPickerDialogSettings();

			if (this.commonDialogService.ShowDialog(settings))
				this.zipSettings.ImageFilesExtractedFolder.Value = settings.FolderPath;
		}

		public AsyncReactiveCommand CreateZip { get; }

		private Task onCreateZip()
		{
			if (!Directory.Exists(this.ImageFilesExtractedFolderPath.Value))
			{
				this.dialogService.ShowNotify("アーカイブファイルの解凍先フォルダが見つかりません。", "エラー");

				return Task.CompletedTask;
			}

			return Task.Run(async () => await new ImageSourceAgent().StartCreateZipAsync(this.zipSettings, this.appSettings));
		}

		#endregion

		#region privateプロパティ

		private ReactivePropertySlim<bool> deleteEnabled { get; set; }

		private ReactivePropertySlim<bool> moveUpEnabled { get; set; }

		private ReactivePropertySlim<bool> moveDownEnabled { get; set; }

		#endregion

		#region コンストラクタ

		/// <summary>
		/// アプリケーション設定を表します。
		/// </summary>
		private IImaZipCoreProto01Settings appSettings = null;
		/// <summary>
		/// コモンダイアログサービスを表します。
		/// </summary>
		private ICommonDialogService commonDialogService = null;
		/// <summary>
		/// ダイアログサービスを表します。
		/// </summary>
		private IDialogService dialogService = null;

		private ZipFileSettings zipSettings = new ZipFileSettings();

		private ObservableCollection<ImageSource> imgSrcList = new ObservableCollection<ImageSource>();

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="comDlgService">コモンダイアログサービスを表すICommonDialogService。</param>
		/// <param name="imaZipSettings">アプリケーション設定を表すIImaZipCoreProto01Settings。</param>
		public ZipFileListPanelViewModel(ICommonDialogService comDlgService,
								   IImaZipCoreProto01Settings imaZipSettings,
								   IDialogService dlgService)
		{
			this.commonDialogService = comDlgService;
			this.appSettings = imaZipSettings;
			this.dialogService = dlgService;

			this.ImageSources = this.zipSettings.ImageSources
				.ToReadOnlyReactiveCollection(i => new ImageSourceViewModel(i))
				.AddTo(this.disposable);
			this.ImageFilesExtractedFolderPath = this.zipSettings.ImageFilesExtractedFolder
				.AddTo(this.disposable);

			this.deleteEnabled = new ReactivePropertySlim<bool>(false)
				.AddTo(this.disposable);
			this.moveUpEnabled = new ReactivePropertySlim<bool>(false)
				.AddTo(this.disposable);
			this.moveDownEnabled = new ReactivePropertySlim<bool>(false)
				.AddTo(this.disposable);

			this.SelectionChanged = new ReactiveCommand<SelectionChangedEventArgs>()
				.WithSubscribe(e => this.onSelectionChanged(e))
				.AddTo(this.disposable);

			this.AddImageSource = new ReactiveCommand<string>()
				.WithSubscribe(p => this.onAddImageSource(p))
				.AddTo(this.disposable);

			this.DeleteSource = this.deleteEnabled
				.ToReactiveCommand()
				.WithSubscribe(() => this.onDeleteSource())
				.AddTo(this.disposable);
			this.MoveUp = this.moveUpEnabled
				.ToReactiveCommand()
				.WithSubscribe(() => this.onMoveUp())
				.AddTo(this.disposable);
			this.MoveDown = this.moveDownEnabled
				.ToReactiveCommand()
				.WithSubscribe(() => this.onMoveDown())
				.AddTo(this.disposable);

			this.SelectExtractFolder = new ReactiveCommand()
				.WithSubscribe(() => this.onSelectExtractFolder())
				.AddTo(this.disposable);

			this.CreateZip = this.zipSettings.IsComplete
				.ToAsyncReactiveCommand()
				.WithSubscribe(() => this.onCreateZip())
				.AddTo(this.disposable);

			var folderSequences = new ObservableCollection<int>(Enumerable.Range(1, 4));
			this.FolderNameSequenceDigits = folderSequences
				.ToReadOnlyReactiveCollection(i => i)
				.AddTo(this.disposable);
			this.FolderNameSequenceDigit = this.zipSettings.FolderNameSequenceDigit
				.ToReactiveProperty()
				.AddTo(this.disposable);
			this.FolderNameTemplate = this.zipSettings.FolderNameTemplate
				.ToReactiveProperty()
				.AddTo(this.disposable);
		}

		#endregion
	}
}
