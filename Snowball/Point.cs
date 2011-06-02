using System;

namespace Snowball
{
	public class Point
	{
		public static readonly Point Zero = new Point(0, 0);

		/// <summary>
		/// The X component.
		/// </summary>
		public int X;

		/// <summary>
		/// The Y component.
		/// </summary>
		public int Y;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public Point(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
	}
}
