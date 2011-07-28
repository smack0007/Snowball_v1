using System;

namespace Snowball.Input
{
	/// <summary>
	/// EventArgs associated with typing.
	/// </summary>
	public class KeyPressEventArgs : EventArgs
	{
		/// <summary>
		/// The 
		/// </summary>
		public char KeyChar
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public KeyPressEventArgs()
			: base()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="keyCode"></param>
		public KeyPressEventArgs(char keyCode)
			: base()
		{
			this.KeyChar = keyCode;
		}
	}
}
