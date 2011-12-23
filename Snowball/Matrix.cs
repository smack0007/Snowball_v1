#region SlimDX License
/*
* Copyright (c) 2007-2010 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball
{
	public struct Matrix
	{
		public static readonly Matrix Zero = new Matrix();

		public static readonly Matrix Identity = new Matrix() { M11 = 1.0f, M22 = 1.0f, M33 = 1.0f, M44 = 1.0f };

		public float M11;
		public float M12;
		public float M13;
		public float M14;
		public float M21;
		public float M22;
		public float M23;
		public float M24;
		public float M31;
		public float M32;
		public float M33;
		public float M34;
		public float M41;
		public float M42;
		public float M43;
		public float M44;

		public Matrix(float m11, float m12, float m13, float m14,
		              float m21, float m22, float m23, float m24,
		              float m31, float m32, float m33, float m34,
			          float m41, float m42, float m43, float m44)
		{
			this.M11 = m11;
			this.M12 = m12;
			this.M13 = m13;
			this.M14 = m14;
			this.M21 = m21;
			this.M22 = m22;
			this.M23 = m23;
			this.M24 = m24;
			this.M31 = m31;
			this.M32 = m32;
			this.M33 = m33;
			this.M34 = m34;
			this.M41 = m41;
			this.M42 = m42;
			this.M43 = m43;
			this.M44 = m44;
		}
				
		public static Matrix CreateRotationX(float radians)
		{
			Matrix returnMatrix = Matrix.Identity;

			returnMatrix.M22 = (float)Math.Cos(radians);
			returnMatrix.M23 = (float)Math.Sin(radians);
			returnMatrix.M32 = -returnMatrix.M23;
			returnMatrix.M33 = returnMatrix.M22;

			return returnMatrix;
		}

		public static void CreateRotationX(float radians, out Matrix result)
		{
			result = Matrix.Identity;

			result.M22 = (float)Math.Cos(radians);
			result.M23 = (float)Math.Sin(radians);
			result.M32 = -result.M23;
			result.M33 = result.M22;
		}

		public static Matrix CreateRotationY(float radians)
		{
			Matrix returnMatrix = Matrix.Identity;

			returnMatrix.M11 = (float)Math.Cos(radians);
			returnMatrix.M13 = (float)Math.Sin(radians);
			returnMatrix.M31 = -returnMatrix.M13;
			returnMatrix.M33 = returnMatrix.M11;

			return returnMatrix;
		}

		public static void CreateRotationY(float radians, out Matrix result)
		{
			result = Matrix.Identity;

			result.M11 = (float)Math.Cos(radians);
			result.M13 = (float)Math.Sin(radians);
			result.M31 = -result.M13;
			result.M33 = result.M11;
		}

		public static Matrix CreateRotationZ(float radians)
		{
			Matrix returnMatrix = Matrix.Identity;

			returnMatrix.M11 = (float)Math.Cos(radians);
			returnMatrix.M12 = (float)Math.Sin(radians);
			returnMatrix.M21 = -returnMatrix.M12;
			returnMatrix.M22 = returnMatrix.M11;

			return returnMatrix;
		}

		public static void CreateRotationZ(float radians, out Matrix result)
		{
			result = Matrix.Identity;

			result.M11 = (float)Math.Cos(radians);
			result.M12 = (float)Math.Sin(radians);
			result.M21 = -result.M12;
			result.M22 = result.M11;
		}

		public static Matrix CreateScale(float scale)
		{
			Matrix returnMatrix = Matrix.Identity;

			returnMatrix.M11 = scale;
			returnMatrix.M22 = scale;
			returnMatrix.M33 = scale;

			return returnMatrix;
		}

		public static void CreateScale(float scale, out Matrix result)
		{
			result = Matrix.Identity;

			result.M11 = scale;
			result.M22 = scale;
			result.M33 = scale;
		}

		public static Matrix CreateScale(float xScale, float yScale, float zScale)
		{
			Matrix returnMatrix = Matrix.Identity;

			returnMatrix.M11 = xScale;
			returnMatrix.M22 = yScale;
			returnMatrix.M33 = zScale;

			return returnMatrix;
		}

		public static void CreateScale(float xScale, float yScale, float zScale, out Matrix result)
		{
			result = Matrix.Identity;

			result.M11 = xScale;
			result.M22 = yScale;
			result.M33 = zScale;
		}

		public static Matrix CreateScale(Vector3 scales)
		{
			Matrix returnMatrix = Matrix.Identity;

			returnMatrix.M11 = scales.X;
			returnMatrix.M22 = scales.Y;
			returnMatrix.M33 = scales.Z;

			return returnMatrix;
		}

		public static void CreateScale(ref Vector3 scales, out Matrix result)
		{
			result = Matrix.Identity;

			result.M11 = scales.X;
			result.M22 = scales.Y;
			result.M33 = scales.Z;
		}

		public static Matrix CreateTranslation(float xPosition, float yPosition)
		{
			Matrix returnMatrix = Matrix.Identity;

			returnMatrix.M41 = xPosition;
			returnMatrix.M42 = yPosition;

			return returnMatrix;
		}

		public static void CreateTranslation(float xPosition, float yPosition, out Matrix result)
		{
			result = Matrix.Identity;

			result.M41 = xPosition;
			result.M42 = yPosition;
		}

		public static Matrix CreateTranslation(float xPosition, float yPosition, float zPosition)
		{
			Matrix returnMatrix = Matrix.Identity;

			returnMatrix.M41 = xPosition;
			returnMatrix.M42 = yPosition;
			returnMatrix.M43 = zPosition;

			return returnMatrix;
		}

		public static void CreateTranslation(float xPosition, float yPosition, float zPosition, out Matrix result)
		{
			result = Matrix.Identity;

			result.M41 = xPosition;
			result.M42 = yPosition;
			result.M43 = zPosition;
		}

		public static Matrix CreateTranslation(Vector2 position)
		{
			Matrix returnMatrix = Matrix.Identity;

			returnMatrix.M41 = position.X;
			returnMatrix.M42 = position.Y;

			return returnMatrix;
		}

		public static void CreateTranslation(ref Vector2 position, out Matrix result)
		{
			result = Matrix.Identity;

			result.M41 = position.X;
			result.M42 = position.Y;
		}

		public static Matrix CreateTranslation(Vector3 position)
		{
			Matrix returnMatrix = Matrix.Identity;

			returnMatrix.M41 = position.X;
			returnMatrix.M42 = position.Y;
			returnMatrix.M43 = position.Z;

			return returnMatrix;
		}

		public static void CreateTranslation(ref Vector3 position, out Matrix result)
		{
			result = Matrix.Identity;

			result.M41 = position.X;
			result.M42 = position.Y;
			result.M43 = position.Z;
		}

		public static Matrix Invert(ref Matrix value)
		{
			Matrix result;
			Invert(ref value, out result);
			return result;
		}

		/// <summary>
		/// Calculates the inverse of the specified matrix.
		/// </summary>
		/// <param name="value">The matrix whose inverse is to be calculated.</param>
		/// <param name="result">When the method completes, contains the inverse of the specified matrix.</param>
		public static void Invert(ref Matrix value, out Matrix result)
		{
			float b0 = (value.M31 * value.M42) - (value.M32 * value.M41);
			float b1 = (value.M31 * value.M43) - (value.M33 * value.M41);
			float b2 = (value.M34 * value.M41) - (value.M31 * value.M44);
			float b3 = (value.M32 * value.M43) - (value.M33 * value.M42);
			float b4 = (value.M34 * value.M42) - (value.M32 * value.M44);
			float b5 = (value.M33 * value.M44) - (value.M34 * value.M43);

			float d11 = value.M22 * b5 + value.M23 * b4 + value.M24 * b3;
			float d12 = value.M21 * b5 + value.M23 * b2 + value.M24 * b1;
			float d13 = value.M21 * -b4 + value.M22 * b2 + value.M24 * b0;
			float d14 = value.M21 * b3 + value.M22 * -b1 + value.M23 * b0;

			float det = value.M11 * d11 - value.M12 * d12 + value.M13 * d13 - value.M14 * d14;
			if (Math.Abs(det) <= 1e-6f)
			{
				result = Matrix.Zero;
				return;
			}

			det = 1f / det;

			float a0 = (value.M11 * value.M22) - (value.M12 * value.M21);
			float a1 = (value.M11 * value.M23) - (value.M13 * value.M21);
			float a2 = (value.M14 * value.M21) - (value.M11 * value.M24);
			float a3 = (value.M12 * value.M23) - (value.M13 * value.M22);
			float a4 = (value.M14 * value.M22) - (value.M12 * value.M24);
			float a5 = (value.M13 * value.M24) - (value.M14 * value.M23);

			float d21 = value.M12 * b5 + value.M13 * b4 + value.M14 * b3;
			float d22 = value.M11 * b5 + value.M13 * b2 + value.M14 * b1;
			float d23 = value.M11 * -b4 + value.M12 * b2 + value.M14 * b0;
			float d24 = value.M11 * b3 + value.M12 * -b1 + value.M13 * b0;

			float d31 = value.M42 * a5 + value.M43 * a4 + value.M44 * a3;
			float d32 = value.M41 * a5 + value.M43 * a2 + value.M44 * a1;
			float d33 = value.M41 * -a4 + value.M42 * a2 + value.M44 * a0;
			float d34 = value.M41 * a3 + value.M42 * -a1 + value.M43 * a0;

			float d41 = value.M32 * a5 + value.M33 * a4 + value.M34 * a3;
			float d42 = value.M31 * a5 + value.M33 * a2 + value.M34 * a1;
			float d43 = value.M31 * -a4 + value.M32 * a2 + value.M34 * a0;
			float d44 = value.M31 * a3 + value.M32 * -a1 + value.M33 * a0;

			result.M11 = +d11 * det; result.M12 = -d21 * det; result.M13 = +d31 * det; result.M14 = -d41 * det;
			result.M21 = -d12 * det; result.M22 = +d22 * det; result.M23 = -d32 * det; result.M24 = +d42 * det;
			result.M31 = +d13 * det; result.M32 = -d23 * det; result.M33 = +d33 * det; result.M34 = -d43 * det;
			result.M41 = -d14 * det; result.M42 = +d24 * det; result.M43 = -d34 * det; result.M44 = +d44 * det;
		}

		/// <summary>
		/// Determines the product of two matrices.
		/// </summary>
		/// <param name="left">The first matrix to multiply.</param>
		/// <param name="right">The second matrix to multiply.</param>
		/// <param name="result">The product of the two matrices.</param>
		public static void Multiply(ref Matrix left, ref Matrix right, out Matrix result)
		{
			result = new Matrix();
			result.M11 = (left.M11 * right.M11) + (left.M12 * right.M21) + (left.M13 * right.M31) + (left.M14 * right.M41);
			result.M12 = (left.M11 * right.M12) + (left.M12 * right.M22) + (left.M13 * right.M32) + (left.M14 * right.M42);
			result.M13 = (left.M11 * right.M13) + (left.M12 * right.M23) + (left.M13 * right.M33) + (left.M14 * right.M43);
			result.M14 = (left.M11 * right.M14) + (left.M12 * right.M24) + (left.M13 * right.M34) + (left.M14 * right.M44);
			result.M21 = (left.M21 * right.M11) + (left.M22 * right.M21) + (left.M23 * right.M31) + (left.M24 * right.M41);
			result.M22 = (left.M21 * right.M12) + (left.M22 * right.M22) + (left.M23 * right.M32) + (left.M24 * right.M42);
			result.M23 = (left.M21 * right.M13) + (left.M22 * right.M23) + (left.M23 * right.M33) + (left.M24 * right.M43);
			result.M24 = (left.M21 * right.M14) + (left.M22 * right.M24) + (left.M23 * right.M34) + (left.M24 * right.M44);
			result.M31 = (left.M31 * right.M11) + (left.M32 * right.M21) + (left.M33 * right.M31) + (left.M34 * right.M41);
			result.M32 = (left.M31 * right.M12) + (left.M32 * right.M22) + (left.M33 * right.M32) + (left.M34 * right.M42);
			result.M33 = (left.M31 * right.M13) + (left.M32 * right.M23) + (left.M33 * right.M33) + (left.M34 * right.M43);
			result.M34 = (left.M31 * right.M14) + (left.M32 * right.M24) + (left.M33 * right.M34) + (left.M34 * right.M44);
			result.M41 = (left.M41 * right.M11) + (left.M42 * right.M21) + (left.M43 * right.M31) + (left.M44 * right.M41);
			result.M42 = (left.M41 * right.M12) + (left.M42 * right.M22) + (left.M43 * right.M32) + (left.M44 * right.M42);
			result.M43 = (left.M41 * right.M13) + (left.M42 * right.M23) + (left.M43 * right.M33) + (left.M44 * right.M43);
			result.M44 = (left.M41 * right.M14) + (left.M42 * right.M24) + (left.M43 * right.M34) + (left.M44 * right.M44);
		}

		/// <summary>
		/// Determines the product of two matrices.
		/// </summary>
		/// <param name="left">The first matrix to multiply.</param>
		/// <param name="right">The second matrix to multiply.</param>
		/// <returns>The product of the two matrices.</returns>
		public static Matrix Multiply(Matrix left, Matrix right)
		{
			Matrix result;
			Multiply(ref left, ref right, out result);
			return result;
		}

		public static Matrix operator *(Matrix matrix1, Matrix matrix2)
		{
			Matrix returnMatrix = new Matrix();
			Matrix.Multiply(ref matrix1, ref matrix2, out returnMatrix);
			return returnMatrix;
		}
	}
}
