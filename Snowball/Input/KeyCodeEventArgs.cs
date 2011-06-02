using System;

namespace Snowball.Input
{
	public class KeyCodeEventArgs : EventArgs
	{
		public char KeyCode
		{
			get;
			internal set;
		}

		public KeyCodeEventArgs()
		{
		}

		public KeyCodeEventArgs(char keyCode)
		{
			this.KeyCode = keyCode;
		}
	}
}
