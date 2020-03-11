using System.Text;
using HalationGhost;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace DapperSample
{
	public class ConsoleBuffer : BindableModelBase
	{
		public ReactivePropertySlim<string> ConsoleText { get; set; }

		public void AppendLineToBuffer(string text)
		{
			var buf = new StringBuilder(this.ConsoleText.Value);
			buf.AppendLine(text);

			this.ConsoleText.Value = buf.ToString();
		}

		public ConsoleBuffer()
		{
			this.ConsoleText = new ReactivePropertySlim<string>(string.Empty)
				.AddTo(this.Disposable);
		}
	}
}
