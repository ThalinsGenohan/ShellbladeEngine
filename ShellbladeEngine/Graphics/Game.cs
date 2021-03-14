using System;
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

		public Vector2u Resolution { get; }

		public   Scene        Scene      { get; set; }
		public   Color        ClearColor { get; set; } = Color.Black;
		internal RenderWindow Window     { get; set; }

		public  Action<Time> LoopFunction => Scene.Loop;
		public  Vector2u     Size         => Window.Size;
		private DrawableList Drawables    => Scene.Drawables;
		private Input        Input        => Scene.Input;

		public Game(uint sizeX, uint sizeY, uint resX, uint resY, string title) : this(new Vector2u(sizeX, sizeY), new Vector2u(resX, resY), title) { }

		public Game(Vector2u windowSize, Vector2u resolution, string title)
		{
			Resolution = resolution;

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

			var lastTime = 0f;

			var debugText = new Text
			{
				GlobalPosition = new Vector2i(1, 1),
				LineSpacing    = 1,
				Visible        = true,
				String = "{f:tiny}FPS: -- (--.--)\n" +
				         "Avg. Delta: --.--ms\n" +
				         "Objects: ----\n" +
				         "UI Objs: ----",
			};

			var frameCounter = 0;
			var averageFps   = 0f;
			var averageDt    = 0f;

			while (Window.IsOpen)
			{
				Time  dt  = deltaClock.Restart();
				float fps = 1f / dt.AsSeconds();

				frameCounter++;
				averageFps += fps;
				averageDt  += dt.AsMilliseconds();

				float secs = runClock.ElapsedTime.AsSeconds();
				if (secs - lastTime >= 1f)
				{
					lastTime = secs;
					debugText.String = "{f:tiny}" + $"FPS: {frameCounter:D2} ({averageFps / frameCounter:F2})\n" +
					                   $"Avg. Delta: {averageDt / frameCounter:F2}ms\n" +
					                   $"Objects: {Drawables.Count}\n" +
					                   $"UI Objs: {Input.UI.ElementCount}";
					frameCounter = 0;
					averageFps   = 0f;
					averageDt    = 0f;
				}

				Window.DispatchEvents();

				Input.DoHoldInputs();

				LoopFunction(dt);

				Window.Clear(ClearColor);

				for (var i = 0; i < Drawables.Count; i++)
					Window.Draw(Drawables[i]);

				Window.Draw(Input.UI);
				Window.Draw(debugText);

				Window.Display();
			}

			ISerializer serializer = new SerializerBuilder().Build();
			File.WriteAllText("./ui.yaml", serializer.Serialize(Input.UI));
		}
	}
}
