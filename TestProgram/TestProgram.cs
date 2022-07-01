using System.IO;
using OtterRPG.Scenes;
using SFML.Audio;
using SFML.System;
using Shellblade;
using Shellblade.Graphics;
using Shellblade.Graphics.UI;

namespace OtterRPG
{
	internal class TestProgram
	{
#if DEBUG
		public const bool Debug = true;
#else
		public const bool Debug = false;
#endif

		private const uint   ResWidth    = 432;
		private const uint   ResHeight   = 240;
		private const uint   WindowScale = 4;
		private const string FontsDir    = "assets/fonts/";
		private const string VoicesDir    = "assets/sound/voice/";

		private static uint WindowWidth  => ResWidth * WindowScale;
		private static uint WindowHeight => ResHeight * WindowScale;

		private static void Main()
		{
			string[] fontFiles = Directory.GetFiles(FontsDir, "*.png");
			foreach (string file in fontFiles)
			{
				string nameDir = file.Replace(".png", "");
				Text.Fonts.Add(nameDir.Replace(FontsDir, ""), new Font(nameDir));
			}

			Text.Voices.Add("silent", null);
			string[] voiceFiles = Directory.GetFiles(VoicesDir, "*.ogg");
			foreach (string file in voiceFiles)
			{
				string nameDir = file.Replace(".ogg", "");
				Text.Voices.Add(nameDir.Replace(VoicesDir, ""), new SoundBuffer(file));
			}


			Game.Initialize(new Vector2u(WindowWidth, WindowHeight), new Vector2u(ResWidth, ResHeight), "Test", Debug);
			Game.LoadScene(new TestScene());
			Game.Run();
		}
	}
}
