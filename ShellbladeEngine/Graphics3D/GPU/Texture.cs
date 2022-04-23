using System;
using OpenTK.Graphics.OpenGL;
using SFML.Graphics;

namespace Shellblade.Graphics3D.GPU;

internal class Texture : IDisposable
{
	private int _handle;

	public Texture(string path,
	               TextureType? textureType = null,
	               TextureWrapMode wrap = TextureWrapMode.Repeat,
	               TextureMagFilter filter = TextureMagFilter.Nearest)
	{
		var img = new Image("assets/" + path);
		img.FlipVertically();

		Load(img.Pixels, img.Size.X, img.Size.Y, textureType ?? TextureType.Rgba32, wrap, filter);

		img.Dispose();
	}

	private void Load<T>(T[] data,
	                     uint width,
	                     uint height,
	                     TextureType textureType,
	                     TextureWrapMode wrap,
	                     TextureMagFilter filter)
		where T : unmanaged
	{
		_handle = GL.GenTexture();
		Bind(TextureUnit.Texture0);

		GL.TexImage2D<T>(
			TextureTarget.Texture2D,
			0,
			textureType.PixelInternalFormat,
			(int)width,
			(int)height,
			0,
			textureType.PixelFormat,
			textureType.PixelType,
			data
		);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,     (int)wrap);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,     (int)wrap);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)filter);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)filter);

		GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
	}

	public void Bind(TextureUnit textureSlot)
	{
		GL.ActiveTexture(textureSlot);
		GL.BindTexture(TextureTarget.Texture2D, _handle);
		GL.Enable(EnableCap.Texture2D);
	}

	public void Attach(FramebufferAttachment attachment)
	{
		GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachment, TextureTarget.Texture2D, _handle, 0);
	}

	public void Dispose()
	{
		GL.DeleteTexture(_handle);
	}

	internal struct TextureType
	{
		public PixelInternalFormat PixelInternalFormat { get; set; }
		public PixelFormat         PixelFormat         { get; set; }
		public PixelType           PixelType           { get; set; }

		public static TextureType Rgba32 = new()
		{
			PixelInternalFormat = PixelInternalFormat.Rgba,
			PixelFormat         = PixelFormat.Rgba,
			PixelType           = PixelType.UnsignedByte,
		};

		public static TextureType Rgb24 = new()
		{
			PixelInternalFormat = PixelInternalFormat.Rgb,
			PixelFormat         = PixelFormat.Rgb,
			PixelType           = PixelType.UnsignedByte,
		};

		public static TextureType Depth24 = new()
		{
			PixelInternalFormat = PixelInternalFormat.DepthComponent24,
			PixelFormat         = PixelFormat.DepthComponent,
			PixelType           = PixelType.UnsignedInt,
		};
	}
}

