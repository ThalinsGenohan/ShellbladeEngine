namespace OtterRPG
{
	internal class Player : Character
	{
		public Player()
		{
		}

		public Player(string name, uint level, uint str, uint inte, uint agi, uint vit, uint fort)
			: base(name, level, str, inte, agi, vit, fort)
		{
		}

		public int Experience { get; set; }
	}
}
