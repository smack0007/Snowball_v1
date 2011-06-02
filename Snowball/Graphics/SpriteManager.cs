using System;
using System.Collections.Generic;

namespace Snowball.Graphics
{
	/// <summary>
	/// Manages a collection of Sprites.
	/// </summary>
	public class SpriteManager : IRenderable
	{
		List<Sprite> sprites;

		/// <summary>
		/// Constructor.
		/// </summary>
		public SpriteManager()
		{
			this.sprites = new List<Sprite>();
		}

		/// <summary>
		/// Adds a Sprite to be managed.
		/// </summary>
		/// <param name="sprite"></param>
		public void AddSprite(Sprite sprite)
		{
			this.sprites.Add(sprite);
		}

		/// <summary>
		/// Removes a Sprite.
		/// </summary>
		/// <param name="sprite"></param>
		public void RemoveSprite(Sprite sprite)
		{
			this.sprites.Remove(sprite);
		}

		/// <summary>
		/// Draws all Sprites.
		/// </summary>
		/// <param name="renderer"></param>
		public void Draw(IRenderer renderer)
		{
			for(int i = 0; i < this.sprites.Count; i++)
				this.sprites[i].Draw(renderer);
		}
	}
}
