using System;
using System.Collections.Generic;
using System.Text;

namespace DapperSample
{
	public interface IRepositoryFactory
	{
		public ICharacterRepository CreateCharacterRepository();
	}
}
