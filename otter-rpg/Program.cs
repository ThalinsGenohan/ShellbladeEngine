using System.Collections.Generic;
using SFML.System;
using SFML.Window;
using Shellblade.Graphics;
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
			new Font(@"fonts\vCT")
			{
				Size          = new Vector2i(8, 8),
				SpaceSize     = 4,
				VariableWidth = true,
			},
			new Font(@"fonts\vWM")
			{
				Size           = new Vector2i(8, 8),
				SpaceSize      = 3,
				VariableWidth  = true,
				TrackingOffset = -1,
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

		private static void Main(string[] args)
		{
			var playerName = "Sei";

			Textbox.Strings["player.name"] = () => playerName;

			playerName = "Thalins";

			var tb = new Textbox(new Vector2i(8, 8), new Vector2i(256, 64))
			{
				Tracking  = 1,
				Font      = CurrentFont,
				Text      = "Hello world! {alpha:50}OoOoOoO, sPoOoOoKy GhOoOoOsT tEeEeExT",
				TextDelay = 50,
			};

			var window = new Window(new Vector2u(ResWidth * WindowScale, ResHeight * WindowScale), new Vector2u(ResWidth, ResHeight), "Test");

			window.Drawables.Add("textbox", tb, 10);
			window.KeyboardEvents.Add(Keyboard.Key.Enter, tb.Next);
			window.KeyboardEvents.Add(Keyboard.Key.Left,  () => { tb.ChangeFont(ChangeFont(false)); });
			window.KeyboardEvents.Add(Keyboard.Key.Right, () => { tb.ChangeFont(ChangeFont(true)); });

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
