﻿using System.Numerics;

namespace Shellblade.Graphics3D
{
	internal class Transform
	{
		public Vector3    Position { get; set; } = Vector3.Zero;
		public float      Scale    { get; set; } = 1f;
		public Quaternion Rotation { get; set; } = Quaternion.Identity;

		public Matrix4x4 ViewMatrix => Matrix4x4.Identity * Matrix4x4.CreateFromQuaternion(Rotation) *
		                               Matrix4x4.CreateScale(Scale) * Matrix4x4.CreateTranslation(Position);
	}
}
