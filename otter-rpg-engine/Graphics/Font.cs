using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SFML.Graphics;
using SFML.System;
using YamlDotNet.Serialization;

namespace Shellblade.Graphics
{
	public class Font
	{
		private readonly Texture _texture;

		private int _spaceSize;

		public Dictionary<char, Character> Characters { get; } = new Dictionary<char, Character>();

		public bool     VariableWidth  { get; set; } = true;
		public int      TrackingOffset { get; set; } = 1;
		public Vector2i Size           { get; set; } = new Vector2i(8, 8);

		public int SpaceSize
		{
			get => VariableWidth ? _spaceSize : Size.X;
			set
			{
				_spaceSize            = value;
				//Characters[' '].Width = value;
			}
		}

		private struct FontConfig
		{
			public int                   TrackingOffset;
			public Dictionary<char, int> WidthOverrides;
		}

		public Font(string name)
		{
			_texture = new Texture($"{name}.png");

			if (!File.Exists($"{name}.yaml"))
			{
				Console.WriteLine($"Configuration file not detected for '{name}' font! Generating...");

				var fc = new FontConfig
				{
					TrackingOffset = 1,
					WidthOverrides = new Dictionary<char, int>
					{
						{ ' ', 3 },
					},
				};
				ISerializer serializer = new SerializerBuilder().Build();
				string      yaml       = serializer.Serialize(fc);

				File.WriteAllText($"{name}.yaml", yaml);
			}

			IDeserializer deserializer = new DeserializerBuilder().Build();
			var           config       = deserializer.Deserialize<FontConfig>(File.ReadAllText($"{name}.yaml"));

			TrackingOffset = config.TrackingOffset;

			int[] widths = GetSizes(name);
			for (var c = ' '; c <= '~'; c++)
			{
				int w = config.WidthOverrides.ContainsKey(c)
					        ? config.WidthOverrides[c]
					        : widths[c - ' '];
				Characters.Add(c, new Character(_texture, new IntRect(Size.X * ((c - ' ') % 16), Size.Y * ((c - ' ') / 16), Size.X, Size.Y), w));
			}

			SpaceSize = Characters[' '].Width;

			Console.WriteLine("Font loaded:\n" +
			                  $"\tName: {name}\n" +
			                  $"\tTracking Offset: {TrackingOffset}\n" +
			                  $"\tSpace Size: {SpaceSize}\n" +
			                  $"\tOverrides");
		}

		private static int[] GetSizes(string name)
		{
			const int charSize = 8;
			const int cols     = 16;
			const int rows     = 6;

			var fontImage = new Image($"{name}.png");
			var widths    = new int[rows][];

			for (var y = 0; y < rows; y++)
			{
				widths[y] = new int[cols];
				for (var x = 0; x < cols; x++)
				{
					var w = 0;
					for (var u = 0; u < charSize; u++)
					for (var v = 0; v < charSize; v++)
						if (fontImage.GetPixel((uint)(x * charSize + u), (uint)(y * charSize + v)).A > 0)
							w = u + 1;

					widths[y][x] = w;
				}
			}

			widths[0][0] = 3;

			var ret = new int[cols * rows];
			for (var i = 0; i < ret.Length; i++)
				ret[i] = widths[i / cols][i % cols];

			return ret;
		}

		public class Character
		{
			private readonly Texture _texture;
			private readonly IntRect _rect;

			public int Width { get; set; }

			public Sprite Sprite => new Sprite(_texture, _rect);

			public Character(Texture texture, IntRect rect, int width)
			{
				_texture = texture;
				_rect    = rect;
				Width    = width;
			}
		}
	}
}
