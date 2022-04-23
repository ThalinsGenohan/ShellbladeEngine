using System;
using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics
{
	public class TileMap
	{
		public uint    Width         { get; }
		public uint    Height        { get; }
		public byte[] Tiles         { get; }
		public int    TileSetHandle { get; set; }

		public ref byte this[uint x, uint y] => ref Tiles[x + y * Width];

		public TileMap(uint width, uint height)
		{
			Width  = width;
			Height = height;
			Tiles  = new byte[width * height];
		}

		/*private readonly VertexArray _vertices = new(PrimitiveType.Quads);
		private          Texture     _tileset;

		public void Load(string tileset, Vector2u tileSize, int[] tiles, uint width, uint height)
		{
			_tileset = new Texture(tileset);

			_vertices.PrimitiveType = PrimitiveType.Quads;
			_vertices.Resize(width * height * 4);

			for (uint i = 0; i < width; ++i)
			{
				for (uint j = 0; j < height; ++j)
				{
					int tileNumber = tiles[i + j * width];

					long tu = tileNumber % (_tileset.Size.X / tileSize.X);
					long tv = tileNumber / (_tileset.Size.X / tileSize.X);

					uint q = (i + j * width) * 4;

					_vertices.Append(new Vertex(new Vector2f(i * tileSize.X,  j * tileSize.Y),
					                            new Vector2f(tu * tileSize.X, tv * tileSize.Y)));
					_vertices.Append(new Vertex(new Vector2f((i + 1) * tileSize.X,  j * tileSize.Y),
					                            new Vector2f((tu + 1) * tileSize.X, tv * tileSize.Y)));
					_vertices.Append(new Vertex(new Vector2f((i + 1) * tileSize.X,  (j + 1) * tileSize.Y),
					                            new Vector2f((tu + 1) * tileSize.X, (tv + 1) * tileSize.Y)));
					_vertices.Append(new Vertex(new Vector2f(i * tileSize.X,  (j + 1) * tileSize.Y),
					                            new Vector2f(tu * tileSize.X, (tv + 1) * tileSize.Y)));
				}
			}
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform *= Transform;
			states.Texture   =  _tileset;
			target.Draw(_vertices, states);
		}*/
	}
}
