using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Shellblade;
using Shellblade.Graphics;
using Shellblade.Graphics.UI;

namespace OtterRPG.Scenes
{
	public class TestScene : Scene
	{
		private readonly Textbox _textbox;

		public TestScene(Game game) : base(game)
		{
			Game = game;

			_textbox = new Textbox(new Vector2i(8, (int)(Game.Resolution.Y - 8 - 64)), new Vector2i((int)(Game.Resolution.X - 16), 64))
			{
				Text = "{f:regular}Hello this is a test of the small font!\n" +
				       "{f:tall}This is a test of the tall font {f:tiny}jk it's the tiny one\n" +
				       "{f:italic}And now a small one on its own line.\n" +
				       "{f:bold}And one more for good measure.\f" +
				       "{f:tiny}Little tiny baby text, look how many things I can write in this textbox, wow, are we even on the next line yet, probably, but I don't know until I run it",
				TextDelay = 25,
			};

			var button = new Button(new Vector2i(16, 16), new Texture(@"assets\testbox.png"))
			{
				Color   = new Color(0xff, 0xff, 0xff, 0xff / 2),
				OnClick = () => { Console.WriteLine("Boop! The test button has been pressed!"); },
			};
			button.OnMouseOver = () => { button.Color = new Color(0xff, 0xff, 0xff, 0xff); };
			button.OnMouseOff  = () => { button.Color = new Color(0xff, 0xff, 0xff, 0xff / 2); };
			Input.UI.Elements.Add("testButton", button);

			Drawables.Add("textbox", _textbox, 10);
			Drawables.Add("button",  button,   10);

			Input.Buttons.Add(new Input.ButtonInput(Keyboard.Key.Enter, 0) { OnPress = _textbox.Next });

			Game.ClearColor   = Color.Blue;
		}

		public override void Loop(Time dt)
		{
			_textbox.UpdateScroll(dt.AsMilliseconds());
		}
	}
}
