using System;
using System.Collections.Generic;

namespace Snowball.Graphics
{
	public class TextBlock
	{
		public struct Character
		{
			public Rectangle Source
			{
				get;
				internal set;
			}

			public Rectangle Destination
			{
				get;
				internal set;
			}
		}

		Character[] characters;
		Size size;
		Vector2 scale;

		public TextureFont Font
		{
			get;
			private set;
		}

		public string Text
		{
			get;
			private set;
		}

		public Size Size
		{
			get { return this.size; }
		}

		public TextAlignment Alignment
		{
			get;
			private set;
		}

		public Vector2 Scale
		{
			get { return this.scale; }
		}

		public int Length
		{
			get { return this.characters.Length; }
		}

		public Character this[int i]
		{
			get { return this.characters[i]; }
		}

		public TextBlock(TextureFont font, string text, Size size, TextAlignment alignment, Vector2 scale)
		{
			if (font == null)
				throw new ArgumentNullException("font");

			if (text == null)
				throw new ArgumentNullException("text");

			this.Font = font;
			this.Text = text;
			this.size = size;
			this.Alignment = alignment;
			this.scale = scale;

			List<Character> charactersList = new List<Character>();

			Vector2 cursor = Vector2.Zero;
			int lineCount = 1;

			for (int i = 0; i < text.Length; i++)
			{
				// Skip characters we can't render.
				if (text[i] == '\r')
					continue;

				float widthOfChar = 0;

				if (text[i] == '\n' || cursor.X + (widthOfChar = (font[text[i]].Width * scale.X)) > this.size.Width)
				{
					cursor.X = 0;
					cursor.Y += (font.LineHeight + font.LineSpacing) * scale.Y;

					// If the next line extends past the destination, quit.
					if (cursor.Y + font.LineHeight > this.size.Height)
						break;

					lineCount++;

					// We can't render a new line.
					if (text[i] == '\n')
						continue;
				}

				Character character = new Character();
				character.Source = font[text[i]];
				character.Destination = new Rectangle((int)cursor.X, (int)cursor.Y, (int)(character.Source.Width * scale.X), (int)(character.Source.Height * scale.Y));
				charactersList.Add(character);

				cursor.X += widthOfChar + font.CharacterSpacing;
			}

			this.characters = charactersList.ToArray();
			int yPositionOfCurrentLine = -1;
						
			int widthOfCurrentLine = 0;
			int heightOfLines = (int)cursor.Y;

			if (alignment != TextAlignment.TopLeft)
			{								
				for (int i = 0; i < this.characters.Length; i++)
				{
					Rectangle destination = this.characters[i].Destination;

					if (yPositionOfCurrentLine != destination.Y) // If we reach a new line, find the width of it.
					{
						yPositionOfCurrentLine = destination.Y;
						
						int j = i;
						while (j < this.characters.Length && this.characters[j].Destination.Y == yPositionOfCurrentLine)
							j++;

						widthOfCurrentLine = this.characters[j - 1].Destination.Right;							
					}

					switch (this.Alignment)
					{
						case TextAlignment.TopCenter:
						case TextAlignment.MiddleCenter:
						case TextAlignment.BottomCenter:
							destination.X = (this.size.Width / 2) - (widthOfCurrentLine / 2) + destination.X;
							break;

						case TextAlignment.TopRight:
						case TextAlignment.MiddleRight:
						case TextAlignment.BottomRight:
							destination.X = this.size.Width - widthOfCurrentLine + destination.X;
							break;
					}

					switch (this.Alignment)
					{
						case TextAlignment.MiddleLeft:
						case TextAlignment.MiddleCenter:
						case TextAlignment.MiddleRight:
							destination.Y = (this.size.Height / 2) - (heightOfLines / 2) + destination.Y;
							break;

						case TextAlignment.BottomLeft:
						case TextAlignment.BottomCenter:
						case TextAlignment.BottomRight:
							destination.Y = this.size.Height - heightOfLines + destination.Y;
							break;
					}

					this.characters[i].Destination = destination;
				}
			}
		}
	}
}
