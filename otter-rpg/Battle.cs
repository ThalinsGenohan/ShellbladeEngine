using System.Linq;

namespace OtterRPG
{
	internal class Battle
	{
		public Player[]    Players { get; set; }
		public Character[] Enemies { get; set; }

		private uint      TurnCounter   { get; set; }
		public  Character CurrentTurn   { get; set; }
		public  Attack    CurrentAttack { get; set; }

		public void Start(Player[] players)
		{
			Players = players;

			foreach (var player in Players)
			{
				player.Actions         = 0;
				player.Strength.Buffs  = 1f;
				player.Intellect.Buffs = 1f;
				player.Agility.Buffs   = 1f;
				player.Vitality.Buffs  = 1f;
				player.Fortitude.Buffs = 1f;
			}

			foreach (var character in Enemies)
			{
				character.Actions         = 0;
				character.Strength.Buffs  = 1f;
				character.Intellect.Buffs = 1f;
				character.Agility.Buffs   = 1f;
				character.Vitality.Buffs  = 1f;
				character.Fortitude.Buffs = 1f;
			}

			TurnCounter   = 0;
			CurrentTurn   = Players[0];
			CurrentAttack = new Attack();

			while (CheckPlayersAlive() && CheckEnemiesAlive()) TurnCounter++;
		}

		private bool CheckPlayersAlive()
		{
			return Players.Any(player => player.Health > 0);
		}

		private bool CheckEnemiesAlive()
		{
			return Enemies.Any(enemy => enemy.Health > 0);
		}

		public class Attack
		{
			public string  Name       { get; set; }
			public int     Damage     { get; set; }
			public Element Element    { get; set; }
			public uint    ActionCost { get; set; }

			// Effects

			public Character Source { get; set; }
			public Character Target { get; set; }
		}
	}
}
