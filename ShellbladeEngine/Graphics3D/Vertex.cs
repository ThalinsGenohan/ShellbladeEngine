
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Shellblade.Graphics3D
{
	internal class Vertex
	{
		public Vector3 Position  { get; set; } = Vector3.Zero;
		public Vector2 TexCoords { get; set; } = Vector2.Zero;
		public Color   Color     { get; set; } = Color.White;

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
