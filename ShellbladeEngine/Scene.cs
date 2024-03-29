﻿using SFML.System;
using Shellblade.Graphics;

namespace Shellblade
{
	/// <summary> Scene object to build others from, contains drawable list, input handler, and game loop function </summary>
	public abstract class Scene
	{
		/// <summary> List of Drawable objects in the scene </summary>
		public DrawableList Drawables { get; } = new();

		/// <summary> Input handler for the scene </summary>
		public Input Input { get; } = new();

		/// <summary> Game loop code. This function is run every frame before rendering. </summary>
		/// <param name="dt"> Delta time; passed in from Game object </param>
		public abstract void Loop(Time dt);
	}
}
