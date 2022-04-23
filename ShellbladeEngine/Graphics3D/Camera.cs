using System;
using System.Numerics;
using OpenTK.Mathematics;
using Vector3 = System.Numerics.Vector3;

namespace Shellblade.Graphics3D;

public class Camera
{
	public float     AspectRatio => Width / Height;
	public Matrix4x4 ViewMatrix  => Matrix4x4.CreateLookAt(Position, Position + Front, Up);

	public Matrix4x4 ProjectionMatrix =>
		Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(_fieldOfView), AspectRatio, 0.1f, 100f);

	public Vector3 Position { get; set; }
	public float   Width    { get; set; }
	public float   Height   { get; set; }

	public Vector3 Front    { get; private set; }
	public Vector3 Up       { get; private set; }
	public Vector3 Right    { get; private set; }

	public float Yaw
	{
		get => _yaw;
		private set => _yaw = value % 360f;
	}

	public float Pitch
	{
		get => _pitch;
		private set => _pitch = Math.Clamp(value, -179.99f, 180f);
	}

	public float Roll
	{
		get => _roll;
		private set => _roll = value % 360f;
	}

	private float _fieldOfView  = 45f;
	private float _yaw   = -90f;
	private float _pitch = 0f;
	private float _roll  = 0f;

	public Camera(Vector3 position, float width, float height)
	{
		Position = position;
		Width    = width;
		Height   = height;
		UpdateDirections();
	}

	public void MoveRelative(float forward, float right, float up)
	{
		Position += forward * Front;
		Position += right * Right;
		Position += up * Up;
	}

	public void ModifyFoV(float fovOffset)
	{
		_fieldOfView = Math.Clamp(_fieldOfView - fovOffset, 1f, 45f);
	}

	public void ModifyYaw(float yawOffset)
	{
		Yaw += yawOffset;
		UpdateDirections();
	}

	public void ModifyPitch(float pitchOffset)
	{
		Pitch += pitchOffset;
		UpdateDirections();
	}

	public void ModifyRoll(float rollOffset)
	{
		Roll += rollOffset;
		UpdateDirections();
	}

	private void UpdateDirections()
	{
		float yawRads   = MathHelper.DegreesToRadians(Yaw);
		float pitchRads = MathHelper.DegreesToRadians(Pitch);
		float rollRads  = MathHelper.DegreesToRadians(Roll);

		float cosYaw   = MathF.Cos(yawRads);
		float sinYaw   = MathF.Sin(yawRads);
		float cosPitch = MathF.Cos(pitchRads);
		float sinPitch = MathF.Sin(pitchRads);
		float cosRoll = MathF.Cos(rollRads);
		float sinRoll = MathF.Sin(rollRads);

		var front = new Vector3(
			cosYaw * cosPitch,
			sinPitch,
			sinYaw * cosPitch
		);
		Front = Vector3.Normalize(front);

		var up = new Vector3(
			sinYaw * sinRoll + cosYaw * cosRoll * sinPitch,
			cosRoll * cosPitch,
			cosYaw * sinRoll - sinYaw * cosRoll * sinPitch
		);
		Up = Vector3.Normalize(up);

		Right = Vector3.Normalize(Vector3.Cross(Front, Up));
	}
}
