using System.IO;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Shellblade.Graphics;
using YamlDotNet.Serialization;

namespace Shellblade
{
	public class Game
	{
		private readonly View        _view;
		private readonly DebugWindow _debugWindow;
		private readonly bool        _debug;

		public Scene Scene      { get; set; }
		public Color ClearColor { get; set; } = Color.Black;

		internal RenderWindow Window { get; }

		public  Vector2u     Resolution => (Vector2u)_view.Size;
		public  Vector2u     WindowSize => Window.Size;
		private DrawableList Drawables  => Scene.Drawables;
		private Input        Input      => Scene.Input;

		public Game(uint sizeX, uint sizeY, uint resX, uint resY, string title, bool debug = false) : this(new Vector2u(sizeX, sizeY), new Vector2u(resX, resY), title, debug) { }

		public Game(Vector2u windowSize, Vector2u resolution, string title, bool debug = false)
		{
			Window = new RenderWindow(new VideoMode(windowSize.X, windowSize.Y), title, Styles.Close | Styles.Titlebar);
			Window.SetFramerateLimit(60);
			Window.SetKeyRepeatEnabled(false);

			Window.Closed += (sender, args) => Window.Close();

			_view = new View(Window.GetView())
			{
				Size     = (Vector2f)resolution,
				Center   = (Vector2f)resolution / 2f,
				Rotation = 0f,
			};
			Window.SetView(_view);

			Joystick.Update();

			_debug = debug;
			if (_debug)
			{
				_debugWindow  =  new DebugWindow();
				Window.Closed += (sender, args) => _debugWindow.Stop();
			}
		}

		public void LoadScene(Scene scene)
		{
			if (Scene != null) Input.Uninstall();

			Scene = scene;
			Scene.Input.Install(this);
		}

		public void Run()
		{
			var deltaClock = new Clock();
			var runClock   = new Clock();

			while (Window.IsOpen)
			{
				Time dt = deltaClock.Restart();

				if (_debug) _debugWindow.Tick(dt, Drawables.Count, Input.UI.ElementCount);

				Window.DispatchEvents();

				Input.DoHoldInputs();

				Scene.Loop(dt);

				Window.Clear(ClearColor);

				for (var i = 0; i < Drawables.Count; i++)
					Window.Draw(Drawables[i]);

				Window.Draw(Input.UI);

				Window.Display();
			}

			ISerializer serializer = new SerializerBuilder().Build();
			File.WriteAllText("./ui.yaml", serializer.Serialize(Input.UI));
		}

		public void UpdateCursor(Cursor cursor)
        {
			Window.SetMouseCursor(cursor);
        }
	}
}
