using System;
using System.Collections.Generic;
using System.Numerics;
using OpenTK.Graphics.OpenGL;
using Shellblade.Graphics3D.GPU;

namespace Shellblade.Graphics3D;

internal class DrawList
{
	public List<Model> Models { get; }

	private BufferObject  _vbo;
	private BufferObject  _uniform;
	private ShaderProgram _shader;

	public DrawList()
	{
		_vbo     = new BufferObject(BufferTarget.ArrayBuffer,   BufferUsageHint.DynamicDraw);
		_uniform = new BufferObject(BufferTarget.UniformBuffer, BufferUsageHint.DynamicDraw);
		_shader  = new ShaderProgram();
	}

	internal void UpdateBuffers()
	{
		var vboList     = new List<byte>();
		var uniformList = new List<byte>();
		for (ushort i = 0; i < Models.Count; i++)
		{
			Model model = Models[i];
			foreach (Vertex vert in model.Vertices)
			{
				vboList.AddRange(vert.GetGPUData());
				vboList.AddRange(BitConverter.GetBytes(i));
			}
			uniformList.AddRange(model.Transform.GetMatrixBytes());
		}

		_vbo.SetData(vboList.ToArray());
		_uniform.SetData(uniformList.ToArray());
	}
}
