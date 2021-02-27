using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Shellblade.Graphics
{
	public class Window
	{
		private readonly RenderWindow _window;

		public List<Drawable>                   Drawables      { get; } = new List<Drawable>();
		public Dictionary<uint, Action>         JoystickEvents { get; } = new Dictionary<uint, Action>();
		public Dictionary<Keyboard.Key, Action> KeyboardEvents { get; } = new Dictionary<Keyboard.Key, Action>();

		public Action<Time> LoopFunction { get; set; }

		public Window(Vector2u windowSize, Vector2u resolution, string title)
		{
			_window = new RenderWindow(new VideoMode(windowSize.X, windowSize.Y), title, Styles.Close | Styles.Titlebar);
			_window.SetFramerateLimit(60);

			_window.Closed += (sender, args) => _window.Close();
			_window.JoystickButtonPressed += (sender, args) =>
			{
				if (JoystickEvents.ContainsKey(args.Button)) JoystickEvents[args.Button]();
			};
			_window.KeyPressed += (sender, args) =>
			{
				if (KeyboardEvents.ContainsKey(args.Code)) KeyboardEvents[args.Code]();
			};

			var view = new View(_window.GetView())
			{
				Size   = (Vector2f)resolution,
				Center = (Vector2f)resolution / 2f,
			};
			_window.SetView(view);
		}

		public void MainLoop()
		{
			var clock = new Clock();

			while (_window.IsOpen)
			{
				Time dt = clock.Restart();

				_window.DispatchEvents();

				LoopFunction(dt);

				_window.Clear();

				foreach (Drawable drawable in Drawables) _window.Draw(drawable);

				_window.Display();
			}
		}
	}
}
