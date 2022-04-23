using System;
using OpenTK.Graphics.OpenGL;

namespace Shellblade.Graphics3D;

internal class BufferObject<TDataType> : IDisposable
	where TDataType : unmanaged
{
	private readonly int          _handle;
	private readonly BufferTarget _bufferType;

	public BufferObject(IntPtr data, int size, BufferTarget bufferType)
	{
		_bufferType = bufferType;
		_handle     = GL.GenBuffer();
		Bind();
		GL.BufferData(bufferType, size, data, BufferUsageHint.StaticDraw);
	}

	public void Bind()
	{
		GL.BindBuffer(_bufferType, _handle);
	}

	public void Draw(int count, PrimitiveType type = PrimitiveType.Triangles)
	{
		GL.DrawElements(type, count, DrawElementsType.UnsignedInt, _handle);
	}

	public void Dispose()
	{
		GL.DeleteBuffer(_handle);
	}
}
