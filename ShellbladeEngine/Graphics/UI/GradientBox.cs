using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics.UI
{
	public class GradientBox : Box
	{
		private RenderTexture _renderTexture;
		private Texture       Texture => _renderTexture.Texture;
		private Sprite        _sprite = new Sprite();

		private Color[] _colors = new Color[4];

		public Color[] Colors
		{
			get => _colors;
			set
			{
				_colors = value;
				GenerateSprite();
			}
		}

		public override Vector2i Size
		{
			get => base.Size;
			set
			{
				base.Size = value;
				GenerateSprite();
			}
		}

		public GradientBox()
		{
		}

		private void GenerateSprite()
		{
			if (Size.X <= 0f || Size.Y <= 0f) return;

			var bg = new VertexArray(PrimitiveType.Quads, 4)
			{
				[0] = new Vertex(new Vector2f(0f,     0f),     Colors[0]),
				[1] = new Vertex(new Vector2f(Size.X, 0f),     Colors[1]),
				[2] = new Vertex(new Vector2f(Size.X, Size.Y), Colors[2]),
				[3] = new Vertex(new Vector2f(0f,     Size.Y), Colors[3]),
			};
			_renderTexture = new RenderTexture((uint)Size.X, (uint)Size.Y)
			{
				Smooth = false,
			};
			_renderTexture.Clear(new Color(0x00000000));
			_renderTexture.Draw(bg);
			_renderTexture.Display();

			_sprite = new Sprite(Texture)
			{
				Position = (Vector2f)GlobalPosition,
			};
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			target.Draw(_sprite, states);
			base.Draw(target, states);
		}
	}
}
