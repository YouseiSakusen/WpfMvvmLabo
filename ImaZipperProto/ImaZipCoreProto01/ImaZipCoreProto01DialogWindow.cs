using System;
using System.Collections.Generic;
using System.Text;
using MahApps.Metro.Controls;
using Prism.Services.Dialogs;

namespace HalationGhost.WinApps.ImaZip
{
	public partial class ImaZipCoreProto01DialogWindow : MetroWindow, IDialogWindow
	{
		public IDialogResult Result { get; set; }

		public ImaZipCoreProto01DialogWindow()
		{
			this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

			this.Loaded += this.ImaZipCoreProto01DialogWindow_Loaded;
		}

		private void ImaZipCoreProto01DialogWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			this.Loaded -= this.ImaZipCoreProto01DialogWindow_Loaded;

			if ((this.DataContext != null) && (this.DataContext is IDialogAware))
				this.Title = (this.DataContext as IDialogAware).Title;
		}
	}
}
