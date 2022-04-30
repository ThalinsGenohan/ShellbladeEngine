
using System;
using System.Drawing;
using System.Numerics;

namespace Shellblade.Graphics3D
{
	internal class Vertex
	{
		public Vector3 Position  { get; set; }
		public Vector2 TexCoords { get; set; }
		public Color   Color     { get; set; }

		public Vertex()
		{
			Position = Vector3.Zero;
			TexCoords = Vector2.Zero;
			Color = Color.White;
		}

		public Vertex(Vector3 position, Vector2 texCoords)
		{
			Position  = position;
			TexCoords = texCoords;
			Color     = Color.Magenta;
		}

		public Vertex(Vector3 position, Vector2 texCoords, Color color)
		{
			Position  = position;
			TexCoords = texCoords;
			Color     = color;
		}

		public byte[] GetGPUData()
		{
			float[] floats    = { Position.X, Position.Y, Position.Z, TexCoords.X, TexCoords.Y };
			int     floatSize = floats.Length * sizeof(float);
			var     bytes     = new byte[floatSize + 4];
			Buffer.BlockCopy(floats, 0, bytes, 0, floatSize);
			bytes[floatSize]     = Color.R;
			bytes[floatSize + 1] = Color.G;
			bytes[floatSize + 2] = Color.B;
			bytes[floatSize + 3] = Color.A;
			return bytes;
		}
	}
}
