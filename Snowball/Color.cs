using System;

namespace Snowball
{
	/// <summary>
	/// Represents a RGB color.
	/// </summary>
	public struct Color
	{
		public static readonly Color Black = new Color(0, 0, 0, 255);
		public static readonly Color Blue = new Color(0, 0, 255, 255);
		public static readonly Color CornflowerBlue = new Color(100, 149, 237, 255);
		public static readonly Color Green = new Color(0, 255, 0, 255);
		public static readonly Color Magenta = new Color(255, 0, 255, 255);
		public static readonly Color Red = new Color(255, 0, 0, 255);
		public static readonly Color Transparent = new Color(0, 0, 0, 0);
		public static readonly Color White = new Color(255, 255, 255, 255);

		/// <summary>
		/// The red value.
		/// </summary>
		public byte R;

		/// <summary>
		/// The green value.
		/// </summary>
		public byte G;

		/// <summary>
		/// The blue value.
		/// </summary>
		public byte B;

		/// <summary>
		/// The alpha value.
		/// </summary>
		public byte A;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		public Color(byte r, byte g, byte b, byte a)
		{
			this.R = r;
			this.G = g;
			this.B = b;
			this.A = a;
		}

		/// <summary>
		/// Returns a new color with the same values as this one limited by the given color values.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		/// <returns></returns>
		public Color Limit(byte r, byte g, byte b, byte a)
		{
			return new Color(Math.Min(this.R, r),
							 Math.Min(this.G, g),
							 Math.Min(this.B, b),
							 Math.Min(this.A, a));
		}

		/// <summary>
		/// Returns a new color with the same values as this one limited by the given color values.
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public Color Limit(Color color)
		{
			return new Color(Math.Min(this.R, color.R),
							 Math.Min(this.G, color.G),
							 Math.Min(this.B, color.B),
							 Math.Min(this.A, color.A));
		}

		public int ToArgb()
		{
			return (this.A << 24) | (this.R << 16) | (this.G << 8) | this.B;
		}

		public override string ToString()
		{
			return "{ " + this.R + ", " + this.G + ", " + this.B + ", " + this.A + " }";
		}
	}

	public enum ColorFunction
	{
		Limit
	}
}
