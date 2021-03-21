using SFML.Graphics;
using SFML.System;
using System;

namespace Shellblade.Graphics.UI
{
	public class Button : UIElement
	{
		private Sprite _sprite;

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

		public Vector2f GetSpriteScale()
        {
			return _sprite.Scale;
        }

		public void SetSpriteScale()
        {
			var buttonSize = Size;
			var textureSize = Texture.Size;
			var newScale = new Vector2f((float)buttonSize.X / textureSize.X, (float)buttonSize.Y / textureSize.Y);
			_sprite.Scale = newScale;
		}

		public void SetSize(Vector2i size)
        {
			Size = size;
			SetSpriteScale();
        }

		public Button(Vector2i size, Texture texture, Game window) :
			this(size, texture, window, () => { }, () => { }, () => { }) { }

		public Button(Vector2i size, Texture texture, Game window, Action customOnClick, Action customOnMouseOver, Action customOnMouseOff)
		{
			Texture = texture;
			_sprite = new Sprite(Texture)
			{
				TextureRect = new IntRect(0, 0, (int)texture.Size.X, (int)texture.Size.Y),
				Position = new Vector2f(0f, 0f),
			};
			Size = size;

			OnClick     = () => { customOnClick(); };
			OnMouseOver = () => { window.SetCursor(@"assets/cursor_hand.png", 4, 1, window); customOnMouseOver(); };
			OnMouseOff = () => { window.SetCursor(@"assets/cursor_arrow.png", 0, 0, window); customOnMouseOff(); };

			SetSpriteScale();
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			target.Draw(_sprite, states);
			base.Draw(target, states);
		}
	}
}
