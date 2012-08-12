using System;

namespace Snowball.UI
{
	public class ControlEventArgs : EventArgs
	{
		public Control Control
		{
			get;
			internal set;
		}

		public ControlEventArgs()
			: base()
		{
		}
	}
}
