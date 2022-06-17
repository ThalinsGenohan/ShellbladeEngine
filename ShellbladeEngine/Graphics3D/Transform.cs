using System;
using System.Collections.Generic;
using System.Numerics;

namespace Shellblade.Graphics3D
{
	public class Transform
	{
		public Vector3    Position { get; set; } = Vector3.Zero;
		public float      Scale    { get; set; } = 1f;
		public Quaternion Rotation { get; set; } = Quaternion.Identity;

		public Matrix4x4 ViewMatrix => Matrix4x4.Identity * Matrix4x4.CreateFromQuaternion(Rotation) *
		                               Matrix4x4.CreateScale(Scale) * Matrix4x4.CreateTranslation(Position);

		public byte[] GetMatrixBytes()
		{
			var list = new List<byte>();
			list.AddRange(BitConverter.GetBytes(ViewMatrix.M11));
			list.AddRange(BitConverter.GetBytes(ViewMatrix.M21));
			list.AddRange(BitConverter.GetBytes(ViewMatrix.M31));
			list.AddRange(BitConverter.GetBytes(ViewMatrix.M41));
			list.AddRange(BitConverter.GetBytes(ViewMatrix.M12));
			list.AddRange(BitConverter.GetBytes(ViewMatrix.M22));
			list.AddRange(BitConverter.GetBytes(ViewMatrix.M32));
			list.AddRange(BitConverter.GetBytes(ViewMatrix.M42));
			list.AddRange(BitConverter.GetBytes(ViewMatrix.M13));
			list.AddRange(BitConverter.GetBytes(ViewMatrix.M23));
			list.AddRange(BitConverter.GetBytes(ViewMatrix.M33));
			list.AddRange(BitConverter.GetBytes(ViewMatrix.M43));
			list.AddRange(BitConverter.GetBytes(ViewMatrix.M14));
			list.AddRange(BitConverter.GetBytes(ViewMatrix.M24));
			list.AddRange(BitConverter.GetBytes(ViewMatrix.M34));
			list.AddRange(BitConverter.GetBytes(ViewMatrix.M44));
			return list.ToArray();
		}
	}
}
