using SFML.System;
using Shellblade.Graphics;
using Shellblade.Graphics.UI;

namespace Shellblade
{
	public abstract class Scene
	{
		protected Game Game;

		public DrawableList Drawables { get; }
		public Input        Input     { get; set; }

		public UIContainer UI
		{
			get => Input.UI;
			set => Input.UI = value;
		}

		protected Scene(Game game)
		{
			Game = game;

			Drawables = new DrawableList();
			Input     = new Input();
		}

		protected abstract void Loop(Time dt);
	}
}
