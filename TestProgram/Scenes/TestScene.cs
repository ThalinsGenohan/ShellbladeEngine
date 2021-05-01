using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Shellblade;
using Shellblade.Graphics;
using Shellblade.Graphics.UI;
using Image = Shellblade.Graphics.UI.Image;
using Text = Shellblade.Graphics.UI.Text;

namespace OtterRPG.Scenes
{
	public class TestScene : Scene
	{
		private readonly Textbox _textbox;

		public TestScene(Game game) : base(game)
		{
			Game = game;

			/*_textbox = new Textbox(new Vector2i(8,                             (int)(Game.Resolution.Y - 8 - 64)),
			                       new Vector2i((int)(Game.Resolution.X - 16), 64))
			{
				Text      = "{f:regular}Don't mind me, just testing UI things...",
				TextDelay = 25,
			};*/
			_textbox = new Textbox(new Vector2i(8, (int)(Game.Resolution.Y - 8 - 64)), new Vector2i((int)(Game.Resolution.X - 16), 64))
			{
				Text = "{f:regular}Hope this works now...",
			};

			var button = new Image(new Vector2i(16, 16), new Texture(@"assets\testbox.png"))
			{
				Color          = new Color(0xff, 0xff, 0xff, 0xff / 2),
				OnClick        = () => { Console.WriteLine("Boop! The test button has been pressed!"); },
				GlobalPosition = new Vector2i(8, 8),
			};
			button.OnMouseOver = () => { button.Color = new Color(0xff, 0xff, 0xff, 0xff); };
			button.OnMouseOff  = () => { button.Color = new Color(0xff, 0xff, 0xff, 0xff / 2); };

			var button2 = new Image(new Vector2i(16, 16), new Texture(@"assets\testbox2.png"))
			{
				Color          = new Color(0xff, 0xff, 0xff, 0xff / 2),
				OnClick        = () => { Console.WriteLine("Beep! The second test button has been pressed!"); },
				GlobalPosition = new Vector2i(12, 12),
			};
			button2.OnMouseOver = () => { button2.Color = new Color(0xff, 0xff, 0xff, 0xff); };
			button2.OnMouseOff  = () => { button2.Color = new Color(0xff, 0xff, 0xff, 0xff / 2); };

			Input.UI.AddElement("testButton",  button);
			Input.UI.AddElement("testButton2", button2);

			Drawables.Add("textbox", _textbox, 10);

			Input.Buttons.Add(new Input.ButtonInput(Keyboard.Key.Enter, 0) { OnPress = _textbox.Next });

			Game.ClearColor = Color.Blue;
		}

		public override void Loop(Time dt)
		{
			_textbox.UpdateScroll(dt.AsMilliseconds());
		}
	}
}
