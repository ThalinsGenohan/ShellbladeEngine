using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace Shellblade.Graphics3D.GPU;

internal class VertexArrayObject<TVertexType> : IDisposable
where TVertexType : unmanaged
{
	private readonly int _handle;

	public VertexArrayObject(BufferObject<TVertexType> vbo)
	{
		_handle = GL.GenVertexArray();
		Bind();
		vbo.Bind();
	}

	public void VertexAttribPointer(int index, int count, VertexAttribPointerType type, int vertexSize, int offset)
	{
		Bind();

		int tVertexSize = Marshal.SizeOf<TVertexType>();
		unsafe
		{
			GL.VertexAttribPointer(
				index,
				count,
				type,
				false,
				vertexSize * tVertexSize,
				new IntPtr((void*)(offset * tVertexSize))
			);
		}
		GL.EnableVertexAttribArray(index);
	}

	public void Bind()
	{
		GL.BindVertexArray(_handle);
	}

	public void Dispose()
	{
		GL.DeleteVertexArray(_handle);
	}
}
