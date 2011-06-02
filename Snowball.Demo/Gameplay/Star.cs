using System;

namespace Snowball.Demo.Gameplay
{
	/// <summary>
	/// Represents a star flying by in the background during gameplay.
	/// </summary>
	public class Star
	{
		Vector2 position;

		public Vector2 Position
		{
			get { return this.position; }
			set { this.position = value; }
		}

		public float X
		{
			get { return this.position.X; }
			set { this.position.X = value; }
		}

		public float Y
		{
			get { return this.position.Y; }
			set { this.position.Y = value; }
		}

		public int Speed
		{
			get;
			set;
		}

		public int Size
		{
			get;
			set;
		}
	}
}
