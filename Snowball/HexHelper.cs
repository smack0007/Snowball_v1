using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball
{
	/// <summary>
	/// Contains helper methods for working with Hex strings.
	/// </summary>
	internal static class HexHelper
	{
		internal static readonly char[] HexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
				
		/// <summary>
		/// Helper method used in FromHexString.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		internal static byte HexDigitToByte(char c)
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
				
				case 'A':
				case 'a':
					return (byte)10;
				
				case 'B':
				case 'b':
					return (byte)11;
				
				case 'C':
				case 'c':
					return (byte)12;
				
				case 'D':
				case 'd':
					return (byte)13;
				
				case 'E':
				case 'e':
					return (byte)14;
				
				case 'F':
				case 'f':
					return (byte)15;
			}

			return (byte)0;
		}

		/// <summary>
		/// Returns true if the given string is a valid hex color string.
		/// </summary>
		/// <param name="hex"></param>
		/// <returns></returns>
		internal static bool IsValidHexString(string hex)
		{
			hex = hex.ToUpper();

			if (string.IsNullOrEmpty(hex))
				return false;
			
			for (int i = 0; i < hex.Length; i++)
			{
				if (!HexDigits.Contains(hex[i]))
					return false;
			}

			return true;
		}
	}
}
