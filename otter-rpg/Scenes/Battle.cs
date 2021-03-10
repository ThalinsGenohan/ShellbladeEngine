using System;
using System.Collections.Generic;
using System.Linq;
using SFML.System;
using SFML.Window;
using Shellblade;
using Shellblade.Graphics;
using Shellblade.Graphics.UI;

namespace OtterRPG.Scenes
{
	internal class Battle : Scene
	{
		private uint TurnCounter;

		public Player[]    Players       { get; set; }
		public Character[] Enemies       { get; set; }
		public Character   CurrentTurn   { get; set; }
		public Attack      CurrentAttack { get; set; }

		public Battle(Game game) : base(game)
		{
			Input = new Input
			{
				Buttons = new List<Input.ButtonInput>
				{
					new Input.ButtonInput(Keyboard.Key.Enter,  0) { OnPress = Confirm },
					new Input.ButtonInput(Keyboard.Key.RShift, 1) { OnPress = Cancel },
				},
				UI = new UIContainer
				{
					Elements = new Dictionary<string, UIElement>
					{
						{
							"char0.attack", new Button(new Vector2i(0, 0), new Vector2i(0, 0), "")
							{
								Position = new Vector2i(1,  1),
								Size     = new Vector2i(50, 10),
							}
						},
					},
				},
			};
		}

		public void Start(Player[] players)
		{
			Players = players;

			foreach (Player player in Players)
			{
				player.Actions         = 0;
				player.Strength.Buffs  = 1f;
				player.Intellect.Buffs = 1f;
				player.Agility.Buffs   = 1f;
				player.Vitality.Buffs  = 1f;
				player.Fortitude.Buffs = 1f;
			}

			foreach (Character character in Enemies)
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

		protected override void Loop(Time dt)
		{
			throw new NotImplementedException();
		}

		private void Confirm() { }

		private void Cancel() { }

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
