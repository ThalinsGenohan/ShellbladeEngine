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

		public bool VariableWidth { get; set; }

		public int SpaceSize
		{
			get => VariableWidth ? _spaceSize : Size.X;
			set
			{
				_spaceSize            = value;
				Characters[' '].Width = value;
			}
		}

		public Vector2i Size { get; set; }

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

			for (var c = '!'; c <= '~'; c++)
				Characters.Add(c, new Character(_texture, new IntRect(Size.X * ((c - ' ') % 16), Size.Y * ((c - ' ') / 16), Size.X, Size.Y), widths[c - ' ']));

			/*{
				{ " ", new Character(_texture,  new IntRect(Size.X * 0,  Size.Y * 0, SpaceSize, Size.Y)) },
				{ "!", new Character(_texture,  new IntRect(Size.X * 1,  Size.Y * 0, widths[1], Size.Y)) },
				{ "\"", new Character(_texture, new IntRect(Size.X * 2,  Size.Y * 0, widths[2], Size.Y)) },
				{ "#", new Character(_texture,  new IntRect(Size.X * 3,  Size.Y * 0, widths[3], Size.Y)) },
				{ "$", new Character(_texture,  new IntRect(Size.X * 4,  Size.Y * 0, widths[4], Size.Y)) },
				{ "%", new Character(_texture,  new IntRect(Size.X * 5,  Size.Y * 0, widths[5], Size.Y)) },
				{ "&", new Character(_texture,  new IntRect(Size.X * 6,  Size.Y * 0, widths[6], Size.Y)) },
				{ "'", new Character(_texture,  new IntRect(Size.X * 7,  Size.Y * 0, widths[7], Size.Y)) },
				{ "(", new Character(_texture,  new IntRect(Size.X * 8,  Size.Y * 0, widths[8], Size.Y)) },
				{ ")", new Character(_texture,  new IntRect(Size.X * 9,  Size.Y * 0, widths[9], Size.Y)) },
				{ "*", new Character(_texture,  new IntRect(Size.X * 10, Size.Y * 0, widths[10], Size.Y)) },
				{ "+", new Character(_texture,  new IntRect(Size.X * 11, Size.Y * 0, widths[11], Size.Y)) },
				{ ",", new Character(_texture,  new IntRect(Size.X * 12, Size.Y * 0, widths[12], Size.Y)) },
				{ "-", new Character(_texture,  new IntRect(Size.X * 13, Size.Y * 0, widths[13], Size.Y)) },
				{ ".", new Character(_texture,  new IntRect(Size.X * 14, Size.Y * 0, widths[14], Size.Y)) },
				{ "/", new Character(_texture,  new IntRect(Size.X * 15, Size.Y * 0, widths[15], Size.Y)) },

				{ "0", new Character(_texture, new IntRect(Size.X * 0,  Size.Y * 1, widths[16], Size.Y)) },
				{ "1", new Character(_texture, new IntRect(Size.X * 1,  Size.Y * 1, widths[17], Size.Y)) },
				{ "2", new Character(_texture, new IntRect(Size.X * 2,  Size.Y * 1, widths[18], Size.Y)) },
				{ "3", new Character(_texture, new IntRect(Size.X * 3,  Size.Y * 1, widths[19], Size.Y)) },
				{ "4", new Character(_texture, new IntRect(Size.X * 4,  Size.Y * 1, widths[20], Size.Y)) },
				{ "5", new Character(_texture, new IntRect(Size.X * 5,  Size.Y * 1, widths[21], Size.Y)) },
				{ "6", new Character(_texture, new IntRect(Size.X * 6,  Size.Y * 1, widths[22], Size.Y)) },
				{ "7", new Character(_texture, new IntRect(Size.X * 7,  Size.Y * 1, widths[23], Size.Y)) },
				{ "8", new Character(_texture, new IntRect(Size.X * 8,  Size.Y * 1, widths[24], Size.Y)) },
				{ "9", new Character(_texture, new IntRect(Size.X * 9,  Size.Y * 1, widths[25], Size.Y)) },
				{ ":", new Character(_texture, new IntRect(Size.X * 10, Size.Y * 1, widths[26], Size.Y)) },
				{ ";", new Character(_texture, new IntRect(Size.X * 11, Size.Y * 1, widths[27], Size.Y)) },
				{ "<", new Character(_texture, new IntRect(Size.X * 12, Size.Y * 1, widths[28], Size.Y)) },
				{ "=", new Character(_texture, new IntRect(Size.X * 13, Size.Y * 1, widths[29], Size.Y)) },
				{ ">", new Character(_texture, new IntRect(Size.X * 14, Size.Y * 1, widths[30], Size.Y)) },
				{ "?", new Character(_texture, new IntRect(Size.X * 15, Size.Y * 1, widths[31], Size.Y)) },

				{ "@", new Character(_texture, new IntRect(Size.X * 0,  Size.Y * 2, widths[32], Size.Y)) },
				{ "A", new Character(_texture, new IntRect(Size.X * 1,  Size.Y * 2, widths[33], Size.Y)) },
				{ "B", new Character(_texture, new IntRect(Size.X * 2,  Size.Y * 2, widths[34], Size.Y)) },
				{ "C", new Character(_texture, new IntRect(Size.X * 3,  Size.Y * 2, widths[35], Size.Y)) },
				{ "D", new Character(_texture, new IntRect(Size.X * 4,  Size.Y * 2, widths[36], Size.Y)) },
				{ "E", new Character(_texture, new IntRect(Size.X * 5,  Size.Y * 2, widths[37], Size.Y)) },
				{ "F", new Character(_texture, new IntRect(Size.X * 6,  Size.Y * 2, widths[38], Size.Y)) },
				{ "G", new Character(_texture, new IntRect(Size.X * 7,  Size.Y * 2, widths[39], Size.Y)) },
				{ "H", new Character(_texture, new IntRect(Size.X * 8,  Size.Y * 2, widths[40], Size.Y)) },
				{ "I", new Character(_texture, new IntRect(Size.X * 9,  Size.Y * 2, widths[41], Size.Y)) },
				{ "J", new Character(_texture, new IntRect(Size.X * 10, Size.Y * 2, widths[42], Size.Y)) },
				{ "K", new Character(_texture, new IntRect(Size.X * 11, Size.Y * 2, widths[43], Size.Y)) },
				{ "L", new Character(_texture, new IntRect(Size.X * 12, Size.Y * 2, widths[44], Size.Y)) },
				{ "M", new Character(_texture, new IntRect(Size.X * 13, Size.Y * 2, widths[45], Size.Y)) },
				{ "N", new Character(_texture, new IntRect(Size.X * 14, Size.Y * 2, widths[46], Size.Y)) },
				{ "O", new Character(_texture, new IntRect(Size.X * 15, Size.Y * 2, widths[47], Size.Y)) },

				{ "P", new Character(_texture,  new IntRect(Size.X * 0,  Size.Y * 3, widths[48], Size.Y)) },
				{ "Q", new Character(_texture,  new IntRect(Size.X * 1,  Size.Y * 3, widths[49], Size.Y)) },
				{ "R", new Character(_texture,  new IntRect(Size.X * 2,  Size.Y * 3, widths[50], Size.Y)) },
				{ "S", new Character(_texture,  new IntRect(Size.X * 3,  Size.Y * 3, widths[51], Size.Y)) },
				{ "T", new Character(_texture,  new IntRect(Size.X * 4,  Size.Y * 3, widths[52], Size.Y)) },
				{ "U", new Character(_texture,  new IntRect(Size.X * 5,  Size.Y * 3, widths[53], Size.Y)) },
				{ "V", new Character(_texture,  new IntRect(Size.X * 6,  Size.Y * 3, widths[54], Size.Y)) },
				{ "W", new Character(_texture,  new IntRect(Size.X * 7,  Size.Y * 3, widths[55], Size.Y)) },
				{ "X", new Character(_texture,  new IntRect(Size.X * 8,  Size.Y * 3, widths[56], Size.Y)) },
				{ "Y", new Character(_texture,  new IntRect(Size.X * 9,  Size.Y * 3, widths[57], Size.Y)) },
				{ "Z", new Character(_texture,  new IntRect(Size.X * 10, Size.Y * 3, widths[58], Size.Y)) },
				{ "[", new Character(_texture,  new IntRect(Size.X * 11, Size.Y * 3, widths[59], Size.Y)) },
				{ "\\", new Character(_texture, new IntRect(Size.X * 12, Size.Y * 3, widths[60], Size.Y)) },
				{ "]", new Character(_texture,  new IntRect(Size.X * 13, Size.Y * 3, widths[61], Size.Y)) },
				{ "^", new Character(_texture,  new IntRect(Size.X * 14, Size.Y * 3, widths[62], Size.Y)) },
				{ "_", new Character(_texture,  new IntRect(Size.X * 15, Size.Y * 3, widths[63], Size.Y)) },

				{ "`", new Character(_texture, new IntRect(Size.X * 0,  Size.Y * 4, widths[64], Size.Y)) },
				{ "a", new Character(_texture, new IntRect(Size.X * 1,  Size.Y * 4, widths[65], Size.Y)) },
				{ "b", new Character(_texture, new IntRect(Size.X * 2,  Size.Y * 4, widths[66], Size.Y)) },
				{ "c", new Character(_texture, new IntRect(Size.X * 3,  Size.Y * 4, widths[67], Size.Y)) },
				{ "d", new Character(_texture, new IntRect(Size.X * 4,  Size.Y * 4, widths[68], Size.Y)) },
				{ "e", new Character(_texture, new IntRect(Size.X * 5,  Size.Y * 4, widths[69], Size.Y)) },
				{ "f", new Character(_texture, new IntRect(Size.X * 6,  Size.Y * 4, widths[70], Size.Y)) },
				{ "g", new Character(_texture, new IntRect(Size.X * 7,  Size.Y * 4, widths[71], Size.Y)) },
				{ "h", new Character(_texture, new IntRect(Size.X * 8,  Size.Y * 4, widths[72], Size.Y)) },
				{ "i", new Character(_texture, new IntRect(Size.X * 9,  Size.Y * 4, widths[73], Size.Y)) },
				{ "j", new Character(_texture, new IntRect(Size.X * 10, Size.Y * 4, widths[74], Size.Y)) },
				{ "k", new Character(_texture, new IntRect(Size.X * 11, Size.Y * 4, widths[75], Size.Y)) },
				{ "l", new Character(_texture, new IntRect(Size.X * 12, Size.Y * 4, widths[76], Size.Y)) },
				{ "m", new Character(_texture, new IntRect(Size.X * 13, Size.Y * 4, widths[77], Size.Y)) },
				{ "n", new Character(_texture, new IntRect(Size.X * 14, Size.Y * 4, widths[78], Size.Y)) },
				{ "o", new Character(_texture, new IntRect(Size.X * 15, Size.Y * 4, widths[79], Size.Y)) },

				{ "p", new Character(_texture, new IntRect(Size.X * 0,  Size.Y * 5, widths[80], Size.Y)) },
				{ "q", new Character(_texture, new IntRect(Size.X * 1,  Size.Y * 5, widths[81], Size.Y)) },
				{ "r", new Character(_texture, new IntRect(Size.X * 2,  Size.Y * 5, widths[82], Size.Y)) },
				{ "s", new Character(_texture, new IntRect(Size.X * 3,  Size.Y * 5, widths[83], Size.Y)) },
				{ "t", new Character(_texture, new IntRect(Size.X * 4,  Size.Y * 5, widths[84], Size.Y)) },
				{ "u", new Character(_texture, new IntRect(Size.X * 5,  Size.Y * 5, widths[85], Size.Y)) },
				{ "v", new Character(_texture, new IntRect(Size.X * 6,  Size.Y * 5, widths[86], Size.Y)) },
				{ "w", new Character(_texture, new IntRect(Size.X * 7,  Size.Y * 5, widths[87], Size.Y)) },
				{ "x", new Character(_texture, new IntRect(Size.X * 8,  Size.Y * 5, widths[88], Size.Y)) },
				{ "y", new Character(_texture, new IntRect(Size.X * 9,  Size.Y * 5, widths[89], Size.Y)) },
				{ "z", new Character(_texture, new IntRect(Size.X * 10, Size.Y * 5, widths[90], Size.Y)) },
				{ "{", new Character(_texture, new IntRect(Size.X * 11, Size.Y * 5, widths[91], Size.Y)) },
				{ "|", new Character(_texture, new IntRect(Size.X * 12, Size.Y * 5, widths[92], Size.Y)) },
				{ "}", new Character(_texture, new IntRect(Size.X * 13, Size.Y * 5, widths[93], Size.Y)) },
				{ "~", new Character(_texture, new IntRect(Size.X * 14, Size.Y * 5, widths[94], Size.Y)) },
			};*/
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
