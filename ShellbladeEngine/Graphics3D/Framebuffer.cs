using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace Shellblade.Graphics3D;

internal class Framebuffer : IDisposable
{
	public readonly int _handle;

	private readonly Dictionary<string, Texture> _textures = new();

	public Framebuffer()
	{
		_handle = GL.GenFramebuffer();
		Bind();
	}

	public void AddTexture(string name, FramebufferAttachment attachment, Texture texture)
	{
		_textures[name] = texture;
		texture.Attach(attachment);
	}

	public void Complete()
	{
		GL.BindTexture(TextureTarget.Texture2D, 0);

		if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
		{
			throw new Exception("Framebuffer is not complete!");
		}

		GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
	}

	public Texture GetTexture(string name)
	{
		return _textures[name];
	}

	public void Bind()
	{
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, _handle);
	}

	public void BindTexture(string name, TextureUnit textureSlot)
	{
		GetTexture(name).Bind(textureSlot);
	}

	public void DisposeTextures()
	{
		foreach (Texture texture in _textures.Values)
		{
			texture.Dispose();
		}
	}

	public void Dispose()
	{
		GL.DeleteFramebuffer(_handle);
	}
}
