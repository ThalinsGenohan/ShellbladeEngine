using SFML.System;
using Shellblade.Graphics;

namespace Shellblade
{
	/// <summary> Scene object to build others from, contains drawable list, input handler, and game loop function </summary>
	public abstract class Scene
	{
		protected Game Game;

		/// <summary> List of Drawable objects in the scene </summary>
		public DrawableList Drawables { get; } = new DrawableList();

		/// <summary> Input handler for the scene </summary>
		public Input Input { get; } = new Input();

		protected Scene(Game game)
		{
			Game = game;
		}

		/// <summary> Game loop code. This function is run every frame before rendering. </summary>
		/// <param name="dt"> Delta time; passed in from Game object </param>
		public abstract void Loop(Time dt);
	}
}
