namespace DapperSample
{
	public class RepositoryFactory : IRepositoryFactory
	{
		public ICharacterRepository CreateCharacterRepository()
		{
			return new CharacterRepository();
		}
	}
}
