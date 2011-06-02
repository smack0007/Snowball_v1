using System;

namespace Snowball
{
	public struct Rectangle
	{
		public static readonly Rectangle Empty = new Rectangle(0, 0, 0, 0);

		/// <summary>
		/// The X position of the Rectangle.
		/// </summary>
		public int X;

		/// <summary>
		/// The Y position of the Rectangle.
		/// </summary>
		public int Y;

		/// <summary>
		/// The width of the Rectangle.
		/// </summary>
		public int Width;

		/// <summary>
		/// The height of the Rectangle.
		/// </summary>
		public int Height;

		/// <summary>
		/// The left side of the Rectangle.
		/// </summary>
		public int Left
		{
			get { return this.X; }
			set { this.X = value; }
		}

		/// <summary>
		/// The top side of the Rectangle.
		/// </summary>
		public int Top
		{
			get { return this.Y; }
			set { this.Y = value; }
		}

		/// <summary>
		/// The right side of the Rectangle.
		/// </summary>
		public int Right
		{
			get { return this.X + this.Width; }
			set { this.X = value - this.Width; }
		}

		/// <summary>
		/// The bottom side of the Rectangle.
		/// </summary>
		public int Bottom
		{
			get { return this.Y + this.Height; }
			set { this.Y = value - this.Height; }
		}

		/// <summary>
		/// Initializes a new Rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public Rectangle(int x, int y, int width, int height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}

		/// <summary>
		/// Returns true if r1 instersects r2.
		/// </summary>
		/// <param name="r1"></param>
		/// <param name="r2"></param>
		/// <returns></returns>
		public static bool Intersects(Rectangle r1, Rectangle r2)
		{
			return Intersects(ref r1, ref r2);
		}

		/// <summary>
		/// Returns true if r1 intersects r2.
		/// </summary>
		/// <param name="r1"></param>
		/// <param name="r2"></param>
		/// <returns></returns>
		public static bool Intersects(ref Rectangle r1, ref Rectangle r2)
		{
			if(r2.Left > r1.Right || r2.Right < r1.Left ||
			   r2.Top > r1.Bottom || r2.Bottom < r1.Top)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Returns true if the Rectangle intersects with the other.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Intersects(Rectangle other)
		{
			return Intersects(ref this, ref other);
		}

		/// <summary>
		/// Returns true if the Rectangle intersects with the other.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Intersects(ref Rectangle other)
		{
			return Intersects(ref this, ref other);
		}
	}
}
