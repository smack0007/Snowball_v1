using System;

namespace Snowball
{
	/// <summary>
	/// EventArgs associated with typing.
	/// </summary>
	public class GameWindowKeyPressEventArgs : EventArgs
	{
		public int KeyCode
		{
			get;
			set;
		}

		/// <summary>
		/// The char of the key that was pressed.
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
