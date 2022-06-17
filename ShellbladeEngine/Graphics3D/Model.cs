using System.Collections.Generic;
using SFML.Graphics;

namespace Shellblade.Graphics3D
{
	public class Model
	{
		public Transform Transform { get; set; }

		public Shader Shader
		{
			get => _shader;
			set
			{
				_shader = value;
			}
		}

		internal Layer3D      Layer    { get; set; }
		internal List<Vertex> Vertices { get; set; }

		private Shader _shader;

		public Model()
		{
			Transform = new Transform();
			Vertices  = new List<Vertex>();
		}
	}
}
