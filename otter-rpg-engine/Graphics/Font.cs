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

		public int      TrackingOffset { get; set; }
		public Vector2i Size           { get; set; } = new Vector2i(8, 8);

		public int SpaceSize
		{
			get => _spaceSize;
			set
			{
				_spaceSize            = value;
				Characters[' '].Width = value;
			}
		}

		private class FontConfig : Config
		{
			public int                   TrackingOffset;
			public Dictionary<char, int> WidthOverrides;

			public override void Load(string filename)
			{
				var config = _deserializer.Deserialize<FontConfig>(File.ReadAllText(filename));
				TrackingOffset = config.TrackingOffset;
				WidthOverrides = config.WidthOverrides;
			}

			public override void Save(string filename)
			{
				File.WriteAllText(filename, _serializer.Serialize(this));
			}
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
				fc.Save($"{name}.yaml");
			}

			var config = new FontConfig();
			config.Load($"{name}.yaml");

			TrackingOffset = config.TrackingOffset;

			int[] widths = GetSizes(name);
			for (var c = ' '; c <= '~'; c++)
			{
				int w = config.WidthOverrides.ContainsKey(c)
					        ? config.WidthOverrides[c]
					        : widths[c - ' '];

				int left = Size.X * ((c - ' ') % 16);
				int top = Size.Y * ((c - ' ') / 16);

				Characters.Add(c, new Character(_texture, new IntRect(left, top, Size.X, Size.Y), w));
			}

			SpaceSize = Characters[' '].Width;

			Console.WriteLine("Font loaded:\n" +
			                  $"\tName: {name}\n" +
			                  $"\tTracking Offset: {TrackingOffset}\n" +
			                  $"\tSpace Size: {SpaceSize}\n" +
			                  $"\tOverrides: {config.WidthOverrides.Keys.Aggregate("", (current, key) => current + $"'{key}' ")}");
		}

		private int[] GetSizes(string name)
		{
			const int cols     = 16;
			const int rows     = 6;

			var fontImage = new Image($"{name}.png");
			Size  = new Vector2i((int)fontImage.Size.X / cols, (int)fontImage.Size.Y / rows);

			var widths = new int[rows][];
			for (var y = 0; y < rows; y++)
			{
				widths[y] = new int[cols];
				for (var x = 0; x < cols; x++)
				{
					var w = 0;
					for (var u = 0; u < Size.X; u++)
					for (var v = 0; v < Size.Y; v++)
						if (fontImage.GetPixel((uint)(x * Size.X + u), (uint)(y * Size.Y + v)).A > 0)
						{
							w = u + 1;
							break;
						}

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
