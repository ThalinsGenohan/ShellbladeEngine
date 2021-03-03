using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Shellblade.Graphics;
using Font = Shellblade.Graphics.Font;
using Window = Shellblade.Graphics.Window;

namespace OtterRPG
{
	internal class Program
	{
		private const uint ResWidth    = 320;
		private const uint ResHeight   = 240;
		private const uint WindowScale = 4;

		public static List<Font> Fonts = new List<Font>
		{
			new Font(@"fonts\vWM")
			{
				Size           = new Vector2i(8, 8),
				SpaceSize      = 3,
				VariableWidth  = true,
				TrackingOffset = -1,
			},
			new Font(@"fonts\vCT")
			{
				Size          = new Vector2i(8, 8),
				SpaceSize     = 4,
				VariableWidth = true,
			},
			new Font(@"fonts\vLA")
			{
				Size           = new Vector2i(8, 8),
				SpaceSize      = 3,
				VariableWidth  = true,
				TrackingOffset = -1,
			},
		};

		private static int _font = 0;

		public static Font CurrentFont => Fonts[_font];

		public static Font ChangeFont(bool forward)
		{
			if (forward)
			{
				_font++;
				if (_font >= Fonts.Count) _font = 0;
			}
			else
			{
				if (_font <= 0) _font = Fonts.Count;
				_font--;
			}

			return CurrentFont;
		}

		public static void M7Translate(Mode7 m7, int x, int y)
		{
			m7.Translate(x, y);
		}

		private static void Main(string[] args)
		{
			var playerName = "Sei";

			Textbox.Strings["player.name"] = () => playerName;

			playerName = "Thalins";

			var tb = new Textbox(new Vector2i(8, (int)(ResHeight - 8 - 64)), new Vector2i((int)(ResWidth - 16), 64))
			{
				Tracking = 1,
				Fonts = new Dictionary<string, Font>
				{
					{ "regular", Fonts[0] },
					{ "bold", Fonts[1] },
					{ "italic", Fonts[2] },
				},
				Text = "Hello! Look at my textboxes! They can do neat things. Like automatically go to the next line! Or even the next textbox! Wow! That's kinda cool! They can also do {c:red}f{c:yellow}a{c:green}n{c:blue}c{c:magenta}y {reset}colors! Neat!\f" +
					   "Oh and did I mention multiple font options? Right now I have PMD (normal), {f:bold}Chrono Trigger (bold){f:regular}, and {f:italic}Link's Awakening (italics){f:regular}. And I just added the ability to switch between those mid-textbox! It's not much, but I think they're neat and I'm proud of them.",
				TextDelay = 25,
			};

			var m7 = new Mode7
			{
				Resolution = new Vector2u(ResWidth, ResHeight),
				/*Scroll     = new Vector2u(0,            0),
				Center     = new Vector2u(ResWidth / 2, ResHeight / 2),
				Scale      = new Vector2f(1f, 1f),
				Rotation   = 0.0,
				Skew       = new Vector2f(5f, 0f),*/
				FromImage = new Image(@"P:\CS\otter-rpg\otter-rpg-engine\Graphics\alttp.png"),
			};
			m7.Translate(0, 1);

			var window = new Window(new Vector2u(ResWidth * WindowScale, ResHeight * WindowScale), new Vector2u(ResWidth, ResHeight), "Test");

			const float rotSpeed  = 5f;
			const int   moveSpeed = 2;

			window.Drawables.Add("textbox", tb, 10, false);
			window.Drawables.Add("mode7",   m7, 0, true);
			window.KeyboardEvents.Add(Keyboard.Key.Enter, new Window.InputFunction(tb.Next));
			window.KeyboardEvents.Add(Keyboard.Key.W,     new Window.InputFunction(onHold: () => { M7Translate(m7, 0, -moveSpeed); }));
			window.KeyboardEvents.Add(Keyboard.Key.A,     new Window.InputFunction(onHold: () => { M7Translate(m7, -moveSpeed, 0); }));
			window.KeyboardEvents.Add(Keyboard.Key.S,     new Window.InputFunction(onHold: () => { M7Translate(m7, 0, moveSpeed); }));
			window.KeyboardEvents.Add(Keyboard.Key.D,     new Window.InputFunction(onHold: () => { M7Translate(m7, moveSpeed, 0); }));

			window.JoystickEvents.Add(0, new Window.InputFunction(tb.Next));

			window.LoopFunction = dt => { tb.UpdateScroll(dt.AsMilliseconds()); };
			window.MainLoop();

			/*var p1 = new Player("Thalins", 1, 1, 1, 1, 1, 1);

			p1.Health = p1.MaxHealth = 100;

			Console.WriteLine($"{p1.Name} Lv {p1.Level}\n" +
			                  $"- STR: {p1.Strength.Total}\n" +
			                  $"- INT: {p1.Intellect.Total}\n" +
			                  $"- AGI: {p1.Agility.Total}\n" +
			                  $"- VIT: {p1.Vitality.Total}\n" +
			                  $"- FOR: {p1.Fortitude.Total}\n");

			var samName = "Samurai";
			var samDesc = "A powerful warrior who can overpower their enemies without the need for a shield.";
			var samStats = new Dictionary<Character.StatWeight, float>
			{
				{ Character.StatWeight.Health, 1.2f },
				{ Character.StatWeight.Strength, 2f },
				{ Character.StatWeight.Intellect, 0.9f },
				{ Character.StatWeight.Agility, 0.9f },
				{ Character.StatWeight.Vitality, 1.1f },
				{ Character.StatWeight.Fortitude, 1f },
			};
			var samWeapons = new List<Weapon.WeaponType>
			{
				Weapon.WeaponType.Broadsword,
				Weapon.WeaponType.Greatsword,
				Weapon.WeaponType.Bow,
			};
			var samurai = new Job(samName, samDesc, samStats, samWeapons);

			var dict = new Dictionary<string, string>
			{
				{ "jobSamuraiName", "Samurai" },
				{ "jobSamuraiDesc", "A powerful warrior who can overpower their enemies without the need for a shield." },
			};

			var yaml = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
			File.WriteAllText("lang/en-US.yaml", yaml.Serialize(dict));*/
		}
	}
}
