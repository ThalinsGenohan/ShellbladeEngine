using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
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

		private Button AttackButton { get; }
		private Button SkillsButton { get; }
		private Button MagicButton  { get; }
		private Button ItemButton  { get; }

		private Box Box { get; }


		public Battle(Game game) : base(game)
		{
			AttackButton = new Button(new Vector2i(16, 16), new Texture(@"assets\temp-icons.png", new IntRect(0, 0, 16, 16)))
			{
				Color         = new Color(0xffffff7f),
				OnClick       = () => { Console.WriteLine("Attack button clicked!"); },
				LocalPosition = new Vector2i(1, 1),
			};
			AttackButton.OnMouseOver = () => { AttackButton.Opacity = 1f; };
			AttackButton.OnMouseOff  = () => { AttackButton.Opacity = 0.5f; };

			SkillsButton = new Button(new Vector2i(16, 16), new Texture(@"assets\temp-icons.png", new IntRect(16, 0, 16, 16)))
			{
				Color         = new Color(0xffffff7f),
				OnClick       = () => { Console.WriteLine("Skills button clicked!"); },
				LocalPosition = new Vector2i(1, 18),
			};
			SkillsButton.OnMouseOver = () => { SkillsButton.Opacity = 1f; };
			SkillsButton.OnMouseOff  = () => { SkillsButton.Opacity = 0.5f; };

			MagicButton = new Button(new Vector2i(16, 16), new Texture(@"assets\temp-icons.png", new IntRect(32, 0, 16, 16)))
			{
				Color         = new Color(0xffffff7f),
				OnClick       = () => { Console.WriteLine("Magic button clicked!"); },
				LocalPosition = new Vector2i(1, 35),
			};
			MagicButton.OnMouseOver = () => { MagicButton.Opacity = 1f; };
			MagicButton.OnMouseOff  = () => { MagicButton.Opacity = 0.5f; };

			ItemButton = new Button(new Vector2i(16, 16), new Texture(@"assets\temp-icons.png", new IntRect(48, 0, 16, 16)))
			{
				Color         = new Color(0xffffff7f),
				OnClick       = () => { Console.WriteLine("Item button clicked!"); },
				LocalPosition = new Vector2i(1, 52),
			};
			ItemButton.OnMouseOver = () => { ItemButton.Opacity = 1f; };
			ItemButton.OnMouseOff  = () => { ItemButton.Opacity = 0.5f; };

			Box = new FilledBox
			{
				Color          = new Color(0x5C1AE1e5),
				GlobalPosition = new Vector2i(1,                          (int)Game.Resolution.Y - 70),
				Size           = new Vector2i((int)Game.Resolution.X - 2, 69),
			};

			Box.AddChild("attack", AttackButton);
			Box.AddChild("skills", SkillsButton);
			Box.AddChild("magic", MagicButton);
			Box.AddChild("item", ItemButton);

			var attackText = new Shellblade.Graphics.UI.Text
			{
				Color         = Color.White,
				LocalPosition = new Vector2i(18, 0),
				Instant       = true,
				String = "{f:tall}Attack",
			};
			var skillsText = new Shellblade.Graphics.UI.Text
			{
				Color         = Color.White,
				LocalPosition = new Vector2i(18, 0),
				Instant       = true,
				String        = "{f:tall}Skills",
			};
			var magicText = new Shellblade.Graphics.UI.Text
			{
				Color         = Color.White,
				LocalPosition = new Vector2i(18, 0),
				Instant       = true,
				String        = "{f:tall}Magic",
			};
			var itemText = new Shellblade.Graphics.UI.Text
			{
				Color         = Color.White,
				LocalPosition = new Vector2i(18, 0),
				Instant       = true,
				String        = "{f:tall}Item",
			};

			AttackButton.AddChild("text", attackText);
			SkillsButton.AddChild("text", skillsText);
			MagicButton.AddChild("text", magicText);
			ItemButton.AddChild("text", itemText);

			Input.Buttons = new List<Input.ButtonInput>
			{
				new Input.ButtonInput(Keyboard.Key.Enter,  0) { OnPress = Confirm },
				new Input.ButtonInput(Keyboard.Key.RShift, 1) { OnPress = Cancel },
			};
			Input.UI = new UIContainer
			{
				Elements = new Dictionary<string, UIElement>
				{
					{ "box", Box },
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

		public override void Loop(Time dt)
		{

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
