using System;
using System.Collections.Generic;

namespace Snowball.Graphics
{
	public class TextureFont
	{
		Dictionary<char, Rectangle> rectangles;
				
		public Texture Texture
		{
			get;
			private set;
		}

		public int LineHeight
		{
			get;
			private set;
		}

		public Rectangle this[char ch]
		{
			get { return this.rectangles[ch]; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="texture">The texture used by the font.</param>
		/// <param name="rectangles">Dictionary of characters to rectangles.</param>
		public TextureFont(Texture texture, Dictionary<char, Rectangle> rectangles)
		{
			if(texture == null)
				throw new ArgumentNullException("texture");

			if(rectangles == null)
				throw new ArgumentNullException("rectangles");

			this.Texture = texture;
			this.rectangles = rectangles;

			foreach(Rectangle rectangle in this.rectangles.Values)
				if(rectangle.Height > this.LineHeight)
					this.LineHeight = rectangle.Height;
		}

		/// <summary>
		/// Measures the size of the string.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public Vector2 MeasureString(string s)
		{
			Vector2 size = new Vector2();

			size.Y = this.LineHeight;

			int lineWidth = 0;
			for(int i = 0; i < s.Length; i++)
			{
				if(s[i] == '\n')
				{
					if(lineWidth > size.X)
						size.X = lineWidth;

					lineWidth = 0;

					size.Y += this.LineHeight;
				}
				else
				{
					lineWidth += this.rectangles[s[i]].Width;
				}
			}

			if(lineWidth > size.X)
				size.X = lineWidth;

			return size;
		}
	}
}
