using System.IO;
using OtterRPG.Scenes;
using Shellblade.Graphics;
using Shellblade.Graphics.UI;

namespace OtterRPG
{
	internal class Program
	{
		private const uint   ResWidth    = 320;
		private const uint   ResHeight   = 240;
		private const uint   WindowScale = 4;
		private const string fontsDir    = "fonts/";

		private static uint WindowWidth  => ResWidth * WindowScale;
		private static uint WindowHeight => ResHeight * WindowScale;

		private static void Main(string[] args)
		{
			string[] fontFiles = Directory.GetFiles(fontsDir, "*.png");
			foreach (string file in fontFiles)
			{
				string nameDir = file.Replace(".png", "");
				Text.Fonts.Add(nameDir.Replace(fontsDir, ""), new Font(nameDir));
			}

			var game = new Game(WindowWidth, WindowHeight, ResWidth, ResHeight, "Test");
			game.LoadScene(new TestScene(game));

			game.Run();
		}
	}
}
