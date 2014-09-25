using System;

namespace Snowball
{
	public struct Point
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

		public override bool Equals(object obj)
		{
			if (obj is Point)
				return this.Equals((Point)obj);

			return false;
		}

		public bool Equals(Point other)
		{
			return this.X == other.X && this.Y == other.Y;
		}

		public override int GetHashCode()
		{
			return this.X ^ this.Y;
		}

		public override string ToString()
		{
			return "{" + this.X + ", " + this.Y + "}";
		}

		public static bool operator ==(Point p1, Point p2)
		{
			return (p1.X == p2.X) && (p1.Y == p2.Y);
		}

		public static bool operator !=(Point p1, Point p2)
		{
			return (p1.X != p2.X) || (p1.Y != p2.Y);
		}

		public static Point operator +(Point p1, Point p2)
		{
			return new Point(p1.X + p2.X, p1.Y + p2.Y);
		}

		public static Point operator -(Point p1, Point p2)
		{
			return new Point(p1.X - p2.X, p1.Y - p2.Y);
		}

		public static Point operator *(Point p1, Point p2)
		{
			return new Point(p1.X * p2.X, p1.Y * p2.Y);
		}

		public static Point operator *(Point v, int val)
		{
			return new Point(v.X * val, v.Y * val);
		}

		public static Point operator /(Point p1, Point p2)
		{
			return new Point(p1.X / p2.X, p1.Y / p2.Y);
		}

		public static Point operator /(Point v, int val)
		{
			return new Point(v.X / val, v.Y / val);
		}

		public static implicit operator Vector2(Point p)
		{
			return new Vector2(p.X, p.Y);
		}
	}
}
