namespace OtterRPG
{
	internal class Player : Character
	{
		public int Experience { get; set; }

		public Player() { }

		public Player(string name, uint level, uint str, uint inte, uint agi, uint vit, uint fort)
			: base(name, level, str, inte, agi, vit, fort) { }
	}
}
