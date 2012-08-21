using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowball.Input;

namespace Snowball.UI
{
	public class MouseEventArgs : EventArgs
	{
		Point position;

		public MouseButtons? Button
		{
			get;
			internal set;
		}

		public Point Position
		{
			get { return this.position; }
			internal set { this.position = value; }
		}

		public int X
		{
			get { return this.position.X; }
		}

		public int Y
		{
			get { return this.position.Y; }
		}

		public MouseEventArgs()
			: base()
		{
		}
	}
}
