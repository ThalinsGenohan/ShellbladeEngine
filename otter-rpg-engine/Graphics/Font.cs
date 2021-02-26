using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics
{
	public class Font
	{
		private readonly Texture _texture;
		private          int     _spaceSize;

		public bool     VariableWidth  { get; set; }
		public int      TrackingOffset { get; set; }
		public Vector2i Size           { get; set; }

		public int SpaceSize
		{
			get => VariableWidth ? _spaceSize : Size.X;
			set
			{
				_spaceSize            = value;
				Characters[' '].Width = value;
			}
		}

		public Dictionary<char, Character> Characters { get; }



		public Font(string name) : this(name, new Vector2i(8, 8))
		{
		}

		public Font(string name, Vector2i size)
		{
			_texture = new Texture($"{name}.png");
			Size     = size;

			int[] widths =
				(from line in File.ReadAllLines($"{name}.size")
				 from value in Regex.Split(line, @",\s*").Where(s => !string.IsNullOrEmpty(s))
				 select Convert.ToInt32(value)).ToArray();

			Characters = new Dictionary<char, Character>
			{
				{ ' ', new Character(_texture, new IntRect(0, 0, Size.X, Size.Y), SpaceSize) },
			};

			TrackingOffset = 0;

			for (var c = '!'; c <= '~'; c++)
				Characters.Add(c, new Character(_texture, new IntRect(Size.X * ((c - ' ') % 16), Size.Y * ((c - ' ') / 16), Size.X, Size.Y), widths[c - ' ']));
		}

		public class Character
		{
			private readonly Texture _texture;
			private readonly IntRect _rect;

			public int Width { get; set; }

			public Sprite Sprite => new Sprite(_texture, _rect);

			public Character(Texture texture, IntRect rect) : this(texture, rect, rect.Width)
			{
			}

			public Character(Texture texture, IntRect rect, int width)
			{
				_texture = texture;
				_rect    = rect;
				Width    = width;
			}
		}
	}
}
