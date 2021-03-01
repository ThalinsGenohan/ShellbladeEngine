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

		public DrawableList                     Drawables      { get; } = new DrawableList();
		public Dictionary<uint, Action>         JoystickEvents { get; } = new Dictionary<uint, Action>();
		public Dictionary<Keyboard.Key, Action> KeyboardEvents { get; } = new Dictionary<Keyboard.Key, Action>();

		public Action<Time> LoopFunction { get; set; }

		private View _view;

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

			_view = new View(_window.GetView())
			{
				Size   = (Vector2f)resolution,
				Center = (Vector2f)resolution / 2f,
				Rotation = 0f,
			};
			_window.SetView(_view);
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

				for (var i = 0; i < Drawables.Count; i++) _window.Draw(Drawables[i].Drawable);

				_window.Display();
			}
		}
	}
}
