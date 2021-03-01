using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Shellblade.Graphics
{
	public class Window
	{
		private readonly RenderWindow _window;

		public DrawableList                            Drawables      { get; } = new DrawableList();
		public Dictionary<uint, InputFunction>         JoystickEvents { get; } = new Dictionary<uint, InputFunction>();
		public Dictionary<Keyboard.Key, InputFunction> KeyboardEvents { get; } = new Dictionary<Keyboard.Key, InputFunction>();

		public Action<Time> LoopFunction { get; set; }

		public class InputFunction
		{
			public Action Action  { get; set; }
			public bool   Active  { get; set; } = false;
			public bool   Pressed { get; set; } = false;
			public bool   Hold    { get; }

			public InputFunction(Action action, bool hold = true)
			{
				Action = action;
				Hold   = hold;
			}
		}

		private View _view;

		public Window(Vector2u windowSize, Vector2u resolution, string title)
		{
			_window = new RenderWindow(new VideoMode(windowSize.X, windowSize.Y), title, Styles.Close | Styles.Titlebar);
			_window.SetFramerateLimit(60);

			_window.Closed += (sender, args) => _window.Close();
			_window.JoystickButtonPressed += (sender, args) =>
			{
				if (!JoystickEvents.ContainsKey(args.Button)) return;

				InputFunction e = JoystickEvents[args.Button];
				if (e.Pressed) return;

				e.Pressed = e.Active = true;
			};
			_window.KeyPressed += (sender, args) =>
			{
				if (!KeyboardEvents.ContainsKey(args.Code)) return;

				InputFunction e = KeyboardEvents[args.Code];
				if (e.Pressed) return;

				e.Pressed = e.Active = true;
			};
			_window.JoystickButtonReleased += (sender, args) =>
			{
				if (JoystickEvents.ContainsKey(args.Button))
					JoystickEvents[args.Button].Pressed = JoystickEvents[args.Button].Active = false;
			};
			_window.KeyReleased += (sender, args) =>
			{
				if (KeyboardEvents.ContainsKey(args.Code))
					KeyboardEvents[args.Code].Pressed = KeyboardEvents[args.Code].Active = false;
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

				foreach (InputFunction jsEvent in JoystickEvents.Values.Where(jsEvent => jsEvent.Active))
				{
					if (!jsEvent.Hold) jsEvent.Active = false;
					jsEvent.Action();
				}

				foreach (InputFunction kbEvent in KeyboardEvents.Values.Where(e => e.Active))
				{
					if (!kbEvent.Hold) kbEvent.Active = false;
					kbEvent.Action();
				}

				LoopFunction(dt);

				_window.Clear();

				for (var i = 0; i < Drawables.Count; i++) _window.Draw(Drawables[i].Drawable);

				_window.Display();
			}
		}
	}
}
