using System;

namespace Snowball
{
	internal static class TypeConverter
	{
		public static System.Drawing.Rectangle Convert(Rectangle rectangle)
		{
			return new System.Drawing.Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}
	}
}
