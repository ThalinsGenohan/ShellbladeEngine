using System.IO;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Shellblade.Graphics;
using YamlDotNet.Serialization;

namespace Shellblade
{
	public static class Game
	{
		private static View        _view;
		private static DebugWindow _debugWindow;
		private static bool        _debug;

		public static Scene Scene      { get; set; }
		public static Color ClearColor { get; set; } = Color.Black;

		internal static RenderWindow Window { get; private set; }

		public static Vector2u Resolution => (Vector2u)_view.Size;
		public static Vector2u WindowSize => Window.Size;
		public static Clock    Timer      { get; private set; }

		private static DrawableList Drawables  => Scene.Drawables;
		private static Input        Input      => Scene.Input;

		public static void Initialize(Vector2u windowSize, Vector2u resolution, string title, bool debug = false)
		{
			Window = new RenderWindow(new VideoMode(windowSize.X, windowSize.Y), title, Styles.Close | Styles.Titlebar);
			Window.SetFramerateLimit(60);
			Window.SetKeyRepeatEnabled(false);

			Window.Closed += (_, _) => Window.Close();

			_view = new View(Window.GetView())
			{
				Size     = (Vector2f)resolution,
				Center   = (Vector2f)resolution / 2f,
				Rotation = 0f,
			};
			Window.SetView(_view);

			Joystick.Update();

			_debug = debug;
			if (!_debug) return;

			_debugWindow  =  new DebugWindow();
			Window.Closed += (_, _) => _debugWindow.Stop();

			Timer = new Clock();
		}

		public static void LoadScene(Scene scene)
		{
			if (Scene != null) Input.Uninstall();

			Scene = scene;
			Scene.Input.Install();
		}

		public static void Run()
		{
			var deltaClock = new Clock();
			var runClock   = new Clock();

			while (Window.IsOpen)
			{
				DeltaTime = deltaClock.Restart();

				if (_debug) _debugWindow.Tick(DeltaTime, Drawables.Count, Input.UI.TotalElementCount);

				Window.DispatchEvents();

				Input.DoHoldInputs();

				Scene.Loop(DeltaTime);

				Window.Clear(ClearColor);

				for (var i = 0; i < Drawables.Count; i++)
					Window.Draw(Drawables[i]);

				Window.Draw(Input.UI);

				Window.Display();
			}

			ISerializer serializer = new SerializerBuilder().Build();
			File.WriteAllText("./ui.yaml", serializer.Serialize(Input.UI));
		}

		public static Time DeltaTime { get; private set; }

		public static void UpdateCursor(Cursor cursor)
		{
			Window.SetMouseCursor(cursor);
		}
	}
}
