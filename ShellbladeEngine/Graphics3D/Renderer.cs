using System.Collections.Generic;
using System.Numerics;
using OpenTK.Graphics.OpenGL;
using SFML.Graphics;
using SFML.Window;
using Color = System.Drawing.Color;
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;

namespace Shellblade.Graphics3D
{
	public class Renderer : Drawable
	{
		private RenderTexture _texture;

		private DrawList _drawList;

		public Renderer()
		{
			_texture  = new RenderTexture(Game.WindowSize.X, Game.WindowSize.Y);

			_texture.SetActive(true);

			GL.Enable(EnableCap.DepthTest);
			GL.DepthMask(true);
			GL.ClearDepth(1f);

			GL.Viewport(0, 0, (int)_texture.Size.X, (int)_texture.Size.Y);

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
			GL.ClearColor(Color.Red);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.Begin(PrimitiveType.TriangleStrip);
			_drawList.Render();
			GL.End();

			_texture.Display();

			var sprite = new Sprite(_texture.Texture);

			target.Draw(sprite, states);
		}
	}
}
