using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Shellblade.Graphics
{
	internal class TileMapRenderer : IDisposable
	{
		public const int   TileSize       = 32;
		public const float TileTexSize    = 1f / 16f;
		public const float TileTexPadding = 1f / 256f;

		public  Vector2 Center  { get; set; }
		public  TileMap TileMap { get; private set; }

		private int _shaderHandle;
		private int _vboHandle;
		private int _vaoHandle;
		private int _backBufferWidth;
		private int _backBufferHeight;

		public void Initialize(TileMap tileMap)
		{
			GL.ClearColor(Color.DimGray);
			GL.ClipControl(ClipOrigin.UpperLeft, ClipDepthMode.NegativeOneToOne);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

			TileMap = tileMap;

			CreateShader();
			GenerateVertexBufferObject();
			GenerateVertexArrayObject();
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			GL.BindTexture(TextureTarget.Texture2D, TileMap.TileSetHandle);
			GL.BindVertexArray(_vaoHandle);

			Matrix4 projection = Matrix4.CreateTranslation(-Center.X, -Center.Y, 0) *
			                     Matrix4.CreateScale(TileSize,              TileSize,               1) *
			                     Matrix4.CreateScale(2f / _backBufferWidth, 2f / _backBufferHeight, 1);

			GL.UniformMatrix4(GL.GetUniformLocation(_shaderHandle, "projection"), false, ref projection);
			GL.Uniform2(GL.GetUniformLocation(_shaderHandle, "mapSize"), TileMap.Width, TileMap.Height);

			GL.UseProgram(_shaderHandle);
			GL.DrawArrays(PrimitiveType.Points, 0, TileMap.Tiles.Length);
		}

		private void CreateShader()
		{
			var assembly = Assembly.GetExecutingAssembly();

			const string vertFilePath = "OpenGLTileMapDemos.Resources.GeometryRenderer.vert";
			string       vertSource   = new StreamReader(assembly.GetManifestResourceStream(vertFilePath)).ReadToEnd();

			const string geomFilePath = "OpenGLTileMapDemos.Resources.GeometryRenderer.geom";
			string       geomSource   = new StreamReader(assembly.GetManifestResourceStream(geomFilePath)).ReadToEnd();

			const string fragFilePath = "OpenGLTileMapDemos.Resources.GeometryRenderer.frag";
			string       fragSource   = new StreamReader(assembly.GetManifestResourceStream(fragFilePath)).ReadToEnd();

			int vertHandle = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertHandle, vertSource);
			GL.CompileShader(vertHandle);

			int geomHandle = GL.CreateShader(ShaderType.GeometryShader);
			GL.ShaderSource(geomHandle, geomSource);
			GL.CompileShader(geomHandle);

			int fragHandle = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragHandle, fragSource);
			GL.CompileShader(fragHandle);

			_shaderHandle = GL.CreateProgram();
			GL.AttachShader(_shaderHandle, vertHandle);
			GL.AttachShader(_shaderHandle, geomHandle);
			GL.AttachShader(_shaderHandle, fragHandle);
			GL.LinkProgram(_shaderHandle);

			GL.DetachShader(_shaderHandle, vertHandle);
			GL.DeleteShader(vertHandle);

			GL.DetachShader(_shaderHandle, geomHandle);
			GL.DeleteShader(geomHandle);

			GL.DetachShader(_shaderHandle, fragHandle);
			GL.DeleteShader(fragHandle);
		}

		private void GenerateVertexBufferObject()
		{
			_vboHandle = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vboHandle);
			GL.BufferData(BufferTarget.ArrayBuffer,
			              TileMap.Tiles.Length * sizeof(byte),
			              TileMap.Tiles,
			              BufferUsageHint.StaticDraw);
		}

		public void GenerateVertexArrayObject()
		{
			_vaoHandle = GL.GenVertexArray();
			GL.BindVertexArray(_vaoHandle);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vboHandle);

			GL.EnableVertexAttribArray(0);
			GL.VertexAttribIPointer(0, 1, VertexAttribIntegerType.UnsignedByte, sizeof(byte), IntPtr.Zero);
		}

		public void Dispose()
		{
			GL.DeleteVertexArray(_vaoHandle);
			_vaoHandle = 0;

			GL.DeleteBuffer(_vboHandle);
			_vboHandle = 0;

			GL.DeleteProgram(_shaderHandle);
		}

		public void OnBackBufferResized(int width, int height)
		{
			_backBufferWidth = width;
			_backBufferHeight = height;
			GL.Viewport(0, 0, width, height);
		}
	}
}
