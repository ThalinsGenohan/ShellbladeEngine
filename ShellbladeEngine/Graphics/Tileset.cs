using System.IO;
using SFML.System;
using YamlDotNet.Serialization;

namespace Shellblade.Graphics
{
	public class Tileset
	{
		public uint     Columns   { get; private init; }
		public string   ImagePath { get; private init; }
		public Vector2u ImageSize { get; private init; }
		public int      Margin    { get; private init; }
		public string   Name      { get; private init; }
		public int      Spacing   { get; private init; }
		public uint     TileCount { get; private init; }
		public Vector2u TileSize  { get; private init; }

		public static Tileset LoadFromFile(string filename)
		{
			var import = new DeserializerBuilder().Build().Deserialize<TilesetImport>(File.ReadAllText(filename));

			var tileset = new Tileset
			{
				Columns   = import.columns,
				ImagePath = import.image,
				ImageSize = new Vector2u(import.imagewidth, import.imageheight),
				Margin    = import.margin,
				Name      = import.name,
				Spacing   = import.spacing,
				TileCount = import.tilecount,
				TileSize  = new Vector2u(import.tilewidth, import.tileheight),
			};

			return tileset;
		}

		private struct TilesetImport
		{
			// ReSharper disable InconsistentNaming
			public uint   columns      { get; private set; }
			public string image        { get; private set; }
			public uint   imageheight  { get; private set; }
			public uint   imagewidth   { get; private set; }
			public int    margin       { get; private set; }
			public string name         { get; private set; }
			public int    spacing      { get; private set; }
			public uint   tilecount    { get; private set; }
			public string tiledversion { get; private set; }
			public uint   tileheight   { get; private set; }
			public uint   tilewidth    { get; private set; }
			public string type         { get; private set; }
			public string version      { get; private set; }
			// ReSharper restore InconsistentNaming
		}
	}
}
