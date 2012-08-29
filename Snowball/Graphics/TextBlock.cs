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

		public TextBlock(TextureFont font, string text, Size size, TextAlignment alignment)
			: this(font, text, size, alignment, Vector2.One)
		{
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
						
			float heightOfSingleLine = font.LineHeight * scale.Y;
			float heightOfAllLines = 0;

			if (heightOfSingleLine <= size.Height)
			{
				Vector2 cursor = Vector2.Zero;

				for (int i = 0; i < text.Length; i++)
				{
					// Skip characters we can't render.
					if (text[i] == '\r')
						continue;

					float widthOfChar = 0;

					if (text[i] == '\n' || cursor.X + (widthOfChar = font[text[i]].Width * scale.X) > this.size.Width)
					{
						cursor.X = 0;
						cursor.Y += heightOfSingleLine + font.LineSpacing;

						// If the next line extends past the destination, quit.
						if (cursor.Y + heightOfSingleLine > this.size.Height)
							break;

						heightOfAllLines = (int)(cursor.Y + heightOfSingleLine);

						// We can't render a new line.
						if (text[i] == '\n')
							continue;
					}

					Character character = new Character();
					character.Source = font[text[i]];
					character.Destination = new Rectangle((int)cursor.X, (int)cursor.Y, (int)widthOfChar, (int)heightOfSingleLine);
					charactersList.Add(character);

					cursor.X += widthOfChar + font.CharacterSpacing;
				}
			}

			this.characters = charactersList.ToArray();
			int yPositionOfCurrentLine = -1;
						
			float widthOfCurrentLine = 0;

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
							destination.X = (int)((this.size.Width / 2) - (widthOfCurrentLine / 2) + destination.X);
							break;

						case TextAlignment.TopRight:
						case TextAlignment.MiddleRight:
						case TextAlignment.BottomRight:
							destination.X = (int)(this.size.Width - widthOfCurrentLine + destination.X);
							break;
					}

					switch (this.Alignment)
					{
						case TextAlignment.MiddleLeft:
						case TextAlignment.MiddleCenter:
						case TextAlignment.MiddleRight:
							destination.Y = (int)((this.size.Height / 2) - (heightOfAllLines / 2) + destination.Y);
							break;

						case TextAlignment.BottomLeft:
						case TextAlignment.BottomCenter:
						case TextAlignment.BottomRight:
							destination.Y = (int)(this.size.Height - heightOfAllLines + destination.Y);
							break;
					}

					this.characters[i].Destination = destination;
				}
			}
		}
	}
}
