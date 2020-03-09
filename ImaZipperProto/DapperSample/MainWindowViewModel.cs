using HalationGhost.WinApps;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DapperSample
{
	public class MainWindowViewModel : HalationGhostViewModelBase
	{
		public ReactiveCommand GetDynamic { get; }

		public MainWindowViewModel()
		{
			
		}
	}
}
