using System.IO;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using YamlDotNet.Serialization;
using Text = Shellblade.Graphics.UI.Text;

namespace Shellblade.Graphics
{
	public class Game
	{
		private readonly View _view;

#if DEBUG
		private readonly DebugWindow _debug;
#endif

		public Scene Scene      { get; set; }
		public Color ClearColor { get; set; } = Color.Black;

		internal RenderWindow Window { get; }

		public  Vector2u     Resolution => (Vector2u)_view.Size;
		public  Vector2u     WindowSize => Window.Size;
		private DrawableList Drawables  => Scene.Drawables;
		private Input        Input      => Scene.Input;

		public Game(uint sizeX, uint sizeY, uint resX, uint resY, string title) : this(new Vector2u(sizeX, sizeY), new Vector2u(resX, resY), title) { }

		public Game(Vector2u windowSize, Vector2u resolution, string title)
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

#if DEBUG
			_debug = new DebugWindow();
#endif
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

			var debugText = new Text
			{
				GlobalPosition = new Vector2i(1, 1),
				LineSpacing    = 1,
				Visible        = true,
				String = "{f:tiny}FPS: -- (--.--)\n" +
				         "Avg. Delta: --.--ms\n" +
				         "Objects: ----\n" +
				         "UI Objs: ----\n" +
				         "Key Inputs:",
			};

			while (Window.IsOpen)
			{
				Time dt = deltaClock.Restart();

#if DEBUG
				_debug.Tick(dt, Drawables.Count, Input.UI.ElementCount);
#endif

				Window.DispatchEvents();

				Input.DoHoldInputs();

				Scene.Loop(dt);

				Window.Clear(ClearColor);

				for (var i = 0; i < Drawables.Count; i++)
					Window.Draw(Drawables[i]);

				Window.Draw(Input.UI);

				Window.Display();
			}

#if DEBUG
			_debug.Stop();
#endif

			ISerializer serializer = new SerializerBuilder().Build();
			File.WriteAllText("./ui.yaml", serializer.Serialize(Input.UI));
		}
	}
}
