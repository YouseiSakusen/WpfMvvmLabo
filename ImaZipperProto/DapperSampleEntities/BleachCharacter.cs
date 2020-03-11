using HalationGhost;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace DapperSample
{
	public class BleachCharacter : BindableModelBase
	{
		private int id = 0;
		public int Id
		{
			get { return id; }
			set { SetProperty(ref id, value); }
		}

		private string name = string.Empty;
		public string Name
		{
			get { return name; }
			set { SetProperty(ref name, value); }
		}

		private string kana = string.Empty;
		public string Kana
		{
			get { return kana; }
			set { SetProperty(ref kana, value); }
		}

		private string birthday = string.Empty;
		public string Birthday
		{
			get { return birthday; }
			set { SetProperty(ref birthday, value); }
		}

		private int orgId = 0;
		public int OrganizationId
		{
			get { return orgId; }
			set { SetProperty(ref orgId, value); }
		}

		private string orgName = string.Empty;
		public string OrganizationName
		{
			get { return orgName; }
			set { SetProperty(ref orgName, value); }
		}

		private int zanpakuId = 0;
		public int ZanpakutouId
		{
			get { return zanpakuId; }
			set { SetProperty(ref zanpakuId, value); }
		}

		private string zanpakuSword = string.Empty;
		public string ZanpakutouName
		{
			get { return zanpakuSword; }
			set { SetProperty(ref zanpakuSword, value); }
		}

		//public ReactivePropertySlim<int> Id { get; set; }

		//public ReactivePropertySlim<string> Name { get; set; }

		//public ReactivePropertySlim<string> Furigana { get; set; }

		//public ReactivePropertySlim<string> Birthday { get; set; }

		//public ReactivePropertySlim<int> OrganizationId { get; set;	 }

		//public ReactivePropertySlim<string> OrganizationName { get; set; }

		//public ReactivePropertySlim<int> ZanpakutouId { get; set; }

		//public ReactivePropertySlim<string> ZanpakutouName { get; set; }

		//public BleachCharacter()
		//{
		//	this.Id = new ReactivePropertySlim<int>(0)
		//		.AddTo(this.Disposable);
		//	this.Name = new ReactivePropertySlim<string>(string.Empty)
		//		.AddTo(this.Disposable);
		//	this.Furigana = new ReactivePropertySlim<string>(string.Empty)
		//		.AddTo(this.Disposable);
		//	this.Birthday = new ReactivePropertySlim<string>(string.Empty)
		//		.AddTo(this.Disposable);
		//	this.OrganizationId = new ReactivePropertySlim<int>(0)
		//		.AddTo(this.Disposable);
		//	this.OrganizationName = new ReactivePropertySlim<string>(string.Empty)
		//		.AddTo(this.Disposable);
		//	this.ZanpakutouId = new ReactivePropertySlim<int>(0)
		//		.AddTo(this.Disposable);
		//	this.ZanpakutouName = new ReactivePropertySlim<string>(string.Empty)
		//		.AddTo(this.Disposable);
		//}
	}
}
