﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Snowball.Graphics
{
	public sealed class Sprite
	{
		int frame;
		Vector2 position;
		Vector2 origin;
		float rotation;
		Color color;
		List<Sprite> children;
		ReadOnlyCollection<Sprite> readOnlyChildren;

		public SpriteSheet Sheet
		{
			get;
			private set;
		}

		public int Frame
		{
			get { return this.frame; }
			set { this.frame = value; }
		}

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

		public int Width
		{
			get { return this.Sheet.FrameWidth; }
		}

		public int Height
		{
			get { return this.Sheet.FrameHeight; }
		}

		public Vector2 Origin
		{
			get { return this.origin; }
			set { this.origin = value; }
		}

		public float Rotation
		{
			get { return this.rotation; }
			set { this.rotation = value; }
		}

		public Color Color
		{
			get { return this.color; }
			set { this.color = value; }
		}

		public ReadOnlyCollection<Sprite> Children
		{
			get { return this.readOnlyChildren; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="spriteSheet"></param>
		public Sprite(SpriteSheet spriteSheet)
		{
			if (spriteSheet == null)
				throw new ArgumentNullException("spriteSheet");

			this.Sheet = spriteSheet;

			this.Color = Color.White;
		}

		private Matrix CalculateTransformMatrix()
		{
			return Matrix.CreateTranslation(-this.origin.X, -this.origin.Y) *
				   Matrix.CreateRotationZ(this.rotation) *
				   Matrix.CreateTranslation(this.position.X, this.position.Y);
		}

		public void AddChild(Sprite sprite)
		{
			if (sprite == null)
				throw new ArgumentNullException("sprite");

			if (this.children == null)
			{
				this.children = new List<Sprite>();
				this.readOnlyChildren = new ReadOnlyCollection<Sprite>(this.children);
			}

			this.children.Add(sprite);
		}

		public void Draw(IRenderer renderer)
		{
			Matrix transform = this.CalculateTransformMatrix();
			renderer.DrawSprite(this.Sheet, this.frame, transform, this.Color);

			if (this.children != null && this.children.Count > 0)
			{
				renderer.PushMatrix(transform);
				renderer.PushColor(this.color);

				for(int i = 0; i < this.children.Count; i++)
					this.children[i].Draw(renderer);

				renderer.PopMatrix();
				renderer.PopColor();
			}
		}

		/// <summary>
		/// Returns true if the current Sprite collides with the given Sprite.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool CollidesWith(Sprite other)
		{
			if (CollisionHelper.PerPixelIntersect(this.Sheet, this.position - this.origin, this.frame, other.Sheet, other.position - other.origin, other.frame))
				return true;

			if (other.children != null)
			{
				foreach(Sprite otherChild in other.children)
				{
					if (CollisionHelper.PerPixelIntersect(this.Sheet, this.position - this.origin, this.frame, otherChild.Sheet, otherChild.position - otherChild.origin, otherChild.frame))
						return true;
				}
			}

			if (this.children != null)
			{
				foreach(Sprite child in this.children)
				{
					if (child.CollidesWith(other))
						return true;
				}
			}

			return false;
		}
	}
}
