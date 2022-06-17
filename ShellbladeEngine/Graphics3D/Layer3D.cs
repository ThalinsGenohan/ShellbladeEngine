using System.Collections.Generic;
using Shellblade.Graphics;

namespace Shellblade.Graphics3D
{
	public class Layer3D : LayerBase
	{
		public Dictionary<string, DrawList> DrawLists { get; private set; }
	}
}
