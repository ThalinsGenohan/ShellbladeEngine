using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Shellblade;
using Shellblade.Graphics;
using Image = Shellblade.Graphics.UI.Image;

namespace OtterRPG.Scenes
{
	public class TestScene : Scene
	{
		private readonly Textbox _textbox;

		public TestScene()
		{
			_textbox = new Textbox(new Vector2i(8,                             (int)(Game.Resolution.Y - 8 - 64)),
			                       new Vector2i((int)(Game.Resolution.X - 16), 64))
			{
				Text      = "{v:default}This text should be in the default voice.\n{d:500}" +
				            "{v:high}This one should be in the high voice.\n{d:500}" +
				            "{v:low}And finally it should be low here.",
				TextDelay = 25,
			};

			Drawables.Add("textbox", _textbox, 10);

			Input.Buttons.Add(new Input.ButtonInput(Keyboard.Key.Enter, 0) { OnPress = _textbox.Next });

			Game.ClearColor = Color.Blue;


			Tileset tileset = Tileset.LoadFromFile("assets/tilesets/tileset.tsj");
			Console.WriteLine(tileset);
		}

		public override void Loop(Time dt)
		{
			//_textbox.UpdateScroll(dt.AsMilliseconds());
		}
	}
}
