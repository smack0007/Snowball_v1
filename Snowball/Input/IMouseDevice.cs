using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.Input
{
	public interface IMouseDevice
	{
		/// <summary>
		/// The position of the cursor.
		/// </summary>
		Point Position { get; }

		/// <summary>
		/// The X position of the cursor.
		/// </summary>
		int X { get; }

		/// <summary>
		/// The Y position of the cursor.
		/// </summary>
		int Y { get; }
	}
}
