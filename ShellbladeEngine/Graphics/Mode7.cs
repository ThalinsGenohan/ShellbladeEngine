using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics
{
	public class Mode7 : Drawable
	{
		private readonly Sprite _sprite = new();

		private readonly Transform _trans = new();

		private Image    _toImage;
		private Image    _fromImage;
		private Vector2u _scroll;
		private Vector2u _center;
		private double   _rotation;
		private Vector2f _scale;
		private Vector2u _resolution;
		private bool     _rendered = false;
		private Vector2f _skew;

		public Image FromImage
		{
			get => _fromImage;
			set
			{
				_fromImage = value;
				_rendered  = false;
			}
		}

		public Vector2u Scroll
		{
			get => _scroll;
			set
			{
				_scroll   = value;
				_rendered = false;
			}
		}

		public Vector2u Center
		{
			get => _center;
			set
			{
				_center   = value;
				_rendered = false;
			}
		}

		public double Rotation
		{
			get => _rotation;
			set
			{
				_rotation = value;
				_rendered = false;
			}
		}

		public Vector2f Scale
		{
			get => _scale;
			set
			{
				_scale    = value;
				_rendered = false;
			}
		}

		public Vector2u Resolution
		{
			get => _resolution;
			set
			{
				_resolution = value;
				_toImage    = new Image(value.X, value.Y);
				_rendered   = false;
			}
		}

		public Vector2f Skew
		{
			get => _skew;
			set
			{
				_skew     = value;
				_rendered = false;
			}
		}

		/*private double _h   => Scroll.X / 256.0;
		private double _v   => Scroll.Y / 256.0;
		private double _x   => Center.X / 256.0;
		private double _y   => Center.Y / 256.0;
		private double _a   => Math.Cos(rads) * Scale.X;
		private double _b   => Math.Sin(rads) * (Scale.X + Skew.X * Scale.Y);
		private double _c   => -Math.Sin(rads) * (Scale.Y + Skew.Y * Scale.X);
		private double _d   => Math.Cos(rads) * Scale.Y;
		private double rads => Rotation * Math.PI / 180.0;*/

		public void Draw(RenderTarget target, RenderStates states)
		{
			if (!_rendered) DrawTexture();
			target.Draw(_sprite, states);
		}

		public void Translate(int x, int y)
		{
			_rendered = false;
			_trans.Translate(x, y);
		}

		private void DrawTexture()
		{
			_rendered = true;

			Vector2u size = _toImage.Size;

			for (uint yy = 0; yy < size.Y; yy++)
			for (uint xx = 0; xx < size.X; xx++)
			{
				Vector2i vect = GetVect(xx, yy);

				Color pixel;
				if (vect.X == -1 || vect.Y == -1)
					pixel  = new Color(0, 0, 0, 0);
				else pixel = FromImage.GetPixel((uint)vect.X, (uint)vect.Y);
				_toImage.SetPixel(xx, yy, pixel);
			}

			if (_sprite.Texture == null) _sprite.Texture = new Texture(_toImage);
			else _sprite.Texture.Update(_toImage);
		}

		private Vector2i GetVect(uint x, uint y)
		{
			Vector2f v = _trans.Matrix * new Vector2f(x, y);

			/*double xi   = x / 256.0 + _h - _x;
			double yi   = y / 256.0 + _v - _y;
			double getX = _a * xi + _b * yi + _x;
			double getY = _c * xi + _d * yi + _y;

			var uX = (int)(getX * 256.0);
			var uY = (int)(getY * 256.0);*/

			if (v.X < 0 || v.Y < 0 || v.X > FromImage.Size.X || v.Y > FromImage.Size.Y) v.X = v.Y = -1;

			return new Vector2i((int)v.X, (int)v.Y);
		}

		public class Matrix
		{
			private readonly double[][] _numbers;

			public double A
			{
				get => this[0][0];
				set => this[0][0] = value;
			}

			public double B
			{
				get => this[0][1];
				set => this[0][1] = value;
			}

			public double C
			{
				get => this[1][0];
				set => this[1][0] = value;
			}

			public double D
			{
				get => this[1][1];
				set => this[1][1] = value;
			}

			public double X
			{
				get => this[0][2];
				set => this[0][2] = value;
			}

			public double Y
			{
				get => this[1][2];
				set => this[1][2] = value;
			}

			public double[] this[int index]
			{
				get => _numbers[index];
				set => _numbers[index] = value;
			}

			public Matrix(double[][] values)
			{
				_numbers = values;
			}

			public Matrix(double[] values)
			{
				_numbers = new[]
				{
					new[] { values[0], values[1], values[2] },
					new[] { values[3], values[4], values[5] },
					new[] { values[6], values[7], values[8] },
				};
			}

			public Matrix(double x0,
			              double x1,
			              double x2,
			              double y0,
			              double y1,
			              double y2,
			              double z0,
			              double z1,
			              double z2)
			{
				_numbers = new[]
				{
					new[] { x0, x1, x2 },
					new[] { y0, y1, y2 },
					new[] { z0, z1, z2 },
				};
			}

			public Matrix()
			{
				_numbers = new[]
				{
					new[] { 0.0, 0.0, 0.0 },
					new[] { 0.0, 0.0, 0.0 },
					new[] { 0.0, 0.0, 0.0 },
				};
			}

			public Matrix(Matrix copy)
			{
				_numbers = copy._numbers.Clone() as double[][];
			}

			public static Matrix operator *(Matrix a, Matrix b)
			{
				var m = new Matrix();
				for (var x = 0; x < 3; x++)
				for (var y = 0; y < 3; y++)
				for (var i = 0; i < 3; i++)
					m[y][x] += a[i][x] * b[y][i];

				return m;
			}

			public static double[] operator *(Matrix a, double[] b)
			{
				return new[]
				{
					a[0][0] * b[0] + a[0][1] * b[1] + a[0][2] * b[2],
					a[1][0] * b[0] + a[1][1] * b[1] + a[1][2] * b[2],
					a[2][0] * b[0] + a[2][1] * b[1] + a[2][2] * b[2],
				};
			}

			public static Vector2f operator *(Matrix a, Vector2f b)
			{
				double[] c = a * new[] { b.X, b.Y, 1.0 };
				return new Vector2f((float)c[0], (float)c[1]);
			}

			public static Vector3f operator *(Matrix a, Vector3f b)
			{
				double[] c = a * new double[] { b.X, b.Y, b.Z };
				return new Vector3f((float)c[0], (float)c[1], (float)c[2]);
			}

			public override string ToString() =>
				$"{this[0][0]},{this[0][1]},{this[0][2]}\n{this[1][0]},{this[1][1]},{this[1][2]}\n{this[2][0]},{this[2][1]},{this[2][2]}";
		}

		public class Transform
		{
			public static readonly Matrix Identity = new(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0);

			public Matrix Matrix { get; set; } = new(Identity);

			public void Translate(int x, int y)
			{
				Matrix = new Matrix(1.0, 0.0, x, 0.0, 1.0, y, 0.0, 0.0, 1.0) * Matrix;
			}
		}
	}
}
