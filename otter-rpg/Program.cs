using SFML.Graphics;
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
				Tracking = 0,
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
		}
	}
}
