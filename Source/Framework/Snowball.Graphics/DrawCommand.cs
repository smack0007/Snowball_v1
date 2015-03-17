using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.Graphics
{
	public class DrawCommand
	{
		internal bool IsFree { get; set; }

		public VertexBuffer VertexBuffer { get; set; }

		public Effect Effect { get; set; }

		public IList<Texture> Textures { get; set; }
	}
}
