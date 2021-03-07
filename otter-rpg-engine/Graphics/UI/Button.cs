using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics.UI
{
	public class Button : UIElement
	{
		private readonly Sprite _sprite;

		public Font    Font          { get; set; }
		public string  DisplayedText { get; set; }
		public Texture Texture       { get; set; }

		public Color Color
		{
			get => _sprite.Color;
			set => _sprite.Color = value;
		}

		public Button(Vector2i pos, Vector2i size, string texturePath)
		{
			Texture = new Texture(texturePath)
			{
				Repeated = true,
			};
			_sprite = new Sprite(Texture)
			{
				TextureRect = new IntRect(0, 0, size.X, size.Y),
				Position    = (Vector2f)pos,
			};

			BoundingBox = new IntRect(pos, size);
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			target.Draw(_sprite, states);
		}
	}
}
