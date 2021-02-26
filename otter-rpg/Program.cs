using SFML.System;
using SFML.Window;
using Shellblade.Graphics;
using Window = Shellblade.Graphics.Window;

namespace OtterRPG
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			const uint ratioX = 4, ratioY = 3;
			const uint mult   = 80;

			var font5 = new Font(@"fonts\v5")
			{
				SpaceSize     = 2,
				VariableWidth = true,
			};
			var fontCT = new Font(@"fonts\vCT")
			{
				Size          = new Vector2i(8, 8),
				SpaceSize     = 2,
				VariableWidth = true,
			};

			var tb = new Textbox(new Vector2i(8, 8), new Vector2i(256, 64))
			{
				Kerning = 1,
				Font    = fontCT,
				Text = "Testing   testing, hello world! This should bleed onto the next two lines, I hope it works." +
				       "\nNow if I write a lot it should hopefully bleed all the way into another box, wow, so much text, I sure hope it does what it's supposed to do.",
			};

			var window = new Window(ratioX * mult, ratioY * mult, "Test");

			window.Drawables.Add(tb);
			window.KeyboardEvents.Add(Keyboard.Key.Enter, tb.Next);

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
