using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.UserInterface
{
	public class UserInterfacePropertyChangedEventArgs : EventArgs
	{
		public UserInterfaceProperties Property
		{
			get;
			set;
		}

		public UserInterfacePropertyChangedEventArgs()
			: base()
		{
		}
	}
}
