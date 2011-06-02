using System;

namespace Snowball
{
	/// <summary>
	/// Manages type conversions from engine types to SlimDx types.
	/// </summary>
	internal class TypeConverter
	{
		internal static System.Drawing.Color Convert(Snowball.Color color)
		{
			return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
		}
	}
}
