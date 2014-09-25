using System;

namespace Snowball
{
	public struct Vector2
	{
		public static readonly Vector2 Zero = new Vector2();

		public static readonly Vector2 UnitX = new Vector2(1.0f, 0.0f);

		public static readonly Vector2 UnitY = new Vector2(0.0f, 1.0f);

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

		public override bool Equals(object obj)
		{
			if (obj is Vector2)
				return this.Equals((Vector2)obj);
			
			return false;
		}

		public bool Equals(Vector2 other)
		{
			return this.X == other.X && this.Y == other.Y;
		}

		public override int GetHashCode()
		{
			return (int)this.X ^ (int)this.Y;
		}

		public override string ToString()
		{
			return "{" + this.X + ", " + this.Y + "}";
		}

		/// <summary>
		/// Calculates an angle from a single Vector2.
		/// </summary>
		/// <returns>The angle as a float.</returns>
		public float CalculateAngle()
		{
			float angle = 0.0f;

			if (this.X != 0.0f && this.Y != 0.0f)
			{
				Vector2 v = this.Normalize();

				angle = (float)Math.Acos(v.Y);

				if (v.X < 0.0f)
					angle = -angle;
			}

			return angle;
		}

		public float CalculateAngle(Vector2 v)
		{
			return CalculateAngle(ref this, ref v);
		}

		public float CalculateAngle(ref Vector2 v)
		{
			return CalculateAngle(ref this, ref v);
		}

		/// <summary>
		/// Calculates an angle from 2 Vector2(s).
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <returns>The angle as a float.</returns>
		public static float CalculateAngle(Vector2 v1, Vector2 v2)
		{
			return CalculateAngle(ref v1, ref v2);
		}

		/// <summary>
		/// Calculates an angle from 2 Vector2(s).
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <returns>The angle as a float.</returns>
		public static float CalculateAngle(ref Vector2 v1, ref Vector2 v2)
		{
			return (float)Math.Atan2((double)(v1.Y - v2.Y), (double)(v1.X - v2.X)) - 1.57079637f; // 1.57079637f == 90 degrees
		}

		/// <summary>
		/// Calculates the length of the Vector2.
		/// </summary>
		/// <returns></returns>
		public float Length()
		{
			return (float)Math.Sqrt((this.X * this.X) + (this.Y * this.Y));
		}

		/// <summary>
		/// Returns a new 
		/// </summary>
		/// <returns></returns>
		public Vector2 Normalize()
		{
			float length = this.Length();
			return new Vector2(this.X / length, this.Y / length);
		}

		/// <summary>
		/// Orbits a Vector2 around an origin.
		/// </summary>
		/// <param name="point"></param>
		/// <param name="origin"></param>
		/// <param name="rotation"></param>
		/// <returns></returns>
		public static void RotateAboutOrigin(ref Vector2 point, Vector2 origin, float rotation)
		{
			Vector2 u = point - origin; // point relative to origin  

			if (u == Vector2.Zero)
				return;

			float a = (float)Math.Atan2(u.Y, u.X); // angle relative to origin  
			a += rotation; // rotate  

			// u is now the new point relative to origin
			float length = u.Length();
			u = new Vector2((float)Math.Cos(a) * length, (float)Math.Sin(a) * length);
			
			point = u + origin;
		}

		/// <summary>
		/// Returns a new Vector2 where the X and Y are whole pixels. 
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public Vector2 ToNearestPixel()
		{
			return new Vector2((int)this.X, (int)this.Y);
		}
				
		public static void Transform(ref Vector2 position, ref Matrix matrix, out Vector2 result)
		{
			result = new Vector2((position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41,
								 (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42);
		}

		public static Vector2 Transform(Vector2 position, ref Matrix matrix)
		{
			Vector2 result;
			Transform(ref position, ref matrix, out result);
			return result;
		}

		public static Vector2 Transform(Vector2 position, Matrix matrix)
		{
			Vector2 result;
			Transform(ref position, ref matrix, out result);
			return result;
		}

		public static void TransformNormal(ref Vector2 normal, ref Matrix transform, out Vector2 result)
		{
			result = new Vector2((normal.X * transform.M11) + (normal.Y * transform.M21),
				                 (normal.X * transform.M12) + (normal.Y * transform.M22));
		}

		public static Vector2 TransformNormal(ref Vector2 normal, ref Matrix transform)
		{
			Vector2 result;
			TransformNormal(ref normal, ref transform, out result);
			return result;
		}

		public static Vector2 TransformNormal(Vector2 normal, ref Matrix transform)
		{
			Vector2 result;
			TransformNormal(ref normal, ref transform, out result);
			return result;
		}

		public static bool operator ==(Vector2 v1, Vector2 v2)
		{
			return (v1.X == v2.X) && (v1.Y == v2.Y);
		}

		public static bool operator !=(Vector2 v1, Vector2 v2)
		{
			return (v1.X != v2.X) || (v1.Y != v2.Y);
		}

		public static Vector2 operator +(Vector2 v1, Vector2 v2)
		{
			return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
		}

		public static Vector2 operator -(Vector2 v1, Vector2 v2)
		{
			return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
		}

		public static Vector2 operator *(Vector2 v1, Vector2 v2)
		{
			return new Vector2(v1.X * v2.X, v1.Y * v2.Y);
		}

		public static Vector2 operator *(Vector2 v, float val)
		{
			return new Vector2(v.X * val, v.Y * val);
		}

		public static Vector2 operator /(Vector2 v1, Vector2 v2)
		{
			return new Vector2(v1.X / v2.X, v1.Y / v2.Y);
		}

		public static Vector2 operator /(Vector2 v, float val)
		{
			return new Vector2(v.X / val, v.Y / val);
		}
	}
}
