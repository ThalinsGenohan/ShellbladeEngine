using System;
using System.Collections.Generic;
using System.Numerics;
using OpenTK.Graphics.OpenGL;
using Shellblade.Graphics3D.GPU;

namespace Shellblade.Graphics3D;

public class DrawList
{
	public List<Model> Models { get; }
	public Camera      Camera { get; }

	private BufferObject  _vbo;
	private BufferObject  _uniform;
	private ShaderProgram _shader;

	public DrawList()
	{
		_vbo     = new BufferObject(BufferTarget.ArrayBuffer,   BufferUsageHint.DynamicDraw);
		_uniform = new BufferObject(BufferTarget.UniformBuffer, BufferUsageHint.DynamicDraw);
		_shader  = new ShaderProgram("shaders/drawList.vert", "shaders/drawList.frag");

		Models = new List<Model>();
		Camera = new Camera(Vector3.Zero, 800, 600);
	}

	internal void UpdateBuffers()
	{
		var vboList     = new List<byte>();
		var uniformList = new List<byte>();
		for (uint i = 0; i < Models.Count; i++)
		{
			Model model = Models[(int)i];
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

	internal void CreateArrayTexture()
	{
		var texels = new byte[255 * 255 * 255];
		for (var i = 0; i < 255 * 255 * 255; i++)
		{
			texels[i] = 0xFF;
		}

		GL.TexStorage3D(TextureTarget3d.Texture2DArray, 1, SizedInternalFormat.Rgba8, 255, 255, 255);
		GL.TexSubImage3D(TextureTarget.Texture2DArray,
		                 0,
		                 0,
		                 0,
		                 0,
		                 255,
		                 255,
		                 255,
		                 PixelFormat.Rgba,
		                 PixelType.UnsignedByte,
		                 texels
		);
	}

	internal void Render()
	{
		UpdateBuffers();
		_shader.SetUniform("uView",       Camera.ViewMatrix);
		_shader.SetUniform("uProjection", Camera.ProjectionMatrix);
		//_shader.SetUniform("uViewPos",    Camera.Position);
		foreach (Model model in Models)
		{
			_shader.SetUniform("uModelPos", model.Transform.ViewMatrix);
			GL.DrawArrays(PrimitiveType.Triangles, 0, model.Vertices.Count);
		}
	}
}
