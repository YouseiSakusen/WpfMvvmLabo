using System;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace HalationGhost.WinApps.Behaviors
{
	/// <summary>
	/// DataContextをDisposeするTriggerActionを表します。
	/// </summary>
	public class DataContextDisposeAction : TriggerAction<FrameworkElement>
	{
		/// <summary>
		/// Actionを実行します。
		/// </summary>
		/// <param name="parameter">パラメータを表すobject。</param>
		protected override void Invoke(object parameter)
		{
			var disposable = this.AssociatedObject?.DataContext as IDisposable;
			disposable?.Dispose();
		}
	}
}
