using System;
using OpenTK.Graphics.OpenGL;

namespace Shellblade.Graphics3D.GPU;

internal class BufferObject : IDisposable
{
	private readonly int             _handle;
	private readonly BufferTarget    _bufferTarget;
	private readonly BufferUsageHint _usageHint;

	public BufferObject(BufferTarget bufferTarget, BufferUsageHint usageHint)
	{
		_bufferTarget = bufferTarget;
		_usageHint    = usageHint;
		_handle       = GL.GenBuffer();
	}

	public BufferObject(BufferTarget bufferTarget, BufferUsageHint usageHint, byte[] data)
	{
		_bufferTarget = bufferTarget;
		_usageHint    = usageHint;
		_handle       = GL.GenBuffer();
		SetData(data);
	}

	public void Bind()
	{
		GL.BindBuffer(_bufferTarget, _handle);
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
		GL.BufferData(_bufferTarget, data.Length, data, _usageHint);
	}
}
