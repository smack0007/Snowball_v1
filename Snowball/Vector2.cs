using System;

namespace Snowball
{
	public struct Vector2
	{
		public static readonly Vector2 Zero = new Vector2();
		public static readonly Vector2 One = new Vector2(1.0f, 1.0f);

		/// <summary>
		/// X component.
		/// </summary>
		public float X;

		/// <summary>
		/// Y component.
		/// </summary>
		public float Y;

		/// <summary>
		/// Initializes a new Vector2.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public Vector2(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		public static Vector2 Transform(Vector2 position, Matrix matrix)
		{
			Vector2 result;
			Transform(ref position, ref matrix, out result);
			return result;
		}

		public static void Transform(ref Vector2 position, ref Matrix matrix, out Vector2 result)
		{
			result = new Vector2((position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41,
								 (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42);
		}
	}
}
