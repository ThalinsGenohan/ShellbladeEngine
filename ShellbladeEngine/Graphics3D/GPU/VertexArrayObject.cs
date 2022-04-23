using System;
using OpenTK.Graphics.OpenGL;

namespace Shellblade.Graphics3D.GPU;

internal class VertexArrayObject : IDisposable
{
	private readonly int _handle;

	public VertexArrayObject(BufferObject vbo)
	{
		_handle = GL.GenVertexArray();
		Bind();
		vbo.Bind();
	}

	public void VertexAttribPointer(int index, int count, VertexAttribPointerType type, int vertexSize, int offset)
	{
		Bind();
		GL.VertexAttribPointer(index, count, type, false, vertexSize, offset);
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
