using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.Graphics
{
	public struct RendererSettings
	{
		public static readonly RendererSettings Default = new RendererSettings()
		{
			VertexBufferSize = 1024,
			MatrixStackSize = 8,
			ColorStackSize = 8,
			ColorStackFunction = ColorFunction.Limit
		};

		public int VertexBufferSize;
		public int MatrixStackSize;
		public int ColorStackSize;
		public ColorFunction ColorStackFunction;
	}
}
