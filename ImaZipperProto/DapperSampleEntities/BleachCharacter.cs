using HalationGhost;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace DapperSample
{
	/// <summary>BLEACHのキャラクターを表します。</summary>
	public class BleachCharacter : BindableModelBase
	{
		//public long Id { get; set; } = 0;

		//public string Name { get; set; } = string.Empty;

		//public string Furigana { get; set; } = string.Empty;

		//public string Birthday { get; set; } = string.Empty;

		//public long OrganizationId { get; set; } = 0;

		//public string OrganizationName { get; set; } = string.Empty;

		//public long ZanpakutouId { get; set; } = 0;

		//public string ZanpakutouName { get; set; } = string.Empty;

		private ReactivePropertySlim<long> _id;

		/// <summary>キャラクターIDを取得・設定します。</summary>
		public ReactivePropertySlim<long> Id
		{
			get => this._id;
			set
			{
				this._id?.Dispose();
				this._id = value;
			}
		}

		//public ReactivePropertySlim<long> Id { get; }

		private ReactivePropertySlim<string> _name;

		/// <summary>キャラクター名を取得・設定します。</summary>
		public ReactivePropertySlim<string> Name
		{
			get => this._name;
			set
			{
				this._name?.Dispose();
				this._name = value;
			}
		}


		//public ReactivePropertySlim<string> Name { get; }

		public ReactivePropertySlim<string> Furigana { get; set; }

		public ReactivePropertySlim<string> Birthday { get; set; }

		public ReactivePropertySlim<long> OrganizationId { get; set; }

		public ReactivePropertySlim<string> OrganizationName { get; set; }

		public ReactivePropertySlim<long> ZanpakutouId { get; set; }

		public ReactivePropertySlim<string> ZanpakutouName { get; set; }

		public ReactivePropertySlim<string> BankaiName { get; set; }

		/// <summary>コンストラクタ。</summary>
		public BleachCharacter()
		{
			this._id = new ReactivePropertySlim<long>(0);
			this._name = new ReactivePropertySlim<string>(string.Empty);
			this.Furigana = new ReactivePropertySlim<string>(string.Empty);
			this.Birthday = new ReactivePropertySlim<string>(string.Empty);
			this.OrganizationId = new ReactivePropertySlim<long>(0);
			this.OrganizationName = new ReactivePropertySlim<string>(string.Empty);
			this.ZanpakutouId = new ReactivePropertySlim<long>(0);
			this.ZanpakutouName = new ReactivePropertySlim<string>(string.Empty);
			this.BankaiName = new ReactivePropertySlim<string>(string.Empty);
		}
	}
}
