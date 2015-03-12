using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.Graphics
{
	public sealed partial class VertexBuffer<T> : DisposableObject
		where T : struct
	{
		public int Count
		{
			get;
			private set;
		}

		public VertexBuffer(GraphicsDevice graphicsDevice)
		{
			if (graphicsDevice == null)
				throw new ArgumentNullException("graphicsDevice");

			this.Construct(graphicsDevice);
		}
	}
}
