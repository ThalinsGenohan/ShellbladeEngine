using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics.UI
{
	public class Image : UIElement
	{
		private readonly Sprite _sprite;

		public Texture Texture { get; set; }

		public Color Color
		{
			get => _sprite.Color;
			set => _sprite.Color = value;
		}

		public float Opacity
		{
			get => Color.A / 255f;
			set => Color = new Color(Color.R, Color.G, Color.B, (byte)(value * 255f));
		}

		public override Vector2i GlobalPosition
		{
			get => base.GlobalPosition;
			set
			{
				base.GlobalPosition = value;
				_sprite.Position    = (Vector2f)value;
			}
		}

		public override Vector2i LocalPosition
		{
			get => base.LocalPosition;
			set
			{
				base.LocalPosition = value;
				_sprite.Position   = (Vector2f)GlobalPosition;
			}
		}

		public Image(Vector2i size, Texture texture)
		{
			Texture = texture;
			_sprite = new Sprite(Texture)
			{
				TextureRect = new IntRect(0, 0, size.X, size.Y),
				Position    = new Vector2f(0f, 0f),
			};
			Size = size;
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			target.Draw(_sprite, states);
			base.Draw(target, states);
		}
	}
}
