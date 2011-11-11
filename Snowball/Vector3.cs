using System;

namespace Snowball
{
	public struct Vector3
	{
		public static readonly Vector3 Zero = new Vector3();

		public static readonly Vector3 UnitX = new Vector3(1.0f, 0.0f, 0.0f);

		public static readonly Vector3 UnitY = new Vector3(0.0f, 1.0f, 0.0f);

		public static readonly Vector3 UnitZ = new Vector3(0.0f, 0.0f, 1.0f);
		
		public static readonly Vector3 One = new Vector3(1.0f, 1.0f, 1.0f);

		/// <summary>
		/// X component.
		/// </summary>
		public float X;

		/// <summary>
		/// Y component.
		/// </summary>
		public float Y;

		/// <summary>
		/// Z component.
		/// </summary>
		public float Z;

		/// <summary>
		/// Initializes a new Vector3.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public Vector3(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		public static Vector3 Transform(Vector3 position, Matrix matrix)
		{
			Vector3 result;
			Transform(ref position, ref matrix, out result);
			return result;
		}

		public static void Transform(ref Vector3 position, ref Matrix matrix, out Vector3 result)
		{
			result = new Vector3((position.X * matrix.M11) + (position.Y * matrix.M21) + (position.Z * matrix.M31) + matrix.M41,
								 (position.X * matrix.M12) + (position.Y * matrix.M22) + (position.Z * matrix.M32) + matrix.M42,
								 (position.X * matrix.M13) + (position.Y * matrix.M23) + (position.Z * matrix.M33) + matrix.M43);
		}
	}
}
