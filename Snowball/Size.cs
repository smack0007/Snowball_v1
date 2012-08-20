using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball
{
	public struct Size
	{
		public static readonly Size Zero = new Size(0, 0);

		/// <summary>
		/// The X component.
		/// </summary>
		public int Width;

		/// <summary>
		/// The Y component.
		/// </summary>
		public int Height;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public Size(int width, int height)
		{
			this.Width = width;
			this.Height = height;
		}

		public override bool Equals(object obj)
		{
			if (obj is Size)
				return this.Equals((Size)obj);

			return false;
		}

		public bool Equals(Size other)
		{
			return this.Width == other.Width && this.Height == other.Height;
		}

		public override int GetHashCode()
		{
			return this.Width ^ this.Height;
		}

		public override string ToString()
		{
			return "{" + this.Width + ", " + this.Height + "}";
		}

		public static bool operator ==(Size p1, Size p2)
		{
			return (p1.Width == p2.Width) && (p1.Height == p2.Height);
		}

		public static bool operator !=(Size p1, Size p2)
		{
			return (p1.Width != p2.Width) || (p1.Height != p2.Height);
		}

		public static Size operator +(Size p1, Size p2)
		{
			return new Size(p1.Width + p2.Width, p1.Height + p2.Height);
		}

		public static Size operator -(Size p1, Size p2)
		{
			return new Size(p1.Width - p2.Width, p1.Height - p2.Height);
		}

		public static Size operator *(Size p1, Size p2)
		{
			return new Size(p1.Width * p2.Width, p1.Height * p2.Height);
		}

		public static Size operator *(Size v, int val)
		{
			return new Size(v.Width * val, v.Height * val);
		}

		public static Size operator /(Size p1, Size p2)
		{
			return new Size(p1.Width / p2.Width, p1.Height / p2.Height);
		}

		public static Size operator /(Size v, int val)
		{
			return new Size(v.Width / val, v.Height / val);
		}
	}
}
