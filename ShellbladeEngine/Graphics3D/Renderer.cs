using System.Collections.Generic;
using System.Numerics;
using OpenTK.Graphics.OpenGL;
using SFML.Graphics;
using SFML.Window;

namespace Shellblade.Graphics3D
{
	public class Renderer : Drawable
	{
		private RenderTexture _texture;

		private DrawList _drawList;

		public Renderer()
		{
			_texture  = new RenderTexture(800, 600);
			_drawList = new DrawList();
			var cube = new Model
			{
				Vertices = new List<Vertex>
				{
					new(new Vector3(-20, -20, -20), new Vector2(0, 0)),
					new(new Vector3(-20, 20,  -20), new Vector2(1, 0)),
					new(new Vector3(-20, -20, 20), new Vector2(0,  1)),
					new(new Vector3(-20, -20, 20), new Vector2(0,  1)),
					new(new Vector3(-20, 20,  -20), new Vector2(1, 0)),
					new(new Vector3(-20, 20,  20), new Vector2(1,  1)),

					new(new Vector3(20, -20, -20), new Vector2(0, 0)),
					new(new Vector3(20, 20,  -20), new Vector2(1, 0)),
					new(new Vector3(20, -20, 20), new Vector2(0,  1)),
					new(new Vector3(20, -20, 20), new Vector2(0,  1)),
					new(new Vector3(20, 20,  -20), new Vector2(1, 0)),
					new(new Vector3(20, 20,  20), new Vector2(1,  1)),

					new(new Vector3(-20, -20, -20), new Vector2(0, 0)),
					new(new Vector3(20,  -20, -20), new Vector2(1, 0)),
					new(new Vector3(-20, -20, 20), new Vector2(0,  1)),
					new(new Vector3(-20, -20, 20), new Vector2(0,  1)),
					new(new Vector3(20,  -20, -20), new Vector2(1, 0)),
					new(new Vector3(20,  -20, 20), new Vector2(1,  1)),

					new(new Vector3(-20, 20, -20), new Vector2(0, 0)),
					new(new Vector3(20,  20, -20), new Vector2(1, 0)),
					new(new Vector3(-20, 20, 20), new Vector2(0,  1)),
					new(new Vector3(-20, 20, 20), new Vector2(0,  1)),
					new(new Vector3(20,  20, -20), new Vector2(1, 0)),
					new(new Vector3(20,  20, 20), new Vector2(1,  1)),

					new(new Vector3(-20, -20, -20), new Vector2(0, 0)),
					new(new Vector3(20,  -20, -20), new Vector2(1, 0)),
					new(new Vector3(-20, 20,  -20), new Vector2(0, 1)),
					new(new Vector3(-20, 20,  -20), new Vector2(0, 1)),
					new(new Vector3(20,  -20, -20), new Vector2(1, 0)),
					new(new Vector3(20,  20,  -20), new Vector2(1, 1)),

					new(new Vector3(-20, -20, 20), new Vector2(0, 0)),
					new(new Vector3(20,  -20, 20), new Vector2(1, 0)),
					new(new Vector3(-20, 20,  20), new Vector2(0, 1)),
					new(new Vector3(-20, 20,  20), new Vector2(0, 1)),
					new(new Vector3(20,  -20, 20), new Vector2(1, 0)),
					new(new Vector3(20,  20,  20), new Vector2(1, 1)),
				},
			};
			_drawList.Models.Add(cube);

			_drawList.Camera.Position = new Vector3(0f, 0f, 50f);
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			_texture.SetActive(true);

			GL.Clear(ClearBufferMask.DepthBufferBit);
			_drawList.Render();
			_texture.Display();

			var sprite = new Sprite(_texture.Texture);

			target.Draw(sprite, states);
		}
	}
}
