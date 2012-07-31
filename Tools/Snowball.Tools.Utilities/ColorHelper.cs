using System;
using System.Drawing;
using System.Linq;

namespace Snowball.Tools.Utilities
{
	public static class ColorHelper
	{
		private static readonly char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

		/// <summary>
		/// Converts a Color to a hex string.
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static string ToHexString(this Color color)
		{
			char[] chars = new char[8];

			int b = color.R;
			chars[0] = hexDigits[b >> 4];
			chars[1] = hexDigits[b & 0xF];

			b = color.G;
			chars[2] = hexDigits[b >> 4];
			chars[3] = hexDigits[b & 0xF];

			b = color.B;
			chars[4] = hexDigits[b >> 4];
			chars[5] = hexDigits[b & 0xF];

			b = color.A;
			chars[6] = hexDigits[b >> 4];
			chars[7] = hexDigits[b & 0xF];

			return new string(chars);
		}

		/// <summary>
		/// Helper method used in FromHexString.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		private static byte HexDigitToByte(char c)
		{
			switch (c)
			{
				case '0': return (byte)0;
				case '1': return (byte)1;
				case '2': return (byte)2;
				case '3': return (byte)3;
				case '4': return (byte)4;
				case '5': return (byte)5;
				case '6': return (byte)6;
				case '7': return (byte)7;
				case '8': return (byte)8;
				case '9': return (byte)9;
				case 'A': return (byte)10;
				case 'B': return (byte)11;
				case 'C': return (byte)12;
				case 'D': return (byte)13;
				case 'E': return (byte)14;
				case 'F': return (byte)15;
			}

			return (byte)0;
		}

		/// <summary>
		/// Returns true if the given string is a valid hex color string.
		/// </summary>
		/// <param name="hex"></param>
		/// <returns></returns>
		public static bool IsValidHexString(string hex)
		{
			hex = hex.ToUpper();

			if (string.IsNullOrEmpty(hex))
				return false;

			if (hex.Length != 8)
				return false;

			for (int i = 0; i < hex.Length; i++)
			{
				if (!hexDigits.Contains(hex[i]))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Creates a color from a hex string.
		/// </summary>
		/// <param name="hex"></param>
		/// <returns></returns>
		public static Color FromHexString(string hex)
		{
			if (string.IsNullOrEmpty(hex) || hex.Length != 8)
				return Color.Black;

			hex = hex.ToUpper();

			int r = (HexDigitToByte(hex[0]) << 4) + HexDigitToByte(hex[1]);
			int g = (HexDigitToByte(hex[2]) << 4) + HexDigitToByte(hex[3]);
			int b = (HexDigitToByte(hex[4]) << 4) + HexDigitToByte(hex[5]);
			int a = (HexDigitToByte(hex[6]) << 4) + HexDigitToByte(hex[7]);

			return Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
		}
	}
}
