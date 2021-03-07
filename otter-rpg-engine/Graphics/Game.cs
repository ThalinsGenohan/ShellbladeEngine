using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Shellblade.Graphics
{
	public class Game
	{
		private readonly View _view;

		public Vector2u Resolution { get; }

		public   Scene        Scene        { get; set; }
		public   Action<Time> LoopFunction { get; set; }
		public   Color        ClearColor   { get; set; } = Color.Black;
		internal RenderWindow Window       { get; set; }

		public Vector2u Size => Window.Size;

		private DrawableList Drawables => Scene.Drawables;
		private Input        Input     => Scene.Input;

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

			var debugFont = new SFML.Graphics.Font(@"P:\CS\otter-rpg\otter-rpg-engine\Graphics\CONSOLA.TTF");
			var debugText = new Text("", debugFont, 7)
			{
				OutlineColor     = Color.Black,
				OutlineThickness = 1f,
			};

			var frameCounter = 0;

			while (Window.IsOpen)
			{
				Time  dt  = deltaClock.Restart();
				float fps = 1f / dt.AsSeconds();

				frameCounter++;
				float secs = runClock.ElapsedTime.AsSeconds();
				if (secs - lastTime >= 1f)
				{
					lastTime                  = secs;
					debugText.DisplayedString = $"FPS: {frameCounter} ({fps:F2})\nObjects: {Drawables.Count}";
					frameCounter              = 0;
				}

				Window.DispatchEvents();

				Input.DoHoldInputs();

				LoopFunction(dt);

				Window.Clear(ClearColor);

				for (var i = 0; i < Drawables.Count; i++)
					Window.Draw(Drawables[i]);

				Window.Draw(debugText);

				Window.Display();
			}
		}
	}
}
