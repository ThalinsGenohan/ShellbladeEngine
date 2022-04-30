using System.Collections.Generic;

namespace Shellblade.Graphics3D
{
	internal class Model
	{
		public Transform    Transform { get; set; }
		public List<Vertex> Vertices  { get; set; }

		public Model()
		{
			Transform = new Transform();
			Vertices  = new List<Vertex>();
		}
	}
}
