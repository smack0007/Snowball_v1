using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.Graphics
{
	public interface ITextureFont
	{
		Texture Texture { get; }

		int LineHeight { get; }
		
		/// <summary>
		/// The amount of space to use between each character when rendering a string.
		/// </summary>
		int CharacterSpacing { get; }

		/// <summary>
		/// The amount of space to use between each line when rendering a string.
		/// </summary>
		int LineSpacing { get; }

		Rectangle this[char ch] { get; }

		bool ContainsCharacter(char ch);

		/// <summary>
		/// Measures the given string.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		Vector2 MeasureString(string s);

		/// <summary>
		/// Measures the size of the string.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="start">The index of the string at which to start measuring.</param>
		/// <param name="length">How many characters to measure from the start.</param>
		/// <returns></returns>
		Vector2 MeasureString(string s, int start, int length);
	}
}
