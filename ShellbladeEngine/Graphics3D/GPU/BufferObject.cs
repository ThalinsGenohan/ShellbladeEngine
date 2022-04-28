using System;
using OpenTK.Graphics.OpenGL;

namespace Shellblade.Graphics3D.GPU;

internal class BufferObject : IDisposable
{
	public BufferTarget BufferTarget { get; }

	private readonly int             _handle;
	private readonly BufferUsageHint _usageHint;

	public BufferObject(BufferTarget bufferTarget, BufferUsageHint usageHint)
	{
		BufferTarget = bufferTarget;
		_usageHint    = usageHint;
		_handle       = GL.GenBuffer();
	}

	public BufferObject(BufferTarget bufferTarget, BufferUsageHint usageHint, byte[] data)
	{
		BufferTarget = bufferTarget;
		_usageHint    = usageHint;
		_handle       = GL.GenBuffer();
		SetData(data);
	}

	public void Bind()
	{
		GL.BindBuffer(BufferTarget, _handle);
	}

	public void Draw(int count, PrimitiveType type = PrimitiveType.Triangles)
	{
		GL.DrawElements(type, count, DrawElementsType.UnsignedInt, _handle);
	}

	public void Dispose()
	{
		GL.DeleteBuffer(_handle);
	}

	public void SetData(byte[] data)
	{
		Bind();
		GL.BufferData(BufferTarget, data.Length, data, _usageHint);
	}
}
