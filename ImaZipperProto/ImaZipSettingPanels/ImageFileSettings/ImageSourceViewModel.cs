using MaterialDesignThemes.Wpf;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace HalationGhost.WinApps.ImaZip.ImageFileSettings
{
	public class ImageSourceViewModel : HalationGhostViewModelBase
	{
		#region プロパティ

		public ReadOnlyReactivePropertySlim<string> Path { get; }

		public ReactivePropertySlim<PackIconKind> ItemIcon { get; }

		public ReactivePropertySlim<bool> IsSelected { get; set; }

		#endregion

		#region コンストラクタ

		public ImageSourceViewModel(ImageSource source)
		{
			this.Path = source.Path
				.ToReadOnlyReactivePropertySlim()
				.AddTo(this.disposable);
			this.ItemIcon = new ReactivePropertySlim<PackIconKind>(this.getIcon(source))
				.AddTo(this.disposable);

			this.IsSelected = new ReactivePropertySlim<bool>(false)
				.AddTo(this.disposable);
		}

		private PackIconKind getIcon(ImageSource source)
		{
			var iconKind = PackIconKind.Archive;

			if (source.SourceKind.Value == ImageSourceType.Folder)
				iconKind = PackIconKind.Folder;

			return iconKind;
		}

		#endregion
	}
}
