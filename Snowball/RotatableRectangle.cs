using System;

namespace Snowball
{
	/// <summary>
	/// Represents a Rectangle which can be rotated.
	/// </summary>
	public sealed class RotatableRectangle
	{
		Vector2 position;
		int width, height;
		Vector2 origin;
		float rotation;

		bool recalculateCorners;
		Vector2 topLeft, topRight, bottomLeft, bottomRight;

		/// <summary>
		/// The underlying Rectangle.
		/// </summary>
		public Rectangle BaseRectangle
		{
			get
			{
				return new Rectangle((int)this.position.X + (int)this.origin.X, (int)this.position.Y + (int)this.origin.Y,
									 this.width, this.height);
			}

			set
			{
				this.position = new Vector2(value.X, value.Y);
				this.width = value.Width;
				this.height = value.Height;
				this.rotation = 0;

				this.recalculateCorners = true;
			}
		}


		/// <summary>
		/// The position of the Rectangle.
		/// </summary>
		public Vector2 Position
		{
			get { return this.position; }

			set
			{
				this.position = value;
				this.recalculateCorners = true;
			}
		}

		/// <summary>
		/// The X position of the Rectangle.
		/// </summary>
		public float X
		{
			get { return this.position.X; }

			set
			{
				this.position.X = value;
				this.recalculateCorners = true;
			}
		}

		/// <summary>
		/// The Y position of the Rectangle.
		/// </summary>
		public float Y
		{
			get { return this.position.Y; }

			set
			{
				this.position.Y = value;
				this.recalculateCorners = true;
			}
		}

		/// <summary>
		/// The width of the Rectangle.
		/// </summary>
		public int Width
		{
			get { return this.width; }

			set
			{
				this.width = value;
				this.recalculateCorners = true;
			}
		}

		/// <summary>
		/// The height of the Rectangle.
		/// </summary>
		public int Height
		{
			get { return this.height; }

			set
			{
				this.height = value;
				this.recalculateCorners = true;
			}
		}

		/// <summary>
		/// The center of rotation for the Rectangle.
		/// </summary>
		public Vector2 Origin
		{
			get { return this.origin; }

			set
			{
				this.origin = value;
				this.recalculateCorners = true;
			}
		}

		/// <summary>
		/// The amount by which the Rectangle is rotated.
		/// </summary>
		public float Rotation
		{
			get { return this.rotation; }

			set
			{
				this.rotation = value;

				if(this.rotation < 0.0f)
					this.rotation += MathHelper.TwoPi;

				if(this.rotation >= MathHelper.TwoPi)
					this.rotation -= MathHelper.TwoPi;

				this.recalculateCorners = true;
			}
		}

		/// <summary>
		/// Gets the top left corner of the Rectangle.
		/// </summary>
		public Vector2 TopLeft
		{
			get
			{
				if(this.recalculateCorners)
					CalculateCorners();

				return this.topLeft;
			}
		}

		/// <summary>
		/// Gets the top right corner of the Rectangle.
		/// </summary>
		public Vector2 TopRight
		{
			get
			{
				if(this.recalculateCorners)
					CalculateCorners();

				return this.topRight;
			}
		}

		/// <summary>
		/// Gets the bottom left corner of the Rectangle.
		/// </summary>
		public Vector2 BottomLeft
		{
			get
			{
				if(this.recalculateCorners)
					CalculateCorners();

				return this.bottomLeft;
			}
		}

		/// <summary>
		/// Gets the bottom right corner of the Rectangle.
		/// </summary>
		public Vector2 BottomRight
		{
			get
			{
				if(this.recalculateCorners)
					CalculateCorners();

				return this.bottomRight;
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public RotatableRectangle()
			: this(Rectangle.Empty, Vector2.Zero, 0.0f)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="baseRectangle"></param>
		public RotatableRectangle(Rectangle baseRectangle)
			: this(baseRectangle, Vector2.Zero, 0.0f)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="baseRectangle"></param>
		/// <param name="origin"></param>
		/// <param name="rotation"></param>
		public RotatableRectangle(Rectangle baseRectangle, Vector2 origin, float rotation)
		{
			this.BaseRectangle = baseRectangle;
			this.origin = origin;
			this.rotation = rotation;

			this.recalculateCorners = true;
		}

		/// <summary>
		/// Calculates positions for the corners of the Rectangle when the position, size, rotation, or origin changes.
		/// </summary>
		private void CalculateCorners()
		{
			this.topLeft = new Vector2(this.position.X - this.origin.X, this.position.Y - this.origin.Y);
			this.topRight = new Vector2(this.position.X - this.origin.X + this.width, this.position.Y - this.origin.Y);
			this.bottomLeft = new Vector2(this.position.X - this.origin.X, this.position.Y - this.origin.Y + this.height);
			this.bottomRight = new Vector2(this.position.X - this.origin.X + this.width, this.position.Y - this.origin.Y + this.height);

			Vector2.RotateAboutOrigin(ref this.topLeft, this.position, this.rotation);
			Vector2.RotateAboutOrigin(ref this.topRight, this.position, this.rotation);
			Vector2.RotateAboutOrigin(ref this.bottomLeft, this.position, this.rotation);
			Vector2.RotateAboutOrigin(ref this.bottomRight, this.position, this.rotation);

			this.recalculateCorners = false;
		}

		/// <summary>
		/// Adds to the Width and Height of the Rectangle.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void Inflate(int width, int height)
		{
			this.width += width;
			this.height += height;
			this.recalculateCorners = true;
		}

		/// <summary>
		/// Moves the rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void Move(float x, float y)
		{
			this.position.X += x;
			this.position.Y += y;
			this.recalculateCorners = true;
		}

		/// <summary>
		/// Returns true if the Rectangle intersects the other Rectangle.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Intersects(Rectangle other)
		{
			if(this.recalculateCorners)
				CalculateCorners();

			Vector2 axis1 = this.topRight - this.topLeft;
			Vector2 axis2 = this.topRight - this.bottomRight;
			Vector2 axis3 = new Vector2(0, other.Top - other.Bottom);
			Vector2 axis4 = new Vector2(other.Left - other.Right, 0);

			if(!IsAxisCollision(other, axis1))
				return false;

			if(!IsAxisCollision(other, axis2))
				return false;

			if(!IsAxisCollision(other, axis3))
				return false;

			if(!IsAxisCollision(other, axis4))
				return false;

			return true;
		}

		/// <summary>
		/// Returns true if the given Rectangle is colliding with the given axis.
		/// </summary>
		/// <param name="other"></param>
		/// <param name="axis"></param>
		/// <returns></returns>
		private bool IsAxisCollision(Rectangle other, Vector2 axis)
		{
			int thisMinScaler, otherMinScaler, thisMaxScaler, otherMaxScaler;

			// this scalers
			int scaler = GenerateScalar(this.topLeft, axis);

			thisMinScaler = scaler;
			thisMaxScaler = scaler;

			scaler = GenerateScalar(this.topRight, axis);

			if(scaler < thisMinScaler)
				thisMinScaler = scaler;

			if(scaler > thisMaxScaler)
				thisMaxScaler = scaler;

			scaler = GenerateScalar(this.bottomLeft, axis);

			if(scaler < thisMinScaler)
				thisMinScaler = scaler;

			if(scaler > thisMaxScaler)
				thisMaxScaler = scaler;

			scaler = GenerateScalar(this.bottomRight, axis);

			if(scaler < thisMinScaler)
				thisMinScaler = scaler;

			if(scaler > thisMaxScaler)
				thisMaxScaler = scaler;

			// other scalers
			scaler = GenerateScalar(new Vector2(other.Left, other.Top), axis);

			otherMinScaler = scaler;
			otherMaxScaler = scaler;

			scaler = GenerateScalar(new Vector2(other.Right, other.Top), axis);

			if(scaler < otherMinScaler)
				otherMinScaler = scaler;

			if(scaler > otherMaxScaler)
				otherMaxScaler = scaler;

			scaler = GenerateScalar(new Vector2(other.Left, other.Bottom), axis);

			if(scaler < otherMinScaler)
				otherMinScaler = scaler;

			if(scaler > otherMaxScaler)
				otherMaxScaler = scaler;

			scaler = GenerateScalar(new Vector2(other.Right, other.Bottom), axis);

			if(scaler < otherMinScaler)
				otherMinScaler = scaler;

			if(scaler > otherMaxScaler)
				otherMaxScaler = scaler;

			if(thisMinScaler <= otherMaxScaler && thisMaxScaler >= otherMaxScaler)
				return true;
			else if(otherMinScaler <= thisMaxScaler && otherMaxScaler >= thisMaxScaler)
				return true;

			return false;
		}

		/// <summary>
		/// Returns true if the Rectangle intersects with the other Rectangle.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Intersects(RotatableRectangle other)
		{
			if(this.recalculateCorners)
				CalculateCorners();

			if(other.recalculateCorners)
				other.CalculateCorners();

			Vector2 axis1 = this.topRight - this.topLeft;
			Vector2 axis2 = this.topRight - this.bottomRight;
			Vector2 axis3 = other.topLeft - other.bottomLeft;
			Vector2 axis4 = other.topLeft - other.topRight;

			if(!IsAxisCollision(other, axis1))
				return false;

			if(!IsAxisCollision(other, axis2))
				return false;

			if(!IsAxisCollision(other, axis3))
				return false;

			if(!IsAxisCollision(other, axis4))
				return false;

			return true;
		}

		/// <summary>
		/// Returns true if the given Rectangle is colliding with the given axis.
		/// </summary>
		/// <param name="other"></param>
		/// <param name="axis"></param>
		/// <returns></returns>
		private bool IsAxisCollision(RotatableRectangle other, Vector2 axis)
		{
			int thisMinScaler, otherMinScaler, thisMaxScaler, otherMaxScaler;

			// this scalers
			int scaler = GenerateScalar(this.topLeft, axis);

			thisMinScaler = scaler;
			thisMaxScaler = scaler;

			scaler = GenerateScalar(this.topRight, axis);

			if(scaler < thisMinScaler)
				thisMinScaler = scaler;

			if(scaler > thisMaxScaler)
				thisMaxScaler = scaler;

			scaler = GenerateScalar(this.bottomLeft, axis);

			if(scaler < thisMinScaler)
				thisMinScaler = scaler;

			if(scaler > thisMaxScaler)
				thisMaxScaler = scaler;

			scaler = GenerateScalar(this.bottomRight, axis);

			if(scaler < thisMinScaler)
				thisMinScaler = scaler;

			if(scaler > thisMaxScaler)
				thisMaxScaler = scaler;

			// other scalers
			scaler = GenerateScalar(other.topLeft, axis);

			otherMinScaler = scaler;
			otherMaxScaler = scaler;

			scaler = GenerateScalar(other.topRight, axis);

			if(scaler < otherMinScaler)
				otherMinScaler = scaler;

			if(scaler > otherMaxScaler)
				otherMaxScaler = scaler;

			scaler = GenerateScalar(other.bottomLeft, axis);

			if(scaler < otherMinScaler)
				otherMinScaler = scaler;

			if(scaler > otherMaxScaler)
				otherMaxScaler = scaler;

			scaler = GenerateScalar(other.bottomRight, axis);

			if(scaler < otherMinScaler)
				otherMinScaler = scaler;

			if(scaler > otherMaxScaler)
				otherMaxScaler = scaler;

			if(thisMinScaler <= otherMaxScaler && thisMaxScaler >= otherMaxScaler)
				return true;
			else if(otherMinScaler <= thisMaxScaler && otherMaxScaler >= thisMaxScaler)
				return true;

			return false;
		}

		/// <summary>
		/// Generates a scaler.
		/// </summary>
		/// <param name="corner"></param>
		/// <param name="axis"></param>
		/// <returns></returns>
		private int GenerateScalar(Vector2 corner, Vector2 axis)
		{
			float numerator = (corner.X * axis.X) + (corner.Y * axis.Y);
			float denominator = (axis.X * axis.X) + (axis.Y * axis.Y);
			float result = numerator / denominator;
			Vector2 cornerProjected = new Vector2(result * axis.X, result * axis.Y);

			float scalar = (axis.X * cornerProjected.X) + (axis.Y * cornerProjected.Y);
			return (int)scalar;
		}

		/// <summary>
		/// Returns true if the Rectangle contains the given point.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public bool Contains(Vector2 point)
		{
			return Intersects(new Rectangle((int)point.X, (int)point.Y, 0, 0));
		}

		/// <summary>
		/// Returns true if the Rectangle contains the given point.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public bool Contains(Point point)
		{
			return Intersects(new Rectangle(point.X, point.Y, 0, 0));
		}
	}
}
