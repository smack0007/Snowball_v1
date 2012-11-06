using System;

namespace Snowball.Graphics
{
	internal static class TypeConverter
	{
		internal static System.Drawing.Rectangle Convert(Rectangle rectangle)
		{
			return new System.Drawing.Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}

		internal static System.Drawing.Color Convert(Color color)
		{
			return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
		}
	}
}
