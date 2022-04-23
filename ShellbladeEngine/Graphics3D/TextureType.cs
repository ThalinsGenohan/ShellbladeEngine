using OpenTK.Graphics.OpenGL;

namespace Shellblade.Graphics3D;

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
