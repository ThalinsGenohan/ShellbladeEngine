using System;
using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics
{
	public class Mode7 : Drawable
	{
		private Image _toImage;

		private Sprite _sprite = new Sprite();

		public Image FromImage { get; set; }

		public Vector2u Scroll   { get; set; }
		public Vector2u Center   { get; set; }
		public double   Rotation { get; set; }
		public Vector2f Scale    { get; set; }

		public Vector2u Resolution { get; set; }

		private double _h   => Scroll.X / 256.0;
		private double _v   => Scroll.Y / 256.0;
		private double _x   => Center.X / 256.0;
		private double _y   => Center.Y / 256.0;
		private double _a   => Math.Cos(rads) * Scale.X;
		private double _b   => Math.Sin(rads) * Scale.X;
		private double _c   => -Math.Sin(rads) * Scale.Y;
		private double _d   => Math.Cos(rads) * Scale.Y;
		private double rads => Rotation * Math.PI / 180.0;

		public Texture DrawTexture()
		{
			_toImage = new Image(Resolution.X, Resolution.Y);

			Vector2u size = _toImage.Size;

			for (uint yy = 0; yy < size.Y; yy++)
			for (uint xx = 0; xx < size.X; xx++)
			{
				Vector2u vect  = GetVect(xx, yy);
				Color    pixel = FromImage.GetPixel(vect.X, vect.Y);
				_toImage.SetPixel(xx, yy, pixel);
			}

			_toImage.SaveToFile(@"P:\CS\otter-rpg\otter-rpg-engine\Graphics\mode7.png");

			return new Texture(_toImage);
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			_sprite.Texture = DrawTexture();

			target.Draw(_sprite, states);
		}

		private Vector2u GetVect(uint x, uint y)
		{
			double xi   = x / 256.0 + _h - _x;
			double yi   = y / 256.0 + _v - _y;
			double getX = _a * xi + _b * yi + _x;
			double getY = _c * xi + _d * yi + _y;

			var uX = (int)(getX * 256.0);
			var uY = (int)(getY * 256.0);

			if (uX < 0 || uY < 0 || uX > FromImage.Size.X || uY > FromImage.Size.Y) uX = uY = 0;

			return new Vector2u((uint)uX, (uint)uY);
		}
	}
}
