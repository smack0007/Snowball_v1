using System;

namespace Snowball
{
	/// <summary>
	/// Represents a RGB color.
	/// </summary>
	public struct Color
	{
		#region Static colors

		public static readonly Color AliceBlue = new Color(0xF0, 0xF8, 0xFF, 0xFF);
		public static readonly Color AntiqueWhite = new Color(0xFA, 0xEB, 0xD7, 0xFF);
		public static readonly Color Aqua = new Color(0x00, 0xFF, 0xFF, 0xFF);
		public static readonly Color Aquamarine = new Color(0x7F, 0xFF, 0xD4, 0xFF);
		public static readonly Color Azure = new Color(0xF0, 0xFF, 0xFF, 0xFF);
		public static readonly Color Beige = new Color(0xF5, 0xF5, 0xDC, 0xFF);
		public static readonly Color Bisque = new Color(0xFF, 0xE4, 0xC4, 0xFF);
		public static readonly Color Black = new Color(0x00, 0x00, 0x00, 0xFF);
		public static readonly Color BlanchedAlmond = new Color(0xFF, 0xEB, 0xCD, 0xFF);
		public static readonly Color Blue = new Color(0x00, 0x00, 0xFF, 0xFF);
		public static readonly Color BlueViolet = new Color(0x8A, 0x2B, 0xE2, 0xFF);
		public static readonly Color Brown = new Color(0xA5, 0x2A, 0x2A, 0xFF);
		public static readonly Color BurlyWood = new Color(0xDE, 0xB8, 0x87, 0xFF);
		public static readonly Color CadetBlue = new Color(0x5F, 0x9E, 0xA0, 0xFF);
		public static readonly Color Chartreuse = new Color(0x7F, 0xFF, 0x00, 0xFF);
		public static readonly Color Chocolate = new Color(0xD2, 0x69, 0x1E, 0xFF);
		public static readonly Color Coral = new Color(0xFF, 0x7F, 0x50, 0xFF);
		public static readonly Color CornflowerBlue = new Color(0x64, 0x95, 0xED, 0xFF);
		public static readonly Color Cornsilk = new Color(0xFF, 0xF8, 0xDC, 0xFF);
		public static readonly Color Crimson = new Color(0xDC, 0x14, 0x3C, 0xFF);
		public static readonly Color Cyan = new Color(0x00, 0xFF, 0xFF, 0xFF);
		public static readonly Color DarkBlue = new Color(0x00, 0x00, 0x8B, 0xFF);
		public static readonly Color DarkCyan = new Color(0x00, 0x8B, 0x8B, 0xFF);
		public static readonly Color DarkGoldenRod = new Color(0xB8, 0x86, 0x0B, 0xFF);
		public static readonly Color DarkGray = new Color(0xA9, 0xA9, 0xA9, 0xFF);
		public static readonly Color DarkGrey = new Color(0xA9, 0xA9, 0xA9, 0xFF);
		public static readonly Color DarkGreen = new Color(0x00, 0x64, 0x00, 0xFF);
		public static readonly Color DarkKhaki = new Color(0xBD, 0xB7, 0x6B, 0xFF);
		public static readonly Color DarkMagenta = new Color(0x8B, 0x00, 0x8B, 0xFF);
		public static readonly Color DarkOliveGreen = new Color(0x55, 0x6B, 0x2F, 0xFF);
		public static readonly Color Darkorange = new Color(0xFF, 0x8C, 0x00, 0xFF);
		public static readonly Color DarkOrchid = new Color(0x99, 0x32, 0xCC, 0xFF);
		public static readonly Color DarkRed = new Color(0x8B, 0x00, 0x00, 0xFF);
		public static readonly Color DarkSalmon = new Color(0xE9, 0x96, 0x7A, 0xFF);
		public static readonly Color DarkSeaGreen = new Color(0x8F, 0xBC, 0x8F, 0xFF);
		public static readonly Color DarkSlateBlue = new Color(0x48, 0x3D, 0x8B, 0xFF);
		public static readonly Color DarkSlateGray = new Color(0x2F, 0x4F, 0x4F, 0xFF);
		public static readonly Color DarkSlateGrey = new Color(0x2F, 0x4F, 0x4F, 0xFF);
		public static readonly Color DarkTurquoise = new Color(0x00, 0xCE, 0xD1, 0xFF);
		public static readonly Color DarkViolet = new Color(0x94, 0x00, 0xD3, 0xFF);
		public static readonly Color DeepPink = new Color(0xFF, 0x14, 0x93, 0xFF);
		public static readonly Color DeepSkyBlue = new Color(0x00, 0xBF, 0xFF, 0xFF);
		public static readonly Color DimGray = new Color(0x69, 0x69, 0x69, 0xFF);
		public static readonly Color DimGrey = new Color(0x69, 0x69, 0x69, 0xFF);
		public static readonly Color DodgerBlue = new Color(0x1E, 0x90, 0xFF, 0xFF);
		public static readonly Color FireBrick = new Color(0xB2, 0x22, 0x22, 0xFF);
		public static readonly Color FloralWhite = new Color(0xFF, 0xFA, 0xF0, 0xFF);
		public static readonly Color ForestGreen = new Color(0x22, 0x8B, 0x22, 0xFF);
		public static readonly Color Fuchsia = new Color(0xFF, 0x00, 0xFF, 0xFF);
		public static readonly Color Gainsboro = new Color(0xDC, 0xDC, 0xDC, 0xFF);
		public static readonly Color GhostWhite = new Color(0xF8, 0xF8, 0xFF, 0xFF);
		public static readonly Color Gold = new Color(0xFF, 0xD7, 0x00, 0xFF);
		public static readonly Color GoldenRod = new Color(0xDA, 0xA5, 0x20, 0xFF);
		public static readonly Color Gray = new Color(0x80, 0x80, 0x80, 0xFF);
		public static readonly Color Grey = new Color(0x80, 0x80, 0x80, 0xFF);
		public static readonly Color Green = new Color(0x00, 0x80, 0x00, 0xFF);
		public static readonly Color GreenYellow = new Color(0xAD, 0xFF, 0x2F, 0xFF);
		public static readonly Color HoneyDew = new Color(0xF0, 0xFF, 0xF0, 0xFF);
		public static readonly Color HotPink = new Color(0xFF, 0x69, 0xB4, 0xFF);
		public static readonly Color IndianRed = new Color(0xCD, 0x5C, 0x5C, 0xFF);
		public static readonly Color Indigo = new Color(0x4B, 0x00, 0x82, 0xFF);
		public static readonly Color Ivory = new Color(0xFF, 0xFF, 0xF0, 0xFF);
		public static readonly Color Khaki = new Color(0xF0, 0xE6, 0x8C, 0xFF);
		public static readonly Color Lavender = new Color(0xE6, 0xE6, 0xFA, 0xFF);
		public static readonly Color LavenderBlush = new Color(0xFF, 0xF0, 0xF5, 0xFF);
		public static readonly Color LawnGreen = new Color(0x7C, 0xFC, 0x00, 0xFF);
		public static readonly Color LemonChiffon = new Color(0xFF, 0xFA, 0xCD, 0xFF);
		public static readonly Color LightBlue = new Color(0xAD, 0xD8, 0xE6, 0xFF);
		public static readonly Color LightCoral = new Color(0xF0, 0x80, 0x80, 0xFF);
		public static readonly Color LightCyan = new Color(0xE0, 0xFF, 0xFF, 0xFF);
		public static readonly Color LightGoldenRodYellow = new Color(0xFA, 0xFA, 0xD2, 0xFF);
		public static readonly Color LightGray = new Color(0xD3, 0xD3, 0xD3, 0xFF);
		public static readonly Color LightGrey = new Color(0xD3, 0xD3, 0xD3, 0xFF);
		public static readonly Color LightGreen = new Color(0x90, 0xEE, 0x90, 0xFF);
		public static readonly Color LightPink = new Color(0xFF, 0xB6, 0xC1, 0xFF);
		public static readonly Color LightSalmon = new Color(0xFF, 0xA0, 0x7A, 0xFF);
		public static readonly Color LightSeaGreen = new Color(0x20, 0xB2, 0xAA, 0xFF);
		public static readonly Color LightSkyBlue = new Color(0x87, 0xCE, 0xFA, 0xFF);
		public static readonly Color LightSlateGray = new Color(0x77, 0x88, 0x99, 0xFF);
		public static readonly Color LightSlateGrey = new Color(0x77, 0x88, 0x99, 0xFF);
		public static readonly Color LightSteelBlue = new Color(0xB0, 0xC4, 0xDE, 0xFF);
		public static readonly Color LightYellow = new Color(0xFF, 0xFF, 0xE0, 0xFF);
		public static readonly Color Lime = new Color(0x00, 0xFF, 0x00, 0xFF);
		public static readonly Color LimeGreen = new Color(0x32, 0xCD, 0x32, 0xFF);
		public static readonly Color Linen = new Color(0xFA, 0xF0, 0xE6, 0xFF);
		public static readonly Color Magenta = new Color(0xFF, 0x00, 0xFF, 0xFF);
		public static readonly Color Maroon = new Color(0x80, 0x00, 0x00, 0xFF);
		public static readonly Color MediumAquaMarine = new Color(0x66, 0xCD, 0xAA, 0xFF);
		public static readonly Color MediumBlue = new Color(0x00, 0x00, 0xCD, 0xFF);
		public static readonly Color MediumOrchid = new Color(0xBA, 0x55, 0xD3, 0xFF);
		public static readonly Color MediumPurple = new Color(0x93, 0x70, 0xD8, 0xFF);
		public static readonly Color MediumSeaGreen = new Color(0x3C, 0xB3, 0x71, 0xFF);
		public static readonly Color MediumSlateBlue = new Color(0x7B, 0x68, 0xEE, 0xFF);
		public static readonly Color MediumSpringGreen = new Color(0x00, 0xFA, 0x9A, 0xFF);
		public static readonly Color MediumTurquoise = new Color(0x48, 0xD1, 0xCC, 0xFF);
		public static readonly Color MediumVioletRed = new Color(0xC7, 0x15, 0x85, 0xFF);
		public static readonly Color MidnightBlue = new Color(0x19, 0x19, 0x70, 0xFF);
		public static readonly Color MintCream = new Color(0xF5, 0xFF, 0xFA, 0xFF);
		public static readonly Color MistyRose = new Color(0xFF, 0xE4, 0xE1, 0xFF);
		public static readonly Color Moccasin = new Color(0xFF, 0xE4, 0xB5, 0xFF);
		public static readonly Color NavajoWhite = new Color(0xFF, 0xDE, 0xAD, 0xFF);
		public static readonly Color Navy = new Color(0x00, 0x00, 0x80, 0xFF);
		public static readonly Color OldLace = new Color(0xFD, 0xF5, 0xE6, 0xFF);
		public static readonly Color Olive = new Color(0x80, 0x80, 0x00, 0xFF);
		public static readonly Color OliveDrab = new Color(0x6B, 0x8E, 0x23, 0xFF);
		public static readonly Color Orange = new Color(0xFF, 0xA5, 0x00, 0xFF);
		public static readonly Color OrangeRed = new Color(0xFF, 0x45, 0x00, 0xFF);
		public static readonly Color Orchid = new Color(0xDA, 0x70, 0xD6, 0xFF);
		public static readonly Color PaleGoldenRod = new Color(0xEE, 0xE8, 0xAA, 0xFF);
		public static readonly Color PaleGreen = new Color(0x98, 0xFB, 0x98, 0xFF);
		public static readonly Color PaleTurquoise = new Color(0xAF, 0xEE, 0xEE, 0xFF);
		public static readonly Color PaleVioletRed = new Color(0xD8, 0x70, 0x93, 0xFF);
		public static readonly Color PapayaWhip = new Color(0xFF, 0xEF, 0xD5, 0xFF);
		public static readonly Color PeachPuff = new Color(0xFF, 0xDA, 0xB9, 0xFF);
		public static readonly Color Peru = new Color(0xCD, 0x85, 0x3F, 0xFF);
		public static readonly Color Pink = new Color(0xFF, 0xC0, 0xCB, 0xFF);
		public static readonly Color Plum = new Color(0xDD, 0xA0, 0xDD, 0xFF);
		public static readonly Color PowderBlue = new Color(0xB0, 0xE0, 0xE6, 0xFF);
		public static readonly Color Purple = new Color(0x80, 0x00, 0x80, 0xFF);
		public static readonly Color Red = new Color(0xFF, 0x00, 0x00, 0xFF);
		public static readonly Color RosyBrown = new Color(0xBC, 0x8F, 0x8F, 0xFF);
		public static readonly Color RoyalBlue = new Color(0x41, 0x69, 0xE1, 0xFF);
		public static readonly Color SaddleBrown = new Color(0x8B, 0x45, 0x13, 0xFF);
		public static readonly Color Salmon = new Color(0xFA, 0x80, 0x72, 0xFF);
		public static readonly Color SandyBrown = new Color(0xF4, 0xA4, 0x60, 0xFF);
		public static readonly Color SeaGreen = new Color(0x2E, 0x8B, 0x57, 0xFF);
		public static readonly Color SeaShell = new Color(0xFF, 0xF5, 0xEE, 0xFF);
		public static readonly Color Sienna = new Color(0xA0, 0x52, 0x2D, 0xFF);
		public static readonly Color Silver = new Color(0xC0, 0xC0, 0xC0, 0xFF);
		public static readonly Color SkyBlue = new Color(0x87, 0xCE, 0xEB, 0xFF);
		public static readonly Color SlateBlue = new Color(0x6A, 0x5A, 0xCD, 0xFF);
		public static readonly Color SlateGray = new Color(0x70, 0x80, 0x90, 0xFF);
		public static readonly Color SlateGrey = new Color(0x70, 0x80, 0x90, 0xFF);
		public static readonly Color Snow = new Color(0xFF, 0xFA, 0xFA, 0xFF);
		public static readonly Color SpringGreen = new Color(0x00, 0xFF, 0x7F, 0xFF);
		public static readonly Color SteelBlue = new Color(0x46, 0x82, 0xB4, 0xFF);
		public static readonly Color Tan = new Color(0xD2, 0xB4, 0x8C, 0xFF);
		public static readonly Color Teal = new Color(0x00, 0x80, 0x80, 0xFF);
		public static readonly Color Thistle = new Color(0xD8, 0xBF, 0xD8, 0xFF);
		public static readonly Color Tomato = new Color(0xFF, 0x63, 0x47, 0xFF);
		public static readonly Color Transparent = new Color(0x00, 0x00, 0x00, 0x00);
		public static readonly Color Turquoise = new Color(0x40, 0xE0, 0xD0, 0xFF);
		public static readonly Color Violet = new Color(0xEE, 0x82, 0xEE, 0xFF);
		public static readonly Color Wheat = new Color(0xF5, 0xDE, 0xB3, 0xFF);
		public static readonly Color White = new Color(0xFF, 0xFF, 0xFF, 0xFF);
		public static readonly Color WhiteSmoke = new Color(0xF5, 0xF5, 0xF5, 0xFF);
		public static readonly Color Yellow = new Color(0xFF, 0xFF, 0x00, 0xFF);
		public static readonly Color YellowGreen = new Color(0x9A, 0xCD, 0x32, 0xFF);

		#endregion

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
		/// Returns a new color with the values limited by the given color's values.
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public Color Limit(Color color)
		{
			return new Color(
				Math.Min(this.R, color.R),
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

		/// <summary>
		/// Converts the color to a hex string.
		/// </summary>
		/// <returns></returns>
		public string ToHexString()
		{
			char[] chars = new char[8];

			int bits = this.R;
			chars[0] = HexHelper.HexDigits[bits >> 4];
			chars[1] = HexHelper.HexDigits[bits & 0xF];

			bits = this.G;
			chars[2] = HexHelper.HexDigits[bits >> 4];
			chars[3] = HexHelper.HexDigits[bits & 0xF];

			bits = this.B;
			chars[4] = HexHelper.HexDigits[bits >> 4];
			chars[5] = HexHelper.HexDigits[bits & 0xF];

			bits = this.A;
			chars[6] = HexHelper.HexDigits[bits >> 4];
			chars[7] = HexHelper.HexDigits[bits & 0xF];

			return new string(chars);
		}

		/// <summary>
		/// Creates a color from a hex string.
		/// </summary>
		/// <param name="hex"></param>
		/// <returns></returns>
		public static Color FromHexString(string hex)
		{
			if (string.IsNullOrEmpty(hex) || hex.Length != 8 || !HexHelper.IsValidHexString(hex))
				return Color.Black;
						
			int r = (HexHelper.HexDigitToByte(hex[0]) << 4) + HexHelper.HexDigitToByte(hex[1]);
			int g = (HexHelper.HexDigitToByte(hex[2]) << 4) + HexHelper.HexDigitToByte(hex[3]);
			int b = (HexHelper.HexDigitToByte(hex[4]) << 4) + HexHelper.HexDigitToByte(hex[5]);
			int a = (HexHelper.HexDigitToByte(hex[6]) << 4) + HexHelper.HexDigitToByte(hex[7]);

			return new Color((byte)r, (byte)g, (byte)b, (byte)a);
		}
	}
}
