using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.Graphics
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class VertexElementAttribute : Attribute
	{
		public VertexElementUsage Usage { get; private set; }

		public int Index { get; private set; }

		public VertexElementAttribute(VertexElementUsage usage, int index)
		{
			this.Usage = usage;
			this.Index = index;
		}
	}
}
