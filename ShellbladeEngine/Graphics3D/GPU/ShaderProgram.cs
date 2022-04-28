using System;
using System.IO;
using System.Numerics;
using OpenTK.Graphics.OpenGL;

namespace Shellblade.Graphics3D.GPU;

internal class ShaderProgram : IDisposable
{
	private readonly int _handle;

	public ShaderProgram(string vertexPath, string fragmentPath)
	{
		int vertex   = LoadShader(ShaderType.VertexShader,   "assets/" + vertexPath);
		int fragment = LoadShader(ShaderType.FragmentShader, "assets/" + fragmentPath);

		_handle = GL.CreateProgram();
		GL.AttachShader(_handle, vertex);
		GL.AttachShader(_handle, fragment);
		GL.LinkProgram(_handle);

		GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out int status);
		if (status == 0)
		{
			throw new Exception($"Program failed to link with error: {GL.GetProgramInfoLog(_handle)}");
		}

		GL.DetachShader(_handle, vertex);
		GL.DetachShader(_handle, fragment);
		GL.DeleteShader(vertex);
		GL.DeleteShader(fragment);
	}

	public void Use()
	{
		GL.UseProgram(_handle);
	}

	public void SetUniform(string name, int value)
	{
		int location = GL.GetUniformLocation(_handle, name);
		if (location == -1)
		{
			throw new Exception($"{name} uniform not found on shader.");
		}

		GL.Uniform1(location, value);
	}

	public void SetUniform(string name, float value)
	{
		int location = GL.GetUniformLocation(_handle, name);
		if (location == -1)
		{
			throw new Exception($"{name} uniform not found on shader.");
		}

		GL.Uniform1(location, value);
	}

	public void SetUniform(string name, Matrix4x4 value)
	{
		int location = GL.GetUniformLocation(_handle, name);
		if (location == -1)
		{
			throw new Exception($"{name} uniform not found on shader.");
		}

		unsafe
		{
			GL.UniformMatrix4(location, 1, false, (float*)&value);
		}
	}

	public void SetUniform(string name, Vector2 value)
	{
		int location = GL.GetUniformLocation(_handle, name);
		if (location == -1)
		{
			throw new Exception($"{name} uniform not found on shader.");
		}

		unsafe
		{
			GL.Uniform2(location, 1, (float*)&value);
		}
	}

	public void SetUniform(string name, Vector3 value)
	{
		int location = GL.GetUniformLocation(_handle, name);
		if (location == -1)
		{
			throw new Exception($"{name} uniform not found on shader.");
		}

		unsafe
		{
			GL.Uniform3(location, 1, (float*)&value);
		}
	}

	public void SetUniform(string name, Matrix4x4[] value)
	{
		int location = GL.GetUniformLocation(_handle, name);
		if (location == -1)
		{
			throw new Exception($"{name} uniform not found on shader.");
		}

		unsafe
		{
			fixed (Matrix4x4* addr = &value[0])
				GL.UniformMatrix4(location, value.Length, false, (float*)addr);
		}
	}

	public void Dispose()
	{
		GL.DeleteProgram(_handle);
	}

	private static int LoadShader(ShaderType type, string path)
	{
		string src    = File.ReadAllText(path);
		int    handle = GL.CreateShader(type);
		GL.ShaderSource(handle, src);
		GL.CompileShader(handle);

		string infoLog = GL.GetShaderInfoLog(handle);
		if (!string.IsNullOrWhiteSpace(infoLog))
		{
			throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
		}
		return handle;
	}
}
