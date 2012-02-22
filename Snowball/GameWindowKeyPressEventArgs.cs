using System;

namespace Snowball.Input
{
	/// <summary>
	/// EventArgs associated with typing.
	/// </summary>
	public class GameWindowKeyPressEventArgs : EventArgs
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
		public GameWindowKeyPressEventArgs()
			: base()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="keyCode"></param>
		public GameWindowKeyPressEventArgs(char keyCode)
			: base()
		{
			this.KeyChar = keyCode;
		}
	}
}
