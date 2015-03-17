using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Snowball.Graphics
{
	public abstract partial class VertexBuffer : DisposableObject
	{
		public int SizeOfVertex { get; protected set; }

		public int Capacity { get; protected set; }

		public int Count { get; protected set; }

		public VertexBufferUsage Usage { get; protected set; }

		internal VertexBuffer()
		{
		}
	}

	public sealed partial class VertexBuffer<T> : VertexBuffer
		where T : struct
	{
		bool hasData;

		public VertexBuffer(GraphicsDevice graphicsDevice, int capacity, VertexBufferUsage usage)
			: base()
		{
			if (graphicsDevice == null)
				throw new ArgumentNullException("graphicsDevice");

			this.Construct(graphicsDevice, capacity, usage);

			this.SizeOfVertex = Marshal.SizeOf(typeof(T));
			this.Capacity = capacity;
			this.Usage = usage;
		}

		public void SetData(T[] data)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			if (this.Usage == VertexBufferUsage.Static && this.hasData)
				throw new GraphicsException("VertexBuffer with Usage set to Static cannot be written to more than once.");

			if (data.Length > this.Capacity)
				throw new GraphicsException("Data exceeds the capacity of the VertexBuffer.");

			this.SetDataInternal(data);

			this.hasData = true;
			this.Count = data.Length;
		}
	}
}
