using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shellblade.Graphics3D.GPU;

namespace Shellblade.Graphics3D;

internal class DrawList
{
	public List<Model> Models { get; }

	private BufferObject      _modelsBufferObject;
	private VertexArrayObject _vao;

	internal void Stitch()
	{
		
	}
}
