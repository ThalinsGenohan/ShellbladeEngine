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
		internal RenderWindow RenderWindow { get; set; }

		public DrawableList                            Drawables      { get; } = new DrawableList();
		public Dictionary<uint, InputFunction>         JoystickEvents { get; } = new Dictionary<uint, InputFunction>();
		public Dictionary<Keyboard.Key, InputFunction> KeyboardEvents { get; } = new Dictionary<Keyboard.Key, InputFunction>();

		public Action<Time> LoopFunction { get; set; }

		public class InputFunction
		{
			public Action OnPress   { get; set; }
			public Action OnHold    { get; set; }
			public Action OnRelease { get; set; }

			public bool IsPressed  { get; set; } = false;
			public bool WasPressed { get; set; } = false;

			public InputFunction(Action onPress = null, Action onHold = null, Action onRelease = null)
			{
				OnPress   = onPress ?? (() => { });
				OnHold    = onHold ?? (() => { });
				OnRelease = onRelease ?? (() => { });
			}
		}

		private RenderTexture _backgroundTexture;

		private View _view;

		internal Input Input { get; set; }

		public Window(Vector2u windowSize, Vector2u resolution, string title)
		{
			RenderWindow = new RenderWindow(new VideoMode(windowSize.X, windowSize.Y), title, Styles.Close | Styles.Titlebar);
			RenderWindow.SetFramerateLimit(60);
			RenderWindow.SetKeyRepeatEnabled(false);

			RenderWindow.Closed += (sender, args) => RenderWindow.Close();

			RenderWindow.JoystickButtonPressed += (sender, args) =>
			{
				if (!JoystickEvents.ContainsKey(args.Button)) return;

				InputFunction e = JoystickEvents[args.Button];
				if (e.WasPressed) return;

				e.WasPressed = e.IsPressed = true;
				e.OnPress();
			};
			RenderWindow.KeyPressed += (sender, args) =>
			{
				if (!KeyboardEvents.ContainsKey(args.Code)) return;

				InputFunction e = KeyboardEvents[args.Code];
				if (e.WasPressed) return;

				e.WasPressed = e.IsPressed = true;
				e.OnPress();
			};
			RenderWindow.JoystickButtonReleased += (sender, args) =>
			{
				if (!JoystickEvents.ContainsKey(args.Button)) return;

				InputFunction e = JoystickEvents[args.Button];
				if (!e.WasPressed) return;

				e.WasPressed = e.IsPressed = false;
				e.OnRelease();
			};
			RenderWindow.KeyReleased += (sender, args) =>
			{
				if (!KeyboardEvents.ContainsKey(args.Code)) return;

				InputFunction e = KeyboardEvents[args.Code];
				if (!e.WasPressed) return;

				e.WasPressed = e.IsPressed = false;
				e.OnRelease();
			};

			_view = new View(RenderWindow.GetView())
			{
				Size   = (Vector2f)resolution,
				Center = (Vector2f)resolution / 2f,
				Rotation = 0f,
			};
			RenderWindow.SetView(_view);

			_backgroundTexture = new RenderTexture(RenderWindow.Size.X, RenderWindow.Size.Y);
			_background = new Mode7
			{
				Resolution = windowSize,
			};

			Joystick.Update();
		}

		private Mode7 _background;

		public void MainLoop()
		{
			var deltaClock = new Clock();
			var runClock   = new Clock();

			float lastTime = 0f;

			var debugFont = new SFML.Graphics.Font(@"P:\CS\otter-rpg\otter-rpg-engine\Graphics\CONSOLA.TTF");
			var debugText = new Text("", debugFont, 7)
			{
				OutlineColor = Color.Black,
				OutlineThickness = 1f,
			};

			var frameCounter = 0;

			while (RenderWindow.IsOpen)
			{
				Time dt  = deltaClock.Restart();
				float  fps = 1f / dt.AsSeconds();

				frameCounter++;
				float secs = runClock.ElapsedTime.AsSeconds();
				if (secs - lastTime >= 1f)
				{
					lastTime                  = secs;
					debugText.DisplayedString = $"FPS: {frameCounter} ({fps:F2})\nObjects: {Drawables.Count}";
					frameCounter              = 0;
				}

				RenderWindow.DispatchEvents();

				foreach (InputFunction jsEvent in JoystickEvents.Values.Where(jsEvent => jsEvent.IsPressed))
					jsEvent.OnHold();

				foreach (InputFunction kbEvent in KeyboardEvents.Values.Where(e => e.IsPressed))
					kbEvent.OnHold();

				LoopFunction(dt);

				RenderWindow.Clear();

				_backgroundTexture.Clear();
				foreach (DrawableList.DrawableItem drawable in Drawables.List.Where(d => d.Background))
				{
					_backgroundTexture.Draw(drawable);
				}
				_backgroundTexture.Display();

				//_background.FromImage = _backgroundTexture.Texture.CopyToImage();

				//RenderWindow.Draw(_background);

				foreach (DrawableList.DrawableItem drawable in Drawables.List.Where(d => !d.Background))
				{
					RenderWindow.Draw(drawable);
				}

				RenderWindow.Draw(debugText);

				RenderWindow.Display();
			}
		}
	}
}
